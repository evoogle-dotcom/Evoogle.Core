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

/// <summary>
///     Tests for <see cref="ObjectJsonConverter"/> mirroring style of <see cref="TypeJsonConverterTests"/>.
/// </summary>
public class ObjectJsonConverterTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public class JsonDeserializeTest : XUnitTest
    {
        #region User Supplied Properties
        public string? SourceJson { get; init; }
        public object? ExpectedValue { get; init; }
        public string? ExpectedException { get; init; }
        public Type? ExpectedRuntimeType { get; init; }
        #endregion

        #region Calculated Properties
        private object? ActualValue { get; set; }
        private string? ActualException { get; set; }

        private static JsonSerializerOptions Options { get; } = new JsonSerializerOptions { Converters = { new ObjectJsonConverter() } };
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            this.WriteLine($"Source JSON: {this.SourceJson.SafeToString()}");
            this.WriteLine();

            if (this.ExpectedException != null)
            {
                this.WriteLine($"Expected Exception: {this.ExpectedException.SafeToString()}");
            }
            else
            {
                this.WriteLine($"Expected Value: {this.ExpectedValue.SafeToString()}");
                if (this.ExpectedRuntimeType != null)
                {
                    this.WriteLine($"Expected Runtime Type: {this.ExpectedRuntimeType.SafeToName()}");
                }
            }
        }

        protected override void Act()
        {
            try
            {
                // Deserialize as object using our converter
                this.ActualValue = JsonSerializer.Deserialize<object?>(this.SourceJson!, Options);
                this.WriteLine($"Actual   Value: {this.ActualValue.SafeToString()}");
                if (this.ActualValue != null)
                {
                    this.WriteLine($"Actual   Runtime Type: {this.ActualValue.GetType().SafeToName()}");
                }
            }
            catch (Exception exception)
            {
                this.ActualException = exception.GetType().Name;
                this.WriteLine($"Actual   Exception: {this.ActualException.SafeToString()}");
            }
        }

        protected override void Assert()
        {
            if (this.ExpectedException != null)
            {
                this.ActualValue.Should().BeNull();
                this.ActualException.Should().NotBeNull();
                this.ActualException.Should().Be(this.ExpectedException);
                return;
            }

            this.ActualException.Should().BeNull();

            if (this.ExpectedRuntimeType != null && this.ActualValue != null)
            {
                this.ActualValue.GetType().Should().Be(this.ExpectedRuntimeType);
            }

            if (this.ExpectedValue == null)
            {
                this.ActualValue.Should().BeNull();
                return;
            }

            // Use structural equivalence for complex objects & collections; direct equality for primitives & strings.
            var expectedType = this.ExpectedValue.GetType();
            if (expectedType != typeof(string) && typeof(System.Collections.IEnumerable).IsAssignableFrom(expectedType) && expectedType != typeof(byte[]))
            {
                this.ActualValue.Should().BeEquivalentTo(this.ExpectedValue);
            }
            else if (!expectedType.IsPrimitive && !expectedType.IsEnum && expectedType != typeof(string) && expectedType != typeof(decimal))
            {
                this.ActualValue.Should().BeEquivalentTo(this.ExpectedValue);
            }
            else
            {
                this.ActualValue.Should().Be(this.ExpectedValue);
            }
        }
        #endregion
    }

    public class JsonSerializeTest : XUnitTest
    {
        #region User Supplied Properties
        public object? SourceValue { get; init; }
        public string? ExpectedJson { get; init; }
        #endregion

        #region Calculated Properties
        private string? ActualJson { get; set; }
        private static JsonSerializerOptions Options { get; } = new JsonSerializerOptions { Converters = { new ObjectJsonConverter() } };
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            this.WriteLine($"Source Value: {this.SourceValue.SafeToString()}");
            this.WriteLine();
            this.WriteLine($"Expected JSON: {this.ExpectedJson.SafeToString()}");
        }

        protected override void Act()
        {
            this.ActualJson = JsonSerializer.Serialize(this.SourceValue, Options);
            this.WriteLine($"Actual   JSON: {this.ActualJson.SafeToString()}");
        }

        protected override void Assert()
        {
            var actualMinusWhitespace = this.ActualJson.RemoveWhitespace();
            var expectedMinusWhitespace = this.ExpectedJson.RemoveWhitespace();
            actualMinusWhitespace.Should().Be(expectedMinusWhitespace);
        }
        #endregion
    }
    #endregion

    #region Helper Types
    public class ComplexPoco
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
    #endregion

    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] DeserializeTheoryData =>
    [
        // Null literal
        new JsonDeserializeTest
        {
            Name = "object Should Deserialize From Null Literal",
            SourceJson = "null",
            ExpectedValue = null,
            ExpectedRuntimeType = null
        },

        // Primitive values (no envelope)
        new JsonDeserializeTest
        {
            Name = "object Should Throw JsonException For Number Literal Without Envelope",
            SourceJson = "123",
            ExpectedValue = null,
            ExpectedException = nameof(JsonException)
        },
        new JsonDeserializeTest
        {
            Name = "object Should Throw JsonException For String Literal Without Envelope",
            SourceJson = "\"abc\"",
            ExpectedValue = null,
            ExpectedException = nameof(JsonException)
        },

        // Envelope - Complex POCO
        new JsonDeserializeTest
        {
            Name = "object Should Deserialize Envelope ComplexPoco",
            SourceJson = "{\"$type\":\"Evoogle.Json.ObjectJsonConverterTests\u002BComplexPoco, Evoogle.Core.Tests\",\"$value\":{\"Id\":7,\"Name\":\"Test\"}}",
            ExpectedValue = new ComplexPoco { Id = 7, Name = "Test" },
            ExpectedRuntimeType = typeof(ComplexPoco)
        },

        // Envelope - List<int>
        new JsonDeserializeTest
        {
            Name = "object Should Deserialize Envelope List<int>",
            SourceJson = "{\"$type\":\"System.Collections.Generic.List\u00601[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib\",\"$value\":[1,2,3]}",
            ExpectedValue = new List<int> { 1, 2, 3 },
            ExpectedRuntimeType = typeof(List<int>)
        },

        // Array/Object without envelope should throw
        new JsonDeserializeTest
        {
            Name = "object Should Throw JsonException For Array Without Envelope",
            SourceJson = "[1,2,3]",
            ExpectedException = nameof(JsonException)
        },
        new JsonDeserializeTest
        {
            Name = "object Should Throw JsonException For Object Without Envelope",
            SourceJson = "{\"A\":1,\"B\":\"two\"}",
            ExpectedException = nameof(JsonException)
        },

        // Invalid envelope type should throw
        new JsonDeserializeTest
        {
            Name = "object Should Throw JsonException For Invalid Envelope Type",
            SourceJson = "{\"$type\":\"Invalid.Type.Name, Invalid.Assembly\",\"$value\":{\"X\":1}}",
            ExpectedException = nameof(JsonException)
        },
    ];

    public static TheoryDataRow<IXUnitTest>[] SerializeTheoryData =>
    [
        // Null literal
        new JsonSerializeTest
        {
            Name = "object Should Serialize Null Literal",
            SourceValue = null,
            ExpectedJson = "null"
        },

        // Primitive Int32 (envelope)
        new JsonSerializeTest
        {
            Name = "object Should Serialize Envelope Int32",
            SourceValue = 123,
            ExpectedJson = $"{{\"$type\":\"{typeof(int).AssemblyQualifiedName!.Replace("+", "\\u002B").Replace("`", "\\u0060")}\",\"$value\":123}}"
        },
        // Primitive String (envelope)
        new JsonSerializeTest
        {
            Name = "object Should Serialize Envelope String",
            SourceValue = "abc",
            ExpectedJson = $"{{\"$type\":\"{typeof(string).AssemblyQualifiedName!.Replace("+", "\\u002B").Replace("`", "\\u0060")}\",\"$value\":\"abc\"}}"
        },

        // Complex POCO envelope
        new JsonSerializeTest
        {
            Name = "object Should Serialize Envelope ComplexPoco",
            SourceValue = new ComplexPoco { Id = 7, Name = "Test" },
            ExpectedJson = $"{{\"$type\":\"{typeof(ComplexPoco).AssemblyQualifiedName!.Replace("+", "\\u002B").Replace("`", "\\u0060")}\",\"$value\":{{\"Id\":7,\"Name\":\"Test\"}}}}"
        },

        // List<int> envelope
        new JsonSerializeTest
        {
            Name = "object Should Serialize Envelope List<int>",
            SourceValue = new List<int> { 1, 2, 3 },
            ExpectedJson = $"{{\"$type\":\"{typeof(List<int>).AssemblyQualifiedName!.Replace("+", "\\u002B").Replace("`", "\\u0060")}\",\"$value\":[1,2,3]}}"
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
