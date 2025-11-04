// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.ComponentModel.DataAnnotations;

using Evoogle.Logging;

using Microsoft.Extensions.Logging;

namespace Evoogle.Json;

/// <summary>
///     Partial <see cref="JsonConverterBase{T}"/> that provides helpers for collecting
///     and reporting validation errors during (de)serialization.
/// </summary>
/// <typeparam name="T">The CLR type being converted.</typeparam>
public abstract partial class JsonConverterBase<T>
{
    #region Validation Methods
    /// <summary>
    ///     Adds a <see cref="ValidationResult"/> to the provided collection, allocating the list if necessary.
    /// </summary>
    /// <param name="results">
    ///     The results collection; may be <c>null</c>. If <c>null</c>, a new list is created.
    /// </param>
    /// <param name="message">The validation error message.</param>
    /// <param name="memberName">The member name associated with the error.</param>
    protected static void AddValidationError(ref List<ValidationResult>? results, string message, string memberName)
    {
        results ??= [];
        results.Add(new ValidationResult(message, [memberName]));
    }

    /// <summary>
    ///     Adds a standardized validation error indicating that a collection property is present but empty.
    /// </summary>
    /// <param name="results">The validation results collection; may be <c>null</c>.</param>
    /// <param name="propertyName">The name of the property.</param>
    protected static void AddEmptyCollectionPropertyError(ref List<ValidationResult>? results, string propertyName)
        => AddValidationError(ref results, $"Empty collection property: {propertyName}.", propertyName);

    /// <summary>
    ///     Adds a standardized validation error for a property that failed a custom rule.
    /// </summary>
    /// <param name="results">The validation results collection; may be <c>null</c>.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="reason">A human-readable reason describing why the property is invalid.</param>
    protected static void AddInvalidPropertyError(ref List<ValidationResult>? results, string propertyName, string reason)
        => AddValidationError(ref results, $"Invalid property: {propertyName}. Reason: {reason}", propertyName);

    /// <summary>
    ///     Adds a standardized validation error indicating that a required property is missing.
    /// </summary>
    /// <param name="results">The validation results collection; may be <c>null</c>.</param>
    /// <param name="propertyName">The name of the required property that is missing.</param>
    protected static void AddMissingPropertyError(ref List<ValidationResult>? results, string propertyName)
        => AddValidationError(ref results, $"Missing property: {propertyName}.", propertyName);

    /// <summary>
    ///     Throws a converter-specific exception when one or more validation errors are present.
    ///     Also logs the consolidated error message at <see cref="LogLevel.Error"/>.
    /// </summary>
    /// <typeparam name="TException">The exception type to throw.</typeparam>
    /// <param name="typeName">The human-readable type name being validated (for logging).</param>
    /// <param name="validationResults">The set of validation results; may be <c>null</c> or empty.</param>
    /// <param name="exceptionFactory">Factory that creates a <typeparamref name="TException"/> from the combined error message.</param>
    /// <exception cref="TException">Thrown when there are one or more non-empty error messages.</exception>
    protected void ThrowIfInvalid<TException>
    (
        string typeName,
        IEnumerable<ValidationResult>? validationResults,
        Func<string, TException> exceptionFactory
    )
        where TException : Exception
    {
        var messages = validationResults?
            .Where(v => v != ValidationResult.Success && !string.IsNullOrWhiteSpace(v.ErrorMessage))
            .Select(v => v!.ErrorMessage!)
            .ToArray();

        if (messages is null || messages.Length == 0)
        {
            return;
        }

        var message = string.Join(Environment.NewLine, messages);
        this.LogError("Validation failed for '{TypeName}': {Message}", typeName, message);

        throw exceptionFactory(message);
    }
    #endregion
}