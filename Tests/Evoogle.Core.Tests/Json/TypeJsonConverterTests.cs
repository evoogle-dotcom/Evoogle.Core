// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;
using Evoogle.Extensions;
using Evoogle.XUnit;

using FluentAssertions;

namespace Evoogle.Json;

public class TypeJsonConverterTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public class JsonDeserializeTest : XUnitTest
    {
        #region User Supplied Properties
        public string? SourceJson { get; init; }
        public Type? ExpectedType { get; init; }
        public string? ExpectedException { get; init; }
        #endregion

        #region Calculated Properties
        private Type? ActualType { get; set; }
        private string? ActualException { get; set; }

        private static JsonSerializerOptions Options { get; } = new JsonSerializerOptions { Converters = { new TypeJsonConverter() } };
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            this.WriteLine($"Source JSON: {this.SourceJson.SafeToString()}");
            this.WriteLine();

            if (this.ExpectedType != null)
            {
                this.WriteLine($"Expected Type: {this.ExpectedType.SafeToName()}");
            }

            if (this.ExpectedException != null)
            {
                this.WriteLine($"Expected Exception: {this.ExpectedException.SafeToString()}");
            }
        }

        protected override void Act()
        {
            try
            {
                this.ActualType = JsonSerializer.Deserialize<Type?>(this.SourceJson!, Options);
                this.WriteLine($"Actual   Type: {this.ActualType.SafeToName()}");
            }
            catch (Exception exception)
            {
                var actualException = exception.GetType().Name;
                this.ActualException = actualException;

                this.WriteLine($"Actual   Exception: {this.ActualException.SafeToString()}");
            }
        }

        protected override void Assert()
        {
            if (this.ExpectedType != null)
            {
                this.ActualException.Should().BeNull();

                this.ActualType.Should().NotBeNull();
                this.ActualType.Should().Be(this.ExpectedType);
            }
            else if (this.ExpectedException != null)
            {
                this.ActualType.Should().BeNull();

                this.ActualException.Should().NotBeNull();
                this.ActualException.Should().Be(this.ExpectedException);
            }
        }
        #endregion
    }

    public class JsonSerializeTest : XUnitTest
    {
        #region User Supplied Properties
        public Type? SourceType { get; init; }
        public string? ExpectedJson { get; init; }
        #endregion

        #region Calculated Properties
        private string? ActualJson { get; set; }
        private static JsonSerializerOptions Options { get; } = new JsonSerializerOptions { Converters = { new TypeJsonConverter() } };
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            this.WriteLine($"Source Type: {this.SourceType.SafeToName()}");
            this.WriteLine();

            this.WriteLine($"Expected JSON: {this.ExpectedJson.SafeToString()}");
        }

        protected override void Act()
        {
            this.ActualJson = JsonSerializer.Serialize(this.SourceType, Options);
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
    public class MyOuterClass
    {
        public class MyInnerClass
        {
        }
    }

    public static TheoryDataRow<IXUnitTest>[] DeserializeTheoryData =>
    [
        // Should Deserialize From Null Literal
        new JsonDeserializeTest
        {
            Name = "Type Should Deserialize From Null Literal",
            SourceJson = "null",
            ExpectedType = null
        },

        // Should Deserialize From JSON String Successfully
        new JsonDeserializeTest
        {
            Name = "Type Should Deserialize From String - String",
            SourceJson = @"""System.String, System.Private.CoreLib""",
            ExpectedType = typeof(string)
        },
        new JsonDeserializeTest
        {
            Name = "Type Should Deserialize From String - Int32",
            SourceJson = @"""System.Int32, System.Private.CoreLib""",
            ExpectedType = typeof(int)
        },
        new JsonDeserializeTest
        {
            Name = "Type Should Deserialize From String - List<Int32>",
            SourceJson = @"""System.Collections.Generic.List\u00601[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib""",
            ExpectedType = typeof(List<int>)
        },
        new JsonDeserializeTest
        {
            Name = "Type Should Deserialize From String - Dictionary<String,Int32>",
            SourceJson = @"""System.Collections.Generic.Dictionary\u00602[[System.String, System.Private.CoreLib],[System.Int32, System.Private.CoreLib]], System.Private.CoreLib""",
            ExpectedType = typeof(Dictionary<string, int>)
        },
        new JsonDeserializeTest
        {
            Name = "Type Should Deserialize From String - List<>",
            SourceJson = @"""System.Collections.Generic.List\u00601, System.Private.CoreLib""",
            ExpectedType = typeof(List<>)
        },
        new JsonDeserializeTest
        {
            Name = "Type Should Deserialize From String - Dictionary<,>",
            SourceJson = @"""System.Collections.Generic.Dictionary\u00602, System.Private.CoreLib""",
            ExpectedType = typeof(Dictionary<,>)
        },
        new JsonDeserializeTest
        {
            Name = "Type Should Deserialize From String - MyOuterClass",
            SourceJson = @"""Evoogle.Json.TypeJsonConverterTests\u002BMyOuterClass, Evoogle.Core.Tests""",
            ExpectedType = typeof(MyOuterClass)
        },
        new JsonDeserializeTest
        {
            Name = "Type Should Deserialize From String - MyOuterClass.MyInnerClass",
            SourceJson = @"""Evoogle.Json.TypeJsonConverterTests\u002BMyOuterClass\u002BMyInnerClass, Evoogle.Core.Tests""",
            ExpectedType = typeof(MyOuterClass.MyInnerClass)
        },

        // Type Should Throw Exception From Empty or Whitespace String Name
        new JsonDeserializeTest
        {
            Name = "Type Should Throw JsonException From Empty String Name - Empty String",
            SourceJson = @"""""",
            ExpectedException = nameof(JsonException)
        },
        new JsonDeserializeTest
        {
            Name = "Type Should Throw JsonException From Empty String Name - Whitespace String",
            SourceJson = @"""       """,
            ExpectedException = nameof(JsonException)
        },

        // Type Should Throw Exception From Invalid String Name
        new JsonDeserializeTest
        {
            Name = "Type Should Throw JsonException From Invalid String Name - Invalid",
            SourceJson = @"""Invalid""",
            ExpectedException = nameof(JsonException)
        },
    ];

    public static TheoryDataRow<IXUnitTest>[] SerializeTheoryData =>
    [
        // Should Serialize To JSON String Successfully
        new JsonSerializeTest
        {
            Name = "Type Should Serialize To String - String",
            SourceType = typeof(string),
            ExpectedJson = @"""System.String, System.Private.CoreLib"""
        },
        new JsonSerializeTest
        {
            Name = "Type Should Serialize To String - Int32",
            SourceType = typeof(int),
            ExpectedJson = @"""System.Int32, System.Private.CoreLib"""
        },
        new JsonSerializeTest
        {
            Name = "Type Should Serialize To String - List<Int32>",
            SourceType = typeof(List<int>),
            ExpectedJson = @"""System.Collections.Generic.List\u00601[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib"""
        },
        new JsonSerializeTest
        {
            Name = "Type Should Serialize To String - Dictionary<String,Int32>",
            SourceType = typeof(Dictionary<string, int>),
            ExpectedJson = @"""System.Collections.Generic.Dictionary\u00602[[System.String, System.Private.CoreLib],[System.Int32, System.Private.CoreLib]], System.Private.CoreLib"""
        },
        new JsonSerializeTest
        {
            Name = "Type Should Serialize To String - List<>",
            SourceType = typeof(List<>),
            ExpectedJson = @"""System.Collections.Generic.List\u00601, System.Private.CoreLib"""
        },
        new JsonSerializeTest
        {
            Name = "Type Should Serialize To String - Dictionary<,>",
            SourceType = typeof(Dictionary<,>),
            ExpectedJson = @"""System.Collections.Generic.Dictionary\u00602, System.Private.CoreLib"""
        },
        new JsonSerializeTest
        {
            Name = "Type Should Serialize To String - MyOuterClass",
            SourceType = typeof(MyOuterClass),
            ExpectedJson = @"""Evoogle.Json.TypeJsonConverterTests\u002BMyOuterClass, Evoogle.Core.Tests"""
        },
        new JsonSerializeTest
        {
            Name = "Type Should Serialize To String - MyOuterClass.MyInnerClass",
            SourceType = typeof(MyOuterClass.MyInnerClass),
            ExpectedJson = @"""Evoogle.Json.TypeJsonConverterTests\u002BMyOuterClass\u002BMyInnerClass, Evoogle.Core.Tests"""
        },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(DeserializeTheoryData))]
    public void Deserialize(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(SerializeTheoryData))]
    public void Serialize(IXUnitTest test) => test.Execute(this);
    #endregion
}
