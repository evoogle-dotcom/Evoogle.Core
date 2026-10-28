// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
using System.Reflection;

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
        public required int? ParameterPosition { get; init; }
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
            this.WriteLine($"ParameterPosition = {this.ParameterPosition}");
            this.WriteLine();
            this.WriteLine($"Expected = {this.Expected}");
        }

        protected override void Act()
        {
            var methodInfo = this.Type.GetMethod(this.MethodName)!;
            var parameterInfo = this.ParameterPosition is int parameterPosition
                ? methodInfo.GetParameters()[parameterPosition]
                : methodInfo.ReturnParameter;

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
        .. (
            from methodInfo in typeof(Sample).GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            orderby methodInfo.Name
            let expected = MemberNullabilityTestData.ExpectedByMemberName[methodInfo.Name]
            from parameterInfo in methodInfo.GetParameters().Append(methodInfo.ReturnParameter)
            let parameterPosition = parameterInfo.Position < 0 ? (int?)null : parameterInfo.Position
            let parameterName = parameterPosition.HasValue ? $"Parameter{parameterPosition}" : "Return"
            select new TheoryDataRow<IXUnitTest>
            (
                new GetNullabilityInfoTest
                {
                    Name = $"{nameof(Sample)}.{methodInfo.Name}.{parameterName}",
                    Type = typeof(Sample),
                    MethodName = methodInfo.Name,
                    ParameterPosition = parameterPosition,
                    Expected = expected
                }
            )
        )
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(GetNullabilityInfoTheoryData))]
    public void GetNullabilityInfo(IXUnitTest test) => test.Execute(this);
    #endregion
}
