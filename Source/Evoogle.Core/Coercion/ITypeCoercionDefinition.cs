// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle.Coercion;

/// <summary>
///     Abstracts a non-generic type coercion definition for a specific combination of input to output type.
/// </summary>
public interface ITypeCoercionDefinition
{
    #region Properties
    /// <summary>Gets the CLR input type for this CLR input to output coercion definition.</summary>
    Type InputType { get; }

    /// <summary>Gets the CLR output type for this CLR input to output coercion definition.</summary>
    Type OutputType { get; }
    #endregion

    #region Methods
    /// <summary>
    ///     Abstracts a non-generic coercion exection for this CLR input to output coercion definition.
    ///     Throws a <see cref="TypeCoercionException"/> if unable to coerce the input object into the output object.
    /// </summary>
    /// <param name="input">Input object to coerce from. May be null.</param>
    /// <param name="context">Runtime context for the coerce execution.</param>
    /// <returns>
    ///     The coerced output object from the input object, otherwise a <see cref="TypeCoercionException"/> is thrown.
    /// </returns>
    object Coerce(object input, TypeCoercionContext context);
    #endregion
}

/// <summary>
///     Abstracts a generic type coercion definition for a specific combination of input to output type as generic parameters known at compile time.
/// </summary>
/// <typeparam name="TInput">CLR input type for this CLR input to output coercion definition.</typeparam>
/// <typeparam name="TOutput">CLR output type for this CLR input to output coercion definition.</typeparam>
public interface ITypeCoercionDefinition<TInput, TOutput> : ITypeCoercionDefinition
{
    #region Methods
    /// <summary>
    ///     Abstracts a generic coercion execution for this CLR input to output coercion definition.
    ///     Throws a <see cref="TypeCoercionException"/> if unable to coerce the input object into the output object.
    /// </summary>
    /// <param name="input">Input object to coerce from. May be null.</param>
    /// <param name="context">Runtime context for the coerce execution.</param>
    /// <returns>
    ///     The coerced output object from the input object, otherwise a <see cref="TypeCoercionException"/> is thrown.
    /// </returns>
    TOutput Coerce(TInput input, TypeCoercionContext context);
    #endregion
}

