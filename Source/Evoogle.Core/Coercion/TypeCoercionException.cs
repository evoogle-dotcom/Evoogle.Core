// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle.Coercion;

/// <summary>
///     Represents an exception that is thrown when unable to coerce from an input type to an output type.
/// </summary>
public class TypeCoercionException : Exception
{
    #region Constructors
    public TypeCoercionException(string message)
        : base(message)
    { }

    public TypeCoercionException(string message, Exception innerException)
        : base(message, innerException)
    { }
    #endregion

    #region Factory Methods
    public static TypeCoercionException Create(object? input, Type outputType, Exception? innerException = null)
    {
        var inputTypeName = input?.GetType()?.Name;
        var outputTypeName = outputType.Name;

        var message = $"Unable to coerce object {{Value={input.SafeToString()}}} from type {{Name={inputTypeName.SafeToString()}}} to type {{Name={outputTypeName.SafeToString()}}}.";
        var exception = innerException == null
            ? new TypeCoercionException(message)
            : new TypeCoercionException(message, innerException);
        return exception;
    }

    public static TypeCoercionException Create<TInput, TOutput>(TInput? input, Exception? innerException = null)
    {
        var outputType = typeof(TOutput);
        return Create(input, outputType, innerException);
    }
    #endregion
}