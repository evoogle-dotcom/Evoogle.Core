// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Collections;

using Evoogle.Extensions.Internal;

namespace Evoogle.Extensions;

/// <summary>
///     Extension methods for the .NET <see cref="IEnumerable"/>  and <see cref="IEnumerable{T}"/> interfaces.
/// </summary>
public static class EnumerableExtensions
{
    #region Extension Methods
    /// <summary>
    ///     Check if enumerable has the same value throughout and if so return the value.
    ///     Safe to call on a null or empty enumerable, will return <see langword="true"/> and the default value if enumerable is null or empty.
    /// </summary>
    /// <typeparam name="T">Type of objects contained in the enumerable object.</typeparam>
    /// <param name="enumerable">Enumerable object to call extension method on.</param>
    /// <param name="value">If the enumerable is not empty and has the same value the actual value, else the default of T.</param>
    /// <param name="comparer">Optional equality comparer to check for equality, default equality comparer will be used if not passed.</param>
    /// <returns><see langword="true"/> if enumerable is empty, null or contains the same value throughout, <see langword="false"/> otherwise.</returns>
    public static bool AllEqual<T>(this IEnumerable<T>? enumerable, out T? value, EqualityComparer<T>? comparer)
    {
        if (enumerable == null)
        {
            value = default;
            return true;
        }

        using var enumerator = enumerable.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            value = default;
            return true;
        }

        value = enumerator.Current;
        comparer ??= EqualityComparer<T>.Default;

        while (enumerator.MoveNext())
        {
            if (!comparer.Equals(value, enumerator.Current))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    ///     Returns an empty enumerable if the enumerable is null, otherwise returns the original enumerable.
    /// </summary>
    /// <typeparam name="T">Type of objects contained in the enumerable.</typeparam>
    /// <param name="enumerable">Enumerable object to call extension method on.</param>
    /// <returns>An empty enumerable if the enumerable is null, otherwise the original enumerable.</returns>
    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? enumerable) => enumerable ?? [];

    /// <summary>
    ///     Predicate if the enumerable is null or empty.
    /// </summary>
    /// <typeparam name="T">Type of objects contained in the enumerable object.</typeparam>
    /// <param name="enumerable">Enumerable object to call extension method on.</param>
    /// <returns><see langword="true"/> if the enumerable object is null or empty; <see langword="false"/> otherwise.</returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? enumerable) => enumerable?.Any() != true;

    /// <summary>
    ///     Casts the elements of an enumerable to the specified type even if the enumerable is null.
    ///     If null, returns an empty enumerable of the specified type.
    /// </summary>
    /// <typeparam name="T">Type of objects contained in the enumerable object.</typeparam>
    /// <param name="enumerable">Enumerable object to call extension method on.</param>
    /// <returns>Enumerable with all elements cast to the specified type, empty enumerable if null.</returns>
    public static IEnumerable<T> SafeCast<T>(this IEnumerable? enumerable)
    {
        enumerable ??= Enumerable.Empty<T>();
        return enumerable.Cast<T>();
    }

    /// <summary>
    ///     Returns underlying array if enumerable is an actual array, otherwise an array is created from the enumerable.
    ///     If null, returns an empty array of the specified type.
    /// </summary>
    /// <typeparam name="T">Type of objects contained in the enumerable object.</typeparam>
    /// <param name="enumerable">Enumerable object to call extension method on.</param>
    /// <returns>Underlying array if enumerable is an actual array, otherwise an array is created from the enumerable or an empty array if null.</returns>
    public static T[] SafeToArray<T>(this IEnumerable<T>? enumerable)
    {
        enumerable ??= [];
        var array = enumerable as T[] ?? [.. enumerable];
        return array;
    }

    /// <summary>
    ///     Joins all the elements in the enumerable into a single string using the specified character delimiter,
    ///     with optional null and empty text substitution and optional custom formatting.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <param name="enumerable">
    ///     The enumerable to join. If null, <paramref name="nullText"/> will be returned. If empty, <paramref name="emptyText"/> will be returned.
    /// </param>
    /// <param name="delimiter">The character to use as a delimiter between elements in the resulting string.</param>
    /// <param name="nullText">
    ///     Optional text to use if the enumerable is null. Defaults to "&lt;null&gt;".
    /// </param>
    /// <param name="emptyText">
    ///     Optional text to use if the enumerable is empty. Defaults to "&lt;empty&gt;".
    /// </param>
    /// <param name="formatter">
    ///     Optional formatter function to convert each element to a string. If not provided, <c>ToString()</c> will be used.
    /// </param>
    /// <returns>
    ///     A delimited string of formatted elements, or <paramref name="nullText"/>/<paramref name="emptyText"/> as appropriate.
    /// </returns>
    public static string SafeToDelimitedString<T>(
        this IEnumerable<T>? enumerable,
        char delimiter,
        string? nullText = ExtensionsDefaults.DefaultNullText,
        string? emptyText = ExtensionsDefaults.DefaultEmptyText,
        Func<T?, string>? formatter = null)
    {
        return SafeToDelimitedStringCore
        (
            enumerable,
            delimiter,
            nullText,
            emptyText,
            formatter,
            string.Join
        );
    }

