// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle.NTree;

/// <summary>
///     Abstracts a node within a 1-N tree.
/// </summary>
/// <typeparam name="TNode">
///     Type of each node within the 1-n tree.
/// </typeparam>
public abstract class Node<TNode> : INode<TNode>
    where TNode : Node<TNode>
{
    #region Properties
    /// <summary>Gets the name of this node.</summary>
    public abstract string Name { get; }

    /// <summary>Gets the root node of the 1-N tree.</summary>
    public TNode Root { get; private set; }

    /// <summary>Gets the parent node of this node.</summary>
    public TNode? Parent { get; private set; }

    /// <summary>Gets the first child node of this node.</summary>
    public TNode? FirstChild { get; private set; }

    /// <summary>Gets the last child node of this node.</summary>
    public TNode? LastChild { get; private set; }

    /// <summary>Gets the next sibling node of this node.</summary>
    public TNode? NextSibling { get; private set; }

    /// <summary>Gets the previous sibling node of this node.</summary>
    public TNode? PreviousSibling { get; private set; }
    #endregion

    #region Computed Properties
    /// <summary>Predicate if this node has any child nodes.</summary>
    public bool HasChildren => this.FirstChild != null;

    /// <summary>Predicate if this node has a parent node.</summary>
    public bool HasParent => this.Parent != null;

    /// <summary>Predicate if this node has any sibling nodes.</summary>
    public bool HasSiblings => this.NextSibling != null || this.PreviousSibling != null;
    #endregion

    #region Constructors
    protected Node()
    {
        this.Root = (TNode)this;
    }

    protected Node(TNode node)
    {
        this.Root = (TNode)this;

        this.AddChildImpl(node);
    }

    protected Node(IEnumerable<TNode> nodeCollection)
    {
        this.Root = (TNode)this;

        this.AddChildRangeImpl(nodeCollection);
    }
    #endregion

    #region Child Methods
    /// <summary>
    ///     Adds a node as the last child of this parent node.
    /// </summary>
    /// <param name="node">Node to add as the last child of this parent node.</param>
    public void AddChild(TNode node)
    {
        this.AddChildImpl(node);
    }

    /// <summary>
    ///     Adds a range of nodes as the last children of this parent node.
    /// </summary>
    /// <param name="nodeCollection">Node collection to add as the last children of this parent node.</param>
    public void AddChildRange(IEnumerable<TNode> nodeCollection)
    {
        this.AddChildRangeImpl(nodeCollection);
    }

    /// <summary>
    ///     Removes an existing child node from this parent node.
    /// </summary>
    /// <param name="node">Child node to remove from this parent node.</param>
    public void RemoveChild(TNode node)
    {
        // Ensure node exists as a child node to this parent node.
        var isChild = ReferenceEquals(this, node.Parent);
        if (!isChild)
            return;

        /////////////////////////////////////////////////////////////////
        // Remove child node
        /////////////////////////////////////////////////////////////////

        // 1. If the child node to be removed is the first child of this parent node.
        if (ReferenceEquals(node, this.FirstChild))
        {
            this.FirstChild = node.NextSibling;
        }

        // 2. If the child node to be removed is the last child of this parent node.
        if (ReferenceEquals(node, this.LastChild))
        {
            this.LastChild = node.PreviousSibling;
        }

        // 3. If the child node to be removed has a next sibling node.
        if (node.NextSibling != null)
        {
            node.NextSibling.PreviousSibling = node.PreviousSibling;
        }

        // 4. If the child node to be removed has a previous sibling node.
        if (node.PreviousSibling != null)
        {
            node.PreviousSibling.NextSibling = node.NextSibling;
        }
    }

    /// <summary>
    ///     Replaces an existing child node with a new child node for this parent node.
    /// </summary>
    /// <param name="oldNode">Old child node to remove from this parent node.</param>
    /// <param name="newNode">New child node to replace the old node with for this parent node.</param>
    public void ReplaceChild(TNode oldNode, TNode newNode)
    {
        // Ensure old node exists as a child before removing.
        var oldNodeIsChild = ReferenceEquals(this, oldNode.Parent);
        if (!oldNodeIsChild)
            return;

        // Validate node has not already been added to the tree.
        newNode.ValidateChildCanBeAdded();

        /////////////////////////////////////////////////////////////////
        // Replace old child node with new child node.
        /////////////////////////////////////////////////////////////////
        newNode.Root = oldNode.Root;
        newNode.Parent = oldNode.Parent;
        newNode.NextSibling = oldNode.NextSibling;
        newNode.PreviousSibling = oldNode.PreviousSibling;

        // Handle special cases where the old node was referenced by other nodes.

        // 1. If the child node to be replaced is the first child of this parent node.
        if (ReferenceEquals(oldNode, this.FirstChild))
        {
            this.FirstChild = newNode;
        }

        // 2. If the child node to be replaced is the last child of this parent node.
        if (ReferenceEquals(oldNode, this.LastChild))
        {
            this.LastChild = newNode;
        }

        // 3. If the node to be replaced previous node was not null.
        if (oldNode.PreviousSibling != null)
        {
            oldNode.PreviousSibling.NextSibling = newNode;
        }

        // 4. If the node to be replaced next node was not null.
        if (oldNode.NextSibling != null)
        {
            oldNode.NextSibling.PreviousSibling = newNode;
        }
    }

    private void AddChildImpl(TNode node)
    {
        // Validate node can be inserted into the tree.
        node.ValidateChildCanBeAdded();

        // Initialize the root and parent properties of the inserting node.
        node.Root = this.Root;
        node.Parent = (TNode)this;

        // Handle special case of the the parent node having no child nodes.
        if (!this.HasChildren)
        {
            // Insert node as the first node of this parent node.
            this.FirstChild = node;
            this.LastChild = node;

            node.NextSibling = null;
            node.PreviousSibling = null;
            return;
        }

        // Insert node as the last child of this parent node.
        var previousLastChild = this.LastChild ?? throw new NullReferenceException(nameof(this.LastChild));
        this.LastChild = node;
        previousLastChild.NextSibling = node;

        node.NextSibling = null;
        node.PreviousSibling = previousLastChild;
    }

    private void AddChildRangeImpl(IEnumerable<TNode> nodeCollection)
    {
        if (nodeCollection == null)
            return;

        foreach (var node in nodeCollection)
        {
            this.AddChildImpl(node);
        }
    }

    private void ValidateChildCanBeAdded()
    {
        if (this.Parent == null &&
            this.FirstChild == null &&
            this.LastChild == null &&
            this.NextSibling == null &&
            this.PreviousSibling == null)
        {
            return;
        }

        var message = $"Node {{Name={this.Name}}} has already been added to a previous tree.";
        throw new InvalidOperationException(message);
    }
    #endregion

    #region Enumerator Methods
    /// <summary>
    ///     Create a breadth-first enumerator for a 1-N tree starting at this node.
    /// </summary>
    /// <returns>
    ///     Newly created breadth-first enumerator for a 1-N tree starting at this node.
    /// </returns>
    public IEnumerator<TNode> CreateBreadFirstEnumerator()
    {
        return new BreadthFirstEnumerator<TNode>((TNode)this);
    }

    /// <summary>
    ///     Create a depth-first (post order) enumerator for a 1-N tree starting at this node.
    /// </summary>
    /// <returns>
    ///     Newly created depth-first (post order) enumerator for a 1-N tree starting at this node.
    /// </returns>
    public IEnumerator<TNode> CreateDepthFirstEnumerator()
    {
        return new DepthFirstEnumerator<TNode>((TNode)this);
    }
    #endregion

    #region Traversal Methods
    /// <summary>
    ///     Traverse with the given enumerator and visiting each node this 1-N tree starting at this node.
    ///     Traverse will stop when the visit function returns false, otherwise traversal will continue.
    /// </summary>
    /// <param name="enumerator">Enumerator for this 1-N tree starting at this node.</param>
    /// <param name="visitorFunction">
    ///     Visitor function that visits the current node in the traversal.
    ///     Traversal will continue as long as the visitor function returns true, will stop if the visitor function returns false.
    /// </param>
    public void Traverse(IEnumerator<TNode> enumerator, Func<TNode, bool> visitorFunction)
    {
        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;

            var visitResult = visitorFunction(current);
            if (!visitResult)
                return;
        }
    }

    /// <summary>
    ///     Traverse with the given enumerator and visiting each node this 1-N tree starting at this node.
    ///     Traverse will stop when the visitor object returns done, otherwise traversal will continue.
    /// </summary>
    /// <param name="enumerator">Enumerator for this 1-N tree starting at this node.</param>
    /// <param name="visitor">
    ///     Visitor object that visits the current node in the traversal.
    ///     Traversal will continue as long as the visitor object returns continue, will stop if the visitor function returns done.
    /// </param>
    public void Traverse(IEnumerator<TNode> enumerator, INodeVisitor<TNode> visitor)
    {
        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;

            var visitResult = visitor.Visit(current);
            if (visitResult == VisitResult.Done)
                return;
        }
    }
    #endregion
}