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
    /// <param name="nullHandling">Specifies how null property values are handled.</param>
    /// <returns>An immutable array containing the property names in object order.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="nullHandling"/> is not a valid <see cref="JsonReaderNullPropertyHandling"/>
    ///     value.
    /// </exception>
    /// <exception cref="JsonException">
    ///     The reader is not positioned at the start of a JSON object, the object is malformed,
    ///     the payload ends unexpectedly, or a null property value is encountered when
    ///     <paramref name="nullHandling"/> is <see cref="JsonReaderNullPropertyHandling.Throw"/>.
    /// </exception>
    public static ImmutableArray<string> ReadObjectPropertyNames(
        this ref Utf8JsonReader reader,
        JsonReaderNullPropertyHandling nullHandling)
    {
        if (nullHandling is not (JsonReaderNullPropertyHandling.Allow
            or JsonReaderNullPropertyHandling.Ignore
            or JsonReaderNullPropertyHandling.Throw))
        {
            throw new ArgumentOutOfRangeException(nameof(nullHandling));
        }

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

            var nameReader = lookahead;

            // Move from the property name to its value.
            if (!lookahead.Read())
            {
                throw new JsonException("Unexpected end of JSON.");
            }

            if (lookahead.TokenType == JsonTokenType.Null)
            {
                if (nullHandling == JsonReaderNullPropertyHandling.Ignore)
                {
                    continue;
                }

                if (nullHandling == JsonReaderNullPropertyHandling.Throw)
                {
                    throw new JsonException($"Property '{nameReader.GetString()}' has an unexpected null value.");
                }
            }

            var propertyName = nameReader.GetString()!;
            propertyNames.Add(propertyName);

            // Skip a nested object or array without examining its contents.
            lookahead.Skip();
        }

        return propertyNames.ToImmutable();
    }
    #endregion
}
