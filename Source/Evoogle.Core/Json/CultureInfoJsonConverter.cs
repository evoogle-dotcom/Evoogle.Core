// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Evoogle.Json;

/// <summary>
///     A System.Text.Json converter that serializes and deserializes <see cref="CultureInfo"/> instances using their culture name (for example, "en-US").
/// </summary>
/// <remarks>
///     During deserialization, this converter reads a JSON string and resolves it to a predefined culture via <see cref="CultureInfo.GetCultureInfo(string, bool)"/>.
///     Empty or whitespace input results in <c>null</c>.
///     During serialization, it writes the <see cref="CultureInfo.Name"/> to JSON.
/// </remarks>
public class CultureInfoJsonConverter : JsonConverter<CultureInfo>
{
    #region JsonConverter Methods
    /// <summary>
    ///     Reads a JSON string containing a culture name and returns the corresponding predefined <see cref="CultureInfo"/>.
    /// </summary>
    /// <param name="reader">The JSON reader positioned at a string token.</param>
    /// <param name="typeToConvert">The type to convert. This parameter is not used.</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>
    ///     A <see cref="CultureInfo"/> if the input string is non-empty; otherwise, <c>null</c>.
    /// </returns>
    /// <exception cref="CultureNotFoundException">
    ///     Thrown when the culture name is not found among predefined cultures.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the current JSON token does not represent a string.
    /// </exception>
    public override CultureInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var cultureName = reader.GetString();
        if (string.IsNullOrWhiteSpace(cultureName))
        {
            return null;
        }

        try
        {
            var cultureInfo = CultureInfo.GetCultureInfo(cultureName, predefinedOnly: true);
            return cultureInfo;
        }
        catch (CultureNotFoundException cultureNotFoundException)
        {
            throw new JsonException($"The culture '{cultureName}' is not a predefined culture.", cultureNotFoundException);
        }
    }

    /// <summary>
    ///     Writes the culture's <see cref="CultureInfo.Name"/> as a JSON string value.
    /// </summary>
    /// <param name="writer">The JSON writer to write to.</param>
    /// <param name="value">The <see cref="CultureInfo"/> to serialize.</param>
    /// <param name="options">The serializer options.</param>
    public override void Write(Utf8JsonWriter writer, CultureInfo value, JsonSerializerOptions options)
    {
        var cultureName = value.Name;
        writer.WriteStringValue(cultureName);
    }
    #endregion
}
