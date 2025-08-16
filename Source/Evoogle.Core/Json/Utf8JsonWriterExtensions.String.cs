// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Globalization;
using System.Text.Json;

namespace Evoogle.Json;

/// <inheritdoc cref="Utf8JsonWriterExtensions"/>
public static partial class Utf8JsonWriterExtensions
{
    #region TryWritePropertyAsString Extension Methods
    public static bool TryWritePropertyAsString(this Utf8JsonWriter writer, string propertyName, DateTime value, JsonSerializerOptions options, EqualityComparer<DateTime>? equalityComparer = null, string format = "O", IFormatProvider? formatProvider = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (n, dt) => writer.WriteString(n, dt.ToString(format, formatProvider ?? CultureInfo.InvariantCulture)),
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsString(this Utf8JsonWriter writer, string propertyName, DateTime? value, JsonSerializerOptions options, EqualityComparer<DateTime>? equalityComparer = null, string format = "O", IFormatProvider? formatProvider = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (n, dt) => writer.WriteString(n, dt.ToString(format, formatProvider ?? CultureInfo.InvariantCulture)),
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsString(this Utf8JsonWriter writer, string propertyName, DateTimeOffset value, JsonSerializerOptions options, EqualityComparer<DateTimeOffset>? equalityComparer = null, string format = "O", IFormatProvider? formatProvider = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (n, dto) => writer.WriteString(n, dto.ToString(format, formatProvider ?? CultureInfo.InvariantCulture)),
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsString(this Utf8JsonWriter writer, string propertyName, DateTimeOffset? value, JsonSerializerOptions options, EqualityComparer<DateTimeOffset>? equalityComparer = null, string format = "O", IFormatProvider? formatProvider = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (n, dto) => writer.WriteString(n, dto.ToString(format, formatProvider ?? CultureInfo.InvariantCulture)),
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsString<TEnum>(this Utf8JsonWriter writer, string propertyName, TEnum value, JsonSerializerOptions options, EqualityComparer<TEnum>? equalityComparer = null)
        where TEnum : struct, Enum
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (n, enumeration) => writer.WriteString(n, enumeration.ToString()),
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsString<TEnum>(this Utf8JsonWriter writer, string propertyName, TEnum? value, JsonSerializerOptions options, EqualityComparer<TEnum>? equalityComparer = null)
        where TEnum : struct, Enum
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (n, enumeration) => writer.WriteString(n, enumeration.ToString()),
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsString(this Utf8JsonWriter writer, string propertyName, Guid value, JsonSerializerOptions options, EqualityComparer<Guid>? equalityComparer = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            writer.WriteString,
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsString(this Utf8JsonWriter writer, string propertyName, Guid? value, JsonSerializerOptions options, EqualityComparer<Guid>? equalityComparer = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            writer.WriteString,
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsString(this Utf8JsonWriter writer, string propertyName, string? value, JsonSerializerOptions options)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            writer.WriteString
        );
    }

    public static bool TryWritePropertyAsString(this Utf8JsonWriter writer, string propertyName, TimeSpan value, JsonSerializerOptions options, EqualityComparer<TimeSpan>? equalityComparer = null, string format = "c", IFormatProvider? formatProvider = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (n, ts) => writer.WriteString(n, ts.ToString(format, formatProvider ?? CultureInfo.InvariantCulture)),
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsString(this Utf8JsonWriter writer, string propertyName, TimeSpan? value, JsonSerializerOptions options, EqualityComparer<TimeSpan>? equalityComparer = null, string format = "c", IFormatProvider? formatProvider = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (n, ts) => writer.WriteString(n, ts.ToString(format, formatProvider ?? CultureInfo.InvariantCulture)),
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsString(this Utf8JsonWriter writer, string propertyName, Ulid value, JsonSerializerOptions options, EqualityComparer<Ulid>? equalityComparer = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (n, ulid) =>
            {
                writer.WritePropertyName(n);
                JsonSerializer.Serialize(writer, ulid, options);
            },
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsString(this Utf8JsonWriter writer, string propertyName, Ulid? value, JsonSerializerOptions options, EqualityComparer<Ulid>? equalityComparer = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (n, ulid) =>
            {
                writer.WritePropertyName(n);
                JsonSerializer.Serialize(writer, ulid, options);
            },
            equalityComparer
        );
    }
    #endregion
}
