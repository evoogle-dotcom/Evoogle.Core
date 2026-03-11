// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.Extensions.Internal;

namespace Evoogle.Extensions;

/// <summary>
///     Extension methods for .NET <see cref="Type"/> class.
/// </summary>
public static class TypeExtensions
{
    #region Methods
    /// <summary>
    ///     Safely returns the <c>Name</c> of the specified type, or fallback text if the type is null or the name is empty.
    /// </summary>
    /// <param name="type">The type whose <c>Name</c> should be returned.</param>
    /// <param name="nullText">Optional fallback text if the type is null. Defaults to "&lt;null&gt;".</param>
    /// <param name="emptyText">Optional fallback text if the name is empty or whitespace. Defaults to "&lt;empty&gt;".</param>
    /// <returns>
    ///     The name of the type, or <paramref name="nullText"/> if the type is null,
    ///     or <paramref name="emptyText"/> if the name is null or whitespace.
    /// </returns>
    public static string SafeToName(this Type? type, string? nullText = ExtensionsDefaults.DefaultNullText, string? emptyText = ExtensionsDefaults.DefaultEmptyText) => SafeToNameCore(type?.Name, nullText, emptyText);

    /// <summary>
    ///     Safely returns the <c>FullName</c> of the specified type, or fallback text if the type is null or the full name is empty.
    /// </summary>
    /// <param name="type">The type whose <c>FullName</c> should be returned.</param>
    /// <param name="nullText">Optional fallback text if the type is null. Defaults to "&lt;null&gt;".</param>
    /// <param name="emptyText">Optional fallback text if the full name is empty or whitespace. Defaults to "&lt;empty&gt;".</param>
    /// <returns>
    ///     The full name of the type, or <paramref name="nullText"/> if the type is null,
    ///     or <paramref name="emptyText"/> if the full name is null or whitespace.
    /// </returns>
    public static string SafeToFullName(this Type? type, string? nullText = ExtensionsDefaults.DefaultNullText, string? emptyText = ExtensionsDefaults.DefaultEmptyText) => SafeToNameCore(type?.FullName, nullText, emptyText);
    #endregion

    #region Implementation Methods
    /// <summary>
    ///     Internal helper method that returns the given type name string, or fallback text if it is null or whitespace.
    /// </summary>
    /// <param name="typeName">The name or full name string to validate.</param>
    /// <param name="nullText">Fallback text if <paramref name="typeName"/> is null.</param>
    /// <param name="emptyText">Fallback text if <paramref name="typeName"/> is whitespace or empty.</param>
    /// <returns>
    ///     The original name if valid; otherwise, fallback text for null or empty cases.
    /// </returns>
    private static string SafeToNameCore(string? typeName, string? nullText = ExtensionsDefaults.DefaultNullText, string? emptyText = ExtensionsDefaults.DefaultEmptyText)
    {
        if (typeName == null)
        {
            return nullText ?? ExtensionsDefaults.DefaultNullText;
        }

        if (string.IsNullOrWhiteSpace(typeName))
        {
            return emptyText ?? ExtensionsDefaults.DefaultEmptyText;
        }

        return typeName;
    }
    #endregion
}
