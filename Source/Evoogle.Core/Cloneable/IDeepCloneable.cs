// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.Cloneable;

/// <summary>
///     Abstracts any object that can create a deep clone of itself.
/// </summary>
public interface IDeepCloneable
{
    #region Methods
    /// <summary>
    ///     Creates a new object that is a deep clone of the current object.
    /// </summary>
    /// <returns>
    ///     A new object that is a deep clone of this instance.
    /// </returns>
    object? DeepClone();
    #endregion
}
