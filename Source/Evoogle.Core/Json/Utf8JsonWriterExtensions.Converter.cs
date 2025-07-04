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
    #region JsonConverter Extension Methods
    /// <summary>
    ///     Writes an enum value as a JSON property using a custom <see cref="EnumJsonConverter{TEnum}"/> if permitted by <paramref name="options"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to write.</typeparam>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> used for writing JSON.</param>
    /// <param name="propertyName">The name of the JSON property to write.</param>
    /// <param name="value">The enum value to write.</param>
    /// <param name="options">Serialization options that control property ignore conditions.</param>
    /// <param name="converter">The custom converter used to serialize the enum value.</param>
    /// <param name="equalityComparer">Optional comparer used to determine whether the value is the default.</param>
    /// <remarks>
    /// This method respects the ignore behavior defined in <paramref name="options"/> and delegates value serialization to the provided <paramref name="converter"/>.
    /// </remarks>
    public static void WriteConditionalPropertyWithConverter<TEnum>(this Utf8JsonWriter writer, string propertyName, TEnum value, JsonSerializerOptions options, EnumJsonConverter<TEnum> converter, EqualityComparer<TEnum>? equalityComparer = null)
        where TEnum : struct, Enum
    {
        writer.WriteConditionalProperty
        (
            propertyName,
            value,
            options,
            (name, enumeration) =>
            {
                writer.WritePropertyName(name);
                converter.Write(writer, enumeration, options);
            },
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a nullable enum value as a JSON property using a custom <see cref="EnumJsonConverter{TEnum}"/> if permitted by <paramref name="options"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to write.</typeparam>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> used for writing JSON.</param>
    /// <param name="propertyName">The name of the JSON property to write.</param>
    /// <param name="value">The nullable enum value to write.</param>
    /// <param name="options">Serialization options that control property ignore conditions.</param>
    /// <param name="converter">The custom converter used to serialize the enum value.</param>
    /// <param name="equalityComparer">Optional comparer used to determine whether the non-null value is the default.</param>
    /// <remarks>
    /// If <paramref name="value"/> is null and <paramref name="options"/> specifies to ignore nulls, the property is skipped.
    /// Otherwise, the value is serialized via the provided <paramref name="converter"/>.
    /// </remarks>
    public static void WriteConditionalPropertyWithConverter<TEnum>(this Utf8JsonWriter writer, string propertyName, TEnum? value, JsonSerializerOptions options, EnumJsonConverter<TEnum> converter, EqualityComparer<TEnum>? equalityComparer = null)
        where TEnum : struct, Enum
    {
        writer.WriteConditionalNullableProperty
        (
            propertyName,
            value,
            options,
            (name, enumeration) =>
            {
                writer.WritePropertyName(name);
                converter.Write(writer, enumeration, options);
            },
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a <see cref="Type"/> as a JSON property using a custom <see cref="TypeJsonConverter"/>, if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> used for writing JSON.</param>
    /// <param name="propertyName">The name of the JSON property to write.</param>
    /// <param name="value">The <see cref="Type"/> to write, or null.</param>
    /// <param name="options">Serialization options that control null or default value suppression.</param>
    /// <param name="converter">The <see cref="TypeJsonConverter"/> responsible for writing the type.</param>
    /// <remarks>
    /// If <paramref name="value"/> is null and <paramref name="options"/> suppresses nulls, the property is not written.
    /// Otherwise, the type is serialized using the provided <paramref name="converter"/>.
    /// </remarks>
    public static void WriteConditionalPropertyWithConverter(this Utf8JsonWriter writer, string propertyName, Type? value, JsonSerializerOptions options, TypeJsonConverter converter)
    {
        writer.WriteConditionalReferenceProperty
        (
            propertyName,
            value,
            options,
            (name, type) =>
            {
                writer.WritePropertyName(name);
                converter.Write(writer, type, options);
            }
        );
    }
    #endregion
}
