// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
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