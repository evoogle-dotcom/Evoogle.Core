// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Data;

using Evoogle.XUnit;

using FluentAssertions;

using Xunit.Abstractions;

namespace Evoogle;

public class DotNetTypeExtensionTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public class GetBaseTypeTest : XUnitTest
    {
        public Type Type { get; set; } = null!;
        public Type? ExpectedBaseType { get; set; }

        public string ExpectedBaseTypeName { get; set; } = null!;

        public Type? ActualBaseType { get; set; }
        public string ActualBaseTypeName { get; set; } = null!;

        protected override void Arrange()
        {
            this.WriteLine($"Type = {this.Type.Name}");
            this.WriteLine();

            this.ExpectedBaseTypeName = this.ExpectedBaseType != null ? this.ExpectedBaseType.Name : "null";
            this.WriteLine($"Expected Base Type = {this.ExpectedBaseTypeName}");
        }

        protected override void Act()
        {
            this.ActualBaseType = this.Type.GetBaseType();

            this.ActualBaseTypeName = this.ActualBaseType != null ? this.ActualBaseType.Name : "null";
            this.WriteLine($"Actual Base Type   = {this.ActualBaseTypeName}");
        }

        protected override void Assert()
        {
            this.ActualBaseTypeName.Should().Be(this.ExpectedBaseTypeName);
        }
    }

    public class GetBaseTypesTest : XUnitTest
    {
        public Type Type { get; set; } = null!;
        public IEnumerable<Type> ExpectedBaseTypes { get; set; } = null!;

        public string ExpectedBaseTypeNames { get; set; } = null!;

        public IEnumerable<Type> ActualBaseTypes { get; set; } = null!;
        public string ActualBaseTypeNames { get; set; } = null!;

        protected override void Arrange()
        {
            this.WriteLine($"Type = {this.Type.Name}");
            this.WriteLine();

            this.ExpectedBaseTypeNames = this.ExpectedBaseTypes.EmptyIfNull().Select(x => x.Name).SafeToDelimitedString(',');
            this.WriteLine($"Expected Base Types    = {this.ExpectedBaseTypeNames}");
        }

        protected override void Act()
        {
            this.ActualBaseTypes = this.Type.GetBaseTypes();

            this.ActualBaseTypeNames = this.ActualBaseTypes.EmptyIfNull().Select(x => x.Name).SafeToDelimitedString(',');
            this.WriteLine($"Actual Base Types      = {this.ActualBaseTypeNames}");
        }

        protected override void Assert()
        {
            this.ActualBaseTypeNames.Should().Be(this.ExpectedBaseTypeNames);
        }
    }

    public class GetConstructorTest : XUnitTest
    {
        public Type Type { get; set; } = null!;
        public TypeReflectionFlags ReflectionFlags { get; set; }
        public IEnumerable<Type> ParameterTypes { get; set; } = null!;
        public bool ExpectedConstructorFound { get; set; }

        public bool ActualConstructorFound { get; set; }

        protected override void Arrange()
        {
            var parameterTypes = this.ParameterTypes.EmptyIfNull().Select(x => x.Name).SafeToDelimitedString(',');

            this.WriteLine($"Type               = {this.Type.Name}");
            this.WriteLine($"Reflection Flags   = {this.ReflectionFlags}");
            this.WriteLine($"Parameter Types    = {parameterTypes}");
            this.WriteLine();
            this.WriteLine($"Expected Constructor Found = {this.ExpectedConstructorFound}");
        }

        protected override void Act()
        {
            var constructorInfo = this.Type.GetConstructor(this.ReflectionFlags, this.ParameterTypes);

            this.ActualConstructorFound = constructorInfo != null;
            this.WriteLine($"Actual Constructor Found   = {this.ActualConstructorFound}");
        }

        protected override void Assert()
        {
            this.ActualConstructorFound.Should().Be(this.ExpectedConstructorFound);
        }
    }

    public class GetConstructorsTest : XUnitTest
    {
        public Type Type { get; set; } = null!;
        public TypeReflectionFlags ReflectionFlags { get; set; }
        public long ExpectedConstructorCount { get; set; }

        public long ActualConstructorCount { get; set; }

        protected override void Arrange()
        {
            this.WriteLine($"Type               = {this.Type.Name}");
            this.WriteLine($"Reflection Flags   = {this.ReflectionFlags}");
            this.WriteLine();
            this.WriteLine($"Expected Constructor Count = {this.ExpectedConstructorCount}");
        }

        protected override void Act()
        {
            var actualConstructors = this.Type.GetConstructors(this.ReflectionFlags);

            this.ActualConstructorCount = actualConstructors.EmptyIfNull().Count();
            this.WriteLine($"Actual Constructor Count   = {this.ActualConstructorCount}");
        }

        protected override void Assert()
        {
            this.ActualConstructorCount.Should().Be(this.ExpectedConstructorCount);
        }
    }

    public class GetFieldTest : XUnitTest
    {
        public Type Type { get; set; } = null!;
        public TypeReflectionFlags ReflectionFlags { get; set; }
        public string FieldName { get; set; } = null!;
        public string? ExpectedFieldName { get; set; }

        public string? ActualFieldName { get; set; }

        protected override void Arrange()
        {
            this.WriteLine($"Type               = {this.Type.Name}");
            this.WriteLine($"Reflection Flags   = {this.ReflectionFlags}");
            this.WriteLine($"Field Name         = {this.FieldName}");
            this.WriteLine();
            this.WriteLine($"Expected Field Name    = {this.ExpectedFieldName.SafeToString()}");
        }

        protected override void Act()
        {
            var fieldInfo = this.Type.GetField(this.FieldName, this.ReflectionFlags);

            this.ActualFieldName = fieldInfo?.Name;
            this.WriteLine($"Actual Field Name      = {this.ActualFieldName.SafeToString()}");
        }

        protected override void Assert()
        {
            if (this.ExpectedFieldName == null)
            {
                this.ActualFieldName.Should().BeNull();
                return;
            }

            this.ActualFieldName.Should().Be(this.ExpectedFieldName);
        }
    }

    public class GetFieldsTest : XUnitTest
    {
        public Type Type { get; set; } = null!;
        public TypeReflectionFlags ReflectionFlags { get; set; }
        public IEnumerable<string> ExpectedFieldNames { get; set; } = null!;

        public IEnumerable<string> ActualFieldNames { get; set; } = null!;

        protected override void Arrange()
        {
            this.WriteLine($"Type               = {this.Type.Name}");
            this.WriteLine($"Reflection Flags   = {this.ReflectionFlags}");
            this.WriteLine();
            this.WriteLine($"Expected Field Names   = {this.ExpectedFieldNames.Order().SafeToDelimitedString(',')}");
        }

        protected override void Act()
        {
            this.ActualFieldNames = this.Type.GetFields(this.ReflectionFlags).EmptyIfNull().Select(x => x.Name);
            this.WriteLine($"Actual Field Names     = {this.ActualFieldNames.Order().SafeToDelimitedString(',')}");
        }

        protected override void Assert()
        {
            this.ActualFieldNames.Order().Should().BeEquivalentTo(this.ExpectedFieldNames.Order());
        }
    }

    public class GetMethodTest : XUnitTest
    {
        public Type Type { get; set; } = null!;
        public TypeReflectionFlags ReflectionFlags { get; set; }
        public IEnumerable<Type> ParameterTypes { get; set; } = null!;
        public string MethodName { get; set; } = null!;
        public string? ExpectedMethodName { get; set; }

        public string? ActualMethodName { get; set; }

        protected override void Arrange()
        {
            var parameterTypes = this.ParameterTypes.EmptyIfNull().Select(x => x.Name).SafeToDelimitedString(',');

            this.WriteLine($"Type               = {this.Type.Name}");
            this.WriteLine($"Reflection Flags   = {this.ReflectionFlags}");
            this.WriteLine($"Parameter Types    = {parameterTypes}");
            this.WriteLine($"Method Name        = {this.MethodName}");
            this.WriteLine();
            this.WriteLine($"Expected Method Name   = {this.ExpectedMethodName.SafeToString()}");
        }

        protected override void Act()
        {
            var methodInfo = this.Type.GetMethod(this.MethodName, this.ReflectionFlags, this.ParameterTypes);

            this.ActualMethodName = methodInfo?.Name;
            this.WriteLine($"Actual Method Found    = {this.ActualMethodName.SafeToString()}");
        }

        protected override void Assert()
        {
            if (this.ExpectedMethodName == null)
            {
                this.ActualMethodName.Should().BeNull();
                return;
            }

            this.ActualMethodName.Should().Be(this.ExpectedMethodName);
        }
    }

    public class GetMethodsTest : XUnitTest
    {
        public Type Type { get; set; } = null!;
        public TypeReflectionFlags ReflectionFlags { get; set; }
        public IEnumerable<string> ExpectedMethodNames { get; set; } = null!;

        public IEnumerable<string> ActualMethodNames { get; set; } = null!;

        protected override void Arrange()
        {
            this.WriteLine($"Type               = {this.Type.Name}");
            this.WriteLine($"Reflection Flags   = {this.ReflectionFlags}");
            this.WriteLine();
            this.WriteLine($"Expected Method Names   = {this.ExpectedMethodNames.Order().SafeToDelimitedString(',')}");
        }

        protected override void Act()
        {
            this.ActualMethodNames = this.Type.GetMethods(this.ReflectionFlags).EmptyIfNull().Select(x => x.Name);
            this.WriteLine($"Actual Method Names     = {this.ActualMethodNames.Order().SafeToDelimitedString(',')}");
        }

        protected override void Assert()
        {
            // Ignore the inherited methods from Object.
            // So assert actual at least contains all of expected but may have more.
            this.ActualMethodNames.Order().Should().Contain(this.ExpectedMethodNames.Order());
        }
    }

    public class GetPropertyTest : XUnitTest
    {
        public Type Type { get; set; } = null!;
        public TypeReflectionFlags ReflectionFlags { get; set; }
        public string PropertyName { get; set; } = null!;
        public string? ExpectedPropertyName { get; set; }

        public string? ActualPropertyName { get; set; }

        protected override void Arrange()
        {
            this.WriteLine($"Type               = {this.Type.Name}");
            this.WriteLine($"Reflection Flags   = {this.ReflectionFlags}");
            this.WriteLine($"Property Name      = {this.PropertyName}");
            this.WriteLine();
            this.WriteLine($"Expected Property Name = {this.ExpectedPropertyName.SafeToString()}");
        }

        protected override void Act()
        {
            var PropertyInfo = this.Type.GetProperty(this.PropertyName, this.ReflectionFlags);

            this.ActualPropertyName = PropertyInfo?.Name;
            this.WriteLine($"Actual Property Name   = {this.ActualPropertyName.SafeToString()}");
        }

        protected override void Assert()
        {
            if (this.ExpectedPropertyName == null)
            {
                this.ActualPropertyName.Should().BeNull();
                return;
            }

            this.ActualPropertyName.Should().Be(this.ExpectedPropertyName);
        }
    }

    public class GetPropertiesTest : XUnitTest
    {
        public Type Type { get; set; } = null!;
        public TypeReflectionFlags ReflectionFlags { get; set; }
        public IEnumerable<string> ExpectedPropertyNames { get; set; } = null!;

        public IEnumerable<string> ActualPropertyNames { get; set; } = null!;

        protected override void Arrange()
        {
            this.WriteLine($"Type               = {this.Type.Name}");
            this.WriteLine($"Reflection Flags   = {this.ReflectionFlags}");
            this.WriteLine();
            this.WriteLine($"Expected Property Names    = {this.ExpectedPropertyNames.Order().SafeToDelimitedString(',')}");
        }

        protected override void Act()
        {
            this.ActualPropertyNames = this.Type.GetProperties(this.ReflectionFlags).EmptyIfNull().Select(x => x.Name);
            this.WriteLine($"Actual Property Names      = {this.ActualPropertyNames.Order().SafeToDelimitedString(',')}");
        }

        protected override void Assert()
        {
            this.ActualPropertyNames.Order().Should().BeEquivalentTo(this.ExpectedPropertyNames.Order());
        }
    }

    public class IsComplexTest : XUnitTest
    {
        public Type Type { get; set; } = null!;
        public bool Expected { get; set; }

        public bool Actual { get; set; }

        protected override void Arrange()
        {
            this.WriteLine($"Type = {this.Type.Name}");
            this.WriteLine();
            this.WriteLine($"Expected   = {this.Expected}");
        }

        protected override void Act()
        {
            this.Actual = this.Type.IsComplex();

            this.WriteLine($"Actual     = {this.Actual}");
        }

        protected override void Assert()
        {
            this.Actual.Should().Be(this.Expected);
        }
    }

    public class IsEnumerableOfTTest : XUnitTest
    {
        public Type Type { get; set; } = null!;
        public bool Expected { get; set; }

        public bool Actual { get; set; }

        protected override void Arrange()
        {
            this.WriteLine($"Type = {this.Type.Name}");
            this.WriteLine();
            this.WriteLine($"Expected   = {this.Expected}");
        }

        protected override void Act()
        {
            this.Actual = this.Type.IsEnumerableOfT();

            this.WriteLine($"Actual     = {this.Actual}");
        }

        protected override void Assert()
        {
            this.Actual.Should().Be(this.Expected);
        }
    }

    public class IsImplementationOfTest : XUnitTest
    {
        public Type DerivedType { get; set; } = null!;
        public Type BaseType { get; set; } = null!;
        public bool Expected { get; set; }

        public bool Actual { get; set; }

        protected override void Arrange()
        {
            this.WriteLine($"Derived Type   = {this.DerivedType.Name}");
            this.WriteLine($"Base Type      = {this.BaseType.Name}");
            this.WriteLine();
            this.WriteLine($"Expected   = {this.Expected}");
        }

        protected override void Act()
        {
            this.Actual = this.DerivedType.IsImplementationOf(this.BaseType);

            this.WriteLine($"Actual     = {this.Actual}");
        }

        protected override void Assert()
        {
            this.Actual.Should().Be(this.Expected);
        }
    }

    public class IsSimpleTest : XUnitTest
    {
        public Type Type { get; set; } = null!;
        public bool Expected { get; set; }

        public bool Actual { get; set; }

        protected override void Arrange()
        {
            this.WriteLine($"Type = {this.Type.Name}");
            this.WriteLine();
            this.WriteLine($"Expected   = {this.Expected}");
        }

        protected override void Act()
        {
            this.Actual = this.Type.IsSimple();

            this.WriteLine($"Actual     = {this.Actual}");
        }

        protected override void Assert()
        {
            this.Actual.Should().Be(this.Expected);
        }
    }

    public class IsSubclassOrImplementationOfTest : XUnitTest
    {
        public Type DerivedType { get; set; } = null!;
        public Type BaseType { get; set; } = null!;
        public bool Expected { get; set; }

        public bool Actual { get; set; }

        protected override void Arrange()
        {
            this.WriteLine($"Derived Type   = {this.DerivedType.Name}");
            this.WriteLine($"Base Type      = {this.BaseType.Name}");
            this.WriteLine();
            this.WriteLine($"Expected   = {this.Expected}");
        }

        protected override void Act()
        {
            this.Actual = this.DerivedType.IsSubclassOrImplementationOf(this.BaseType);

            this.WriteLine($"Actual     = {this.Actual}");
        }

        protected override void Assert()
        {
            this.Actual.Should().Be(this.Expected);
        }
    }
    #endregion

    #region Test Types
    private enum PrimaryColor
    {
        Red,
        Green,
        Blue
    };

    private interface IAnimal
    {
        void Move();
    }

    private interface IShape
    {
        void Draw();
    }

    private abstract class AbstractAnimal : IAnimal
    {
        public abstract void Move();
    }

    private abstract class AbstractShape : IShape
    {
        public abstract void Draw();
    }

    private class Rectangle : AbstractShape
    {
        public override void Draw()
        {
        }
    }

    private class Square : Rectangle
    {
        public override void Draw()
        {
        }
    }

    private interface ICollection<out T>
    {
        T[] Items { get; }
    }

    private interface IHost<out T>
    {
        T HostedObject { get; }
    }

    private abstract class AbstractCollection<T> : ICollection<T>
    {
        public abstract T[] Items { get; }
    }

    private class Collection<T> : AbstractCollection<T>
    {
        public override T[] Items { get; } = null!;
    }

    private class DecoratedCollection<T> : Collection<T>
    {
        public int Count { get; }
    }

    private abstract class AbstractHost<T> : IHost<T>
    {
        public abstract T HostedObject { get; }
    }

    private class Host<T>(T hostedObject) : AbstractHost<T>
    {
        public override T HostedObject { get; } = hostedObject;
    }

