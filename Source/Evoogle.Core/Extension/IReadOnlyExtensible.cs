// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Diagnostics.CodeAnalysis;

namespace Evoogle.Extension;

/// <summary>
///     Abstracts read-only access to extensions attached by type.
/// </summary>
public interface IReadOnlyExtensible
{
    #region Properties
    /// <summary>
    ///     Gets the extension count for the extensible object.
    /// </summary>
    long ExtensionCount { get; }

    /// <summary>
    ///     Gets the attached extensions in deterministic insertion order.
    /// </summary>
    IReadOnlyDictionary<Type, object> Extensions { get; }
    #endregion

    #region Methods
    /// <summary>
    ///     Tries to retrieve the attached extension object of the specified extension type.
    /// </summary>
    /// <param name="extensionType">The type of extension to retrieve.</param>
    /// <param name="extension">When this method returns, contains the attached extension object if found; otherwise, null.</param>
    /// <returns>True if the extension is found; otherwise, false.</returns>
    bool TryGetExtension(Type extensionType, [NotNullWhen(true)] out object? extension);
    #endregion
}
