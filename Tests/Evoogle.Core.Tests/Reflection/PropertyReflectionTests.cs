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
    public class IsStaticTest : XUnitTest
    {
        public Type Type { get; init; } = null!;
        public string PropertyName { get; init; } = null!;
        public bool Expected { get; init; }

        private bool Actual { get; set; }

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
    public class Widget
    {
        public string Property { get; } = string.Empty;
        public static string StaticProperty { get; } = string.Empty;
    }
    #endregion

    #region Theory Data
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
    [MemberData(nameof(IsStaticTheoryData))]
    public void IsStatic(IXUnitTest test) => test.Execute(this);
    #endregion
}
