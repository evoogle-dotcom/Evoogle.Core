// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Xunit.Abstractions;

namespace Evoogle.XUnit;

/// <summary>
///     Common baseclass for <see cref="XUnitTest" /> and <see cref="XUnitTestAsync" /> respectively.
/// </summary>
public abstract class XUnitTestBase : IXunitSerializable
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
    public override string ToString()
    {
        return this.Name;
    }
    #endregion

    #region IXunitSerializable Implementation
    public virtual void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue("Name", this.Name);
    }

    public virtual void Deserialize(IXunitSerializationInfo info)
    { }
    #endregion

    #region Write Methods
    protected void WriteLine()
    {
        this.Parent?.WriteLine();
    }

    protected void WriteLine(string message)
    {
        this.Parent?.WriteLine(message);
    }

    protected void WriteDashedLine()
    {
        this.Parent?.WriteDashedLine();
    }
    #endregion
}