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
