// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;
using System.Text.Json.Serialization;

using Evoogle.Extensions;
using Evoogle.Logging;

using Microsoft.Extensions.Logging;

namespace Evoogle.Json;

/// <summary>
///     Abstract base for <see cref="System.Text.Json"/> converters, providing
///     structured logging, context creation, and helper methods for parsing
///     JSON arrays and objects in a safe, reusable way.
/// </summary>
/// <typeparam name="T">The CLR type handled by this converter.</typeparam>
/// <remarks>
///     Responsibilities:
///     - Centralizes Read/Write lifecycles and logging.
///     - Delegates context-specific logic to <see cref="CreateReadContext"/> / <see cref="CreateWriteContext"/>.
///     - Exposes helpers (<see cref="ReadJsonArray{TContext}"/>, <see cref="ReadJsonObject{TContext}"/>) for token-safe reading of arrays and objects.
/// </remarks>
public abstract partial class JsonConverterBase<T>(ILogger? logger) : JsonConverter<T>, IHasLogger
{
    #region IHasLogger Properties
    /// <summary>
    ///     Logger used by the converter and its contexts. A <see cref="MultiplexingLogger"/> is created
    ///     around the provided <paramref name="logger"/> to ensure consistent logging behavior.
    /// </summary>
    public ILogger Logger { get; } = new MultiplexingLogger(logger, MultiplexingLoggerMode.None);
    #endregion

    #region JsonConverter<T> Methods
    /// <summary>
    ///     Reads JSON into an instance of <typeparamref name="T"/>.
    ///     This method handles null tokens, constructs a read context, and delegates parsing
    ///     to <see cref="ReadCore(ref Utf8JsonReader, IReadContext)"/> followed by
    /// <see cref="CreateValue(IReadContext)"/>.
    /// </summary>
    /// <param name="reader">The JSON reader.</param>
    /// <param name="typeToConvert">The target type to convert to (usually <typeparamref name="T"/>).</param>
    /// <param name="options">Serializer options.</param>
    /// <returns>An instance of <typeparamref name="T"/> or null.</returns>
    public override sealed T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (this.Logger.IsEnabled(LogLevel.Trace))
        {
            this.LogTrace("Deserializing {Value}", typeof(T).SafeToName());
        }

        var value = default(T?);
        try
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                value = default;
                return value;
            }

            var context = this.CreateReadContext(this.Logger, options);
            this.ReadCore(ref reader, context);

            value = this.CreateValue(context);
        }
        finally
        {
            if (this.Logger.IsEnabled(LogLevel.Debug))
            {
                this.LogDebug("Deserialized  {Value}", value.SafeToString());
            }
        }
        return value;
    }

    /// <summary>
    ///     Writes an instance of <typeparamref name="T"/> to JSON.
    ///     This method constructs a write context and delegates to <see cref="WriteCore(Utf8JsonWriter, T, IWriteContext)"/>.
    /// </summary>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">Serializer options.</param>
    public override sealed void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (this.Logger.IsEnabled(LogLevel.Trace))
        {
            this.LogTrace("Serializing {Value}", value.SafeToString());
        }

        var context = this.CreateWriteContext(this.Logger, options);
        this.WriteCore(writer, value, context);

        if (this.Logger.IsEnabled(LogLevel.Debug))
        {
            this.LogDebug("Serialized  {Value}", value.SafeToString());
        }
    }
    #endregion

    #region JsonConverterBase<T> Methods
    /// <summary>
    ///     Creates the read context for this conversion pass.
    /// </summary>
    /// <param name="logger">The logger to associate with the context.</param>
    /// <param name="options">Serializer options.</param>
    /// <returns>An <see cref="IReadContext"/> implementation used by <see cref="ReadCore(ref Utf8JsonReader, IReadContext)"/>.</returns>
    protected abstract IReadContext CreateReadContext(ILogger logger, JsonSerializerOptions options);

    /// <summary>
    ///     Produces the final <typeparamref name="T"/> instance from data accumulated in <paramref name="context"/>.
    ///     Called after <see cref="ReadCore(ref Utf8JsonReader, IReadContext)"/> completes.
    /// </summary>
    /// <param name="context">The read context containing parsed data.</param>
    /// <returns>The created instance, or null.</returns>
    protected abstract T? CreateValue(IReadContext context);

    /// <summary>
    ///    Creates the write context for this conversion pass.
    /// </summary>
    /// <param name="logger">The logger to associate with the context.</param>
    /// <param name="options">Serializer options.</param>
    /// <returns>An <see cref="IWriteContext"/> implementation used by <see cref="WriteCore(Utf8JsonWriter, T, IWriteContext)"/>.</returns>
    protected abstract IWriteContext CreateWriteContext(ILogger logger, JsonSerializerOptions options);

    /// <summary>
    ///     Performs the core read logic. Implementations should parse the current JSON value,
    ///     populate fields in <paramref name="context"/>, and leave the reader positioned at
    ///     the last token of the consumed value.
    /// </summary>
    /// <param name="reader">The JSON reader positioned at the value to read.</param>
    /// <param name="context">The read context to populate.</param>
    protected abstract void ReadCore(ref Utf8JsonReader reader, IReadContext context);

    /// <summary>
    ///     Performs the core write logic. Implementations should write the JSON representation
    ///     of <paramref name="value"/> using information in <paramref name="context"/>.
    /// </summary>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="context">The write context providing options and logging.</param>
    protected abstract void WriteCore(Utf8JsonWriter writer, T value, IWriteContext context);
    #endregion
}
