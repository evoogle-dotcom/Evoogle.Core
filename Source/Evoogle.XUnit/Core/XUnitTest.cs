// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.XUnit;

/// <summary>
///     Captures boilerplate code for an individual named xUnit test to break the unit test into explicit arrange, act, asert steps
///     that are executed in the context of an xUnit tests container.
/// </summary>
public abstract class XUnitTest : XUnitTestBase, IXUnitTest
{
    #region IXUnitTest Implementation
    /// <summary>
    ///     Executes the unit test by writing the name of the unit test followed by calling the arrange, act, and assert steps of the unit test.
    /// </summary>
    /// <param name="parent">Parent unit tests container object.</param>
    public virtual void Execute(XUnitTests parent)
    {
        this.Parent = parent;

        this.WriteDashedLine();
        this.WriteLine($"Test Name: {this.Name}");
        this.WriteLine();

        this.Arrange();
        this.Act();
        this.Assert();

        this.WriteLine();
    }
    #endregion

    #region XUnitTest Methods
    /// <summary>Noop implementation of the arrange step.</summary>
    protected virtual void Arrange()
    {
    }

    /// <summary>Noop implementation of the act step.</summary>
    protected virtual void Act()
    {
    }

    /// <summary>Noop implementation of the assert step.</summary>
    protected virtual void Assert()
    {
    }
    #endregion
}
