// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.Extensions;

namespace Evoogle.Cloneable;

/// <summary>
///     Base class for creating a deep clone of itself by JSON serialization/deserialization.
/// </summary>
public abstract class DeepCloneable : IDeepCloneable
{
    #region IDeepCloneable Methods
    /// <summary>
    ///     Creates a new object that is a deep clone of the current object by JSON serialization/deserialization.
    /// </summary>
    /// <returns>
    ///     A new object that is a deep clone of this instance.
    /// </returns>
    public virtual object? DeepClone()
    {
        var sourceType = this.GetType();
        return this.DeepCopy(sourceType);
    }
    #endregion
}

/// <summary>
///     Base class for creating a strongly-typed deep clone of itself using JSON serialization/deserialization.
/// </summary>
/// <typeparam name="T">
///     The type of the object implementing this base class. Typically the derived type itself.
/// </typeparam>
public abstract class DeepCloneable<T> : IDeepCloneable<T>
{
    #region IDeepCloneable Methods
    /// <summary>
    ///     Creates a deep clone of the current object using JSON serialization and deserialization.
    /// </summary>
    /// <returns>
    ///     A new instance of type <typeparamref name="T"/> that is a deep clone of the current object.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the runtime type of the current instance is not assignable to <typeparamref name="T"/>.
    /// </exception>
    object? IDeepCloneable.DeepClone() => this.DeepClone();
    #endregion

    #region IDeepCloneable<T> Methods
    /// <inheritdoc/>
    public virtual T DeepClone()
    {
        if (this is not T)
        {
            throw new InvalidOperationException($"Type mismatch: {this.GetType().Name} is not assignable to {typeof(T).Name}");
        }

        return (T)this.DeepCopy(typeof(T))!;
    }
    #endregion
}
