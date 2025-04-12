// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Text.Json;
using System.Text.Json.Serialization;

using Evoogle.Reflection;

namespace Evoogle.Json;

/// <summary>
///     JSON converter for the <see cref="Type"/>> .NET class.
/// </summary>
public class TypeJsonConverter : JsonConverter<Type>
{
    #region JsonConverter Methods
    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/> method.
    /// </summary>
    public override Type? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var typeName = reader.GetString();
        if (typeName == null)
        {
            throw new JsonException("Unable deserialize .NET type because JSON text was null.");
        }

        var type = DeserializeTypeName(typeName);
        return type;
    }

    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Write(Utf8JsonWriter, T, JsonSerializerOptions)"/> method.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, Type type, JsonSerializerOptions options)
    {
        var typeName = SerializeTypeName(type);
        writer.WriteStringValue(typeName);
    }
    #endregion

    #region Implementation Methods
    /// <summary>
    ///     Deserializes a string representation of a .NET type into its corresponding <see cref="Type"/> object.
    /// </summary>
    /// <param name="typeName">The string representation of the type, typically a compact qualified name (e.g., "System.String").</param>
    /// <returns>The <see cref="Type"/> object corresponding to the provided <paramref name="typeName"/>.</returns>
    /// <exception cref="JsonException">Thrown when the <paramref name="typeName"/> cannot be resolved to a valid .NET type.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="typeName"/> is null.</exception>
    /// <remarks>
    ///     This method uses <see cref="Type.GetType(string, bool)"/> to resolve the type name. The <paramref name="typeName"/>
    ///     should match the format produced by <see cref="SerializeTypeName(Type)"/> to ensure successful roundtrip conversion.
    /// </remarks>    
    public static Type DeserializeTypeName(string typeName)
    {
        var type = Type.GetType(typeName, throwOnError: true) ?? throw new JsonException("Unable deserialize .NET type from incoming parameter {{typeName={typeName}}}.");
        return type;
    }

    /// <summary>
    ///     Serializes a .NET <see cref="Type"/> object into a string representation suitable for roundtrip deserialization.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to serialize into a string.</param>
    /// <returns>A compact qualified name of the <paramref name="type"/> (e.g., "System.String").</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is null.</exception>
    /// <remarks>
    ///     This method relies on <see cref="TypeReflection.GetCompactQualifiedName(Type)"/> to produce a compact, 
    ///     fully-qualified type name that can be deserialized back into the original <see cref="Type"/> using <see cref="DeserializeTypeName(string)"/>.
    /// </remarks>
    public static string SerializeTypeName(Type type)
    {
        var typeCompactQualilfiedName = TypeReflection.GetCompactQualifiedName(type);
        return typeCompactQualilfiedName;
    }
    #endregion
}