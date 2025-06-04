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
    /// <summary>Conditionally writes an <c>enum</c> property with an <see cref="EnumJsonConverter{TEnum}"/>.</summary>
    public static void WritePropertyWithConverter<TEnum>(this Utf8JsonWriter writer, string propertyName, TEnum value, JsonSerializerOptions options, EnumJsonConverter<TEnum> converter, EqualityComparer<TEnum>? equalityComparer = null)
        where TEnum : struct, Enum
    {
        writer.WritePropertyValueType
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

    /// <summary>Conditionally writes an <c>enum?</c> property with an <see cref="EnumJsonConverter{TEnum}"/>.</summary>
    public static void WritePropertyWithConverter<TEnum>(this Utf8JsonWriter writer, string propertyName, TEnum? value, JsonSerializerOptions options, EnumJsonConverter<TEnum> converter, EqualityComparer<TEnum>? equalityComparer = null)
        where TEnum : struct, Enum
    {
        writer.WritePropertyNullableValueType
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

    /// <summary>Conditionally writes an <c>Type?</c> property with an <see cref="TypeJsonConverter"/>.</summary>
    public static void WritePropertyWithConverter(this Utf8JsonWriter writer, string propertyName, Type? value, JsonSerializerOptions options, TypeJsonConverter converter)
    {
        writer.WritePropertyReferenceType
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
