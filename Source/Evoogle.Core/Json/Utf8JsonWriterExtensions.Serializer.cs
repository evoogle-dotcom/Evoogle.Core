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
    /// <summary>
    ///     Writes a value type property using the default <see cref="JsonSerializer"/> if permitted by <paramref name="options"/>.
    /// </summary>
    /// <typeparam name="T">The value type to serialize.</typeparam>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> used for writing JSON.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The non-nullable value to serialize.</param>
    /// <param name="options">The serializer options that determine whether to ignore default values.</param>
    /// <param name="equalityComparer">Optional equality comparer for detecting default values.</param>
    /// <remarks>
    /// This method uses <see cref="JsonSerializer.Serialize{TValue}(Utf8JsonWriter, TValue, JsonSerializerOptions)"/>
    /// to write the value if it is not filtered out by ignore conditions such as <see cref="JsonIgnoreCondition.WhenWritingDefault"/>.
    /// </remarks>
    public static void WriteConditionalPropertyWithSerializer<T>(this Utf8JsonWriter writer, string propertyName, T value, JsonSerializerOptions options, EqualityComparer<T>? equalityComparer = null)
        where T : struct
    {
        writer.WriteConditionalProperty
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

    /// <summary>
    ///     Writes a nullable value type property using the default <see cref="JsonSerializer"/> if permitted by <paramref name="options"/>.
    /// </summary>
    /// <typeparam name="T">The underlying value type.</typeparam>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> used for writing JSON.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The nullable value to serialize.</param>
    /// <param name="options">The serializer options that determine whether to ignore null or default values.</param>
    /// <param name="equalityComparer">Optional equality comparer for detecting default values.</param>
    /// <remarks>
    /// If <paramref name="value"/> is null and ignore nulls is enabled, the property will be skipped.
    /// Otherwise, it will be serialized using the built-in <see cref="JsonSerializer"/>.
    /// </remarks>
    public static void WriteConditionalPropertyWithSerializer<T>(this Utf8JsonWriter writer, string propertyName, T? value, JsonSerializerOptions options, EqualityComparer<T>? equalityComparer = null)
        where T : struct
    {
        writer.WriteConditionalNullableProperty
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

    /// <summary>
    ///     Writes a reference type property using the default <see cref="JsonSerializer"/> if permitted by <paramref name="options"/>.
    /// </summary>
    /// <typeparam name="T">The reference type to serialize.</typeparam>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> used for writing JSON.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The reference object to serialize.</param>
    /// <param name="options">The serializer options that determine whether to ignore nulls.</param>
    /// <remarks>
    /// If <paramref name="value"/> is null and ignore nulls is enabled, the property will not be written.
    /// Otherwise, it is serialized using the default <see cref="JsonSerializer"/>.
    /// </remarks>
    public static void WriteConditionalPropertyWithSerializer<T>(this Utf8JsonWriter writer, string propertyName, T? value, JsonSerializerOptions options)
        where T : class
    {
        writer.WriteConditionalReferenceProperty
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
