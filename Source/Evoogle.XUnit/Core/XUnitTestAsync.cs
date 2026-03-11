// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.XUnit;

/// <summary>
///     Captures boilerplate code for an individual named xUnit test (async) using
///     explicit Arrange, Act, Assert steps executed in an xUnit tests container.
/// </summary>
public abstract class XUnitTestAsync : XUnitTestBase, IXUnitTestAsync
{
    #region IXUnitTestAsync Methods
    /// <summary>
    ///     Executes the unit test asynchronously by writing the test header and calling the Arrange/Act/Assert steps.
    /// </summary>
    /// <param name="parent">The parent tests container.</param>
    public async Task ExecuteAsync(XUnitTests parent)
    {
        this.SetParent(parent);

        this.WriteDashedLine();
        this.WriteLine($"Test Name: {this.Name}");
        this.WriteLine();

        await this.ArrangeAsync().ConfigureAwait(false);
        await this.ActAsync().ConfigureAwait(false);
        await this.AssertAsync().ConfigureAwait(false);

        this.WriteLine();
    }
    #endregion

    #region Arrange/Act/Assert Async Methods
    /// <summary>No-op implementation of the arrange step.</summary>
    protected virtual Task ArrangeAsync() => Task.CompletedTask;

    /// <summary>No-op implementation of the act step.</summary>
    protected virtual Task ActAsync() => Task.CompletedTask;

    /// <summary>No-op implementation of the assert step.</summary>
    protected virtual Task AssertAsync() => Task.CompletedTask;
    #endregion
}
