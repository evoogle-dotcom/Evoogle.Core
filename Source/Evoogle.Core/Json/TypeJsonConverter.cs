// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;
using System.Text.Json.Serialization;

using Evoogle.Reflection;

namespace Evoogle.Json;

/// <summary>
///     A custom JSON converter for serializing and deserializing .NET <see cref="Type"/> objects.
///     This converter allows for the conversion of <see cref="Type"/> instances to and from JSON strings, using a compact, fully-qualified type name representation.
/// </summary>
public class TypeJsonConverter : JsonConverter<Type>
{
    #region JsonConverter Methods
    /// <summary>
    ///     Deserializes a JSON string into a .NET <see cref="Type"/> object.
    /// </summary>
    /// <param name="reader">The UTF-8 JSON reader providing the string value.</param>
    /// <param name="typeToConvert">The type of object to convert to (in this case, <see cref="Type"/>).</param>
    /// <param name="options">Options for the JSON serializer.</param>
    /// <returns>The deserialized <see cref="Type"/> object.</returns>
    /// <exception cref="JsonException">Thrown when the JSON string is null or cannot be resolved to a valid .NET type.</exception>
    public override Type? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var typeName = reader.GetString();
        if (typeName == null)
        {
            throw new JsonException("Unable deserialize .NET type because JSON text was null.");
        }

        var type = GetDeserializeType(typeName);
        return type;
    }

    /// <summary>
    ///     Serializes a .NET <see cref="Type"/> object into a JSON string.
    /// </summary>
    /// <param name="writer">The UTF-8 JSON writer to output the string.</param>
    /// <param name="type">The <see cref="Type"/> object to serialize.</param>
    /// <param name="options">Options for the JSON serializer.</param>
    public override void Write(Utf8JsonWriter writer, Type type, JsonSerializerOptions options)
    {
        var typeName = GetSerializeTypeName(type);
        writer.WriteStringValue(typeName);
    }
    #endregion

    #region Utility Methods
    /// <summary>
    ///     Deserializes a string representation of a .NET type into its corresponding <see cref="Type"/> object.
    /// </summary>
    /// <param name="typeName">The string representation of the type, typically a compact qualified name (e.g., "System.String").</param>
    /// <returns>The <see cref="Type"/> object corresponding to the provided <paramref name="typeName"/>.</returns>
    /// <exception cref="JsonException">Thrown when the <paramref name="typeName"/> cannot be resolved to a valid .NET type.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="typeName"/> is null (handled internally by <see cref="Type.GetType"/>).</exception>
    /// <remarks>
    ///     This method uses <see cref="Type.GetType(string, bool)"/> to resolve the type name. The <paramref name="typeName"/>
    ///     should match the format produced by <see cref="GetSerializeTypeName(Type)"/> to ensure successful roundtrip conversion.
    /// </remarks>    
    public static Type GetDeserializeType(string typeName)
    {
        try
        {
            var type = Type.GetType(typeName, throwOnError: true) ?? throw new JsonException("Unable deserialize .NET type from incoming parameter {{typeName={typeName}}}.");
            return type;
        }
        catch (Exception exception)
        {
            throw new JsonException("Unable deserialize .NET type from incoming parameter {{typeName={typeName}}}.", exception);
        }
    }

    /// <summary>
    ///     Serializes a .NET <see cref="Type"/> object into a string representation suitable for roundtrip deserialization.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to serialize into a string.</param>
    /// <returns>A compact qualified name of the <paramref name="type"/> (e.g., "System.String").</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is null.</exception>
    /// <remarks>
    ///     This method relies on <see cref="TypeReflection.GetCompactQualifiedName(Type)"/> to produce a compact,
    ///     fully-qualified type name that can be deserialized back into the original <see cref="Type"/> using <see cref="GetDeserializeType(string)"/>.
    /// </remarks>
    public static string GetSerializeTypeName(Type type)
    {
        var typeCompactQualilfiedName = TypeReflection.GetCompactQualifiedName(type);
        return typeCompactQualilfiedName;
    }
    #endregion
}
