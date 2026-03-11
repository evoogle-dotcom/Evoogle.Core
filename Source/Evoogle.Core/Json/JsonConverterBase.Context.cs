// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;

using Evoogle.Logging;

using Microsoft.Extensions.Logging;

namespace Evoogle.Json;

/// <summary>
///     Partial <see cref="JsonConverterBase{T}"/> that provides helpers for converter contexts.
/// </summary>
public abstract partial class JsonConverterBase<T>
{
    #region Types
    /// <summary>
    ///     Base contract for converter contexts that flow through read/write operations.
    ///     Provides access to <see cref="JsonSerializerOptions"/> and a logger via <see cref="IHasLogger"/>.
    /// </summary>
    protected interface IContext : IHasLogger
    {
        #region Properties
        /// <summary>
        ///     The serializer options currently in effect for this operation.
        /// </summary>
        JsonSerializerOptions Options { get; }
        #endregion
    }

    /// <summary>
    ///     Marker interface for read contexts.
    /// </summary>
    protected interface IReadContext : IContext
    {
        #region Methods
        /// <summary>
        ///     Called when a null element is encountered within an array while reading.
        ///     Default implementation logs at Trace level.
        /// </summary>
        /// <param name="index">Zero-based index of the null element.</param>
        virtual void OnReadOfNullArrayItem(int index)
        {
            if (this.Logger.IsEnabled(LogLevel.Trace))
            {
                this.LogTrace("Skipping null JSON array item: [{Index}]", index);
            }
        }

        /// <summary>
        ///     Called when a property is present with a null value while reading.
        ///     Default implementation logs at Trace level.
        /// </summary>
        /// <param name="name">The property name.</param>
        virtual void OnReadOfNullProperty(string name)
        {
            if (this.Logger.IsEnabled(LogLevel.Trace))
            {
                this.LogTrace("Skipping null JSON property: '{Name}'", name);
            }
        }

        /// <summary>
        ///     Called when an unknown property is encountered while reading.
        ///     Default implementation logs at Warning level and skips the value.
        /// </summary>
        /// <param name="name">The unrecognized property name.</param>
        virtual void OnReadOfUnknownProperty(string name)
        {
            if (this.Logger.IsEnabled(LogLevel.Warning))
            {
                this.LogWarning("Skipping unknown JSON property: '{Name}'", name);
            }
        }
        #endregion
    }

    /// <summary>
    ///     Marker interface for write contexts.
    /// </summary>
    protected interface IWriteContext : IContext;

    /// <summary>
    ///     Abstract base context providing logger, serializer options, naming policy, and pre-computed property
    ///     names for converter read/write operations.
    /// </summary>
    /// <typeparam name="TPropertyNames">The type that holds the serialized property names for this converter.</typeparam>
    /// <param name="logger">The logger to use during conversion.</param>
    /// <param name="options">The serializer options in effect for this operation.</param>
    /// <param name="propertyNamingPolicy">The naming policy used for property name conversion.</param>
    /// <param name="propertyNames">The pre-computed property names for this converter.</param>
    protected abstract class DefaultContext<TPropertyNames>
    (
        ILogger logger,
        JsonSerializerOptions options,
        JsonNamingPolicy propertyNamingPolicy,
        TPropertyNames propertyNames
    ) : IContext
    {
        /// <inheritdoc />
        public ILogger Logger { get; } = logger;

        /// <inheritdoc />
        public JsonSerializerOptions Options { get; } = options;

        /// <summary>Gets the naming policy used when converting property names.</summary>
        public JsonNamingPolicy PropertyNamingPolicy { get; } = propertyNamingPolicy;

        /// <summary>Gets the pre-computed property names for this converter.</summary>
        public TPropertyNames PropertyNames { get; } = propertyNames;
    }

    /// <summary>
    ///     Default read context that extends <see cref="DefaultContext{TPropertyNames}"/> with read handlers
    ///     and transient read data for accumulating values during deserialization.
    /// </summary>
    /// <typeparam name="TPropertyNames">The type that holds the serialized property names for this converter.</typeparam>
    /// <typeparam name="TReadData">The type used to accumulate data while reading JSON.</typeparam>
    /// <typeparam name="TReadHandlers">The type that holds per-property read handler delegates.</typeparam>
    /// <param name="logger">The logger to use during conversion.</param>
    /// <param name="options">The serializer options in effect for this operation.</param>
    /// <param name="propertyNamingPolicy">The naming policy used for property name conversion.</param>
    /// <param name="propertyNames">The pre-computed property names for this converter.</param>
    /// <param name="readHandlers">The per-property handler delegates used during reading.</param>
    /// <param name="readData">The transient data object populated while reading.</param>
    protected class DefaultReadContext<TPropertyNames, TReadData, TReadHandlers>
    (
        ILogger logger,
        JsonSerializerOptions options,
        JsonNamingPolicy propertyNamingPolicy,
        TPropertyNames propertyNames,
        TReadHandlers readHandlers,
        TReadData readData
    ) : DefaultContext<TPropertyNames>(logger, options, propertyNamingPolicy, propertyNames), IReadContext
    {
        /// <summary>Gets the per-property read handler delegates used during deserialization.</summary>
        public TReadHandlers ReadHandlers { get; } = readHandlers;

        /// <summary>Gets the transient data object that is populated while reading JSON.</summary>
        public TReadData ReadData { get; } = readData;
    }

    /// <summary>
    ///     Default write context that extends <see cref="DefaultContext{TPropertyNames}"/> for write operations.
    /// </summary>
    /// <typeparam name="TPropertyNames">The type that holds the serialized property names for this converter.</typeparam>
    /// <param name="logger">The logger to use during conversion.</param>
    /// <param name="options">The serializer options in effect for this operation.</param>
    /// <param name="propertyNamingPolicy">The naming policy used for property name conversion.</param>
    /// <param name="propertyNames">The pre-computed property names for this converter.</param>
    protected class DefaultWriteContext<TPropertyNames>
    (
        ILogger logger,
        JsonSerializerOptions options,
        JsonNamingPolicy propertyNamingPolicy,
        TPropertyNames propertyNames
    ) : DefaultContext<TPropertyNames>(logger, options, propertyNamingPolicy, propertyNames), IWriteContext
    { }
    #endregion
}
