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
        public MemberNullableInfo Expected { get; init; } = null!;
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
            Name = "With Nullable Reference",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NullableReference),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(string),
                Nullability = MemberNullability.Nullable,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Non Nullable Reference",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NonNullableReference),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(string),
                Nullability = MemberNullability.NonNullable,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Nullable Value",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NullableValue),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(int?),
                Nullability = MemberNullability.Nullable,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Non Nullable Value",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NonNullableValue),
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(int),
                Nullability = MemberNullability.NonNullable,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = "With Nullable Collection With Nullable Reference Elements",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NullableCollectionWithNullableReferenceElements),
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
            Name = "With Nullable Collection With Non Nullable Reference Elements",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NullableCollectionWithNonNullableReferenceElements),
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
            Name = "With Non Nullable Collection With Nullable Reference Elements",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NonNullableCollectionWithNullableReferenceElements),
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
            Name = "With Non Nullable Collection With Non Nullable Reference Elements",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NonNullableCollectionWithNonNullableReferenceElements),
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
            Name = "With Nullable Collection With Nullable Value Elements",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NullableCollectionWithNullableValueElements),
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
            Name = "With Nullable Collection With Non Nullable Value Elements",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NullableCollectionWithNonNullableValueElements),
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
            Name = "With Non Nullable Collection With Nullable Value Elements",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NonNullableCollectionWithNullableValueElements),
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
            Name = "With Non Nullable Collection With Non Nullable Value Elements",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NonNullableCollectionWithNonNullableValueElements),
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
            Name = "With Nullable Collection With Nullable Collection With Nullable Reference Elements",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NullableCollectionWithNullableCollectionWithNullableReferenceElements),
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
            Name = "With Nullable Collection With Nullable Collection With Nullable Collection With Nullable Reference Elements",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NullableCollectionWithNullableCollectionWithNullableCollectionWithNullableReferenceElements),
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
            Name = "With Nullable Collection With Nullable Collection With Nullable Value Elements",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NullableCollectionWithNullableCollectionWithNullableValueElements),
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
            Name = "With Nullable Collection With Nullable Collection With Nullable Collection With Nullable Value Elements",
            Type = typeof(Sample),
            PropertyName = nameof(Sample.NullableCollectionWithNullableCollectionWithNullableCollectionWithNullableValueElements),
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
            Name = "With Unknown Reference",
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
            Name = "With Unknown Collection With Unknown Reference Elements",
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
