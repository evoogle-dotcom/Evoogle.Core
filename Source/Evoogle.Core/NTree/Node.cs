// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.Extensions;
using Evoogle.NTree.Internal;

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
    public abstract string Name { get; }

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

    #region Computed Properties
    /// <summary>Predicate if this node has any child nodes.</summary>
    public bool HasChildren => this.FirstChild != null;

    /// <summary>Predicate if this node has a parent node.</summary>
    public bool HasParent => this.Parent != null;

    /// <summary>Predicate if this node has any sibling nodes.</summary>
    public bool HasSiblings => this.NextSibling != null || this.PreviousSibling != null;
    #endregion

    #region Constructors
    // Initially, the node is its own root
    protected Node() => this.Root = (TNode)this;

    protected Node(TNode child)
        : this() => this.AddChild(child);

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

    #region Enumerator Methods
    /// <summary>
    ///     Create a breadth-first enumerator for a 1-N tree starting at this node.
    /// </summary>
    /// <returns>
    ///     Newly created breadth-first enumerator for a 1-N tree starting at this node.
    /// </returns>
    public IEnumerator<TNode> CreateBreadFirstEnumerator() => new BreadthFirstEnumerator<TNode>((TNode)this);

    /// <summary>
    ///     Create a depth-first (post order) enumerator for a 1-N tree starting at this node.
    /// </summary>
    /// <returns>
    ///     Newly created depth-first (post order) enumerator for a 1-N tree starting at this node.
    /// </returns>
    public IEnumerator<TNode> CreateDepthFirstEnumerator() => new DepthFirstEnumerator<TNode>((TNode)this);
    #endregion

    #region Traversal Methods
    /// <summary>
    ///     Enumerates the immediate child nodes of this node.
    /// </summary>
    /// <returns>Sequence of direct children in left-to-right order.</returns>
    public IEnumerable<TNode> Children()
    {
        var child = this.FirstChild;
        while (child != null)
        {
            yield return child;
            child = child.NextSibling;
        }
    }

    /// <summary>
    ///     Enumerates all descendant nodes (excluding this node) using the specified traversal strategy.
    /// </summary>
    /// <param name="enumerator">
    ///     Enumerator that defines the traversal strategy (e.g., breadth-first, depth-first).
    /// </param>
    /// <returns>Sequence of all descendant nodes.</returns>
    public IEnumerable<TNode> Descendants(IEnumerator<TNode> enumerator)
    {
        using (enumerator)
        {
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                if (!ReferenceEquals(current, this))
                {
                    yield return current;
                }
            }
        }
    }

    /// <summary>
    ///     Enumerates all descendant nodes (excluding this node) using the specified traversal strategy.
    /// </summary>
    /// <param name="strategy">
    ///     Optional traversal strategy to use (defaults to <see cref="TraversalStrategy.BreadthFirst"/>).
    /// </param>
    /// <returns>Sequence of all descendant nodes.</returns>
    public IEnumerable<TNode> Descendants(TraversalStrategy strategy = TraversalStrategy.BreadthFirst) =>
        strategy switch
        {
            TraversalStrategy.BreadthFirst => this.Descendants(this.CreateBreadFirstEnumerator()),
            TraversalStrategy.DepthFirst => this.Descendants(this.CreateDepthFirstEnumerator()),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy))
        };

    /// <summary>
    ///     Enumerates the path from the root node down to this node.
    /// </summary>
    /// <returns>
    ///     Sequence of nodes starting from the root down through each parent, ending with this node.
    /// </returns>
    public IEnumerable<TNode> GetPathFromRoot()
    {
        var stack = new Stack<TNode>();
        var current = (TNode)this;

        while (current != null)
        {
            stack.Push(current);
            current = current.Parent;
        }

        while (stack.Count > 0)
        {
            yield return stack.Pop();
        }
    }

    /// <summary>
    ///     Returns a string representation of the path from the root node to this node.
    /// </summary>
    /// <param name="delimiter">
    ///     Delimiter used to separate node names in the resulting path string.
    ///     Defaults to <c>"-&gt;"</c>.
    /// </param>
    /// <returns>
    ///     A single string representing the sequence of node names from the root to this node,
    ///     joined by the specified delimiter.
    /// </returns>
    /// <example>
    ///     For a node with path <c>1 → 11 → 111</c> and the default delimiter,
    ///     the result would be <c>"1-&gt;11-&gt;111"</c>.
    /// </example>
    public string GetPathString(string delimiter = "->") => string.Join(delimiter, this.GetPathFromRoot().Select(n => n.Name));

    /// <summary>
    ///     Enumerates the path from this node up to the root node.
    /// </summary>
    /// <returns>
    ///     Sequence of nodes starting from this node up through its ancestors, ending at the root.
    /// </returns>
    public IEnumerable<TNode> GetPathToRoot()
    {
        var current = (TNode)this;
        while (current != null)
        {
            yield return current;
            current = current.Parent;
        }
    }

    /// <summary>
    ///     Determines whether this node is a descendant of the specified ancestor node.
    /// </summary>
    /// <param name="potentialAncestor">Node to test against.</param>
    /// <returns>
    ///     True if this node is a descendant of <paramref name="potentialAncestor"/>, otherwise false.
    /// </returns>
    public bool IsDescendantOf(TNode potentialAncestor)
    {
        var current = this.Parent;
        while (current != null)
        {
            if (ReferenceEquals(current, potentialAncestor))
            {
                return true;
            }

            current = current.Parent;
        }
        return false;
    }

    /// <summary>
    ///     Enumerates this node and all its descendant nodes using the specified traversal strategy.
    /// </summary>
    /// <param name="enumerator">
    ///     Enumerator that defines the traversal strategy (e.g., breadth-first, depth-first).
    /// </param>
    /// <returns>Sequence including this node followed by all its descendants.</returns>
    public IEnumerable<TNode> SelfAndDescendants(IEnumerator<TNode> enumerator)
    {
        using (enumerator)
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
    }

    /// <summary>
    ///     Enumerates this node and all descendant nodes using the specified traversal strategy.
    /// </summary>
    /// <param name="strategy">
    ///     Optional traversal strategy to use (defaults to <see cref="TraversalStrategy.BreadthFirst"/>).
    /// </param>
    /// <returns>Sequence including this node followed by all its descendants.</returns>
    public IEnumerable<TNode> SelfAndDescendants(TraversalStrategy strategy = TraversalStrategy.BreadthFirst) =>
        strategy switch
        {
            TraversalStrategy.BreadthFirst => this.SelfAndDescendants(this.CreateBreadFirstEnumerator()),
            TraversalStrategy.DepthFirst => this.SelfAndDescendants(this.CreateDepthFirstEnumerator()),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy))
        };

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
            {
                return;
            }
        }
    }

    /// <summary>
    ///     Traverses this node and its descendants using the specified traversal strategy and visitor function.
    /// </summary>
    /// <param name="strategy">
    ///     The traversal strategy to use, such as <see cref="TraversalStrategy.BreadthFirst"/> or <see cref="TraversalStrategy.DepthFirst"/>.
    /// </param>
    /// <param name="visitorFunction">
    ///     A delegate that is called for each visited node. Return <c>true</c> to continue traversal; return <c>false</c> to stop early.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="visitorFunction"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="strategy"/> is not a valid <see cref="TraversalStrategy"/> value.
    /// </exception>
    /// <remarks>
    ///     Traversal includes this node and all of its descendants, and proceeds according to the specified strategy.
    ///     The traversal stops immediately if the <paramref name="visitorFunction"/> returns <c>false</c>.
    /// </remarks>
    public void Traverse(TraversalStrategy strategy, Func<TNode, bool> visitorFunction)
    {
        if (visitorFunction is null)
        {
            throw new ArgumentNullException(nameof(visitorFunction));
        }

        var enumerator = strategy switch
        {
            TraversalStrategy.BreadthFirst => this.CreateBreadFirstEnumerator(),
            TraversalStrategy.DepthFirst => this.CreateDepthFirstEnumerator(),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy))
        };

        this.Traverse(enumerator, visitorFunction);
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
            {
                return;
            }
        }
    }

    /// <summary>
    ///     Traverses this node and its descendants using the specified traversal strategy and visitor object.
    /// </summary>
    /// <param name="strategy">
    ///     The traversal strategy to use, such as <see cref="TraversalStrategy.BreadthFirst"/> or <see cref="TraversalStrategy.DepthFirst"/>.
    /// </param>
    /// <param name="visitor">
    ///     An object implementing <see cref="INodeVisitor{TNode}"/> that determines how each node is processed and whether traversal should continue.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="visitor"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="strategy"/> is not a valid <see cref="TraversalStrategy"/> value.
    /// </exception>
    /// <remarks>
    ///     The traversal includes this node and all of its descendants, following the specified strategy.
    ///     If the <paramref name="visitor"/>'s <see cref="INodeVisitor{TNode}.Visit"/> method returns <see cref="VisitResult.Done"/>, traversal stops immediately.
    /// </remarks>
    public void Traverse(TraversalStrategy strategy, INodeVisitor<TNode> visitor)
    {
        if (visitor is null)
        {
            throw new ArgumentNullException(nameof(visitor));
        }

        var enumerator = strategy switch
        {
            TraversalStrategy.BreadthFirst => this.CreateBreadFirstEnumerator(),
            TraversalStrategy.DepthFirst => this.CreateDepthFirstEnumerator(),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy))
        };

        this.Traverse(enumerator, visitor);
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
