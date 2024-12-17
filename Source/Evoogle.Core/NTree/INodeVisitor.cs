// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle.NTree;

public interface INodeVisitor<TNode>
    where TNode : Node<TNode>
{
    #region Methods
    VisitResult Visit(TNode node);
    #endregion
}