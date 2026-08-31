// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.NTree.Internal;

namespace Evoogle.NTree;

/// <summary>
///     Provides traversal operations for read-only 1-N tree nodes.
/// </summary>
/// <remarks>
///     Implementations of <see cref="INode{TNode}"/> must expose a consistent, acyclic
///     topology of reference-identity nodes. Traversal does not perform cycle detection.
/// </remarks>
public static class NodeExtensions
{
    #region Enumerator Methods
    /// <summary>Creates a breadth-first enumerator starting at the specified node.</summary>
    /// <typeparam name="TNode">The concrete node type.</typeparam>
    /// <param name="node">The node at which traversal starts.</param>
    /// <returns>A breadth-first enumerator that includes <paramref name="node"/>.</returns>
    public static IEnumerator<TNode> CreateBreadthFirstEnumerator<TNode>(this TNode node)
        where TNode : class, INode<TNode>
    {
        ArgumentNullException.ThrowIfNull(node);
        return new BreadthFirstEnumerator<TNode>(node);
    }

    /// <summary>Creates a depth-first preorder enumerator starting at the specified node.</summary>
    /// <typeparam name="TNode">The concrete node type.</typeparam>
    /// <param name="node">The node at which traversal starts.</param>
    /// <returns>A depth-first preorder enumerator that includes <paramref name="node"/>.</returns>
    public static IEnumerator<TNode> CreateDepthFirstEnumerator<TNode>(this TNode node)
        where TNode : class, INode<TNode>
    {
        ArgumentNullException.ThrowIfNull(node);
        return new DepthFirstEnumerator<TNode>(node);
    }
    #endregion

    #region Traversal Methods
    /// <summary>Enumerates the immediate children of the specified node.</summary>
    /// <typeparam name="TNode">The concrete node type.</typeparam>
    /// <param name="node">The node whose children are enumerated.</param>
    /// <returns>The direct children in left-to-right order.</returns>
    public static IEnumerable<TNode> Children<TNode>(this TNode node)
        where TNode : class, INode<TNode>
    {
        ArgumentNullException.ThrowIfNull(node);
        return EnumerateChildren(node);

        static IEnumerable<TNode> EnumerateChildren(TNode parent)
        {
            var child = parent.FirstChild;
            while (child is not null)
            {
                yield return child;
                child = child.NextSibling;
            }
        }
    }

    /// <summary>
    ///     Enumerates descendants, excluding the specified node, with the supplied enumerator.
    /// </summary>
    /// <typeparam name="TNode">The concrete node type.</typeparam>
    /// <param name="node">The node excluded from the returned sequence.</param>
    /// <param name="enumerator">The enumerator that defines traversal order.</param>
    /// <returns>All enumerated descendants other than <paramref name="node"/>.</returns>
    public static IEnumerable<TNode> Descendants<TNode>
    (
        this TNode node,
        IEnumerator<TNode> enumerator
    )
        where TNode : class, INode<TNode>
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(enumerator);
        return EnumerateDescendants(node, enumerator);

