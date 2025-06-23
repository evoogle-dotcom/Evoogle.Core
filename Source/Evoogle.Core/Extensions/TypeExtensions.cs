// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.Extensions;

/// <summary>
///     Extension methods for .NET <see cref="Type"> class.
/// </summary>
public static class TypeExtensions
{
    #region Fields
    private const string DefaultNullText = "<null>";
    private const string DefaultEmptyText = "<empty>";
    #endregion

    #region Methods
    public static string SafeToName(this Type? type, string? nullText = DefaultNullText, string? emptyText = DefaultEmptyText)
    {
        return SafeToNameCore(type?.Name, nullText, emptyText);
    }

    public static string SafeToFullName(this Type? type, string? nullText = DefaultNullText, string? emptyText = DefaultEmptyText)
    {
        return SafeToNameCore(type?.FullName, nullText, emptyText);
    }
    #endregion

    #region Implementation Methods
    private static string SafeToNameCore(string? typeName, string? nullText = DefaultNullText, string? emptyText = DefaultEmptyText)
    {
        if (typeName == null)
            return nullText ?? DefaultNullText;

        if (string.IsNullOrWhiteSpace(typeName))
            return emptyText ?? DefaultEmptyText;

        return typeName;
    }
    #endregion
}
