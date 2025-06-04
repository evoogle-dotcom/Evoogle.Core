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
    #region Boolean Extension Methods
    /// <summary>Conditionally writes a <c>bool</c> boolean property.</summary>
    public static void WritePropertyBoolean(this Utf8JsonWriter writer, string propertyName, bool value, JsonSerializerOptions options, EqualityComparer<bool>? equalityComparer = null)
    {
        writer.WritePropertyValueType
        (
            propertyName,
            value,
            options,
            writer.WriteBoolean,
            equalityComparer
        );
    }

    /// <summary>Conditionally writes a <c>bool?</c> boolean property.</summary>
    public static void WritePropertyBoolean(this Utf8JsonWriter writer, string propertyName, bool? value, JsonSerializerOptions options, EqualityComparer<bool>? equalityComparer = null)
    {
        writer.WritePropertyNullableValueType
        (
            propertyName,
            value,
            options,
            writer.WriteBoolean,
            equalityComparer
        );
    }
    #endregion
}
