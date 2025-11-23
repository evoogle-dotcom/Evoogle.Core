// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Collections.Concurrent;
using System.Text.Json;

using Evoogle.Extension;
using Evoogle.Extensions;
using Evoogle.Logging;

using Microsoft.Extensions.Logging;

namespace Evoogle.Json;

/// <summary>
///     Partial <see cref="JsonConverterBase{T}"/> that provides helpers and lifecycle hooks
///     for reading and writing attached extension objects on extensible types.
/// </summary>
/// <typeparam name="T">The CLR type being converted.</typeparam>
public abstract partial class JsonConverterBase<T>
{
    #region Types
    /// <summary>
    ///     Strongly-typed bag of JSON property names used by extensible types.
    /// </summary>
    protected readonly record struct ExtensibleBasePropertyNames
    {
        #region Properties
        /// <summary>
        ///     The JSON property name under which extensions are serialized.
        /// </summary>
        public required string Extensions { get; init; }
        #endregion
    }

    /// <summary>
    ///     Transient data captured while deserializing extensible types.
    /// </summary>
    protected class ExtensibleReadData
    {
        #region Properties
        /// <summary>
        ///     The raw extension objects, keyed by their serialized type name.
        /// </summary>
        public Dictionary<string, object>? Extensions { get; set; }
        #endregion
    }
    #endregion

    #region Fields
    /// <summary>
    ///    Cache of <see cref="ExtensibleBasePropertyNames"/> instances keyed by <see cref="JsonNamingPolicy"/>.
    /// </summary>
    private static readonly ConcurrentDictionary<JsonNamingPolicy, ExtensibleBasePropertyNames> _extensiblePropertyNamesCache = new();
    #endregion

    #region JsonConverterBase<T> Methods
    protected static JsonReaderHandler<DefaultReadContext<TPropertyNames, TReadData, TReadHandlers>> CreateExtensionsHandler<TPropertyNames, TReadData, TReadHandlers>()
        where TReadData : ExtensibleReadData, new()
    {
        return (ref Utf8JsonReader reader, DefaultReadContext<TPropertyNames, TReadData, TReadHandlers> context)
            => context.ReadData.Extensions = ReadJsonExtensionsObject(ref reader, context);
    }

    /// <summary>
    ///     Gets the cached <see cref="ExtensibleBasePropertyNames"/> for the specified <paramref name="policy"/>.
    /// </summary>
    protected static ExtensibleBasePropertyNames GetExtensiblePropertyNames(JsonNamingPolicy policy)
       => _extensiblePropertyNamesCache.GetOrAdd(policy, p => new ExtensibleBasePropertyNames
       {
           Extensions = p.ConvertName(nameof(ExtensibleBase.Extensions))
       });

    /// <summary>
    ///     Called just before an extension instance is deserialized.
    ///     Override to add tracing or to enforce policies.
    /// </summary>
    /// <param name="extensionType">The runtime type that will be deserialized for the extension.</param>
    protected virtual void OnDeserializingExtensionType(Type? extensionType)
    {
        if (this.Logger.IsEnabled(LogLevel.Trace))
        {
            this.LogTrace("Deserializing extension type: {ExtensionType}", extensionType.SafeToName());
        }
    }

    /// <summary>
    ///     Called right after an extension instance has been deserialized.
    /// </summary>
    /// <param name="extensionType">The runtime type that was deserialized.</param>
    protected virtual void OnDeserializedExtensionType(Type? extensionType)
    {
        if (this.Logger.IsEnabled(LogLevel.Debug))
        {
            this.LogDebug("Deserialized  extension type: {ExtensionType}", extensionType.SafeToName());
        }
    }

    /// <summary>
    ///     Called just before an extension instance is serialized.
    /// </summary>
    /// <param name="extensionType">The runtime type that will be serialized.</param>
    protected virtual void OnSerializingExtensionType(Type? extensionType)
    {
        if (this.Logger.IsEnabled(LogLevel.Trace))
        {
            this.LogTrace("Serializing extension type: {ExtensionType}", extensionType.SafeToName());
        }
    }

    /// <summary>
    ///     Called right after an extension instance has been serialized.
    /// </summary>
    /// <param name="extensionType">The runtime type that was serialized.</param>
    protected virtual void OnSerializedExtensionType(Type? extensionType)
    {
        if (this.Logger.IsEnabled(LogLevel.Debug))
        {
            this.LogDebug("Serialized  extension type: {ExtensionType}", extensionType.SafeToName());
        }
    }
    #endregion

