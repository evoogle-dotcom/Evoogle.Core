// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Globalization;
using System.Text.Json;

using Cysharp.Serialization.Json;

namespace Evoogle.Json;

/// <inheritdoc cref="Utf8JsonWriterExtensions"/>
public static partial class Utf8JsonWriterExtensions
{
    #region WritePropertyWithConverter Extension Methods
    /// <summary>
    ///     Writes an enum property using the specified converter, including default enum values.
    /// </summary>
    /// <remarks>
    ///     Always writes the property regardless of <see cref="JsonSerializerOptions.DefaultIgnoreCondition"/>.
    ///     Use this method for required enum identities and structural discriminators.
    /// </remarks>
    /// <typeparam name="TEnum">The enum type to write.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The enum value to write.</param>
    /// <param name="options">The serializer options passed to the converter.</param>
    /// <param name="converter">The converter used to write the value.</param>
    public static void WritePropertyWithConverter<TEnum>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        TEnum value,
        JsonSerializerOptions options,
        EnumJsonConverter<TEnum> converter
    )
        where TEnum : struct, Enum
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(propertyName);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(converter);

        writer.WritePropertyName(propertyName);
        converter.Write(writer, value, options);
    }
    #endregion

    #region TryWriteWithConverter Extension Methods
    /// <summary>
    ///     Attempts to write a <see cref="CultureInfo"/> value using the specified converter when the
    ///     serializer options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="cultureInfo">The culture info to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="converter">The converter used to write the value.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <param name="equalityComparer">The comparer used to compare non-null values with the default.</param>
    /// <returns><c>true</c> if the value was written; otherwise, <c>false</c>.</returns>
    public static bool TryWriteWithConverter
    (
        this Utf8JsonWriter writer,
        CultureInfo? cultureInfo,
        JsonSerializerOptions options,
        CultureInfoJsonConverter converter,
        CultureInfo? defaultValue = default,
        EqualityComparer<CultureInfo>? equalityComparer = null
    )
    {
        return writer.TryWrite
        (
            cultureInfo,
            options,
            (converter, options),
            static (writer, value, state) => state.converter.Write(writer, value, state.options),
            defaultValue,
            equalityComparer
        );
    }

    /// <summary>
    ///     Attempts to write an enum value using the specified converter when the serializer
    ///     options permit it.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to write.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="value">The enum value to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="converter">The converter used to write the value.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <returns><c>true</c> if the value was written; otherwise, <c>false</c>.</returns>
    public static bool TryWriteWithConverter<TEnum>
    (
        this Utf8JsonWriter writer,
        TEnum value,
        JsonSerializerOptions options,
        EnumJsonConverter<TEnum> converter,
        TEnum defaultValue = default
    )
        where TEnum : struct, Enum
    {
        return writer.TryWrite
        (
            value,
            options,
            (converter, options),
            static (writer, value, state) => state.converter.Write(writer, value, state.options),
            defaultValue
        );
    }

    /// <summary>
    ///     Attempts to write a nullable enum value using the specified converter when the
    ///     serializer options permit it.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to write.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="value">The enum value to write, if present.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="converter">The converter used to write the value.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <returns><c>true</c> if the value was written; otherwise, <c>false</c>.</returns>
    public static bool TryWriteWithConverter<TEnum>
    (
        this Utf8JsonWriter writer,
        TEnum? value,
        JsonSerializerOptions options,
        EnumJsonConverter<TEnum> converter,
        TEnum defaultValue = default
    )
        where TEnum : struct, Enum
    {
        return writer.TryWrite
        (
            value,
            options,
            (converter, options),
            static (writer, value, state) => state.converter.Write(writer, value, state.options),
            defaultValue
        );
    }

    /// <summary>
    ///     Attempts to write a <see cref="Type"/> value using the specified converter when the
    ///     serializer options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="type">The type to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="converter">The converter used to write the value.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <returns><c>true</c> if the value was written; otherwise, <c>false</c>.</returns>
    public static bool TryWriteWithConverter
    (
        this Utf8JsonWriter writer,
        Type? type,
        JsonSerializerOptions options,
        TypeJsonConverter converter,
        Type? defaultValue = default
    )
    {
        return writer.TryWrite
        (
            type,
            options,
            (converter, options),
            static (writer, value, state) => state.converter.Write(writer, value, state.options),
            defaultValue
        );
    }

    /// <summary>
    ///     Attempts to write an Ulid value using the specified converter when the serializer
    ///     options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="value">The Ulid value to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="converter">The converter used to write the value.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <returns><c>true</c> if the value was written; otherwise, <c>false</c>.</returns>
    public static bool TryWriteWithConverter
    (
        this Utf8JsonWriter writer,
        Ulid value,
        JsonSerializerOptions options,
        UlidJsonConverter converter,
        Ulid defaultValue = default
    )
    {
        return writer.TryWrite
        (
            value,
            options,
            (converter, options),
            static (writer, value, state) => state.converter.Write(writer, value, state.options),
            defaultValue
        );
    }

    /// <summary>
    ///     Attempts to write a nullable Ulid value using the specified converter when the
    ///     serializer options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="value">The Ulid value to write, if present.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="converter">The converter used to write the value.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <returns><c>true</c> if the value was written; otherwise, <c>false</c>.</returns>
    public static bool TryWriteWithConverter
    (
        this Utf8JsonWriter writer,
        Ulid? value,
        JsonSerializerOptions options,
        UlidJsonConverter converter,
        Ulid? defaultValue = default
    )
    {
        return writer.TryWrite
        (
            value,
            options,
            (converter, options),
            static (writer, value, state) => state.converter.Write(writer, value, state.options),
            defaultValue
        );
    }
    #endregion

    #region TryWritePropertyWithConverter Extension Methods
    /// <summary>
    ///     Attempts to write a <see cref="CultureInfo"/> property using the specified converter when the
    ///     serializer options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="cultureInfo">The cultureInfo to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="converter">The converter used to write the value.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <param name="equalityComparer">The comparer used to compare non-null values with the default.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyWithConverter
    (
        this Utf8JsonWriter writer,
        string propertyName,
        CultureInfo? cultureInfo,
        JsonSerializerOptions options,
        CultureInfoJsonConverter converter,
        CultureInfo? defaultValue = default,
        EqualityComparer<CultureInfo>? equalityComparer = null
    )
    {
        return writer.TryWriteProperty
        (
            propertyName,
            cultureInfo,
            options,
            (converter, options),
            static (writer, value, state) => state.converter.Write(writer, value, state.options),
            defaultValue,
            equalityComparer
        );
    }

    /// <summary>
    ///     Attempts to write an enum property using the specified converter when the serializer
    ///     options permit it.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to write.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The enum value to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="converter">The converter used to write the value.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyWithConverter<TEnum>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        TEnum value,
        JsonSerializerOptions options,
        EnumJsonConverter<TEnum> converter,
        TEnum defaultValue = default
    )
        where TEnum : struct, Enum
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (converter, options),
            static (writer, value, state) => state.converter.Write(writer, value, state.options),
            defaultValue
        );
    }

    /// <summary>
    ///     Attempts to write a nullable enum property using the specified converter when the
    ///     serializer options permit it.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to write.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The enum value to write, if present.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="converter">The converter used to write the value.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyWithConverter<TEnum>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        TEnum? value,
        JsonSerializerOptions options,
        EnumJsonConverter<TEnum> converter,
        TEnum? defaultValue = default
    )
        where TEnum : struct, Enum
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (converter, options),
            static (writer, value, state) => state.converter.Write(writer, value, state.options),
            defaultValue
        );
    }

    /// <summary>
    ///     Attempts to write a <see cref="Type"/> property using the specified converter when the
    ///     serializer options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="type">The type to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="converter">The converter used to write the value.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyWithConverter
    (
        this Utf8JsonWriter writer,
        string propertyName,
        Type? type,
        JsonSerializerOptions options,
        TypeJsonConverter converter,
        Type? defaultValue = default
    )
    {
        return writer.TryWriteProperty
        (
            propertyName,
            type,
            options,
            (converter, options),
            static (writer, value, state) => state.converter.Write(writer, value, state.options),
            defaultValue
        );
    }

    /// <summary>
    ///     Attempts to write an Ulid property using the specified converter when the serializer
    ///     options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The Ulid value to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="converter">The converter used to write the value.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyWithConverter
    (
        this Utf8JsonWriter writer,
        string propertyName,
        Ulid value,
        JsonSerializerOptions options,
        UlidJsonConverter converter,
        Ulid defaultValue = default
    )
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (converter, options),
            static (writer, value, state) => state.converter.Write(writer, value, state.options),
            defaultValue
        );
    }

    /// <summary>
    ///     Attempts to write a nullable Ulid property using the specified converter when the
    ///     serializer options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The Ulid value to write, if present.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="converter">The converter used to write the value.</param>
    /// <param name="defaultValue">The value omitted under WhenWritingDefault; defaults to the CLR default.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWritePropertyWithConverter
    (
        this Utf8JsonWriter writer,
        string propertyName,
        Ulid? value,
        JsonSerializerOptions options,
        UlidJsonConverter converter,
        Ulid? defaultValue = default
    )
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            (converter, options),
            static (writer, value, state) => state.converter.Write(writer, value, state.options),
            defaultValue
        );
    }
    #endregion
}
