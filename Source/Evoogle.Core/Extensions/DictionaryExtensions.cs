// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle;

/// <summary>
///     Extension methods for the .NET <see cref="IDictionary{TKey,TValue}"/> interface.
/// </summary>
public static class DictionaryExtensions
{
    #region Extension Methods
    /// <summary>
    ///     Get the value by key from the dictionary, throws an exception if the value does not exist in the dictionary.
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="dictionary">Dictionary object to call extension method on.</param>
    /// <param name="key">Key to get the value by in the dictionary.</param>
    /// <returns>The indexed value by key, throws a <see cref="KeyNotFoundException"/> if the value does not exist in the dictionary.</returns>
    public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        if (dictionary.TryGetValue(key, out TValue? value))
            return value;

        var message = $"Unable to get value for given key '{key}' from dictionary, key does not exist in dictionary.";
        throw new KeyNotFoundException(message);
    }
    #endregion
}