// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.Coercion;

/// <summary>
///     Implementation of <see cref="ITypeCoercionDefinition{TInput, TOutput}"/> that uses a function object as the concrete coerce implementation
///     for this combination of CLR input and output types.
/// </summary>
/// <typeparam name="TInput">CLR input type for this CLR input to output coercion definition.</typeparam>
/// <typeparam name="TOutput">CLR output type for this CLR input to output coercion definition.</typeparam>
/// <param name="coerceFunc">Function object that is the concrete coerce implementation for this combination of CLR input and output types.</param>
public class TypeCoercionDefinitionFunc<TInput, TOutput>(Func<TInput, TypeCoercionContext, TOutput> coerceFunc)
    : ITypeCoercionDefinition<TInput, TOutput>
{
    #region ITypeCoercionDefinition Properties
    /// <summary>Gets the input type for this coercion definition.</summary>    
    public Type InputType => typeof(TInput);

    /// <summary>Gets the output type for this coercion definition.</summary>
    public Type OutputType => typeof(TOutput);
    #endregion

    #region Properties
    private Func<TInput, TypeCoercionContext, TOutput> CoerceFunc { get; } = coerceFunc;
    #endregion

    #region ITypeCoercionDefinition Methods
    /// <summary>
    ///     Coerces an object of type <paramref name="input"/> to the output type using the defined coercion function.
    /// </summary>
    /// <param name="input">The input value to coerce.</param>
    /// <param name="context">The context for coercion.</param>
    /// <returns>The coerced value.</returns>    
    public object Coerce(object input, TypeCoercionContext context) => this.CoerceFunc((TInput)input, context)!;
    #endregion

    #region ITypeCoercionDefinition<TInput, TOutput> Methods
    /// <summary>
    ///     Coerces an input value of type <typeparamref name="TInput"/> to the output type using the defined coercion function.
    /// </summary>
    /// <param name="input">The input value to coerce.</param>
    /// <param name="context">The context for coercion.</param>
    /// <returns>The coerced value.</returns>    
    public TOutput Coerce(TInput input, TypeCoercionContext context) => this.CoerceFunc(input, context);
    #endregion
}
