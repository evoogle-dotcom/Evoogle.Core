// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.NTree;

/// <summary>
///     Abstracts a visitor in the visitor design pattern for a 1-N tree.
/// </summary>
/// <typeparam name="TNode"></typeparam>
public interface INodeVisitor<TNode>
    where TNode : Node<TNode>
{
    #region Methods
    /// <summary>
    ///     Abstracts an individual visit operation on a single node within a 1-N tree.
    /// </summary>
    /// <param name="node">Node to visit.</param>
    /// <returns>Whether the overall visiting operation should continue or is done.</returns>
    VisitResult Visit(TNode node);
    #endregion
}