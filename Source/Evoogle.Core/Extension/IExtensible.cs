// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Diagnostics.CodeAnalysis;

namespace Evoogle.Extension;

/// <summary>
///     Abstracts an object that supports attaching and retrieving extensions by type.
/// </summary>
public interface IExtensible
{
    #region Methods
    /// <summary>
    ///     Attaches an extension object based on the extension type.
    /// </summary>
    /// <param name="extensionType">The type of extension to attach.</param>
    /// <param name="extension">The extension object to attach.</param>
    void AttachExtension(Type extensionType, object extension);

    /// <summary>
    ///     Detaches an extension object based on the extension type.
    /// </summary>
    /// <param name="extensionType">The type of extension to detach.</param>
    /// <returns>Extension object if the extension is detached; otherwise, null.</returns>
    object? DetachExtension(Type extensionType);

    /// <summary>
    ///     Tries to retrieve the attached extension object of the specified extension type.
    /// </summary>
    /// <param name="extensionType">The type of extension to retrieve.</param>
    /// <param name="extension">When this method returns, contains the attached extension object if found; otherwise, null.</param>
    /// <returns>True if the extension is found; otherwise, false.</returns>
    bool TryGetExtension(Type extensionType, [NotNullWhen(true)] out object? extension);
    #endregion
}
