// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Linq.Dynamic.Core.CustomTypeProviders;

namespace Evoogle.NTree;

[DynamicLinqType]
public class TestNode : Node<TestNode>
{
    #region Properties
    public override string Name { get; }
    #endregion

    #region Constructors
    public TestNode(string name)
    {
        this.Name = name;
    }

    public TestNode(string name, params TestNode[] nodeCollection)
        : base(nodeCollection.AsEnumerable())
    {
        this.Name = name;
    }
    #endregion

    #region Test Methods
    public static IEnumerator<TestNode> CreateBreadthFirstEnumerator(TestNode testNode)
        => testNode.CreateBreadthFirstEnumerator();

    public static IEnumerator<TestNode> CreateDepthFirstEnumerator(TestNode testNode) => testNode.CreateDepthFirstEnumerator();

    public static TestNode CreateNode(string name)
    {
        var node = new TestNode(name);
        return node;
    }

    public static TestNode CreateTree(int maxDepth, int maxChildren)
    {
        var root = new TestNode("1");
        if (maxDepth == 0)
        {
            return root;
        }

        BuildTree(maxDepth, maxChildren, 1, root);

        return root;
    }

    public static void ReplaceFirstChild(TestNode parent, TestNode newChild)
    {
        var firstChild = parent.FirstChild;
        if (firstChild == null)
        {
            return;
        }

        parent.ReplaceChild(firstChild, newChild);
    }

    public static void ReplaceSecondChild(TestNode parent, TestNode newChild)
    {
        var firstChild = parent.FirstChild;
        if (firstChild == null)
        {
            return;
        }

        var secondChild = firstChild.NextSibling;
        if (secondChild == null)
        {
            return;
        }

        parent.ReplaceChild(secondChild, newChild);
    }

    public static void ReplaceThirdChild(TestNode parent, TestNode newChild)
    {
        var firstChild = parent.FirstChild;
        if (firstChild == null)
        {
            return;
        }

        var secondChild = firstChild.NextSibling;
        if (secondChild == null)
        {
            return;
        }

        var thirdChild = secondChild.NextSibling;
        if (thirdChild == null)
        {
            return;
        }

        parent.ReplaceChild(thirdChild, newChild);
    }

    public static void RemoveFirstChild(TestNode tree)
    {
        var firstChild = tree.FirstChild;
        if (firstChild == null)
        {
            return;
        }

        tree.RemoveChild(firstChild);
    }

    public static void RemoveSecondChild(TestNode tree)
    {
        var firstChild = tree.FirstChild;
        if (firstChild == null)
        {
            return;
        }

        var secondChild = firstChild.NextSibling;
        if (secondChild == null)
        {
            return;
        }

        tree.RemoveChild(secondChild);
    }

    public static void RemoveThirdChild(TestNode tree)
    {
        var firstChild = tree.FirstChild;
        if (firstChild == null)
        {
            return;
        }

        var secondChild = firstChild.NextSibling;
        if (secondChild == null)
        {
            return;
        }

        var thirdChild = secondChild.NextSibling;
        if (thirdChild == null)
        {
            return;
        }

        tree.RemoveChild(thirdChild);
    }

    private static void BuildTree(int maxDepth, int maxChildren, int currentDepth, TestNode parent)
    {
        // Create child nodes
        var childNodes = new List<TestNode>();
        var parentName = parent.Name;
        for (var childNumber = 1; childNumber <= maxChildren; ++childNumber)
        {
            var childName = $"{parentName}{childNumber}";
            var childNode = new TestNode(childName);
            childNodes.Add(childNode);

            parent.AddChild(childNode);
        }

        // If we are at maximum depth, then return.
        if (currentDepth == maxDepth)
        {
            return;
        }

        for (var i = 0; i < maxChildren; ++i)
        {
            var childNode = childNodes[i];
            BuildTree(maxDepth, maxChildren, currentDepth + 1, childNode);
        }
    }
    #endregion

    #region Object Methods
    public override string ToString() => this.Name;
    #endregion
}
