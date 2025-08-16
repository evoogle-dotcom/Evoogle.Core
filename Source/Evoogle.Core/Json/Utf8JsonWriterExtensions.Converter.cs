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
    #region TryWriteWithConverter Extension Methods
    public static bool TryWriteWithConverter<TEnum>
    (
        this Utf8JsonWriter writer,
        TEnum value,
        JsonSerializerOptions options,
        EnumJsonConverter<TEnum> converter,
        EqualityComparer<TEnum>? equalityComparer = null
    )
        where TEnum : struct, Enum
    {
        return writer.TryWrite
        (
            value,
            options,
            (v) =>
            {
                converter.Write(writer, v, options);
            },
            equalityComparer
        );
    }

    public static bool TryWriteWithConverter<TEnum>
    (
        this Utf8JsonWriter writer,
        TEnum? value,
        JsonSerializerOptions options,
        EnumJsonConverter<TEnum> converter,
        EqualityComparer<TEnum>? equalityComparer = null
    )
        where TEnum : struct, Enum
    {
        return writer.TryWrite
        (
            value,
            options,
            (v) =>
            {
                converter.Write(writer, v, options);
            },
            equalityComparer
        );
    }

    public static bool TryWriteWithConverter
    (
        this Utf8JsonWriter writer,
        Type? type,
        JsonSerializerOptions options,
        TypeJsonConverter converter
    )
    {
        return writer.TryWrite
        (
            type,
            options,
            (t) =>
            {
                converter.Write(writer, t, options);
            }
        );
    }
    #endregion

    #region TryWritePropertyWithConverter Extension Methods
    public static bool TryWritePropertyWithConverter<TEnum>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        TEnum value,
        JsonSerializerOptions options,
        EnumJsonConverter<TEnum> converter,
        EqualityComparer<TEnum>? equalityComparer = null
    )
        where TEnum : struct, Enum
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (n, v) =>
            {
                writer.WritePropertyName(n);
                converter.Write(writer, v, options);
            },
            equalityComparer
        );
    }

    public static bool TryWritePropertyWithConverter<TEnum>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        TEnum? value,
        JsonSerializerOptions options,
        EnumJsonConverter<TEnum> converter,
        EqualityComparer<TEnum>? equalityComparer = null
    )
        where TEnum : struct, Enum
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (n, v) =>
            {
                writer.WritePropertyName(n);
                converter.Write(writer, v, options);
            },
            equalityComparer
        );
    }

    public static bool TryWritePropertyWithConverter
    (
        this Utf8JsonWriter writer,
        string propertyName,
        Type? type,
        JsonSerializerOptions options,
        TypeJsonConverter converter
    )
    {
        return writer.TryWriteProperty
        (
            propertyName,
            type,
            options,
            (n, t) =>
            {
                writer.WritePropertyName(n);
                converter.Write(writer, t, options);
            }
        );
    }
    #endregion
}
