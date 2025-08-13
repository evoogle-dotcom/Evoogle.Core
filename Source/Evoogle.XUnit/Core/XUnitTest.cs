// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.XUnit;

/// <summary>
///     Captures boilerplate code for an individual named xUnit test to break the unit test into
///     explicit Arrange, Act, Assert steps that are executed in the context of an xUnit tests container.
/// </summary>
public abstract class XUnitTest : XUnitTestBase, IXUnitTest
{
    #region Constructors
    protected XUnitTest(string? name = null) : base(name) { }
    #endregion

    #region IXUnitTest Methods
    /// <summary>
    ///     Executes the unit test by writing the test header and calling the Arrange, Act, and Assert steps.
    /// </summary>
    /// <param name="parent">The parent tests container.</param>
    public void Execute(XUnitTests parent)
    {
        this.SetParent(parent);

        this.WriteDashedLine();
        this.WriteLine($"Test Name: {this.Name}");
        this.WriteLine();

        this.Arrange();
        this.Act();
        this.Assert();

        this.WriteLine();
    }
    #endregion

    #region Arrange/Act/Assert
    /// <summary>No-op implementation of the arrange step.</summary>
    protected virtual void Arrange() { }

    /// <summary>No-op implementation of the act step.</summary>
    protected virtual void Act() { }

    /// <summary>No-op implementation of the assert step.</summary>
    protected virtual void Assert() { }
    #endregion
}
