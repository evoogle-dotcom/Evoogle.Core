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

public class NullableEnumJsonConverterTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public class DeserializeTest<TEnum> : XUnitTest
        where TEnum : struct, Enum
    {
        #region User Supplied Properties
        public required string SourceJson { get; init; }
        public required EnumJsonInvalidValuePolicy InvalidValuePolicy { get; init; }
        public TEnum? ExpectedEnum { get; init; }
        public bool ExpectsNull { get; init; }
        public Type? ExpectedExceptionType { get; init; }
        #endregion

        #region Calculated Properties
        private TEnum? ActualEnum { get; set; }
        private Exception? ActualException { get; set; }
        private JsonSerializerOptions JsonSerializerOptions { get; } = new();
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            this.JsonSerializerOptions.Converters.Add
            (
                new NullableEnumJsonConverter<TEnum>(this.InvalidValuePolicy)
            );
        }

        protected override void Act()
        {
            try
            {
                this.ActualEnum = JsonSerializer.Deserialize<TEnum?>(this.SourceJson, this.JsonSerializerOptions);
            }
            catch (Exception exception)
            {
                this.ActualException = exception;
            }
        }

        protected override void Assert()
        {
            if (this.ExpectedExceptionType is not null)
            {
                this.ActualEnum.Should().BeNull();
                this.ActualException.Should().BeOfType(this.ExpectedExceptionType);
                return;
            }

            this.ActualException.Should().BeNull();

            if (this.ExpectsNull)
            {
                this.ActualEnum.Should().BeNull();
                return;
            }

            this.ActualEnum.Should().Be(this.ExpectedEnum);
        }
        #endregion
    }

    public class SerializeTest<TEnum> : XUnitTest
        where TEnum : struct, Enum
    {
        #region User Supplied Properties
        public required TEnum? SourceEnum { get; init; }
        public required EnumJsonInvalidValuePolicy InvalidValuePolicy { get; init; }
        public required string ExpectedJson { get; init; }
        #endregion

        #region Calculated Properties
        private string? ActualJson { get; set; }
        private JsonSerializerOptions JsonSerializerOptions { get; } = new();
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            this.JsonSerializerOptions.Converters.Add
            (
                new NullableEnumJsonConverter<TEnum>(this.InvalidValuePolicy)
            );
        }

        protected override void Act()
        {
            this.ActualJson = JsonSerializer.Serialize(this.SourceEnum, this.JsonSerializerOptions);
        }

        protected override void Assert()
        {
            this.ActualJson.RemoveWhitespace().Should().Be(this.ExpectedJson.RemoveWhitespace());
        }
        #endregion
    }
    #endregion

    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] DeserializeTheoryData =>
    [
        new DeserializeTest<EnumJsonConverterTests.WireColor>
        {
            Name = "ReturnNull deserializes EnumMember values",
            SourceJson = @"""BRIGHT_RED""",
            InvalidValuePolicy = EnumJsonInvalidValuePolicy.ReturnNull,
            ExpectedEnum = EnumJsonConverterTests.WireColor.Red
        },
        new DeserializeTest<EnumJsonConverterTests.WirePermissions>
        {
            Name = "ReturnNull deserializes Flags EnumMember values",
            SourceJson = @"""CAN_READ, CAN_WRITE""",
            InvalidValuePolicy = EnumJsonInvalidValuePolicy.ReturnNull,
            ExpectedEnum = EnumJsonConverterTests.WirePermissions.Read |
                EnumJsonConverterTests.WirePermissions.Write
        },
        new DeserializeTest<EnumJsonConverterTests.Color>
        {
            Name = "ReturnNull returns null for a null token",
            SourceJson = "null",
            InvalidValuePolicy = EnumJsonInvalidValuePolicy.ReturnNull,
            ExpectsNull = true
        },
        new DeserializeTest<EnumJsonConverterTests.Color>
        {
            Name = "ReturnNull returns null for an empty enum value",
            SourceJson = @"""""",
            InvalidValuePolicy = EnumJsonInvalidValuePolicy.ReturnNull,
            ExpectsNull = true
        },
        new DeserializeTest<EnumJsonConverterTests.Color>
        {
            Name = "ReturnNull returns null for an unknown enum value",
            SourceJson = @"""Unknown""",
            InvalidValuePolicy = EnumJsonInvalidValuePolicy.ReturnNull,
            ExpectsNull = true
        },
        new DeserializeTest<EnumJsonConverterTests.Color>
        {
            Name = "ReturnNull returns null for a numeric token",
            SourceJson = "1",
            InvalidValuePolicy = EnumJsonInvalidValuePolicy.ReturnNull,
            ExpectsNull = true
        },
        new DeserializeTest<EnumJsonConverterTests.Color>
        {
            Name = "ReturnNull consumes an array token and returns null",
            SourceJson = @"[""Red""]",
            InvalidValuePolicy = EnumJsonInvalidValuePolicy.ReturnNull,
            ExpectsNull = true
        },
        new DeserializeTest<EnumJsonConverterTests.Color>
        {
            Name = "ReturnNull consumes an object token and returns null",
            SourceJson = @"{ ""Value"": ""Red"" }",
            InvalidValuePolicy = EnumJsonInvalidValuePolicy.ReturnNull,
            ExpectsNull = true
        },
        new DeserializeTest<EnumJsonConverterTests.WireColor>
        {
            Name = "Throw deserializes a non-null EnumMember value",
            SourceJson = @"""BRIGHT_RED""",
            InvalidValuePolicy = EnumJsonInvalidValuePolicy.Throw,
            ExpectedEnum = EnumJsonConverterTests.WireColor.Red
        },
        new DeserializeTest<EnumJsonConverterTests.Color>
        {
            Name = "Throw rejects a null token",
            SourceJson = "null",
            InvalidValuePolicy = EnumJsonInvalidValuePolicy.Throw,
            ExpectedExceptionType = typeof(JsonException)
        },
        new DeserializeTest<EnumJsonConverterTests.Color>
        {
            Name = "Throw rejects an empty enum value",
            SourceJson = @"""""",
            InvalidValuePolicy = EnumJsonInvalidValuePolicy.Throw,
            ExpectedExceptionType = typeof(JsonException)
        },
        new DeserializeTest<EnumJsonConverterTests.Color>
        {
            Name = "Throw rejects an unknown enum value",
            SourceJson = @"""Unknown""",
            InvalidValuePolicy = EnumJsonInvalidValuePolicy.Throw,
            ExpectedExceptionType = typeof(JsonException)
        },
        new DeserializeTest<EnumJsonConverterTests.Color>
        {
            Name = "Throw rejects a numeric token",
            SourceJson = "1",
            InvalidValuePolicy = EnumJsonInvalidValuePolicy.Throw,
            ExpectedExceptionType = typeof(JsonException)
        },
        new DeserializeTest<EnumJsonConverterTests.Color>
        {
            Name = "Throw rejects an array token",
            SourceJson = @"[""Red""]",
            InvalidValuePolicy = EnumJsonInvalidValuePolicy.Throw,
            ExpectedExceptionType = typeof(JsonException)
        },
        new DeserializeTest<EnumJsonConverterTests.Color>
        {
            Name = "Throw rejects an object token",
            SourceJson = @"{ ""Value"": ""Red"" }",
            InvalidValuePolicy = EnumJsonInvalidValuePolicy.Throw,
            ExpectedExceptionType = typeof(JsonException)
        },
    ];

    public static TheoryDataRow<IXUnitTest>[] SerializeTheoryData =>
    [
        new SerializeTest<EnumJsonConverterTests.WireColor>
        {
            Name = "ReturnNull serializes EnumMember values",
            SourceEnum = EnumJsonConverterTests.WireColor.Red,
            InvalidValuePolicy = EnumJsonInvalidValuePolicy.ReturnNull,
            ExpectedJson = @"""BRIGHT_RED"""
        },
        new SerializeTest<EnumJsonConverterTests.WireColor>
        {
            Name = "Throw serializes EnumMember values identically",
            SourceEnum = EnumJsonConverterTests.WireColor.Red,
            InvalidValuePolicy = EnumJsonInvalidValuePolicy.Throw,
            ExpectedJson = @"""BRIGHT_RED"""
        },
        new SerializeTest<EnumJsonConverterTests.Color>
        {
            Name = "Nullable enum converter serializes null",
            SourceEnum = null,
            InvalidValuePolicy = EnumJsonInvalidValuePolicy.ReturnNull,
            ExpectedJson = "null"
        },
        new SerializeTest<EnumJsonConverterTests.Color>
        {
            Name = "Throw serializes null",
            SourceEnum = null,
            InvalidValuePolicy = EnumJsonInvalidValuePolicy.Throw,
            ExpectedJson = "null"
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
