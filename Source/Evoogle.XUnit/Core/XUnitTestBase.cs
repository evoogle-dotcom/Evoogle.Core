// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle.XUnit;

/// <summary>
///     Common baseclass for <see cref="XUnitTest" /> and <see cref="XUnitTestAsync" /> respectively.
/// </summary>
public abstract class XUnitTestBase
{
    #region Properties
    public string Name { get; set; }

    protected XUnitTests? Parent { get; set; }
    #endregion

    #region Constructors
    protected XUnitTestBase()
    {
        this.Name = this.GetType().Name;
    }
    #endregion

    #region Object Overrides
    /// <summary>Returns the name of the unit test.</summary>
    /// <returns>Name of the unit test.</returns>
    public override string ToString()
    {
        return this.Name;
    }
    #endregion

    #region Write Methods
    /// <summary>Writes an empty line to the test text output.</summary>
    protected void WriteLine()
    {
        this.Parent?.WriteLine();
    }

    /// <summary>
    ///     Writes a message line to the test text output.
    /// </summary>
    /// <param name="message">Message to be output to the test text output.</param>
    protected void WriteLine(string message)
    {
        this.Parent?.WriteLine(message);
    }

    /// <summary>Writes a dashed line to the test text output.</summary>
    protected void WriteDashedLine()
    {
        this.Parent?.WriteDashedLine();
    }

    /// <summary>Writes a double dashed line to the test text output.</summary>
    public void WriteDoubleDashedLine()
    {
        this.Parent?.WriteDoubleDashedLine();
    }
    #endregion
}