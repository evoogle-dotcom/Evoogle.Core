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
///     Derived classes should annotate methods with either xUnit theory or facts as needed.
/// </remarks>
public abstract class XUnitTests(ITestOutputHelper output)
{
    #region Fields
    private const string DoubleDashedLine =
        "=============================================================================";

    private const string SingleDashedLine =
        "-----------------------------------------------------------------------------";

    private static readonly bool _suppressOutput = Environment.GetEnvironmentVariable("XUNIT_SUPPRESS_OUTPUT") == "1";
    #endregion

    #region Properties
    private ITestOutputHelper Output { get; } = output;
    #endregion

    #region Write Methods
    internal void WriteLine()
    {
        // If the output is suppressed, do not write anything.
        // This is useful for CI environments where output can be noisy.
        if (_suppressOutput)
        {
            return;
        }

        this.Output.WriteLine(string.Empty);
    }

    internal void WriteLine(string message)
    {
        // If the output is suppressed, do not write anything.
        // This is useful for CI environments where output can be noisy.
        if (_suppressOutput)
        {
            return;
        }

        this.Output.WriteLine(message);
    }

    internal void WriteDashedLine()
    {
        // If the output is suppressed, do not write anything.
        // This is useful for CI environments where output can be noisy.
        if (_suppressOutput)
        {
            return;
        }

        this.Output.WriteLine(SingleDashedLine);
    }

    internal void WriteDoubleDashedLine()
    {
        // If the output is suppressed, do not write anything.
        // This is useful for CI environments where output can be noisy.
        if (_suppressOutput)
        {
            return;
        }

        this.Output.WriteLine(DoubleDashedLine);
    }
    #endregion
}
