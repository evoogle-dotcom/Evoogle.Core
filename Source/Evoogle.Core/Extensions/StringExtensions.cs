// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Text.RegularExpressions;

namespace Evoogle;

/// <summary>
///     Extension methods for .NET <see cref="string"> class.
/// </summary>
public static partial class StringExtensions
{
    #region Methods
    /// <summary>
    ///     Mask a string by revealing the left most character, '*' characters in the middle, and revealing up to 4 characters on the right all respect to string length.
    ///     Useful for masking sensitive information like passwords or credit card numbers in log files, etc.
    /// </summary>
    /// <param name="str">String to mask.</param>
    /// <returns>
    ///     New string created from the parameter string masked if the parameter string is not null or empty, otherwise a copy of the parameter string is returned.
    /// </returns>
    public static string? Mask(this string? str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        var len = str.Length;
        var leftLen = len > 4 ? 1 : 0;
        var rightLen = len > 6 ? Math.Min((len - 6) / 2, 4) : 0;
        return str[..leftLen] + new string('*', len - leftLen - rightLen) + str[(len - rightLen)..];
    }

    /// <summary>
    ///     Remove all whitespace characters from the string.
    /// </summary>
    /// <param name="str">String to remove any whitespace from.</param>
    /// <returns>New string created from the parameter string with any whitespace removed.</returns>
    public static string? RemoveWhitespace(this string? str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        return WhitespaceRegex().Replace(str, string.Empty);
    }

    [GeneratedRegex("\\s", RegexOptions.CultureInvariant)]
    private static partial Regex WhitespaceRegex();
    #endregion
}
