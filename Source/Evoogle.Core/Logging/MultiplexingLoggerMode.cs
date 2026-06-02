// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Diagnostics;

using Microsoft.Extensions.Logging;

namespace Evoogle.Logging;

/// <summary>
///     Specifies one or more log destinations for a <see cref="MultiplexingLogger"/>.
///     Multiple values can be combined using bitwise OR.
/// </summary>
[Flags]
public enum MultiplexingLoggerMode
{
    /// <summary>
    ///     No logging is performed.
    /// </summary>
    None = 0,

    /// <summary>
    ///     Log to the provided <see cref="ILogger"/> instance, if available.
    /// </summary>
    Logger = 1 << 0,

    /// <summary>
    ///     Log to <see cref="Debug.WriteLine(string)"/> if a debugger is attached.
    /// </summary>
    Debug = 1 << 1,

    /// <summary>
    ///     Log to <see cref="Console.WriteLine(string)"/>, regardless of debugger presence.
    /// </summary>
    Console = 1 << 2,

    /// <summary>
    ///     Log to all supported destinations: <c>Logger | Debug | Console</c>.
    /// </summary>
    All = Logger | Debug | Console
}
