// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle.Coercion;

public class GenericCoerceTest<TInput, TOutput> : CoerceTest<TInput, TOutput>
{
    #region Methods
    protected override TOutput? CoerceImpl(ITypeCoercion typeCoercion, TInput? input, TypeCoercionContext context)
    {
        var actualOutput = typeCoercion.Coerce<TInput, TOutput>(input, context);
        return actualOutput;
    }
    #endregion
}
