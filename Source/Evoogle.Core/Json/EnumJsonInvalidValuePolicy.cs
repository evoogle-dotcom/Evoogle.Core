// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.Json;

/// <summary>
///     Specifies how <see cref="NullableEnumJsonConverter{TEnum}"/> handles a JSON value that
///     cannot be converted to its enum type.
/// </summary>
public enum EnumJsonInvalidValuePolicy
{
    #region Members
    /// <summary>
    ///     Throws a <see cref="System.Text.Json.JsonException"/> when the JSON value is null or
    ///     cannot be converted to the enum type.
    /// </summary>
    Throw,

    /// <summary>
    ///     Returns <see langword="null"/> when the JSON value is null or cannot be converted to
    ///     the enum type.
    /// </summary>
    ReturnNull
    #endregion
}
