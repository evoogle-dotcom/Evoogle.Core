// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;
using System.Text.Json.Serialization;

using Evoogle.Json.Internal;

namespace Evoogle.Json;

/// <summary>
///     A custom JSON converter for enums that serializes and deserializes enum values as strings instead of their numeric values.
///     For enums with the [Flags] attribute, it supports  multiple values represented as a comma-separated list of strings.
/// </summary>
/// <typeparam name="TEnum">The enum type to be converted.</typeparam>
public class EnumJsonConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    #region JsonConverter Methods
    /// <summary>
    ///     Deserializes a JSON string into the corresponding enum value.
    /// </summary>
    /// <param name="reader">The UTF-8 JSON reader providing the string value.</param>
    /// <param name="typeToConvert">The type of enum to convert to (TEnum).</param>
    /// <param name="options">Options for the JSON serializer.</param>
    /// <returns>The deserialized enum value of type TEnum.</returns>
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }

        if (EnumJsonConversion<TEnum>.TryParse(value, out var enumeration, out var failure))
        {
            return enumeration;
        }

        throw failure switch
        {
            EnumJsonParseFailure.InvalidFlagsValue => new JsonException($"Invalid enum value '{value}' for [Flags] enum {{Type={this.Type.Name}}}"),
            EnumJsonParseFailure.CommaInNonFlagsValue => new JsonException($"Comma is not allowed for non-[Flags] enum {{Type={this.Type.Name}}}"),
            EnumJsonParseFailure.ParseFailure => new JsonException($"Unable to parse enum value '{value}' for enum {{Type={this.Type.Name}}}"),
            _ => new JsonException($"Invalid enum value '{value}' for enum {{Type={this.Type.Name}}}")
        };
    }

    /// <summary>
    ///     Serializes an enum value to a JSON string.
    /// </summary>
    /// <param name="writer">The UTF-8 JSON writer to output the string.</param>
    /// <param name="value">The enum value to serialize.</param>
    /// <param name="options">Options for the JSON serializer.</param>
    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        => writer.WriteStringValue(EnumJsonConversion<TEnum>.Format(value));
    #endregion
}
