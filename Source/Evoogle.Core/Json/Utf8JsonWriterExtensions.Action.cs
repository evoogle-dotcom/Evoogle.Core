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
