// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.Coercion;

public class NonGenericCoerceTest<TInput, TOutput> : CoerceTest<TInput, TOutput>
{
    #region Methods
    protected override TOutput? CoerceImpl(TypeCoercion typeCoercion, TInput? input, TypeCoercionContext context)
    {
        var actualOutput = (TOutput?)typeCoercion.Coerce(input, typeof(TOutput), context);
        return actualOutput;
    }
    #endregion
}
