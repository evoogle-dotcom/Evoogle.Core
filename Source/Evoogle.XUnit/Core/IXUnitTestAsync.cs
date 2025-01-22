// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle.XUnit;

/// <summary>
///     Abstracts an individual xUnit test that is executed asynchronously inside an xUnit tests container.
/// </summary>
public interface IXUnitTestAsync
{
    #region Properties
    /// <summary>Gets the name of the individual xUnit test.</summary>
    string Name { get; }
    #endregion

    #region Methods
    /// <summary>Exectute asynchronously the named individual xUnit test.</summary>
    /// <param name="parent">Parent xUnit tests container to execute this xUnit unit test within.</param>
    Task ExecuteAsync(XUnitTests parent);
    #endregion
}
