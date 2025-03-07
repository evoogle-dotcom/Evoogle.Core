// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle.Coercion;

/// <summary>
///     Represents an exception that is thrown when unable to coerce from an input type to an output type.
/// </summary>
public class TypeCoercionException : Exception
{
    #region Constructors
    /// <summary>
    ///     Initializes a new instance of the <see cref="TypeCoercionException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public TypeCoercionException(string message)
        : base(message)
    { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TypeCoercionException"/> class with a specified error message and inner exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public TypeCoercionException(string message, Exception innerException)
        : base(message, innerException)
    { }
    #endregion

    #region Factory Methods
    /// <summary>
    ///     Creates an instance of the <see cref="TypeCoercionException"/> with a message indicating the coercion failure.
    /// </summary>
    /// <param name="input">The input object that was being coerced.</param>
    /// <param name="outputType">The target type to which coercion was attempted.</param>
    /// <param name="innerException">An optional inner exception that caused the failure.</param>
    /// <returns>The created exception.</returns>
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

    /// <summary>
    ///     Creates an instance of the <see cref="TypeCoercionException"/> for generic types with a message indicating the coercion failure.
    /// </summary>
    /// <typeparam name="TInput">The input type that was being coerced.</typeparam>
    /// <typeparam name="TOutput">The target output type for coercion.</typeparam>
    /// <param name="input">The input value that was being coerced.</param>
    /// <param name="innerException">An optional inner exception that caused the failure.</param>
    /// <returns>The created exception.</returns>
    public static TypeCoercionException Create<TInput, TOutput>(TInput? input, Exception? innerException = null)
    {
        var outputType = typeof(TOutput);
        return Create(input, outputType, innerException);
    }
    #endregion
}
