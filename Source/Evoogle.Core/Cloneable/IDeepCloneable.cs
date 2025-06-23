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

/// <summary>
///     Represents a contract for deep cloning an object and returning a strongly-typed result.
/// </summary>
/// <typeparam name="T">
///     The type of object returned from the <see cref="DeepClone"/> method.
/// </typeparam>
public interface IDeepCloneable<out T> : IDeepCloneable
{
    #region Methods
    /// <summary>
    ///     Creates a new object that is a deep clone of the current object.
    /// </summary>
    /// <returns>
    ///     A new instance of type <typeparamref name="T"/> that is a deep clone of this instance.
    /// </returns>
    new T DeepClone();
    #endregion
}
