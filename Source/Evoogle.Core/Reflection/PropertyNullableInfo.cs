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
    /// <summary>Represents detailed nullability information about each collection layer.</summary>
    public sealed record CollectionLayerInfo
    {
        public Type CollectionType { get; init; } = null!;
        public bool IsCollection { get; init; }
        public bool IsCollectionNullable { get; init; }
        public Type ElementType { get; init; } = null!;
        public bool IsElementNullable { get; init; }
    }
    #endregion

    #region Properties
    /// <summary>True if the property itself is nullable.</summary>
    public bool IsNullable { get; init; }

    /// <summary>True if the top-level property type is a collection (excluding string).</summary>
    public bool IsCollection => this.CollectionChain.Count > 0;

    /// <summary>All recursive levels of collection and element types with nullability info.</summary>
    public IReadOnlyList<CollectionLayerInfo> CollectionChain { get; init; } = [];
    #endregion
}
