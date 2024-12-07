// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Evoogle;

/// <summary>
///     Runtime argument checks for developer convenience.
/// </summary>
public static class Argument
{
    #region Methods
    /// <summary>
    ///     Checks if a nullable argument is null and throws a <see cref="ArgumentNullException" /> if the argument is null.
    /// </summary>
    /// <typeparam name="T">Type of argument to check.</typeparam>
    /// <param name="argument">Actual nullable argument to check at runtime if it null.</param>
    /// <param name="argumentName">Compile time resolution of the argument name.</param>
    public static void NotNull<T>([NotNull] T? argument, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
    {
        if (argument != null)
            return;

        ThrowArgumentNullException(argumentName);
    }

    /// <summary>
    ///     Checks if a nullable string argument is null or whitespace and throws a <see cref="ArgumentException" /> if the string argument is null or whitespace.
    /// </summary>
    /// <param name="argument">Actual nullable string argument to check at runtime if it is null or whitespace.</param>
    /// <param name="argumentName">Compile time resolution of the argument name.</param>
    public static void NotNullOrWhitespace([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
    {
        if (!string.IsNullOrWhiteSpace(argument))
            return;

        var message = $"Text \"{argumentName}\" is null or only whitespace.";
        ThrowArgumentException(message, argumentName);
    }

    [DoesNotReturn]
    private static void ThrowArgumentException(string? message, string? argumentName) => throw new ArgumentException(message, argumentName);

    [DoesNotReturn]
    private static void ThrowArgumentNullException(string? argumentName) => throw new ArgumentNullException(argumentName);
    #endregion
}
