// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Diagnostics.CodeAnalysis;

namespace Evoogle.Extension;

/// <summary>
///     Extension methods for the <see cref="IExtensible"/> abstraction.
/// </summary>
public static class ExtensibleExtensions
{
    #region Extension Methods
    /// <summary>
    ///     Attaches an extension object based on the extension type.
    /// </summary>
    /// <typeparam name="TExtension">The type of extension to attach.</typeparam>
    /// <param name="extension">The extension object to attach.</param>
    public static void AttachExtension<TExtension>(this IExtensible extensible, TExtension extension)
        where TExtension : class
    {
        var extensionType = typeof(TExtension);
        extensible.AttachExtension(extensionType, extension);
    }

    /// <summary>
    ///     Determines whether the specified extension type is already attached to the extensible object.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension to check.</typeparam>
    /// <param name="extensible">The extensible object.</param>
    /// <returns>True if the extension is attached; otherwise, false.</returns>
    public static bool ContainsExtension<TExtension>(this IExtensible extensible)
        where TExtension : class =>
        // Return true if extension is already attached, false otherwise.
        extensible.TryGetExtension<TExtension>(out var _);

    /// <summary>
    ///     Creates a default instance of the extension if it is not already attached to the extensible object.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension to create and attach.</typeparam>
    /// <param name="extensible">The extensible object.</param>
    public static void CreateExtension<TExtension>(this IExtensible extensible)
        where TExtension : class, new()
    {
        // If the extension already exists, then do nothing.
        if (extensible.ContainsExtension<TExtension>())
            return;

        // Create a default extension and attach it.
        var extension = new TExtension();
        extensible.AttachExtension(extension);
    }

    /// <summary>
    ///     Detaches an extension object based on the extension type.
    /// </summary>
    /// <typeparam name="TExtension">The type of extension to detach.</typeparam>
    /// <returns>Extension object if the extension is detached; otherwise, null.</returns>
    public static TExtension? DetachExtension<TExtension>(this IExtensible extensible)
        where TExtension : class
    {
        var extensionType = typeof(TExtension);
        return extensible.DetachExtension(extensionType) as TExtension;
    }

    /// <summary>
    ///     Gets the attached extension of the specified type, or creates, attaches, and returns a new default instance if none exists.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    /// <param name="extensible">The extensible object.</param>
    /// <returns>The existing or newly created extension instance.</returns>
    public static TExtension GetOrAttachExtension<TExtension>(this IExtensible extensible)
        where TExtension : class, new()
    {
        // If extension is already attached, then return the attached extension.
        if (extensible.TryGetExtension<TExtension>(out var extension))
            return extension;

        // Create a default extension and attach it.
        extension = new TExtension();
        extensible.AttachExtension(extension);
        return extension;
    }

    /// <summary>
    ///     Modifies an existing extension or creates, attaches, and then modifies a new default instance.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    /// <param name="extensible">The extensible object.</param>
    /// <param name="modifyExtensionAction">The action to modify the extension instance.</param>
    public static void ModifyExtension<TExtension>(this IExtensible extensible, Action<TExtension> modifyExtensionAction)
        where TExtension : class, new() => modifyExtensionAction(extensible.GetOrAttachExtension<TExtension>());

    /// <summary>
    ///     Tries to retrieve the attached extension object of the specified extension type.
    /// </summary>
    /// <typeparam name="TExtension">The type of extension to retrieve.</typeparam>
    /// <param name="extension">When this method returns, contains the attached extension object if found; otherwise, null.</param>
    /// <returns>True if the extension is found; otherwise, false.</returns>
    public static bool TryGetExtension<TExtension>(this IExtensible extensible, [NotNullWhen(true)] out TExtension? extension)
        where TExtension : class
    {
        var extensionType = typeof(TExtension);
        if (extensible.TryGetExtension(extensionType, out var ext) && ext is TExtension typedExtension)
        {
            extension = typedExtension;
            return true;
        }

        extension = null;
        return false;
    }
    #endregion
}
