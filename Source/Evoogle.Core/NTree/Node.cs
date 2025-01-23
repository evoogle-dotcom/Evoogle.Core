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
        // Initially, the node is its own root
        this.Root = (TNode)this;
    }

    protected Node(TNode child)
        : this()
    {
        this.AddChild(child);
    }

    protected Node(IEnumerable<TNode> childCollection)
        : this()
    {
        foreach (var child in childCollection)
        {
            this.AddChild(child);
        }
    }
    #endregion

    #region Child Methods
    /// <summary>
    ///     Adds a node as the last child of this parent node.
    /// </summary>
    /// <param name="child">Node to add as the last child of this parent node.</param>
    public void AddChild(TNode child)
    {
        ValidateChildCanBeAdded(child);

        /////////////////////////////////////////////////////////////////
        // Add new child node
        /////////////////////////////////////////////////////////////////

        // Initialize the root and parent properties of the inserting node.
        child.Root = this.Root;
        child.Parent = (TNode)this;

        // Handle special case of the the parent node having no child nodes.
        if (!this.HasChildren)
        {
            // Insert node as the first node of this parent node.
            this.FirstChild = child;
            this.LastChild = child;

            child.NextSibling = null;
            child.PreviousSibling = null;
            return;
        }

        // Insert node as the last child of this parent node.
        var previousLastChild = this.LastChild ?? throw new NullReferenceException(nameof(this.LastChild));
        this.LastChild = child;
        previousLastChild.NextSibling = child;

        child.NextSibling = null;
        child.PreviousSibling = previousLastChild;
    }

    /// <summary>
    ///     Adds a range of nodes as the last children of this parent node.
    /// </summary>
    /// <param name="childCollection">Node collection to add as the last children of this parent node.</param>
    public void AddChildRange(IEnumerable<TNode> childCollection)
    {
        foreach (var child in childCollection)
        {
            this.AddChild(child);
        }
    }

    /// <summary>
    ///     Removes an existing child node from this parent node.
    /// </summary>
    /// <param name="child">Child node to remove from this parent node.</param>
    public void RemoveChild(TNode child)
    {
        ValidateChildCanBeRemoved((TNode)this, child);

        /////////////////////////////////////////////////////////////////
        // Remove old child node
        /////////////////////////////////////////////////////////////////

        // 1. If the child node to be removed is the first child of this parent node.
        if (ReferenceEquals(child, this.FirstChild))
        {
            this.FirstChild = child.NextSibling;
        }

        // 2. If the child node to be removed is the last child of this parent node.
        if (ReferenceEquals(child, this.LastChild))
        {
            this.LastChild = child.PreviousSibling;
        }

        // 3. If the child node to be removed has a next sibling node.
        if (child.NextSibling != null)
        {
            child.NextSibling.PreviousSibling = child.PreviousSibling;
        }

        // 4. If the child node to be removed has a previous sibling node.
        if (child.PreviousSibling != null)
        {
            child.PreviousSibling.NextSibling = child.NextSibling;
        }

        // Child is now the root of a new tree.
        child.NextSibling = null;
        child.PreviousSibling = null;
        child.Parent = null; // Remove the parent link
        child.Root = child; // Reset the root link to itself        
    }

    /// <summary>
    ///     Removes a range of child nodes from this parent node.
    /// </summary>
    /// <param name="childCollection">Node collection to remove from this parent node.</param>
    public void RemoveChildRange(IEnumerable<TNode> childCollection)
    {
        foreach (var child in childCollection)
        {
            this.AddChild(child);
        }
    }

    /// <summary>
    ///     Replaces an existing child node with a new child node for this parent node.
    /// </summary>
    /// <param name="oldChild">Old child node to remove from this parent node.</param>
    /// <param name="newChild">New child node to replace the old node with for this parent node.</param>
    public void ReplaceChild(TNode oldChild, TNode newChild)
    {
        ValidateChildCanBeRemoved((TNode)this, oldChild);
        ValidateChildCanBeAdded(newChild);

        /////////////////////////////////////////////////////////////////
        // Replace old child node with new child node.
        /////////////////////////////////////////////////////////////////
        newChild.Root = oldChild.Root;
        newChild.Parent = oldChild.Parent;
        newChild.NextSibling = oldChild.NextSibling;
        newChild.PreviousSibling = oldChild.PreviousSibling;

        // Handle special cases where the old node was referenced by other nodes.

        // 1. If the child node to be replaced is the first child of this parent node.
        if (ReferenceEquals(oldChild, this.FirstChild))
        {
            this.FirstChild = newChild;
        }

        // 2. If the child node to be replaced is the last child of this parent node.
        if (ReferenceEquals(oldChild, this.LastChild))
        {
            this.LastChild = newChild;
        }

        // 3. If the node to be replaced previous node was not null.
        if (oldChild.PreviousSibling != null)
        {
            oldChild.PreviousSibling.NextSibling = newChild;
        }

        // 4. If the node to be replaced next node was not null.
        if (oldChild.NextSibling != null)
        {
            oldChild.NextSibling.PreviousSibling = newChild;
        }
    }

    private static void ValidateChildCanBeAdded(TNode child)
    {
        if (child.Parent == null &&
            child.FirstChild == null &&
            child.LastChild == null &&
            child.NextSibling == null &&
            child.PreviousSibling == null)
        {
            return;
        }

        var message = $"Node {{Name={child.Name}}} has already been added to a previous tree.";
        throw new InvalidOperationException(message);
    }

    private static void ValidateChildCanBeRemoved(TNode parent, TNode child)
    {
        // Ensure child node exists as a child node to parent node.
        if (ReferenceEquals(parent, child.Parent))
            return;

        var message = $"Can not remove child node {{Name={child.Name} ParentName={child.Parent.SafeToString()}}} as it is not a child of parent node {{{parent.Name}}}.";
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