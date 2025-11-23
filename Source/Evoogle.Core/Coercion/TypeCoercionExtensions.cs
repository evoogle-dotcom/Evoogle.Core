// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.Coercion;

/// <summary>
///     Extension methods for the <see cref="ITypeCoercion"/> abstraction.
/// </summary>
/// <remarks>
///     Provides utility methods for coercing types safely with default values in case of failure.
/// </remarks>
public static class TypeCoercionExtensions
{
    #region Extension Methods
    /// <summary>
    ///     Attempts to coerce a value of type <paramref name="input"/> to the specified output type <paramref name="outputType"/>.
    ///     If coercion fails, a default value is returned.
    /// </summary>
    /// <param name="typeCoercion">The <see cref="ITypeCoercion"/> object that performs the coercion.</param>
    /// <param name="input">The input value to coerce.</param>
    /// <param name="outputType">The target output type to coerce to.</param>
    /// <param name="output">The output value after coercion, or the default value if coercion fails.</param>
    /// <param name="context">The context that provides additional details for coercion.</param>
    /// <param name="defaultValue">The default value to return if coercion fails.</param>
    /// <returns><c>true</c> if coercion succeeds, <c>false</c> if coercion fails and the default value is returned.</returns>
    /// <remarks>
    ///     This method safely attempts to coerce the input value to the output type.
    ///     If coercion fails, the output is assigned the default value and <c>false</c> is returned.
    /// </remarks>
    public static bool TryCoerce<TInput, TOutput>(this TypeCoercion typeCoercion, TInput? input, out TOutput? output, TypeCoercionContext context, TOutput? defaultValue = default)
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

    /// <summary>
    ///     Attempts to coerce a value of type <typeparamref name="TInput"/> to the specified output type <typeparamref name="TOutput"/>.
    ///     If coercion fails, a default value is returned.
    /// </summary>
    /// <typeparam name="TInput">The input type to coerce from.</typeparam>
    /// <typeparam name="TOutput">The target type to coerce to.</typeparam>
    /// <param name="typeCoercion">The <see cref="ITypeCoercion"/> object that performs the coercion.</param>
    /// <param name="input">The input value to coerce.</param>
    /// <param name="output">The output value after coercion, or the default value if coercion fails.</param>
    /// <param name="context">The context that provides additional details for coercion.</param>
    /// <param name="defaultValue">The default value to return if coercion fails.</param>
    /// <returns><c>true</c> if coercion succeeds, <c>false</c> if coercion fails and the default value is returned.</returns>
    /// <remarks>
    ///     This method safely attempts to coerce the input value to the output type.
    ///     If coercion fails, the output is assigned the default value and <c>false</c> is returned.
    /// </remarks>
    public static bool TryCoerce(this TypeCoercion typeCoercion, object? input, Type outputType, out object? output, TypeCoercionContext context, object? defaultValue = null)
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
