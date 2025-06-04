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
    /// <summary>Conditionally writes a <c>decimal</c> number property.</summary>
    public static void WritePropertyNumber(this Utf8JsonWriter writer, string propertyName, decimal value, JsonSerializerOptions options, EqualityComparer<decimal>? equalityComparer = null)
    {
        writer.WritePropertyValueType
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    /// <summary>Conditionally writes a <c>decimal?</c> number property.</summary>
    public static void WritePropertyNumber(this Utf8JsonWriter writer, string propertyName, decimal? value, JsonSerializerOptions options, EqualityComparer<decimal>? equalityComparer = null)
    {
        writer.WritePropertyNullableValueType
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    /// <summary>Conditionally writes a <c>double</c> number property.</summary>
    public static void WritePropertyNumber(this Utf8JsonWriter writer, string propertyName, double value, JsonSerializerOptions options, EqualityComparer<double>? equalityComparer = null)
    {
        writer.WritePropertyValueType
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    /// <summary>Conditionally writes a <c>double?</c> number property.</summary>
    public static void WritePropertyNumber(this Utf8JsonWriter writer, string propertyName, double? value, JsonSerializerOptions options, EqualityComparer<double>? equalityComparer = null)
    {
        writer.WritePropertyNullableValueType
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    /// <summary>Conditionally writes a <c>float</c> number property.</summary>
    public static void WritePropertyNumber(this Utf8JsonWriter writer, string propertyName, float value, JsonSerializerOptions options, EqualityComparer<float>? equalityComparer = null)
    {
        writer.WritePropertyValueType
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    /// <summary>Conditionally writes a <c>float?</c> number property.</summary>
    public static void WritePropertyNumber(this Utf8JsonWriter writer, string propertyName, float? value, JsonSerializerOptions options, EqualityComparer<float>? equalityComparer = null)
    {
        writer.WritePropertyNullableValueType
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    /// <summary>Conditionally writes an <c>int</c> number property.</summary>
    public static void WritePropertyNumber(this Utf8JsonWriter writer, string propertyName, int value, JsonSerializerOptions options, EqualityComparer<int>? equalityComparer = null)
    {
        writer.WritePropertyValueType
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    /// <summary>Conditionally writes an <c>int?</c> number property.</summary>
    public static void WritePropertyNumber(this Utf8JsonWriter writer, string propertyName, int? value, JsonSerializerOptions options, EqualityComparer<int>? equalityComparer = null)
    {
        writer.WritePropertyNullableValueType
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    /// <summary>Conditionally writes a <c>long</c> number property.</summary>
    public static void WritePropertyNumber(this Utf8JsonWriter writer, string propertyName, long value, JsonSerializerOptions options, EqualityComparer<long>? equalityComparer = null)
    {
        writer.WritePropertyValueType
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    /// <summary>Conditionally writes a <c>long?</c> number property.</summary>
    public static void WritePropertyNumber(this Utf8JsonWriter writer, string propertyName, long? value, JsonSerializerOptions options, EqualityComparer<long>? equalityComparer = null)
    {
        writer.WritePropertyNullableValueType
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
