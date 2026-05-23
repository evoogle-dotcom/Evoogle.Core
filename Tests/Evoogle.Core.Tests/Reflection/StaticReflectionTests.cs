// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Linq.Expressions;

using Evoogle.Extensions;
using Evoogle.XUnit;

using FluentAssertions;

namespace Evoogle.Reflection;

public class StaticReflectionTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public class GetMemberNameTest : XUnitTest
    {
        #region User Supplied Properties
        public string Expected { get; init; } = null!;
        public string Actual { get; init; } = null!;

        public static Widget Instance { get; } = new Widget();
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            this.WriteLine($"Expected = {this.Expected.SafeToString()}");
            this.WriteLine($"Actual   = {this.Actual.SafeToString()}");
        }

        protected override void Assert() => this.Actual.Should().Be(this.Expected);
        #endregion
    }

    public class GetMemberNameEdgeCaseTest : XUnitTest
    {
        public string CaseName { get; init; } = null!;
        public string Expected { get; init; } = null!;
        public string Actual { get; private set; } = null!;

        protected override void Arrange()
        {
            this.WriteLine($"Case     = {this.CaseName}");
            this.WriteLine($"Expected = {this.Expected}");
        }

        protected override void Act()
        {
            var expression = this.CaseName switch
            {
                nameof(ConstantInt) => (Expression)Expression.Constant(42, typeof(int)),
                nameof(ConstantNull) => (Expression)Expression.Constant(null, typeof(object)),
                nameof(BinaryWithProperty) =>
                Expression.Add
                (
                    Expression.Property(Expression.Parameter(typeof(Widget), "w"), nameof(Widget.Property)),
                    Expression.Constant("test"), typeof(string).GetMethod("Concat", [typeof(string), typeof(string)])!
                ),
                nameof(UnsupportedLoop) => (Expression)Expression.Loop(Expression.Empty()),
                _ => throw new InvalidOperationException("Unknown case.")
            };

            this.Actual = StaticReflection.GetMemberName(expression, throwOnUnsupported: false);
            this.WriteLine($"Actual   = {this.Actual}");
        }

        protected override void Assert()
        {
            this.Actual.Should().Be(this.Expected);
        }

        public const string ConstantInt = "ConstantInt";
        public const string ConstantNull = "ConstantNull";
        public const string BinaryWithProperty = "BinaryWithProperty";
        public const string UnsupportedLoop = "UnsupportedLoop";
    }

    public class GetMemberPathTest : XUnitTest
    {
        public string[] Expected { get; init; } = null!;
        public string[] Actual { get; init; } = null!;

        protected override void Arrange()
        {
            this.WriteLine($"Expected = [{string.Join(", ", this.Expected)}]");
            this.WriteLine($"Actual   = [{string.Join(", ", this.Actual)}]");
        }

        protected override void Assert() => this.Actual.Should().Equal(this.Expected);
    }

    public class GetMemberPathExceptionTest : XUnitTest
    {
        public string CaseName { get; init; } = null!;
        public Type ExpectedExceptionType { get; init; } = null!;
        private Exception? _actualException;

        protected override void Arrange()
        {
            this.WriteLine($"Case                  = {this.CaseName}");
            this.WriteLine($"ExpectedExceptionType = {this.ExpectedExceptionType.Name}");
        }

        protected override void Act()
        {
            try
            {
                _ = this.CaseName switch
                {
                    nameof(NullExpression) => StaticReflection.GetMemberPath<Widget, string>(null!),
                    nameof(MethodCall) => StaticReflection.GetMemberPath<Widget, string>(x => x.Method()),
                    nameof(StaticMember) => StaticReflection.GetMemberPath<Widget, string>(_ => Widget.StaticProperty),
                    _ => throw new InvalidOperationException($"Unknown case: {this.CaseName}")
                };
            }
            catch (ArgumentException ex)
            {
                _actualException = ex;
                this.WriteLine($"Caught {ex.GetType().Name}: {ex.Message}");
            }
        }

        protected override void Assert()
        {
            _actualException.Should().NotBeNull();
            _actualException.Should().BeOfType(this.ExpectedExceptionType);
        }

        public const string NullExpression = "NullExpression";
        public const string MethodCall = "MethodCall";
        public const string StaticMember = "StaticMember";
    }
    #endregion

    #region Test Data
    public class Widget
    {
        public string Property { get; set; } = string.Empty;

        public static string StaticProperty { get; set; } = string.Empty;

        public string Method() => string.Empty;
        public string Method(int a) => string.Empty;
        public string Method(int a, int b) => string.Empty;

        public static string StaticMethod() => string.Empty;
        public static string StaticMethod(int a) => string.Empty;
        public static string StaticMethod(int a, int b) => string.Empty;

        public void VoidMethod()
        { }
        public void VoidMethod(int a)
        { }
        public void VoidMethod(int a, int b)
        { }

        public static void StaticVoidMethod()
        { }
        public static void StaticVoidMethod(int a)
        { }
        public static void StaticVoidMethod(int a, int b)
        { }

        public string Field = string.Empty;

        public static string StaticField = string.Empty;
    }

    public class Person
    {
        public int Id { get; set; }
        public Address HomeAddress { get; set; } = new();

        public class Address
        {
            public string City { get; set; } = string.Empty;
            public Contact PrimaryContact { get; set; } = new();

            public class Contact
            {
                public string Phone { get; set; } = string.Empty;
            }
        }
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

    public static TheoryDataRow<IXUnitTest>[] GetMemberNameEdgeCaseTheoryData =>
    [
        new GetMemberNameEdgeCaseTest
        {
            Name = "With Constant Expression Int",
            CaseName = GetMemberNameEdgeCaseTest.ConstantInt,
            Expected = "Constant_42"
        },
        new GetMemberNameEdgeCaseTest
        {
            Name = "With Constant Expression Null",
            CaseName = GetMemberNameEdgeCaseTest.ConstantNull,
            Expected = "Constant_null"
        },
        new GetMemberNameEdgeCaseTest
        {
            Name = "With Binary Expression and Property",
            CaseName = GetMemberNameEdgeCaseTest.BinaryWithProperty,
            Expected = "Property"
        },
        new GetMemberNameEdgeCaseTest
        {
            Name = "With Unsupported Expression",
            CaseName = GetMemberNameEdgeCaseTest.UnsupportedLoop,
            Expected = "Unsupported_LoopExpression"
        }
    ];

    public static TheoryDataRow<IXUnitTest>[] GetMemberPathTheoryData =>
    [
        new GetMemberPathTest
        {
            Name = "With Single Segment",
            Expected = ["Id"],
            Actual = StaticReflection.GetMemberPath<Person, int>(x => x.Id)
        },
        new GetMemberPathTest
        {
            Name = "With Two Segments",
            Expected = ["HomeAddress", "City"],
            Actual = StaticReflection.GetMemberPath<Person, string>(x => x.HomeAddress.City)
        },
        new GetMemberPathTest
        {
            Name = "With Three Segments",
            Expected = ["HomeAddress", "PrimaryContact", "Phone"],
            Actual = StaticReflection.GetMemberPath<Person, string>(x => x.HomeAddress.PrimaryContact.Phone)
        },
    ];

    public static TheoryDataRow<IXUnitTest>[] GetMemberPathExceptionTheoryData =>
    [
        new GetMemberPathExceptionTest
        {
            Name = "With Null Expression Throws ArgumentNullException",
            CaseName = GetMemberPathExceptionTest.NullExpression,
            ExpectedExceptionType = typeof(ArgumentNullException)
        },
        new GetMemberPathExceptionTest
        {
            Name = "With Method Call Expression Throws ArgumentException",
            CaseName = GetMemberPathExceptionTest.MethodCall,
            ExpectedExceptionType = typeof(ArgumentException)
        },
        new GetMemberPathExceptionTest
        {
            Name = "With Static Member Access Throws ArgumentException",
            CaseName = GetMemberPathExceptionTest.StaticMember,
            ExpectedExceptionType = typeof(ArgumentException)
        },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(GetMemberNameTheoryData))]
    public void GetMemberName(IXUnitTest test) => test.Execute(this);
    #endregion

    [Theory]
    [MemberData(nameof(GetMemberNameEdgeCaseTheoryData))]
    public void GetMemberNameEdgeCases(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(GetMemberPathTheoryData))]
    public void GetMemberPath(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(GetMemberPathExceptionTheoryData))]
    public void GetMemberPathExceptions(IXUnitTest test) => test.Execute(this);
}
