// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Globalization;
using System.Text.Json;

using Evoogle.Extensions;
using Evoogle.XUnit;

using FluentAssertions;

namespace Evoogle.Json;

public class CultureInfoJsonConverterTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public enum TestCultureEnum
    {
        None,
        EnUs,
        FrFr
    }

    public class JsonDeserializeTest : XUnitTest
    {
        #region User Supplied Properties
        public string? SourceJson { get; init; }
        public TestCultureEnum? ExpectedCultureEnum { get; init; }
        public string? ExpectedException { get; init; }
        #endregion

        #region Calculated Properties
        private CultureInfo? ExpectedCultureInfo { get; set; }
        private CultureInfo? ActualCultureInfo { get; set; }
        private string? ActualException { get; set; }

        private static JsonSerializerOptions Options { get; } = new JsonSerializerOptions { Converters = { new CultureInfoJsonConverter() } };
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            this.WriteLine($"Source JSON:          {this.SourceJson.SafeToString()}");
            this.WriteLine($"Expected CultureEnum: {this.ExpectedCultureEnum.SafeToString()}");

            this.ExpectedCultureInfo = GetCultureInfoFromEnum(this.ExpectedCultureEnum);
            this.WriteLine($"Expected CultureInfo: {this.ExpectedCultureInfo.SafeToString()}");
            this.WriteLine();

            if (this.ExpectedException != null)
            {
                this.WriteLine($"Expected Exception: {this.ExpectedException.SafeToString()}");
                this.WriteLine();
            }

            this.ExpectedCultureInfo = GetCultureInfoFromEnum(this.ExpectedCultureEnum);
        }

        protected override void Act()
        {
            try
            {
                this.ActualCultureInfo = JsonSerializer.Deserialize<CultureInfo?>(this.SourceJson!, Options);
                this.WriteLine($"Actual CultureInfo: {this.ActualCultureInfo.SafeToString()}");
            }
            catch (Exception exception)
            {
                var actualException = exception.GetType().Name;
                this.ActualException = actualException;

                this.WriteLine($"Actual Exception: {this.ActualException.SafeToString()}");
            }
        }

        protected override void Assert()
        {
            if (this.ExpectedCultureInfo != null)
            {
                this.ActualException.Should().BeNull();

                this.ActualCultureInfo.Should().NotBeNull();
                this.ActualCultureInfo.Should().Be(this.ExpectedCultureInfo);
            }
            else if (this.ExpectedException != null)
            {
                this.ActualCultureInfo.Should().BeNull();

                this.ActualException.Should().NotBeNull();
                this.ActualException.Should().Be(this.ExpectedException);
            }
        }
        #endregion
    }

    public class JsonSerializeTest : XUnitTest
    {
        #region User Supplied Properties
        public TestCultureEnum SourceCultureEnum { get; init; }
        public string? ExpectedJson { get; init; }
        #endregion

        #region Calculated Properties
        private CultureInfo? SourceCultureInfo { get; set; }
        private string? ActualJson { get; set; }
        private static JsonSerializerOptions Options { get; } = new JsonSerializerOptions { Converters = { new CultureInfoJsonConverter() } };
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            this.WriteLine($"Source CultureEnum: {this.SourceCultureEnum.SafeToString()}");

            this.SourceCultureInfo = GetCultureInfoFromEnum(this.SourceCultureEnum);
            this.WriteLine($"Source CultureInfo: {this.SourceCultureInfo.SafeToString()}");
            this.WriteLine();

            this.WriteLine($"Expected JSON: {this.ExpectedJson.SafeToString()}");
        }

        protected override void Act()
        {
            this.ActualJson = JsonSerializer.Serialize(this.SourceCultureInfo, Options);
            this.WriteLine($"Actual   JSON: {this.ActualJson.SafeToString()}");
        }

        protected override void Assert()
        {
            var actualJsonMinusWhitespace = this.ActualJson.RemoveWhitespace();
            var expectedJsonMinusWhitespace = this.ExpectedJson.RemoveWhitespace();

            actualJsonMinusWhitespace.Should().Be(expectedJsonMinusWhitespace);
        }
        #endregion
    }
    #endregion

    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] DeserializeTheoryData =>
    [
        // Should Deserialize From Null Literal Successfully
        new JsonDeserializeTest
        {
            Name = $"Null Literal Should Deserialize To CultureInfo: {_nullCulture.SafeToString()}",
            SourceJson = @"null",
            ExpectedCultureEnum = TestCultureEnum.None
        },

        // Should Deserialize From JSON String Successfully
        new JsonDeserializeTest
        {
            Name = $"en-US String Should Deserialize To CultureInfo: {_enUsCulture.SafeToString()}",
            SourceJson = @"""en-US""",
            ExpectedCultureEnum = TestCultureEnum.EnUs
        },
        new JsonDeserializeTest
        {
            Name = $"fr-FR String Should Deserialize To CultureInfo: {_frFrCulture.SafeToString()}",
            SourceJson = @"""fr-FR""",
            ExpectedCultureEnum = TestCultureEnum.FrFr
        },

        new JsonDeserializeTest
        {
            Name = $"Empty String Should Deserialize To CultureInfo: {_nullCulture.SafeToString()}",
            SourceJson = @"""""",
            ExpectedCultureEnum = TestCultureEnum.None
        },
        new JsonDeserializeTest
        {
            Name = $"Whitespace String Should Deserialize To CultureInfo: {_nullCulture.SafeToString()}",
            SourceJson = @"""       """,
            ExpectedCultureEnum = TestCultureEnum.None
        },

        // Should Throw Exception From Invalid String Name
        new JsonDeserializeTest
        {
            Name = "FooBar String Should Throw JsonException",
            SourceJson = @"""FooBar""",
            ExpectedException = nameof(JsonException)
        },
    ];

    public static TheoryDataRow<IXUnitTest>[] SerializeTheoryData =>
    [
        // Should Serialize To JSON String Successfully
        new JsonSerializeTest
        {
            Name = $"CultureInfo: {_nullCulture.SafeToString()} Should Serialize To Null Literal",
            SourceCultureEnum = TestCultureEnum.None,
            ExpectedJson = @"null"
        },
        new JsonSerializeTest
        {
            Name = $"CultureInfo: {_enUsCulture.SafeToString()} Should Serialize To JSON String - en-US",
            SourceCultureEnum = TestCultureEnum.EnUs,
            ExpectedJson = @"""en-US"""
        },
        new JsonSerializeTest
        {
            Name = $"CultureInfo: {_frFrCulture.SafeToString()} Should Serialize To JSON String - fr-FR",
            SourceCultureEnum = TestCultureEnum.FrFr,
            ExpectedJson = @"""fr-FR"""
        },
    ];

    private static readonly CultureInfo? _nullCulture = null;
    private static readonly CultureInfo _enUsCulture = new("en-US");
    private static readonly CultureInfo _frFrCulture = new("fr-FR");
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(DeserializeTheoryData))]
    public void Deserialize(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(SerializeTheoryData))]
    public void Serialize(IXUnitTest test) => test.Execute(this);
    #endregion

    #region Helper Methods
    private static CultureInfo? GetCultureInfoFromEnum(TestCultureEnum? cultureEnum) => cultureEnum switch
    {
        null => null,
        TestCultureEnum.None => null,
        TestCultureEnum.EnUs => new CultureInfo("en-US"),
        TestCultureEnum.FrFr => new CultureInfo("fr-FR"),
        _ => throw new InvalidOperationException($"Unsupported culture enum value: {cultureEnum}"),
    };
    #endregion
}
