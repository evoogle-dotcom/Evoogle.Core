// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Evoogle.Json;

/// <summary>
///     JSON converter for the <see cref="Type"/>> .NET class.
/// </summary>
public class TypeJsonConverter : JsonConverter<Type>
{
    #region JsonConverter Overrides
    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/> method.
    /// </summary>
    public override Type? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var typeCompactQualilfiedName = reader.GetString();
        if (typeCompactQualilfiedName == null)
        {
            throw new NullReferenceException("Can not convert JSON to .NET Type object because the JSON text was null instead of the type compact qualified name as expected.");
        }

        var type = Type.GetType(typeCompactQualilfiedName);
        return type;
    }

    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Write(Utf8JsonWriter, T, JsonSerializerOptions)"/> method.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, Type type, JsonSerializerOptions options)
    {
        var typeCompactQualilfiedName = type.GetCompactQualifiedName();
        writer.WriteStringValue(typeCompactQualilfiedName);
    }
    #endregion
}