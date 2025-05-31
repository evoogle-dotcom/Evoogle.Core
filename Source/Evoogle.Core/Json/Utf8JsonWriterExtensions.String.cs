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
    #region String Extension Methods
    /// <summary>Conditionally writes a <c>DateTime</c> property as a string.</summary>
    public static void WritePropertyString(this Utf8JsonWriter writer, string propertyName, DateTime value, JsonSerializerOptions options, EqualityComparer<DateTime>? equalityComparer = null)
    {
        writer.WritePropertyValueType
        (
            propertyName,
            value,
            options,
            writer.WriteString,
            equalityComparer
        );
    }

    /// <summary>Conditionally writes a <c>DateTime?</c> property as a string.</summary>
    public static void WritePropertyString(this Utf8JsonWriter writer, string propertyName, DateTime? value, JsonSerializerOptions options, EqualityComparer<DateTime>? equalityComparer = null)
    {
        writer.WritePropertyNullableValueType
        (
            propertyName,
            value,
            options,
            writer.WriteString,
            equalityComparer
        );
    }

    /// <summary>Conditionally writes a <c>DateTimeOffset</c> property as a string.</summary>
    public static void WritePropertyString(this Utf8JsonWriter writer, string propertyName, DateTimeOffset value, JsonSerializerOptions options, EqualityComparer<DateTimeOffset>? equalityComparer = null)
    {
        writer.WritePropertyValueType
        (
            propertyName,
            value,
            options,
            writer.WriteString,
            equalityComparer
        );
    }

    /// <summary>Conditionally writes a <c>DateTimeOffset?</c> property as a string.</summary>
    public static void WritePropertyString(this Utf8JsonWriter writer, string propertyName, DateTimeOffset? value, JsonSerializerOptions options, EqualityComparer<DateTimeOffset>? equalityComparer = null)
    {
        writer.WritePropertyNullableValueType
        (
            propertyName,
            value,
            options,
            writer.WriteString,
            equalityComparer
        );
    }

    /// <summary>Conditionally writes an <c>enum</c> property as a string.</summary>
    public static void WritePropertyString<TEnum>(this Utf8JsonWriter writer, string propertyName, TEnum value, JsonSerializerOptions options, EqualityComparer<TEnum>? equalityComparer = null)
        where TEnum : struct, Enum
    {
        writer.WritePropertyValueType
        (
            propertyName,
            value,
            options,
            (name, enumeration) => writer.WriteString(name, enumeration.ToString()),
            equalityComparer
        );
    }

    /// <summary>Conditionally writes an <c>enum?</c> property as a string.</summary>
    public static void WritePropertyString<TEnum>(this Utf8JsonWriter writer, string propertyName, TEnum? value, JsonSerializerOptions options, EqualityComparer<TEnum>? equalityComparer = null)
        where TEnum : struct, Enum
    {
        writer.WritePropertyNullableValueType
        (
            propertyName,
            value,
            options,
            (name, enumeration) => writer.WriteString(name, enumeration.ToString()),
            equalityComparer
        );
    }

    /// <summary>Conditionally writes a <c>Guid</c> property as a string.</summary>
    public static void WritePropertyString(this Utf8JsonWriter writer, string propertyName, Guid value, JsonSerializerOptions options, EqualityComparer<Guid>? equalityComparer = null)
    {
        writer.WritePropertyValueType
        (
            propertyName,
            value,
            options,
            writer.WriteString,
            equalityComparer
        );
    }

    /// <summary>Conditionally writes a <c>Guid?</c> property as a string.</summary>
    public static void WritePropertyString(this Utf8JsonWriter writer, string propertyName, Guid? value, JsonSerializerOptions options, EqualityComparer<Guid>? equalityComparer = null)
    {
        writer.WritePropertyNullableValueType
        (
            propertyName,
            value,
            options,
            writer.WriteString,
            equalityComparer
        );
    }

    /// <summary>Conditionally writes a <c>string</c> property.</summary>
    public static void WritePropertyString(this Utf8JsonWriter writer, string propertyName, string? value, JsonSerializerOptions options)
    {
        writer.WritePropertyReferenceType
        (
            propertyName,
            value,
            options,
            writer.WriteString
        );
    }

    /// <summary>Conditionally writes a <c>TimeSpan</c> property as a string.</summary>
    public static void WritePropertyString(this Utf8JsonWriter writer, string propertyName, TimeSpan value, JsonSerializerOptions options, EqualityComparer<TimeSpan>? equalityComparer = null)
    {
        writer.WritePropertyValueType
        (
            propertyName,
            value,
            options,
            (name, timeSpan) => writer.WriteString(name, timeSpan.ToString("c", CultureInfo.InvariantCulture)),
            equalityComparer
        );
    }

    /// <summary>Conditionally writes a <c>TimeSpan?</c> property as a string.</summary>
    public static void WritePropertyString(this Utf8JsonWriter writer, string propertyName, TimeSpan? value, JsonSerializerOptions options, EqualityComparer<TimeSpan>? equalityComparer = null)
    {
        writer.WritePropertyNullableValueType
        (
            propertyName,
            value,
            options,
            (name, timeSpan) => writer.WriteString(name, timeSpan.ToString("c", CultureInfo.InvariantCulture)),
            equalityComparer
        );
    }

    /// <summary>Conditionally writes a <c>Ulid</c> property as a string (via UlidJsonConverter).</summary>
    public static void WritePropertyString(this Utf8JsonWriter writer, string propertyName, Ulid value, JsonSerializerOptions options, EqualityComparer<Ulid>? equalityComparer = null)
    {
        writer.WritePropertyValueType
        (
            propertyName,
            value,
            options,
            (name, ulid) =>
            {
                writer.WritePropertyName(name);
                JsonSerializer.Serialize(writer, ulid, options);
            },
            equalityComparer
        );
    }

    /// <summary>Conditionally writes a <c>Ulid?</c> property as a string (via UlidJsonConverter).</summary>
    public static void WritePropertyString(this Utf8JsonWriter writer, string propertyName, Ulid? value, JsonSerializerOptions options, EqualityComparer<Ulid>? equalityComparer = null)
    {
        writer.WritePropertyNullableValueType
        (
            propertyName,
            value,
            options,
            (name, ulid) =>
            {
                writer.WritePropertyName(name);
                JsonSerializer.Serialize(writer, ulid, options);
            },
            equalityComparer
        );
    }
    #endregion
}