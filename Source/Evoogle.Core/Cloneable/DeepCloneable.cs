// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.Cloneable;

/// <summary>
///     Base class for creating a deep clone of itself by JSON serialization/deserialization.
/// </summary>
/// <typeparam name="T">
///     Actual concrete type of object to create a deep clone of by JSON serialization/deserialization.
///     Typically this will be the actual concrete class inheriting from DeepCloneable and passing itself as T.
/// </typeparam>
public abstract class DeepCloneable<T> : IDeepCloneable
{
    #region IDeepCloneable Implementation
    /// <summary>
    ///     Creates a new object that is a deep clone of the current object by JSON serialization/deserialization.
    /// </summary>
    /// <returns>
    ///     A new object that is a deep clone of this instance.
    /// </returns>
    public object? DeepClone()
    {
        var sourceType = typeof(T);
        return this.DeepCopyWithJson(sourceType);
    }
    #endregion
}