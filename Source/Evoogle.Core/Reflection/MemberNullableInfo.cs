// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.Reflection;

/// <summary>
///     Represents the nullability information of a data member which can either be a property or field.
///     Includes reference and value types, collection types, and element types. Supports recursive collection layers.
/// </summary>
public sealed record MemberNullableInfo
{
    #region Types
    /// <summary>
    ///     Represents detailed nullability information about a collection type and its element type.
    ///     Useful for nested or multi-layered collections.
    /// </summary>
    public sealed record CollectionInfo
    {
        /// <summary>The collection type at this level.</summary>
        public Type CollectionType { get; init; } = null!;

        /// <summary>The element type within the collection.</summary>
        public Type ElementType { get; init; } = null!;

        /// <summary>Indicates the nullability of the collection itself.</summary>
        public MemberNullability CollectionNullability { get; init; }

        /// <summary>Indicates the nullability of the element type within the collection.</summary>
        public MemberNullability ElementNullability { get; init; }
    }
    #endregion

    #region Properties
    /// <summary>The type of the member being inspected.</summary>
    public Type MemberType { get; init; } = null!;

    /// <summary>The nullability of the member itself.</summary>
    public MemberNullability Nullability { get; init; }

    /// <summary>
    ///     Recursive chain of collection types (if applicable), each layer describing its type and nullability.
    ///     Empty if the member is not a collection.
    /// </summary>
    public IReadOnlyList<CollectionInfo> CollectionChain { get; init; } = [];
    #endregion

    #region Computed Properties
    /// <summary>True if the top-level member type is a collection (excluding string).</summary>
    public bool IsCollection => this.CollectionChain.Count > 0;
    #endregion
}
