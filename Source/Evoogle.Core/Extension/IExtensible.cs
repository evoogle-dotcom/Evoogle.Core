// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Diagnostics.CodeAnalysis;

namespace Evoogle.Extension;

/// <summary>
///     Abstracts an object that supports attaching and retrieving extensions by type.
/// </summary>
public interface IExtensible
{
    #region Methods
    /// <summary>
    ///     Attaches an extension object of type T to the implementing class.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    /// <param name="extension">The extension object to attach.</param>
    void AttachExtension<TExtension>(TExtension extension);

    /// <summary>
    ///     Tries to retrieve the attached extension of the specified type.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    /// <param name="extension">When this method returns, contains the attached extension if found; otherwise, null.</param>
    /// <returns>True if the extension is found; otherwise, false.</returns>
    bool TryGetExtension<TExtension>([NotNullWhen(true)] out TExtension? extension)
        where TExtension : class;

    /// <summary>
    ///     Removes an extension of type T from the implementing class.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension to remove.</typeparam>
    void DetachExtension<TExtension>();
    #endregion
}