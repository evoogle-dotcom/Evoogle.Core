// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle.XUnit;

/// <summary>
///     Captures boilerplate code for an individual named xUnit test to break the unit test into explicit arrange, act, asert steps
///     that are executed asynchronously in the context of an xUnit tests container.
/// </summary>
public abstract class XUnitTestAsync : XUnitTestBase, IXUnitTestAsync
{
    #region IXUnitTestAsync Implementation
    /// <summary>
    ///     Executes the unit test asynchronously by writing the name of the unit test followed by calling the arrange, act, and assert steps of the unit test.
    /// </summary>
    /// <param name="parent">Parent unit tests container object.</param>
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
    /// <summary>Noop implementation of the arrange step.</summary>
    protected virtual Task ArrangeAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>Noop implementation of the act step.</summary>
    protected virtual Task ActAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>Noop implementation of the assert step.</summary>
    protected virtual Task AssertAsync()
    {
        return Task.CompletedTask;
    }
    #endregion
}