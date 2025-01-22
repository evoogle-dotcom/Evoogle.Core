// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
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

        this.WriteLine($"Test Name: {this.Name}");
        this.WriteDashedLine();

        this.Arrange();
        this.Act();
        this.Assert();
    }
    #endregion

    #region XUnitTest Overrides
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