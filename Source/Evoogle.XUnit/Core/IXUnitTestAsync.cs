// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Xunit.Sdk;

namespace Evoogle.XUnit;

/// <summary>
///     Abstracts an individual xunit test that is executed asynchronously inside an xunit tests container.
/// </summary>
public interface IXUnitTestAsync : IXunitSerializable
{
    #region Properties
    /// <summary>Gets the name of the individual xunit test.</summary>
    string Name { get; }
    #endregion

    #region Methods
    /// <summary>Exectute asynchronously the named individual xunit test.</summary>
    /// <param name="parent">Parent xunit texts container to execute this xunit unit test within.</param>
    Task ExecuteAsync(XUnitTests parent);
    #endregion
}
