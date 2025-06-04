// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle;

/// <summary>
///     Extension methods for .NET <see cref="Type"> class.
/// </summary>
public static class TypeExtensions
{
    #region Methods
    /// <summary>
    ///     Gets a safe ToString method invocation of the .NET Type object even if the .NET Type object is null.
    ///     Provides optional parameters to customize the text when the .NET Type object is null or the .NET Type object ToString method returned an empty string.
    /// </summary>
    /// <param name="type">.NET Type object to call extension method on.</param>
    /// <param name="emptyText">
    ///     Optional parameter to set what text should be used if the reference object ToString is indeed empty.
    ///     Defaults to the text '<empty>' if not supplied.
    /// </param>
    /// <param name="nullText">
    ///     Optional parameter to set what text should be used if the reference object or the ToString result is indeed null.
    ///     Defaults to the text '<null>' if not supplied.
    /// </param>
    /// <returns>
    ///     The actual <c>ToString</c> representation if available.
    ///     If the reference type is null or the actual <c>ToString</c> representation is null, then the parameter nullText is returned.
    ///     If the reference type <c>ToString</c> representation is empty, then the parameter emptyText is returned.
    /// </returns>
    public static string SafeToString(this Type? type, string? emptyText = "<empty>", string? nullText = "<null>")
    {
        var toStringResult = type?.Name?.ToString();

        if (toStringResult == null)
            return nullText ?? "<null>";

        if (string.IsNullOrWhiteSpace(toStringResult))
            return emptyText ?? "<empty>";

        return toStringResult;
    }
    #endregion
}
