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
    /// <summary>
    ///     Writes a <see cref="DateTime"/> value as a JSON string property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The <see cref="DateTime"/> value to serialize.</param>
    /// <param name="options">The serializer options controlling ignore behavior.</param>
    /// <param name="equalityComparer">Optional comparer used to determine default values.</param>
    /// <param name="format">The format string (default is ISO 8601 format \"O\").</param>
    /// <param name="formatProvider">Optional provider for culture-specific formatting.</param>
    public static void WriteConditionalPropertyAsString(this Utf8JsonWriter writer, string propertyName, DateTime value, JsonSerializerOptions options, EqualityComparer<DateTime>? equalityComparer = null, string format = "O", IFormatProvider? formatProvider = null)
    {
        writer.WriteConditionalProperty
        (
            propertyName,
            value,
            options,
            (name, dt) => writer.WriteString(name, dt.ToString(format, formatProvider ?? CultureInfo.InvariantCulture)),
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a nullable <see cref="DateTime"/> value as a JSON string property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The nullable <see cref="DateTime"/> value to serialize.</param>
    /// <param name="options">The serializer options controlling ignore behavior.</param>
    /// <param name="equalityComparer">Optional comparer used to determine default values.</param>
    /// <param name="format">The format string (default is \"O\").</param>
    /// <param name="formatProvider">Optional provider for formatting (default is invariant).</param>
    public static void WriteConditionalPropertyAsString(this Utf8JsonWriter writer, string propertyName, DateTime? value, JsonSerializerOptions options, EqualityComparer<DateTime>? equalityComparer = null, string format = "O", IFormatProvider? formatProvider = null)
    {
        writer.WriteConditionalNullableProperty
        (
            propertyName,
            value,
            options,
            (name, dt) => writer.WriteString(name, dt.ToString(format, formatProvider ?? CultureInfo.InvariantCulture)),
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a <see cref="DateTimeOffset"/> value as a JSON string property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The <see cref="DateTimeOffset"/> value to serialize.</param>
    /// <param name="options">The serializer options controlling ignore behavior.</param>
    /// <param name="equalityComparer">Optional comparer used to determine default values.</param>
    /// <param name="format">The format string (default is ISO 8601 format \"O\").</param>
    /// <param name="formatProvider">Optional provider for culture-specific formatting.</param>
    public static void WriteConditionalPropertyAsString(this Utf8JsonWriter writer, string propertyName, DateTimeOffset value, JsonSerializerOptions options, EqualityComparer<DateTimeOffset>? equalityComparer = null, string format = "O", IFormatProvider? formatProvider = null)
    {
        writer.WriteConditionalProperty
        (
            propertyName,
            value,
            options,
            (name, dto) => writer.WriteString(name, dto.ToString(format, formatProvider ?? CultureInfo.InvariantCulture)),
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a nullable <see cref="DateTimeOffset"/> value as a JSON string property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The nullable <see cref="DateTimeOffset"/> value to serialize.</param>
    /// <param name="options">The serializer options controlling ignore behavior.</param>
    /// <param name="equalityComparer">Optional comparer used to determine default values.</param>
    /// <param name="format">The format string (default is \"O\").</param>
    /// <param name="formatProvider">Optional provider for formatting (default is invariant).</param>
    public static void WriteConditionalPropertyAsString(this Utf8JsonWriter writer, string propertyName, DateTimeOffset? value, JsonSerializerOptions options, EqualityComparer<DateTimeOffset>? equalityComparer = null, string format = "O", IFormatProvider? formatProvider = null)
    {
        writer.WriteConditionalNullableProperty
        (
            propertyName,
            value,
            options,
            (name, dto) => writer.WriteString(name, dto.ToString(format, formatProvider ?? CultureInfo.InvariantCulture)),
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes an enum value as a string property using <c>ToString()</c> if permitted by <paramref name="options"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The enum value to write.</param>
    /// <param name="options">The serializer options controlling ignore behavior.</param>
    /// <param name="equalityComparer">Optional comparer used to detect default values.</param>
    public static void WriteConditionalPropertyAsString<TEnum>(this Utf8JsonWriter writer, string propertyName, TEnum value, JsonSerializerOptions options, EqualityComparer<TEnum>? equalityComparer = null)
        where TEnum : struct, Enum
    {
        writer.WriteConditionalProperty
        (
            propertyName,
            value,
            options,
            (name, enumeration) => writer.WriteString(name, enumeration.ToString()),
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a nullable enum value as a string property using <c>ToString()</c> if permitted by <paramref name="options"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The nullable enum value to serialize.</param>
    /// <param name="options">The serializer options controlling ignore behavior.</param>
    /// <param name="equalityComparer">Optional comparer used to determine default values.</param>
    public static void WriteConditionalPropertyAsString<TEnum>(this Utf8JsonWriter writer, string propertyName, TEnum? value, JsonSerializerOptions options, EqualityComparer<TEnum>? equalityComparer = null)
        where TEnum : struct, Enum
    {
        writer.WriteConditionalNullableProperty
        (
            propertyName,
            value,
            options,
            (name, enumeration) => writer.WriteString(name, enumeration.ToString()),
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a <see cref="Guid"/> value as a JSON string property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The <see cref="Guid"/> to write.</param>
    /// <param name="options">The serializer options used to control null/default ignore behavior.</param>
    /// <param name="equalityComparer">Optional comparer to detect default values.</param>
    public static void WriteConditionalPropertyAsString(this Utf8JsonWriter writer, string propertyName, Guid value, JsonSerializerOptions options, EqualityComparer<Guid>? equalityComparer = null)
    {
        writer.WriteConditionalProperty
        (
            propertyName,
            value,
            options,
            writer.WriteString,
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a nullable <see cref="Guid"/> value as a JSON string property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The nullable <see cref="Guid"/> value to serialize.</param>
    /// <param name="options">The serializer options controlling ignore behavior.</param>
    /// <param name="equalityComparer">Optional comparer used to determine default values.</param>
    public static void WriteConditionalPropertyAsString(this Utf8JsonWriter writer, string propertyName, Guid? value, JsonSerializerOptions options, EqualityComparer<Guid>? equalityComparer = null)
    {
        writer.WriteConditionalNullableProperty
        (
            propertyName,
            value,
            options,
            writer.WriteString,
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a <see cref="string"/> value as a JSON property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The string value to write (may be null).</param>
    /// <param name="options">The serializer options controlling whether nulls should be written.</param>
    public static void WriteConditionalPropertyAsString(this Utf8JsonWriter writer, string propertyName, string? value, JsonSerializerOptions options)
    {
        writer.WriteConditionalReferenceProperty
        (
            propertyName,
            value,
            options,
            writer.WriteString
        );
    }

    /// <summary>
    ///     Writes a <see cref="TimeSpan"/> value as a JSON string property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The <see cref="TimeSpan"/> value to serialize.</param>
    /// <param name="options">The serializer options controlling ignore behavior.</param>
    /// <param name="equalityComparer">Optional comparer used to determine default values.</param>
    /// <param name="format">The format string (default is \"c\").</param>
    /// <param name="formatProvider">Optional provider for culture-specific formatting.</param>
    public static void WriteConditionalPropertyAsString(this Utf8JsonWriter writer, string propertyName, TimeSpan value, JsonSerializerOptions options, EqualityComparer<TimeSpan>? equalityComparer = null, string format = "c", IFormatProvider? formatProvider = null)
    {
        writer.WriteConditionalProperty
        (
            propertyName,
            value,
            options,
            (name, ts) => writer.WriteString(name, ts.ToString(format, formatProvider ?? CultureInfo.InvariantCulture)),
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a nullable <see cref="TimeSpan"/> value as a JSON string property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The nullable <see cref="TimeSpan"/> value to serialize.</param>
    /// <param name="options">The serializer options controlling ignore behavior.</param>
    /// <param name="equalityComparer">Optional comparer used to determine default values.</param>
    /// <param name="format">The format string (default is \"c\").</param>
    /// <param name="formatProvider">Optional provider for formatting (default is invariant).</param>
    public static void WriteConditionalPropertyAsString(this Utf8JsonWriter writer, string propertyName, TimeSpan? value, JsonSerializerOptions options, EqualityComparer<TimeSpan>? equalityComparer = null, string format = "c", IFormatProvider? formatProvider = null)
    {
        writer.WriteConditionalNullableProperty
        (
            propertyName,
            value,
            options,
            (name, ts) => writer.WriteString(name, ts.ToString(format, formatProvider ?? CultureInfo.InvariantCulture)),
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes an <see cref="Ulid"/> value as a JSON string property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The <see cref="Ulid"/> to serialize.</param>
    /// <param name="options">The serializer options controlling ignore behavior.</param>
    /// <param name="equalityComparer">Optional comparer used to determine default values.</param>
    public static void WriteConditionalPropertyAsString(this Utf8JsonWriter writer, string propertyName, Ulid value, JsonSerializerOptions options, EqualityComparer<Ulid>? equalityComparer = null)
    {
        writer.WriteConditionalProperty
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

    /// <summary>
    ///     Writes a nullable <see cref="Ulid"/> value as a JSON string property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The <see cref="Ulid"/> to serialize.</param>
    /// <param name="options">The serializer options controlling ignore behavior.</param>
    /// <param name="equalityComparer">Optional comparer used to determine default values.</param>
    public static void WriteConditionalPropertyAsString(this Utf8JsonWriter writer, string propertyName, Ulid? value, JsonSerializerOptions options, EqualityComparer<Ulid>? equalityComparer = null)
    {
        writer.WriteConditionalNullableProperty
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
