// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;

using Evoogle.XUnit;

using FluentAssertions;

namespace Evoogle.Json;

public class EnumJsonConverterTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public class JsonDeserializeTest<TEnum> : XUnitTest
        where TEnum : struct, Enum
    {
        #region Calculated Properties
        public TEnum? ActualEnum { get; set; }
        public string? ActualException { get; set; }

        private static JsonSerializerOptions Options { get; } = new JsonSerializerOptions { Converters = { new EnumJsonConverter<TEnum>() } };
        #endregion

        #region User Supplied Properties
        public string? SourceJson { get; set; }
        public TEnum? ExpectedEnum { get; set; }
        public string? ExpectedException { get; set; }
        #endregion

        protected override void Arrange()
        {
            this.WriteLine($"Source        JSON: {this.SourceJson.SafeToString()}");

            if (this.ExpectedEnum != null)
            {
                this.WriteLine($"Expected      Enum: {this.ExpectedEnum.SafeToString()}");
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
                this.ActualEnum = JsonSerializer.Deserialize<TEnum?>(this.SourceJson!, Options);
                this.WriteLine($"Actual        Enum: {this.ActualEnum.SafeToString()}");
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
            if (this.ExpectedEnum != null)
            {
                this.ActualException.Should().BeNull();

                this.ActualEnum.Should().NotBeNull();
                this.ActualEnum.Should().Be(this.ExpectedEnum);
            }
            else if (this.ExpectedException != null)
            {
                this.ActualEnum.Should().BeNull();

                this.ActualException.Should().NotBeNull();
                this.ActualException.Should().Be(this.ExpectedException);
            }
        }
    }

    public class JsonSerializeTest<TEnum> : XUnitTest
        where TEnum : struct, Enum
    {
        #region Calculated Properties
        public string? ActualJson { get; set; }
        private static JsonSerializerOptions Options { get; } = new JsonSerializerOptions { Converters = { new EnumJsonConverter<TEnum>() } };
        #endregion

        #region User Supplied Properties
        public TEnum? SourceEnum { get; set; }
        public string? ExpectedJson { get; set; }
        #endregion

        protected override void Arrange()
        {
            this.WriteLine($"Source    Enum: {this.SourceEnum.SafeToString()}");
            this.WriteLine($"Expected  JSON: {this.ExpectedJson.SafeToString()}");
        }

        protected override void Act()
        {
            this.ActualJson = JsonSerializer.Serialize(this.SourceEnum, Options);
            this.WriteLine($"Actual    JSON: {this.ActualJson.SafeToString()}");
        }

        protected override void Assert()
        {
            var actualJsonMinusWhitespace = this.ActualJson.RemoveWhitespace();
            var expectedJsonMinusWhitespace = this.ExpectedJson.RemoveWhitespace();

            actualJsonMinusWhitespace.Should().Be(expectedJsonMinusWhitespace);
        }
    }
    #endregion

    #region Theory Data
    public enum Color
    {
        Transparent,
        Red,
        Green,
        Blue
    }

    [Flags]
    public enum Permissions
    {
        None = 0,
        Read = 1,
        Write = 2,
        Execute = 4
    }

    public static TheoryDataRow<IXUnitTest>[] DeserializeTheoryData =>
    [
        // Regular Enum Should Deserialize From String Name
        new JsonDeserializeTest<Color>
        {
            Name = "Regular Enum Should Deserialize From String Name - Red",
            SourceJson = @"""Red""",
            ExpectedEnum = Color.Red
        },
        new JsonDeserializeTest<Color>
        {
            Name = "Regular Enum Should Deserialize From String Name (Case Insensitive) - RED",
            SourceJson = @"""RED""",
            ExpectedEnum = Color.Red
        },
        new JsonDeserializeTest<Color>
        {
            Name = "Regular Enum Should Deserialize From String Name (Case Insensitive) - red",
            SourceJson = @"""red""",
            ExpectedEnum = Color.Red
        },
        new JsonDeserializeTest<Color>
        {
            Name = "Regular Enum Should Deserialize From String Name - Green",
            SourceJson = @"""Green""",
            ExpectedEnum = Color.Green
        },
        new JsonDeserializeTest<Color>
        {
            Name = "Regular Enum Should Deserialize From String Name - Blue",
            SourceJson = @"""Blue""",
            ExpectedEnum = Color.Blue
        },

        // Regular Enum Should Deserialize From Null Literal
        new JsonDeserializeTest<Color>
        {
            Name = "Regular Enum Should Deserialize From Null Literal",
            SourceJson = "null",
            ExpectedEnum = null
        },

        // Regular Enum Should Deserialize From Empty or Whitespace String Name
        new JsonDeserializeTest<Color>
        {
            Name = "Regular Enum Should Deserialize From Empty String Name - Empty String",
            SourceJson = @"""""",
            ExpectedEnum = default(Color)
        },
        new JsonDeserializeTest<Color>
        {
            Name = "Regular Enum Should Deserialize From Whitespace String Name - Whitespace String",
            SourceJson = @"""       """,
            ExpectedEnum = default(Color)
        },

        // Regular Enum Should Throw Exception From Invalid String Name
        new JsonDeserializeTest<Color>
        {
            Name = "Regular Enum Should Throw JsonException From Invalid String Name - Invalid",
            SourceJson = @"""Invalid""",
            ExpectedException = nameof(JsonException)
        },
        new JsonDeserializeTest<Color>
        {
            Name = "Regular Enum Should Throw JsonException From Invalid String Name - Red, Green",
            SourceJson = @"""Red, Green""",
            ExpectedException = nameof(JsonException)
        },

        // Flags Enum Should Deserialize From Comma Separated String Names
        new JsonDeserializeTest<Permissions>
        {
            Name = "Flags Enum Should Deserialize From Comma Separated String Names - Read",
            SourceJson = @"""Read""",
            ExpectedEnum = Permissions.Read
        },
        new JsonDeserializeTest<Permissions>
        {
            Name = "Flags Enum Should Deserialize From Comma Separated String Names - Read, Write",
            SourceJson = @"""Read, Write""",
            ExpectedEnum = Permissions.Read | Permissions.Write
        },
        new JsonDeserializeTest<Permissions>
        {
            Name = "Flags Enum Should Deserialize From Comma Separated String Names - Read, Write, Execute",
            SourceJson = @"""Read, Write, Execute""",
            ExpectedEnum = Permissions.Read | Permissions.Write | Permissions.Execute
        },
        new JsonDeserializeTest<Permissions>
        {
            Name = "Flags Enum Should Deserialize From Comma Separated String Names (Case Insensitive) - READ, WRITE, EXECUTE",
            SourceJson = @"""READ, WRITE, EXECUTE""",
            ExpectedEnum = Permissions.Read | Permissions.Write | Permissions.Execute
        },
        new JsonDeserializeTest<Permissions>
        {
            Name = "Flags Enum Should Deserialize From Comma Separated String Names (Case Insensitive) - read, write, execute",
            SourceJson = @"""read, write, execute""",
            ExpectedEnum = Permissions.Read | Permissions.Write | Permissions.Execute
        },

        // Flags Enum Should Deserialize From Null Literal
        new JsonDeserializeTest<Permissions>
        {
            Name = "Flags Enum Should Deserialize From Null Literal",
            SourceJson = "null",
            ExpectedEnum = null
        },

        // Flags Enum Should Deserialize From Empty or Whitespace String Name
        new JsonDeserializeTest<Permissions>
        {
            Name = "Flags Enum Should Deserialize From Empty String Name - Empty String",
            SourceJson = @"""""",
            ExpectedEnum = default(Permissions)
        },
        new JsonDeserializeTest<Permissions>
        {
            Name = "Flags Enum Should Deserialize From Whitespace String Name - Whitespace String",
            SourceJson = @"""       """,
            ExpectedEnum = default(Permissions)
        },

        // Flags Enum Should Throw Exception From Invalid String Name
        new JsonDeserializeTest<Permissions>
        {
            Name = "Flags Enum Should Throw JsonException From Invalid String Name - Invalid",
            SourceJson = @"""Invalid""",
            ExpectedException = nameof(JsonException)
        },
        new JsonDeserializeTest<Permissions>
        {
            Name = "Flags Enum Should Throw JsonException From Invalid String Name - Read, Invalid",
            SourceJson = @"""Read, Invalid""",
            ExpectedException = nameof(JsonException)
        },
    ];

    public static TheoryDataRow<IXUnitTest>[] SerializeTheoryData =>
    [
        // Regular Enum Should Serialize To String Name
        new JsonSerializeTest<Color>
        {
            Name = "Regular Enum Should Serialize To String Name - Color.Red",
            SourceEnum = Color.Red,
            ExpectedJson = @"""Red"""
        },
        new JsonSerializeTest<Color>
        {
            Name = "Regular Enum Should Serialize To String Name - Color.Green",
            SourceEnum = Color.Green,
            ExpectedJson = @"""Green"""
        },
        new JsonSerializeTest<Color>
        {
            Name = "Regular Enum Should Serialize To String Name - Color.Blue",
            SourceEnum = Color.Blue,
            ExpectedJson = @"""Blue"""
        },

        // Flags Enum Should Serialize To Comma Separated String Names
        new JsonSerializeTest<Permissions>
        {
            Name = "Flags Enum Should Serialize To Comma Separated String Names - Permissions.Read",
            SourceEnum = Permissions.Read,
            ExpectedJson = @"""Read"""
        },
        new JsonSerializeTest<Permissions>
        {
            Name = "Flags Enum Should Serialize To Comma Separated String Names - Permissions.Read | Permissions.Write",
            SourceEnum = Permissions.Read | Permissions.Write,
            ExpectedJson = @"""Read, Write"""
        },
        new JsonSerializeTest<Permissions>
        {
            Name = "Flags Enum Should Serialize To Comma Separated String Names - Permissions.Read | Permissions.Write | Permissions.Execute",
            SourceEnum = Permissions.Read | Permissions.Write | Permissions.Execute,
            ExpectedJson = @"""Read, Write, Execute"""
        },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(DeserializeTheoryData))]
    public void Deserialize(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(SerializeTheoryData))]
    public void Serialize(IXUnitTest test)
    {
        test.Execute(this);
    }
    #endregion
}