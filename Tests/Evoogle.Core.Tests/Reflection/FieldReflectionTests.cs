// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Reflection;

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
    private static readonly Dictionary<string, bool> _expectedStaticByFieldName =
        new()
        {
            [nameof(Widget.Field)] = false,
            [nameof(Widget.StaticField)] = true
        };

    public static TheoryDataRow<IXUnitTest>[] GetNullabilityInfoTestTheoryData =>
    [
        .. (
            from type in new[] { typeof(Sample), typeof(UnknownSample) }
            from fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            orderby type.Name, fieldInfo.Name
            let fieldName = fieldInfo.Name
            let expected = MemberNullabilityTestData.ExpectedByMemberName[fieldName]
            select new TheoryDataRow<IXUnitTest>
            (
                new GetNullabilityInfoTest
                {
                    Name = $"{type.Name}.{fieldName}",
                    Type = type,
                    FieldName = fieldName,
                    Expected = expected
                }
            )
        )
    ];

    public static TheoryDataRow<IXUnitTest>[] IsStaticTheoryData =>
    [
        .. (
            from fieldInfo in typeof(Widget).GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
            orderby fieldInfo.Name
            let fieldName = fieldInfo.Name
            let expected = _expectedStaticByFieldName[fieldName]
            select new TheoryDataRow<IXUnitTest>
            (
                new IsStaticTest
                {
                    Name = $"{nameof(Widget)}.{fieldName}",
                    Type = typeof(Widget),
                    FieldName = fieldName,
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
