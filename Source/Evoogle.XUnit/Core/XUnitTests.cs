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
    #endregion

    #region Properties
    private ITestOutputHelper Output { get; } = output;
    #endregion

    #region Write Methods
    internal void WriteLine()
    {
        this.Output.WriteLine(string.Empty);
    }

    internal void WriteLine(string message)
    {
        this.Output.WriteLine(message);
    }

    internal void WriteDashedLine()
    {
        this.Output.WriteLine(SingleDashedLine);
    }

    internal void WriteDoubleDashedLine()
    {
        this.Output.WriteLine(DoubleDashedLine);
    }
    #endregion
}