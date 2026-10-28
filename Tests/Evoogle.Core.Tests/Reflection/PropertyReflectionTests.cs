// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

using FluentAssertions;

namespace Evoogle.Reflection;

public class PropertyReflectionTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public class GetNullabilityInfoTest : XUnitTest
    {
        #region User Supplied Properties
        public required Type Type { get; init; }
        public required string PropertyName { get; init; }
        public required MemberNullableInfo Expected { get; init; }
        #endregion

        #region Calculated Properties
        private MemberNullableInfo? Actual { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            this.WriteLine($"Type     = {this.Type.Name}");
            this.WriteLine($"Property = {this.PropertyName}");
            this.WriteLine();
            this.WriteLine($"Expected = {this.Expected}");
        }

        protected override void Act()
        {
            var propertyInfo = this.Type.GetProperty(this.PropertyName)!;

            this.Actual = PropertyReflection.GetNullabilityInfo(propertyInfo);
            this.WriteLine($"Actual   = {this.Actual}");
        }

        protected override void Assert() => this.Actual.Should().BeEquivalentTo(this.Expected);
        #endregion
    }

    public class IsStaticTest : XUnitTest
    {
        #region User Supplied Properties
        public required Type Type { get; init; }
        public required string PropertyName { get; init; }
        public required bool Expected { get; init; }
        #endregion

        #region Calculated Properties
        private bool Actual { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            this.WriteLine($"Type     = {this.Type.Name}");
            this.WriteLine($"Property = {this.PropertyName}");
            this.WriteLine();
            this.WriteLine($"Expected = {this.Expected}");
        }

        protected override void Act()
        {
            var propertyInfo = this.Type.GetProperty(this.PropertyName)!;

            this.Actual = PropertyReflection.IsStatic(propertyInfo);
            this.WriteLine($"Actual   = {this.Actual}");
        }

        protected override void Assert() => this.Actual.Should().Be(this.Expected);
        #endregion
    }
    #endregion

    #region Test Data
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public class Sample
    {
        public string? NullableString { get; set; }
        public string String { get; set; }

        public int? NullableInt { get; set; }
        public int Int { get; set; }

        public List<string?>? NullableListOfNullableStrings { get; set; }
        public List<string>? NullableListOfStrings { get; set; }
        public List<string?> ListOfNullableStrings { get; set; }
        public List<string> ListOfStrings { get; set; }

        public List<int?>? NullableListOfNullableInts { get; set; }
        public List<int>? NullableListOfInts { get; set; }
        public List<int?> ListOfNullableInts { get; set; }
        public List<int> ListOfInts { get; set; }

        public List<List<string?>?>? NullableListOfNullableListsOfNullableStrings { get; set; }
        public List<List<List<string?>?>?>? NullableListOfNullableListsOfNullableListsOfNullableStrings { get; set; }

        public List<List<int?>?>? NullableListOfNullableListsOfNullableInts { get; set; }
        public List<List<List<int?>?>?>? NullableListOfNullableListsOfNullableListsOfNullableInts { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

#nullable disable
    public class UnknownSample
    {
        public string UnknownReference { get; set; }
        public List<string> UnknownCollectionWithUnknownReferenceElements { get; set; }
    }
#nullable restore

    public class Widget
    {
        public string Property { get; } = string.Empty;
        public static string StaticProperty { get; } = string.Empty;
    }
    #endregion

    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GetNullabilityInfoTestTheoryData =>
    [
        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.NullableString)}",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NullableString),
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
            PropertyName = nameof(Sample.String),
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
            PropertyName = nameof(Sample.NullableInt),
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
            PropertyName = nameof(Sample.Int),
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
            PropertyName = nameof(Sample.NullableListOfNullableStrings),
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
            PropertyName = nameof(Sample.NullableListOfStrings),
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
            PropertyName = nameof(Sample.ListOfNullableStrings),
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
            PropertyName = nameof(Sample.ListOfStrings),
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
            PropertyName = nameof(Sample.NullableListOfNullableInts),
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
            PropertyName = nameof(Sample.NullableListOfInts),
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
            PropertyName = nameof(Sample.ListOfNullableInts),
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
            PropertyName = nameof(Sample.ListOfInts),
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
            PropertyName = nameof(Sample.NullableListOfNullableListsOfNullableStrings),
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
            PropertyName = nameof(Sample.NullableListOfNullableListsOfNullableListsOfNullableStrings),
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
            PropertyName = nameof(Sample.NullableListOfNullableListsOfNullableInts),
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
            PropertyName = nameof(Sample.NullableListOfNullableListsOfNullableListsOfNullableInts),
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
            PropertyName = nameof(UnknownSample.UnknownReference),
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
            PropertyName = nameof(UnknownSample.UnknownCollectionWithUnknownReferenceElements),
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
            Name = $"{nameof(Widget)}.{nameof(Widget.Property)}",
            Type = typeof(Widget),
            PropertyName = nameof(Widget.Property),
            Expected = false
        },
        new IsStaticTest
        {
            Name = $"{nameof(Widget)}.{nameof(Widget.StaticProperty)}",
            Type = typeof(Widget),
            PropertyName = nameof(Widget.StaticProperty),
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
