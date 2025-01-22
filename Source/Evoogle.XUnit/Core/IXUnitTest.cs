// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle.XUnit;

/// <summary>
///     Abstracts an individual named xUnit test that is executed inside an xUnit tests container.
/// </summary>
public interface IXUnitTest
{
    #region Properties
    /// <summary>Gets the name of the individual xUnit test.</summary>
    string Name { get; }
    #endregion

    #region Methods
    /// <summary>Exectute the named individual xUnit test.</summary>
    /// <param name="parent">Parent xUnit tests container to execute this xUnit unit test within.</param>
    void Execute(XUnitTests parent);
    #endregion
}