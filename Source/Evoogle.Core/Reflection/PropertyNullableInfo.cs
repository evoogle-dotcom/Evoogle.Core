// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.Reflection;

/// <summary>
///     Represents the nullability information of a property.
///     Includes reference and value types, collection types, and element types. Supports recursive collection layers.
/// </summary>
public sealed record PropertyNullableInfo
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

        /// <summary>Indicates whether the collection itself is nullable.</summary>
        public bool IsCollectionNullable { get; init; }

        /// <summary>Indicates whether the element type within the collection is nullable.</summary>
        public bool IsElementNullable { get; init; }
    }
    #endregion

    #region Properties
    /// <summary>The type of the property being inspected.</summary>
    public Type PropertyType { get; init; } = null!;

    /// <summary>True if the property itself is nullable.</summary>
    public bool IsNullable { get; init; }

    /// <summary>
    ///     Recursive chain of collection types (if applicable), each layer describing its type and nullability.
    ///     Empty if the property is not a collection.
    /// </summary>
    public IReadOnlyList<CollectionInfo> CollectionChain { get; init; } = [];
    #endregion

    #region Computed Properties
    /// <summary>True if the top-level property type is a collection (excluding string).</summary>
    public bool IsCollection => this.CollectionChain.Count > 0;
    #endregion
}
