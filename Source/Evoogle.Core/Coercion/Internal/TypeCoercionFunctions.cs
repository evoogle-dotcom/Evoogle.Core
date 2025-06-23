// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Linq.Expressions;
using System.Reflection;

using Evoogle.Extensions;
using Evoogle.Reflection;

namespace Evoogle.Coercion.Internal;

/// <summary>
///     This API supports the Evoogle.Core infrastructure and is not intended to be used directly from your code.
///     This API may change or be removed in future releases.
/// </summary>
internal partial class TypeCoercion : ITypeCoercion
{
    #region Fields
    private static readonly MethodInfo CoerceMethodInfoOpen = TypeReflection.GetGenericMethodDefinition(
        typeof(TypeCoercion),
        nameof(Coerce),
        BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException($"Unable to get generic {nameof(Coerce)} method definition on the {nameof(TypeCoercion)} class.");

    private static readonly MethodInfo ParseStringToEnumMethodInfoOpen = TypeReflection.GetGenericMethodDefinition(
        typeof(TypeCoercion),
        nameof(ParseStringToEnum),
        BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException($"Unable to get generic {nameof(ParseStringToEnum)} method definition on the {nameof(TypeCoercion)} class.");

    private static readonly string NullableValuePropertyName = nameof(Nullable<int>.Value);
    #endregion

    #region Types
    private static class CompiledLambdaCache<TInput, TOutput>
    {
        #region Properties
        public static Func<TInput, TOutput> CastInputToOutputLambda => LazyCastInputToOutputLambda.Value;

        public static Func<TypeCoercion, TInput, TypeCoercionContext, TOutput> CoerceInputToNullableOutputLambda => LazyCoerceInputToNullableOutputLambda.Value;
        public static Func<TypeCoercion, TInput, TypeCoercionContext, TOutput> CoerceNullableInputToOutputLambda => LazyCoerceNullableInputToOutputLambda.Value;
        public static Func<TypeCoercion, TInput, TypeCoercionContext, TOutput> CoerceNullableInputToNullableOutputLambda => LazyCoerceNullableInputToNullableOutputLambda.Value;

        public static Func<TypeCoercion, TInput, TypeCoercionContext, TOutput> CoerceInputToEnumOutputLambda => LazyCoerceInputToEnumOutputLambda.Value;

        private static Lazy<Func<TInput, TOutput>> LazyCastInputToOutputLambda { get; } = new Lazy<Func<TInput, TOutput>>(CreateCastInputToOutputLambda, LazyThreadSafetyMode.PublicationOnly);

        private static Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>> LazyCoerceInputToNullableOutputLambda { get; } = new Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>>(CreateCoerceInputToNullableOutputLambda, LazyThreadSafetyMode.PublicationOnly);
        private static Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>> LazyCoerceNullableInputToOutputLambda { get; } = new Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>>(CreateCoerceNullableInputToOutputLambda, LazyThreadSafetyMode.PublicationOnly);
        private static Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>> LazyCoerceNullableInputToNullableOutputLambda { get; } = new Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>>(CreateCoerceNullableInputToNullableOutputLambda, LazyThreadSafetyMode.PublicationOnly);

        private static Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>> LazyCoerceInputToEnumOutputLambda { get; } = new Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>>(CreateCoerceInputToEnumOutputLambda, LazyThreadSafetyMode.PublicationOnly);
        #endregion

        #region Methods
        private static Func<TypeCoercion, TInput, TypeCoercionContext, TOutput> CreateCoerceInputToEnumOutputLambda()
        {
            var coercionType = typeof(TypeCoercion);
            var inputType = typeof(TInput);
            var contextType = typeof(TypeCoercionContext);
            var outputType = typeof(TOutput);

            var coercionParameterExpression = Expression.Parameter(coercionType, "coercion");
            var inputParameterExpression = Expression.Parameter(inputType, "input");
            var contextParameterExpression = Expression.Parameter(contextType, "context");

            // Handle special cases:

            // 1. String To Enum
            if (TypeReflection.IsString(inputType))
            {
                var parseStringToEnumMethodClosed = ParseStringToEnumMethodInfoOpen.MakeGenericMethod(outputType);

                var callParseStringToEnumMethodExpression = Expression.Call(
                    parseStringToEnumMethodClosed,
                    inputParameterExpression,
                    contextParameterExpression);

                var castCallParseStringToEnumMethodExpression = Expression.ConvertChecked(callParseStringToEnumMethodExpression, outputType);

                var parseStringToEnumLambdaExpression = Expression.Lambda<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>>(
                    castCallParseStringToEnumMethodExpression,
                    coercionParameterExpression,
                    inputParameterExpression,
                    contextParameterExpression);

                var parseStringToEnumLambda = parseStringToEnumLambdaExpression.Compile();
                return parseStringToEnumLambda;
            }

            // Default case, coerce input to enumeration underlying type and cast that coerce result to the enumeration.
            var outputUnderlyingType = Enum.GetUnderlyingType(outputType) ?? throw new InvalidOperationException($"Unable to get the enum output underlying type {{Name={outputType.Name}}}.");
            var coerceMethodInfoClosed = CoerceMethodInfoOpen.MakeGenericMethod(inputType, outputUnderlyingType);

            var callCoerceMethodExpression = Expression.Call(
                coercionParameterExpression,
                coerceMethodInfoClosed,
                inputParameterExpression,
                contextParameterExpression);

            var castCallCoerceMethodExpression = Expression.ConvertChecked(callCoerceMethodExpression, outputType);

            var coerceInputToEnumOutputLambdaExpression = Expression.Lambda<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>>(
                castCallCoerceMethodExpression,
                coercionParameterExpression,
                inputParameterExpression,
                contextParameterExpression);

            var coerceInputToEnumOutputLambda = coerceInputToEnumOutputLambdaExpression.Compile();
            return coerceInputToEnumOutputLambda;
        }

        private static Func<TypeCoercion, TInput, TypeCoercionContext, TOutput> CreateCoerceInputToNullableOutputLambda()
        {
            var coercionType = typeof(TypeCoercion);
            var inputType = typeof(TInput);
            var contextType = typeof(TypeCoercionContext);
            var outputType = typeof(TOutput);
            var outputUnderlyingType = Nullable.GetUnderlyingType(outputType) ?? throw new InvalidOperationException($"Unable to get the nullable output underlying type {{Name={outputType.Name}}}.");

            var coercionParameterExpression = Expression.Parameter(coercionType, "coercion");
            var inputParameterExpression = Expression.Parameter(inputType, "input");
            var contextParameterExpression = Expression.Parameter(contextType, "context");

            var coerceMethodInfoClosed = CoerceMethodInfoOpen.MakeGenericMethod(inputType, outputUnderlyingType);

            var callCoerceMethodExpression = Expression.Call(
                coercionParameterExpression,
                coerceMethodInfoClosed,
                inputParameterExpression,
                contextParameterExpression);

            var castCallCoerceMethodExpression = Expression.ConvertChecked(callCoerceMethodExpression, outputType);

            var coerceInputToNullableOutputLambdaExpression = Expression.Lambda<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>>(
                castCallCoerceMethodExpression,
                coercionParameterExpression,
                inputParameterExpression,
                contextParameterExpression);

            var coerceInputToNullableOutputLambda = coerceInputToNullableOutputLambdaExpression.Compile();
            return coerceInputToNullableOutputLambda;
        }

        private static Func<TypeCoercion, TInput, TypeCoercionContext, TOutput> CreateCoerceNullableInputToOutputLambda()
        {
            var coercionType = typeof(TypeCoercion);
            var inputType = typeof(TInput);
            var inputUnderlyingType = Nullable.GetUnderlyingType(inputType) ?? throw new InvalidOperationException($"Unable to get the nullable input underlying type {{Name={inputType.Name}}}.");
            var contextType = typeof(TypeCoercionContext);
            var outputType = typeof(TOutput);

            var coercionParameterExpression = Expression.Parameter(coercionType, "coercion");
            var inputParameterExpression = Expression.Parameter(inputType, "input");
            var contextParameterExpression = Expression.Parameter(contextType, "context");

            var coerceMethodInfoClosed = CoerceMethodInfoOpen.MakeGenericMethod(inputUnderlyingType, outputType);
            var inputValuePropertyInfo = TypeReflection.GetProperty(inputType, NullableValuePropertyName, BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException($"Unable to get the nullable input type {{Name={inputType.Name}}} property {{Name={NullableValuePropertyName}}}.");

            var inputParameterValuePropertyExpression = Expression.Property(inputParameterExpression, inputValuePropertyInfo);

            var callCoerceMethodExpression = Expression.Call(
                coercionParameterExpression,
                coerceMethodInfoClosed,
                inputParameterValuePropertyExpression,
                contextParameterExpression);

            var castCallCoerceMethodExpression = Expression.ConvertChecked(callCoerceMethodExpression, outputType);

            var coerceNullableInputToOutputLambdaExpression = Expression.Lambda<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>>(
                castCallCoerceMethodExpression,
                coercionParameterExpression,
                inputParameterExpression,
                contextParameterExpression);

            var coerceNullableInputToOutputLambda = coerceNullableInputToOutputLambdaExpression.Compile();
            return coerceNullableInputToOutputLambda;
        }

        private static Func<TypeCoercion, TInput, TypeCoercionContext, TOutput> CreateCoerceNullableInputToNullableOutputLambda()
        {
            var coercionType = typeof(TypeCoercion);
            var inputType = typeof(TInput);
            var inputUnderlyingType = Nullable.GetUnderlyingType(inputType) ?? throw new InvalidOperationException($"Unable to get the nullable input underlying type {{Name={inputType.Name}}}.");
            var contextType = typeof(TypeCoercionContext);
            var outputType = typeof(TOutput);
            var outputUnderlyingType = Nullable.GetUnderlyingType(outputType) ?? throw new InvalidOperationException($"Unable to get the nullable output underlying type {{Name={outputType.Name}}}.");

            var coercionParameterExpression = Expression.Parameter(coercionType, "coercion");
            var inputParameterExpression = Expression.Parameter(inputType, "input");
            var contextParameterExpression = Expression.Parameter(contextType, "context");

            var coerceMethodInfoClosed = CoerceMethodInfoOpen.MakeGenericMethod(inputUnderlyingType, outputUnderlyingType);
            var inputValuePropertyInfo = TypeReflection.GetProperty(inputType, NullableValuePropertyName, BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException($"Unable to get the nullable input type {{Name={inputType.Name}}} property {{Name={NullableValuePropertyName}}}.");

            var inputParameterValuePropertyExpression = Expression.Property(inputParameterExpression, inputValuePropertyInfo);

            var callCoerceMethodExpression = Expression.Call(
                coercionParameterExpression,
                coerceMethodInfoClosed,
                inputParameterValuePropertyExpression,
                contextParameterExpression);

            var castCallCoerceMethodExpression = Expression.ConvertChecked(callCoerceMethodExpression, outputType);

            var coerceNullableInputToNullableOutputLambdaExpression = Expression.Lambda<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>>(
                castCallCoerceMethodExpression,
                coercionParameterExpression,
                inputParameterExpression,
                contextParameterExpression);

            var coerceNullableInputToNullableOutputLambda = coerceNullableInputToNullableOutputLambdaExpression.Compile();
            return coerceNullableInputToNullableOutputLambda;
        }

        private static Func<TInput, TOutput> CreateCastInputToOutputLambda()
        {
            var inputType = typeof(TInput);
            var outputType = typeof(TOutput);

            var inputParameterExpression = Expression.Parameter(inputType, "input");
            var castInputToOutputExpression = Expression.ConvertChecked(inputParameterExpression, outputType);
            var castInputToOutputLambdaExpression = Expression
               .Lambda<Func<TInput, TOutput>>(castInputToOutputExpression, inputParameterExpression);
            var castInputToOutputLambda = castInputToOutputLambdaExpression.Compile();
            return castInputToOutputLambda;
        }
        #endregion
    }
    #endregion

    #region Methods
    private static object CoerceInputToNullableOutput(TypeCoercion typeCoercion, object input, Type outputType, TypeCoercionContext context)
    {
        var outputUnderlyingType = Nullable.GetUnderlyingType(outputType) ?? throw new InvalidOperationException($"Unable to get the nullable output underlying type {{Name={outputType.Name}}}.");
        return typeCoercion.Coerce(input, outputUnderlyingType, context)!;
    }

    private static TOutput CoerceInputToNullableOutput<TInput, TOutput>(TypeCoercion typeCoercion, TInput input, TypeCoercionContext context)
    {
        var output = CompiledLambdaCache<TInput, TOutput>.CoerceInputToNullableOutputLambda(typeCoercion, input, context);
        return output;
    }

    private static TOutput CoerceNullableInputToOutput<TInput, TOutput>(TypeCoercion typeCoercion, TInput input, TypeCoercionContext context)
    {
        var output = CompiledLambdaCache<TInput, TOutput>.CoerceNullableInputToOutputLambda(typeCoercion, input, context);
        return output;
    }

    private static object CoerceNullableInputToNullableOutput(TypeCoercion typeCoercion, object input, Type outputType, TypeCoercionContext context)
    {
        var outputUnderlyingType = Nullable.GetUnderlyingType(outputType) ?? throw new InvalidOperationException($"Unable to get the nullable output underlying type {{Name={outputType.Name}}}.");
        return typeCoercion.Coerce(input, outputUnderlyingType, context)!;
    }

    private static TOutput CoerceNullableInputToNullableOutput<TInput, TOutput>(TypeCoercion typeCoercion, TInput input, TypeCoercionContext context)
    {
        var output = CompiledLambdaCache<TInput, TOutput>.CoerceNullableInputToNullableOutputLambda(typeCoercion, input, context);
        return output;
    }

    private static object CoerceInputToEnumOutput(TypeCoercion typeCoercion, object input, Type inputType, Type outputType, TypeCoercionContext context)
    {
        // Handle special cases:

        // 1. String To Enum
        if (TypeReflection.IsString(inputType))
        {
            var inputAsString = (string)input;
            return ParseStringToEnum(inputAsString, outputType, context)!;
        }

        // Default case, coerce input to enumeration underlying type and cast that coerce result to the enumeration.
        var outputUnderlyingType = Enum.GetUnderlyingType(outputType) ?? throw new InvalidOperationException($"Unable to get the enum output underlying type {{Name={outputType.Name}}}.");
        var outputUnderlyingValue = typeCoercion.Coerce(input, outputUnderlyingType, context)!;

        var output = Enum.ToObject(outputType, outputUnderlyingValue);
        return output;
    }

    private static TOutput CoerceInputToEnumOutput<TInput, TOutput>(TypeCoercion typeCoercion, TInput input, TypeCoercionContext context)
    {
        var output = CompiledLambdaCache<TInput, TOutput>.CoerceInputToEnumOutputLambda(typeCoercion, input, context);
        return output;
    }

    private static TOutput CastInputToOutput<TInput, TOutput>(TInput input)
    {
        var output = CompiledLambdaCache<TInput, TOutput>.CastInputToOutputLambda(input);
        return output;
    }

    private static object ParseStringToEnum(string input, Type outputType, TypeCoercionContext context)
    {
        var output = Enum.Parse(outputType, input);
        return output;
    }

    private static TEnum ParseStringToEnum<TEnum>(string input, TypeCoercionContext context)
        where TEnum : struct
    {
        var output = Enum.Parse<TEnum>(input);
        return output;
    }

    private static void ValidateEnum(Type outputType, object output)
    {
        var isDefined = Enum.IsDefined(outputType, output);
        if (isDefined)
            return;

        var message = $"Unable to create enumeration {{Type={outputType.Name}}} with input {{Value={output.SafeToString()}}} as the input is not a defined enumeration value.";
        throw new InvalidOperationException(message);
    }

    private static void ValidateEnum<TEnum>(TEnum output)
    {
        var outputType = typeof(TEnum);
        ValidateEnum(outputType, output!);
    }
    #endregion
}
