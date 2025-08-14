// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Diagnostics.CodeAnalysis;

namespace Evoogle.Extension;

/// <summary>
///     Base class that implements <see cref="IExtensible"/> abstraction to allow attaching extensions dynamically.
/// </summary>
public abstract class ExtensibleBase : IExtensible
{
    #region IExtensible Properties
    /// <inheritdoc />
    public long ExtensionCount => this.Extensions != null ? this.Extensions.Count : 0;
    #endregion

    #region Properties
    /// <summary>
    ///     Gets ordered dictionary to store extensions keyed by their type while maintaining their insertion order.
    /// </summary>
    public OrderedDictionary<Type, object>? Extensions { get; set; }
    #endregion

    #region IExtensible Methods
    /// <inheritdoc />
    public void AttachExtension(Type extensionType, object extension)
    {
        ArgumentNullException.ThrowIfNull(extensionType);
        ArgumentNullException.ThrowIfNull(extension);

        // Add the extension to the dictionary using its type as the key.
        this.Extensions ??= [];
        this.Extensions.Add(extensionType, extension);
    }

    /// <inheritdoc />
    public object? DetachExtension(Type extensionType)
    {
        ArgumentNullException.ThrowIfNull(extensionType);

        if (this.Extensions == null)
        {
            return null;
        }

        // Remove the extension from the dictionary.
        var result = this.Extensions.Remove(extensionType, out var extension);
        if (result && extension != null)
        {
            return extension;
        }

        return null;
    }

    /// <inheritdoc />
    public bool TryGetExtension(Type extensionType, [NotNullWhen(true)] out object? extension)
    {
        ArgumentNullException.ThrowIfNull(extensionType);

        if (this.Extensions == null)
        {
            extension = null;
            return false;
        }

        if (this.Extensions.TryGetValue(extensionType, out var ext))
        {
            // Set output to extension object found.
            extension = ext;
            return true;
        }

        // If not found, set output to null.
        extension = null;
        return false;
    }
    #endregion
}
