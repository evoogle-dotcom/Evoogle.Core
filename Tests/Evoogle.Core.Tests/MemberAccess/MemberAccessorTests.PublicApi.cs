// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Reflection;

using Evoogle.Coercion;
using Evoogle.XUnit;

using FluentAssertions;

namespace Evoogle.MemberAccess;

public partial class MemberAccessorTests
{
    #region Types
    private enum ThrowingOperation
    {
        GetObject,
        GetCoercingObject,
        GetGeneric,
        GetCoercingGeneric,
        GetIncompatible,
        GetFailedCoercion,
        GetMissing,
        GetStaticMismatch,
        SetObject,
        SetCoercingObject,
        SetGeneric,
        SetCoercingGeneric,
        SetMissing,
        SetInstanceMismatch,
        SetBoxedStruct,
        GetStaticObject,
        GetCoercingStaticObject,
        GetStaticGeneric,
        GetCoercingStaticGeneric,
        GetStaticIncompatible,
        GetStaticFailedCoercion,
        GetInstanceMismatch,
        SetStaticObject,
        SetCoercingStaticObject,
        SetStaticGeneric,
        SetCoercingStaticGeneric,
        SetStaticMissing,
        SetStaticMismatch,
        SetByRef,
        SetCoercingByRef,
    }

