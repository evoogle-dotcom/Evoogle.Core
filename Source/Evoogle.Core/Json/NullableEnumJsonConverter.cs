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
///     Converts nullable enum values to and from their JSON string representations using a
///     configured policy for invalid JSON values.
/// </summary>
/// <typeparam name="TEnum">The enum type to convert.</typeparam>
public sealed class NullableEnumJsonConverter<TEnum> : JsonConverter<TEnum?>
    where TEnum : struct, Enum
{
    #region Fields
    private readonly EnumJsonInvalidValuePolicy _invalidValuePolicy;
    #endregion

    #region Constructors
    /// <summary>
    ///     Initializes a new instance of the <see cref="NullableEnumJsonConverter{TEnum}"/> class.
    /// </summary>
    /// <param name="invalidValuePolicy">
    ///     The policy to apply when a JSON value cannot be converted to <typeparamref name="TEnum"/>.
    /// </param>
    public NullableEnumJsonConverter(EnumJsonInvalidValuePolicy invalidValuePolicy)
    {
        if (invalidValuePolicy is not EnumJsonInvalidValuePolicy.Throw and not EnumJsonInvalidValuePolicy.ReturnNull)
        {
            throw new ArgumentOutOfRangeException(nameof(invalidValuePolicy));
        }

        _invalidValuePolicy = invalidValuePolicy;
    }
    #endregion

    #region JsonConverter Methods
    /// <inheritdoc/>
    public override bool HandleNull => true;

    /// <inheritdoc/>
    public override TEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();
            if (!string.IsNullOrWhiteSpace(value) &&
                EnumJsonConversion<TEnum>.TryParse(value, out var enumeration, out _))
            {
                return enumeration;
            }

            return this.HandleInvalidValue(value);
        }

        if (reader.TokenType is JsonTokenType.StartArray or JsonTokenType.StartObject)
        {
            reader.Skip();
        }

        return this.HandleInvalidToken(reader.TokenType);
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TEnum? value, JsonSerializerOptions options)
    {
        if (value is TEnum enumeration)
        {
            writer.WriteStringValue(EnumJsonConversion<TEnum>.Format(enumeration));
            return;
        }

        writer.WriteNullValue();
    }
    #endregion

    #region Implementation Methods
    private TEnum? HandleInvalidToken(JsonTokenType tokenType)
    {
        if (_invalidValuePolicy == EnumJsonInvalidValuePolicy.ReturnNull)
        {
            return null;
        }

        throw new JsonException($"JSON token '{tokenType}' cannot be converted to enum {{Type={typeof(TEnum).Name}}}");
    }

    private TEnum? HandleInvalidValue(string? value)
    {
        if (_invalidValuePolicy == EnumJsonInvalidValuePolicy.ReturnNull)
        {
            return null;
        }

        throw new JsonException($"Invalid enum value '{value}' for enum {{Type={typeof(TEnum).Name}}}");
    }
    #endregion
}
