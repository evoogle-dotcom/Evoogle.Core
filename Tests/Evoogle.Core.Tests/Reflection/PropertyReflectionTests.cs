// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Reflection;

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
    private static readonly Dictionary<string, bool> _expectedStaticByPropertyName =
        new()
        {
            [nameof(Widget.Property)] = false,
            [nameof(Widget.StaticProperty)] = true
        };

    public static TheoryDataRow<IXUnitTest>[] GetNullabilityInfoTestTheoryData =>
    [
        .. (
            from type in new[] { typeof(Sample), typeof(UnknownSample) }
            from propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            orderby type.Name, propertyInfo.Name
            let propertyName = propertyInfo.Name
            let expected = MemberNullabilityTestData.ExpectedByMemberName[propertyName]
            select new TheoryDataRow<IXUnitTest>
            (
                new GetNullabilityInfoTest
                {
                    Name = $"{type.Name}.{propertyName}",
                    Type = type,
                    PropertyName = propertyName,
                    Expected = expected
                }
            )
        )
    ];

    public static TheoryDataRow<IXUnitTest>[] IsStaticTheoryData =>
    [
        .. (
            from propertyInfo in typeof(Widget).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
            orderby propertyInfo.Name
            let propertyName = propertyInfo.Name
            let expected = _expectedStaticByPropertyName[propertyName]
            select new TheoryDataRow<IXUnitTest>
            (
                new IsStaticTest
                {
                    Name = $"{nameof(Widget)}.{propertyName}",
                    Type = typeof(Widget),
                    PropertyName = propertyName,
                    Expected = expected
                }
            )
        )
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
