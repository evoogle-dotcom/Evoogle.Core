// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Xunit;

namespace Evoogle.XUnit;

/// <summary>
///     Abstracts an xUnit tests container.
/// </summary>
/// <param name="output">xUnit helper object to provide text output with.</param>
/// <remarks>
///     Derived classes should annotate methods with either xUnit Theories or Facts as needed.
/// </remarks>
public abstract class XUnitTests(ITestOutputHelper output)
{
    #region Fields
    private const string _singleDashedLine = "----------------------------------------";
    private const string _doubleDashedLine = "========================================";

    // Support both "1" and "true" to make CI configuration easier.
    private static readonly bool _suppressOutput =
        string.Equals(Environment.GetEnvironmentVariable("XUNIT_SUPPRESS_OUTPUT"), "1", StringComparison.Ordinal) ||
        (bool.TryParse(Environment.GetEnvironmentVariable("XUNIT_SUPPRESS_OUTPUT"), out var b) && b);
    #endregion

    #region Properties
    protected ITestOutputHelper Output { get; } = output;
    #endregion

    #region Helper API used by test instances
    internal void WriteLine(string message)
    {
        if (_suppressOutput) return;
        this.Output.WriteLine(message);
    }

    internal void WriteDashedLine()
    {
        if (_suppressOutput) return;
        this.Output.WriteLine(_singleDashedLine);
    }

    internal void WriteDoubleDashedLine()
    {
        if (_suppressOutput) return;
        this.Output.WriteLine(_doubleDashedLine);
    }
    #endregion

    #region Convenience runners (optional)
    protected void Run(IXUnitTest test)
    {
        ArgumentNullException.ThrowIfNull(test);
        test.Execute(this);
    }

    protected Task RunAsync(IXUnitTestAsync test)
    {
        ArgumentNullException.ThrowIfNull(test);
        return test.ExecuteAsync(this);
    }
    #endregion
}
