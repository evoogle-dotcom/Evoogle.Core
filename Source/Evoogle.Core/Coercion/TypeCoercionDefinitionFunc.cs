// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
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
    public Type InputType => typeof(TInput);
    public Type OutputType => typeof(TOutput);
    #endregion

    #region Properties
    private Func<TInput, TypeCoercionContext, TOutput> CoerceFunc { get; } = coerceFunc;
    #endregion

    #region ITypeCoercionDefinition Methods
    public object Coerce(object input, TypeCoercionContext context)
    {
        return this.CoerceFunc((TInput)input, context)!;
    }
    #endregion

    #region ITypeCoercionDefinition<TInput, TOutput> Methods
    public TOutput Coerce(TInput input, TypeCoercionContext context)
    {
        return this.CoerceFunc(input, context);
    }
    #endregion
}
