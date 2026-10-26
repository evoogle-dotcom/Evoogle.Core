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
    /// <summary>
    ///     Attempts to write a <see cref="DateTime"/> property when the serializer options
    ///     permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The date and time value to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="format">The format string to use.</param>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <param name="equalityComparer">Optional comparer used to detect default values.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyAsString
    (
        this Utf8JsonWriter writer,
        string propertyName,
        DateTime value,
        JsonSerializerOptions options,
        string format = "O",
        IFormatProvider? formatProvider = null,
        DateTime defaultValue = default,
        EqualityComparer<DateTime>? equalityComparer = null
    )
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (format, formatProvider),
            static (writer, value, state) => writer.WriteStringValue(value.ToString(state.format, state.formatProvider ?? CultureInfo.InvariantCulture)),
            defaultValue,
            equalityComparer
        );
    }

    /// <summary>
    ///     Attempts to write a nullable <see cref="DateTime"/> property when the serializer
    ///     options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The date and time value to write, if present.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="format">The format string to use.</param>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <param name="equalityComparer">Optional comparer used to detect default values.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyAsString
    (
        this Utf8JsonWriter writer,
        string propertyName,
        DateTime? value,
        JsonSerializerOptions options,
        string format = "O",
        IFormatProvider? formatProvider = null,
        DateTime? defaultValue = default,
        EqualityComparer<DateTime?>? equalityComparer = null
    )
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (format, formatProvider),
            static (writer, value, state) => writer.WriteStringValue(value.ToString(state.format, state.formatProvider ?? CultureInfo.InvariantCulture)),
            defaultValue,
            equalityComparer
        );
    }

    /// <summary>
    ///     Attempts to write a <see cref="DateTimeOffset"/> property when the serializer options
    ///     permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The date and time offset value to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="format">The format string to use.</param>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <param name="equalityComparer">Optional comparer used to detect default values.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyAsString
    (
        this Utf8JsonWriter writer,
        string propertyName,
        DateTimeOffset value,
        JsonSerializerOptions options,
        string format = "O",
        IFormatProvider? formatProvider = null,
        DateTimeOffset defaultValue = default,
        EqualityComparer<DateTimeOffset>? equalityComparer = null
    )
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (format, formatProvider),
            static (writer, value, state) => writer.WriteStringValue(value.ToString(state.format, state.formatProvider ?? CultureInfo.InvariantCulture)),
            defaultValue,
            equalityComparer
        );
    }

    /// <summary>
    ///     Attempts to write a nullable <see cref="DateTimeOffset"/> property when the serializer
    ///     options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The date and time offset value to write, if present.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="format">The format string to use.</param>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <param name="equalityComparer">Optional comparer used to detect default values.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyAsString
    (
        this Utf8JsonWriter writer,
        string propertyName,
        DateTimeOffset? value,
        JsonSerializerOptions options,
        string format = "O",
        IFormatProvider? formatProvider = null,
        DateTimeOffset? defaultValue = default,
        EqualityComparer<DateTimeOffset?>? equalityComparer = null
    )
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (format, formatProvider),
            static (writer, value, state) => writer.WriteStringValue(value.ToString(state.format, state.formatProvider ?? CultureInfo.InvariantCulture)),
            defaultValue,
            equalityComparer
        );
    }

    /// <summary>
    ///     Attempts to write an enum property as a string when the serializer options permit it.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The enum value to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyAsString<TEnum>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        TEnum value,
        JsonSerializerOptions options,
        TEnum defaultValue = default
    )
        where TEnum : struct, Enum
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            static (writer, value) => writer.WriteStringValue(value.ToString()),
            defaultValue: defaultValue
        );
    }

    /// <summary>
    ///     Attempts to write a nullable enum property as a string when the serializer options
    ///     permit it.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The enum value to write, if present.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyAsString<TEnum>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        TEnum? value,
        JsonSerializerOptions options,
        TEnum? defaultValue = default
    )
        where TEnum : struct, Enum
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            static (writer, value) => writer.WriteStringValue(value.ToString()),
            defaultValue: defaultValue
        );
    }

    /// <summary>
    ///     Attempts to write a <see cref="Guid"/> property when the serializer options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The <see cref="Guid"/> value to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyAsString
    (
        this Utf8JsonWriter writer,
        string propertyName,
        Guid value,
        JsonSerializerOptions options,
        Guid defaultValue = default
    )
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            static (writer, value) => writer.WriteStringValue(value),
            defaultValue: defaultValue
        );
    }

    /// <summary>
    ///     Attempts to write a nullable <see cref="Guid"/> property when the serializer options
    ///     permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The <see cref="Guid"/> value to write, if present.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyAsString
    (
        this Utf8JsonWriter writer,
        string propertyName,
        Guid? value,
        JsonSerializerOptions options,
        Guid? defaultValue = default
    )
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            static (writer, value) => writer.WriteStringValue(value),
            defaultValue: defaultValue
        );
    }

    /// <summary>
    ///     Attempts to write a string property when the serializer options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The string value to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <param name="equalityComparer">The comparer used to compare non-null values with the default.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyAsString
    (
        this Utf8JsonWriter writer,
        string propertyName,
        string? value,
        JsonSerializerOptions options,
        string? defaultValue = default,
        EqualityComparer<string>? equalityComparer = null
    )
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            static (writer, value) => writer.WriteStringValue(value),
            defaultValue,
            equalityComparer
        );
    }

    /// <summary>
    ///     Attempts to write a <see cref="TimeSpan"/> property when the serializer options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The time span value to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="format">The format string to use.</param>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <param name="equalityComparer">Optional comparer used to detect default values.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyAsString
    (
        this Utf8JsonWriter writer,
        string propertyName,
        TimeSpan value,
        JsonSerializerOptions options,
        string format = "c",
        IFormatProvider? formatProvider = null,
        TimeSpan defaultValue = default,
        EqualityComparer<TimeSpan>? equalityComparer = null
    )
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (format, formatProvider),
            static (writer, value, state) => writer.WriteStringValue(value.ToString(state.format, state.formatProvider ?? CultureInfo.InvariantCulture)),
            defaultValue,
            equalityComparer
        );
    }

    /// <summary>
    ///     Attempts to write a nullable <see cref="TimeSpan"/> property when the serializer
    ///     options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The time span value to write, if present.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="format">The format string to use.</param>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <param name="equalityComparer">Optional comparer used to detect default values.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyAsString
    (
        this Utf8JsonWriter writer,
        string propertyName,
        TimeSpan? value,
        JsonSerializerOptions options,
        string format = "c",
        IFormatProvider? formatProvider = null,
        TimeSpan? defaultValue = default,
        EqualityComparer<TimeSpan?>? equalityComparer = null
    )
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (format, formatProvider),
            static (writer, value, state) => writer.WriteStringValue(value.ToString(state.format, state.formatProvider ?? CultureInfo.InvariantCulture)),
            defaultValue,
            equalityComparer
        );
    }

    /// <summary>
    ///     Attempts to write a <see cref="Ulid"/> property as a JSON string when the serializer
    ///     options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The <see cref="Ulid"/> value to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyAsString
    (
        this Utf8JsonWriter writer,
        string propertyName,
        Ulid value,
        JsonSerializerOptions options,
        Ulid defaultValue = default
    )
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            options,
            static (writer, value, options) => JsonSerializer.Serialize(writer, value, options),
            defaultValue: defaultValue
        );
    }

    /// <summary>
    ///     Attempts to write a nullable <see cref="Ulid"/> property as a JSON string when the
    ///     serializer options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The <see cref="Ulid"/> value to write, if present.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyAsString
    (
        this Utf8JsonWriter writer,
        string propertyName,
        Ulid? value,
        JsonSerializerOptions options,
        Ulid? defaultValue = default
    )
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            options,
            static (writer, value, options) => JsonSerializer.Serialize(writer, value, options),
            defaultValue: defaultValue
        );
    }
    #endregion
}
