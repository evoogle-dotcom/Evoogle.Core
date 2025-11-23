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
        public Type Type { get; init; } = null!;
        public string FieldName { get; init; } = null!;
        public MemberNullableInfo Expected { get; init; } = null!;
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
        public Type Type { get; init; } = null!;
        public string FieldName { get; init; } = null!;
        public bool Expected { get; init; }
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
        public string? NullableReference;
        public string NonNullableReference;

        public int? NullableValue;
        public int NonNullableValue;

        public List<string?>? NullableCollectionWithNullableReferenceElements;
        public List<string>? NullableCollectionWithNonNullableReferenceElements;
        public List<string?> NonNullableCollectionWithNullableReferenceElements;
        public List<string> NonNullableCollectionWithNonNullableReferenceElements;

        public List<int?>? NullableCollectionWithNullableValueElements;
        public List<int>? NullableCollectionWithNonNullableValueElements;
        public List<int?> NonNullableCollectionWithNullableValueElements;
        public List<int> NonNullableCollectionWithNonNullableValueElements;

        public List<List<string?>?>? NullableCollectionWithNullableCollectionWithNullableReferenceElements;
        public List<List<List<string?>?>?>? NullableCollectionWithNullableCollectionWithNullableCollectionWithNullableReferenceElements;

        public List<List<int?>?>? NullableCollectionWithNullableCollectionWithNullableValueElements;
        public List<List<List<int?>?>?>? NullableCollectionWithNullableCollectionWithNullableCollectionWithNullableValueElements;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value

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
            Name = "With Nullable Reference",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableReference),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(string),
                IsNullable = true,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Non Nullable Reference",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NonNullableReference),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(string),
                IsNullable = false,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Nullable Value",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableValue),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(int?),
                IsNullable = true,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Non Nullable Value",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NonNullableValue),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(int),
                IsNullable = false,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Nullable Collection With Nullable Reference Elements",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableCollectionWithNullableReferenceElements),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<string?>),
                IsNullable = true,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<string?>),
                        ElementType = typeof(string),
                        IsCollectionNullable = true,
                        IsElementNullable = true
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Nullable Collection With Non Nullable Reference Elements",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableCollectionWithNonNullableReferenceElements),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<string>),
                IsNullable = true,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<string>),
                        ElementType = typeof(string),
                        IsCollectionNullable = true,
                        IsElementNullable = false
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Non Nullable Collection With Nullable Reference Elements",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NonNullableCollectionWithNullableReferenceElements),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<string?>),
                IsNullable = false,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<string?>),
                        ElementType = typeof(string),
                        IsCollectionNullable = false,
                        IsElementNullable = true
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Non Nullable Collection With Non Nullable Reference Elements",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NonNullableCollectionWithNonNullableReferenceElements),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<string>),
                IsNullable = false,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<string>),
                        ElementType = typeof(string),
                        IsCollectionNullable = false,
                        IsElementNullable = false
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Nullable Collection With Nullable Value Elements",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableCollectionWithNullableValueElements),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<int?>),
                IsNullable = true,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<int?>),
                        ElementType = typeof(int?),
                        IsCollectionNullable = true,
                        IsElementNullable = true
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Nullable Collection With Non Nullable Value Elements",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableCollectionWithNonNullableValueElements),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<int>),
                IsNullable = true,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<int>),
                        ElementType = typeof(int),
                        IsCollectionNullable = true,
                        IsElementNullable = false
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Non Nullable Collection With Nullable Value Elements",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NonNullableCollectionWithNullableValueElements),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<int?>),
                IsNullable = false,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<int?>),
                        ElementType = typeof(int?),
                        IsCollectionNullable = false,
                        IsElementNullable = true
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Non Nullable Collection With Non Nullable Value Elements",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NonNullableCollectionWithNonNullableValueElements),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<int>),
                IsNullable = false,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<int>),
                        ElementType = typeof(int),
                        IsCollectionNullable = false,
                        IsElementNullable = false
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Nullable Collection With Nullable Collection With Nullable Reference Elements",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableCollectionWithNullableCollectionWithNullableReferenceElements),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<List<string?>?>),
                IsNullable = true,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<List<string?>?>),
                        ElementType = typeof(List<string?>),
                        IsCollectionNullable = true,
                        IsElementNullable = true
                    },
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<string?>),
                        ElementType = typeof(string),
                        IsCollectionNullable = true,
                        IsElementNullable = true
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Nullable Collection With Nullable Collection With Nullable Collection With Nullable Reference Elements",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableCollectionWithNullableCollectionWithNullableCollectionWithNullableReferenceElements),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<List<List<string?>?>?>),
                IsNullable = true,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<List<List<string?>?>?>),
                        ElementType = typeof(List<List<string?>?>),
                        IsCollectionNullable = true,
                        IsElementNullable = true
                    },
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<List<string?>?>),
                        ElementType = typeof(List<string?>),
                        IsCollectionNullable = true,
                        IsElementNullable = true
                    },
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<string?>),
                        ElementType = typeof(string),
                        IsCollectionNullable = true,
                        IsElementNullable = true
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Nullable Collection With Nullable Collection With Nullable Value Elements",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableCollectionWithNullableCollectionWithNullableValueElements),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<List<int?>?>),
                IsNullable = true,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<List<int?>?>),
                        ElementType = typeof(List<int?>),
                        IsCollectionNullable = true,
                        IsElementNullable = true
                    },
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<int?>),
                        ElementType = typeof(int?),
                        IsCollectionNullable = true,
                        IsElementNullable = true
                    }
                ]
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Nullable Collection With Nullable Collection With Nullable Collection With Nullable Value Elements",
            Type = typeof(Sample),
            FieldName = nameof(Sample.NullableCollectionWithNullableCollectionWithNullableCollectionWithNullableValueElements),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(List<List<List<int?>?>?>),
                IsNullable = true,
                CollectionChain = [
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<List<List<int?>?>?>),
                        ElementType = typeof(List<List<int?>?>),
                        IsCollectionNullable = true,
                        IsElementNullable = true
                    },
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<List<int?>?>),
                        ElementType = typeof(List<int?>),
                        IsCollectionNullable = true,
                        IsElementNullable = true
                    },
                    new MemberNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<int?>),
                        ElementType = typeof(int?),
                        IsCollectionNullable = true,
                        IsElementNullable = true
                    }
                ]
            }
        },
    ];

    public static TheoryDataRow<IXUnitTest>[] IsStaticTheoryData =>
    [
        new IsStaticTest
        {
            Name = "With Instance Field",
            Type = typeof(Widget),
            FieldName = nameof(Widget.Field),
            Expected = false
        },
        new IsStaticTest
        {
            Name = "With Static Field",
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