    /// <summary>
    ///     Joins all the elements in the enumerable into a single string using the specified string delimiter,
    ///     with optional null and empty text substitution and optional custom formatting.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <param name="enumerable">
    ///     The enumerable to join. If null, <paramref name="nullText"/> will be returned. If empty, <paramref name="emptyText"/> will be returned.
    /// </param>
    /// <param name="delimiter">The string to use as a delimiter between elements in the resulting string.</param>
    /// <param name="nullText">
    ///     Optional text to use if the enumerable is null. Defaults to "&lt;null&gt;".
    /// </param>
    /// <param name="emptyText">
    ///     Optional text to use if the enumerable is empty. Defaults to "&lt;empty&gt;".
    /// </param>
    /// <param name="formatter">
    ///     Optional formatter function to convert each element to a string. If not provided, <c>ToString()</c> will be used.
    /// </param>
    /// <returns>
    ///     A delimited string of formatted elements, or <paramref name="nullText"/>/<paramref name="emptyText"/> as appropriate.
    /// </returns>
    public static string SafeToDelimitedString<T>(
        this IEnumerable<T>? enumerable,
        string delimiter,
        string? nullText = ExtensionsDefaults.DefaultNullText,
        string? emptyText = ExtensionsDefaults.DefaultEmptyText,
        Func<T?, string>? formatter = null)
    {
        return SafeToDelimitedStringCore
        (
            enumerable,
            delimiter,
            nullText,
            emptyText,
            formatter,
            string.Join
        );
    }

