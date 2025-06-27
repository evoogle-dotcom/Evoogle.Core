// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Diagnostics.CodeAnalysis;

using Evoogle.Reflection;

namespace Evoogle.Coercion.Internal;

/// <summary>
///     This API supports the Evoogle.Core infrastructure and is not intended to be used directly from your code.
///     This API may change or be removed in future releases.
/// </summary>
internal partial class TypeCoercion : ITypeCoercion
{
    #region Properties
    private Dictionary<Tuple<Type, Type>, ITypeCoercionDefinition> Definitions { get; } = [];
    #endregion

    #region Constructors
    public TypeCoercion() => this.AddBuiltInDefinitions();
    #endregion

    #region ITypeCoercion Methods
    public object? Coerce(object? input, Type outputType, TypeCoercionContext context)
    {
        // Try corce for null input.
        if (TryCoerceForNullInput(input, outputType))
        {
            return default;
        }

        // This point forward, input is not null.
        var inputType = GetInputType(input);
        try
        {
            // Try coerce if the input and output types are the same or assignable from input to output.
            if (TryCoerceForSameOrAssignableTypes(input, inputType, outputType, out var output))
            {
                return output;
            }

            // Try coerce definition for this exact combination of input and output types.
            if (this.TryCoerceByDefinition(input, inputType, outputType, context, out output))
            {
                return output;
            }

            // Try coerce if any of the input and output types are nullable.
            if (this.TryCoerceForNullableTypes(input, inputType, outputType, context, out output))
            {
                return output;
            }

            // Try coerce if any of the input and output types are enums.
            if (this.TryCoerceForEnumTypes(input, inputType, outputType, context, out output))
            {
                return output;
            }
        }
        catch (Exception innerException)
        {
            throw TypeCoercionException.Create(input, outputType, innerException);
        }

        throw TypeCoercionException.Create(input, outputType);
    }

    public TOutput? Coerce<TInput, TOutput>(TInput? input, TypeCoercionContext context)
    {
        // Try corce for null input.
        if (TryCoerceForNullInput<TInput, TOutput>(input))
        {
            return default;
        }

        // This point forward, input is not null.
        try
        {
            // Try coerce if the input and output types are the same or assignable from input to output.
            if (TryCoerceForSameOrAssignableTypes<TInput, TOutput>(input, out var output))
            {
                return output;
            }

            // Try coerce definition for this exact combination of input and output types.
            if (this.TryCoerceByDefinition(input, context, out output))
            {
                return output;
            }

            // Try coerce if any of the input and output types are nullable.
            if (this.TryCoerceForNullableTypes(input, context, out output))
            {
                return output;
            }

            // Try coerce if any of the input and output types are enums.
            if (this.TryCoerceForEnumTypes(input, context, out output))
            {
                return output;
            }
        }
        catch (Exception innerException)
        {
            throw TypeCoercionException.Create<TInput, TOutput>(input, innerException);
        }

        throw TypeCoercionException.Create<TInput, TOutput>(input);
    }
    #endregion

    #region Implementation Methods
    private void AddBuiltInDefinitions()
    {
        foreach (var definition in BuiltInDefinitions)
        {
            this.AddDefinition(definition);
        }
    }

    private void AddDefinition(ITypeCoercionDefinition definition)
    {
        var key = CreateDefinitionKey(definition);
        this.Definitions.Add(key, definition);
    }

    private static Tuple<Type, Type> CreateDefinitionKey(ITypeCoercionDefinition definition)
    {
        var inputType = definition.InputType;
        var outputType = definition.OutputType;

        var key = CreateDefinitionKey(inputType, outputType);
        return key;
    }

    private static Tuple<Type, Type> CreateDefinitionKey(Type inputType, Type outputType)
    {
        var key = new Tuple<Type, Type>(inputType, outputType);
        return key;
    }

    private static Type GetInputType(object input)
    {
        // Handle special case when input is Type
        if (input is Type)
        {
            return typeof(Type);
        }

        var inputType = input.GetType();
        return inputType;
    }

    private bool TryCoerceByDefinition(object input, Type inputType, Type outputType, TypeCoercionContext context, out object? output)
    {
        if (!this.TryGetDefinition(inputType, outputType, out ITypeCoercionDefinition? definition))
        {
            output = default;
            return false;
        }

        output = definition.Coerce(input, context);
        return true;
    }

    private bool TryCoerceByDefinition<TInput, TOutput>(TInput input, TypeCoercionContext context, out TOutput? output)
    {
        if (!this.TryGetDefinition(out ITypeCoercionDefinition<TInput, TOutput>? definition))
        {
            output = default;
            return false;
        }

        output = definition.Coerce(input, context);
        return true;
    }

    private bool TryCoerceForEnumTypes(object input, Type inputType, Type outputType, TypeCoercionContext context, out object? output)
    {
        var isInputTypeEnum = TypeReflection.IsEnum(inputType);
        var isOutputTypeEnum = TypeReflection.IsEnum(outputType);

        if (isInputTypeEnum && isOutputTypeEnum)
        {
            output = CoerceEnumInputToEnumOutput(input, inputType, outputType, context);
            ValidateEnum(outputType, output);
            return true;
        }
        else if (isInputTypeEnum)
        {
            output = CoerceEnumInputToOutput(input, inputType, outputType, context);
            return true;
        }
        else if (isOutputTypeEnum)
        {
            output = CoerceInputToEnumOutput(input, inputType, outputType, context);
            ValidateEnum(outputType, output);
            return true;
        }

        output = default;
        return false;
    }

