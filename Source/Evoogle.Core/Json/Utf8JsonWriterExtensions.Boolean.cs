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
    /// <summary>
    ///     Attempts to write a boolean property when the serializer options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The boolean value to write.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="equalityComparer">
    ///     Optional comparer used to detect default values.
    /// </param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
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

    /// <summary>
    ///     Attempts to write a nullable boolean property when the serializer options permit it.
    /// </summary>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The boolean value to write, if present.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="equalityComparer">
    ///     Optional comparer used to detect default values.
    /// </param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
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
