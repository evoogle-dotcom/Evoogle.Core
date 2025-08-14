// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Collections;

namespace Evoogle.NTree.Internal;

/// <summary>
///     This API supports the Evoogle.Core infrastructure and is not intended to be used directly from your code.
///     This API may change or be removed in future releases.
/// </summary>
internal class BreadthFirstEnumerator<TNode>(TNode node) : IEnumerator<TNode>
    where TNode : Node<TNode>
{
    #region Properties
    private TNode Initial { get; } = node;

    private Queue<TNode> Queue { get; } = new Queue<TNode>([node]);
    #endregion

    #region IEnumerator<TNode> Properties
    public TNode Current => this.NullableCurrent ?? throw new NullReferenceException($"{nameof(this.Current)} is undefined.");

    public TNode? NullableCurrent { get; set; }
    #endregion

    #region IEnumerator Properties
    object IEnumerator.Current => this.Current;
    #endregion

    #region IEnumerator Methods
    public bool MoveNext()
    {
        if (this.Queue.Count == 0)
        {
            this.NullableCurrent = null;
            return false;
        }

        var current = this.Queue.Dequeue();

        var child = current.FirstChild;
        while (child != null)
        {
            this.Queue.Enqueue(child);
            child = child.NextSibling;
        }

        this.NullableCurrent = current;
        return true;
    }

    public void Reset()
    {
        this.Queue.Clear();
        this.Queue.Enqueue(this.Initial);

        this.NullableCurrent = null;
    }
    #endregion

    #region IDisposable Methods
    public void Dispose()
    { }
    #endregion
}
