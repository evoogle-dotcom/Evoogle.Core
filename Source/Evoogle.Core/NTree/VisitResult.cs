// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.NTree;

/// <summary>
///     Represents guidance on how to continue visiting the 1-N tree.
/// </summary>
public enum VisitResult
{
    /// <summary>
    ///     Visiting continues.
    /// </summary>
    Continue,

    /// <summary>
    ///     Visiting is done and should stop immediately.
    /// </summary>
    Done
}
