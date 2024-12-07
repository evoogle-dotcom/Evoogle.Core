// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle;

/// <summary>
///     Type based reflection flags that control what reflected types are included in a search by the <see cref="TypeExtensions"/> static class.
/// </summary>
[Flags]
public enum TypeReflectionFlags
{
    /// <summary>Specifies no reflection flag.</summary>
    Default = 0,

    /// <summary>Specifies that the case of the member name should not be considered when searching.</summary>
    IgnoreCase = 1,

    /// <summary>Specifies that only members declared at the level of the supplied type's hierarchy should be considered. Inherited members are not considered.</summary>
    DeclaredOnly = 1 << 1,

    /// <summary>Specifies that instance members are to be included in the search.</summary>
    Instance = 1 << 2,

    /// <summary>Specifies that static members are to be included in the search.</summary>
    Static = 1 << 3,

    /// <summary>Specifies that public members are to be included in the search.</summary>
    Public = 1 << 4,

    /// <summary>Specifies that non-public members are to be included in the search.</summary>
    NonPublic = 1 << 5,
}