        static IEnumerable<TNode> EnumerateDescendants
        (
            TNode ancestor,
            IEnumerator<TNode> traversalEnumerator
        )
        {
            using (traversalEnumerator)
            {
                while (traversalEnumerator.MoveNext())
                {
                    var current = traversalEnumerator.Current;
                    if (!ReferenceEquals(current, ancestor))
                    {
                        yield return current;
                    }
                }
            }
        }
    }

    /// <summary>Enumerates descendants using the specified traversal strategy.</summary>
    /// <typeparam name="TNode">The concrete node type.</typeparam>
    /// <param name="node">The node whose descendants are enumerated.</param>
    /// <param name="strategy">The traversal strategy.</param>
    /// <returns>All descendants, excluding <paramref name="node"/>.</returns>
    public static IEnumerable<TNode> Descendants<TNode>
    (
        this TNode node,
        TraversalStrategy strategy = TraversalStrategy.BreadthFirst
    )
        where TNode : class, INode<TNode>
    {
        ArgumentNullException.ThrowIfNull(node);

        var enumerator = strategy switch
        {
            TraversalStrategy.BreadthFirst => node.CreateBreadthFirstEnumerator(),
            TraversalStrategy.DepthFirst => node.CreateDepthFirstEnumerator(),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy))
        };

        return node.Descendants(enumerator);
    }

    /// <summary>Enumerates the path from the root down to the specified node.</summary>
    /// <typeparam name="TNode">The concrete node type.</typeparam>
    /// <param name="node">The path's terminal node.</param>
    /// <returns>The root-to-node path, including both endpoints.</returns>
    public static IEnumerable<TNode> GetPathFromRoot<TNode>(this TNode node)
        where TNode : class, INode<TNode>
    {
        ArgumentNullException.ThrowIfNull(node);
        return EnumeratePathFromRoot(node);

        static IEnumerable<TNode> EnumeratePathFromRoot(TNode terminalNode)
        {
            var stack = new Stack<TNode>();
            var current = terminalNode;

            while (current is not null)
            {
                stack.Push(current);
                current = current.Parent;
            }

            while (stack.Count > 0)
            {
                yield return stack.Pop();
            }
        }
    }

    /// <summary>Formats the root-to-node path using each node's name.</summary>
    /// <typeparam name="TNode">The concrete named-node type.</typeparam>
    /// <param name="node">The path's terminal node.</param>
    /// <param name="delimiter">The delimiter placed between node names.</param>
    /// <returns>The formatted root-to-node path.</returns>
    public static string GetPathString<TNode>(this TNode node, string delimiter = "->")
        where TNode : class, INamedNode<TNode>
    {
        ArgumentNullException.ThrowIfNull(node);
        return node.GetPathString(static current => current.Name, delimiter);
    }

    /// <summary>Formats the root-to-node path using a caller-supplied name selector.</summary>
    /// <typeparam name="TNode">The concrete node type.</typeparam>
    /// <param name="node">The path's terminal node.</param>
    /// <param name="nameSelector">Selects the name used for each path node.</param>
    /// <param name="delimiter">The delimiter placed between selected names.</param>
    /// <returns>The formatted root-to-node path.</returns>
    public static string GetPathString<TNode>
    (
        this TNode node,
        Func<TNode, string> nameSelector,
        string delimiter = "->"
    )
        where TNode : class, INode<TNode>
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(nameSelector);
        return string.Join(delimiter, node.GetPathFromRoot().Select(nameSelector));
    }

    /// <summary>Enumerates the path from the specified node up to the root.</summary>
    /// <typeparam name="TNode">The concrete node type.</typeparam>
    /// <param name="node">The path's initial node.</param>
    /// <returns>The node-to-root path, including both endpoints.</returns>
    public static IEnumerable<TNode> GetPathToRoot<TNode>(this TNode node)
        where TNode : class, INode<TNode>
    {
        ArgumentNullException.ThrowIfNull(node);
        return EnumeratePathToRoot(node);

        static IEnumerable<TNode> EnumeratePathToRoot(TNode initialNode)
        {
            var current = initialNode;
            while (current is not null)
            {
                yield return current;
                current = current.Parent;
            }
        }
    }

    /// <summary>Determines whether a node is a descendant of another node.</summary>
    /// <typeparam name="TNode">The concrete node type.</typeparam>
    /// <param name="node">The potential descendant.</param>
    /// <param name="potentialAncestor">The potential ancestor.</param>
    /// <returns><see langword="true"/> when the ancestor appears in the parent chain.</returns>
    public static bool IsDescendantOf<TNode>(this TNode node, TNode potentialAncestor)
        where TNode : class, INode<TNode>
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(potentialAncestor);

        var current = node.Parent;
        while (current is not null)
        {
            if (ReferenceEquals(current, potentialAncestor))
            {
                return true;
            }

            current = current.Parent;
        }

        return false;
    }

    /// <summary>Enumerates a node and its descendants with the supplied enumerator.</summary>
    /// <typeparam name="TNode">The concrete node type.</typeparam>
    /// <param name="node">The node included at the start of the traversal.</param>
    /// <param name="enumerator">The enumerator that defines traversal order.</param>
    /// <returns>The enumerated nodes, including <paramref name="node"/>.</returns>
    public static IEnumerable<TNode> SelfAndDescendants<TNode>
    (
        this TNode node,
        IEnumerator<TNode> enumerator
    )
        where TNode : class, INode<TNode>
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(enumerator);
        return EnumerateSelfAndDescendants(enumerator);

        static IEnumerable<TNode> EnumerateSelfAndDescendants
        (
            IEnumerator<TNode> traversalEnumerator
        )
        {
            using (traversalEnumerator)
            {
                while (traversalEnumerator.MoveNext())
                {
                    yield return traversalEnumerator.Current;
                }
            }
        }
    }

    /// <summary>Enumerates a node and its descendants using a traversal strategy.</summary>
    /// <typeparam name="TNode">The concrete node type.</typeparam>
    /// <param name="node">The node included at the start of the traversal.</param>
    /// <param name="strategy">The traversal strategy.</param>
    /// <returns>The traversed nodes, including <paramref name="node"/>.</returns>
    public static IEnumerable<TNode> SelfAndDescendants<TNode>
    (
        this TNode node,
        TraversalStrategy strategy = TraversalStrategy.BreadthFirst
    )
        where TNode : class, INode<TNode>
    {
        ArgumentNullException.ThrowIfNull(node);

        var enumerator = strategy switch
        {
            TraversalStrategy.BreadthFirst => node.CreateBreadthFirstEnumerator(),
            TraversalStrategy.DepthFirst => node.CreateDepthFirstEnumerator(),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy))
        };

        return node.SelfAndDescendants(enumerator);
    }

    /// <summary>Traverses nodes with an enumerator and delegate visitor.</summary>
    /// <typeparam name="TNode">The concrete node type.</typeparam>
    /// <param name="node">The traversal's starting node.</param>
    /// <param name="enumerator">The enumerator that defines traversal order.</param>
    /// <param name="visitorFunction">Returns whether traversal should continue.</param>
    public static void Traverse<TNode>
    (
        this TNode node,
        IEnumerator<TNode> enumerator,
        Func<TNode, bool> visitorFunction
    )
        where TNode : class, INode<TNode>
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(enumerator);
        ArgumentNullException.ThrowIfNull(visitorFunction);

        while (enumerator.MoveNext())
        {
            if (!visitorFunction(enumerator.Current))
            {
                return;
            }
        }
    }

    /// <summary>Traverses nodes with a strategy and delegate visitor.</summary>
    /// <typeparam name="TNode">The concrete node type.</typeparam>
    /// <param name="node">The traversal's starting node.</param>
    /// <param name="strategy">The traversal strategy.</param>
    /// <param name="visitorFunction">Returns whether traversal should continue.</param>
    public static void Traverse<TNode>
    (
        this TNode node,
        TraversalStrategy strategy,
        Func<TNode, bool> visitorFunction
    )
        where TNode : class, INode<TNode>
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(visitorFunction);

        var enumerator = strategy switch
        {
            TraversalStrategy.BreadthFirst => node.CreateBreadthFirstEnumerator(),
            TraversalStrategy.DepthFirst => node.CreateDepthFirstEnumerator(),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy))
        };

        node.Traverse(enumerator, visitorFunction);
    }

    /// <summary>Traverses nodes with an enumerator and visitor object.</summary>
    /// <typeparam name="TNode">The concrete node type.</typeparam>
    /// <param name="node">The traversal's starting node.</param>
    /// <param name="enumerator">The enumerator that defines traversal order.</param>
    /// <param name="visitor">The visitor that controls continuation.</param>
    public static void Traverse<TNode>
    (
        this TNode node,
        IEnumerator<TNode> enumerator,
        INodeVisitor<TNode> visitor
    )
        where TNode : class, INode<TNode>
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(enumerator);
        ArgumentNullException.ThrowIfNull(visitor);

        while (enumerator.MoveNext())
        {
            if (visitor.Visit(enumerator.Current) == VisitResult.Done)
            {
                return;
            }
        }
    }

    /// <summary>Traverses nodes with a strategy and visitor object.</summary>
    /// <typeparam name="TNode">The concrete node type.</typeparam>
    /// <param name="node">The traversal's starting node.</param>
    /// <param name="strategy">The traversal strategy.</param>
    /// <param name="visitor">The visitor that controls continuation.</param>
    public static void Traverse<TNode>
    (
        this TNode node,
        TraversalStrategy strategy,
        INodeVisitor<TNode> visitor
    )
        where TNode : class, INode<TNode>
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(visitor);

        var enumerator = strategy switch
        {
            TraversalStrategy.BreadthFirst => node.CreateBreadthFirstEnumerator(),
            TraversalStrategy.DepthFirst => node.CreateDepthFirstEnumerator(),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy))
        };

        node.Traverse(enumerator, visitor);
    }
    #endregion
}
