// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.XUnit;

/// <summary>
///     Common base class for <see cref="XUnitTest" /> and <see cref="XUnitTestAsync" /> respectively.
/// </summary>
public abstract class XUnitTestBase
{
    #region Constructors
    protected XUnitTestBase(string? name = null)
    {
        this.Name = name ?? this.GetType().Name;
    }
    #endregion

    #region Properties
    /// <summary>Gets the display name for the test.</summary>
    public string Name { get; set; }

    /// <summary>Gets the parent test container instance while the test is running.</summary>
    protected XUnitTests? Parent { get; private set; }
    #endregion

    #region Lifecycle
    internal void SetParent(XUnitTests parent) => this.Parent = parent;
    #endregion

    #region Output helpers (delegate to parent)
    /// <summary>Writes a message to the test text output.</summary>
    protected void WriteLine(string? message = null) => this.Parent?.WriteLine(message ?? string.Empty);

    /// <summary>Writes a dashed line to the test text output.</summary>
    protected void WriteDashedLine() => this.Parent?.WriteDashedLine();

    /// <summary>Writes a double dashed line to the test text output.</summary>
    protected void WriteDoubleDashedLine() => this.Parent?.WriteDoubleDashedLine();
    #endregion
}
