// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Collections.Concurrent;
using System.Text.Json;

using Microsoft.Extensions.Logging;

namespace Evoogle.Json;

/// <summary>
///     Partial <see cref="JsonConverterBase{T}"/> that provides helpers for caching.
/// </summary>
public abstract partial class JsonConverterBase<T>
{
    #region Types
    private static class Cache<TPropertyNames, TReadHandlers>
    {
        public static readonly ConcurrentDictionary<JsonNamingPolicy, TPropertyNames> PropertyNames = new();
        public static readonly ConcurrentDictionary<JsonNamingPolicy, TReadHandlers> ReadHandlers = new();
    }
    #endregion

    #region Methods
    /// <summary>
    ///   Creates a default read context with cached property names and read handlers.
    /// </summary>
    /// <typeparam name="TPropertyNames">The type representing property names.</typeparam>
    /// <typeparam name="TReadData">The type representing read data.</typeparam>
    /// <typeparam name="TReadHandlers">The type representing read handlers.</typeparam>
    /// <param name="logger">The logger to use.</param>
    /// <param name="options">The JSON serializer options.</param>
    /// <param name="buildPropertyNames">A function to build property names for a given naming policy.</param>
    /// <param name="buildReadHandlers">A function to build read handlers for given property names.</param>
    /// <returns>A new instance of <see cref="DefaultReadContext{TPropertyNames, TReadData, TReadHandlers}"/>.</returns>
    /// <remarks>
    ///     This method uses caching to avoid redundant construction of property names and read handlers.
    /// </remarks>
    protected static DefaultReadContext<TPropertyNames, TReadData, TReadHandlers> CreateDefaultReadContext<TPropertyNames, TReadData, TReadHandlers>
    (
        ILogger logger,
        JsonSerializerOptions options,
        Func<JsonNamingPolicy, TPropertyNames> buildPropertyNames,
        Func<TPropertyNames, TReadHandlers> buildReadHandlers
    )
        where TReadData : new()
    {
        var policy = options.GetPropertyNamingPolicy();
        var names = Cache<TPropertyNames, TReadHandlers>.PropertyNames.GetOrAdd(policy, buildPropertyNames);
        var handlers = Cache<TPropertyNames, TReadHandlers>.ReadHandlers.GetOrAdd(policy, _ => buildReadHandlers(names));
        var readData = new TReadData();
        return new DefaultReadContext<TPropertyNames, TReadData, TReadHandlers>(logger, options, policy, names, handlers, readData);
    }

    /// <summary>
    ///     Creates a default write context with cached property names.
    /// </summary>
    /// <typeparam name="TPropertyNames">The type representing property names.</typeparam>
    /// <param name="logger">The logger to use.</param>
    /// <param name="options">The JSON serializer options.</param>
    /// <param name="buildPropertyNames">A function to build property names for a given naming policy.</param>
    /// <returns>A new instance of <see cref="DefaultWriteContext{TPropertyNames}"/>.</returns>
    /// <remarks>
    ///     This method uses caching to avoid redundant construction of property names.
    /// </remarks>
    protected static DefaultWriteContext<TPropertyNames> CreateDefaultWriteContext<TPropertyNames>
    (
        ILogger logger,
        JsonSerializerOptions options,
        Func<JsonNamingPolicy, TPropertyNames> buildPropertyNames
    )
    {
        var policy = options.GetPropertyNamingPolicy();
        var names = Cache<TPropertyNames, object>.PropertyNames.GetOrAdd(policy, buildPropertyNames);
        return new DefaultWriteContext<TPropertyNames>(logger, options, policy, names);
    }
    #endregion
}
