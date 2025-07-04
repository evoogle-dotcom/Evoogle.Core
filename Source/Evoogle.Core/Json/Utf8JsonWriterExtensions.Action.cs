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
    ///     Writes a value type property using a custom action delegate to serialize the value if allowed by <paramref name="options"/>.
    /// </summary>
    /// <typeparam name="T">The value type to serialize.</typeparam>
    /// <param name="writer">The JSON writer instance.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The non-nullable value to write.</param>
    /// <param name="options">Serialization options that determine ignore conditions.</param>
    /// <param name="writeValueAction">The delegate responsible for writing the value.</param>
    /// <param name="equalityComparer">Optional comparer used to detect default values.</param>    
    public static void WriteConditionalPropertyWithAction<T>(this Utf8JsonWriter writer, string propertyName, T value, JsonSerializerOptions options, Action<T> writeValueAction, EqualityComparer<T>? equalityComparer = null)
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
                writeValueAction(valueObject);
            },
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a nullable value type property using a custom action delegate to serialize the value if allowed by <paramref name="options"/>.
    /// </summary>
    /// <typeparam name="T">The underlying value type.</typeparam>
    /// <param name="writer">The JSON writer instance.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The nullable value to write.</param>
    /// <param name="options">Serialization options that determine ignore conditions.</param>
    /// <param name="writeValueAction">The delegate responsible for writing the non-null value.</param>
    /// <param name="equalityComparer">Optional comparer used to detect default values.</param>
    public static void WriteConditionalPropertyWithAction<T>(this Utf8JsonWriter writer, string propertyName, T? value, JsonSerializerOptions options, Action<T> writeValueAction, EqualityComparer<T>? equalityComparer = null)
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
                writeValueAction(nullableValueObject);
            },
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a reference type property using a custom action delegate to serialize the value if allowed by <paramref name="options"/>.
    /// </summary>
    /// <typeparam name="T">The reference type.</typeparam>
    /// <param name="writer">The JSON writer instance.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The reference type value to write.</param>
    /// <param name="options">Serialization options that determine ignore conditions.</param>
    /// <param name="writeValueAction">The delegate responsible for writing the non-null value.</param>
    public static void WriteConditionalPropertyWithAction<T>(this Utf8JsonWriter writer, string propertyName, T? value, JsonSerializerOptions options, Action<T> writeValueAction)
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
                writeValueAction(referenceObject);
            }
        );
    }
    #endregion
}
