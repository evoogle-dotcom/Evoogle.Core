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
        public Type Type { get; init; } = null!;
        public string PropertyName { get; init; } = null!;
        public PropertyNullableInfo Expected { get; init; } = null!;
        #endregion

        #region Calculated Properties
        private PropertyNullableInfo? Actual { get; set; }
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
        public Type Type { get; init; } = null!;
        public string PropertyName { get; init; } = null!;
        public bool Expected { get; init; }
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
        public string? NullableReference { get; set; }
        public string NonNullableReference { get; set; }

        public int? NullableValue { get; set; }
        public int NonNullableValue { get; set; }

        public List<string?>? NullableCollectionWithNullableReferenceElements { get; set; }
        public List<string>? NullableCollectionWithNonNullableReferenceElements { get; set; }
        public List<string?> NonNullableCollectionWithNullableReferenceElements { get; set; }
        public List<string> NonNullableCollectionWithNonNullableReferenceElements { get; set; }

        public List<int?>? NullableCollectionWithNullableValueElements { get; set; }
        public List<int>? NullableCollectionWithNonNullableValueElements { get; set; }
        public List<int?> NonNullableCollectionWithNullableValueElements { get; set; }
        public List<int> NonNullableCollectionWithNonNullableValueElements { get; set; }

        public List<List<string?>?>? NullableCollectionWithNullableCollectionWithNullableReferenceElements { get; set; }
        public List<List<List<string?>?>?>? NullableCollectionWithNullableCollectionWithNullableCollectionWithNullableReferenceElements { get; set; }

        public List<List<int?>?>? NullableCollectionWithNullableCollectionWithNullableValueElements { get; set; }
        public List<List<List<int?>?>?>? NullableCollectionWithNullableCollectionWithNullableCollectionWithNullableValueElements { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

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
            Name = "With Nullable Reference",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NullableReference),
            Expected = new PropertyNullableInfo
            {
                PropertyType = typeof(string),
                IsNullable = true,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Non Nullable Reference",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NonNullableReference),
            Expected = new PropertyNullableInfo
            {
                PropertyType = typeof(string),
                IsNullable = false,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Nullable Value",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NullableValue),
            Expected = new PropertyNullableInfo
            {
                PropertyType = typeof(int?),
                IsNullable = true,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Non Nullable Value",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NonNullableValue),
            Expected = new PropertyNullableInfo
            {
                PropertyType = typeof(int),
                IsNullable = false,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Nullable Collection With Nullable Reference Elements",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NullableCollectionWithNullableReferenceElements),
            Expected = new PropertyNullableInfo
            {
                PropertyType = typeof(List<string?>),
                IsNullable = true,
                CollectionChain = [
                    new PropertyNullableInfo.CollectionInfo
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
            PropertyName = nameof(Sample.NullableCollectionWithNonNullableReferenceElements),
            Expected = new PropertyNullableInfo
            {
                PropertyType = typeof(List<string>),
                IsNullable = true,
                CollectionChain = [
                    new PropertyNullableInfo.CollectionInfo
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
            PropertyName = nameof(Sample.NonNullableCollectionWithNullableReferenceElements),
            Expected = new PropertyNullableInfo
            {
                PropertyType = typeof(List<string?>),
                IsNullable = false,
                CollectionChain = [
                    new PropertyNullableInfo.CollectionInfo
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
            PropertyName = nameof(Sample.NonNullableCollectionWithNonNullableReferenceElements),
            Expected = new PropertyNullableInfo
            {
                PropertyType = typeof(List<string>),
                IsNullable = false,
                CollectionChain = [
                    new PropertyNullableInfo.CollectionInfo
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
            PropertyName = nameof(Sample.NullableCollectionWithNullableValueElements),
            Expected = new PropertyNullableInfo
            {
                PropertyType = typeof(List<int?>),
                IsNullable = true,
                CollectionChain = [
                    new PropertyNullableInfo.CollectionInfo
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
            PropertyName = nameof(Sample.NullableCollectionWithNonNullableValueElements),
            Expected = new PropertyNullableInfo
            {
                PropertyType = typeof(List<int>),
                IsNullable = true,
                CollectionChain = [
                    new PropertyNullableInfo.CollectionInfo
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
            PropertyName = nameof(Sample.NonNullableCollectionWithNullableValueElements),
            Expected = new PropertyNullableInfo
            {
                PropertyType = typeof(List<int?>),
                IsNullable = false,
                CollectionChain = [
                    new PropertyNullableInfo.CollectionInfo
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
            PropertyName = nameof(Sample.NonNullableCollectionWithNonNullableValueElements),
            Expected = new PropertyNullableInfo
            {
                PropertyType = typeof(List<int>),
                IsNullable = false,
                CollectionChain = [
                    new PropertyNullableInfo.CollectionInfo
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
            PropertyName = nameof(Sample.NullableCollectionWithNullableCollectionWithNullableReferenceElements),
            Expected = new PropertyNullableInfo
            {
                PropertyType = typeof(List<List<string?>?>),
                IsNullable = true,
                CollectionChain = [
                    new PropertyNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<List<string?>?>),
                        ElementType = typeof(List<string?>),
                        IsCollectionNullable = true,
                        IsElementNullable = true
                    },
                    new PropertyNullableInfo.CollectionInfo
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
            PropertyName = nameof(Sample.NullableCollectionWithNullableCollectionWithNullableCollectionWithNullableReferenceElements),
            Expected = new PropertyNullableInfo
            {
                PropertyType = typeof(List<List<List<string?>?>?>),
                IsNullable = true,
                CollectionChain = [
                    new PropertyNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<List<List<string?>?>?>),
                        ElementType = typeof(List<List<string?>?>),
                        IsCollectionNullable = true,
                        IsElementNullable = true
                    },
                    new PropertyNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<List<string?>?>),
                        ElementType = typeof(List<string?>),
                        IsCollectionNullable = true,
                        IsElementNullable = true
                    },
                    new PropertyNullableInfo.CollectionInfo
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
            PropertyName = nameof(Sample.NullableCollectionWithNullableCollectionWithNullableValueElements),
            Expected = new PropertyNullableInfo
            {
                PropertyType = typeof(List<List<int?>?>),
                IsNullable = true,
                CollectionChain = [
                    new PropertyNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<List<int?>?>),
                        ElementType = typeof(List<int?>),
                        IsCollectionNullable = true,
                        IsElementNullable = true
                    },
                    new PropertyNullableInfo.CollectionInfo
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
            PropertyName = nameof(Sample.NullableCollectionWithNullableCollectionWithNullableCollectionWithNullableValueElements),
            Expected = new PropertyNullableInfo
            {
                PropertyType = typeof(List<List<List<int?>?>?>),
                IsNullable = true,
                CollectionChain = [
                    new PropertyNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<List<List<int?>?>?>),
                        ElementType = typeof(List<List<int?>?>),
                        IsCollectionNullable = true,
                        IsElementNullable = true
                    },
                    new PropertyNullableInfo.CollectionInfo
                    {
                        CollectionType = typeof(List<List<int?>?>),
                        ElementType = typeof(List<int?>),
                        IsCollectionNullable = true,
                        IsElementNullable = true
                    },
                    new PropertyNullableInfo.CollectionInfo
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
            Name = "With Instance Property",
            Type = typeof(Widget),
            PropertyName = nameof(Widget.Property),
            Expected = false
        },
        new IsStaticTest
        {
            Name = "With Static Property",
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
