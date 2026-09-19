// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;

namespace Evoogle.Json;

/// <inheritdoc cref="Utf8JsonWriterExtensions"/>
public static partial class Utf8JsonWriterExtensions
{
    #region TryWritePropertyAsArray Extension Methods
    /// <summary>
    ///     Attempts to write a property containing a JSON array when the serializer options
    ///     permit the collection value to be included.
    /// </summary>
    /// <typeparam name="TItem">The type of items in the array.</typeparam>
    /// <param name="writer">The writer to which the property is written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="collection">The collection of items to write.</param>
    /// <param name="options">The serializer options that control when values are ignored.</param>
    /// <param name="writeItem">The action that writes each item to the JSON writer.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="writer"/>, <paramref name="propertyName"/>,
    ///     <paramref name="options"/>, or <paramref name="writeItem"/> is <see langword="null"/>.
    /// </exception>
    public static bool TryWritePropertyAsArray<TItem>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        IEnumerable<TItem>? collection,
        JsonSerializerOptions options,
        Action<Utf8JsonWriter, TItem> writeItem
    )
    {
        return writer.TryWriteProperty
        (
            propertyName,
            collection,
            options,
            writeItem,
            static (writer, collection, writeItem) => writer.WriteJsonArray(collection, writeItem)
        );
    }

    /// <summary>
    ///     Attempts to write a property containing a JSON array when the serializer options
    ///     permit the collection value to be included, passing state to the item-writing action.
    /// </summary>
    /// <typeparam name="TItem">The type of items in the array.</typeparam>
    /// <typeparam name="TState">The type of state passed to the item-writing action.</typeparam>
    /// <param name="writer">The writer to which the property is written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="collection">The collection of items to write.</param>
    /// <param name="state">The state passed to the item-writing action.</param>
    /// <param name="options">The serializer options that control when values are ignored.</param>
    /// <param name="writeItem">The action that writes each item to the JSON writer.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="writer"/>, <paramref name="propertyName"/>,
    ///     <paramref name="options"/>, or <paramref name="writeItem"/> is <see langword="null"/>.
    /// </exception>
    public static bool TryWritePropertyAsArray<TItem, TState>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        IEnumerable<TItem>? collection,
        TState state,
        JsonSerializerOptions options,
        Action<Utf8JsonWriter, TItem, TState> writeItem
    )
    {
        return writer.TryWriteProperty
        (
            propertyName,
            collection,
            options,
            (state, writeItem),
            static (writer, collection, callbackState) =>
                writer.WriteJsonArray(collection, callbackState.state, callbackState.writeItem)
        );
    }

    /// <summary>
    ///     Writes a property containing a JSON array from a contiguous span.
    ///     The property is always written.
    /// </summary>
    /// <typeparam name="TItem">The type of items in the array.</typeparam>
    /// <param name="writer">The writer to which the property is written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="span">The contiguous span of items to write.</param>
    /// <param name="writeItem">The action that writes each item to the JSON writer.</param>
    /// <returns><c>true</c> because the property is always written.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="writer"/>, <paramref name="propertyName"/>,
    ///     or <paramref name="writeItem"/> is <see langword="null"/>.
    /// </exception>
    public static bool TryWritePropertyAsArray<TItem>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        ReadOnlySpan<TItem> span,
        Action<Utf8JsonWriter, TItem> writeItem
    )
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(propertyName);
        ArgumentNullException.ThrowIfNull(writeItem);

        writer.WritePropertyName(propertyName);
        writer.WriteJsonArray(span, writeItem);
        return true;
    }

    /// <summary>
    ///     Writes a property containing a JSON array from a contiguous span, passing state to
    ///     the item-writing action. The property is always written.
    /// </summary>
    /// <typeparam name="TItem">The type of items in the array.</typeparam>
    /// <typeparam name="TState">The type of state passed to the item-writing action.</typeparam>
    /// <param name="writer">The writer to which the property is written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="span">The contiguous span of items to write.</param>
    /// <param name="state">The state passed to the item-writing action.</param>
    /// <param name="writeItem">The action that writes each item to the JSON writer.</param>
    /// <returns><c>true</c> because the property is always written.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="writer"/>, <paramref name="propertyName"/>,
    ///     or <paramref name="writeItem"/> is <see langword="null"/>.
    /// </exception>
    public static bool TryWritePropertyAsArray<TItem, TState>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        ReadOnlySpan<TItem> span,
        TState state,
        Action<Utf8JsonWriter, TItem, TState> writeItem
    )
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(propertyName);
        ArgumentNullException.ThrowIfNull(writeItem);

        writer.WritePropertyName(propertyName);
        writer.WriteJsonArray(span, state, writeItem);
        return true;
    }
    #endregion

    #region WriteJsonArray Extension Methods
    /// <summary>
    ///     Writes a JSON array using the provided collection and item-writing action.
    /// </summary>
    /// <typeparam name="TItem">The type of items in the array.</typeparam>
    /// <param name="writer">The writer to which the array is written.</param>
    /// <param name="collection">The collection of items to write.</param>
    /// <param name="writeItem">The action that writes each item to the JSON writer.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="writer"/>, <paramref name="collection"/>, or
    ///     <paramref name="writeItem"/> is <see langword="null"/>.
    /// </exception>
    public static void WriteJsonArray<TItem>
    (
        this Utf8JsonWriter writer,
        IEnumerable<TItem> collection,
        Action<Utf8JsonWriter, TItem> writeItem
    )
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(writeItem);

        writer.WriteStartArray();
        foreach (var item in collection)
        {
            writeItem(writer, item);
        }
        writer.WriteEndArray();
    }

    /// <summary>
    ///     Writes a JSON array using the provided collection and item-writing action, passing
    ///     state to the action for each item.
    /// </summary>
    /// <typeparam name="TItem">The type of items in the array.</typeparam>
    /// <typeparam name="TState">The type of state passed to the item-writing action.</typeparam>
    /// <param name="writer">The writer to which the array is written.</param>
    /// <param name="collection">The collection of items to write.</param>
    /// <param name="state">The state passed to the item-writing action.</param>
    /// <param name="writeItem">The action that writes each item to the JSON writer.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="writer"/>, <paramref name="collection"/>, or
    ///     <paramref name="writeItem"/> is <see langword="null"/>.
    /// </exception>
    public static void WriteJsonArray<TItem, TState>
    (
        this Utf8JsonWriter writer,
        IEnumerable<TItem> collection,
        TState state,
        Action<Utf8JsonWriter, TItem, TState> writeItem
    )
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(writeItem);

        writer.WriteStartArray();
        foreach (var item in collection)
        {
            writeItem(writer, item, state);
        }
        writer.WriteEndArray();
    }

    /// <summary>
    ///     Writes a JSON array using the provided contiguous span and item-writing action.
    /// </summary>
    /// <typeparam name="TItem">The type of items in the array.</typeparam>
    /// <param name="writer">The writer to which the array is written.</param>
    /// <param name="span">The contiguous span of items to write.</param>
    /// <param name="writeItem">The action that writes each item to the JSON writer.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="writer"/> or <paramref name="writeItem"/>
    ///     is <see langword="null"/>.
    /// </exception>
    public static void WriteJsonArray<TItem>
    (
        this Utf8JsonWriter writer,
        ReadOnlySpan<TItem> span,
        Action<Utf8JsonWriter, TItem> writeItem
    )
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(writeItem);

        writer.WriteStartArray();
        for (var i = 0; i < span.Length; i++)
        {
            writeItem(writer, span[i]);
        }
        writer.WriteEndArray();
    }

    /// <summary>
    ///     Writes a JSON array using the provided contiguous span and item-writing action,
    ///     passing state to the action for each item.
    /// </summary>
    /// <typeparam name="TItem">The type of items in the array.</typeparam>
    /// <typeparam name="TState">The type of state passed to the item-writing action.</typeparam>
    /// <param name="writer">The writer to which the array is written.</param>
    /// <param name="span">The contiguous span of items to write.</param>
    /// <param name="state">The state passed to the item-writing action.</param>
    /// <param name="writeItem">The action that writes each item to the JSON writer.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="writer"/> or <paramref name="writeItem"/>
    ///     is <see langword="null"/>.
    /// </exception>
    public static void WriteJsonArray<TItem, TState>
    (
        this Utf8JsonWriter writer,
        ReadOnlySpan<TItem> span,
        TState state,
        Action<Utf8JsonWriter, TItem, TState> writeItem
    )
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(writeItem);

        writer.WriteStartArray();
        for (var i = 0; i < span.Length; i++)
        {
            writeItem(writer, span[i], state);
        }
        writer.WriteEndArray();
    }
    #endregion
}
