// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Evoogle.Extensions;
using Evoogle.XUnit;
using Evoogle.XUnit.Json;

using FluentAssertions;

namespace Evoogle.Json;

[DynamicLinqType]
public class Utf8JsonWriterExensionTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public class WritePropertyTest : XUnitTest
    {
        #region User Supplied Properties
        [JsonConverter(typeof(ExpressionActionJsonConverter<Utf8JsonWriter>))]
        public Expression<Action<Utf8JsonWriter>> WritePropertyExpression { get; init; } = null!;

        public string ExpectedJson { get; init; } = null!;
        #endregion

        #region Calculated Properties
        private string ActualJson { get; set; } = null!;
        #endregion

        #region XUnitTest Methods
        protected override void Arrange() => this.WriteLine($"Expected JSON: {this.ExpectedJson.SafeToString()}");

        protected override void Act()
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = false });

            writer.WriteStartObject();

            var writeProperty = this.WritePropertyExpression.Compile() ?? throw new InvalidOperationException($"Unable to compile {nameof(this.WritePropertyExpression)} into a function object.");
            writeProperty(writer);

            writer.WriteEndObject();
            writer.Flush();

            this.ActualJson = Encoding.UTF8.GetString(stream.ToArray());
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
    private const string _defaultDateTimeString = @"0001-01-01T00:00:00.0000000";
    private const string _defaultDateTimeOffsetString = @"0001-01-01T00:00:00.0000000\u002B00:00";
    private const string _defaultEnumString = @"None";
    private const string _defaultTimeSpanString = @"00:00:00";
    private const string _defaultGuidString = @"00000000-0000-0000-0000-000000000000";
    private const string _defaultUlidString = @"00000000000000000000000000";

    public static TheoryDataRow<IXUnitTest>[] WritePropertyTheoryData =>
    [
        // Boolean Tests
        new WritePropertyTest
        {
            Name = "Type=Boolean, Value=false, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyBoolean(a, "false", nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{""value"":false}",
        },
        new WritePropertyTest
        {
            Name = "Type=Boolean, Value=false, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyBoolean(a, "false", nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Boolean, Value=false, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyBoolean(a, "false", nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":false}",
        },

        // Converter Tests

        // .. CultureInfo (null) With Converter
        new WritePropertyTest
        {
            Name = "CultureInfo=CultureInfo With Converter, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyCultureInfoWithConverter(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "CultureInfo=CultureInfo With Converter, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyCultureInfoWithConverter(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "CultureInfo=CultureInfo With Converter, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyCultureInfoWithConverter(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":null}}",
        },

        // .. Enum (default) With Converter
        new WritePropertyTest
        {
            Name = "Type=Enum With Converter, Value=default, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyEnumWithConverter(a, _defaultEnumString, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = $@"{{""value"":""{_defaultEnumString}""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=Enum With Converter, Value=default, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyEnumWithConverter(a, _defaultEnumString, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Enum With Converter, Value=default, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyEnumWithConverter(a, _defaultEnumString, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":""{_defaultEnumString}""}}",
        },

        // .. Type (null) With Converter
        new WritePropertyTest
        {
            Name = "Type=Type With Converter, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyTypeWithConverter(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Type With Converter, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyTypeWithConverter(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Type With Converter, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyTypeWithConverter(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":null}}",
        },

        // Number Tests

        // .. Decimal
        new WritePropertyTest
        {
            Name = "Type=Decimal, Value=0, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyDecimal(a, "0", nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{""value"":0}",
        },
        new WritePropertyTest
        {
            Name = "Type=Decimal, Value=0, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyDecimal(a, "0", nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Decimal, Value=0, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyDecimal(a, "0", nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":0}",
        },

        // .. Double
        new WritePropertyTest
        {
            Name = "Type=Double, Value=0, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyDouble(a, "0", nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{""value"":0}",
        },
        new WritePropertyTest
        {
            Name = "Type=Double, Value=0, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyDouble(a, "0", nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Double, Value=0, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyDouble(a, "0", nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":0}",
        },

        // .. Float
        new WritePropertyTest
        {
            Name = "Type=Float, Value=0, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyFloat(a, "0", nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{""value"":0}",
        },
        new WritePropertyTest
        {
            Name = "Type=Float, Value=0, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyFloat(a, "0", nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Float, Value=0, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyFloat(a, "0", nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":0}",
        },

        // .. Int
        new WritePropertyTest
        {
            Name = "Type=Int, Value=0, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyInt(a, "0", nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{""value"":0}",
        },
        new WritePropertyTest
        {
            Name = "Type=Int, Value=0, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyInt(a, "0", nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Int, Value=0, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyInt(a, "0", nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":0}",
        },

        // .. Long
        new WritePropertyTest
        {
            Name = "Type=Long, Value=0, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyLong(a, "0", nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{""value"":0}",
        },
        new WritePropertyTest
        {
            Name = "Type=Long, Value=0, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyLong(a, "0", nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Long, Value=0, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyLong(a, "0", nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":0}",
        },

        // Serializer Tests

        // .. Boolean
        new WritePropertyTest
        {
            Name = "Type=Boolean With Serializer, Value=false, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyBooleanWithSerializer(a, "false", nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{""value"":false}",
        },
        new WritePropertyTest
        {
            Name = "Type=Boolean With Serializer, Value=false, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyBooleanWithSerializer(a, "false", nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Boolean With Serializer, Value=false, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyBooleanWithSerializer(a, "false", nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":false}",
        },

        // .. Decimal
        new WritePropertyTest
        {
            Name = "Type=Decimal With Serializer, Value=0, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyDecimalWithSerializer(a, "0", nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{""value"":0}",
        },
        new WritePropertyTest
        {
            Name = "Type=Decimal With Serializer, Value=0, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyDecimalWithSerializer(a, "0", nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Decimal With Serializer, Value=0, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyDecimalWithSerializer(a, "0", nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":0}",
        },

        // .. Guid
        new WritePropertyTest
        {
            Name = "Type=Guid With Serializer, Value=0, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyGuidWithSerializer(a, _defaultGuidString, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = $@"{{""value"":""{_defaultGuidString}""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=Guid With Serializer, Value=0, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyGuidWithSerializer(a, _defaultGuidString, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Guid With Serializer, Value=0, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyGuidWithSerializer(a, _defaultGuidString, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":""{_defaultGuidString}""}}",
        },

        // .. String (empty)
        new WritePropertyTest
        {
            Name = "Type=String With Serializer, Value=empty, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyStringWithSerializer(a, "", nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = $@"{{""value"":""""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=String With Serializer, Value=empty, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyStringWithSerializer(a, "", nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = $@"{{""value"":""""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=String With Serializer, Value=empty, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyStringWithSerializer(a, "", nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":""""}}",
        },

        // .. String (null)
        new WritePropertyTest
        {
            Name = "Type=String With Serializer, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyStringWithSerializer(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=String With Serializer, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyStringWithSerializer(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=String With Serializer, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyStringWithSerializer(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":null}}",
        },

        // String Tests

        // .. DateTime
        new WritePropertyTest
        {
            Name = "Type=DateTime, Value=default, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyDateTime(a, _defaultDateTimeString, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = $@"{{""value"":""{_defaultDateTimeString}""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=DateTime, Value=default, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyDateTime(a, _defaultDateTimeString, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=DateTime, Value=default, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyDateTime(a, _defaultDateTimeString, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":""{_defaultDateTimeString}""}}",
        },

        // .. DateTimeOffset
        new WritePropertyTest
        {
            Name = "Type=DateTimeOffset, Value=default, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyDateTimeOffset(a, _defaultDateTimeOffsetString, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = $@"{{""value"":""{_defaultDateTimeOffsetString}""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=DateTimeOffset, Value=default, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyDateTimeOffset(a, _defaultDateTimeOffsetString, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=DateTimeOffset, Value=default, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyDateTimeOffset(a, _defaultDateTimeOffsetString, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":""{_defaultDateTimeOffsetString}""}}",
        },

        // .. Enum
        new WritePropertyTest
        {
            Name = "Type=Enum, Value=default, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyEnum(a, _defaultEnumString, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = $@"{{""value"":""{_defaultEnumString}""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=Enum, Value=default, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyEnum(a, _defaultEnumString, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Enum, Value=default, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyEnum(a, _defaultEnumString, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":""{_defaultEnumString}""}}",
        },

        // .. Guid
        new WritePropertyTest
        {
            Name = "Type=Guid, Value=default, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyGuid(a, _defaultGuidString, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = $@"{{""value"":""{_defaultGuidString}""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=Guid, Value=default, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyGuid(a, _defaultGuidString, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Guid, Value=default, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyGuid(a, _defaultGuidString, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":""{_defaultGuidString}""}}",
        },

        // .. String (empty)
        new WritePropertyTest
        {
            Name = "Type=String, Value=empty, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyString(a, "", nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = $@"{{""value"":""""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=String, Value=empty, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyString(a, "", nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = $@"{{""value"":""""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=String, Value=empty, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyString(a, "", nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":""""}}",
        },

        // .. String (null)
        new WritePropertyTest
        {
            Name = "Type=String, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyString(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=String, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyString(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=String, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyString(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":null}}",
        },

        // .. TimeSpan
        new WritePropertyTest
        {
            Name = "Type=TimeSpan, Value=default, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyTimeSpan(a, _defaultTimeSpanString, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = $@"{{""value"":""{_defaultTimeSpanString}""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=TimeSpan, Value=default, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyTimeSpan(a, _defaultTimeSpanString, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=TimeSpan, Value=default, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyTimeSpan(a, _defaultTimeSpanString, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":""{_defaultTimeSpanString}""}}",
        },

        // .. Ulid
        new WritePropertyTest
        {
            Name = "Type=Ulid, Value=default, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyUlid(a, _defaultUlidString, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = $@"{{""value"":""{_defaultUlidString}""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=Ulid, Value=default, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyUlid(a, _defaultUlidString, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Ulid, Value=default, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyUlid(a, _defaultUlidString, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":""{_defaultUlidString}""}}",
        },

        // Nullable Boolean Tests

        // .. Boolean (null)
        new WritePropertyTest
        {
            Name = "Type=Nullable Boolean, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableBoolean(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Boolean, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableBoolean(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Boolean, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableBoolean(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":null}",
        },

        // .. Boolean (default)
        new WritePropertyTest
        {
            Name = "Type=Nullable Boolean, Value=false, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableBoolean(a, "false", nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{""value"":false}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Boolean, Value=false, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableBoolean(a, "false", nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Boolean, Value=false, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableBoolean(a, "false", nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":false}",
        },

        // Nullable Converter Tests

        // .. Enum (null)
        new WritePropertyTest
        {
            Name = "Type=Nullable Enum With Converter, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableEnum(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Enum With Converter, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableEnum(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Enum With Converter, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableEnum(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":null}}",
        },

        // .. Enum (default)
        new WritePropertyTest
        {
            Name = "Type=Nullable Enum With Converter, Value=default, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableEnum(a, _defaultEnumString, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = $@"{{""value"":""{_defaultEnumString}""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Enum With Converter, Value=default, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableEnum(a, _defaultEnumString, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Enum With Converter, Value=default, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableEnum(a, _defaultEnumString, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":""{_defaultEnumString}""}}",
        },

        // Nullable Number Tests

        // .. Decimal (null)
        new WritePropertyTest
        {
            Name = "Type=Nullable Decimal, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDecimal(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Decimal, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDecimal(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Decimal, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDecimal(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":null}",
        },

        // .. Double (null)
        new WritePropertyTest
        {
            Name = "Type=Nullable Double, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDouble(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Double, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDouble(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Double, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDouble(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":null}",
        },

        // .. Float (null)
        new WritePropertyTest
        {
            Name = "Type=Nullable Float, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableFloat(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Float, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableFloat(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Float, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableFloat(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":null}",
        },

        // .. Int (null)
        new WritePropertyTest
        {
            Name = "Type=Nullable Int, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableInt(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Int, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableInt(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Int, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableInt(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":null}",
        },

        // .. Long (null)
        new WritePropertyTest
        {
            Name = "Type=Nullable Long, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableLong(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Long, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableLong(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Long, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableLong(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":null}",
        },

        // .. Decimal (default)
        new WritePropertyTest
        {
            Name = "Type=Nullable Decimal, Value=0, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDecimal(a, "0", nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{""value"":0}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Decimal, Value=0, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDecimal(a, "0", nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Decimal, Value=0, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDecimal(a, "0", nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":0}",
        },

        // .. Double (default)
        new WritePropertyTest
        {
            Name = "Type=Nullable Double, Value=0, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDouble(a, "0", nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{""value"":0}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Double, Value=0, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDouble(a, "0", nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Double, Value=0, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDouble(a, "0", nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":0}",
        },

        // .. Float (default)
        new WritePropertyTest
        {
            Name = "Type=Nullable Float, Value=0, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableFloat(a, "0", nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{""value"":0}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Float, Value=0, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableFloat(a, "0", nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Float, Value=0, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableFloat(a, "0", nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":0}",
        },

        // .. Int (default)
        new WritePropertyTest
        {
            Name = "Type=Nullable Int, Value=0, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableInt(a, "0", nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{""value"":0}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Int, Value=0, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableInt(a, "0", nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Int, Value=0, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableInt(a, "0", nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":0}",
        },

        // .. Long (default)
        new WritePropertyTest
        {
            Name = "Type=Nullable Long, Value=0, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableLong(a, "0", nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{""value"":0}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Long, Value=0, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableLong(a, "0", nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Long, Value=0, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableLong(a, "0", nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":0}",
        },

        // Nullable Serializer Tests

        // .. Nullable Boolean (null)
        new WritePropertyTest
        {
            Name = "Type=Nullable Boolean With Serializer, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableBooleanWithSerializer(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Boolean With Serializer, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableBooleanWithSerializer(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Boolean With Serializer, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableBooleanWithSerializer(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":null}",
        },

        // .. Nullable Boolean (default)
        new WritePropertyTest
        {
            Name = "Type=Nullable Boolean With Serializer, Value=false, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableBooleanWithSerializer(a, "false", nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{""value"":false}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Boolean With Serializer, Value=false, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableBooleanWithSerializer(a, "false", nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Boolean With Serializer, Value=false, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableBooleanWithSerializer(a, "false", nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":false}",
        },

        // .. Nullable Decimal (null)
        new WritePropertyTest
        {
            Name = "Type=Nullable Decimal With Serializer, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDecimalWithSerializer(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Decimal With Serializer, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDecimalWithSerializer(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Decimal With Serializer, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDecimalWithSerializer(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":null}",
        },

        // .. Nullable Decimal (default)
        new WritePropertyTest
        {
            Name = "Type=Nullable Decimal With Serializer, Value=0, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDecimalWithSerializer(a, "0", nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{""value"":0}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Decimal With Serializer, Value=0, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDecimalWithSerializer(a, "0", nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Decimal With Serializer, Value=0, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDecimalWithSerializer(a, "0", nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = @"{""value"":0}",
        },

        // .. Guid (null)
        new WritePropertyTest
        {
            Name = "Type=Nullable Guid With Serializer, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableGuidWithSerializer(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Guid With Serializer, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableGuidWithSerializer(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Guid With Serializer, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableGuidWithSerializer(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":null}}",
        },

        // .. Guid (default)
        new WritePropertyTest
        {
            Name = "Type=Nullable Guid With Serializer, Value=default, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableGuidWithSerializer(a, _defaultGuidString, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = $@"{{""value"":""{_defaultGuidString}""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Guid With Serializer, Value=default, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableGuidWithSerializer(a, _defaultGuidString, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Guid With Serializer, Value=default, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableGuidWithSerializer(a, _defaultGuidString, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":""{_defaultGuidString}""}}",
        },

        // Nullable String Tests

        // .. DateTime (null)
        new WritePropertyTest
        {
            Name = "Type=Nullable DateTime, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDateTime(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable DateTime, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDateTime(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable DateTime, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDateTime(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":null}}",
        },

        // .. DateTimeOffset (null)
        new WritePropertyTest
        {
            Name = "Type=Nullable DateTimeOffset, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDateTimeOffset(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable DateTimeOffset, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDateTimeOffset(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable DateTimeOffset, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDateTimeOffset(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":null}}",
        },

        // .. Enum (null)
        new WritePropertyTest
        {
            Name = "Type=Nullable Enum, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableEnum(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Enum, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableEnum(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Enum, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableEnum(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":null}}",
        },

        // .. Guid (null)
        new WritePropertyTest
        {
            Name = "Type=Nullable Guid, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableGuid(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Guid, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableGuid(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Guid, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableGuid(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":null}}",
        },

        // .. TimeSpan (null)
        new WritePropertyTest
        {
            Name = "Type=Nullable TimeSpan, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableTimeSpan(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable TimeSpan, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableTimeSpan(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable TimeSpan, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableTimeSpan(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":null}}",
        },

        // .. Ulid (null)
        new WritePropertyTest
        {
            Name = "Type=Nullable Ulid, Value=null, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableUlid(a, null, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Ulid, Value=null, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableUlid(a, null, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Ulid, Value=null, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableUlid(a, null, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":null}}",
        },

        // .. DateTime (default)
        new WritePropertyTest
        {
            Name = "Type=Nullable DateTime, Value=default, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDateTime(a, _defaultDateTimeString, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = $@"{{""value"":""{_defaultDateTimeString}""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable DateTime, Value=default, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDateTime(a, _defaultDateTimeString, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable DateTime, Value=default, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDateTime(a, _defaultDateTimeString, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":""{_defaultDateTimeString}""}}",
        },

        // .. DateTimeOffset (default)
        new WritePropertyTest
        {
            Name = "Type=Nullable DateTimeOffset, Value=default, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDateTimeOffset(a, _defaultDateTimeOffsetString, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = $@"{{""value"":""{_defaultDateTimeOffsetString}""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable DateTimeOffset, Value=default, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDateTimeOffset(a, _defaultDateTimeOffsetString, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable DateTimeOffset, Value=default, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableDateTimeOffset(a, _defaultDateTimeOffsetString, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":""{_defaultDateTimeOffsetString}""}}",
        },

        // .. Enum (default)
        new WritePropertyTest
        {
            Name = "Type=Nullable Enum, Value=default, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableEnum(a, _defaultEnumString, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = $@"{{""value"":""{_defaultEnumString}""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Enum, Value=default, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableEnum(a, _defaultEnumString, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Enum, Value=default, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableEnum(a, _defaultEnumString, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":""{_defaultEnumString}""}}",
        },

        // .. Guid (default)
        new WritePropertyTest
        {
            Name = "Type=Nullable Guid, Value=default, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableGuid(a, _defaultGuidString, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = $@"{{""value"":""{_defaultGuidString}""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Guid, Value=default, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableGuid(a, _defaultGuidString, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Guid, Value=default, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableGuid(a, _defaultGuidString, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":""{_defaultGuidString}""}}",
        },

        // .. TimeSpan (default)
        new WritePropertyTest
        {
            Name = "Type=Nullable TimeSpan, Value=default, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableTimeSpan(a, _defaultTimeSpanString, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = $@"{{""value"":""{_defaultTimeSpanString}""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable TimeSpan, Value=default, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableTimeSpan(a, _defaultTimeSpanString, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable TimeSpan, Value=default, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableTimeSpan(a, _defaultTimeSpanString, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":""{_defaultTimeSpanString}""}}",
        },

        // .. Ulid (default)
        new WritePropertyTest
        {
            Name = "Type=Nullable Ulid, Value=default, Condition=WhenWritingNull",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableUlid(a, _defaultUlidString, nameof(JsonIgnoreCondition.WhenWritingNull)),
            ExpectedJson = $@"{{""value"":""{_defaultUlidString}""}}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Ulid, Value=default, Condition=WhenWritingDefault",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableUlid(a, _defaultUlidString, nameof(JsonIgnoreCondition.WhenWritingDefault)),
            ExpectedJson = @"{}",
        },
        new WritePropertyTest
        {
            Name = "Type=Nullable Ulid, Value=default, Condition=Never",
            WritePropertyExpression = (a) => Utf8JsonWriterTestHelper.WritePropertyNullableUlid(a, _defaultUlidString, nameof(JsonIgnoreCondition.Never)),
            ExpectedJson = $@"{{""value"":""{_defaultUlidString}""}}",
        },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(WritePropertyTheoryData))]
    public void WriteProperty(IXUnitTest test) => test.Execute(this);
    #endregion
}
