// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle.Coercion;

public interface ITypeCoercion
{
    #region Methods
    object? Coerce(object? input, Type outputType, TypeCoercionContext context);

    TOutput? Coerce<TInput, TOutput>(TInput? input, TypeCoercionContext context);
    #endregion
}
