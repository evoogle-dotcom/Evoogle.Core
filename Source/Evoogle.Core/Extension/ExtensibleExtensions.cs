// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle.Extension;

/// <summary>
///     Extension methods for the <see cref="IExtensible"/> abstraction.
/// </summary>
public static class ExtensibleExtensions
{
    #region Extension Methods
    public static bool ContainsExtension<TExtension>(this IExtensible extensible)
        where TExtension : class
    {
        // Return true if extension is already attached, false otherwise.
        return extensible.TryGetExtension<TExtension>(out var _);
    }

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

    public static void ModifyExtension<TExtension>(this IExtensible extensible, Action<TExtension> modifyExtensionAction)
        where TExtension : class, new()
    {
        // Add an extension if it does not already exist.
        if (extensible.TryGetExtension<TExtension>(out var extension))
        {
            // Modify existing extension instance.
            modifyExtensionAction(extension);
            return;
        }

        // Create a default extension and attach it.
        extension = new TExtension();
        extensible.AttachExtension(extension);

        // Modify new and default extension instance.
        modifyExtensionAction(extension);
    }
    #endregion
}