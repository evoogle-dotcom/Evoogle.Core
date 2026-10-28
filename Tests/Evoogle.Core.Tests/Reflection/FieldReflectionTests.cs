// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

using FluentAssertions;

namespace Evoogle.Reflection;

public class FieldReflectionTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public class GetNullabilityInfoTest : XUnitTest
    {
        #region User Supplied Properties
        public required Type Type { get; init; }
        public required string FieldName { get; init; }
        public required MemberNullableInfo Expected { get; init; }
        #endregion

        #region Calculated Properties
        private MemberNullableInfo? Actual { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            this.WriteLine($"Type  = {this.Type.Name}");
            this.WriteLine($"Field = {this.FieldName}");
            this.WriteLine();
            this.WriteLine($"Expected = {this.Expected}");
        }

        protected override void Act()
        {
            var fieldInfo = this.Type.GetField(this.FieldName)!;

            this.Actual = FieldReflection.GetNullabilityInfo(fieldInfo);
            this.WriteLine($"Actual   = {this.Actual}");
        }

        protected override void Assert() => this.Actual.Should().BeEquivalentTo(this.Expected);
        #endregion
    }

    public class IsStaticTest : XUnitTest
    {
        #region User Supplied Properties
        public required Type Type { get; init; }
        public required string FieldName { get; init; }
        public required bool Expected { get; init; }
        #endregion

        #region Calculated Properties
        private bool Actual { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            this.WriteLine($"Type  = {this.Type.Name}");
            this.WriteLine($"Field = {this.FieldName}");
            this.WriteLine();
            this.WriteLine($"Expected = {this.Expected}");
        }

        protected override void Act()
        {
            var fieldInfo = this.Type.GetField(this.FieldName)!;

            this.Actual = FieldReflection.IsStatic(fieldInfo);
            this.WriteLine($"Actual   = {this.Actual}");
        }

        protected override void Assert() => this.Actual.Should().Be(this.Expected);
        #endregion
    }
    #endregion

    #region Test Data
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public class Sample
    {
        public string? NullableString;
        public string String;

        public int? NullableInt;
        public int Int;

        public List<string?>? NullableListOfNullableStrings;
        public List<string>? NullableListOfStrings;
        public List<string?> ListOfNullableStrings;
        public List<string> ListOfStrings;

        public List<int?>? NullableListOfNullableInts;
        public List<int>? NullableListOfInts;
        public List<int?> ListOfNullableInts;
        public List<int> ListOfInts;

        public List<List<string?>?>? NullableListOfNullableListsOfNullableStrings;
        public List<List<List<string?>?>?>? NullableListOfNullableListsOfNullableListsOfNullableStrings;

        public List<List<int?>?>? NullableListOfNullableListsOfNullableInts;
        public List<List<List<int?>?>?>? NullableListOfNullableListsOfNullableListsOfNullableInts;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value

#nullable disable
    public class UnknownSample
    {
        public string UnknownReference;
        public List<string> UnknownCollectionWithUnknownReferenceElements;
    }
#nullable restore

    public class Widget
    {
        public string Field = string.Empty;
#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static string StaticField = string.Empty;
#pragma warning restore CA2211 // Non-constant fields should not be visible
    }
    #endregion

    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GetNullabilityInfoTestTheoryData =>
    [
        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.NullableString)}",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableString),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(string),
                Nullability = MemberNullability.Nullable,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.String)}",
            Type = typeof(Sample),
            FieldName = nameof(Sample.String),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(string),
                Nullability = MemberNullability.NonNullable,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.NullableInt)}",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableInt),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(int?),
                Nullability = MemberNullability.Nullable,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.Int)}",
            Type = typeof(Sample),
            FieldName = nameof(Sample.Int),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(int),
                Nullability = MemberNullability.NonNullable,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.NullableListOfNullableStrings)}",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableListOfNullableStrings),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<string?>),
                Nullability = MemberNullability.Nullable,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<string?>),
                        ElementType = typeof(string),
                        CollectionNullability = MemberNullability.Nullable,
                        ElementNullability = MemberNullability.Nullable
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.NullableListOfStrings)}",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableListOfStrings),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<string>),
                Nullability = MemberNullability.Nullable,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<string>),
                        ElementType = typeof(string),
                        CollectionNullability = MemberNullability.Nullable,
                        ElementNullability = MemberNullability.NonNullable
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.ListOfNullableStrings)}",
            Type = typeof(Sample),
            FieldName = nameof(Sample.ListOfNullableStrings),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<string?>),
                Nullability = MemberNullability.NonNullable,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<string?>),
                        ElementType = typeof(string),
                        CollectionNullability = MemberNullability.NonNullable,
                        ElementNullability = MemberNullability.Nullable
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.ListOfStrings)}",
            Type = typeof(Sample),
            FieldName = nameof(Sample.ListOfStrings),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<string>),
                Nullability = MemberNullability.NonNullable,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<string>),
                        ElementType = typeof(string),
                        CollectionNullability = MemberNullability.NonNullable,
                        ElementNullability = MemberNullability.NonNullable
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.NullableListOfNullableInts)}",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableListOfNullableInts),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<int?>),
                Nullability = MemberNullability.Nullable,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<int?>),
                        ElementType = typeof(int?),
                        CollectionNullability = MemberNullability.Nullable,
                        ElementNullability = MemberNullability.Nullable
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.NullableListOfInts)}",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableListOfInts),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<int>),
                Nullability = MemberNullability.Nullable,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<int>),
                        ElementType = typeof(int),
                        CollectionNullability = MemberNullability.Nullable,
                        ElementNullability = MemberNullability.NonNullable
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.ListOfNullableInts)}",
            Type = typeof(Sample),
            FieldName = nameof(Sample.ListOfNullableInts),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<int?>),
                Nullability = MemberNullability.NonNullable,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<int?>),
                        ElementType = typeof(int?),
                        CollectionNullability = MemberNullability.NonNullable,
                        ElementNullability = MemberNullability.Nullable
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.ListOfInts)}",
            Type = typeof(Sample),
            FieldName = nameof(Sample.ListOfInts),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<int>),
                Nullability = MemberNullability.NonNullable,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<int>),
                        ElementType = typeof(int),
                        CollectionNullability = MemberNullability.NonNullable,
                        ElementNullability = MemberNullability.NonNullable
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.NullableListOfNullableListsOfNullableStrings)}",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableListOfNullableListsOfNullableStrings),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<List<string?>?>),
                Nullability = MemberNullability.Nullable,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<List<string?>?>),
                        ElementType = typeof(List<string?>),
                        CollectionNullability = MemberNullability.Nullable,
                        ElementNullability = MemberNullability.Nullable
                    },
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<string?>),
                        ElementType = typeof(string),
                        CollectionNullability = MemberNullability.Nullable,
                        ElementNullability = MemberNullability.Nullable
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.NullableListOfNullableListsOfNullableListsOfNullableStrings)}",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableListOfNullableListsOfNullableListsOfNullableStrings),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<List<List<string?>?>?>),
                Nullability = MemberNullability.Nullable,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<List<List<string?>?>?>),
                        ElementType = typeof(List<List<string?>?>),
                        CollectionNullability = MemberNullability.Nullable,
                        ElementNullability = MemberNullability.Nullable
                    },
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<List<string?>?>),
                        ElementType = typeof(List<string?>),
                        CollectionNullability = MemberNullability.Nullable,
                        ElementNullability = MemberNullability.Nullable
                    },
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<string?>),
                        ElementType = typeof(string),
                        CollectionNullability = MemberNullability.Nullable,
                        ElementNullability = MemberNullability.Nullable
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.NullableListOfNullableListsOfNullableInts)}",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableListOfNullableListsOfNullableInts),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<List<int?>?>),
                Nullability = MemberNullability.Nullable,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<List<int?>?>),
                        ElementType = typeof(List<int?>),
                        CollectionNullability = MemberNullability.Nullable,
                        ElementNullability = MemberNullability.Nullable
                    },
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<int?>),
                        ElementType = typeof(int?),
                        CollectionNullability = MemberNullability.Nullable,
                        ElementNullability = MemberNullability.Nullable
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.NullableListOfNullableListsOfNullableListsOfNullableInts)}",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableListOfNullableListsOfNullableListsOfNullableInts),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<List<List<int?>?>?>),
                Nullability = MemberNullability.Nullable,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<List<List<int?>?>?>),
                        ElementType = typeof(List<List<int?>?>),
                        CollectionNullability = MemberNullability.Nullable,
                        ElementNullability = MemberNullability.Nullable
                    },
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<List<int?>?>),
                        ElementType = typeof(List<int?>),
                        CollectionNullability = MemberNullability.Nullable,
                        ElementNullability = MemberNullability.Nullable
                    },
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<int?>),
                        ElementType = typeof(int?),
                        CollectionNullability = MemberNullability.Nullable,
                        ElementNullability = MemberNullability.Nullable
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(UnknownSample)}.{nameof(UnknownSample.UnknownReference)}",
            Type = typeof(UnknownSample),
            FieldName = nameof(UnknownSample.UnknownReference),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(string),
                Nullability = MemberNullability.Unknown,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(UnknownSample)}.{nameof(UnknownSample.UnknownCollectionWithUnknownReferenceElements)}",
            Type = typeof(UnknownSample),
            FieldName = nameof(UnknownSample.UnknownCollectionWithUnknownReferenceElements),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<string>),
                Nullability = MemberNullability.Unknown,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<string>),
                        CollectionNullability = MemberNullability.Unknown,
                        ElementType = typeof(string),
                        ElementNullability = MemberNullability.Unknown
                    }
                ]
            }
        },
    ];

    public static TheoryDataRow<IXUnitTest>[] IsStaticTheoryData =>
    [
        new IsStaticTest
        {
            Name = $"{nameof(Widget)}.{nameof(Widget.Field)}",
            Type = typeof(Widget),
            FieldName = nameof(Widget.Field),
            Expected = false
        },
        new IsStaticTest
        {
            Name = $"{nameof(Widget)}.{nameof(Widget.StaticField)}",
            Type = typeof(Widget),
            FieldName = nameof(Widget.StaticField),
            Expected = true
        },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(GetNullabilityInfoTestTheoryData))]
    public void GetNullabilityInfo(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(IsStaticTheoryData))]
    public void IsStatic(IXUnitTest test) => test.Execute(this);
    #endregion
}
