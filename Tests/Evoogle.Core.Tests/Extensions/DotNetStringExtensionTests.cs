// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

using FluentAssertions;

namespace Evoogle.Extensions;

public class DotNetStringExtensionTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public class MaskTest : XUnitTest
    {
        #region User Supplied Properties
        public string? Source { get; init; }
        public string? Expected { get; init; }
        public char MaskChar { get; init; } = '*';
        public int UnmaskedLeftCount { get; init; } = 1;
        public int? UnmaskedRightCount { get; init; } = null;
        public int MinMaskedCount { get; init; } = 8;
        #endregion

        #region Calculated Properties
        private string? Actual { get; set; }
        #endregion

        protected override void Arrange()
        {
            this.WriteLine($"Source: {this.Source.SafeToString()}");
            this.WriteLine();
            this.WriteLine($"Expected: {this.Expected.SafeToString()}");
        }

        protected override void Act()
        {
            this.Actual = this.Source.Mask(this.MaskChar, this.UnmaskedLeftCount, this.UnmaskedRightCount, this.MinMaskedCount);
            this.WriteLine($"Actual:   {this.Actual.SafeToString()}");
        }

        protected override void Assert() => this.Actual.Should().Be(this.Expected);
    }

    public class RemoveWhitespaceTest : XUnitTest
    {
        public string? Source { get; init; }
        public string? Expected { get; init; }
        private string? Actual { get; set; }

        protected override void Act() => this.Actual = this.Source.RemoveWhitespace();

        protected override void Assert() => this.Actual.Should().Be(this.Expected);
    }
    #endregion

    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] MaskTheoryData =>
    [
        new MaskTest { Name = "Null String", Source = null, Expected = null},
        new MaskTest { Name = "Empty String", Source = string.Empty, Expected = string.Empty },

        new MaskTest { Name = "01 Character String", Source = "0", Expected = "*" },
        new MaskTest { Name = "02 Character String", Source = "01", Expected = "**" },
        new MaskTest { Name = "03 Character String", Source = "012", Expected = "***" },
        new MaskTest { Name = "04 Character String", Source = "0123", Expected = "****" },
        new MaskTest { Name = "05 Character String", Source = "01234", Expected = "*****" },
        new MaskTest { Name = "06 Character String", Source = "012345", Expected = "******" },
        new MaskTest { Name = "07 Character String", Source = "0123456", Expected = "*******" },
        new MaskTest { Name = "08 Character String", Source = "01234567", Expected = "********" },
        new MaskTest { Name = "09 Character String", Source = "012345678", Expected = "********8" },
        new MaskTest { Name = "10 Character String", Source = "0123456789", Expected = "********89" },
        new MaskTest { Name = "11 Character String", Source = "01234567890", Expected = "0********90" },
        new MaskTest { Name = "12 Character String", Source = "012345678901", Expected = "0********901" },
        new MaskTest { Name = "13 Character String", Source = "0123456789012", Expected = "0*********012" },
        new MaskTest { Name = "14 Character String", Source = "01234567890123", Expected = "0*********0123" },
        new MaskTest { Name = "15 Character String", Source = "012345678901234", Expected = "0**********1234" },
        new MaskTest { Name = "16 Character String", Source = "0123456789012345", Expected = "0***********2345" },

        new MaskTest { Name = "01 Character String With MaskChar='?' and UnmaskedLeftCount=3 and UnmaskedRightCount=5 and MinMaskedCount=4", Source = "0", Expected = "?", MaskChar = '?', UnmaskedLeftCount = 3, UnmaskedRightCount = 5, MinMaskedCount = 4 },
        new MaskTest { Name = "02 Character String With MaskChar='?' and UnmaskedLeftCount=3 and UnmaskedRightCount=5 and MinMaskedCount=4", Source = "01", Expected = "??", MaskChar = '?', UnmaskedLeftCount = 3, UnmaskedRightCount = 5, MinMaskedCount = 4 },
        new MaskTest { Name = "03 Character String With MaskChar='?' and UnmaskedLeftCount=3 and UnmaskedRightCount=5 and MinMaskedCount=4", Source = "012", Expected = "???", MaskChar = '?', UnmaskedLeftCount = 3, UnmaskedRightCount = 5, MinMaskedCount = 4 },
        new MaskTest { Name = "04 Character String With MaskChar='?' and UnmaskedLeftCount=3 and UnmaskedRightCount=5 and MinMaskedCount=4", Source = "0123", Expected = "????", MaskChar = '?', UnmaskedLeftCount = 3, UnmaskedRightCount = 5, MinMaskedCount = 4 },
        new MaskTest { Name = "05 Character String With MaskChar='?' and UnmaskedLeftCount=3 and UnmaskedRightCount=5 and MinMaskedCount=4", Source = "01234", Expected = "????4", MaskChar = '?', UnmaskedLeftCount = 3, UnmaskedRightCount = 5, MinMaskedCount = 4 },
        new MaskTest { Name = "06 Character String With MaskChar='?' and UnmaskedLeftCount=3 and UnmaskedRightCount=5 and MinMaskedCount=4", Source = "012345", Expected = "????45", MaskChar = '?', UnmaskedLeftCount = 3, UnmaskedRightCount = 5, MinMaskedCount = 4 },
        new MaskTest { Name = "07 Character String With MaskChar='?' and UnmaskedLeftCount=3 and UnmaskedRightCount=5 and MinMaskedCount=4", Source = "0123456", Expected = "????456", MaskChar = '?', UnmaskedLeftCount = 3, UnmaskedRightCount = 5, MinMaskedCount = 4 },
        new MaskTest { Name = "08 Character String With MaskChar='?' and UnmaskedLeftCount=3 and UnmaskedRightCount=5 and MinMaskedCount=4", Source = "01234567", Expected = "????4567", MaskChar = '?', UnmaskedLeftCount = 3, UnmaskedRightCount = 5, MinMaskedCount = 4 },
        new MaskTest { Name = "09 Character String With MaskChar='?' and UnmaskedLeftCount=3 and UnmaskedRightCount=5 and MinMaskedCount=4", Source = "012345678", Expected = "????45678", MaskChar = '?', UnmaskedLeftCount = 3, UnmaskedRightCount = 5, MinMaskedCount = 4 },
        new MaskTest { Name = "10 Character String With MaskChar='?' and UnmaskedLeftCount=3 and UnmaskedRightCount=5 and MinMaskedCount=4", Source = "0123456789", Expected = "0????56789", MaskChar = '?', UnmaskedLeftCount = 3, UnmaskedRightCount = 5, MinMaskedCount = 4 },
        new MaskTest { Name = "11 Character String With MaskChar='?' and UnmaskedLeftCount=3 and UnmaskedRightCount=5 and MinMaskedCount=4", Source = "01234567890", Expected = "01????67890", MaskChar = '?', UnmaskedLeftCount = 3, UnmaskedRightCount = 5, MinMaskedCount = 4 },
        new MaskTest { Name = "12 Character String With MaskChar='?' and UnmaskedLeftCount=3 and UnmaskedRightCount=5 and MinMaskedCount=4", Source = "012345678901", Expected = "012????78901", MaskChar = '?', UnmaskedLeftCount = 3, UnmaskedRightCount = 5, MinMaskedCount = 4 },
        new MaskTest { Name = "13 Character String With MaskChar='?' and UnmaskedLeftCount=3 and UnmaskedRightCount=5 and MinMaskedCount=4", Source = "0123456789012", Expected = "012?????89012", MaskChar = '?', UnmaskedLeftCount = 3, UnmaskedRightCount = 5, MinMaskedCount = 4 },
        new MaskTest { Name = "14 Character String With MaskChar='?' and UnmaskedLeftCount=3 and UnmaskedRightCount=5 and MinMaskedCount=4", Source = "01234567890123", Expected = "012??????90123", MaskChar = '?', UnmaskedLeftCount = 3, UnmaskedRightCount = 5, MinMaskedCount = 4 },
        new MaskTest { Name = "15 Character String With MaskChar='?' and UnmaskedLeftCount=3 and UnmaskedRightCount=5 and MinMaskedCount=4", Source = "012345678901234", Expected = "012???????01234", MaskChar = '?', UnmaskedLeftCount = 3, UnmaskedRightCount = 5, MinMaskedCount = 4 },
        new MaskTest { Name = "16 Character String With MaskChar='?' and UnmaskedLeftCount=3 and UnmaskedRightCount=5 and MinMaskedCount=4", Source = "0123456789012345", Expected = "012????????12345", MaskChar = '?', UnmaskedLeftCount = 3, UnmaskedRightCount = 5, MinMaskedCount = 4 },
    ];

    public static TheoryDataRow<IXUnitTest>[] RemoveWhitespaceTheoryData =>
    [
        new RemoveWhitespaceTest { Name = "Null String", Source = null, Expected = null },
        new RemoveWhitespaceTest { Name = "Empty String", Source = string.Empty, Expected = string.Empty },
        new RemoveWhitespaceTest { Name = "No Whitespace In String", Source = "helloworld", Expected = "helloworld" },
        new RemoveWhitespaceTest { Name = "Whitespace In String", Source = "\thello world\n", Expected = "helloworld" },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(MaskTheoryData))]
    public void Mask(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(RemoveWhitespaceTheoryData))]
    public void RemoveWhitespace(IXUnitTest test) => test.Execute(this);
    #endregion
}
