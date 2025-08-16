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
    /// <summary>
    ///     Attempts to write an enum value using the specified converter when the serializer
    ///     options permit it.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to write.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="value">The enum value to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="converter">The converter used to write the value.</param>
    /// <param name="equalityComparer">
    ///     Optional comparer used to detect default values.
    /// </param>
    /// <returns><c>true</c> if the value was written; otherwise, <c>false</c>.</returns>
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

    /// <summary>
    ///     Attempts to write a nullable enum value using the specified converter when the
    ///     serializer options permit it.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to write.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="value">The enum value to write, if present.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="converter">The converter used to write the value.</param>
    /// <param name="equalityComparer">
    ///     Optional comparer used to detect default values.
    /// </param>
    /// <returns><c>true</c> if the value was written; otherwise, <c>false</c>.</returns>
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

    /// <summary>
    ///     Attempts to write a <see cref="Type"/> value using the specified converter when the
    ///     serializer options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="type">The type to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="converter">The converter used to write the value.</param>
    /// <returns><c>true</c> if the value was written; otherwise, <c>false</c>.</returns>
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
    /// <param name="equalityComparer">
    ///     Optional comparer used to detect default values.
    /// </param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
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
    /// <param name="equalityComparer">
    ///     Optional comparer used to detect default values.
    /// </param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
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

    /// <summary>
    ///     Attempts to write a <see cref="Type"/> property using the specified converter when the
    ///     serializer options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="type">The type to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="converter">The converter used to write the value.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
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
