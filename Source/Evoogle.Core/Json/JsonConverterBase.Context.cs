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
/// <typeparam name="T">The CLR type being converted.</typeparam>
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

    protected abstract class DefaultContext<TPropertyNames>
    (
        ILogger logger,
        JsonSerializerOptions options,
        JsonNamingPolicy propertyNamingPolicy,
        TPropertyNames propertyNames
    ) : IContext
    {
        public ILogger Logger { get; } = logger;
        public JsonSerializerOptions Options { get; } = options;
        public JsonNamingPolicy PropertyNamingPolicy { get; } = propertyNamingPolicy;
        public TPropertyNames PropertyNames { get; } = propertyNames;
    }

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
        public TReadHandlers ReadHandlers { get; } = readHandlers;
        public TReadData ReadData { get; } = readData;
    }

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
