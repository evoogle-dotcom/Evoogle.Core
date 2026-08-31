// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.NTree;

/// <summary>
///     Defines the read-only structural links of a node within a 1-N tree.
/// </summary>
/// <typeparam name="TNode">
///     Type of each node within the 1-N tree.
/// </typeparam>
/// <remarks>
///     Domain nodes can implement this interface directly without deriving from <see cref="Node{TNode}"/>.
///     Naming is optional and is provided by <see cref="INamedNode{TNode}"/>.
/// </remarks>
public interface INode<TNode>
    where TNode : class, INode<TNode>
{
    #region Properties
    /// <summary>Gets the root node of the 1-N tree.</summary>
    TNode Root { get; }

    /// <summary>Gets the parent node of this node.</summary>
    TNode? Parent { get; }

    /// <summary>Gets the first child node of this node.</summary>
    TNode? FirstChild { get; }

    /// <summary>Gets the last child node of this node.</summary>
    TNode? LastChild { get; }

    /// <summary>Gets the next sibling node of this node.</summary>
    TNode? NextSibling { get; }

    /// <summary>Gets the previous sibling node of this node.</summary>
    TNode? PreviousSibling { get; }
    #endregion
}
