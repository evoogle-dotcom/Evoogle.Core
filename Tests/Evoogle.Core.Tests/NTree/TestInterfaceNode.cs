// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.NTree;

public abstract class TestInterfaceNodeBase
{
    #region Properties
    public object BaseMarker { get; } = new();
    #endregion
}

public sealed class TestInterfaceNode :
    TestInterfaceNodeBase,
    INode<TestInterfaceNode>
{
    #region Properties
    public string Label { get; }

    public TestInterfaceNode Root { get; private set; } = null!;

    public TestInterfaceNode? Parent { get; private set; }

    public TestInterfaceNode? FirstChild { get; private set; }

    public TestInterfaceNode? LastChild { get; private set; }

    public TestInterfaceNode? NextSibling { get; private set; }

    public TestInterfaceNode? PreviousSibling { get; private set; }
    #endregion

    #region Constructors
    public TestInterfaceNode(string label)
    {
        this.Label = label;
        this.Root = this;
    }
    #endregion

    #region Methods
    public void AddChild(TestInterfaceNode child)
    {
        ArgumentNullException.ThrowIfNull(child);

        if (child.Parent is not null || child.NextSibling is not null ||
            child.PreviousSibling is not null)
        {
            throw new InvalidOperationException("The child is already attached to a tree.");
        }

        child.Root = this.Root;
        child.Parent = this;

        if (this.LastChild is null)
        {
            this.FirstChild = child;
            this.LastChild = child;
            return;
        }

        child.PreviousSibling = this.LastChild;
        this.LastChild.NextSibling = child;
        this.LastChild = child;
    }

    public static TestInterfaceNode CreateTree()
    {
        var root = new TestInterfaceNode("1");
        var child11 = new TestInterfaceNode("11");
        var child12 = new TestInterfaceNode("12");

        root.AddChild(child11);
        root.AddChild(child12);

        child11.AddChild(new TestInterfaceNode("111"));
        child11.AddChild(new TestInterfaceNode("112"));

        child12.AddChild(new TestInterfaceNode("121"));
        child12.AddChild(new TestInterfaceNode("122"));

        return root;
    }
    #endregion
}
