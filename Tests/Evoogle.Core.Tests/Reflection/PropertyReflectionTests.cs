// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Evoogle.XUnit;

using FluentAssertions;

namespace Evoogle.Reflection;

public class PropertyReflectionTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public class IsStaticTest : XUnitTest
    {
        public Type Type { get; set; } = null!;
        public string PropertyName { get; set; } = null!;
        public bool Expected { get; set; }

        private bool Actual { get; set; }

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

        protected override void Assert()
        {
            this.Actual.Should().Be(this.Expected);
        }
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
    public void IsStatic(IXUnitTest test)
    {
        test.Execute(this);
    }
    #endregion
}