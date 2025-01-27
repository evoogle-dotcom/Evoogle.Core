// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
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