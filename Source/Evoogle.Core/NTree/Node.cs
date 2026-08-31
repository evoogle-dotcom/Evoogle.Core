// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.Extensions;

namespace Evoogle.NTree;

/// <summary>
///     Provides a mutable, named implementation of a node within a 1-N tree.
/// </summary>
/// <typeparam name="TNode">
///     Type of each node within the 1-N tree.
/// </typeparam>
public abstract class Node<TNode> : INamedNode<TNode>
    where TNode : Node<TNode>
{
    #region Events
    /// <summary>Raised after a child node is added to this node.</summary>
    public event Action<TNode, TNode>? ChildAdded;

    /// <summary>Raised after a child node is removed from this node.</summary>
    public event Action<TNode, TNode>? ChildRemoved;

    /// <summary>Raised after a child node is replaced on this node.</summary>
    public event Action<TNode, TNode, TNode>? ChildReplaced;
    #endregion

    #region INode<TNode> Properties
    /// <inheritdoc />
    public TNode Root { get; private set; }

    /// <inheritdoc />
    public TNode? Parent { get; private set; }

    /// <inheritdoc />
    public TNode? FirstChild { get; private set; }

    /// <inheritdoc />
    public TNode? LastChild { get; private set; }

    /// <inheritdoc />
    public TNode? NextSibling { get; private set; }

    /// <inheritdoc />
    public TNode? PreviousSibling { get; private set; }
    #endregion

    #region INamedNode<TNode> Properties
    /// <inheritdoc />
    public abstract string Name { get; }
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
    /// <summary>
    ///     Initializes a new standalone node that is its own root.
    /// </summary>
    protected Node() => this.Root = (TNode)this;

    /// <summary>
    ///     Initializes a new node with a single initial child.
    /// </summary>
    /// <param name="child">The initial child node to add.</param>
    protected Node(TNode child)
        : this() => this.AddChild(child);

    /// <summary>
    ///     Initializes a new node with an initial collection of children.
    /// </summary>
    /// <param name="childCollection">The initial collection of child nodes to add.</param>
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
        try
        {
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
        finally
        {
            // Raise the child added event.
            this.RaiseChildAdded(child);
        }
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
    ///     Adds a range of child nodes from this parent node.
    /// </summary>
    /// <param name="childCollection">
    ///     Array of nodes to add to this parent node.
    /// </param>
    public void AddChildRange(params TNode[] childCollection) => this.AddChildRange(childCollection.AsEnumerable());

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
        try
        {
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

            // Unlink the child node.
            UnlinkNode(child);
        }
        finally
        {
            // Raise the child removed event.
            this.RaiseChildRemoved(child);
        }
    }

    /// <summary>
    ///     Removes a range of child nodes from this parent node.
    /// </summary>
    /// <param name="childCollection">Node collection to remove from this parent node.</param>
    public void RemoveChildRange(IEnumerable<TNode> childCollection)
    {
        foreach (var child in childCollection)
        {
            this.RemoveChild(child);
        }
    }

    /// <summary>
    ///     Removes a range of child nodes from this parent node.
    /// </summary>
    /// <param name="childCollection">
    ///     Array of nodes to remove from this parent node.
    /// </param>
    public void RemoveChildRange(params TNode[] childCollection) => this.RemoveChildRange(childCollection.AsEnumerable());

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
        try
        {
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

            // Unlink the old child node.
            UnlinkNode(oldChild);
        }
        finally
        {
            // Raise the child replaced event.
            this.RaiseChildReplaced(oldChild, newChild);
        }
    }

    private void RaiseChildAdded(TNode child)
    {
        var handler = this.ChildAdded;
        handler?.Invoke((TNode)this, child);
    }

    private void RaiseChildRemoved(TNode child)
    {
        var handler = this.ChildRemoved;
        handler?.Invoke((TNode)this, child);
    }

    private void RaiseChildReplaced(TNode oldChild, TNode newChild)
    {
        var handler = this.ChildReplaced;
        handler?.Invoke((TNode)this, oldChild, newChild);
    }

    private static void ValidateChildCanBeAdded(TNode child)
    {
        ArgumentNullException.ThrowIfNull(child);

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
        {
            return;
        }

        var message = $"Can not remove child node {{Name={child.Name} ParentName={child.Parent.SafeToString()}}} as it is not a child of parent node {{{parent.Name}}}.";
        throw new InvalidOperationException(message);
    }
    #endregion

    #region Implementation Methods
    private static void UnlinkNode(TNode node)
    {
        // Node is now the root of a new tree.
        node.NextSibling = null;
        node.PreviousSibling = null;
        node.Parent = null; // Remove the parent link
        node.Root = node; // Reset the root link to itself
    }
    #endregion
}
