// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

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
    private sealed class CompiledDelegateCacheEntry
    {
        private readonly MemberAccessor _accessor;
        private readonly Type _delegateType;
        private readonly AccessOperation _operation;
        private readonly Lazy<Delegate> _compiledDelegate;

        public CompiledDelegateCacheEntry
        (
            MemberAccessor accessor,
            Type delegateType,
            AccessOperation operation
        )
        {
            _accessor = accessor;
            _delegateType = delegateType;
            _operation = operation;
            _compiledDelegate = new Lazy<Delegate>(this.Compile, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public Delegate CompiledDelegate => _compiledDelegate.Value;

        private Delegate Compile() => MemberAccessCompiler.Compile(_accessor, _delegateType, _operation);
    }

    private sealed class DelegateTypeCache
    {
        private CompiledDelegateCacheEntry? _get;
        private CompiledDelegateCacheEntry? _set;
        private CompiledDelegateCacheEntry? _coercingGet;
        private CompiledDelegateCacheEntry? _coercingSet;

        public CompiledDelegateCacheEntry Get
        (
            MemberAccessor accessor,
            Type delegateType,
            AccessOperation operation
        )
        {
            ref var location = ref this.GetLocation(operation);
            var entry = Volatile.Read(ref location);
            if (entry is not null)
            {
                return entry;
            }

            var created = new CompiledDelegateCacheEntry(accessor, delegateType, operation);
            return Interlocked.CompareExchange(ref location, created, null) ?? created;
        }

        private ref CompiledDelegateCacheEntry? GetLocation(AccessOperation operation)
        {
            switch (operation)
            {
                case AccessOperation.Get:
                    return ref _get;
                case AccessOperation.Set:
                    return ref _set;
                case AccessOperation.CoercingGet:
                    return ref _coercingGet;
                case AccessOperation.CoercingSet:
                    return ref _coercingSet;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation));
            }
        }
    }

    private sealed class MemberDelegateCache
    {
        private readonly ConditionalWeakTable<Type, DelegateTypeCache> _delegateTypes = new();

        public CompiledDelegateCacheEntry Get
        (
            MemberAccessor accessor,
            Type delegateType,
            AccessOperation operation
        )
        {
            var cache = _delegateTypes.GetValue(delegateType, static _ => new DelegateTypeCache());
            return cache.Get(accessor, delegateType, operation);
        }
    }

    private sealed class DeclaringTypeDelegateCache
    {
        private readonly ConcurrentDictionary<MemberInfo, MemberDelegateCache> _members = new();

        public MemberDelegateCache Get(MemberInfo memberInfo)
            => _members.GetOrAdd(memberInfo, static _ => new MemberDelegateCache());
    }
    #endregion

    #region Fields
    private static readonly ConditionalWeakTable<Type, DeclaringTypeDelegateCache> _cache = [];

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
        var isGetter = operation is AccessOperation.Get or AccessOperation.CoercingGet;
        if (isGetter && !accessor.CanRead || !isGetter && !accessor.CanWrite)
        {
            throw new MemberAccessException($"Member '{accessor.MemberInfo.Name}' does not support the requested operation.");
        }

        var declaringTypeCache = _cache.GetValue(accessor.DeclaringType, static _ => new DeclaringTypeDelegateCache());
        var memberCache = declaringTypeCache.Get(accessor.MemberInfo);
        var entry = memberCache.Get(accessor, typeof(TDelegate), operation);

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
        var isByRef = parameters.Length > 0 && parameters[0].IsByRef;
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
                throw new MemberAccessException(
                    $"Setter for value-type member '{member.Name}' requires a by-reference target.");
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
                    throw new MemberAccessException(
                        $"Getter for '{member.Name}' cannot return '{valueType}' without coercion.");
                }

                body = memberType == valueType ? memberAccess : Expression.Convert(memberAccess, valueType);
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
                    valueType == typeof(object)
                        ? CoerceObject
                        (
                            value,
                            memberType,
                            parameters[serviceIndex + 1],
                            parameters[serviceIndex + 2]
                        )
                        : Coerce
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
                    throw new MemberAccessException(
                        $"Setter for '{member.Name}' cannot accept '{valueType}' without coercion.");
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

    private static UnaryExpression CoerceObject
    (
        Expression value,
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
        var coercedValue = Expression.Call
        (
            coercion,
            _nonGenericCoerceMethod,
            value,
            Expression.Constant(outputType),
            resolvedContext
        );

        return Expression.Convert(coercedValue, outputType);
    }
    #endregion
}
