// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.Extension;

/// <summary>
///     Abstracts an object that supports attaching and retrieving extensions by type.
/// </summary>
public interface IExtensible : IReadOnlyExtensible
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
    #endregion
}
