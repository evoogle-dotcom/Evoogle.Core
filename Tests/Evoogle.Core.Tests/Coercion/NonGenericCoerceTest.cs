// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle.Coercion;

public class NonGenericCoerceTest<TInput, TOutput> : CoerceTest<TInput, TOutput>
{
    #region Methods
    protected override TOutput? CoerceImpl(ITypeCoercion typeCoercion, TInput? input, TypeCoercionContext context)
    {
        var actualOutput = (TOutput?)typeCoercion.Coerce(input, typeof(TOutput), context);
        return actualOutput;
    }
    #endregion
}
