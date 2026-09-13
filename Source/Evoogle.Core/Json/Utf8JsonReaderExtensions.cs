// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Collections.Immutable;
using System.Text.Json;

namespace Evoogle.Json;

/// <summary>
///     Provides helper methods for reading structured JSON values from a <see cref="Utf8JsonReader"/>.
/// </summary>
public static class Utf8JsonReaderExtensions
{
    #region Read Methods
    /// <summary>
    ///     Reads an object's property names while skipping nested values.
    /// </summary>
    /// <param name="reader">The UTF-8 JSON reader positioned at the start of an object.</param>
    /// <returns>An immutable array containing the property names in object order.</returns>
    /// <exception cref="JsonException">
    ///     The reader is not positioned at the start of a JSON object, the object is malformed, or the payload ends unexpectedly.
    /// </exception>
    public static ImmutableArray<string> ReadObjectPropertyNames(this ref Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of JSON object.");
        }

        var lookahead = reader;
        var objectDepth = lookahead.CurrentDepth;
        var propertyNames = ImmutableArray.CreateBuilder<string>();

        while (true)
        {
            if (!lookahead.Read())
            {
                throw new JsonException("Unexpected end of JSON.");
            }

            if (lookahead.TokenType == JsonTokenType.EndObject && lookahead.CurrentDepth == objectDepth)
            {
                break;
            }

            if (lookahead.TokenType != JsonTokenType.PropertyName || lookahead.CurrentDepth != objectDepth + 1)
            {
                throw new JsonException("Expected a JSON property name.");
            }

            var propertyName = lookahead.GetString()!;
            propertyNames.Add(propertyName);

            // Move from the property name to its value.
            if (!lookahead.Read())
            {
                throw new JsonException("Unexpected end of JSON.");
            }

            // Skip a nested object or array without examining its contents.
            lookahead.Skip();
        }

        return propertyNames.ToImmutable();
    }
    #endregion
}
