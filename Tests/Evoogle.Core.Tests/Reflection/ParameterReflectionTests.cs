// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
using Evoogle.XUnit;

using FluentAssertions;

namespace Evoogle.Reflection;

public sealed class ParameterReflectionTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Types
    private sealed class GetNullabilityInfoTest : XUnitTest
    {
        #region User Supplied Properties
        public required Type Type { get; init; }
        public required string MethodName { get; init; }
        public required bool IsReturnParameter { get; init; }
        public required MemberNullableInfo Expected { get; init; }
        #endregion

        #region Calculated Properties
        private MemberNullableInfo? Actual { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            this.WriteLine($"Type              = {this.Type.Name}");
            this.WriteLine($"Method            = {this.MethodName}");
            this.WriteLine($"IsReturnParameter = {this.IsReturnParameter}");
            this.WriteLine();
            this.WriteLine($"Expected = {this.Expected}");
        }

        protected override void Act()
        {
            var methodInfo = this.Type.GetMethod(this.MethodName)!;
            var parameterInfo = this.IsReturnParameter
                ? methodInfo.ReturnParameter
                : methodInfo.GetParameters().Single();

            this.Actual = ParameterReflection.GetNullabilityInfo(parameterInfo);

            this.WriteLine();
            this.WriteLine($"Actual   = {this.Actual}");
        }

        protected override void Assert() => this.Actual.Should().BeEquivalentTo(this.Expected);
        #endregion
    }
    #endregion

    #region Test Data
    private static class Sample
    {
        public static string? NullableString(string? value) => value;
        public static string String(string value) => value;

        public static int? NullableInt(int? value) => value;
        public static int Int(int value) => value;

        public static List<string?>? NullableListOfNullableStrings(List<string?>? value) => value;
        public static List<string>? NullableListOfStrings(List<string>? value) => value;
        public static List<string?> ListOfNullableStrings(List<string?> value) => value;
        public static List<string> ListOfStrings(List<string> value) => value;

        public static List<int?>? NullableListOfNullableInts(List<int?>? value) => value;
        public static List<int>? NullableListOfInts(List<int>? value) => value;
        public static List<int?> ListOfNullableInts(List<int?> value) => value;
        public static List<int> ListOfInts(List<int> value) => value;

        public static List<List<string?>?>? NullableListOfNullableListsOfNullableStrings(List<List<string?>?>? value) => value;
        public static List<List<List<string?>?>?>? NullableListOfNullableListsOfNullableListsOfNullableStrings(List<List<List<string?>?>?>? value) => value;

        public static List<List<int?>?>? NullableListOfNullableListsOfNullableInts(List<List<int?>?>? value) => value;
        public static List<List<List<int?>?>?>? NullableListOfNullableListsOfNullableListsOfNullableInts(List<List<List<int?>?>?>? value) => value;
    }
    #endregion

    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GetNullabilityInfoTheoryData =>
    [
        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.NullableString)}",
            Type = typeof(Sample),
            MethodName = nameof(Sample.NullableString),
            IsReturnParameter = false,
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(string),
                Nullability = MemberNullability.Nullable,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.NullableString)}",
            Type = typeof(Sample),
            MethodName = nameof(Sample.NullableString),
            IsReturnParameter = true,
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
            MethodName = nameof(Sample.String),
            IsReturnParameter = false,
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(string),
                Nullability = MemberNullability.NonNullable,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.String)}",
            Type = typeof(Sample),
            MethodName = nameof(Sample.String),
            IsReturnParameter = true,
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
            MethodName = nameof(Sample.NullableInt),
            IsReturnParameter = false,
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(int?),
                Nullability = MemberNullability.Nullable,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.NullableInt)}",
            Type = typeof(Sample),
            MethodName = nameof(Sample.NullableInt),
            IsReturnParameter = true,
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
            MethodName = nameof(Sample.Int),
            IsReturnParameter = false,
            Expected = new MemberNullableInfo
            {
                MemberType = typeof(int),
                Nullability = MemberNullability.NonNullable,
                CollectionChain = []
            }
        },

        new GetNullabilityInfoTest
        {
            Name = $"{nameof(Sample)}.{nameof(Sample.Int)}",
            Type = typeof(Sample),
            MethodName = nameof(Sample.Int),
            IsReturnParameter = true,
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
            MethodName = nameof(Sample.NullableListOfNullableStrings),
            IsReturnParameter = false,
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
            Name = $"{nameof(Sample)}.{nameof(Sample.NullableListOfNullableStrings)}",
            Type = typeof(Sample),
            MethodName = nameof(Sample.NullableListOfNullableStrings),
            IsReturnParameter = true,
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
            MethodName = nameof(Sample.NullableListOfStrings),
            IsReturnParameter = false,
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
            Name = $"{nameof(Sample)}.{nameof(Sample.NullableListOfStrings)}",
            Type = typeof(Sample),
            MethodName = nameof(Sample.NullableListOfStrings),
            IsReturnParameter = true,
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
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(GetNullabilityInfoTheoryData))]
    public void GetNullabilityInfo(IXUnitTest test) => test.Execute(this);
    #endregion
}
