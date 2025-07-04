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
    #region Number Extension Methods
    /// <summary>
    ///     Writes a <c>decimal</c> value as a JSON number property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance used to write the JSON.</param>
    /// <param name="propertyName">The name of the JSON property to write.</param>
    /// <param name="value">The non-nullable decimal value to write.</param>
    /// <param name="options">The serializer options used to determine ignore behavior.</param>
    /// <param name="equalityComparer">Optional equality comparer used to detect default values.</param>
    public static void WriteConditionalPropertyAsNumber(this Utf8JsonWriter writer, string propertyName, decimal value, JsonSerializerOptions options, EqualityComparer<decimal>? equalityComparer = null)
    {
        writer.WriteConditionalProperty
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a nullable <c>decimal</c> value as a JSON number property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance used to write the JSON.</param>
    /// <param name="propertyName">The name of the JSON property to write.</param>
    /// <param name="value">The nullable decimal value to write.</param>
    /// <param name="options">The serializer options used to determine ignore behavior.</param>
    /// <param name="equalityComparer">Optional equality comparer used to detect default values for non-null values.</param>
    public static void WriteConditionalPropertyAsNumber(this Utf8JsonWriter writer, string propertyName, decimal? value, JsonSerializerOptions options, EqualityComparer<decimal>? equalityComparer = null)
    {
        writer.WriteConditionalNullableProperty
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a <c>double</c> value as a JSON number property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance used to write the JSON.</param>
    /// <param name="propertyName">The name of the JSON property to write.</param>
    /// <param name="value">The non-nullable double value to write.</param>
    /// <param name="options">The serializer options used to determine ignore behavior.</param>
    /// <param name="equalityComparer">Optional equality comparer used to detect default values.</param>
    public static void WriteConditionalPropertyAsNumber(this Utf8JsonWriter writer, string propertyName, double value, JsonSerializerOptions options, EqualityComparer<double>? equalityComparer = null)
    {
        writer.WriteConditionalProperty
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a nullable <c>double</c> value as a JSON number property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance used to write the JSON.</param>
    /// <param name="propertyName">The name of the JSON property to write.</param>
    /// <param name="value">The nullable double value to write.</param>
    /// <param name="options">The serializer options used to determine ignore behavior.</param>
    /// <param name="equalityComparer">Optional equality comparer used to detect default values for non-null values.</param>
    public static void WriteConditionalPropertyAsNumber(this Utf8JsonWriter writer, string propertyName, double? value, JsonSerializerOptions options, EqualityComparer<double>? equalityComparer = null)
    {
        writer.WriteConditionalNullableProperty
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a <c>float</c> value as a JSON number property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance used to write the JSON.</param>
    /// <param name="propertyName">The name of the JSON property to write.</param>
    /// <param name="value">The non-nullable float value to write.</param>
    /// <param name="options">The serializer options used to determine ignore behavior.</param>
    /// <param name="equalityComparer">Optional equality comparer used to detect default values.</param>
    public static void WriteConditionalPropertyAsNumber(this Utf8JsonWriter writer, string propertyName, float value, JsonSerializerOptions options, EqualityComparer<float>? equalityComparer = null)
    {
        writer.WriteConditionalProperty
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a nullable <c>float</c> value as a JSON number property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance used to write the JSON.</param>
    /// <param name="propertyName">The name of the JSON property to write.</param>
    /// <param name="value">The nullable float value to write.</param>
    /// <param name="options">The serializer options used to determine ignore behavior.</param>
    /// <param name="equalityComparer">Optional equality comparer used to detect default values for non-null values.</param>
    public static void WriteConditionalPropertyAsNumber(this Utf8JsonWriter writer, string propertyName, float? value, JsonSerializerOptions options, EqualityComparer<float>? equalityComparer = null)
    {
        writer.WriteConditionalNullableProperty
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a <c>int</c> value as a JSON number property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance used to write the JSON.</param>
    /// <param name="propertyName">The name of the JSON property to write.</param>
    /// <param name="value">The non-nullable int value to write.</param>
    /// <param name="options">The serializer options used to determine ignore behavior.</param>
    /// <param name="equalityComparer">Optional equality comparer used to detect default values.</param>
    public static void WriteConditionalPropertyAsNumber(this Utf8JsonWriter writer, string propertyName, int value, JsonSerializerOptions options, EqualityComparer<int>? equalityComparer = null)
    {
        writer.WriteConditionalProperty
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a nullable <c>int</c> value as a JSON number property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance used to write the JSON.</param>
    /// <param name="propertyName">The name of the JSON property to write.</param>
    /// <param name="value">The nullable int value to write.</param>
    /// <param name="options">The serializer options used to determine ignore behavior.</param>
    /// <param name="equalityComparer">Optional equality comparer used to detect default values for non-null values.</param>
    public static void WriteConditionalPropertyAsNumber(this Utf8JsonWriter writer, string propertyName, int? value, JsonSerializerOptions options, EqualityComparer<int>? equalityComparer = null)
    {
        writer.WriteConditionalNullableProperty
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a <c>long</c> value as a JSON number property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance used to write the JSON.</param>
    /// <param name="propertyName">The name of the JSON property to write.</param>
    /// <param name="value">The non-nullable long value to write.</param>
    /// <param name="options">The serializer options used to determine ignore behavior.</param>
    /// <param name="equalityComparer">Optional equality comparer used to detect default values.</param>
    public static void WriteConditionalPropertyAsNumber(this Utf8JsonWriter writer, string propertyName, long value, JsonSerializerOptions options, EqualityComparer<long>? equalityComparer = null)
    {
        writer.WriteConditionalProperty
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a nullable <c>long</c> value as a JSON number property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> instance used to write the JSON.</param>
    /// <param name="propertyName">The name of the JSON property to write.</param>
    /// <param name="value">The nullable long value to write.</param>
    /// <param name="options">The serializer options used to determine ignore behavior.</param>
    /// <param name="equalityComparer">Optional equality comparer used to detect default values for non-null values.</param>
    public static void WriteConditionalPropertyAsNumber(this Utf8JsonWriter writer, string propertyName, long? value, JsonSerializerOptions options, EqualityComparer<long>? equalityComparer = null)
    {
        writer.WriteConditionalNullableProperty
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }
    #endregion
}
