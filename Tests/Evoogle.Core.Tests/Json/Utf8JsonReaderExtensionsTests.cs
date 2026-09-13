// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Collections.Immutable;
using System.Text;
using System.Text.Json;

using Evoogle.XUnit;

using FluentAssertions;

namespace Evoogle.Json;

public class Utf8JsonReaderExtensionsTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public class ReadObjectPropertyNamesTest : XUnitTest
    {
        #region User Supplied Properties
        public required string SourceJson { get; init; } = null!;
        public bool IsFinalBlock { get; init; } = true;
        public ImmutableArray<string>? ExpectedPropertyNames { get; init; }
        public Type? ExpectedExceptionType { get; init; }
        public string? ExpectedExceptionMessage { get; init; }
        #endregion

        #region Calculated Properties
        private ImmutableArray<string>? ActualPropertyNames { get; set; }
        private Exception? ActualException { get; set; }
        private JsonTokenType? InitialTokenType { get; set; }
        private int? InitialDepth { get; set; }
        private long? InitialBytesConsumed { get; set; }
        private long? InitialTokenStartIndex { get; set; }
        private JsonTokenType? ActualTokenType { get; set; }
        private int? ActualDepth { get; set; }
        private long? ActualBytesConsumed { get; set; }
        private long? ActualTokenStartIndex { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            this.WriteLine($"Source JSON: {this.SourceJson}");
            this.WriteLine($"Is Final Block: {this.IsFinalBlock}");
        }

        protected override void Act()
        {
            var sourceBytes = Encoding.UTF8.GetBytes(this.SourceJson);
            var reader = new Utf8JsonReader(sourceBytes, this.IsFinalBlock, state: default);

            if (!reader.Read())
            {
                throw new InvalidOperationException("Expected the reader to read an initial JSON token.");
            }

            this.InitialTokenType = reader.TokenType;
            this.InitialDepth = reader.CurrentDepth;
            this.InitialBytesConsumed = reader.BytesConsumed;
            this.InitialTokenStartIndex = reader.TokenStartIndex;

            try
            {
                this.ActualPropertyNames = reader.ReadObjectPropertyNames();
            }
            catch (Exception exception)
            {
                this.ActualException = exception;
            }

            this.ActualTokenType = reader.TokenType;
            this.ActualDepth = reader.CurrentDepth;
            this.ActualBytesConsumed = reader.BytesConsumed;
            this.ActualTokenStartIndex = reader.TokenStartIndex;
        }

        protected override void Assert()
        {
            if (this.ExpectedExceptionType == null)
            {
                this.ActualException.Should().BeNull();
                this.ActualPropertyNames.Should().Equal(this.ExpectedPropertyNames);
            }
            else
            {
                this.ActualException.Should().BeOfType(this.ExpectedExceptionType);
                this.ActualException!.Message.Should().Be(this.ExpectedExceptionMessage);
            }

            this.ActualTokenType.Should().Be(this.InitialTokenType);
            this.ActualDepth.Should().Be(this.InitialDepth);
            this.ActualBytesConsumed.Should().Be(this.InitialBytesConsumed);
            this.ActualTokenStartIndex.Should().Be(this.InitialTokenStartIndex);
        }
        #endregion
    }
    #endregion

    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] ReadObjectPropertyNamesTheoryData =>
    [
        new ReadObjectPropertyNamesTest
        {
            Name = "Empty Object Should Return No Property Names",
            SourceJson = "{}",
            ExpectedPropertyNames = [],
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Object With Scalar Values Should Return Property Names In Source Order",
            SourceJson = "{\"name\":\"value\",\"count\":1,\"none\":null}",
            ExpectedPropertyNames = ["name", "count", "none"],
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Object With Nested Values Should Return Only Direct Property Names",
            SourceJson = "{\"first\":{\"nested\":\"value\"},\"second\":[1,{\"nested\":true}],\"third\":false}",
            ExpectedPropertyNames = ["first", "second", "third"],
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Object With Escaped Property Name Should Return Decoded Property Name",
            SourceJson = "{\"na\\u006de\":\"value\"}",
            ExpectedPropertyNames = ["name"],
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Array Token Should Throw JsonException",
            SourceJson = "[]",
            ExpectedExceptionType = typeof(JsonException),
            ExpectedExceptionMessage = "Expected start of JSON object.",
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Scalar Token Should Throw JsonException",
            SourceJson = "\"value\"",
            ExpectedExceptionType = typeof(JsonException),
            ExpectedExceptionMessage = "Expected start of JSON object.",
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Incomplete Nonfinal Object Before First Property Should Throw JsonException",
            SourceJson = "{",
            IsFinalBlock = false,
            ExpectedExceptionType = typeof(JsonException),
            ExpectedExceptionMessage = "Unexpected end of JSON.",
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Incomplete Nonfinal Object After Property Name Should Throw JsonException",
            SourceJson = "{\"name\"",
            IsFinalBlock = false,
            ExpectedExceptionType = typeof(JsonException),
            ExpectedExceptionMessage = "Unexpected end of JSON.",
        },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(ReadObjectPropertyNamesTheoryData))]
    public void ReadObjectPropertyNames(IXUnitTest test) => test.Execute(this);
    #endregion
}
