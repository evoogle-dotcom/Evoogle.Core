// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Evoogle.XUnit;

using FluentAssertions;

namespace Evoogle;

public class DotNetObjectExtensionTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public class SafeToStringTest : XUnitTest
    {
        public object? Source { get; set; }
        public string? Expected { get; set; }
        public string? NullText { get; set; }
        public string? EmtpyText { get; set; }
        private string? Actual { get; set; }

        protected override void Act()
        {
            this.Actual = this.Source!.SafeToString(this.EmtpyText!, this.NullText!);
        }

        protected override void Assert()
        {
            this.Actual.Should().Be(this.Expected);
        }
    }
    #endregion

    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] SafeToStringTheoryData =>
    [
        new SafeToStringTest {Name = "Null String", Source = null, Expected = "<null>"},
        new SafeToStringTest {Name = "Null String And Customized Null Text", Source = null, Expected = "NULL!", NullText = "NULL!"},
        new SafeToStringTest {Name = "Empty String", Source = string.Empty, Expected = "<empty>" },
        new SafeToStringTest {Name = "Empty String and Customized Empty Text", Source = string.Empty, Expected = "EMPTY!", EmtpyText = "EMPTY!" },
        new SafeToStringTest {Name = "Non Null String", Source = "helloworld", Expected = "helloworld" },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(SafeToStringTheoryData))]
    public void SafeToString(IXUnitTest test)
    {
        test.Execute(this);
    }
    #endregion
}