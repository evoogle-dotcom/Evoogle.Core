// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;

using Evoogle.Extensions;

namespace Evoogle.Json;

/// <summary>
///     Base class for JSON converters providing common read functionality.
/// </summary>
public abstract partial class JsonConverterBase<T>
{
    #region Types
    /// <summary>
    ///     Delegate that handles reading the current JSON value.
    ///     Implementations must fully consume the current value (scalar/object/array),
    ///     leaving the reader positioned at the last token of that value so caller loops
    ///     can continue correctly.
    /// </summary>
    /// <typeparam name="TContext">The concrete context type, implementing <see cref="IReadContext"/>.</typeparam>
    /// <param name="reader">The JSON reader positioned at the start of the value to consume.</param>
    /// <param name="context">The read context.</param>
    protected delegate void JsonReaderHandler<TContext>(ref Utf8JsonReader reader, TContext context);
    #endregion

    #region Deserialize Methods
    /// <summary>
    ///     Deserializes a property value of the specified type from the JSON reader, throwing
    ///     an exception if the value is null or if deserialization fails.
    /// </summary>
    /// <typeparam name="TOut">The type to deserialize.</typeparam>
    /// <param name="reader">The JSON reader.</param>
    /// <param name="options">The JSON serializer options.</param>
    /// <param name="propertyName">The name of the property being deserialized (for error messages).</param>
    /// <returns>The deserialized value.</returns>
    /// <exception cref="JsonException">
    ///     Thrown if the deserialized value is null or if deserialization fails.
    /// </exception>
    protected static TOut DeserializeOrThrow<TOut>(ref Utf8JsonReader reader, JsonSerializerOptions options, string propertyName)
    {
        try
        {
            var value = JsonSerializer.Deserialize<TOut>(ref reader, options) ?? throw new JsonException($"Failed to deserialize property '{propertyName}': null value.");
            return value;
        }
        catch (JsonException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new JsonException($"Failed to deserialize property '{propertyName}'.", ex);
        }
    }

    /// <summary>
    ///     Deserializes a list of elements from the JSON reader.
    /// </summary>
    /// <typeparam name="TOut">The type of the elements.</typeparam>
    /// <param name="reader">The JSON reader.</param>
    /// <param name="options">The JSON serializer options.</param>
    /// <param name="propertyName">The name of the property being deserialized (for error messages).</param>
    /// <returns>A list of deserialized elements.</returns>
    /// <exception cref="JsonException">
    ///     Thrown if deserialization fails.
    /// </exception>
    protected static List<TOut> DeserializeListOf<TOut>(ref Utf8JsonReader reader, JsonSerializerOptions options, string propertyName)
    {
        var list = DeserializeOrThrow<List<TOut>>(ref reader, options, propertyName);
        return list;
    }

    /// <summary>
    ///     Deserializes a list of derived type elements from the JSON reader, ensuring
    ///     that each element is of the expected derived type.
    /// </summary>
    /// <typeparam name="TBase">The base type of the elements.</typeparam>
    /// <typeparam name="TDerived">The derived type of the elements.</typeparam>
    /// <param name="reader">The JSON reader.</param>
    /// <param name="options">The JSON serializer options.</param>
    /// <param name="propertyName">The name of the property being deserialized (for error messages).</param>
    /// <returns>A list of elements of the derived type.</returns>
    /// <exception cref="JsonException">
    ///     Thrown if any element in the deserialized list is not of the expected derived type.
    /// </exception>
    protected static List<TDerived> DeserializeListOf<TBase, TDerived>(ref Utf8JsonReader reader, JsonSerializerOptions options, string propertyName)
        where TDerived : TBase
    {
        var list = DeserializeOrThrow<List<TBase>>(ref reader, options, propertyName);

        var result = new List<TDerived>(list.Count);
        for (var i = 0; i < list.Count; i++)
        {
            var item = list[i];
            if (item is TDerived ok)
            {
                result.Add(ok);
                continue;
            }

            var actualType = item?.GetType().SafeToName();
            throw new JsonException($"Property '{propertyName}' contains an element at index {i} that is not a {typeof(TDerived).Name}. Actual: {actualType}.");
        }
        return result;
    }
    #endregion

