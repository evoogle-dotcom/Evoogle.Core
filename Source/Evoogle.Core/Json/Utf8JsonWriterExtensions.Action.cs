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
    ///     permit it.
    /// </summary>
    /// <typeparam name="T">The type of the value to write.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="writeAction">The action that writes the value.</param>
    /// <param name="equalityComparer">
    ///     Optional comparer used to detect default values.
    /// </param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyWithAction<T>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        T value,
        JsonSerializerOptions options,
        Action<T> writeAction,
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
                writeAction(v);
            },
            equalityComparer
        );
    }

    /// <summary>
    ///     Attempts to write a nullable property using a custom action when the serializer
    ///     options permit it.
    /// </summary>
    /// <typeparam name="T">The type of the value to write.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="nullableValue">The value to write, if present.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="writeAction">The action that writes the value.</param>
    /// <param name="equalityComparer">
    ///     Optional comparer used to detect default values.
    /// </param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyWithAction<T>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        T? nullableValue,
        JsonSerializerOptions options,
        Action<T> writeAction,
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
                writeAction(nv);
            },
            equalityComparer
        );
    }

    /// <summary>
    ///     Attempts to write a reference type property using a custom action when the serializer
    ///     options permit it.
    /// </summary>
    /// <typeparam name="T">The type of the value to write.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="obj">The object to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="writeAction">The action that writes the value.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyWithAction<T>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        T? obj,
        JsonSerializerOptions options,
        Action<T> writeAction
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
                writeAction(o);
            }
        );
    }
    #endregion
}
