// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.Coercion;

/// <summary>
///     Abstracts methods for type coercion, allowing conversion between different data types.
/// </summary>
/// <remarks>
///     If the input value is <c>null</c>, the method returns <c>null</c>.  
///     If coercion fails for a non-null value, a <see cref="TypeCoercionException"/> is thrown.
/// </remarks>
public interface ITypeCoercion
{
    #region Methods
    /// <summary>
    ///     Attempts to coerce the given <paramref name="input"/> into the specified <paramref name="outputType"/>.
    /// </summary>
    /// <param name="input">The value to be converted. If <c>null</c>, the method returns <c>null</c>.</param>
    /// <param name="outputType">The target type to which the value should be converted.</param>
    /// <param name="context">The context that provides additional details for the coercion process.</param>
    /// <returns>
    ///     The coerced value of the specified type if conversion is successful; otherwise, <c>null</c> if the input is <c>null</c>.
    /// </returns>
    /// <exception cref="TypeCoercionException">
    ///     Thrown if coercion is not possible for a non-null input value.
    /// </exception>
    object? Coerce(object? input, Type outputType, TypeCoercionContext context);

    /// <summary>
    ///     Attempts to coerce a value of type <typeparamref name="TInput"/> into <typeparamref name="TOutput"/>.
    /// </summary>
    /// <typeparam name="TInput">The input type.</typeparam>
    /// <typeparam name="TOutput">The target type to which the value should be converted.</typeparam>
    /// <param name="input">The value to be converted. If <c>null</c>, the method returns <c>null</c>.</param>
    /// <param name="context">The context that provides additional details for the coercion process.</param>
    /// <returns>
    ///     The coerced value of type <typeparamref name="TOutput"/> if conversion is successful; otherwise, <c>null</c> if the input is <c>null</c>.
    /// </returns>
    /// <exception cref="TypeCoercionException">
    ///     Thrown if coercion is not possible for a non-null input value.
    /// </exception>
    TOutput? Coerce<TInput, TOutput>(TInput? input, TypeCoercionContext context);
    #endregion
}
