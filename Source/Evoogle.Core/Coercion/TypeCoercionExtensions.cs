// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle.Coercion;

/// <summary>
///     Extension methods for the <see cref="ITypeCoercion"/> abstraction.
/// </summary>
public static class TypeCoercionExtensions
{
    #region Extension Methods
    public static bool TryCoerce<TInput, TOutput>(this ITypeCoercion typeCoercion, TInput? input, out TOutput? output, TypeCoercionContext context, TOutput? defaultValue = default)
    {
        try
        {
            output = typeCoercion.Coerce<TInput, TOutput>(input, context);
            return true;
        }
        catch
        {
            output = defaultValue;
            return false;
        }
    }

    public static bool TryCoerce(this ITypeCoercion typeCoercion, object? input, Type outputType, out object? output, TypeCoercionContext context, object? defaultValue = null)
    {
        try
        {
            output = typeCoercion.Coerce(input, outputType, context);
            return true;
        }
        catch
        {
            output = defaultValue;
            return false;
        }
    }
    #endregion
}