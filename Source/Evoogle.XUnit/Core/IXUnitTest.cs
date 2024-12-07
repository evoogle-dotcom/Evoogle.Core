// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Xunit.Abstractions;

namespace Evoogle.XUnit;

/// <summary>
///     Abstracts an individual named xunit test that is executed inside an xunit tests container.
/// </summary>
public interface IXUnitTest : IXunitSerializable
{
    #region Properties
    /// <summary>Gets the name of the individual xunit test.</summary>
    string Name { get; }
    #endregion

    #region Methods
    /// <summary>Exectute the named individual xunit test.</summary>
    /// <param name="parent">Parent xunit texts container to execute this xunit unit test within.</param>
    void Execute(XUnitTests parent);
    #endregion
}