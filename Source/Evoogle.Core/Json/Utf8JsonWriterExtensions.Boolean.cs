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
    #region TryWritePropertyAsBoolean Extension Methods
    public static bool TryWritePropertyAsBoolean(this Utf8JsonWriter writer, string propertyName, bool value, JsonSerializerOptions options, EqualityComparer<bool>? equalityComparer = null)
    {
        return writer.TryWriteProperty
        (
            propertyName,
            value,
            options,
            writer.WriteBoolean,
            equalityComparer
        );
    }

    public static bool TryWritePropertyAsBoolean(this Utf8JsonWriter writer, string propertyName, bool? value, JsonSerializerOptions options, EqualityComparer<bool>? equalityComparer = null)
    {
        return writer.TryWriteProperty
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