    #region Read Methods
    /// <summary>
    ///     Reads an extensions object from the current <see cref="Utf8JsonReader"/> position.
    ///     The reader must be positioned on <see cref="JsonTokenType.StartObject"/>.
    ///     Each property is treated as a concrete extension type name and its value as the extension payload.
    /// </summary>
    /// <typeparam name="TContext">The context type that implements <see cref="IReadContext"/>.</typeparam>
    /// <param name="reader">The JSON reader, positioned at the start of the extensions object.</param>
    /// <param name="context">The read context providing options and logging.</param>
    /// <returns>
    ///     A dictionary containing the deserialized extensions keyed by their serialized type name.
    /// </returns>
    /// <exception cref="JsonException">
    ///     Thrown when the current token is not <see cref="JsonTokenType.StartObject"/>,
    ///     when a property value cannot be read, or when an extension cannot be deserialized.
    /// </exception>
    protected static Dictionary<string, object> ReadJsonExtensionsObject<TContext>
    (
        ref Utf8JsonReader reader,
        TContext context
    )
        where TContext : IReadContext
    {
        // Ensure we are at the start of an object.
        // If not, throw an exception to indicate that we expected an object.
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of an object.");
        }

        var extensions = new Dictionary<string, object>();
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

            // Read the property name which represents the extension type name.
            var typeName = reader.GetString()!;
            var extensionType = TypeJsonConverter.GetDeserializeType(typeName);

            // this.OnDeserializingExtensionType(extensionType);

            // Read the property value.
            var readSuccess = reader.Read();
            if (!readSuccess)
            {
                throw new JsonException($"Failed to read value for property '{typeName}'.");
            }

            // Deserialize the extension object using the determined extension type.
            var options = context.Options;
            var extension = JsonSerializer.Deserialize(ref reader, extensionType, options) ?? throw new JsonException($"Failed to deserialize {typeName}.");

            // this.OnDeserializedExtensionType(extensionType);

            extensions.Add(typeName, extension);
        }

        return extensions;
    }

    /// <summary>
    ///     Attaches a set of deserialized extensions to the specified <paramref name="extensibleBase"/>.
    ///     Validates that each extension instance is assignable to its declared type name.
    /// </summary>
    /// <param name="extensibleBase">The extensible target to attach to.</param>
    /// <param name="extensions">The extension instances keyed by serialized type name; may be <c>null</c>.</param>
    /// <exception cref="JsonException">
    ///     Thrown when an extension instance is not assignable to the resolved runtime type for its key.
    /// </exception>
    protected static void AttachExtensions
    (
        ExtensibleBase extensibleBase,
        Dictionary<string, object>? extensions
    )
    {
        if (extensions == null)
        {
            return;
        }

        foreach ((var typeName, var extension) in extensions)
        {
            var extensionType = TypeJsonConverter.GetDeserializeType(typeName);
            if (extension is null || !extensionType.IsInstanceOfType(extension))
            {
                throw new JsonException($"Extension '{typeName}' is not assignable to {extensionType.SafeToName()}.");
            }

            extensibleBase.AttachExtension(extensionType, extension);
        }
    }
    #endregion

    #region Write Methods
    /// <summary>
    ///     Writes an extensions object to <paramref name="writer"/> using the provided
    ///     ordered mapping of extension types to extension instances.
    ///     Produces a JSON object where each property name is the serialized type name
    ///     and the property value is the serialized extension payload.
    /// </summary>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="extensions">An ordered dictionary of extension types and instances.</param>
    /// <param name="context">The write context providing options and logging.</param>
    protected static void WriteExtensions
    (
        Utf8JsonWriter writer,
        OrderedDictionary<Type, object> extensions,
        IWriteContext context
    )
    {
        writer.WriteStartObject();

        foreach (var (extensionType, extension) in extensions)
        {
            var typeName = TypeJsonConverter.GetSerializeTypeName(extensionType);

            // this.OnSerializingExtensionType(extensionType);

            writer.WritePropertyName(typeName);

            var options = context.Options;
            JsonSerializer.Serialize(writer, extension, extensionType, options);

            // this.OnSerializedExtensionType(extensionType);
        }

        writer.WriteEndObject();
    }

    /// <summary>
    ///     Writes the <paramref name="extensibleBase"/> extensions under the specified
    ///     JSON <paramref name="propertyName"/>, if any are present.
    /// </summary>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="propertyName">The JSON property name that will contain the extensions object.</param>
    /// <param name="extensibleBase">The extensible instance that may hold extensions.</param>
    /// <param name="context">The write context providing options and logging.</param>
    protected static void WriteExtensibleBaseExtensions
    (
        Utf8JsonWriter writer,
        string propertyName,
        ExtensibleBase extensibleBase,
        IWriteContext context
    )
    {
        var extensions = extensibleBase.Extensions;
        if (extensions is { Count: > 0 })
        {
            writer.WritePropertyName(propertyName);
            WriteExtensions(writer, extensions, context);
        }
    }
    #endregion
}