    private bool TryCoerceForEnumTypes<TInput, TOutput>(TInput input, TypeCoercionContext context, out TOutput? output)
    {
        var isInputTypeEnum = TypeReflection.IsEnum(typeof(TInput));
        var isOutputTypeEnum = TypeReflection.IsEnum(typeof(TOutput));

        if (isInputTypeEnum && isOutputTypeEnum)
        {
            output = CoerceEnumInputToEnumOutput<TInput, TOutput>(this, input, context);
            ValidateEnum(output);
            return true;
        }
        else if (isInputTypeEnum)
        {
            output = CoerceEnumInputToOutput<TInput, TOutput>(this, input, context);
            return true;
        }
        else if (isOutputTypeEnum)
        {
            output = CoerceInputToEnumOutput<TInput, TOutput>(this, input, context);
            ValidateEnum(output);
            return true;
        }

        output = default;
        return false;
    }

    private bool TryCoerceForNullableTypes(object input, Type inputType, Type outputType, TypeCoercionContext context, out object? output)
    {
        var isInputTypeNullable = TypeReflection.IsNullableType(inputType);
        var isOutputTypeNullable = TypeReflection.IsNullableType(outputType);

        if (isInputTypeNullable && isOutputTypeNullable)
        {
            output = CoerceNullableInputToNullableOutput(this, input, outputType, context);
            return true;
        }
        else if (isInputTypeNullable)
        {
            throw new NotImplementedException();
        }
        else if (isOutputTypeNullable)
        {
            output = CoerceInputToNullableOutput(this, input, outputType, context);
            return true;
        }

        output = default;
        return false;
    }

    private bool TryCoerceForNullableTypes<TInput, TOutput>(TInput input, TypeCoercionContext context, out TOutput? output)
    {
        var isInputTypeNullable = TypeReflection.IsNullableType(typeof(TInput));
        var isOutputTypeNullable = TypeReflection.IsNullableType(typeof(TOutput));

        if (isInputTypeNullable && isOutputTypeNullable)
        {
            output = CoerceNullableInputToNullableOutput<TInput, TOutput>(this, input, context);
            return true;
        }
        else if (isInputTypeNullable)
        {
            output = CoerceNullableInputToOutput<TInput, TOutput>(this, input, context);
            return true;
        }
        else if (isOutputTypeNullable)
        {
            output = CoerceInputToNullableOutput<TInput, TOutput>(this, input, context);
            return true;
        }

        output = default;
        return false;
    }

    private static bool TryCoerceForNullInput([NotNullWhen(false)] object? input, Type outputType)
    {
        // Handle when the input is null.
        if (input == null)
        {
            if (TypeReflection.CanBeNull(outputType))
            {
                return true;
            }

            // Can not coerce null value into a non-nullable value.
            throw TypeCoercionException.Create(input, outputType);
        }

        return false;
    }

    private static bool TryCoerceForNullInput<TInput, TOutput>([NotNullWhen(false)] TInput? input)
    {
        // Handle when the input is null.
        if (input == null)
        {
            if (TypeReflection.CanBeNull<TOutput>())
            {
                return true;
            }

            // Can not coerce null value into a non-nullable value.
            throw TypeCoercionException.Create<TInput, TOutput>(input);
        }

        return false;
    }

    private static bool TryCoerceForSameOrAssignableTypes(object input, Type inputType, Type outputType, out object? output)
    {
        if (inputType == outputType)
        {
            // Will always work because input and output are the same type.
            output = input;
            return true;
        }

        var isOutputTypeAssignableFromInputType = TypeReflection.IsAssignableFrom(outputType, inputType);
        if (isOutputTypeAssignableFromInputType)
        {
            // Will always work because output is assignable from input.
            output = input;
            return true;
        }

        output = default;
        return false;
    }

    private static bool TryCoerceForSameOrAssignableTypes<TInput, TOutput>(TInput input, out TOutput? output)
    {
        var inputType = typeof(TInput);
        var outputType = typeof(TOutput);

        if (inputType == outputType)
        {
            // Cast will always work because input and output are the same type.
            output = CastInputToOutput<TInput, TOutput>(input);
            return true;
        }

        var isOutputTypeAssignableFromInputType = TypeReflection.IsAssignableFrom(outputType, inputType);
        if (isOutputTypeAssignableFromInputType)
        {
            // Cast will always work because output is assignable from input.
            output = CastInputToOutput<TInput, TOutput>(input);
            return true;
        }

        output = default;
        return false;
    }

    private bool TryGetDefinition(Type inputType, Type outputType, [NotNullWhen(true)] out ITypeCoercionDefinition? definition)
    {
        var key = CreateDefinitionKey(inputType, outputType);
        if (!this.Definitions.TryGetValue(key, out definition))
            return false;

        return true;
    }

    private bool TryGetDefinition<TInput, TOutput>([NotNullWhen(true)] out ITypeCoercionDefinition<TInput, TOutput>? definition)
    {
        definition = null;

        var inputType = typeof(TInput);
        var outputType = typeof(TOutput);

        var key = CreateDefinitionKey(inputType, outputType);
        if (!this.Definitions.TryGetValue(key, out var value))
            return false;

        definition = (ITypeCoercionDefinition<TInput, TOutput>)value;
        return true;
    }
    #endregion
}
