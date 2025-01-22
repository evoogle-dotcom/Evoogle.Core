// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

using Evoogle.Json;

using Xunit.Sdk;

namespace Evoogle.XUnit;

/// <summary>
///     xUnit serializer implementation to serialize/deserialize <see cref="XUnitTest"/> or <see cref="XUnitTestAsync"/> derived unit tests.
///     The implementation relies on derived unit tests being able to be serialized/deserialized in/from JSON in a roundtrip paradigm.
/// </summary>
public class XUnitTestSerializer : IXunitSerializer
{
    #region Properties
    private static JsonSerializerOptions DefaultJsonSerializerOptions { get; } = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        WriteIndented = false,
    };
    #endregion
 
    #region Constructors
    static XUnitTestSerializer()
    {
        DefaultJsonSerializerOptions.Converters.Add(new TypeJsonConverter());
    }
    #endregion

    #region IXunitSerializer Implementation
    /// <summary>
    ///     Deserializes the JSON serialized value back into a concrete XUnit or XUnitTestAsync object.
    /// </summary>
    /// <param name="type">CLR type of the concrete unit test.</param>
    /// <param name="serializedValue">JSON to be deserialized into a concrete XUnit or XUnitTestAsync object.</param>
    /// <returns>Newly created concrete XUnit or XUnitTestAsync object.</returns>
    /// <exception cref="InvalidOperationException">Thrown is the JSON deserialization returns null.</exception>
    public object Deserialize(Type type, string serializedValue)
    {
        var value = JsonSerializer.Deserialize(serializedValue, type, DefaultJsonSerializerOptions) ?? throw new InvalidOperationException("JSON serializer deserialilzed JSON {{SerializedValue={serializedValue}}} to null.");
        return value;
    }

    /// <summary>
    ///     Predicate if the given CLR type is serializable or not. The given CLR type must be a subclass of XUnit or XUnitTestAsync.
    /// </summary>
    /// <param name="type">CLR type to be tested if it is serializable.</param>
    /// <param name="value">CLR object to be tested if it is serializable.</param>
    /// <param name="failureReason">If the CLR object is not serializable, the reason why it is not serializable.</param>
    /// <returns>True if given CLR type and object are serializable, false otherwise.</returns>
    public bool IsSerializable(Type type, object? value, [NotNullWhen(false)] out string? failureReason)
    {
        if (type.IsSubclassOf(typeof(XUnitTest)) || type.IsSubclassOf(typeof(XUnitTestAsync)))
        {
            failureReason = null;
            return true;
        }

        failureReason = $"Can not serialize Type {{name={type.Name}}} because it does not implement the {nameof(XUnitTest)} or {nameof(XUnitTestAsync)} interfaces.";
        return false;
    }

    /// <summary>
    ///     Serializes the CLR object into JSON.
    /// </summary>
    /// <param name="value">CLR object to be serialized.</param>
    /// <returns>JSON representation of the CLR object.</returns>
    public string Serialize(object value)
    {
        var type = value.GetType();
        var serializedValue = JsonSerializer.Serialize(value, type, DefaultJsonSerializerOptions);
        return serializedValue;
    }
    #endregion
}
