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
    #region TryWritePropertyAsNumber Extension Methods
    public static bool TryWritePropertyAsNumber(this Utf8JsonWriter writer, string propertyName, decimal value, JsonSerializerOptions options, EqualityComparer<decimal>? equalityComparer = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsNumber(this Utf8JsonWriter writer, string propertyName, decimal? value, JsonSerializerOptions options, EqualityComparer<decimal>? equalityComparer = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsNumber(this Utf8JsonWriter writer, string propertyName, double value, JsonSerializerOptions options, EqualityComparer<double>? equalityComparer = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsNumber(this Utf8JsonWriter writer, string propertyName, double? value, JsonSerializerOptions options, EqualityComparer<double>? equalityComparer = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsNumber(this Utf8JsonWriter writer, string propertyName, float value, JsonSerializerOptions options, EqualityComparer<float>? equalityComparer = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsNumber(this Utf8JsonWriter writer, string propertyName, float? value, JsonSerializerOptions options, EqualityComparer<float>? equalityComparer = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsNumber(this Utf8JsonWriter writer, string propertyName, int value, JsonSerializerOptions options, EqualityComparer<int>? equalityComparer = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsNumber(this Utf8JsonWriter writer, string propertyName, int? value, JsonSerializerOptions options, EqualityComparer<int>? equalityComparer = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsNumber(this Utf8JsonWriter writer, string propertyName, long value, JsonSerializerOptions options, EqualityComparer<long>? equalityComparer = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            writer.WriteNumber,
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsNumber(this Utf8JsonWriter writer, string propertyName, long? value, JsonSerializerOptions options, EqualityComparer<long>? equalityComparer = null)
    {
        return writer.TryWriteProperty
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
