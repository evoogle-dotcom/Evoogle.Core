// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Microsoft.Extensions.Logging;

namespace Evoogle.Logging;

/// <summary>
///     Defines a contract for types that expose a non-null <see cref="ILogger"/> instance.
/// </summary>
public interface IHasLogger
{
    #region Properties
    /// <summary>
    ///     Gets the non-null <see cref="ILogger"/> instance associated with the current object.
    /// </summary>
    ILogger Logger { get; }
    #endregion
}
