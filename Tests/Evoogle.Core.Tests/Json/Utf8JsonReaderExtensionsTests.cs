// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Collections.Immutable;
using System.Text;
using System.Text.Json;
using Evoogle.Extensions;
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
        public JsonReaderNullPropertyHandling NullHandling { get; init; }
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
            this.WriteLine($"SourceJSON: {this.SourceJson}");
            this.WriteLine($"IsFinalBlock: {this.IsFinalBlock}");
            this.WriteLine($"NullHandling: {this.NullHandling}");
            this.WriteLine();

            if (this.ExpectedPropertyNames != null)
            {
                this.WriteLine($"ExpectedPropertyNames: [{string.Join(", ", this.ExpectedPropertyNames)}]");
            }
            if (this.ExpectedExceptionType != null)
            {
                this.WriteLine($"ExpectedExceptionType:    {this.ExpectedExceptionType.SafeToName()}");
            }
            if (this.ExpectedExceptionMessage != null)
            {
                this.WriteLine($"ExpectedExceptionMessage: {this.ExpectedExceptionMessage.SafeToString()}");
            }
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
                this.ActualPropertyNames = reader.ReadObjectPropertyNames(this.NullHandling);
                if (this.ActualPropertyNames != null)
                {
                    this.WriteLine($"ActualPropertyNames:   [{string.Join(", ", this.ActualPropertyNames)}]");
                }
            }
            catch (Exception exception)
            {
                this.ActualException = exception;
                this.WriteLine($"ActualExceptionType:      {this.ActualException.GetType().SafeToName()}");
                this.WriteLine($"ActualExceptionMessage:   {this.ActualException.Message.SafeToString()}");
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

                if (this.ExpectedExceptionMessage != null)
                {
                    this.ActualException!.Message.Should().Be(this.ExpectedExceptionMessage);
                }
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
            NullHandling = JsonReaderNullPropertyHandling.Allow,
            ExpectedPropertyNames = [],
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Object With Scalar Values Should Return Property Names In Source Order",
            SourceJson = "{\"name\":\"value\",\"count\":1,\"none\":null}",
            NullHandling = JsonReaderNullPropertyHandling.Allow,
            ExpectedPropertyNames = ["name", "count", "none"],
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Object With Nested Values Should Return Only Direct Property Names",
            SourceJson = "{\"first\":{\"nested\":\"value\"},\"second\":[1,{\"nested\":true}],\"third\":false}",
            NullHandling = JsonReaderNullPropertyHandling.Allow,
            ExpectedPropertyNames = ["first", "second", "third"],
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Object With Escaped Property Name Should Return Decoded Property Name",
            SourceJson = "{\"na\\u006de\":\"value\"}",
            NullHandling = JsonReaderNullPropertyHandling.Allow,
            ExpectedPropertyNames = ["name"],
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Array Token Should Throw JsonException",
            SourceJson = "[]",
            NullHandling = JsonReaderNullPropertyHandling.Allow,
            ExpectedExceptionType = typeof(JsonException),
            ExpectedExceptionMessage = "Expected start of JSON object.",
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Scalar Token Should Throw JsonException",
            SourceJson = "\"value\"",
            NullHandling = JsonReaderNullPropertyHandling.Allow,
            ExpectedExceptionType = typeof(JsonException),
            ExpectedExceptionMessage = "Expected start of JSON object.",
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Incomplete Nonfinal Object Before First Property Should Throw JsonException",
            SourceJson = "{",
            NullHandling = JsonReaderNullPropertyHandling.Allow,
            IsFinalBlock = false,
            ExpectedExceptionType = typeof(JsonException),
            ExpectedExceptionMessage = "Unexpected end of JSON.",
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Incomplete Nonfinal Object After Property Name Should Throw JsonException",
            SourceJson = "{\"name\"",
            NullHandling = JsonReaderNullPropertyHandling.Allow,
            IsFinalBlock = false,
            ExpectedExceptionType = typeof(JsonException),
            ExpectedExceptionMessage = "Unexpected end of JSON.",
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Object With Null Values And Allow Policy Should Return All Property Names",
            SourceJson = "{\"name\":\"value\",\"count\":1,\"none\":null}",
            NullHandling = JsonReaderNullPropertyHandling.Allow,
            ExpectedPropertyNames = ["name", "count", "none"],
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Object With Null Values And Ignore Policy Should Omit Null Property Names",
            SourceJson = "{\"first\":null,\"second\":1,\"third\":null,\"fourth\":\"text\"}",
            NullHandling = JsonReaderNullPropertyHandling.Ignore,
            ExpectedPropertyNames = ["second", "fourth"],
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Object With Only Null Values And Ignore Policy Should Return Empty",
            SourceJson = "{\"first\":null,\"second\":null}",
            NullHandling = JsonReaderNullPropertyHandling.Ignore,
            ExpectedPropertyNames = [],
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Object Without Null Values And Ignore Policy Should Return All Property Names",
            SourceJson = "{\"name\":\"value\",\"count\":1}",
            NullHandling = JsonReaderNullPropertyHandling.Ignore,
            ExpectedPropertyNames = ["name", "count"],
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Empty Object And Ignore Policy Should Return Empty",
            SourceJson = "{}",
            NullHandling = JsonReaderNullPropertyHandling.Ignore,
            ExpectedPropertyNames = [],
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Object Without Null Values And Throw Policy Should Return All Property Names",
            SourceJson = "{\"name\":\"value\",\"count\":1,\"active\":true}",
            NullHandling = JsonReaderNullPropertyHandling.Throw,
            ExpectedPropertyNames = ["name", "count", "active"],
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Object With Null Value And Throw Policy Should Throw JsonException",
            SourceJson = "{\"name\":\"value\",\"none\":null,\"after\":\"val\"}",
            NullHandling = JsonReaderNullPropertyHandling.Throw,
            ExpectedExceptionType = typeof(JsonException),
            ExpectedExceptionMessage = "Property 'none' has an unexpected null value.",
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Empty Object And Throw Policy Should Return Empty",
            SourceJson = "{}",
            NullHandling = JsonReaderNullPropertyHandling.Throw,
            ExpectedPropertyNames = [],
        },
        new ReadObjectPropertyNamesTest
        {
            Name = "Invalid NullHandling Enum Value Should Throw ArgumentOutOfRangeException",
            SourceJson = "{}",
            NullHandling = (JsonReaderNullPropertyHandling)999,
            ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
        },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(ReadObjectPropertyNamesTheoryData))]
    public void ReadObjectPropertyNames(IXUnitTest test) => test.Execute(this);
    #endregion
}
