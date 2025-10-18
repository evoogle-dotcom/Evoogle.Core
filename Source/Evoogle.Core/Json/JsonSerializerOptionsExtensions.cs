// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;

namespace Evoogle.Json;

/// <summary>
///     Extension methods for .NET <see cref="JsonSerializerOptions"> class.
/// </summary>
public static class JsonSerializerOptionsExtensions
{
    #region Methods
    /// <summary>
    ///    Gets the property naming policy, or a null-object if none is set.
    /// </summary>
    /// <param name="options">The JSON serializer options.</param>
    /// <returns>Non-null property naming policy.</returns>
    public static JsonNamingPolicy GetPropertyNamingPolicy(this JsonSerializerOptions options) => options.PropertyNamingPolicy ?? new NullJsonNamingPolicy();
    #endregion
}
