// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
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
