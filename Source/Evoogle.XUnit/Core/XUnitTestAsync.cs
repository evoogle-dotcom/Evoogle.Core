// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle.XUnit;

/// <summary>
///     Captures boilerplate code for an individual named xunit test to break the unit test into explicit arrange, act, asert steps
///     that are executed asynchronously in the context of an xunit tests container.
/// </summary>
public abstract class XUnitTestAsync : XUnitTestBase, IXUnitTestAsync
{
    #region IXUnitTestAsync Implementation
    public virtual async Task ExecuteAsync(XUnitTests parent)
    {
        this.Parent = parent;

        this.WriteLine($"Test Name: {this.Name}");
        this.WriteDashedLine();

        await this.ArrangeAsync();
        await this.ActAsync();
        await this.AssertAsync();
    }
    #endregion

    #region XUnitTestAsync Overrides
    protected virtual Task ArrangeAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual Task ActAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual Task AssertAsync()
    {
        return Task.CompletedTask;
    }
    #endregion
}