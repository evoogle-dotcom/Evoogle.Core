// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;

namespace Evoogle.Json;

/// <summary>
///     Partial <see cref="JsonConverterBase{T}"/> that provides helpers for writing JSON.
/// </summary>
public abstract partial class JsonConverterBase<T>
{
    #region Write Methods
    /// <summary>
    ///    Writes a JSON array using the provided writer and item writing action.
    /// </summary>
    /// <typeparam name="TItem">The type of items in the array.</typeparam>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="array">The collection of items to write.</param>
    /// <param name="writeItem">An action that writes a single item to the JSON writer.</param>
    protected static void WriteJsonArray<TItem>
    (
        Utf8JsonWriter writer,
        IEnumerable<TItem> array,
        Action<TItem> writeItem
    )
    {
        writer.WriteStartArray();
        try
        {
            foreach (var item in array)
            {
                writeItem(item);
            }
        }
        finally
        {
            writer.WriteEndArray();
        }
    }

    /// <summary>
    ///    Writes a JSON array using the provided writer and item writing action.
    ///    Overload optimized for contiguous memory via ReadOnlySpan to avoid allocations.
    /// </summary>
    /// <typeparam name="TItem">The type of items in the array.</typeparam>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="array">The contiguous span of items to write.</param>
    /// <param name="writeItem">An action that writes a single item to the JSON writer.</param>
    protected static void WriteJsonArray<TItem>
    (
        Utf8JsonWriter writer,
        ReadOnlySpan<TItem> array,
        Action<TItem> writeItem
    )
    {
        writer.WriteStartArray();
        try
        {
            for (var i = 0; i < array.Length; i++)
            {
                writeItem(array[i]);
            }
        }
        finally
        {
            writer.WriteEndArray();
        }
    }

    /// <summary>
    ///    Writes a JSON object using the provided writer and object writing action.
    /// </summary>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="writeObject">An action that writes the object to the JSON writer.</param>
    protected static void WriteJsonObject
    (
        Utf8JsonWriter writer,
        Action writeObject
    )
    {
        writer.WriteStartObject();
        try
        {
            writeObject();
        }
        finally
        {
            writer.WriteEndObject();
        }
    }
    #endregion
}