    private enum FactoryOperation
    {
        Getter,
        CoercingGetter,
        GenericGetter,
        CoercingGenericGetter,
        Setter,
        CoercingSetter,
        GenericSetter,
        CoercingGenericSetter,
        StaticGetter,
        CoercingStaticGetter,
        GenericStaticGetter,
        CoercingGenericStaticGetter,
        StaticSetter,
        CoercingStaticSetter,
        GenericStaticSetter,
        CoercingGenericStaticSetter,
        SetterByRef,
        CoercingSetterByRef,
    }
    #endregion

    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] ThrowingGetTheoryData =>
    [
        ThrowingCase(ThrowingOperation.GetObject, "11"),
        ThrowingCase(ThrowingOperation.GetCoercingObject, 11L),
        ThrowingCase(ThrowingOperation.GetGeneric, "11"),
        ThrowingCase(ThrowingOperation.GetCoercingGeneric, 11L),
        ThrowingFailure(ThrowingOperation.GetIncompatible, nameof(MemberAccessShape.InvocationTrackedValue)),
        ThrowingFailure(ThrowingOperation.GetFailedCoercion, nameof(MemberAccessShape.TextValue)),
        ThrowingFailure(ThrowingOperation.GetMissing, nameof(MemberAccessShape.WriteOnlyValue)),
        ThrowingFailure(ThrowingOperation.GetStaticMismatch, nameof(MemberAccessShape.TextValue)),
    ];

    public static TheoryDataRow<IXUnitTest>[] ThrowingSetTheoryData =>
    [
        ThrowingCase(ThrowingOperation.SetObject, "object-set"),
        ThrowingCase(ThrowingOperation.SetCoercingObject, "17"),
        ThrowingCase(ThrowingOperation.SetGeneric, "generic-set"),
        ThrowingCase(ThrowingOperation.SetCoercingGeneric, "18"),
        ThrowingFailure(ThrowingOperation.SetMissing, nameof(MemberAccessShape.ReadonlyProperty)),
        ThrowingFailure(ThrowingOperation.SetInstanceMismatch, nameof(MemberAccessShape.StaticTextValue)),
        ThrowingFailure(ThrowingOperation.SetBoxedStruct, nameof(Point.X)),
    ];

    public static TheoryDataRow<IXUnitTest>[] ThrowingGetStaticTheoryData =>
    [
        ThrowingCase(ThrowingOperation.GetStaticObject, "12"),
        ThrowingCase(ThrowingOperation.GetCoercingStaticObject, 12L),
        ThrowingCase(ThrowingOperation.GetStaticGeneric, "12"),
        ThrowingCase(ThrowingOperation.GetCoercingStaticGeneric, 12L),
        ThrowingFailure(ThrowingOperation.GetStaticIncompatible, nameof(MemberAccessShape.StaticInvocationTrackedValue)),
        ThrowingFailure(ThrowingOperation.GetStaticFailedCoercion, nameof(MemberAccessShape.StaticTextValue)),
        ThrowingFailure(ThrowingOperation.GetInstanceMismatch, nameof(MemberAccessShape.StaticTextValue)),
    ];

    public static TheoryDataRow<IXUnitTest>[] ThrowingSetStaticTheoryData =>
    [
        ThrowingCase(ThrowingOperation.SetStaticObject, "static-object-set"),
        ThrowingCase(ThrowingOperation.SetCoercingStaticObject, "19"),
        ThrowingCase(ThrowingOperation.SetStaticGeneric, "static-generic-set"),
        ThrowingCase(ThrowingOperation.SetCoercingStaticGeneric, "20"),
        ThrowingFailure(ThrowingOperation.SetStaticMissing, nameof(MemberAccessShape.StaticGetterInvocationCount)),
        ThrowingFailure(ThrowingOperation.SetStaticMismatch, nameof(MemberAccessShape.TextValue)),
    ];

    public static TheoryDataRow<IXUnitTest>[] ThrowingSetByRefTheoryData =>
    [
        ThrowingCase(ThrowingOperation.SetByRef, 21L),
        ThrowingCase(ThrowingOperation.SetCoercingByRef, 22L),
    ];

    public static TheoryDataRow<IXUnitTest>[] FactoryGetterTheoryData => FactoryCases
    (
        FactoryOperation.Getter,
        FactoryOperation.CoercingGetter,
        FactoryOperation.GenericGetter,
        FactoryOperation.CoercingGenericGetter
    );

    public static TheoryDataRow<IXUnitTest>[] FactorySetterTheoryData => FactoryCases
    (
        FactoryOperation.Setter,
        FactoryOperation.CoercingSetter,
        FactoryOperation.GenericSetter,
        FactoryOperation.CoercingGenericSetter
    );

    public static TheoryDataRow<IXUnitTest>[] FactoryStaticGetterTheoryData => FactoryCases
    (
        FactoryOperation.StaticGetter,
        FactoryOperation.CoercingStaticGetter,
        FactoryOperation.GenericStaticGetter,
        FactoryOperation.CoercingGenericStaticGetter
    );

    public static TheoryDataRow<IXUnitTest>[] FactoryStaticSetterTheoryData => FactoryCases
    (
        FactoryOperation.StaticSetter,
        FactoryOperation.CoercingStaticSetter,
        FactoryOperation.GenericStaticSetter,
        FactoryOperation.CoercingGenericStaticSetter
    );

    public static TheoryDataRow<IXUnitTest>[] FactorySetByRefTheoryData => FactoryCases
    (
        FactoryOperation.SetterByRef,
        FactoryOperation.CoercingSetterByRef
    );

    public static TheoryDataRow<IXUnitTest>[] FactoryFailureTheoryData =>
    [
        new FactoryTryCreateTest { Name = $"{nameof(MemberAccessorFactory)} TryCreate setter fails for readonly member", Operation = FactoryOperation.Setter, UseReadonlyMember = true, ExpectedSuccess = false },
        new FactoryTryCreateTest { Name = $"{nameof(MemberAccessorFactory)} TryCreate static getter fails for instance member", Operation = FactoryOperation.StaticGetter, UseMismatchedMember = true, ExpectedSuccess = false },
        new FactoryTryCreateTest { Name = $"{nameof(MemberAccessorFactory)} TryCreate instance getter fails for static member", Operation = FactoryOperation.Getter, UseMismatchedMember = true, ExpectedSuccess = false },
        new FactoryCreateTest { Name = $"{nameof(MemberAccessorFactory)} Create static getter throws for instance member", Operation = FactoryOperation.StaticGetter, UseMismatchedMember = true, ExpectedException = true },
        new FactoryCreateTest { Name = $"{nameof(MemberAccessorFactory)} Create instance getter throws for static member", Operation = FactoryOperation.Getter, UseMismatchedMember = true, ExpectedException = true },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(ThrowingGetTheoryData))]
    public void ThrowingGet(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(ThrowingSetTheoryData))]
    public void ThrowingSet(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(ThrowingGetStaticTheoryData))]
    public void ThrowingGetStatic(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(ThrowingSetStaticTheoryData))]
    public void ThrowingSetStatic(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(ThrowingSetByRefTheoryData))]
    public void ThrowingSetByRef(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(FactoryGetterTheoryData))]
    public void FactoryGetter(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(FactorySetterTheoryData))]
    public void FactorySetter(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(FactoryStaticGetterTheoryData))]
    public void FactoryStaticGetter(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(FactoryStaticSetterTheoryData))]
    public void FactoryStaticSetter(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(FactorySetByRefTheoryData))]
    public void FactorySetByRef(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(FactoryFailureTheoryData))]
    public void FactoryFailure(IXUnitTest test) => test.Execute(this);
    #endregion

    #region Tests
    private sealed class ThrowingApiTest : XUnitTest
    {
        public required ThrowingOperation Operation { get; init; }

        public object? ExpectedValue { get; init; }

        public string? ExpectedExceptionMemberName { get; init; }

        private MemberAccessShape Instance { get; set; } = null!;

        private object? ActualValue { get; set; }

        private Exception? ActualException { get; set; }

        protected override void Arrange()
        {
            this.Instance = new MemberAccessShape { TextValue = "11" };
            MemberAccessShape.StaticTextValue = "12";
            MemberAccessShape.ResetStaticGetterInvocationCount();
        }

        protected override void Act()
        {
            try
            {
                this.ActualValue = this.Invoke();
            }
            catch (Exception exception)
            {
                this.ActualException = exception;
            }
        }

        protected override void Assert()
        {
            if (this.ExpectedExceptionMemberName is null)
            {
                this.ActualException.Should().BeNull();
                this.ActualValue.Should().BeEquivalentTo(this.ExpectedValue);
                return;
            }

            this.ActualException.Should().BeOfType<MemberAccessException>();
            this.ActualException!.Message.Should().Contain(this.ExpectedExceptionMemberName);
            this.ActualException.InnerException.Should().NotBeNull();
            if (this.Operation == ThrowingOperation.GetIncompatible)
            {
                this.Instance.GetterInvocationCount.Should().Be(0);
            }
            if (this.Operation == ThrowingOperation.GetStaticIncompatible)
            {
                MemberAccessShape.StaticGetterInvocationCount.Should().Be(0);
            }
        }

        private object? Invoke()
        {
            var coercion = new TypeCoercion();
            var text = MemberAccessor.CreateProperty(typeof(MemberAccessShape), nameof(MemberAccessShape.TextValue));
            var staticText = MemberAccessor.CreateProperty(typeof(MemberAccessShape), nameof(MemberAccessShape.StaticTextValue), BindingFlags.Public | BindingFlags.Static);

            switch (this.Operation)
            {
                case ThrowingOperation.GetObject:
                    return text.GetValue(this.Instance);
                case ThrowingOperation.GetCoercingObject:
                    return text.GetValue(this.Instance, typeof(long), coercion);
                case ThrowingOperation.GetGeneric:
                    return text.GetValue<MemberAccessShape, string>(this.Instance);
                case ThrowingOperation.GetCoercingGeneric:
                    return text.GetValue<MemberAccessShape, long>(this.Instance, coercion);
                case ThrowingOperation.GetIncompatible:
                    return MemberAccessor.CreateProperty(typeof(MemberAccessShape), nameof(MemberAccessShape.InvocationTrackedValue)).GetValue(this.Instance, typeof(Guid));
                case ThrowingOperation.GetFailedCoercion:
                    this.Instance.TextValue = "invalid";
                    return text.GetValue(this.Instance, typeof(long), coercion);
                case ThrowingOperation.GetMissing:
                    return MemberAccessor.CreateProperty(typeof(MemberAccessShape), nameof(MemberAccessShape.WriteOnlyValue)).GetValue(this.Instance);
                case ThrowingOperation.GetStaticMismatch:
                    return text.GetStaticValue();
                case ThrowingOperation.SetObject:
                    text.SetValue(this.Instance, "object-set");
                    return this.Instance.TextValue;
                case ThrowingOperation.SetCoercingObject:
                    text.SetValue(this.Instance, 17L, coercion);
                    return this.Instance.TextValue;
                case ThrowingOperation.SetGeneric:
                    text.SetValue<MemberAccessShape, string>(this.Instance, "generic-set");
                    return this.Instance.TextValue;
                case ThrowingOperation.SetCoercingGeneric:
                    text.SetValue<MemberAccessShape, long>(this.Instance, 18L, coercion);
                    return this.Instance.TextValue;
                case ThrowingOperation.SetMissing:
                    MemberAccessor.CreateProperty(typeof(MemberAccessShape), nameof(MemberAccessShape.ReadonlyProperty)).SetValue(this.Instance, 1L);
                    return null;
                case ThrowingOperation.SetInstanceMismatch:
                    staticText.SetValue(this.Instance, "value");
                    return null;
                case ThrowingOperation.SetBoxedStruct:
                    MemberAccessor.CreateField(typeof(Point), nameof(Point.X)).SetValue((object)new Point(), 1L);
                    return null;
                case ThrowingOperation.GetStaticObject:
                    return staticText.GetStaticValue();
                case ThrowingOperation.GetCoercingStaticObject:
                    return staticText.GetStaticValue(typeof(long), coercion);
                case ThrowingOperation.GetStaticGeneric:
                    return staticText.GetStaticValue<string>();
                case ThrowingOperation.GetCoercingStaticGeneric:
                    return staticText.GetStaticValue<long>(coercion);
                case ThrowingOperation.GetStaticIncompatible:
                    return MemberAccessor.CreateProperty(typeof(MemberAccessShape), nameof(MemberAccessShape.StaticInvocationTrackedValue), BindingFlags.Public | BindingFlags.Static).GetStaticValue(typeof(Guid));
                case ThrowingOperation.GetStaticFailedCoercion:
                    MemberAccessShape.StaticTextValue = "invalid";
                    return staticText.GetStaticValue(typeof(long), coercion);
                case ThrowingOperation.GetInstanceMismatch:
                    return staticText.GetValue(this.Instance);
                case ThrowingOperation.SetStaticObject:
                    staticText.SetStaticValue("static-object-set");
                    return MemberAccessShape.StaticTextValue;
                case ThrowingOperation.SetCoercingStaticObject:
                    staticText.SetStaticValue(19L, coercion);
                    return MemberAccessShape.StaticTextValue;
                case ThrowingOperation.SetStaticGeneric:
                    staticText.SetStaticValue<string>("static-generic-set");
                    return MemberAccessShape.StaticTextValue;
                case ThrowingOperation.SetCoercingStaticGeneric:
                    staticText.SetStaticValue<long>(20L, coercion);
                    return MemberAccessShape.StaticTextValue;
                case ThrowingOperation.SetStaticMissing:
                    MemberAccessor.CreateProperty(typeof(MemberAccessShape), nameof(MemberAccessShape.StaticGetterInvocationCount), BindingFlags.Public | BindingFlags.Static).SetStaticValue(1);
                    return null;
                case ThrowingOperation.SetStaticMismatch:
                    text.SetStaticValue("value");
                    return null;
                case ThrowingOperation.SetByRef:
                    var directPoint = new Point();
                    MemberAccessor.CreateField(typeof(Point), nameof(Point.X)).SetValueByRef(ref directPoint, 21L);
                    return directPoint.X;
                case ThrowingOperation.SetCoercingByRef:
                    var coercingPoint = new Point();
                    MemberAccessor.CreateField(typeof(Point), nameof(Point.X)).SetValueByRef(ref coercingPoint, "22", coercion);
                    return coercingPoint.X;
                default:
                    throw new ArgumentOutOfRangeException(nameof(this.Operation));
            }
        }
    }

    private abstract class FactoryTestBase : XUnitTest
    {
        public required FactoryOperation Operation { get; init; }

        public bool UseMismatchedMember { get; init; }

        protected MemberInfo? GetMemberInfo()
        {
            if (this.UseMismatchedMember)
            {
                return IsStatic(this.Operation)
                    ? typeof(MemberAccessShape).GetProperty(nameof(MemberAccessShape.TextValue))
                    : typeof(MemberAccessShape).GetProperty(nameof(MemberAccessShape.StaticTextValue));
            }

            return this.Operation switch
            {
                FactoryOperation.SetterByRef or FactoryOperation.CoercingSetterByRef => typeof(Point).GetField(nameof(Point.X)),
                _ when IsStatic(this.Operation) => typeof(MemberAccessShape).GetProperty(nameof(MemberAccessShape.StaticTextValue)),
                _ => typeof(MemberAccessShape).GetProperty(nameof(MemberAccessShape.TextValue)),
            };
        }
    }

    private sealed class FactoryCreateTest : FactoryTestBase
    {
        public bool ExpectedException { get; init; }

        private object? ActualValue { get; set; }

        private Exception? ActualException { get; set; }

        protected override void Act()
        {
            try
            {
                this.ActualValue = InvokeFactory(this.Operation, this.GetMemberInfo()!, useTryCreate: false).Value;
            }
            catch (Exception exception)
            {
                this.ActualException = exception;
            }
        }

        protected override void Assert()
        {
            if (this.ExpectedException)
            {
                this.ActualException.Should().BeOfType<MemberAccessException>();
            }
            else
            {
                this.ActualException.Should().BeNull();
                this.ActualValue.Should().BeEquivalentTo(ExpectedFactoryValue(this.Operation));
            }
        }
    }

    private sealed class FactoryTryCreateTest : FactoryTestBase
    {
        public bool UseNullMember { get; init; }

        public bool UseReadonlyMember { get; init; }

        public bool ExpectedSuccess { get; init; } = true;

        private bool ActualSuccess { get; set; }

        private object? ActualValue { get; set; }

        protected override void Act()
        {
            var memberInfo = this.UseNullMember
                ? null
                : this.UseReadonlyMember
                    ? typeof(MemberAccessShape).GetProperty(nameof(MemberAccessShape.ReadonlyProperty))
                    : this.GetMemberInfo();
            (this.ActualSuccess, this.ActualValue) = InvokeFactory(this.Operation, memberInfo, useTryCreate: true);
        }

        protected override void Assert()
        {
            this.ActualSuccess.Should().Be(this.ExpectedSuccess);
            this.ActualValue.Should().BeEquivalentTo(this.ExpectedSuccess ? ExpectedFactoryValue(this.Operation) : null);
        }
    }
    #endregion

    #region Theory Data Methods
    private static ThrowingApiTest ThrowingCase(ThrowingOperation operation, object? expectedValue) => new()
    {
        Name = $"{nameof(MemberAccessor)} {operation} succeeds",
        Operation = operation,
        ExpectedValue = expectedValue,
    };

    private static ThrowingApiTest ThrowingFailure(ThrowingOperation operation, string memberName) => new()
    {
        Name = $"{nameof(MemberAccessor)} {operation} throws {nameof(MemberAccessException)} for {memberName}",
        Operation = operation,
        ExpectedExceptionMemberName = memberName,
    };

    private static TheoryDataRow<IXUnitTest>[] FactoryCases(params FactoryOperation[] operations)
        => operations.SelectMany(static operation => new TheoryDataRow<IXUnitTest>[]
        {
            new FactoryCreateTest { Name = $"{nameof(MemberAccessorFactory)} Create {operation} returns an executable delegate", Operation = operation },
            new FactoryTryCreateTest { Name = $"{nameof(MemberAccessorFactory)} TryCreate {operation} returns an executable delegate", Operation = operation },
            new FactoryTryCreateTest { Name = $"{nameof(MemberAccessorFactory)} TryCreate {operation} fails for null member", Operation = operation, UseNullMember = true, ExpectedSuccess = false },
        }).ToArray();

    private static bool IsStatic(FactoryOperation operation) => operation is
        FactoryOperation.StaticGetter or
        FactoryOperation.CoercingStaticGetter or
        FactoryOperation.GenericStaticGetter or
        FactoryOperation.CoercingGenericStaticGetter or
        FactoryOperation.StaticSetter or
        FactoryOperation.CoercingStaticSetter or
        FactoryOperation.GenericStaticSetter or
        FactoryOperation.CoercingGenericStaticSetter;

    private static object ExpectedFactoryValue(FactoryOperation operation) => operation switch
    {
        FactoryOperation.Getter or FactoryOperation.GenericGetter => "21",
        FactoryOperation.CoercingGetter or FactoryOperation.CoercingGenericGetter => 21L,
        FactoryOperation.Setter => "object-set",
        FactoryOperation.CoercingSetter => "2",
        FactoryOperation.GenericSetter => "generic-set",
        FactoryOperation.CoercingGenericSetter => "4",
        FactoryOperation.StaticGetter or FactoryOperation.GenericStaticGetter => "22",
        FactoryOperation.CoercingStaticGetter or FactoryOperation.CoercingGenericStaticGetter => 22L,
        FactoryOperation.StaticSetter => "static-set",
        FactoryOperation.CoercingStaticSetter => "6",
        FactoryOperation.GenericStaticSetter => "generic-static-set",
        FactoryOperation.CoercingGenericStaticSetter => "8",
        FactoryOperation.SetterByRef => 9L,
        FactoryOperation.CoercingSetterByRef => 10L,
        _ => throw new ArgumentOutOfRangeException(nameof(operation)),
    };

    private static (bool Success, object? Value) InvokeFactory
    (
        FactoryOperation operation,
        MemberInfo? memberInfo,
        bool useTryCreate
    )
    {
        var instance = new MemberAccessShape { TextValue = "21" };
        MemberAccessShape.StaticTextValue = "22";
        var coercion = new TypeCoercion();

        return operation switch
        {
            FactoryOperation.Getter => GetFactoryValue(useTryCreate ? Try(MemberAccessorFactory.TryCreateGetter(memberInfo, out var value), value) : MemberAccessorFactory.CreateGetter(memberInfo!), instance),
            FactoryOperation.CoercingGetter => GetCoercingFactoryValue(useTryCreate ? Try(MemberAccessorFactory.TryCreateCoercingGetter(memberInfo, out var value), value) : MemberAccessorFactory.CreateCoercingGetter(memberInfo!), instance, coercion),
            FactoryOperation.GenericGetter => GetGenericFactoryValue(useTryCreate ? Try(MemberAccessorFactory.TryCreateGetter<MemberAccessShape, string>(memberInfo, out var value), value) : MemberAccessorFactory.CreateGetter<MemberAccessShape, string>(memberInfo!), instance),
            FactoryOperation.CoercingGenericGetter => GetCoercingGenericFactoryValue(useTryCreate ? Try(MemberAccessorFactory.TryCreateCoercingGetter<MemberAccessShape, long>(memberInfo, out var value), value) : MemberAccessorFactory.CreateCoercingGetter<MemberAccessShape, long>(memberInfo!), instance, coercion),
            FactoryOperation.Setter => SetFactoryValue(useTryCreate ? Try(MemberAccessorFactory.TryCreateSetter(memberInfo, out var value), value) : MemberAccessorFactory.CreateSetter(memberInfo!), instance),
            FactoryOperation.CoercingSetter => SetCoercingFactoryValue(useTryCreate ? Try(MemberAccessorFactory.TryCreateCoercingSetter(memberInfo, out var value), value) : MemberAccessorFactory.CreateCoercingSetter(memberInfo!), instance, coercion),
            FactoryOperation.GenericSetter => SetGenericFactoryValue(useTryCreate ? Try(MemberAccessorFactory.TryCreateSetter<MemberAccessShape, string>(memberInfo, out var value), value) : MemberAccessorFactory.CreateSetter<MemberAccessShape, string>(memberInfo!), instance),
            FactoryOperation.CoercingGenericSetter => SetCoercingGenericFactoryValue(useTryCreate ? Try(MemberAccessorFactory.TryCreateCoercingSetter<MemberAccessShape, long>(memberInfo, out var value), value) : MemberAccessorFactory.CreateCoercingSetter<MemberAccessShape, long>(memberInfo!), instance, coercion),
            FactoryOperation.StaticGetter => GetStaticFactoryValue(useTryCreate ? Try(MemberAccessorFactory.TryCreateStaticGetter(memberInfo, out var value), value) : MemberAccessorFactory.CreateStaticGetter(memberInfo!)),
            FactoryOperation.CoercingStaticGetter => GetCoercingStaticFactoryValue(useTryCreate ? Try(MemberAccessorFactory.TryCreateCoercingStaticGetter(memberInfo, out var value), value) : MemberAccessorFactory.CreateCoercingStaticGetter(memberInfo!), coercion),
            FactoryOperation.GenericStaticGetter => GetGenericStaticFactoryValue(useTryCreate ? Try(MemberAccessorFactory.TryCreateStaticGetter<string>(memberInfo, out var value), value) : MemberAccessorFactory.CreateStaticGetter<string>(memberInfo!)),
            FactoryOperation.CoercingGenericStaticGetter => GetCoercingGenericStaticFactoryValue(useTryCreate ? Try(MemberAccessorFactory.TryCreateCoercingStaticGetter<long>(memberInfo, out var value), value) : MemberAccessorFactory.CreateCoercingStaticGetter<long>(memberInfo!), coercion),
            FactoryOperation.StaticSetter => SetStaticFactoryValue(useTryCreate ? Try(MemberAccessorFactory.TryCreateStaticSetter(memberInfo, out var value), value) : MemberAccessorFactory.CreateStaticSetter(memberInfo!)),
            FactoryOperation.CoercingStaticSetter => SetCoercingStaticFactoryValue(useTryCreate ? Try(MemberAccessorFactory.TryCreateCoercingStaticSetter(memberInfo, out var value), value) : MemberAccessorFactory.CreateCoercingStaticSetter(memberInfo!), coercion),
            FactoryOperation.GenericStaticSetter => SetGenericStaticFactoryValue(useTryCreate ? Try(MemberAccessorFactory.TryCreateStaticSetter<string>(memberInfo, out var value), value) : MemberAccessorFactory.CreateStaticSetter<string>(memberInfo!)),
            FactoryOperation.CoercingGenericStaticSetter => SetCoercingGenericStaticFactoryValue(useTryCreate ? Try(MemberAccessorFactory.TryCreateCoercingStaticSetter<long>(memberInfo, out var value), value) : MemberAccessorFactory.CreateCoercingStaticSetter<long>(memberInfo!), coercion),
            FactoryOperation.SetterByRef => SetByRefFactoryValue(useTryCreate ? Try(MemberAccessorFactory.TryCreateSetterByRef<Point, long>(memberInfo, out var value), value) : MemberAccessorFactory.CreateSetterByRef<Point, long>(memberInfo!)),
            FactoryOperation.CoercingSetterByRef => SetCoercingByRefFactoryValue(useTryCreate ? Try(MemberAccessorFactory.TryCreateCoercingSetterByRef<Point, string>(memberInfo, out var value), value) : MemberAccessorFactory.CreateCoercingSetterByRef<Point, string>(memberInfo!), coercion),
            _ => throw new ArgumentOutOfRangeException(nameof(operation)),
        };
    }

    private static TDelegate? Try<TDelegate>(bool success, TDelegate? value) where TDelegate : Delegate => success ? value : null;

    private static (bool, object?) GetFactoryValue(Func<object, object?>? getter, object target) => (getter is not null, getter?.Invoke(target));
    private static (bool, object?) GetCoercingFactoryValue(Func<object, Type, TypeCoercion, TypeCoercionContext?, object?>? getter, object target, TypeCoercion coercion) => (getter is not null, getter?.Invoke(target, typeof(long), coercion, null));
    private static (bool, object?) GetGenericFactoryValue(Func<MemberAccessShape, string?>? getter, MemberAccessShape target) => (getter is not null, getter?.Invoke(target));
    private static (bool, object?) GetCoercingGenericFactoryValue(Func<MemberAccessShape, TypeCoercion, TypeCoercionContext?, long>? getter, MemberAccessShape target, TypeCoercion coercion) => (getter is not null, getter?.Invoke(target, coercion, null));
    private static (bool, object?) SetFactoryValue(Action<object, object?>? setter, MemberAccessShape target) { setter?.Invoke(target, "object-set"); return (setter is not null, setter is null ? null : target.TextValue); }
    private static (bool, object?) SetCoercingFactoryValue(Action<object, object?, TypeCoercion, TypeCoercionContext?>? setter, MemberAccessShape target, TypeCoercion coercion) { setter?.Invoke(target, 2L, coercion, null); return (setter is not null, setter is null ? null : target.TextValue); }
    private static (bool, object?) SetGenericFactoryValue(Action<MemberAccessShape, string?>? setter, MemberAccessShape target) { setter?.Invoke(target, "generic-set"); return (setter is not null, setter is null ? null : target.TextValue); }
    private static (bool, object?) SetCoercingGenericFactoryValue(Action<MemberAccessShape, long, TypeCoercion, TypeCoercionContext?>? setter, MemberAccessShape target, TypeCoercion coercion) { setter?.Invoke(target, 4L, coercion, null); return (setter is not null, setter is null ? null : target.TextValue); }
    private static (bool, object?) GetStaticFactoryValue(Func<object?>? getter) => (getter is not null, getter?.Invoke());
    private static (bool, object?) GetCoercingStaticFactoryValue(Func<Type, TypeCoercion, TypeCoercionContext?, object?>? getter, TypeCoercion coercion) => (getter is not null, getter?.Invoke(typeof(long), coercion, null));
    private static (bool, object?) GetGenericStaticFactoryValue(Func<string?>? getter) => (getter is not null, getter?.Invoke());
    private static (bool, object?) GetCoercingGenericStaticFactoryValue(Func<TypeCoercion, TypeCoercionContext?, long>? getter, TypeCoercion coercion) => (getter is not null, getter?.Invoke(coercion, null));
    private static (bool, object?) SetStaticFactoryValue(Action<object?>? setter) { setter?.Invoke("static-set"); return (setter is not null, setter is null ? null : MemberAccessShape.StaticTextValue); }
    private static (bool, object?) SetCoercingStaticFactoryValue(Action<object?, TypeCoercion, TypeCoercionContext?>? setter, TypeCoercion coercion) { setter?.Invoke(6L, coercion, null); return (setter is not null, setter is null ? null : MemberAccessShape.StaticTextValue); }
    private static (bool, object?) SetGenericStaticFactoryValue(Action<string?>? setter) { setter?.Invoke("generic-static-set"); return (setter is not null, setter is null ? null : MemberAccessShape.StaticTextValue); }
    private static (bool, object?) SetCoercingGenericStaticFactoryValue(Action<long, TypeCoercion, TypeCoercionContext?>? setter, TypeCoercion coercion) { setter?.Invoke(8L, coercion, null); return (setter is not null, setter is null ? null : MemberAccessShape.StaticTextValue); }
    private static (bool, object?) SetByRefFactoryValue(MemberSetterByRef<Point, long>? setter)
    {
        var point = new Point();
        if (setter is not null)
        {
            setter(ref point, 9L);
        }
        return (setter is not null, setter is null ? null : point.X);
    }

    private static (bool, object?) SetCoercingByRefFactoryValue
    (
        CoercingMemberSetterByRef<Point, string>? setter,
        TypeCoercion coercion
    )
    {
        var point = new Point();
        if (setter is not null)
        {
            setter(ref point, "10", coercion, null);
        }
        return (setter is not null, setter is null ? null : point.X);
    }
    #endregion
}
