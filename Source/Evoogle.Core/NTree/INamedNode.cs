// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.NTree;

/// <summary>
///     Adds a name to a structurally linked 1-N tree node.
/// </summary>
/// <typeparam name="TNode">
///     Type of each named node within the 1-N tree.
/// </typeparam>
/// <remarks>
///     Implement <see cref="INode{TNode}"/> directly when traversal is required but a node does not have a domain-independent name.
/// </remarks>
public interface INamedNode<TNode> : INode<TNode>
    where TNode : class, INamedNode<TNode>
{
    #region Properties
    /// <summary>Gets the name of this node.</summary>
    string Name { get; }
    #endregion
}