    /// <summary>
    ///     Joins all key/value pairs in the enumerable into a single string using the specified character delimiter,
    ///     with optional formatting and substitution text for nulls and empties.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="enumerable">
    ///     The enumerable of key/value pairs to join. If null, <paramref name="nullText"/> is returned. If empty, <paramref name="emptyText"/> is returned.
    /// </param>
    /// <param name="delimiter">The character used to delimit each key=value pair in the output.</param>
    /// <param name="nullText">
    ///     Optional text to return if the enumerable is null. Defaults to "&lt;null&gt;".
    /// </param>
    /// <param name="emptyText">
    ///     Optional text to return if the enumerable is empty. Defaults to "&lt;empty&gt;".
    /// </param>
    /// <param name="keyFormatter">
    ///     Optional formatter function to convert each key to a string. If not provided, <c>ToString()</c> is used.
    /// </param>
    /// <param name="valueFormatter">
    ///     Optional formatter function to convert each value to a string. If not provided, <c>ToString()</c> is used.
    /// </param>
    /// <returns>
    ///     A delimited string of key=value formatted pairs, or <paramref name="nullText"/>/<paramref name="emptyText"/> if applicable.
    /// </returns>
    public static string SafeToDelimitedString<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>>? enumerable,
        char delimiter,
        string? nullText = ExtensionsDefaults.DefaultNullText,
        string? emptyText = ExtensionsDefaults.DefaultEmptyText,
        Func<TKey?, string?>? keyFormatter = null,
        Func<TValue?, string?>? valueFormatter = null)
    {
        return SafeToDelimitedStringCore
        (
            enumerable,
            delimiter,
            nullText,
            emptyText,
            keyFormatter,
            valueFormatter,
            string.Join
        );
    }

    /// <summary>
    ///     Joins all key/value pairs in the enumerable into a single string using the specified string delimiter,
    ///     with optional formatting and substitution text for nulls and empties.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="enumerable">
    ///     The enumerable of key/value pairs to join. If null, <paramref name="nullText"/> is returned. If empty, <paramref name="emptyText"/> is returned.
    /// </param>
    /// <param name="delimiter">The string used to delimit each key=value pair in the output.</param>
    /// <param name="nullText">
    ///     Optional text to return if the enumerable is null. Defaults to "&lt;null&gt;".
    /// </param>
    /// <param name="emptyText">
    ///     Optional text to return if the enumerable is empty. Defaults to "&lt;empty&gt;".
    /// </param>
    /// <param name="keyFormatter">
    ///     Optional formatter function to convert each key to a string. If not provided, <c>ToString()</c> is used.
    /// </param>
    /// <param name="valueFormatter">
    ///     Optional formatter function to convert each value to a string. If not provided, <c>ToString()</c> is used.
    /// </param>
    /// <returns>
    ///     A delimited string of key=value formatted pairs, or <paramref name="nullText"/>/<paramref name="emptyText"/> if applicable.
    /// </returns>
    public static string SafeToDelimitedString<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>>? enumerable,
        string delimiter,
        string? nullText = ExtensionsDefaults.DefaultNullText,
        string? emptyText = ExtensionsDefaults.DefaultEmptyText,
        Func<TKey?, string?>? keyFormatter = null,
        Func<TValue?, string?>? valueFormatter = null)
    {
        return SafeToDelimitedStringCore
        (
            enumerable,
            delimiter,
            nullText,
            emptyText,
            keyFormatter,
            valueFormatter,
            string.Join
        );
    }

    /// <summary>
    ///     Returns underlying list if enumerable is an actual list, otherwise a list is created from the enumerable.
    ///     If null, returns an empty list of the specified type.
    /// </summary>
    /// <typeparam name="T">Type of objects contained in the enumerable object.</typeparam>
    /// <param name="enumerable">Enumerable object to call extension method on.</param>
    /// <returns>Underlying list if enumerable is an actual list, otherwise a list is created from the enumerable or an empty list if null.</returns>
    public static List<T> SafeToList<T>(this IEnumerable<T>? enumerable)
    {
        enumerable ??= [];
        var list = enumerable as List<T> ?? [.. enumerable];
        return list;
    }

    /// <summary>
    ///     Returns underlying read-only collection if enumerable is an actual read-only collection, otherwise a read-only collection is created from the enumerable.
    ///     If null, returns an empty read-only collection of the specified type.
    /// </summary>
    /// <typeparam name="T">Type of objects contained in the enumerable object.</typeparam>
    /// <param name="enumerable">Enumerable object to call extension method on.</param>
    /// <returns>Underlying read-only collection if enumerable is an actual read-only collection, otherwise a read-only collection is created from the enumerable or an empty read-only collection if null.</returns>
    public static IReadOnlyCollection<T> SafeToReadOnlyCollection<T>(this IEnumerable<T>? enumerable)
    {
        enumerable ??= [];
        var readOnlyCollection = enumerable as IReadOnlyCollection<T> ?? [.. enumerable];
        return readOnlyCollection;
    }

    /// <summary>
    ///     Returns underlying read-only list if enumerable is an actual read-only list, otherwise a read-only list is created from the enumerable.
    ///     If null, returns an empty read-only list of the specified type.
    /// </summary>
    /// <typeparam name="T">Type of objects contained in the enumerable object.</typeparam>
    /// <param name="enumerable">Enumerable object to call extension method on.</param>
    /// <returns>Underlying read-only list if enumerable is an actual read-only list, otherwise a read-only list is created from the enumerable or an empty read-only list if null.</returns>
    public static IReadOnlyList<T> SafeToReadOnlyList<T>(this IEnumerable<T>? enumerable)
    {
        enumerable ??= [];
        var readOnlyList = enumerable as IReadOnlyList<T> ?? [.. enumerable];
        return readOnlyList;
    }
    #endregion

    #region Implementation Methods
    private static Func<T?, string?> GetSafeToDelimitedStringFormatter<T>() => x => x?.ToString();

    private static string GetSafeToDelimitedStringPart(string? part, string? nullText, string? emptyText)
    {
        if (!string.IsNullOrEmpty(part))
        {
            return part;
        }

        return part == null ? nullText ?? ExtensionsDefaults.DefaultNullText : emptyText ?? ExtensionsDefaults.DefaultEmptyText;
    }

    private static string SafeToDelimitedStringCore<T, TDelimiter>
    (
        this IEnumerable<T?>? enumerable,
        TDelimiter delimiter,
        string? nullText,
        string? emptyText,
        Func<T?, string?>? formatter,
        Func<TDelimiter, IEnumerable<string>, string> joiner
    )
    {
        if (enumerable == null)
        {
            return nullText ?? ExtensionsDefaults.DefaultNullText;
        }

        if (!enumerable.Any())
        {
            return emptyText ?? ExtensionsDefaults.DefaultEmptyText;
        }

        formatter ??= GetSafeToDelimitedStringFormatter<T>();

        var parts = enumerable.Select(x => GetSafeToDelimitedStringPart(formatter(x), nullText, emptyText));

        var delimited = joiner(delimiter, parts);

        return delimited;
    }

    private static string SafeToDelimitedStringCore<TKey, TValue, TDelimiter>
    (
        this IEnumerable<KeyValuePair<TKey, TValue>>? enumerable,
        TDelimiter delimiter,
        string? nullText,
        string? emptyText,
        Func<TKey?, string?>? keyFormatter,
        Func<TValue?, string?>? valueFormatter,
        Func<TDelimiter, IEnumerable<string>, string> joiner
    )
    {
        if (enumerable == null)
        {
            return nullText ?? ExtensionsDefaults.DefaultNullText;
        }

        if (!enumerable.Any())
        {
            return emptyText ?? ExtensionsDefaults.DefaultEmptyText;
        }

        keyFormatter ??= GetSafeToDelimitedStringFormatter<TKey>();
        valueFormatter ??= GetSafeToDelimitedStringFormatter<TValue>();

        var parts = enumerable.Select(x =>
        {
            var keyPart = GetSafeToDelimitedStringPart(keyFormatter(x.Key), nullText, emptyText);
            var valuePart = GetSafeToDelimitedStringPart(valueFormatter(x.Value), nullText, emptyText);

            return $"{keyPart}={valuePart}";
        });

        var delimited = joiner(delimiter, parts);

        return delimited;
    }
    #endregion
}
