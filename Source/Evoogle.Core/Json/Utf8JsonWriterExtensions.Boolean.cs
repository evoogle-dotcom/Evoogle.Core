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
    /// <summary>
    ///     Writes a <c>bool</c> value as a JSON property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> used for writing JSON.</param>
    /// <param name="propertyName">The name of the property to write.</param>
    /// <param name="value">The non-nullable boolean value to write.</param>
    /// <param name="options">The serializer options that control conditional property writing, such as <see cref="JsonIgnoreCondition.WhenWritingDefault"/>.</param>
    /// <param name="equalityComparer">Optional comparer used to determine whether the value is considered the default.</param>
    /// <remarks>
    ///     If <paramref name="options"/> specifies that default values should be ignored and <paramref name="value"/> is <c>false</c>,
    ///     the property will not be written.
    /// </remarks>    
    public static void WriteConditionalPropertyAsBoolean(this Utf8JsonWriter writer, string propertyName, bool value, JsonSerializerOptions options, EqualityComparer<bool>? equalityComparer = null)
    {
        writer.WriteConditionalProperty
        (
            propertyName,
            value,
            options,
            writer.WriteBoolean,
            equalityComparer
        );
    }

    /// <summary>
    ///     Writes a nullable <c>bool</c> value as a JSON property if permitted by <paramref name="options"/>.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> used for writing JSON.</param>
    /// <param name="propertyName">The name of the property to write.</param>
    /// <param name="value">The nullable boolean value to write.</param>
    /// <param name="options">The serializer options that control conditional property writing, such as <see cref="JsonIgnoreCondition.WhenWritingNull"/> or <see cref="JsonIgnoreCondition.WhenWritingDefault"/>.
    /// </param>
    /// <param name="equalityComparer">
    /// Optional comparer used to determine whether the non-null value is considered the default.
    /// </param>
    /// <remarks>
    /// If <paramref name="value"/> is <c>null</c> and the ignore condition is set to skip nulls, the property will not be written.
    /// </remarks>
    public static void WriteConditionalPropertyAsBoolean(this Utf8JsonWriter writer, string propertyName, bool? value, JsonSerializerOptions options, EqualityComparer<bool>? equalityComparer = null)
    {
        writer.WriteConditionalNullableProperty
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
