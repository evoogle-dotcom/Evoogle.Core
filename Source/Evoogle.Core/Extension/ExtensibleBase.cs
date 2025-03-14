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
    private ConcurrentDictionary<Type, object> Extensions { get; } = new();
    #endregion

    #region IExtensible Methods
    /// <summary>
    ///     Attaches an extension to the object.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    /// <param name="extension">The extension instance to attach.</param>
    /// <exception cref="ArgumentNullException">Thrown if the extension is null.</exception>
    public void AttachExtension<TExtension>(TExtension extension)
    {
        ArgumentNullException.ThrowIfNull(extension);

        // Store the extension in the dictionary using its type as the key.
        var key = typeof(TExtension);
        this.Extensions[key] = extension;
    }

    /// <summary>
    ///     Tries to retrieve the attached extension of the specified type.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension to retrieve.</typeparam>
    /// <param name="extension">When this method returns, contains the attached extension if found; otherwise, null.</param>
    /// <returns>True if the extension is found; otherwise, false.</returns>
    public bool TryGetExtension<TExtension>([NotNullWhen(true)] out TExtension? extension)
        where TExtension : class
    {
        var key = typeof(TExtension);
        if (this.Extensions.TryGetValue(key, out var ext))
        {
            // Set output to extension object found.
            extension = (TExtension)ext;
            return true;
        }

        // If not found, set output to null.
        extension = null;
        return false;
    }

    /// <summary>
    ///     Detaches the extension of the specified type.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension to detach.</typeparam>
    public void DetachExtension<TExtension>()
    {
        // Remove the extension from the dictionary.
        var key = typeof(TExtension);
        this.Extensions.TryRemove(key, out _);
    }
    #endregion
}