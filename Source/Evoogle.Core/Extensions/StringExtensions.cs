// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.RegularExpressions;

namespace Evoogle.Extensions;

/// <summary>
///     Extension methods for .NET <see cref="string"/> class.
/// </summary>
public static partial class StringExtensions
{
    #region Methods
    /// <summary>
    ///     Masks a string by replacing the middle portion with a specified character, keeping the specified number of characters
    ///     unmasked on the left and right, while ensuring a minimum number of masked characters.
    /// </summary>
    /// <param name="str">The input string to mask. If null or whitespace, it is returned as-is.</param>
    /// <param name="maskChar">The character to use for masking. Defaults to '*'.</param>
    /// <param name="unmaskedLeftCount">The number of characters to leave unmasked on the left. Defaults to 1.</param>
    /// <param name="unmaskedRightCount">
    ///     The number of characters to leave unmasked on the right. If null, defaults to <c>Min((length - 6) / 2, 4)</c>.
    /// </param>
    /// <param name="minMaskedCount">
    ///     The minimum number of characters that must be masked. If the string is too short to satisfy this, the entire string is masked.
    ///     Defaults to 8.
    /// </param>
    /// <returns>The masked string with middle characters replaced, ensuring at least <paramref name="minMaskedCount"/> characters are masked.</returns>
    public static string? Mask(
        this string? str,
        char maskChar = '*',
        int unmaskedLeftCount = 1,
        int? unmaskedRightCount = null,
        int minMaskedCount = 8)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return str;
        }

        str = str.Trim();
        var len = str.Length;

        // If not enough characters to satisfy the minimum masking, mask the entire string
        if (len <= minMaskedCount)
        {
            return new string(maskChar, len);
        }

        var leftLen = Math.Clamp(unmaskedLeftCount, 0, len);
        var rightLen = unmaskedRightCount ?? (len > 6 ? Math.Min((len - 6) / 2, 4) : 0);
        rightLen = Math.Clamp(rightLen, 0, len - leftLen);

        var maskedLen = len - leftLen - rightLen;

        // Try to satisfy minimum masked length by reducing right first, then left
        if (maskedLen < minMaskedCount)
        {
            var available = len - minMaskedCount;

            // Reduce rightLen first
            rightLen = Math.Clamp(rightLen, 0, available);
            maskedLen = len - leftLen - rightLen;

            // If still insufficient, reduce leftLen
            if (maskedLen < minMaskedCount)
            {
                leftLen = Math.Clamp(len - rightLen - minMaskedCount, 0, leftLen);
                maskedLen = len - leftLen - rightLen;

                // Final fallback: mask whole string
                if (maskedLen < minMaskedCount)
                {
                    return new string(maskChar, len);
                }
            }
        }

        var masked = new string(maskChar, maskedLen);
        return str[..leftLen] + masked + str[(len - rightLen)..];
    }

    /// <summary>
    ///     Fully masks a string by replacing every character with the specified mask character, regardless of its length.
    ///     Useful for completely hiding sensitive data.
    /// </summary>
    /// <param name="str">The input string to fully mask. If null or empty, it is returned as-is.</param>
    /// <param name="maskChar">The character to use for masking. Defaults to '*'.</param>
    /// <returns>
    ///     A new string of equal length where each character is replaced by <paramref name="maskChar"/>.
    ///     Returns the original string if null or empty.
    /// </returns>
    public static string? MaskFully(this string? str, char maskChar = '*')
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        str = str.Trim();
        return new string(maskChar, str.Length);
    }

    /// <summary>
    ///     Remove all whitespace characters from the string.
    /// </summary>
    /// <param name="str">String to remove any whitespace from.</param>
    /// <returns>New string created from the parameter string with any whitespace removed.</returns>
    public static string? RemoveWhitespace(this string? str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        return WhitespaceRegex().Replace(str, string.Empty);
    }

    [GeneratedRegex("\\s", RegexOptions.CultureInvariant)]
    private static partial Regex WhitespaceRegex();
    #endregion
}
