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
    #region TryWritePropertyWithAction Extension Methods
    /// <summary>
    ///     Attempts to write a property using a custom action when the serializer options
    ///     permit the value to be included.
    /// </summary>
    /// <typeparam name="T">The type of the value to write.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="writeAction">The action that writes the value after the property name.</param>
    /// <param name="equalityComparer">
    ///     Optional comparer used to detect default values.
    /// </param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="writer"/>, <paramref name="propertyName"/>,
    ///     <paramref name="options"/>, or <paramref name="writeAction"/> is <see langword="null"/>.
    /// </exception>
    public static bool TryWritePropertyWithAction<T>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        T value,
        JsonSerializerOptions options,
        Action<Utf8JsonWriter, T> writeAction,
        EqualityComparer<T>? equalityComparer = null
    )
        where T : struct
    {
        return writer.TryWriteProperty(propertyName, value, options, writeAction, equalityComparer);
    }

    /// <summary>
    ///     Attempts to write a nullable property using a custom action when the serializer
    ///     options permit the value to be included.
    /// </summary>
    /// <typeparam name="T">The type of the value to write.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="nullableValue">The value to write, if present.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="writeAction">The action that writes the value after the property name.</param>
    /// <param name="equalityComparer">
    ///     Optional comparer used to detect default values.
    /// </param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="writer"/>, <paramref name="propertyName"/>,
    ///     <paramref name="options"/>, or <paramref name="writeAction"/> is <see langword="null"/>.
    /// </exception>
    public static bool TryWritePropertyWithAction<T>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        T? nullableValue,
        JsonSerializerOptions options,
        Action<Utf8JsonWriter, T> writeAction,
        EqualityComparer<T>? equalityComparer = null
    )
        where T : struct
    {
        return writer.TryWriteProperty(propertyName, nullableValue, options, writeAction, equalityComparer);
    }

    /// <summary>
    ///     Attempts to write a reference type property using a custom action when the serializer
    ///     options permit the value to be included.
    /// </summary>
    /// <typeparam name="T">The type of the value to write.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="obj">The object to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="writeAction">The action that writes the value after the property name.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="writer"/>, <paramref name="propertyName"/>,
    ///     <paramref name="options"/>, or <paramref name="writeAction"/> is <see langword="null"/>.
    /// </exception>
    public static bool TryWritePropertyWithAction<T>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        T? obj,
        JsonSerializerOptions options,
        Action<Utf8JsonWriter, T> writeAction
    )
        where T : class
    {
        return writer.TryWriteProperty(propertyName, obj, options, writeAction);
    }

    /// <summary>
    ///     Writes a property whose value is represented by a contiguous
    ///     <see cref="ReadOnlySpan{T}"/>.
    /// </summary>
    /// <remarks>
    ///     This overload always writes the property and does not apply serializer ignore
    ///     conditions. The <paramref name="writeAction"/> owns serialization of the span value.
    /// </remarks>
    /// <typeparam name="T">The element type contained in the span.</typeparam>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="propertyName">The JSON property name to write.</param>
    /// <param name="span">The read-only span of elements to serialize.</param>
    /// <param name="options">Serializer options reserved for consistency with other overloads.</param>
    /// <param name="writeAction">
    ///     The action that receives the writer and span and writes the span contents.
    /// </param>
    /// <returns><c>true</c> because the property is always written.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="writer"/>, <paramref name="propertyName"/>,
    ///     or <paramref name="writeAction"/> is <see langword="null"/>.
    /// </exception>
    public static bool TryWritePropertyWithAction<T>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        ReadOnlySpan<T> span,
        JsonSerializerOptions options,
        Action<Utf8JsonWriter, ReadOnlySpan<T>> writeAction
    )
    {
        // We intentionally always write the property to avoid span capture complexities in generic filtering.
        writer.WritePropertyName(propertyName);
        writeAction(writer, span);
        return true;
    }
    #endregion
}
