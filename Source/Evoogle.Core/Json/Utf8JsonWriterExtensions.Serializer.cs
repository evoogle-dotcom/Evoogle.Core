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
    #region TryWriteWithSerializer Extension Methods
    /// <summary>
    ///     Attempts to serialize a value using <see cref="JsonSerializer"/> when the serializer
    ///     options permit it.
    /// </summary>
    /// <typeparam name="T">The type of the value to serialize.</typeparam>
    /// <param name="writer">The writer to which the value will be serialized.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="equalityComparer">
    ///     Optional comparer used to detect default values.
    /// </param>
    /// <returns><c>true</c> if the value was written; otherwise, <c>false</c>.</returns>
    public static bool TryWriteWithSerializer<T>
    (
        this Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options,
        EqualityComparer<T>? equalityComparer = null
    )
        where T : struct
    {
        return writer.TryWrite
        (
            value,
            options,
            (v) =>
            {
                JsonSerializer.Serialize(writer, v, options);
            },
            equalityComparer
        );
    }

    /// <summary>
    ///     Attempts to serialize a nullable value using <see cref="JsonSerializer"/> when the
    ///     serializer options permit it.
    /// </summary>
    /// <typeparam name="T">The type of the value to serialize.</typeparam>
    /// <param name="writer">The writer to which the value will be serialized.</param>
    /// <param name="nullableValue">The value to serialize, if present.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="equalityComparer">
    ///     Optional comparer used to detect default values.
    /// </param>
    /// <returns><c>true</c> if the value was written; otherwise, <c>false</c>.</returns>
    public static bool TryWriteWithSerializer<T>
    (
        this Utf8JsonWriter writer,
        T? nullableValue,
        JsonSerializerOptions options,
        EqualityComparer<T>? equalityComparer = null
    )
        where T : struct
    {
        return writer.TryWrite
        (
            nullableValue,
            options,
            (nv) =>
            {
                JsonSerializer.Serialize(writer, nv, options);
            },
            equalityComparer
        );
    }

    /// <summary>
    ///     Attempts to serialize a reference type using <see cref="JsonSerializer"/> when the
    ///     serializer options permit it.
    /// </summary>
    /// <typeparam name="T">The type of the value to serialize.</typeparam>
    /// <param name="writer">The writer to which the value will be serialized.</param>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <returns><c>true</c> if the value was written; otherwise, <c>false</c>.</returns>
    public static bool TryWriteWithSerializer<T>
    (
        this Utf8JsonWriter writer,
        T? obj,
        JsonSerializerOptions options
    )
        where T : class
    {
        return writer.TryWrite
        (
            obj,
            options,
            (o) =>
            {
                JsonSerializer.Serialize(writer, o, options);
            }
        );
    }
    #endregion

    #region TryWritePropertyWithSerializer Extension Methods
    /// <summary>
    ///     Attempts to serialize a value as a property using <see cref="JsonSerializer"/>
    ///     when the serializer options permit it.
    /// </summary>
    /// <typeparam name="T">The type of the value to serialize.</typeparam>
    /// <param name="writer">The writer to which the value will be serialized.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="equalityComparer">
    ///     Optional comparer used to detect default values.
    /// </param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyWithSerializer<T>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        T value,
        JsonSerializerOptions options,
        EqualityComparer<T>? equalityComparer = null
    )
        where T : struct
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (n, v) =>
            {
                writer.WritePropertyName(n);
                JsonSerializer.Serialize(writer, v, options);
            },
            equalityComparer
        );
    }

    /// <summary>
    ///     Attempts to serialize a nullable value as a property using
    ///     <see cref="JsonSerializer"/> when the serializer options permit it.
    /// </summary>
    /// <typeparam name="T">The type of the value to serialize.</typeparam>
    /// <param name="writer">The writer to which the value will be serialized.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="nullableValue">The value to serialize, if present.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="equalityComparer">
    ///     Optional comparer used to detect default values.
    /// </param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyWithSerializer<T>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        T? nullableValue,
        JsonSerializerOptions options,
        EqualityComparer<T>? equalityComparer = null
    )
        where T : struct
    {
        return writer.TryWriteProperty
        (
            propertyName,
            nullableValue,
            options,
            (n, nv) =>
            {
                writer.WritePropertyName(n);
                JsonSerializer.Serialize(writer, nv, options);
            },
            equalityComparer
        );
    }

    /// <summary>
    ///     Attempts to serialize a reference type as a property using
    ///     <see cref="JsonSerializer"/> when the serializer options permit it.
    /// </summary>
    /// <typeparam name="T">The type of the value to serialize.</typeparam>
    /// <param name="writer">The writer to which the value will be serialized.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyWithSerializer<T>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        T? obj,
        JsonSerializerOptions options
    )
        where T : class
    {
        return writer.TryWriteProperty
        (
            propertyName,
            obj,
            options,
            (n, o) =>
            {
                writer.WritePropertyName(n);
                JsonSerializer.Serialize(writer, o, options);
            }
        );
    }
    #endregion
}
