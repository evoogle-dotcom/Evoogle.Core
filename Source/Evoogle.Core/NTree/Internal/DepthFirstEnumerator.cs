// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Collections;

namespace Evoogle.NTree;

/// <summary>
/// This API supports the Evoogle.Core infrastructure and is not intended to be used directly from your code.
/// This API may change or be removed in future releases.
/// </summary>
internal class DepthFirstEnumerator<TNode>(TNode node) : IEnumerator<TNode>
    where TNode : Node<TNode>
{
    #region Properties
    private TNode Initial { get; } = node;

    private Stack<TNode> Stack { get; } = new Stack<TNode>([node]);
    #endregion

    #region IEnumerator<TNode> Properties
    public TNode Current => this.NullableCurrent ?? throw new NullReferenceException($"{nameof(this.Current)} is undefind.");

    public TNode? NullableCurrent { get; set; }
    #endregion

    #region IEnumerator Properties
    object IEnumerator.Current => this.Current;
    #endregion

    #region IEnumerator Methods
    public bool MoveNext()
    {
        if (this.Stack.Count == 0)
        {
            this.NullableCurrent = null;
            return false;
        }

        var current = this.Stack.Pop();

        // Push siblings onto the stack (in reverse order for correct left-to-right traversal)
        var child = current.LastChild;
        while (child != null)
        {
            this.Stack.Push(child);
            child = child.PreviousSibling;
        }

        this.NullableCurrent = current;
        return true;
    }

    public void Reset()
    {
        this.Stack.Clear();
        this.Stack.Push(this.Initial);

        this.NullableCurrent = null;
    }
    #endregion

    #region IDisposable Methods
    public void Dispose() { }
    #endregion
}