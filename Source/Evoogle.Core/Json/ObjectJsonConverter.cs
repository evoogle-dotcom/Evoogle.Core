// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Evoogle.Json;

/// <summary>
///     A <see cref="JsonConverter"/> for <see cref="object"/> that ALWAYS emits and expects a discriminator
///     envelope containing runtime type information.
/// </summary>
/// <remarks>
///     <para>
///         System.Text.Json lacks runtime type information for <see cref="object"/> and would otherwise yield
///         <see cref="JsonElement"/> during deserialization. This converter solves that by always writing an
///         envelope: <c>{ "$type": "AssemblyQualifiedTypeName", "$value": &lt;payload&gt; }</c> and requiring
///         the same format when reading.
///     </para>
///     <para>
///         If the envelope is missing or the <c>$type</c> cannot be resolved, a <see cref="JsonException"/> is
///         thrown. A JSON <c>null</c> token is still read as <c>null</c>.
///     </para>
/// </remarks>
public sealed class ObjectJsonConverter : JsonConverter<object>
{
    #region JsonConverter Methods
    /// <summary>
    ///     Predicate indicating whether this converter can handle the provided <paramref name="typeToConvert"/>.
    /// </summary>
    /// <param name="typeToConvert">The CLR type being queried.</param>
    /// <returns><c>true</c> when <paramref name="typeToConvert"/> is exactly <see cref="object"/>, otherwise <c>false</c>.</returns>
    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(object);

    /// <summary>
    ///     Reads JSON and produces a CLR object; a discriminator envelope is required.
    /// </summary>
    /// <param name="reader">The UTF-8 JSON reader positioned at the value to read.</param>
    /// <param name="typeToConvert">The target CLR type (will always be <see cref="object"/>).</param>
    /// <param name="options">Serializer options currently in scope.</param>
    /// <returns>The deserialized CLR value, or <c>null</c> if the JSON token was <c>null</c>.</returns>
    /// <exception cref="JsonException">Thrown when the envelope is missing or cannot be resolved.</exception>
    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        // Materialize current value so we can inspect it
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        // Envelope: { "$type": "...", "$value": ... } is REQUIRED
        if (root.ValueKind != JsonValueKind.Object ||
            !root.TryGetProperty("$type", out var typeProp) ||
            !root.TryGetProperty("$value", out var valueProp))
        {
            throw new JsonException($"{nameof(ObjectJsonConverter)} requires an envelope with '$type' and '$value'.");
        }

        var typeName = typeProp.GetString();
        if (string.IsNullOrWhiteSpace(typeName))
        {
            throw new JsonException($"{nameof(ObjectJsonConverter)} encountered an empty '$type' discriminator.");
        }

        var targetType = Type.GetType(typeName, throwOnError: false);
        if (targetType is null)
        {
            throw new JsonException($"{nameof(ObjectJsonConverter)} could not resolve type '{typeName}'.");
        }

        return JsonSerializer.Deserialize(valueProp.GetRawText(), targetType, options);
    }

    /// <summary>
    ///     Writes the provided <paramref name="value"/> to JSON using a discriminator envelope containing runtime type information.
    /// </summary>
    /// <param name="writer">The JSON writer to output to.</param>
    /// <param name="value">CLR value being serialized.</param>
    /// <param name="options">Serializer options currently in scope.</param>
    /// <remarks>
    ///     All values are emitted using an envelope with <c>$type</c> (the assembly-qualified name) and <c>$value</c> (the serialized payload).
    ///     For <c>null</c> values the writer emits JSON <c>null</c> before this converter is invoked.
    /// </remarks>
    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        var runtimeType = value.GetType();

        // Envelope with a discriminator for ALL values
        writer.WriteStartObject();
        writer.WriteString("$type", runtimeType.AssemblyQualifiedName);
        writer.WritePropertyName("$value");
        JsonSerializer.Serialize(writer, value, runtimeType, options);
        writer.WriteEndObject();
    }
    #endregion
}
