// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle.XUnit;

/// <summary>
///     Captures boilerplate code for an individual named xunit test to break the unit test into explicit arrange, act, asert steps
///     that are executed in the context of an xunit tests container.
/// </summary>
public abstract class XUnitTest : XUnitTestBase, IXUnitTest
{
    #region IXUnitTest Implementation
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
    protected virtual void Arrange()
    {
    }

    protected virtual void Act()
    {
    }

    protected virtual void Assert()
    {
    }
    #endregion
}