#pragma warning disable CA1822, CS0169, CS0649, IDE0044, IDE0051, IDE0060, RCS1163, RCS1169, RCS1170, RCS1213
    private class ClassWithMethodsBase
    {
        public string PublicMethodBase()
        {
            return string.Empty;
        }

        public string PublicMethodBase(int x)
        {
            return string.Empty;
        }

        public string PublicMethodBase(int x, string y)
        {
            return string.Empty;
        }

        public static string PublicStaticMethodBase()
        {
            return string.Empty;
        }

        public static string PublicStaticMethodBase(int x)
        {
            return string.Empty;
        }

        public static string PublicStaticMethodBase(int x, string y)
        {
            return string.Empty;
        }

        protected string ProtectedMethodBase()
        {
            return string.Empty;
        }

        protected string ProtectedMethodBase(int x)
        {
            return string.Empty;
        }

        protected string ProtectedMethodBase(int x, string y)
        {
            return string.Empty;
        }

        protected static string ProtectedStaticMethodBase()
        {
            return string.Empty;
        }

        protected static string ProtectedStaticMethodBase(int x)
        {
            return string.Empty;
        }

        protected static string ProtectedStaticMethodBase(int x, string y)
        {
            return string.Empty;
        }
    }

    private class ClassWithConstructors
    {
        public ClassWithConstructors()
        {
        }

        public ClassWithConstructors(int x)
        {
        }

        public ClassWithConstructors(int x, string y)
        {
        }

        protected ClassWithConstructors(int x, string y, bool z)
        {
        }
    }

    private class ClassWithMethods : ClassWithMethodsBase
    {
        public string PublicMethod()
        {
            return string.Empty;
        }

        public string PublicMethod(int x)
        {
            return string.Empty;
        }

        public string PublicMethod(int x, string y)
        {
            return string.Empty;
        }

        public static string PublicStaticMethod()
        {
            return string.Empty;
        }

        public static string PublicStaticMethod(int x)
        {
            return string.Empty;
        }

        public static string PublicStaticMethod(int x, string y)
        {
            return string.Empty;
        }

        private string PrivateMethod()
        {
            return string.Empty;
        }

        private string PrivateMethod(int x)
        {
            return string.Empty;
        }

        private string PrivateMethod(int x, string y)
        {
            return string.Empty;
        }

        private static string PrivateStaticMethod()
        {
            return string.Empty;
        }

        private static string PrivateStaticMethod(int x)
        {
            return string.Empty;
        }

        private static string PrivateStaticMethod(int x, string y)
        {
            return string.Empty;
        }
    }

    private class ClassWithPropertiesBase
    {
        public string? PublicPropertyBase { get; set; }
        public static string? PublicStaticPropertyBase { get; set; }

        protected internal string? ProtectedInternalPropertyBase { get; set; }
        protected internal static string? ProtectedInternalStaticPropertyBase { get; set; }

        protected string? ProtectedPropertyBase { get; set; }
        protected static string? ProtectedStaticPropertyBase { get; set; }

        internal string? InternalPropertyBase { get; set; }
        internal static string? InternalStaticPropertyBase { get; set; }

        private string? PrivatePropertyBase { get; set; }
        private static string? PrivateStaticPropertyBase { get; set; }
    }

    private class ClassWithProperties : ClassWithPropertiesBase
    {
        public string? PublicProperty { get; set; }
        public static string? PublicStaticProperty { get; set; }

        protected internal string? ProtectedInternalProperty { get; set; }
        protected internal static string? ProtectedInternalStaticProperty { get; set; }

        protected string? ProtectedProperty { get; set; }
        protected static string? ProtectedStaticProperty { get; set; }

        internal string? InternalProperty { get; set; }
        internal static string? InternalStaticProperty { get; set; }

        private string? PrivateProperty { get; set; }
        private static string? PrivateStaticProperty { get; set; }
    }

    private class ClassWithFieldsBase
    {
        public string? PublicFieldBase;
        public static string? PublicStaticFieldBase;

        protected internal string? ProtectedInternalFieldBase;
        protected internal static string? ProtectedInternalStaticFieldBase;

        protected string? ProtectedFieldBase;
        protected static string? ProtectedStaticFieldBase;

        internal string? InternalFieldBase;
        internal static string? InternalStaticFieldBase;

        private string? PrivateFieldBase;

        private static string? PrivateStaticFieldBase;
    }

    private class ClassWithFields : ClassWithFieldsBase
    {
        public string? PublicField;
        public static string? PublicStaticField;

        protected internal string? ProtectedInternalField;
        protected internal static string? ProtectedInternalStaticField;

        protected string? ProtectedField;
        protected static string? ProtectedStaticField;

        internal string? InternalField;
        internal static string? InternalStaticField;

        private string? PrivateField;

        private static string? PrivateStaticField;
    }
    #endregion

    #region Theory Data
    public static TheoryData<IXUnitTest> GetBaseTypeTheoryData => new()
    {
        { new GetBaseTypeTest { Name = "With Interface Type", Type = typeof(IShape), ExpectedBaseType = null } },
        { new GetBaseTypeTest { Name = "With Type", Type = typeof(AbstractShape), ExpectedBaseType = typeof(object) } },
        { new GetBaseTypeTest { Name = "With Type And Base Type", Type = typeof(Rectangle), ExpectedBaseType = typeof(AbstractShape) } },
        { new GetBaseTypeTest { Name = "With Type And Base Type And Base Type", Type = typeof(Square), ExpectedBaseType = typeof(Rectangle)} },

        { new GetBaseTypeTest { Name = "With Generic Interface Type", Type = typeof(ICollection<>), ExpectedBaseType = null } },
        { new GetBaseTypeTest { Name = "With Generic Type", Type = typeof(AbstractCollection<>), ExpectedBaseType = typeof(object) } },
        { new GetBaseTypeTest { Name = "With Generic Type And Generic Base Type", Type = typeof(Collection<>), ExpectedBaseType = typeof(AbstractCollection<>) } },
        { new GetBaseTypeTest { Name = "With Generic Type And Generic Base Type And Generic Base Type", Type = typeof(DecoratedCollection<>), ExpectedBaseType = typeof(Collection<>) } },
    };

    public static TheoryData<IXUnitTest> GetBaseTypesTheoryData => new()
    {
        { new GetBaseTypesTest { Name = "With Interface Type", Type = typeof(IShape), ExpectedBaseTypes = [] } },
        { new GetBaseTypesTest { Name = "With Type", Type = typeof(AbstractShape), ExpectedBaseTypes = [typeof(object)] } },
        { new GetBaseTypesTest { Name = "With Type And Base Type", Type = typeof(Rectangle), ExpectedBaseTypes = [typeof(AbstractShape), typeof(object)] } },
        { new GetBaseTypesTest { Name = "With Type And Base Type And Base Type", Type = typeof(Square), ExpectedBaseTypes = [typeof(Rectangle), typeof(AbstractShape), typeof(object)] } },

        { new GetBaseTypesTest { Name = "With Generic Interface Type", Type = typeof(ICollection<>), ExpectedBaseTypes = [] } },
        { new GetBaseTypesTest { Name = "With Generic Type", Type = typeof(AbstractCollection<>), ExpectedBaseTypes = [typeof(object)] } },
        { new GetBaseTypesTest { Name = "With Generic Type And Generic Base Type", Type = typeof(Collection<>), ExpectedBaseTypes = [typeof(AbstractCollection<>), typeof(object)] } },
        { new GetBaseTypesTest { Name = "With Generic Type And Generic Base Type And Generic Base Type", Type = typeof(DecoratedCollection<>), ExpectedBaseTypes = [typeof(Collection<>), typeof(AbstractCollection<>), typeof(object)] } },
    };

    public static TheoryData<IXUnitTest> GetConstructorTheoryData => new()
    {
        { new GetConstructorTest { Name = "With Public And 0 Parameters", Type = typeof(ClassWithConstructors), ReflectionFlags = TypeReflectionFlags.Public, ParameterTypes = [], ExpectedConstructorFound = true } },
        { new GetConstructorTest { Name = "With Public And 1 Parameters", Type = typeof(ClassWithConstructors), ReflectionFlags = TypeReflectionFlags.Public, ParameterTypes = [typeof(int)], ExpectedConstructorFound = true } },
        { new GetConstructorTest { Name = "With Public And 2 Parameters", Type = typeof(ClassWithConstructors), ReflectionFlags = TypeReflectionFlags.Public, ParameterTypes = [typeof(int), typeof(string)], ExpectedConstructorFound = true } },
        { new GetConstructorTest { Name = "With Public And 3 Parameters", Type = typeof(ClassWithConstructors), ReflectionFlags = TypeReflectionFlags.Public, ParameterTypes = [typeof(int), typeof(string), typeof(bool)], ExpectedConstructorFound = false } },

        { new GetConstructorTest { Name = "With Public And Non Public And 0 Parameters", Type = typeof(ClassWithConstructors), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.NonPublic, ParameterTypes = [], ExpectedConstructorFound = true } },
        { new GetConstructorTest { Name = "With Public And Non Public And 1 Parameters", Type = typeof(ClassWithConstructors), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.NonPublic, ParameterTypes = [typeof(int)], ExpectedConstructorFound = true } },
        { new GetConstructorTest { Name = "With Public And Non Public And 2 Parameters", Type = typeof(ClassWithConstructors), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.NonPublic, ParameterTypes = [typeof(int), typeof(string)], ExpectedConstructorFound = true } },
        { new GetConstructorTest { Name = "With Public And Non Public And 3 Parameters", Type = typeof(ClassWithConstructors), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.NonPublic, ParameterTypes = [typeof(int), typeof(string), typeof(bool)], ExpectedConstructorFound = true } },

        { new GetConstructorTest { Name = "With Non Public And 0 Parameters", Type = typeof(ClassWithConstructors), ReflectionFlags = TypeReflectionFlags.NonPublic, ParameterTypes = [], ExpectedConstructorFound = false } },
        { new GetConstructorTest { Name = "With Non Public And 1 Parameters", Type = typeof(ClassWithConstructors), ReflectionFlags = TypeReflectionFlags.NonPublic, ParameterTypes = [typeof(int)], ExpectedConstructorFound = false } },
        { new GetConstructorTest { Name = "With Non Public And 2 Parameters", Type = typeof(ClassWithConstructors), ReflectionFlags = TypeReflectionFlags.NonPublic, ParameterTypes = [typeof(int), typeof(string)], ExpectedConstructorFound = false } },
        { new GetConstructorTest { Name = "With Non Public And 3 Parameters", Type = typeof(ClassWithConstructors), ReflectionFlags = TypeReflectionFlags.NonPublic, ParameterTypes = [typeof(int), typeof(string), typeof(bool)], ExpectedConstructorFound = true } },
    };

    public static TheoryData<IXUnitTest> GetConstructorsTheoryData => new()
    {
        { new GetConstructorsTest { Name = "With Public", Type = typeof(ClassWithConstructors), ReflectionFlags = TypeReflectionFlags.Public, ExpectedConstructorCount = 3 } },
        { new GetConstructorsTest { Name = "With Public And Non Public", Type = typeof(ClassWithConstructors), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.NonPublic, ExpectedConstructorCount = 4 } },
        { new GetConstructorsTest { Name = "With Non Public", Type = typeof(ClassWithConstructors), ReflectionFlags = TypeReflectionFlags.NonPublic, ExpectedConstructorCount = 1 } },
    };

    public static TheoryData<IXUnitTest> GetFieldTheoryData => new()
    {
        { new GetFieldTest { Name = "With Declared Only And Public And Instance", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Instance, FieldName = "PublicField", ExpectedFieldName = "PublicField" } },
        { new GetFieldTest { Name = "With Declared Only And Public And Static", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Static, FieldName = "PublicStaticField", ExpectedFieldName = "PublicStaticField" } },
        { new GetFieldTest { Name = "With Declared Only And Non Public And Instance", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance, FieldName = "PrivateField", ExpectedFieldName = "PrivateField" } },
        { new GetFieldTest { Name = "With Declared Only And Non Public And Static", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Static, FieldName = "PrivateStaticField", ExpectedFieldName = "PrivateStaticField" } },
        { new GetFieldTest { Name = "With Public And Instance", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Instance, FieldName = "PublicFieldBase", ExpectedFieldName = "PublicFieldBase" } },
        { new GetFieldTest { Name = "With Public And Static", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Static, FieldName = "PublicStaticFieldBase", ExpectedFieldName = "PublicStaticFieldBase" } },
        { new GetFieldTest { Name = "With Non Public And Instance", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance, FieldName = "PrivateFieldBase", ExpectedFieldName = "PrivateFieldBase" } },
        { new GetFieldTest { Name = "With Non Public And Static", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Static, FieldName = "PrivateStaticFieldBase", ExpectedFieldName = "PrivateStaticFieldBase" } },
    };

    public static TheoryData<IXUnitTest> GetFieldsTheoryData => new()
    {
        { new GetFieldsTest { Name = "With Declared Only And Public And Instance", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Instance, ExpectedFieldNames = ["PublicField"] } },
        { new GetFieldsTest { Name = "With Declared Only And Public And Static", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Static, ExpectedFieldNames = ["PublicStaticField"] } },
        { new GetFieldsTest { Name = "With Declared Only And Public And Instance And Static", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Instance | TypeReflectionFlags.Static, ExpectedFieldNames = ["PublicField", "PublicStaticField"] } },
        { new GetFieldsTest { Name = "With Declared Only And Non Public And Instance", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance, ExpectedFieldNames = ["ProtectedInternalField", "ProtectedField", "InternalField", "PrivateField"] } },
        { new GetFieldsTest { Name = "With Declared Only And Non Public And Static", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Static, ExpectedFieldNames = ["ProtectedInternalStaticField", "ProtectedStaticField", "InternalStaticField", "PrivateStaticField"] } },
        { new GetFieldsTest { Name = "With Declared Only And Non Public And Instance And Static", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance | TypeReflectionFlags.Static, ExpectedFieldNames = ["ProtectedInternalField", "ProtectedInternalStaticField", "ProtectedField", "ProtectedStaticField", "InternalField", "InternalStaticField", "PrivateField", "PrivateStaticField"] } },
        { new GetFieldsTest { Name = "With Declared Only And Public And Non Public And Instance And Static", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance | TypeReflectionFlags.Static, ExpectedFieldNames = ["PublicField", "PublicStaticField", "ProtectedInternalField", "ProtectedInternalStaticField", "ProtectedField", "ProtectedStaticField", "InternalField", "InternalStaticField", "PrivateField", "PrivateStaticField"] } },
        { new GetFieldsTest { Name = "With Public And Instance", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Instance, ExpectedFieldNames = ["PublicField", "PublicFieldBase"] } },
        { new GetFieldsTest { Name = "With Public And Static", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Static, ExpectedFieldNames = ["PublicStaticField", "PublicStaticFieldBase"] } },
        { new GetFieldsTest { Name = "With Public And Instance And Static", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Instance | TypeReflectionFlags.Static, ExpectedFieldNames = ["PublicField", "PublicFieldBase", "PublicStaticField", "PublicStaticFieldBase"] } },
        { new GetFieldsTest { Name = "With Non Public And Instance", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance, ExpectedFieldNames = ["ProtectedInternalField", "ProtectedField", "InternalField", "PrivateField", "ProtectedInternalFieldBase", "ProtectedFieldBase", "InternalFieldBase", "PrivateFieldBase"] } },
        { new GetFieldsTest { Name = "With Non Public And Static", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Static, ExpectedFieldNames = ["ProtectedInternalStaticField", "ProtectedStaticField", "InternalStaticField", "PrivateStaticField", "ProtectedInternalStaticFieldBase", "ProtectedStaticFieldBase", "InternalStaticFieldBase", "PrivateStaticFieldBase"] } },
        { new GetFieldsTest { Name = "With Non Public And Instance And Static", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance | TypeReflectionFlags.Static, ExpectedFieldNames = ["ProtectedInternalField", "ProtectedInternalStaticField", "ProtectedField", "ProtectedStaticField", "InternalField", "InternalStaticField", "PrivateField", "PrivateStaticField", "ProtectedInternalFieldBase", "ProtectedInternalStaticFieldBase", "ProtectedFieldBase", "ProtectedStaticFieldBase", "InternalFieldBase", "InternalStaticFieldBase", "PrivateFieldBase", "PrivateStaticFieldBase"] } },
        { new GetFieldsTest { Name = "With Public And Non Public And Instance And Static", Type = typeof(ClassWithFields), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance | TypeReflectionFlags.Static, ExpectedFieldNames = ["PublicField", "PublicStaticField", "ProtectedInternalField", "ProtectedInternalStaticField", "ProtectedField", "ProtectedStaticField", "InternalField", "InternalStaticField", "PrivateField", "PrivateStaticField", "PublicFieldBase", "PublicStaticFieldBase", "ProtectedInternalFieldBase", "ProtectedInternalStaticFieldBase", "ProtectedFieldBase", "ProtectedStaticFieldBase", "InternalFieldBase", "InternalStaticFieldBase", "PrivateFieldBase", "PrivateStaticFieldBase"] } },
    };

    public static TheoryData<IXUnitTest> GetMethodTheoryData => new()
    {
        { new GetMethodTest { Name = "With Declared Only And Public And Instance And 0 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Instance, ParameterTypes = [], MethodName = "PublicMethod", ExpectedMethodName = "PublicMethod" } },
        { new GetMethodTest { Name = "With Declared Only And Public And Instance And 1 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Instance, ParameterTypes = [typeof(int)], MethodName = "PublicMethod", ExpectedMethodName = "PublicMethod" } },
        { new GetMethodTest { Name = "With Declared Only And Public And Instance And 2 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Instance, ParameterTypes = [typeof(int), typeof(string)], MethodName = "PublicMethod", ExpectedMethodName = "PublicMethod" } },
        { new GetMethodTest { Name = "With Declared Only And Public And Instance And 3 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Instance, ParameterTypes = [typeof(int), typeof(string), typeof(bool)], MethodName = "PublicMethod", ExpectedMethodName = null } },

        { new GetMethodTest { Name = "With Declared Only And Public And Static And 0 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Static, ParameterTypes = [], MethodName = "PublicStaticMethod", ExpectedMethodName = "PublicStaticMethod" } },
        { new GetMethodTest { Name = "With Declared Only And Public And Static And 1 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Static, ParameterTypes = [typeof(int)], MethodName = "PublicStaticMethod", ExpectedMethodName = "PublicStaticMethod" } },
        { new GetMethodTest { Name = "With Declared Only And Public And Static And 2 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Static, ParameterTypes = [typeof(int), typeof(string)], MethodName = "PublicStaticMethod", ExpectedMethodName = "PublicStaticMethod" } },
        { new GetMethodTest { Name = "With Declared Only And Public And Static And 3 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Static, ParameterTypes = [typeof(int), typeof(string), typeof(bool)], MethodName = "PublicStaticMethod", ExpectedMethodName = null } },

        { new GetMethodTest { Name = "With Declared Only And Non Public And Instance And 0 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance, ParameterTypes = [], MethodName = "PrivateMethod", ExpectedMethodName = "PrivateMethod" } },
        { new GetMethodTest { Name = "With Declared Only And Non Public And Instance And 1 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance, ParameterTypes = [typeof(int)], MethodName = "PrivateMethod", ExpectedMethodName = "PrivateMethod" } },
        { new GetMethodTest { Name = "With Declared Only And Non Public And Instance And 2 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance, ParameterTypes = [typeof(int), typeof(string)], MethodName = "PrivateMethod", ExpectedMethodName = "PrivateMethod" } },
        { new GetMethodTest { Name = "With Declared Only And Non Public And Instance And 3 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance, ParameterTypes = [typeof(int), typeof(string), typeof(bool)], MethodName = "PrivateMethod", ExpectedMethodName = null } },

        { new GetMethodTest { Name = "With Declared Only And Non Public And Static And 0 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Static, ParameterTypes = [], MethodName = "PrivateStaticMethod", ExpectedMethodName = "PrivateStaticMethod" } },
        { new GetMethodTest { Name = "With Declared Only And Non Public And Static And 1 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Static, ParameterTypes = [typeof(int)], MethodName = "PrivateStaticMethod", ExpectedMethodName = "PrivateStaticMethod" } },
        { new GetMethodTest { Name = "With Declared Only And Non Public And Static And 2 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Static, ParameterTypes = [typeof(int), typeof(string)], MethodName = "PrivateStaticMethod", ExpectedMethodName = "PrivateStaticMethod" } },
        { new GetMethodTest { Name = "With Declared Only And Non Public And Static And 3 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Static, ParameterTypes = [typeof(int), typeof(string), typeof(bool)], MethodName = "PrivateStaticMethod", ExpectedMethodName = null } },

        { new GetMethodTest { Name = "Public And Instance And 0 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Instance, ParameterTypes = [], MethodName = "PublicMethodBase", ExpectedMethodName = "PublicMethodBase" } },
        { new GetMethodTest { Name = "Public And Instance And 1 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Instance, ParameterTypes = [typeof(int)], MethodName = "PublicMethodBase", ExpectedMethodName = "PublicMethodBase" } },
        { new GetMethodTest { Name = "Public And Instance And 2 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Instance, ParameterTypes = [typeof(int), typeof(string)], MethodName = "PublicMethodBase", ExpectedMethodName = "PublicMethodBase" } },
        { new GetMethodTest { Name = "Public And Instance And 3 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Instance, ParameterTypes = [typeof(int), typeof(string), typeof(bool)], MethodName = "PublicMethodBase", ExpectedMethodName = null } },

        { new GetMethodTest { Name = "Public And Static And 0 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Static, ParameterTypes = [], MethodName = "PublicStaticMethodBase", ExpectedMethodName = "PublicStaticMethodBase" } },
        { new GetMethodTest { Name = "Public And Static And 1 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Static, ParameterTypes = [typeof(int)], MethodName = "PublicStaticMethodBase", ExpectedMethodName = "PublicStaticMethodBase" } },
        { new GetMethodTest { Name = "Public And Static And 2 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Static, ParameterTypes = [typeof(int), typeof(string)], MethodName = "PublicStaticMethodBase", ExpectedMethodName = "PublicStaticMethodBase" } },
        { new GetMethodTest { Name = "Public And Static And 3 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Static, ParameterTypes = [typeof(int), typeof(string), typeof(bool)], MethodName = "PublicStaticMethodBase", ExpectedMethodName = null } },

        { new GetMethodTest { Name = "Non Public And Instance And 0 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance, ParameterTypes = [], MethodName = "ProtectedMethodBase", ExpectedMethodName = "ProtectedMethodBase" } },
        { new GetMethodTest { Name = "Non Public And Instance And 1 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance, ParameterTypes = [typeof(int)], MethodName = "ProtectedMethodBase", ExpectedMethodName = "ProtectedMethodBase" } },
        { new GetMethodTest { Name = "Non Public And Instance And 2 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance, ParameterTypes = [typeof(int), typeof(string)], MethodName = "ProtectedMethodBase", ExpectedMethodName = "ProtectedMethodBase" } },
        { new GetMethodTest { Name = "Non Public And Instance And 3 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance, ParameterTypes = [typeof(int), typeof(string), typeof(bool)], MethodName = "ProtectedMethodBase", ExpectedMethodName = null } },

        { new GetMethodTest { Name = "Non Public And Static And 0 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Static, ParameterTypes = [], MethodName = "ProtectedStaticMethodBase", ExpectedMethodName = "ProtectedStaticMethodBase" } },
        { new GetMethodTest { Name = "Non Public And Static And 1 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Static, ParameterTypes = [typeof(int)], MethodName = "ProtectedStaticMethodBase", ExpectedMethodName = "ProtectedStaticMethodBase" } },
        { new GetMethodTest { Name = "Non Public And Static And 2 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Static, ParameterTypes = [typeof(int), typeof(string)], MethodName = "ProtectedStaticMethodBase", ExpectedMethodName = "ProtectedStaticMethodBase" } },
        { new GetMethodTest { Name = "Non Public And Static And 3 Parameters", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Static, ParameterTypes = [typeof(int), typeof(string), typeof(bool)], MethodName = "ProtectedStaticMethodBase", ExpectedMethodName = null } },
    };

    public static TheoryData<IXUnitTest> GetMethodsTheoryData => new()
    {
        { new GetMethodsTest { Name = "With Declared Only And Public And Instance", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Instance, ExpectedMethodNames = ["PublicMethod", "PublicMethod", "PublicMethod"] } },
        { new GetMethodsTest { Name = "With Declared Only And Public And Static", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Static, ExpectedMethodNames = ["PublicStaticMethod", "PublicStaticMethod", "PublicStaticMethod"] } },
        { new GetMethodsTest { Name = "With Declared Only And Public And Instance And Static", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Instance | TypeReflectionFlags.Static, ExpectedMethodNames = ["PublicMethod", "PublicMethod", "PublicMethod", "PublicStaticMethod", "PublicStaticMethod", "PublicStaticMethod"] } },
        { new GetMethodsTest { Name = "With Declared Only And Non Public And Instance", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance, ExpectedMethodNames = ["PrivateMethod", "PrivateMethod", "PrivateMethod"] } },
        { new GetMethodsTest { Name = "With Declared Only And Non Public And Static", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Static, ExpectedMethodNames = ["PrivateStaticMethod", "PrivateStaticMethod", "PrivateStaticMethod"] } },
        { new GetMethodsTest { Name = "With Declared Only And Non Public And Instance And Static", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance | TypeReflectionFlags.Static, ExpectedMethodNames = ["PrivateMethod", "PrivateMethod", "PrivateMethod", "PrivateStaticMethod", "PrivateStaticMethod", "PrivateStaticMethod"] } },
        { new GetMethodsTest { Name = "With Declared Only And Public And Non Public And Instance And Static", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance | TypeReflectionFlags.Static, ExpectedMethodNames = ["PublicMethod", "PublicMethod", "PublicMethod", "PublicStaticMethod", "PublicStaticMethod", "PublicStaticMethod", "PrivateMethod", "PrivateMethod", "PrivateMethod", "PrivateStaticMethod", "PrivateStaticMethod", "PrivateStaticMethod"] } },
        { new GetMethodsTest { Name = "With Public And Instance", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Instance, ExpectedMethodNames = ["PublicMethod", "PublicMethod", "PublicMethod", "PublicMethodBase", "PublicMethodBase", "PublicMethodBase"] } },
        { new GetMethodsTest { Name = "With Public And Static", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Static, ExpectedMethodNames = ["PublicStaticMethod", "PublicStaticMethod", "PublicStaticMethod", "PublicStaticMethodBase", "PublicStaticMethodBase", "PublicStaticMethodBase"] } },
        { new GetMethodsTest { Name = "With Public And Instance And Static", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Instance | TypeReflectionFlags.Static, ExpectedMethodNames = ["PublicMethod", "PublicMethod", "PublicMethod", "PublicStaticMethod", "PublicStaticMethod", "PublicStaticMethod", "PublicMethodBase", "PublicMethodBase", "PublicMethodBase", "PublicStaticMethodBase", "PublicStaticMethodBase", "PublicStaticMethodBase"] } },
        { new GetMethodsTest { Name = "With Non Public And Instance", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance, ExpectedMethodNames = ["PrivateMethod", "PrivateMethod", "PrivateMethod", "ProtectedMethodBase", "ProtectedMethodBase", "ProtectedMethodBase"] } },
        { new GetMethodsTest { Name = "With Non Public And Static", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Static, ExpectedMethodNames = ["PrivateStaticMethod", "PrivateStaticMethod", "PrivateStaticMethod", "ProtectedStaticMethodBase", "ProtectedStaticMethodBase", "ProtectedStaticMethodBase"] } },
        { new GetMethodsTest { Name = "With Non Public And Instance And Static", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance | TypeReflectionFlags.Static, ExpectedMethodNames = ["PrivateMethod", "PrivateMethod", "PrivateMethod", "ProtectedMethodBase", "ProtectedMethodBase", "ProtectedMethodBase", "PrivateStaticMethod", "PrivateStaticMethod", "PrivateStaticMethod", "ProtectedStaticMethodBase", "ProtectedStaticMethodBase", "ProtectedStaticMethodBase"] } },
        { new GetMethodsTest { Name = "With Public And Non Public And Instance And Static", Type = typeof(ClassWithMethods), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance | TypeReflectionFlags.Static, ExpectedMethodNames = ["PublicMethod", "PublicMethod", "PublicMethod", "PublicMethodBase", "PublicMethodBase", "PublicMethodBase", "PublicStaticMethod", "PublicStaticMethod", "PublicStaticMethod", "PublicStaticMethodBase", "PublicStaticMethodBase", "PublicStaticMethodBase", "PrivateMethod", "PrivateMethod", "PrivateMethod", "ProtectedMethodBase", "ProtectedMethodBase", "ProtectedMethodBase", "PrivateStaticMethod", "PrivateStaticMethod", "PrivateStaticMethod", "ProtectedStaticMethodBase", "ProtectedStaticMethodBase", "ProtectedStaticMethodBase"] } },
    };

    public static TheoryData<IXUnitTest> GetPropertyTheoryData => new()
    {
        { new GetPropertyTest { Name = "With Declared Only And Public And Instance", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Instance, PropertyName = "PublicProperty", ExpectedPropertyName = "PublicProperty" } },
        { new GetPropertyTest { Name = "With Declared Only And Public And Static", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Static, PropertyName = "PublicStaticProperty", ExpectedPropertyName = "PublicStaticProperty" } },
        { new GetPropertyTest { Name = "With Declared Only And Non Public And Instance", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance, PropertyName = "PrivateProperty", ExpectedPropertyName = "PrivateProperty" } },
        { new GetPropertyTest { Name = "With Declared Only And Non Public And Static", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Static, PropertyName = "PrivateStaticProperty", ExpectedPropertyName = "PrivateStaticProperty" } },
        { new GetPropertyTest { Name = "With Public And Instance", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Instance, PropertyName = "PublicPropertyBase", ExpectedPropertyName = "PublicPropertyBase" } },
        { new GetPropertyTest { Name = "With Public And Static", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Static, PropertyName = "PublicStaticPropertyBase", ExpectedPropertyName = "PublicStaticPropertyBase" } },
        { new GetPropertyTest { Name = "With Non Public And Instance", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance, PropertyName = "PrivatePropertyBase", ExpectedPropertyName = "PrivatePropertyBase" } },
        { new GetPropertyTest { Name = "With Non Public And Static", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Static, PropertyName = "PrivateStaticPropertyBase", ExpectedPropertyName = "PrivateStaticPropertyBase" } },
    };

    public static TheoryData<IXUnitTest> GetPropertiesTheoryData => new()
    {
        { new GetPropertiesTest { Name = "With Declared Only And Public And Instance", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Instance, ExpectedPropertyNames = ["PublicProperty"] } },
        { new GetPropertiesTest { Name = "With Declared Only And Public And Static", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Static, ExpectedPropertyNames = ["PublicStaticProperty"] } },
        { new GetPropertiesTest { Name = "With Declared Only And Public And Instance And Static", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Instance | TypeReflectionFlags.Static, ExpectedPropertyNames = ["PublicProperty", "PublicStaticProperty"] } },
        { new GetPropertiesTest { Name = "With Declared Only And Non Public And Instance", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance, ExpectedPropertyNames = ["ProtectedInternalProperty", "ProtectedProperty", "InternalProperty", "PrivateProperty"] } },
        { new GetPropertiesTest { Name = "With Declared Only And Non Public And Static", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Static, ExpectedPropertyNames = ["ProtectedInternalStaticProperty", "ProtectedStaticProperty", "InternalStaticProperty", "PrivateStaticProperty"] } },
        { new GetPropertiesTest { Name = "With Declared Only And Non Public And Instance And Static", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance | TypeReflectionFlags.Static, ExpectedPropertyNames = ["ProtectedInternalProperty", "ProtectedInternalStaticProperty", "ProtectedProperty", "ProtectedStaticProperty", "InternalProperty", "InternalStaticProperty", "PrivateProperty", "PrivateStaticProperty"] } },
        { new GetPropertiesTest { Name = "With Declared Only And Public And Non Public And Instance And Static", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance | TypeReflectionFlags.Static, ExpectedPropertyNames = ["PublicProperty", "PublicStaticProperty", "ProtectedInternalProperty", "ProtectedInternalStaticProperty", "ProtectedProperty", "ProtectedStaticProperty", "InternalProperty", "InternalStaticProperty", "PrivateProperty", "PrivateStaticProperty"] } },
        { new GetPropertiesTest { Name = "With Public And Instance", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Instance, ExpectedPropertyNames = ["PublicProperty", "PublicPropertyBase"] } },
        { new GetPropertiesTest { Name = "With Public And Static", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Static, ExpectedPropertyNames = ["PublicStaticProperty", "PublicStaticPropertyBase"] } },
        { new GetPropertiesTest { Name = "With Public And Instance And Static", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.Instance | TypeReflectionFlags.Static, ExpectedPropertyNames = ["PublicProperty", "PublicPropertyBase", "PublicStaticProperty", "PublicStaticPropertyBase"] } },
        { new GetPropertiesTest { Name = "With Non Public And Instance", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance, ExpectedPropertyNames = ["ProtectedInternalProperty", "ProtectedProperty", "InternalProperty", "PrivateProperty", "ProtectedInternalPropertyBase", "ProtectedPropertyBase", "InternalPropertyBase", "PrivatePropertyBase"] } },
        { new GetPropertiesTest { Name = "With Non Public And Static", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Static, ExpectedPropertyNames = ["ProtectedInternalStaticProperty", "ProtectedStaticProperty", "InternalStaticProperty", "PrivateStaticProperty", "ProtectedInternalStaticPropertyBase", "ProtectedStaticPropertyBase", "InternalStaticPropertyBase", "PrivateStaticPropertyBase"] } },
        { new GetPropertiesTest { Name = "With Non Public And Instance And Static", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance | TypeReflectionFlags.Static, ExpectedPropertyNames = ["ProtectedInternalProperty", "ProtectedInternalStaticProperty", "ProtectedProperty", "ProtectedStaticProperty", "InternalProperty", "InternalStaticProperty", "PrivateProperty", "PrivateStaticProperty", "ProtectedInternalPropertyBase", "ProtectedInternalStaticPropertyBase", "ProtectedPropertyBase", "ProtectedStaticPropertyBase", "InternalPropertyBase", "InternalStaticPropertyBase", "PrivatePropertyBase", "PrivateStaticPropertyBase"] } },
        { new GetPropertiesTest { Name = "With Public And Non Public And Instance And Static", Type = typeof(ClassWithProperties), ReflectionFlags = TypeReflectionFlags.Public | TypeReflectionFlags.NonPublic | TypeReflectionFlags.Instance | TypeReflectionFlags.Static, ExpectedPropertyNames = ["PublicProperty", "PublicStaticProperty", "ProtectedInternalProperty", "ProtectedInternalStaticProperty", "ProtectedProperty", "ProtectedStaticProperty", "InternalProperty", "InternalStaticProperty", "PrivateProperty", "PrivateStaticProperty", "PublicPropertyBase", "PublicStaticPropertyBase", "ProtectedInternalPropertyBase", "ProtectedInternalStaticPropertyBase", "ProtectedPropertyBase", "ProtectedStaticPropertyBase", "InternalPropertyBase", "InternalStaticPropertyBase", "PrivatePropertyBase", "PrivateStaticPropertyBase"] } },
    };

    public static TheoryData<IXUnitTest> IsComplexTheoryData => new()
    {
        { new IsComplexTest { Name = "Bool", Type = typeof(bool), Expected = false } },
        { new IsComplexTest { Name = "Byte Array", Type = typeof(byte[]), Expected = false } },
        { new IsComplexTest { Name = "Byte", Type = typeof(byte), Expected = false } },
        { new IsComplexTest { Name = "Char", Type = typeof(char), Expected = false } },
        { new IsComplexTest { Name = "DateTime", Type = typeof(DateTime), Expected = false } },
        { new IsComplexTest { Name = "DateTimeOffset", Type = typeof(DateTimeOffset), Expected = false } },
        { new IsComplexTest { Name = "Decimal", Type = typeof(decimal), Expected = false } },
        { new IsComplexTest { Name = "Double", Type = typeof(double), Expected = false } },
        { new IsComplexTest { Name = "Enum", Type = typeof(PrimaryColor), Expected = false } },
        { new IsComplexTest { Name = "Float", Type = typeof(float), Expected = false } },
        { new IsComplexTest { Name = "Guid", Type = typeof(Guid), Expected = false } },
        { new IsComplexTest { Name = "Int", Type = typeof(int), Expected = false } },
        { new IsComplexTest { Name = "Long", Type = typeof(long), Expected = false } },
        { new IsComplexTest { Name = "Nullable Bool", Type = typeof(bool?), Expected = false } },
        { new IsComplexTest { Name = "Nullable Byte", Type = typeof(byte?), Expected = false } },
        { new IsComplexTest { Name = "Nullable Char", Type = typeof(char?), Expected = false } },
        { new IsComplexTest { Name = "Nullable DateTime", Type = typeof(DateTime?), Expected = false } },
        { new IsComplexTest { Name = "Nullable DateTimeOffset", Type = typeof(DateTimeOffset?), Expected = false } },
        { new IsComplexTest { Name = "Nullable Decimal", Type = typeof(decimal?), Expected = false } },
        { new IsComplexTest { Name = "Nullable Double", Type = typeof(double?), Expected = false } },
        { new IsComplexTest { Name = "Nullable Enum", Type = typeof(PrimaryColor?), Expected = false } },
        { new IsComplexTest { Name = "Nullable Float", Type = typeof(float?), Expected = false } },
        { new IsComplexTest { Name = "Nullable Guid", Type = typeof(Guid?), Expected = false } },
        { new IsComplexTest { Name = "Nullable Int", Type = typeof(int?), Expected = false } },
        { new IsComplexTest { Name = "Nullable Long", Type = typeof(long?), Expected = false } },
        { new IsComplexTest { Name = "Nullable SByte", Type = typeof(sbyte?), Expected = false } },
        { new IsComplexTest { Name = "Nullable Short", Type = typeof(short?), Expected = false } },
        { new IsComplexTest { Name = "Nullable TimeSpan", Type = typeof(TimeSpan?), Expected = false } },
        { new IsComplexTest { Name = "Nullable UInt", Type = typeof(uint?), Expected = false } },
        { new IsComplexTest { Name = "Nullable ULong", Type = typeof(ulong?), Expected = false } },
        { new IsComplexTest { Name = "Nullable UShort", Type = typeof(ushort?), Expected = false } },
        { new IsComplexTest { Name = "Object", Type = typeof(object), Expected = true } },
        { new IsComplexTest { Name = "SByte", Type = typeof(sbyte), Expected = false } },
        { new IsComplexTest { Name = "Short", Type = typeof(short), Expected = false } },
        { new IsComplexTest { Name = "String", Type = typeof(string), Expected = false } },
        { new IsComplexTest { Name = "TimeSpan", Type = typeof(TimeSpan), Expected = false } },
        { new IsComplexTest { Name = "Type", Type = typeof(Type), Expected = false } },
        { new IsComplexTest { Name = "UInt", Type = typeof(uint), Expected = false } },
        { new IsComplexTest { Name = "ULong", Type = typeof(ulong), Expected = false } },
        { new IsComplexTest { Name = "Uri", Type = typeof(Uri), Expected = false } },
        { new IsComplexTest { Name = "UShort", Type = typeof(ushort), Expected = false } },
        { new IsComplexTest { Name = "Class", Type = typeof(Rectangle), Expected = true } },
        { new IsComplexTest { Name = "Interface", Type = typeof(IShape), Expected = true } },
        { new IsComplexTest { Name = "Open Generic", Type = typeof(ICollection<>), Expected = true } },
        { new IsComplexTest { Name = "Closed Generic", Type = typeof(ICollection<string>), Expected = true } },
    };

    public static TheoryData<IXUnitTest> IsEnumerableOfTTheoryData => new()
    {
        { new IsEnumerableOfTTest { Name = "With Simple Type", Type = typeof(bool), Expected = false } },
        { new IsEnumerableOfTTest { Name = "With Complex Type", Type = typeof(Rectangle), Expected = false } },
        { new IsEnumerableOfTTest { Name = "With Closed Generic List", Type = typeof(List<string>), Expected = true } },
        { new IsEnumerableOfTTest { Name = "With Open Generic List", Type = typeof(List<>), Expected = true } },
        { new IsEnumerableOfTTest { Name = "With Closed Enumerable Of T", Type = typeof(IEnumerable<string>), Expected = true } },
        { new IsEnumerableOfTTest { Name = "With Open Enumerable Of T", Type = typeof(IEnumerable<>), Expected = true } },
        { new IsEnumerableOfTTest { Name = "With Array", Type = typeof(int[]), Expected = true } },
    };

    public static TheoryData<IXUnitTest> IsImplementationOfTheoryData => new()
    {
        { new IsImplementationOfTest { Name = "Negative Case With Generics", DerivedType = typeof(Host<>), BaseType = typeof(ICollection<>), Expected = false } },
        { new IsImplementationOfTest { Name = "Negative Case With Non Generics", DerivedType = typeof(Rectangle), BaseType = typeof(IAnimal), Expected = false } },
        { new IsImplementationOfTest { Name = "Positive Case With Generics", DerivedType = typeof(Host<>), BaseType = typeof(IHost<>), Expected = true } },
        { new IsImplementationOfTest { Name = "Positive Case With Non Generics", DerivedType = typeof(Rectangle), BaseType = typeof(IShape), Expected = true } },
    };

    public static TheoryData<IXUnitTest> IsSimpleTheoryData => new()
    {
        { new IsSimpleTest { Name = "Bool", Type = typeof(bool), Expected = true } },
        { new IsSimpleTest { Name = "ByteArray", Type = typeof(byte[]), Expected = true } },
        { new IsSimpleTest { Name = "Byte", Type = typeof(byte), Expected = true } },
        { new IsSimpleTest { Name = "Char", Type = typeof(char), Expected = true } },
        { new IsSimpleTest { Name = "DateTime", Type = typeof(DateTime), Expected = true } },
        { new IsSimpleTest { Name = "DateTimeOffset", Type = typeof(DateTimeOffset), Expected = true } },
        { new IsSimpleTest { Name = "Decimal", Type = typeof(decimal), Expected = true } },
        { new IsSimpleTest { Name = "Double", Type = typeof(double), Expected = true } },
        { new IsSimpleTest { Name = "Enum", Type = typeof(PrimaryColor), Expected = true } },
        { new IsSimpleTest { Name = "Float", Type = typeof(float), Expected = true } },
        { new IsSimpleTest { Name = "Guid", Type = typeof(Guid), Expected = true } },
        { new IsSimpleTest { Name = "Int", Type = typeof(int), Expected = true } },
        { new IsSimpleTest { Name = "Long", Type = typeof(long), Expected = true } },
        { new IsSimpleTest { Name = "NullableBool", Type = typeof(bool?), Expected = true } },
        { new IsSimpleTest { Name = "NullableByte", Type = typeof(byte?), Expected = true } },
        { new IsSimpleTest { Name = "NullableChar", Type = typeof(char?), Expected = true } },
        { new IsSimpleTest { Name = "NullableDateTime", Type = typeof(DateTime?), Expected = true } },
        { new IsSimpleTest { Name = "NullableDateTimeOffset", Type = typeof(DateTimeOffset?), Expected = true } },
        { new IsSimpleTest { Name = "NullableDecimal", Type = typeof(decimal?), Expected = true } },
        { new IsSimpleTest { Name = "NullableDouble", Type = typeof(double?), Expected = true } },
        { new IsSimpleTest { Name = "NullableEnum", Type = typeof(PrimaryColor?), Expected = true } },
        { new IsSimpleTest { Name = "NullableFloat", Type = typeof(float?), Expected = true } },
        { new IsSimpleTest { Name = "NullableGuid", Type = typeof(Guid?), Expected = true } },
        { new IsSimpleTest { Name = "NullableInt", Type = typeof(int?), Expected = true } },
        { new IsSimpleTest { Name = "NullableLong", Type = typeof(long?), Expected = true } },
        { new IsSimpleTest { Name = "NullableSByte", Type = typeof(sbyte?), Expected = true } },
        { new IsSimpleTest { Name = "NullableShort", Type = typeof(short?), Expected = true } },
        { new IsSimpleTest { Name = "NullableTimeSpan", Type = typeof(TimeSpan?), Expected = true } },
        { new IsSimpleTest { Name = "NullableUInt", Type = typeof(uint?), Expected = true } },
        { new IsSimpleTest { Name = "NullableULong", Type = typeof(ulong?), Expected = true } },
        { new IsSimpleTest { Name = "NullableUShort", Type = typeof(ushort?), Expected = true } },
        { new IsSimpleTest { Name = "SByte", Type = typeof(sbyte), Expected = true } },
        { new IsSimpleTest { Name = "Short", Type = typeof(short), Expected = true } },
        { new IsSimpleTest { Name = "String", Type = typeof(string), Expected = true } },
        { new IsSimpleTest { Name = "TimeSpan", Type = typeof(TimeSpan), Expected = true } },
        { new IsSimpleTest { Name = "Type", Type = typeof(Type), Expected = true } },
        { new IsSimpleTest { Name = "UInt", Type = typeof(uint), Expected = true } },
        { new IsSimpleTest { Name = "ULong", Type = typeof(ulong), Expected = true } },
        { new IsSimpleTest { Name = "Uri", Type = typeof(Uri), Expected = true } },
        { new IsSimpleTest { Name = "UShort", Type = typeof(ushort), Expected = true } },
        { new IsSimpleTest { Name = "Class", Type = typeof(Rectangle), Expected = false } },
        { new IsSimpleTest { Name = "Interface", Type = typeof(IShape), Expected = false } },
        { new IsSimpleTest { Name = "OpenGeneric", Type = typeof(ICollection<>), Expected = false } },
        { new IsSimpleTest { Name = "ClosedGeneric", Type = typeof(ICollection<string>), Expected = false } },
        { new IsSimpleTest { Name = "Object", Type = typeof(object), Expected = false } },
    };

    public static TheoryData<IXUnitTest> IsSubclassOrImplementationOfTheoryData => new()
    {
        { new IsSubclassOrImplementationOfTest { Name = "Negative Case With Generics With Derived And Interface", DerivedType = typeof(Host<>), BaseType = typeof(ICollection<>), Expected = false } },
        { new IsSubclassOrImplementationOfTest { Name = "Negative Case With Generics With Derived And Abstract", DerivedType = typeof(Host<>), BaseType = typeof(AbstractCollection<>), Expected = false } },
        { new IsSubclassOrImplementationOfTest { Name = "Negative Case With Generics With Abstract And Interface", DerivedType = typeof(AbstractHost<>), BaseType = typeof(ICollection<>), Expected = false } },

        { new IsSubclassOrImplementationOfTest { Name = "Negative Case With Non Generics With Derived And Interface", DerivedType = typeof(Rectangle), BaseType = typeof(IAnimal), Expected = false } },
        { new IsSubclassOrImplementationOfTest { Name = "Negative Case With Non Generics With Derived And Abstract", DerivedType = typeof(Rectangle), BaseType = typeof(AbstractAnimal), Expected = false } },
        { new IsSubclassOrImplementationOfTest { Name = "Negative Case With Non Generics With Abstract And Interface", DerivedType = typeof(AbstractShape), BaseType = typeof(IAnimal), Expected = false } },

        { new IsSubclassOrImplementationOfTest { Name = "Positive Case With Non Generics With Derived And Interface", DerivedType = typeof(Rectangle), BaseType = typeof(IShape), Expected = true } },
        { new IsSubclassOrImplementationOfTest { Name = "Positive Case With Non Generics With Derived And Abstract", DerivedType = typeof(Rectangle), BaseType = typeof(AbstractShape), Expected = true } },
        { new IsSubclassOrImplementationOfTest { Name = "Positive Case With Non Generics With Abstract And Interface", DerivedType = typeof(AbstractShape), BaseType = typeof(IShape), Expected = true } },

        { new IsSubclassOrImplementationOfTest { Name = "Positive Case With Generics With Derived And Interface", DerivedType = typeof(Host<>), BaseType = typeof(IHost<>), Expected = true } },
        { new IsSubclassOrImplementationOfTest { Name = "Positive Case With Generics With Derived And Abstract", DerivedType = typeof(Host<>), BaseType = typeof(AbstractHost<>), Expected = true } },
        { new IsSubclassOrImplementationOfTest { Name = "Positive Case With Generics With Abstract And Interface", DerivedType = typeof(AbstractHost<>), BaseType = typeof(IHost<>), Expected = true } },
    };
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(GetBaseTypeTheoryData))]
    public void TestGetBaseType(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(GetBaseTypesTheoryData))]
    public void TestGetBaseTypes(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(GetConstructorTheoryData))]
    public void TestGetConstructor(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(GetConstructorsTheoryData))]
    public void TestGetConstructors(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(GetFieldTheoryData))]
    public void TestGetField(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(GetFieldsTheoryData))]
    public void TestGetFields(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(GetMethodTheoryData))]
    public void TestGetMethod(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(GetMethodsTheoryData))]
    public void TestGetMethods(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(GetPropertyTheoryData))]
    public void TestGetProperty(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(GetPropertiesTheoryData))]
    public void TestGetProperties(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(IsComplexTheoryData))]
    public void TestIsComplex(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(IsEnumerableOfTTheoryData))]
    public void TestIsEnumerableOfT(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(IsImplementationOfTheoryData))]
    public void TestIsImplementationOf(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(IsSimpleTheoryData))]
    public void TestIsSimple(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(IsSubclassOrImplementationOfTheoryData))]
    public void TestIsSubclassOrImplementationOf(IXUnitTest test)
    {
        test.Execute(this);
    }
    #endregion
}