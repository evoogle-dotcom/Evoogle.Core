// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Evoogle.XUnit;

using FluentAssertions;

namespace Evoogle.Reflection;

public class StaticReflectionTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public class GetMemberNameTest : XUnitTest
    {
        #region User Supplied Properties
        public string Expected { get; set; } = null!;
        public string Actual { get; set; } = null!;

        public static Widget Instance { get; } = new Widget();
        #endregion

        protected override void Arrange()
        {
            this.WriteLine($"Expected = {this.Expected.SafeToString()}");
            this.WriteLine($"Actual   = {this.Actual.SafeToString()}");
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
        public string Property { get; set; } = string.Empty;

        public static string StaticProperty { get; set; } = string.Empty;

        public string Method() { return string.Empty; }
        public string Method(int a) { return string.Empty; }
        public string Method(int a, int b) { return string.Empty; }

        public static string StaticMethod() { return string.Empty; }
        public static string StaticMethod(int a) { return string.Empty; }
        public static string StaticMethod(int a, int b) { return string.Empty; }

        public void VoidMethod() { }
        public void VoidMethod(int a) { }
        public void VoidMethod(int a, int b) { }

        public static void StaticVoidMethod() { }
        public static void StaticVoidMethod(int a) { }
        public static void StaticVoidMethod(int a, int b) { }

        public string Field = string.Empty;

        public static string StaticField = string.Empty;
    }
    #endregion

    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GetMemberNameTheoryData =>
    [
        new GetMemberNameTest
        {
            Name = "With Instance Field",
            Expected = nameof(Widget.Field),
            Actual = GetMemberNameTest.Instance.GetMemberName(a => a.Field)
        },

        new GetMemberNameTest
        {
            Name = "With Instance Method And Return Value And 0 Argument(s)",
            Expected = nameof(Widget.Method),
            Actual = GetMemberNameTest.Instance.GetMemberName(a => a.Method())
        },
        new GetMemberNameTest
        {
            Name = "With Instance Method And Return Value And 1 Argument(s)",
            Expected = nameof(Widget.Method),
            Actual = GetMemberNameTest.Instance.GetMemberName(a => a.Method(42))
        },
        new GetMemberNameTest
        {
            Name = "With Instance Method And Return Value And 2 Argument(s)",
            Expected = nameof(Widget.Method),
            Actual = GetMemberNameTest.Instance.GetMemberName(a => a.Method(42, 86))
        },

        new GetMemberNameTest
        {
            Name = "With Instance Method And No Return Value And 0 Argument(s)",
            Expected = nameof(Widget.VoidMethod),
            Actual = GetMemberNameTest.Instance.GetMemberName(a => a.VoidMethod())
        },
        new GetMemberNameTest
        {
            Name = "With Instance Method And No Return Value And 1 Argument(s)",
            Expected = nameof(Widget.VoidMethod),
            Actual = GetMemberNameTest.Instance.GetMemberName(a => a.VoidMethod(42))
        },
        new GetMemberNameTest
        {
            Name = "With Instance Method And No Return Value And 2 Argument(s)",
            Expected = nameof(Widget.VoidMethod),
            Actual = GetMemberNameTest.Instance.GetMemberName(a => a.VoidMethod(42, 86))
        },

        new GetMemberNameTest
        {
            Name = "With Instance Property",
            Expected = nameof(Widget.Property),
            Actual = GetMemberNameTest.Instance.GetMemberName(a => a.Property)
        },

        new GetMemberNameTest
        {
            Name = "With Static Field",
            Expected = nameof(Widget.StaticField),
            Actual = StaticReflection.GetMemberName<Widget>((a) => Widget.StaticField)
        },

        new GetMemberNameTest
        {
            Name = "With Static Method And No Return Value And 0 Argument(s)",
            Expected = nameof(Widget.StaticVoidMethod),
            Actual  = StaticReflection.GetMemberName<Widget>((a) => Widget.StaticVoidMethod())
        },
        new GetMemberNameTest
        {
            Name = "With Static Method And No Return Value And 1 Argument(s)",
            Expected = nameof(Widget.StaticVoidMethod),
            Actual  = StaticReflection.GetMemberName<Widget>((a) => Widget.StaticVoidMethod(42))
        },
        new GetMemberNameTest
        {
            Name = "With Static Method And No Return Value And 2 Argument(s)",
            Expected = nameof(Widget.StaticVoidMethod),
            Actual  = StaticReflection.GetMemberName<Widget>((a) => Widget.StaticVoidMethod(42, 86))
        },

        new GetMemberNameTest
        {
            Name = "With Static Method And Return Value And 0 Argument(s)",
            Expected = nameof(Widget.StaticMethod),
            Actual  = StaticReflection.GetMemberName<Widget>((a) => Widget.StaticMethod())
        },
        new GetMemberNameTest
        {
            Name = "With Static Method And Return Value And 1 Argument(s)",
            Expected = nameof(Widget.StaticMethod),
            Actual  = StaticReflection.GetMemberName<Widget>((a) => Widget.StaticMethod(42))
        },
        new GetMemberNameTest
        {
            Name = "With Static Method And Return Value And 2 Argument(s)",
            Expected = nameof(Widget.StaticMethod),
            Actual  = StaticReflection.GetMemberName<Widget>((a) => Widget.StaticMethod(42, 86))
        },

        new GetMemberNameTest
        {
            Name = "With Static Property",
            Expected = nameof(Widget.StaticProperty),
            Actual = StaticReflection.GetMemberName<Widget>((a) => Widget.StaticProperty)
        },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(GetMemberNameTheoryData))]
    public void GetMemberName(IXUnitTest test)
    {
        test.Execute(this);
    }
    #endregion
}