// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Evoogle.XUnit;

using FluentAssertions;

using Xunit.Abstractions;

namespace Evoogle;

public class DotNetStringExtensionTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public class MaskTest : XUnitTest
    {
        public string? Source { get; set; }
        public string? Expected { get; set; }
        private string? Actual { get; set; }

        protected override void Act()
        {
            this.Actual = this.Source!.Mask();
        }

        protected override void Assert()
        {
            this.Actual.Should().Be(this.Expected);
        }
    }

    public class RemoveWhitespaceTest : XUnitTest
    {
        public string? Source { get; set; }
        public string? Expected { get; set; }
        private string? Actual { get; set; }

        protected override void Act()
        {
            this.Actual = this.Source!.RemoveWhitespace();
        }

        protected override void Assert()
        {
            this.Actual.Should().Be(this.Expected);
        }
    }
    #endregion

    #region Theory Data
    public static TheoryData<IXUnitTest> MaskTheoryData => new()
    {
        { new MaskTest {Name = "Null String", Source = null, Expected = null} },
        { new MaskTest {Name = "Empty String", Source = string.Empty, Expected = string.Empty } },
        { new MaskTest {Name = "01 Character String", Source = "0", Expected = "*" } },
        { new MaskTest {Name = "02 Character String", Source = "01", Expected = "**" } },
        { new MaskTest {Name = "03 Character String", Source = "012", Expected = "***" } },
        { new MaskTest {Name = "04 Character String", Source = "0123", Expected = "****" } },
        { new MaskTest {Name = "05 Character String", Source = "01234", Expected = "0****" } },
        { new MaskTest {Name = "06 Character String", Source = "012345", Expected = "0*****" } },
        { new MaskTest {Name = "07 Character String", Source = "0123456", Expected = "0******" } },
        { new MaskTest {Name = "08 Character String", Source = "01234567", Expected = "0******7" } },
        { new MaskTest {Name = "09 Character String", Source = "012345678", Expected = "0*******8" } },
        { new MaskTest {Name = "10 Character String", Source = "0123456789", Expected = "0*******89" } },
        { new MaskTest {Name = "11 Character String", Source = "01234567890", Expected = "0********90" } },
        { new MaskTest {Name = "12 Character String", Source = "012345678901", Expected = "0********901" } },
        { new MaskTest {Name = "13 Character String", Source = "0123456789012", Expected = "0*********012" } },
        { new MaskTest {Name = "14 Character String", Source = "01234567890123", Expected = "0*********0123" } },
        { new MaskTest {Name = "15 Character String", Source = "012345678901234", Expected = "0**********1234" } },
        { new MaskTest {Name = "16 Character String", Source = "0123456789012345", Expected = "0***********2345" } },
    };

    public static TheoryData<IXUnitTest> RemoveWhitespaceTheoryData => new()
    {
        { new RemoveWhitespaceTest {Name = "Null String", Source = null, Expected = null} },
        { new RemoveWhitespaceTest {Name = "Empty String", Source = string.Empty, Expected = string.Empty } },
        { new RemoveWhitespaceTest {Name = "No Whitespace In String", Source = "helloworld", Expected = "helloworld" } },
        { new RemoveWhitespaceTest {Name = "Whitespace In String", Source = "\thello world\n", Expected = "helloworld" } },
    };
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(MaskTheoryData))]
    public void Mask(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(RemoveWhitespaceTheoryData))]
    public void RemoveWhitespace(IXUnitTest test)
    {
        test.Execute(this);
    }
    #endregion
}