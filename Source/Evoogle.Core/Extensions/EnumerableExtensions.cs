// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Collections;

namespace Evoogle;

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
    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? enumerable)
    {
        return enumerable ?? Enumerable.Empty<T>();
    }

    /// <summary>
    ///     Predicate if the enumerable is null or empty.
    /// </summary>
    /// <typeparam name="T">Type of objects contained in the enumerable object.</typeparam>
    /// <param name="enumerable">Enumerable object to call extension method on.</param>
    /// <returns><see langword="true"/> if the enumerable object is null or empty; <see langword="false"/> otherwise.</returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? enumerable)
    {
        return enumerable?.Any() != true;
    }

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
        enumerable ??= Enumerable.Empty<T>();
        var array = enumerable as T[] ?? enumerable.ToArray();
        return array;
    }

    /// <summary>
    ///     Returns underlying collection if enumerable is an actual collection, otherwise a collection is created from the enumerable.
    ///     If null, returns an empty collection of the specified type.
    /// </summary>
    /// <typeparam name="T">Type of objects contained in the enumerable object.</typeparam>
    /// <param name="enumerable">Enumerable object to call extension method on.</param>
    /// <returns>Underlying collection if enumerable is an actual collection, otherwise a collection is created from the enumerable or an empty collection if null.</returns>
    public static ICollection<T> SafeToCollection<T>(this IEnumerable<T>? enumerable)
    {
        enumerable ??= Enumerable.Empty<T>();
        var collection = enumerable as ICollection<T> ?? enumerable.ToList();
        return collection;
    }

    /// <summary>
    ///     Returns underlying list if enumerable is an actual list, otherwise a list is created from the enumerable.
    ///     If null, returns an empty list of the specified type.
    /// </summary>
    /// <typeparam name="T">Type of objects contained in the enumerable object.</typeparam>
    /// <param name="enumerable">Enumerable object to call extension method on.</param>
    /// <returns>Underlying list if enumerable is an actual list, otherwise a list is created from the enumerable or an empty list if null.</returns>
    public static IList<T> SafeToList<T>(this IEnumerable<T>? enumerable)
    {
        enumerable ??= Enumerable.Empty<T>();
        var list = enumerable as IList<T> ?? enumerable.ToList();
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
        enumerable ??= Enumerable.Empty<T>();
        var readOnlyCollection = enumerable as IReadOnlyCollection<T> ?? enumerable.ToList();
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
        enumerable ??= Enumerable.Empty<T>();
        var readOnlyList = enumerable as IReadOnlyList<T> ?? enumerable.ToList();
        return readOnlyList;
    }

    /// <summary>
    ///     Joins all the enumerable items representations as strings (via ToString) into a single string with respect to the given delimiter string even if the enumerable is null.
    ///     If the enumerable is null, returns null text.
    ///     If the enumerable is empty, returns empty text.
    /// </summary>
    /// <typeparam name="T">Type of objects contained in the enumerable object.</typeparam>
    /// <param name="enumerable">Enumerable object to call extension method on.</param>
    /// <param name="delimiter">
    ///     String to use as a delimiter.
    ///     Delimiter is included in the returned string only if value has more than one element.
    /// </param>
    /// <param name="nullText">
    ///     Optional parameter to set what text should be used if the enumable is indeed null.
    ///     Defaults to the text '<null>' if not supplied.
    /// </param>
    /// <param name="emptyText">
    ///     Optional parameter to set what text should be used if the delimited string is indeed empty.
    ///     Defaults to the text '<empty>' if not supplied.
    /// </param>
    /// <returns>
    ///     Delimited string of all the enumerable items converted to a string if not null or empty, otherwise the null or empty text if the enumerable is null or empty respectively.
    /// </returns>
    public static string SafeToDelimitedString<T>(
        this IEnumerable<T>? enumerable,
        string delimiter,
        string? nullText = "<null>",
        string? emptyText = "<empty>")
    {
        if (enumerable == null)
            return nullText!;

        var delimitedString = string.Join(delimiter, enumerable);
        if (string.IsNullOrWhiteSpace(delimitedString))
            return emptyText!;

        return delimitedString;
    }

    /// <summary>
    ///     Joins all the enumerable items representations as strings (via ToString) into a single string with respect to the given delimiter character even if the enumerable is null.
    ///     If the enumerable is null, returns null text.
    ///     If the enumerable is empty, returns empty text.
    /// </summary>
    /// <typeparam name="T">Type of objects contained in the enumerable object.</typeparam>
    /// <param name="enumerable">Enumerable object to call extension method on.</param>
    /// <param name="delimiter">
    ///     Character to use as a delimiter.
    ///     Delimiter is included in the returned string only if value has more than one element.
    /// </param>
    /// <param name="nullText">
    ///     Optional parameter to set what text should be used if the enumable is indeed null.
    ///     Defaults to the text '<null>' if not supplied.
    /// </param>
    /// <param name="emptyText">
    ///     Optional parameter to set what text should be used if the delimited string is indeed empty.
    ///     Defaults to the text '<empty>' if not supplied.
    /// </param>
    /// <returns>
    ///     Delimited string of all the enumerable items converted to a string if not null or empty, otherwise the null or empty text if the enumerable is null or empty respectively.
    /// </returns>
    public static string SafeToDelimitedString<T>(
        this IEnumerable<T>? enumerable,
        char delimiter,
        string? nullText = "<null>",
        string? emptyText = "<empty>")
    {
        if (enumerable == null)
            return nullText!;

        var delimitedString = string.Join(delimiter, enumerable);
        if (string.IsNullOrWhiteSpace(delimitedString))
            return emptyText!;

        return delimitedString;
    }

    /// <summary>
    ///     Joins all the enumerable key/value pairs representations as strings (via ToString) into a single string with respect to the given delimiter string even if the enumerable is null.
    ///     If the enumerable is null, returns null text.
    ///     If the enumerable is empty, returns empty text.
    /// </summary>
    /// <typeparam name="TKey">Type of key in the key/value pair contained in the enumerable object.</typeparam>
    /// <typeparam name="TValue">Type of value in the key/value pair contained in the enumerable object.</typeparam>
    /// <param name="enumerable">Enumerable object to call extension method on.</param>
    /// <param name="delimiter">
    ///     Character to use as a delimiter.
    ///     Delimiter is included in the returned string only if value has more than one element.
    /// </param>
    /// <param name="nullText">
    ///     Optional parameter to set what text should be used if the enumable is indeed null.
    ///     Defaults to the text '<null>' if not supplied.
    /// </param>
    /// <param name="emptyText">
    ///     Optional parameter to set what text should be used if the delimited string is indeed empty.
    ///     Defaults to the text '<empty>' if not supplied.
    /// </param>
    /// <returns>
    ///     Delimited string of all the enumerable key/value pairs converted to a string if not null or empty, otherwise the null or empty text if the enumerable is null or empty respectively.
    /// </returns>
    public static string SafeToDelimitedString<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>>? enumerable,
        string delimiter,
        string? nullText = "<null>",
        string? emptyText = "<empty>")
    {
        if (enumerable == null)
            return nullText!;

        var delimitedString = string.Join(delimiter, enumerable.Select(x => $"{x.Key}={x.Value}"));
        if (string.IsNullOrWhiteSpace(delimitedString))
            return emptyText!;

        return delimitedString;
    }

    /// <summary>
    ///     Joins all the enumerable key/value pairs representations as strings (via ToString) into a single string with respect to the given delimiter character even if the enumerable is null.
    ///     If the enumerable is null, returns null text.
    ///     If the enumerable is empty, returns empty text.
    /// </summary>
    /// <typeparam name="TKey">Type of key in the key/value pair contained in the enumerable object.</typeparam>
    /// <typeparam name="TValue">Type of value in the key/value pair contained in the enumerable object.</typeparam>
    /// <param name="enumerable">Enumerable object to call extension method on.</param>
    /// <param name="delimiter">
    ///     Character to use as a delimiter.
    ///     Delimiter is included in the returned string only if value has more than one element.
    /// </param>
    /// <param name="nullText">
    ///     Optional parameter to set what text should be used if the enumable is indeed null.
    ///     Defaults to the text '<null>' if not supplied.
    /// </param>
    /// <param name="emptyText">
    ///     Optional parameter to set what text should be used if the delimited string is indeed empty.
    ///     Defaults to the text '<empty>' if not supplied.
    /// </param>
    /// <returns>
    ///     Delimited string of all the enumerable key/value pairs converted to a string if not null or empty, otherwise the null or empty text if the enumerable is null or empty respectively.
    /// </returns>
    public static string SafeToDelimitedString<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>>? enumerable,
        char delimiter,
        string? emptyText = "<empty>",
        string? nullText = "<null>")
    {
        if (enumerable == null)
            return !string.IsNullOrWhiteSpace(nullText) ? nullText : "<null>";

        var sourceAsKeyValuePairStringCollection = enumerable.Select(x => $"{x.Key}={x.Value}");

        var delimitedString = string.Join(delimiter, sourceAsKeyValuePairStringCollection);
        if (string.IsNullOrWhiteSpace(delimitedString))
            return !string.IsNullOrWhiteSpace(emptyText) ? emptyText : "<empty>";

        return delimitedString;
    }
    #endregion
}