    #region Read Methods
    /// <summary>
    ///     Reads a JSON array from the current reader position and invokes a handler for each element.
    ///     The reader must be positioned on <see cref="JsonTokenType.StartArray"/> when called.
    /// </summary>
    /// <typeparam name="TContext">The concrete context type, implementing <see cref="IReadContext"/>.</typeparam>
    /// <param name="reader">The JSON reader.</param>
    /// <param name="context">The read context.</param>
    /// <param name="arrayElementHandlerAccessor">
    ///     Function that provides an element handler for the given context.
    ///     The handler must fully consume each element so the loop can advance correctly.
    /// </param>
    /// <exception cref="JsonException">Thrown if the reader is not on <see cref="JsonTokenType.StartArray"/>.</exception>
    protected static void ReadJsonArray<TContext>
    (
        ref Utf8JsonReader reader,
        TContext context,
        Func<TContext, JsonReaderHandler<TContext>> arrayElementHandlerAccessor
    )
        where TContext : IReadContext
    {
        // Ensure we are at the start of an array.
        // If not, throw an exception to indicate that we expected an array.
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected start of an array.");
        }

        var index = -1;
        var handler = arrayElementHandlerAccessor(context);
        while (reader.Read())
        {
            // Check for end of the array.
            // If we reach the end, break out of the loop.
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                break;
            }

            // Increment the index for each element read.
            // This helps in tracking the position of the element in the array.
            ++index;

            // Handle null array elements.
            // If we encounter a null element, log it and continue to the next element.
            if (reader.TokenType == JsonTokenType.Null)
            {
                // Log the skipped null element and continue to the next element.
                // This prevents null elements from causing issues in the deserialization process.
                context.OnReadOfNullArrayItem(index);
                continue;
            }

            // Handle the current array element using the provided handler.
            handler(ref reader, context);
        }
    }

    /// <summary>
    ///     Reads a JSON object from the current reader position, dispatching known properties to handlers and skipping unknown properties.
    ///     The reader must be positioned on the start of a JSON object.
    /// </summary>
    /// <typeparam name="TContext">The concrete context type, implementing <see cref="IReadContext"/>.</typeparam>
    /// <param name="reader">The JSON reader.</param>
    /// <param name="context">The read context.</param>
    /// <param name="handlers">A dictionary mapping property names to their corresponding handlers.</param>
    /// <param name="nullPropertyNames">
    ///     The JSON property names whose null values should be dispatched to their handlers.
    /// </param>
    /// <exception cref="JsonException">Thrown when the JSON is not valid.</exception>
    /// <remarks>
    ///     This method reads a JSON object from the provided <see cref="Utf8JsonReader"/> and processes its properties using the specified handlers.
    ///     It ensures that the reader is positioned at the start of an object and iterates through its properties.
    ///     For each property, it checks if a handler exists in the provided dictionary.
    ///     If a handler is found, it invokes the handler to process the property's value.
    ///     If a property is null, it logs the occurrence using the context's <see cref="IReadContext.OnReadOfNullProperty(string)"/> method.
    ///     If a property is unknown (i.e., no handler exists), it logs the occurrence using the context's <see cref="IReadContext.OnReadOfUnknownProperty(string)"/> method and skips the property's value.
    /// </remarks>
    protected static void ReadJsonObject<TContext>
    (
        ref Utf8JsonReader reader,
        TContext context,
        Dictionary<string, JsonReaderHandler<TContext>> handlers,
        params string[] nullPropertyNames
    )
        where TContext : IReadContext
    {
        // Ensure we are at the start of an object.
        // If not, throw an exception to indicate that we expected an object.
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of an object.");
        }

        while (reader.Read())
        {
            // Check for end of the object.
            // If we reach the end, break out of the loop.
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            // If we are not at a property name, throw an exception.
            // This ensures we are reading a valid JSON object.
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected object property name.");
            }

            // Read the property name.
            var propertyName = reader.GetString()!;

            // Read the property value.
            var readSuccess = reader.Read();
            if (!readSuccess)
            {
                throw new JsonException($"Failed to read value for property '{propertyName}'.");
            }

            // Handle null property values.
            if (reader.TokenType == JsonTokenType.Null)
            {
                if (nullPropertyNames is not null
                    && Array.IndexOf(nullPropertyNames, propertyName) >= 0
                    && handlers.TryGetValue(propertyName, out var nullHandler))
                {
                    nullHandler(ref reader, context);
                    continue;
                }

                // Log the skipped null property and continue to the next property.
                context.OnReadOfNullProperty(propertyName);
                continue;
            }

            // Check if we have a handler for this property value.
            // If not, skip it and log a warning.
            if (handlers.TryGetValue(propertyName, out var handler))
            {
                // Handle the property value using the corresponding handler.
                handler(ref reader, context);
            }
            else
            {
                // Log a warning for the skipped property.
                // This helps in identifying properties that are not handled by the deserialization logic.
                context.OnReadOfUnknownProperty(propertyName);
                reader.Skip();
            }
        }
    }
    #endregion
}
