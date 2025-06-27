// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Evoogle.Json;

/// <summary>
///     A custom JSON converter for enums that serializes and deserializes enum values as strings instead of their numeric values.
///     For enums with the [Flags] attribute, it supports  multiple values represented as a comma-separated list of strings.
/// </summary>
/// <typeparam name="TEnum">The enum type to be converted.</typeparam>
public class EnumJsonConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    #region Fields
    /// <summary>
    ///     Indicates whether the enum type is decorated with the [Flags] attribute, allowing it to represent a combination of values.
    /// </summary>
    private static readonly bool _hasFlags = typeof(TEnum).GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0;

    /// <summary>
    ///     An array of all valid enum names for the specified enum type, used to validate input during deserialization.
    /// </summary>
    private static readonly string[] _enumNames = Enum.GetNames<TEnum>();
    #endregion

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
            return default;

        if (_hasFlags)
        {
            // Splits the string into parts for [Flags] enums, trimming whitespace from each.
            var parts = value.Split(',').Select(p => p.Trim()).ToArray();
            if (parts.All(part => _enumNames.Any(name => string.Equals(name, part, StringComparison.OrdinalIgnoreCase))))
            {
                // If all parts match valid enum names, parses the combined string into an enum value.
                return this.ParseString(value);
            }
            else
            {
                // Throws an exception if any part of the input is not a valid enum name.
                throw new JsonException($"Invalid enum value '{value}' for [Flags] enum {{Type={this.Type.Name}}}");
            }
        }
        else
        {
            // For non-[Flags] enums, checks that the value does not contain commas.
            if (value.Contains(','))
            {
                throw new JsonException($"Comma is not allowed for non-[Flags] enum {{Type={this.Type.Name}}}");
            }
            // Validates that the input matches a valid enum name (case-insensitive).
            if (_enumNames.Any(name => string.Equals(name, value, StringComparison.OrdinalIgnoreCase)))
            {
                // Parses the string into an enum value if it matches a valid name.
                return this.ParseString(value);
            }
            else
            {
                // Throws an exception if the input does not match any valid enum name.
                throw new JsonException($"Invalid enum value '{value}' for enum {{Type={this.Type.Name}}}");
            }
        }
    }

    /// <summary>
    ///     Serializes an enum value to a JSON string.
    /// </summary>
    /// <param name="writer">The UTF-8 JSON writer to output the string.</param>
    /// <param name="value">The enum value to serialize.</param>
    /// <param name="options">Options for the JSON serializer.</param>
    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString());
    #endregion

    #region Implementation Methods
    private TEnum ParseString(string value)
    {
        if (Enum.TryParse<TEnum>(value, true, out var enumeration))
        {
            return enumeration;
        }

        throw new JsonException($"Unable to parse enum value '{value}' for enum {{Type={this.Type.Name}}}");
    }
    #endregion
}
