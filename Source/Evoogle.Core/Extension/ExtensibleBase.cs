// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Evoogle.Extension;

/// <summary>
///     Base class that implements <see cref="IExtensible"/> abstraction to allow attaching extensions dynamically.
/// </summary>
public abstract class ExtensibleBase : IExtensible
{
    #region Properties
    /// <summary>
    ///     Gets thread-safe dictionary to store extensions keyed by their type.
    /// </summary>
    public ConcurrentDictionary<Type, object> Extensions { get; } = new();
    #endregion

    #region IExtensible Methods
    /// <inheritdoc />
    public void AttachExtension(Type extensionType, object extension)
    {
        ArgumentNullException.ThrowIfNull(extension);

        // Add the extension to the dictionary using its type as the key.
        this.Extensions[extensionType] = extension;
    }

    /// <inheritdoc />
    public object? DetachExtension(Type extensionType)
    {
        // Remove the extension from the dictionary.
        var result = this.Extensions.TryRemove(extensionType, out var extension);
        if (result && extension != null)
        {
            return extension;
        }

        return null;
    }

    /// <inheritdoc />
    public bool TryGetExtension(Type extensionType, [NotNullWhen(true)] out object? extension)
    {
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