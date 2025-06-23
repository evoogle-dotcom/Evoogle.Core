// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Microsoft.Extensions.Logging;

namespace Evoogle.Logging;

/// <summary>
///     Defines a contract for types that expose a typed <see cref="ILogger{T}"/> instance.
/// </summary>
/// <typeparam name="T">
///     The category type for the logger.
///     This is typically the implementing class itself, allowing log entries to be associated with the correct type context.
/// </typeparam>
public interface IHasLogger<T>
{
    #region Properties
    /// <summary>
    ///     Gets the <see cref="ILogger{T}"/> instance associated with the current object.
    /// </summary>
    /// <remarks>
    ///     This allows consuming code to access structured logging scoped to the specified category type <typeparamref name="T"/>.
    /// </remarks>
    ILogger<T> Logger { get; }
    #endregion
}
