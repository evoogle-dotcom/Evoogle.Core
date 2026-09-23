// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

using Evoogle.Coercion;

namespace Evoogle.MemberAccess.Internal;

/// <summary>
///     This API supports the Evoogle.Core infrastructure and is not intended to be used
///     directly from your code.
///     This API may change or be removed in future releases.
/// </summary>
internal enum AccessOperation
{
    #region Values
    Get,
    Set,
    CoercingGet,
    CoercingSet
    #endregion
}

/// <summary>
///     This API supports the Evoogle.Core infrastructure and is not intended to be used
///     directly from your code.
///     This API may change or be removed in future releases.
/// </summary>
internal static class MemberAccessCompiler
{
    #region Types
    private readonly record struct CacheKey
    (
        MemberInfo Member,
        Type DelegateType,
        AccessOperation Operation
    );

    private sealed class CompiledDelegateCacheEntry
    {
        private readonly MemberAccessor _accessor;
        private readonly Type _delegateType;
        private readonly AccessOperation _operation;
        private readonly Lazy<Delegate> _compiledDelegate;

        public CompiledDelegateCacheEntry(CacheKey key, MemberAccessor accessor)
        {
            _accessor = accessor;
            _delegateType = key.DelegateType;
            _operation = key.Operation;
            _compiledDelegate = new Lazy<Delegate>(this.Compile, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public Delegate CompiledDelegate => _compiledDelegate.Value;

        private Delegate Compile() => MemberAccessCompiler.Compile(_accessor, _delegateType, _operation);
    }
    #endregion

    #region Fields
    private static readonly ConcurrentDictionary<CacheKey, CompiledDelegateCacheEntry> _cache = new();

    private static readonly MethodInfo _coerceMethod = typeof(TypeCoercion)
        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
        .Single(static method =>
            method.Name == nameof(TypeCoercion.Coerce) &&
            method.IsGenericMethodDefinition &&
            method.GetParameters().Length == 2);

    private static readonly MethodInfo _nonGenericCoerceMethod = typeof(TypeCoercion)
        .GetMethod(nameof(TypeCoercion.Coerce), [typeof(object), typeof(Type), typeof(TypeCoercionContext)])!;

    private static readonly FieldInfo _defaultCoercionContextField = typeof(TypeCoercionContext)
        .GetField(nameof(TypeCoercionContext.Default), BindingFlags.Public | BindingFlags.Static)!;
    #endregion

    #region Methods
    internal static TDelegate Get<TDelegate>(MemberAccessor accessor, AccessOperation operation)
        where TDelegate : Delegate
    {
        var key = new CacheKey(accessor.MemberInfo, typeof(TDelegate), operation);
        var entry = _cache.GetOrAdd
        (
            key,
            static (cacheKey, state) => new CompiledDelegateCacheEntry(cacheKey, state),
            accessor
        );

        try
        {
            return (TDelegate)entry.CompiledDelegate;
        }
        catch (MemberAccessException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new MemberAccessException($"Could not compile access to member '{accessor.MemberInfo.Name}'.", exception);
        }
    }
    #endregion

    #region Implementation Methods
    private static Delegate Compile
    (
        MemberAccessor accessor,
        Type delegateType,
        AccessOperation operation
    )
    {
        var member = accessor.MemberInfo;
        var isGetter = operation is AccessOperation.Get or AccessOperation.CoercingGet;
        if (isGetter && !accessor.CanRead || !isGetter && !accessor.CanWrite)
        {
            throw new MemberAccessException($"Member '{member.Name}' does not support the requested operation.");
        }

        var invoke = delegateType.GetMethod("Invoke")!;
        var parameterTypes = invoke.GetParameters()
            .Select(static parameter => parameter.ParameterType)
            .ToArray();
        var isCoercing = operation is AccessOperation.CoercingGet or AccessOperation.CoercingSet;
        var parameters = parameterTypes
            .Select(static (type, index) => Expression.Parameter(type, $"argument{index}"))
            .ToArray();
        var isByRef = parameters.Length > 0 && parameters[0].Type.IsByRef;
        var isRuntimeTypedGetter =
            isGetter &&
            isCoercing &&
            invoke.ReturnType == typeof(object) &&
            parameterTypes[accessor.IsStatic ? 0 : 1] == typeof(Type);
        var valueType = isGetter ? invoke.ReturnType : parameterTypes[accessor.IsStatic ? 0 : 1];
        var memberType = accessor.MemberType;

        Expression? target = null;
        var serviceIndex = accessor.IsStatic ? 0 : 1;
        if (!accessor.IsStatic)
        {
            var targetType = isByRef ? parameterTypes[0].GetElementType()! : parameterTypes[0];
            var declaringType = accessor.DeclaringType;
            if
            (
                isByRef && targetType != declaringType ||
                !isByRef &&
                !declaringType.IsAssignableFrom(targetType) &&
                targetType != typeof(object)
            )
            {
                throw new MemberAccessException($"Target type '{targetType}' cannot access member '{member.Name}'.");
            }

            if (!isGetter && (targetType.IsValueType || declaringType.IsValueType) && !isByRef)
            {
                throw new MemberAccessException($"Setter for value-type member '{member.Name}' requires a by-reference target.");
            }

            target = targetType == typeof(object)
                ? Expression.Convert(parameters[0], declaringType)
                : targetType == declaringType || isByRef
                    ? parameters[0]
                    : Expression.Convert(parameters[0], declaringType);
        }

        Expression memberAccess = member switch
        {
            PropertyInfo property => Expression.Property(target, property),
            FieldInfo field => Expression.Field(target, field),
            _ => throw new MemberAccessException($"Unsupported member '{member.Name}'.")
        };

        Expression body;
        if (isGetter)
        {
            if (isRuntimeTypedGetter)
            {
                var requestedType = parameters[serviceIndex];
                var coercion = parameters[serviceIndex + 1];
                var context = Expression.Coalesce
                (
                    parameters[serviceIndex + 2],
                    Expression.Field(null, _defaultCoercionContextField)
                );
                var boxedValue = memberType == typeof(object)
                    ? memberAccess
                    : Expression.Convert(memberAccess, typeof(object));

                body = Expression.Call
                (
                    coercion,
                    _nonGenericCoerceMethod,
                    boxedValue,
                    requestedType,
                    context
                );
            }
            else if (isCoercing)
            {
                body = Coerce
                (
                    memberAccess,
                    memberType,
                    valueType,
                    parameters[serviceIndex],
                    parameters[serviceIndex + 1]
                );
            }
            else
            {
                if (!valueType.IsAssignableFrom(memberType))
                {
                    throw new MemberAccessException($"Getter for '{member.Name}' cannot return '{valueType}' without coercion.");
                }

                body = memberType == valueType
                    ? memberAccess
                    : Expression.Convert(memberAccess, valueType);
            }
        }
        else
        {
            var value = parameters[serviceIndex];
            if (isCoercing)
            {
                body = Expression.Assign
                (
                    memberAccess,
                    Coerce
                    (
                        value,
                        valueType,
                        memberType,
                        parameters[serviceIndex + 1],
                        parameters[serviceIndex + 2]
                    )
                );
            }
            else
            {
                if (!memberType.IsAssignableFrom(valueType) && valueType != typeof(object))
                {
                    throw new MemberAccessException($"Setter for '{member.Name}' cannot accept '{valueType}' without coercion.");
                }

                body = Expression.Assign(memberAccess, memberType == valueType
                    ? parameters[serviceIndex]
                    : Expression.Convert(parameters[serviceIndex], memberType));
            }
        }

        var lambdaExpression = Expression.Lambda(delegateType, body, parameters);
        var lambdaFunction = lambdaExpression.Compile();

        return lambdaFunction;
    }

    private static MethodCallExpression Coerce
    (
        Expression value,
        Type inputType,
        Type outputType,
        ParameterExpression coercion,
        ParameterExpression context
    )
    {
        var resolvedContext = Expression.Coalesce
        (
            context,
            Expression.Field(null, _defaultCoercionContextField)
        );
        var method = _coerceMethod.MakeGenericMethod(inputType, outputType);

        return Expression.Call
        (
            coercion,
            method,
            value,
            resolvedContext
        );
    }
    #endregion
}
