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
    private static readonly MethodInfo _coerceMethodInfoOpen = TypeReflection.GetGenericMethodDefinition(
        typeof(TypeCoercion),
        nameof(Coerce),
        BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException($"Unable to get generic {nameof(Coerce)} method definition on the {nameof(TypeCoercion)} class.");

    private static readonly MethodInfo _convertEnumInputToEnumOutputMethodInfoOpen = TypeReflection.GetGenericMethodDefinition(
        typeof(TypeCoercion),
        nameof(ConvertEnumInputToEnumOutput),
        BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException($"Unable to get generic {nameof(CoerceEnumInputToEnumOutput)} method definition on the {nameof(TypeCoercion)} class.");

    private static readonly MethodInfo _getNameFromEnumMethodInfoOpen = TypeReflection.GetGenericMethodDefinition(
        typeof(TypeCoercion),
        nameof(GetNameFromEnum),
        BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException($"Unable to get generic {nameof(GetNameFromEnum)} method definition on the {nameof(TypeCoercion)} class.");

    private static readonly MethodInfo _parseStringToEnumMethodInfoOpen = TypeReflection.GetGenericMethodDefinition(
        typeof(TypeCoercion),
        nameof(ParseStringToEnum),
        BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException($"Unable to get generic {nameof(ParseStringToEnum)} method definition on the {nameof(TypeCoercion)} class.");

    private static readonly string _nullableValuePropertyName = nameof(Nullable<int>.Value);
    #endregion

    #region Types
    private static class CompiledLambdaCache<TInput, TOutput>
    {
        #region Properties
        public static Func<TInput, TOutput> CastInputToOutputLambda => LazyCastInputToOutputLambda.Value;

        public static Func<TypeCoercion, TInput, TypeCoercionContext, TOutput> CoerceInputToEnumOutputLambda => LazyCoerceInputToEnumOutputLambda.Value;
        public static Func<TypeCoercion, TInput, TypeCoercionContext, TOutput> CoerceEnumInputToOutputLambda => LazyCoerceEnumInputToOutputLambda.Value;
        public static Func<TypeCoercion, TInput, TypeCoercionContext, TOutput> CoerceEnumInputToEnumOutputLambda => LazyCoerceEnumInputToEnumOutputLambda.Value;

        public static Func<TypeCoercion, TInput, TypeCoercionContext, TOutput> CoerceInputToNullableOutputLambda => LazyCoerceInputToNullableOutputLambda.Value;
        public static Func<TypeCoercion, TInput, TypeCoercionContext, TOutput> CoerceNullableInputToOutputLambda => LazyCoerceNullableInputToOutputLambda.Value;
        public static Func<TypeCoercion, TInput, TypeCoercionContext, TOutput> CoerceNullableInputToNullableOutputLambda => LazyCoerceNullableInputToNullableOutputLambda.Value;

        private static Lazy<Func<TInput, TOutput>> LazyCastInputToOutputLambda { get; } = new Lazy<Func<TInput, TOutput>>(CreateCastInputToOutputLambda, LazyThreadSafetyMode.PublicationOnly);

        private static Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>> LazyCoerceInputToEnumOutputLambda { get; } = new Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>>(CreateCoerceInputToEnumOutputLambda, LazyThreadSafetyMode.PublicationOnly);
        private static Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>> LazyCoerceEnumInputToOutputLambda { get; } = new Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>>(CreateCoerceEnumInputToOutputLambda, LazyThreadSafetyMode.PublicationOnly);
        private static Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>> LazyCoerceEnumInputToEnumOutputLambda { get; } = new Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>>(CreateCoerceEnumInputToEnumOutputLambda, LazyThreadSafetyMode.PublicationOnly);

        private static Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>> LazyCoerceInputToNullableOutputLambda { get; } = new Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>>(CreateCoerceInputToNullableOutputLambda, LazyThreadSafetyMode.PublicationOnly);
        private static Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>> LazyCoerceNullableInputToOutputLambda { get; } = new Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>>(CreateCoerceNullableInputToOutputLambda, LazyThreadSafetyMode.PublicationOnly);
        private static Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>> LazyCoerceNullableInputToNullableOutputLambda { get; } = new Lazy<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>>(CreateCoerceNullableInputToNullableOutputLambda, LazyThreadSafetyMode.PublicationOnly);
        #endregion

        #region Cast Factory Methods
        private static Func<TInput, TOutput> CreateCastInputToOutputLambda()
        {
            var inputType = typeof(TInput);
            var outputType = typeof(TOutput);

            var inputParameterExpression = Expression.Parameter(inputType, "input");
            var castInputToOutputExpression = Expression.ConvertChecked(inputParameterExpression, outputType);
            var castInputToOutputLambdaExpression = Expression.Lambda<Func<TInput, TOutput>>(castInputToOutputExpression, inputParameterExpression);
            var castInputToOutputLambda = castInputToOutputLambdaExpression.Compile();
            return castInputToOutputLambda;
        }
        #endregion

        #region Enum Factory Methods
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
                var parseStringToEnumMethodClosed = _parseStringToEnumMethodInfoOpen.MakeGenericMethod(outputType);

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

            // Default case, coerce input type to output enumeration underlying type.
            var outputUnderlyingType = Enum.GetUnderlyingType(outputType) ?? throw new InvalidOperationException($"Unable to get the enum output underlying type {{Name={outputType.Name}}}.");
            var coerceMethodInfoClosed = _coerceMethodInfoOpen.MakeGenericMethod(inputType, outputUnderlyingType);

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

        private static Func<TypeCoercion, TInput, TypeCoercionContext, TOutput> CreateCoerceEnumInputToOutputLambda()
        {
            var coercionType = typeof(TypeCoercion);
            var inputType = typeof(TInput);
            var contextType = typeof(TypeCoercionContext);
            var outputType = typeof(TOutput);

            var coercionParameterExpression = Expression.Parameter(coercionType, "coercion");
            var inputParameterExpression = Expression.Parameter(inputType, "input");
            var contextParameterExpression = Expression.Parameter(contextType, "context");

            // Handle special cases:

            // 1. Enum To String
            if (TypeReflection.IsString(outputType))
            {
                var getNameFromEnumMethodClosed = _getNameFromEnumMethodInfoOpen.MakeGenericMethod(inputType);

                var callGetNameFromEnumMethodExpression = Expression.Call(
                    getNameFromEnumMethodClosed,
                    inputParameterExpression,
                    contextParameterExpression);

                var castCallGetNameFromEnumMethodExpression = Expression.ConvertChecked(callGetNameFromEnumMethodExpression, outputType);

                var getNameFromEnumLambdaExpression = Expression.Lambda<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>>(
                    castCallGetNameFromEnumMethodExpression,
                    coercionParameterExpression,
                    inputParameterExpression,
                    contextParameterExpression);

                var getNameFromEnumLambda = getNameFromEnumLambdaExpression.Compile();
                return getNameFromEnumLambda;
            }

            // Default case, coerce input enumeration underlying type to output type.
            var inputUnderlyingType = Enum.GetUnderlyingType(inputType) ?? throw new InvalidOperationException($"Unable to get the enum input underlying type {{Name={inputType.Name}}}.");
            var coerceMethodInfoClosed = _coerceMethodInfoOpen.MakeGenericMethod(inputUnderlyingType, outputType);

            var castInputToUnderlyingTypeExpression = Expression.ConvertChecked(inputParameterExpression, inputUnderlyingType);

            var callCoerceMethodExpression = Expression.Call(
                coercionParameterExpression,
                coerceMethodInfoClosed,
                castInputToUnderlyingTypeExpression,
                contextParameterExpression);

            var coerceInputToEnumOutputLambdaExpression = Expression.Lambda<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>>(
                callCoerceMethodExpression,
                coercionParameterExpression,
                inputParameterExpression,
                contextParameterExpression);

            var coerceInputToEnumOutputLambda = coerceInputToEnumOutputLambdaExpression.Compile();
            return coerceInputToEnumOutputLambda;
        }

        private static Func<TypeCoercion, TInput, TypeCoercionContext, TOutput> CreateCoerceEnumInputToEnumOutputLambda()
        {
            var coercionType = typeof(TypeCoercion);
            var inputType = typeof(TInput);
            var contextType = typeof(TypeCoercionContext);
            var outputType = typeof(TOutput);

            var coercionParameterExpression = Expression.Parameter(coercionType, "coercion");
            var inputParameterExpression = Expression.Parameter(inputType, "input");
            var contextParameterExpression = Expression.Parameter(contextType, "context");

            // Call ConvertEnumInputToEnumOutput method with the input and context parameters.
            var convertEnumInputToEnumOutputMethodInfoClosed = _convertEnumInputToEnumOutputMethodInfoOpen.MakeGenericMethod(inputType, outputType);
            var callConvertEnumInputToEnumOutputMethodExpression = Expression.Call(
                convertEnumInputToEnumOutputMethodInfoClosed,
                inputParameterExpression,
                contextParameterExpression);

            var convertEnumInputToEnumOutputLambdaExpression = Expression.Lambda<Func<TypeCoercion, TInput, TypeCoercionContext, TOutput>>(
                callConvertEnumInputToEnumOutputMethodExpression,
                coercionParameterExpression,
                inputParameterExpression,
                contextParameterExpression);

            var convertEnumInputToEnumOutputLambda = convertEnumInputToEnumOutputLambdaExpression.Compile();
            return convertEnumInputToEnumOutputLambda;
        }
        #endregion

        #region Nullable Factory Methods
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

            var coerceMethodInfoClosed = _coerceMethodInfoOpen.MakeGenericMethod(inputType, outputUnderlyingType);

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

            var coerceMethodInfoClosed = _coerceMethodInfoOpen.MakeGenericMethod(inputUnderlyingType, outputType);
            var inputValuePropertyInfo = TypeReflection.GetProperty(inputType, _nullableValuePropertyName, BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException($"Unable to get the nullable input type {{Name={inputType.Name}}} property {{Name={_nullableValuePropertyName}}}.");

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

            var coerceMethodInfoClosed = _coerceMethodInfoOpen.MakeGenericMethod(inputUnderlyingType, outputUnderlyingType);
            var inputValuePropertyInfo = TypeReflection.GetProperty(inputType, _nullableValuePropertyName, BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException($"Unable to get the nullable input type {{Name={inputType.Name}}} property {{Name={_nullableValuePropertyName}}}.");

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
        #endregion
    }
    #endregion

    #region Cast Invocation Methods
    private static TOutput CastInputToOutput<TInput, TOutput>(TInput input)
    {
        var output = CompiledLambdaCache<TInput, TOutput>.CastInputToOutputLambda(input);
        return output;
    }
    #endregion

    #region Enum Invocation Generic Methods
    private static TOutput CoerceInputToEnumOutput<TInput, TOutput>(TypeCoercion typeCoercion, TInput input, TypeCoercionContext context)
    {
        var output = CompiledLambdaCache<TInput, TOutput>.CoerceInputToEnumOutputLambda(typeCoercion, input, context);
        return output;
    }

    private static TOutput CoerceEnumInputToOutput<TInput, TOutput>(TypeCoercion typeCoercion, TInput input, TypeCoercionContext context)
    {
        var output = CompiledLambdaCache<TInput, TOutput>.CoerceEnumInputToOutputLambda(typeCoercion, input, context);
        return output;
    }

    private static TOutput CoerceEnumInputToEnumOutput<TInput, TOutput>(TypeCoercion typeCoercion, TInput input, TypeCoercionContext context)
    {
        var output = CompiledLambdaCache<TInput, TOutput>.CoerceEnumInputToEnumOutputLambda(typeCoercion, input, context);
        return output;
    }
    #endregion

    #region Enum Invocation NonGeneric Methods
    private static object CoerceInputToEnumOutput(object input, Type inputType, Type outputType, TypeCoercionContext context)
    {
        // Try by name
        if (TypeReflection.IsString(inputType))
        {
            var inputAsString = (string)input;
            return ParseStringToEnum(inputAsString, outputType, context) ?? throw new InvalidOperationException($"Unable to create enumeration {{Type={outputType.Name}}} with input {{Value={input.SafeToString()}}} as the input is not a defined enumeration value.");
        }

        // Fallback to underlying value transfer (ordinal value)
        var outputUnderlying = Convert.ChangeType(input, Enum.GetUnderlyingType(outputType)) ?? throw new InvalidOperationException($"Unable to convert input {{Value={input.SafeToString()}}} of type {{Type={inputType.Name}}} to underlying type of enumeration {{Type={outputType.Name}}}.");

        var outputByOrdinal = Enum.ToObject(outputType, outputUnderlying);
        return outputByOrdinal;
    }

    private static object CoerceEnumInputToOutput(object input, Type inputType, Type outputType, TypeCoercionContext context)
    {
        // Try by name
        if (TypeReflection.IsString(outputType))
        {
            var inputName = Enum.GetName(inputType, input) ?? throw new InvalidOperationException($"Unable to get the name from enumeration {{Type={inputType.Name}}} with input {{Value={input.SafeToString()}}}.");
            return inputName;
        }

        // Fallback to underlying value transfer (ordinal value)
        var inputUnderlying = Convert.ChangeType(input, Enum.GetUnderlyingType(inputType)) ?? throw new InvalidOperationException($"Unable to convert input {{Value={input.SafeToString()}}} of type {{Type={inputType.Name}}} to underlying type of enumeration {{Type={outputType.Name}}}.");
        var outputByOrdinal = Convert.ChangeType(inputUnderlying, outputType) ?? throw new InvalidOperationException($"Unable to convert input {{Value={inputUnderlying.SafeToString()}}} of type {{Type={Enum.GetUnderlyingType(inputType).Name}}} to underlying type of enumeration {{Type={outputType.Name}}}.");
        return outputByOrdinal;
    }

    private static object CoerceEnumInputToEnumOutput(object input, Type inputType, Type outputType, TypeCoercionContext context)
    {
        // Try by name
        var inputName = Enum.GetName(inputType, input);
        if (inputName != null && Enum.TryParse(outputType, inputName, out var outputByName))
            return outputByName;

        // Fallback to underlying value transfer (ordinal value)
        var inputUnderlying = Convert.ChangeType(input, Enum.GetUnderlyingType(inputType)) ?? throw new InvalidOperationException($"Unable to convert input {{Value={input.SafeToString()}}} of type {{Type={inputType.Name}}} to underlying type of enumeration {{Type={outputType.Name}}}.");
        var outputUnderlying = Convert.ChangeType(inputUnderlying, Enum.GetUnderlyingType(outputType)) ?? throw new InvalidOperationException($"Unable to convert input {{Value={inputUnderlying.SafeToString()}}} of type {{Type={Enum.GetUnderlyingType(inputType).Name}}} to underlying type of enumeration {{Type={outputType.Name}}}.");

        var outputByOrdinal = Enum.ToObject(outputType, outputUnderlying);
        return outputByOrdinal;
    }
    #endregion

    #region Enum Utility Methods
    private static TOutput ConvertEnumInputToEnumOutput<TInput, TOutput>(TInput input, TypeCoercionContext context)
        where TInput : Enum
        where TOutput : struct, Enum
    {
        // Try by name
        var inputType = typeof(TInput);
        var inputName = Enum.GetName(inputType, input);
        if (inputName != null && Enum.TryParse<TOutput>(inputName, out var outputByName))
            return outputByName;

        // Fallback to underlying value transfer (ordinal value)
        var outputType = typeof(TOutput);

        var inputUnderlying = Convert.ChangeType(input, Enum.GetUnderlyingType(inputType)) ?? throw new InvalidOperationException($"Unable to convert input {{Value={input.SafeToString()}}} of type {{Type={inputType.Name}}} to underlying type of enumeration {{Type={typeof(TOutput).Name}}}.");
        var outputUnderlying = Convert.ChangeType(inputUnderlying, Enum.GetUnderlyingType(outputType)) ?? throw new InvalidOperationException($"Unable to convert input {{Value={inputUnderlying.SafeToString()}}} of type {{Type={Enum.GetUnderlyingType(inputType).Name}}} to underlying type of enumeration {{Type={typeof(TOutput).Name}}}.");

        var outputByOrdinal = (TOutput)Enum.ToObject(outputType, outputUnderlying!);
        return outputByOrdinal;
    }

    private static string GetNameFromEnum(object input, Type inputType, TypeCoercionContext context)
    {
        var output = Enum.GetName(inputType, input);
        return output ?? throw new InvalidOperationException($"Unable to get the name from enumeration {{Type={inputType.Name}}} with input {{Value={input.SafeToString()}}}.");
    }

    private static string GetNameFromEnum<TEnum>(TEnum input, TypeCoercionContext context)
        where TEnum : struct, Enum
    {
        var output = Enum.GetName(input);
        return output ?? throw new InvalidOperationException($"Unable to get the name from enumeration {{Type={typeof(TEnum).Name}}} with input {{Value={input.SafeToString()}}}.");
    }

    private static object ParseStringToEnum(string input, Type outputType, TypeCoercionContext context)
    {
        var output = Enum.Parse(outputType, input);
        return output ?? throw new InvalidOperationException($"Unable to create enumeration {{Type={outputType.Name}}} with input {{Value={input}}} as the input is not a defined enumeration value.");
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

    #region Nullable Invocation Generic Methods
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

    private static TOutput CoerceNullableInputToNullableOutput<TInput, TOutput>(TypeCoercion typeCoercion, TInput input, TypeCoercionContext context)
    {
        var output = CompiledLambdaCache<TInput, TOutput>.CoerceNullableInputToNullableOutputLambda(typeCoercion, input, context);
        return output;
    }
    #endregion

    #region Nullable Invocation NonGeneric Methods
    private static object CoerceInputToNullableOutput(TypeCoercion typeCoercion, object input, Type outputType, TypeCoercionContext context)
    {
        var outputUnderlyingType = Nullable.GetUnderlyingType(outputType) ?? throw new InvalidOperationException($"Unable to get the nullable output underlying type {{Name={outputType.Name}}}.");
        return typeCoercion.Coerce(input, outputUnderlyingType, context)!;
    }

    private static object CoerceNullableInputToNullableOutput(TypeCoercion typeCoercion, object input, Type outputType, TypeCoercionContext context)
    {
        var outputUnderlyingType = Nullable.GetUnderlyingType(outputType) ?? throw new InvalidOperationException($"Unable to get the nullable output underlying type {{Name={outputType.Name}}}.");
        return typeCoercion.Coerce(input, outputUnderlyingType, context)!;
    }
    #endregion
}
