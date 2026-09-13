// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.Json;

/// <summary>
///     Specifies how <see cref="Utf8JsonReaderExtensions.ReadObjectPropertyNames"/> handles null
///     property values.
/// </summary>
public enum JsonReaderNullPropertyHandling
{
    #region Members
    /// <summary>
    ///     Accepts null property values and includes the property name in the result.
    /// </summary>
    Allow,

    /// <summary>
    ///     Ignores null property values and omits the property name from the result.
    /// </summary>
    Ignore,

    /// <summary>
    ///     Rejects null property values by throwing a <see cref="System.Text.Json.JsonException"/>.
    /// </summary>
    Throw,
    #endregion
}
