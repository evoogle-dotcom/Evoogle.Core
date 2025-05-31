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
    #region Object Extension Methods
    /// <summary>Conditionally writes a value type property using JSON serialization.</summary>
    public static void WritePropertyWithSerializer<T>(this Utf8JsonWriter writer, string propertyName, T value, JsonSerializerOptions options, EqualityComparer<T>? equalityComparer = null)
        where T : struct
    {
        writer.WritePropertyValueType
        (
            propertyName,
            value,
            options,
            (name, valueObject) =>
            {
                writer.WritePropertyName(name);
                JsonSerializer.Serialize(writer, valueObject, options);
            },
            equalityComparer
        );
    }

    /// <summary>Conditionally writes a nullable value type property using JSON serialization.</summary>
    public static void WritePropertyWithSerializer<T>(this Utf8JsonWriter writer, string propertyName, T? value, JsonSerializerOptions options, EqualityComparer<T>? equalityComparer = null)
        where T : struct
    {
        writer.WritePropertyNullableValueType
        (
            propertyName,
            value,
            options,
            (name, nullableValueObject) =>
            {
                writer.WritePropertyName(name);
                JsonSerializer.Serialize(writer, nullableValueObject, options);
            },
            equalityComparer
        );
    }

    /// <summary>Conditionally writes a reference type property using JSON serialization.</summary>
    public static void WritePropertyWithSerializer<T>(this Utf8JsonWriter writer, string propertyName, T? value, JsonSerializerOptions options)
        where T : class
    {
        writer.WritePropertyReferenceType
        (
            propertyName,
            value,
            options,
            (name, referenceObject) =>
            {
                writer.WritePropertyName(name);
                JsonSerializer.Serialize(writer, referenceObject, options);
            }
        );
    }
    #endregion
}