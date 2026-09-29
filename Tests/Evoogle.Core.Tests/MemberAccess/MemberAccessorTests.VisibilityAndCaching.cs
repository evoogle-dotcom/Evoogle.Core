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
    private enum VisibilityAccessOperation
    {
        ReadPublicGetter,
        ReadPrivateGetter,
        WritePublicSetter,
        WritePrivateSetter,
    }

    private enum CacheIdentityOperation
    {
        ExactAndPublic,
        ExactAndCombined,
        PublicAndNonPublic,
        FactoryGetter,
        FactoryGenericGetter,
        FactoryStaticGetter,
        FactorySetterByRef,
    }

    private enum ConcurrentCacheOperation
    {
        Accessor,
        StaticAccessor,
        Getter,
        GenericGetter,
        CoercingGetter,
        CoercingGenericGetter,
        StaticGetter,
        GenericStaticGetter,
        CoercingStaticGetter,
        CoercingGenericStaticGetter,
    }
    #endregion

    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] PropertyVisibilityTheoryData =>
    [
        VisibilityCase(nameof(MemberAccessShape.PrivateSetterValue), BindingFlags.Public | BindingFlags.Instance, canRead: true, canWrite: false),
        VisibilityCase(nameof(MemberAccessShape.PrivateSetterValue), BindingFlags.NonPublic | BindingFlags.Instance, canRead: false, canWrite: true),
        VisibilityCase(nameof(MemberAccessShape.PrivateSetterValue), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, canRead: true, canWrite: true),
        VisibilityCase(nameof(MemberAccessShape.PrivateGetterValue), BindingFlags.Public | BindingFlags.Instance, canRead: false, canWrite: true),
        VisibilityCase(nameof(MemberAccessShape.PrivateGetterValue), BindingFlags.NonPublic | BindingFlags.Instance, canRead: true, canWrite: false),
        VisibilityCase(nameof(MemberAccessShape.PrivateGetterValue), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, canRead: true, canWrite: true),
    ];

    public static TheoryDataRow<IXUnitTest>[] PropertyVisibilityAccessTheoryData =>
    [
        VisibilityAccessCase(VisibilityAccessOperation.ReadPublicGetter, "private-setter"),
        VisibilityAccessCase(VisibilityAccessOperation.ReadPrivateGetter, "private-getter"),
        VisibilityAccessCase(VisibilityAccessOperation.WritePublicSetter, "public-set"),
        VisibilityAccessCase(VisibilityAccessOperation.WritePrivateSetter, "private-set"),
    ];

    public static TheoryDataRow<IXUnitTest>[] PropertyLookupTheoryData =>
    [
        new PropertyLookupTest { Name = $"{nameof(MemberAccessor.TryCreateProperty)} finds inherited property", MemberName = nameof(MemberAccessShape.InheritedValue), BindingFlags = BindingFlags.Public | BindingFlags.Instance, ExpectedSuccess = true },
        new PropertyLookupTest { Name = $"{nameof(MemberAccessor.TryCreateProperty)} excludes inherited property with {nameof(BindingFlags.DeclaredOnly)}", MemberName = nameof(MemberAccessShape.InheritedValue), BindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly, ExpectedSuccess = false },
        new PropertyLookupTest { Name = $"{nameof(MemberAccessor.TryCreateProperty)} honors {nameof(BindingFlags.IgnoreCase)}", MemberName = nameof(MemberAccessShape.TextValue).ToUpperInvariant(), BindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase, ExpectedSuccess = true },
    ];

    public static TheoryDataRow<IXUnitTest>[] CacheIdentityTheoryData =>
    [
        IdentityCase(CacheIdentityOperation.ExactAndPublic, expectedSame: true),
        IdentityCase(CacheIdentityOperation.ExactAndCombined, expectedSame: true),
        IdentityCase(CacheIdentityOperation.PublicAndNonPublic, expectedSame: false),
        IdentityCase(CacheIdentityOperation.FactoryGetter, expectedSame: true),
        IdentityCase(CacheIdentityOperation.FactoryGenericGetter, expectedSame: true),
        IdentityCase(CacheIdentityOperation.FactoryStaticGetter, expectedSame: true),
        IdentityCase(CacheIdentityOperation.FactorySetterByRef, expectedSame: true),
    ];

    public static TheoryDataRow<IXUnitTest>[] CacheConcurrencyTheoryData =>
        Enum.GetValues<ConcurrentCacheOperation>()
            .Select(static operation => new TheoryDataRow<IXUnitTest>(new CacheConcurrencyTest
            {
                Name = $"{nameof(MemberAccessor)} cache publishes one {operation} identity during concurrent access",
                Operation = operation,
            }))
            .ToArray();
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(PropertyVisibilityTheoryData))]
    public void PropertyVisibility(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(PropertyVisibilityAccessTheoryData))]
    public void PropertyVisibilityAccess(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(PropertyLookupTheoryData))]
    public void PropertyLookup(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(CacheIdentityTheoryData))]
    public void CacheIdentity(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(CacheConcurrencyTheoryData))]
    public void CacheConcurrency(IXUnitTest test) => test.Execute(this);
    #endregion

    #region Tests
    private sealed class PropertyVisibilityTest : XUnitTest
    {
        public required string MemberName { get; init; }

        public required BindingFlags BindingFlags { get; init; }

        public required bool ExpectedCanRead { get; init; }

        public required bool ExpectedCanWrite { get; init; }

        private MemberAccessor? Accessor { get; set; }

        protected override void Act() => this.Accessor = MemberAccessor.CreateProperty(typeof(MemberAccessShape), this.MemberName, this.BindingFlags);

        protected override void Assert()
        {
            this.Accessor.Should().NotBeNull();
            this.Accessor!.CanRead.Should().Be(this.ExpectedCanRead);
            this.Accessor.CanWrite.Should().Be(this.ExpectedCanWrite);
        }
    }

    private sealed class PropertyVisibilityAccessTest : XUnitTest
    {
        public required VisibilityAccessOperation Operation { get; init; }

        public required string ExpectedValue { get; init; }

        private string? ActualValue { get; set; }

        protected override void Act()
        {
            var instance = new MemberAccessShape();
            switch (this.Operation)
            {
                case VisibilityAccessOperation.ReadPublicGetter:
                    this.ActualValue = MemberAccessor.CreateProperty(typeof(MemberAccessShape), nameof(MemberAccessShape.PrivateSetterValue), BindingFlags.Public | BindingFlags.Instance).GetValue<MemberAccessShape, string>(instance);
                    break;
                case VisibilityAccessOperation.ReadPrivateGetter:
                    this.ActualValue = MemberAccessor.CreateProperty(typeof(MemberAccessShape), nameof(MemberAccessShape.PrivateGetterValue), BindingFlags.NonPublic | BindingFlags.Instance).GetValue<MemberAccessShape, string>(instance);
                    break;
                case VisibilityAccessOperation.WritePublicSetter:
                    MemberAccessor.CreateProperty(typeof(MemberAccessShape), nameof(MemberAccessShape.PrivateGetterValue), BindingFlags.Public | BindingFlags.Instance).SetValue(instance, "public-set");
                    this.ActualValue = MemberAccessor.Create(typeof(MemberAccessShape).GetProperty(nameof(MemberAccessShape.PrivateGetterValue))!).GetValue<MemberAccessShape, string>(instance);
                    break;
                case VisibilityAccessOperation.WritePrivateSetter:
                    MemberAccessor.CreateProperty(typeof(MemberAccessShape), nameof(MemberAccessShape.PrivateSetterValue), BindingFlags.NonPublic | BindingFlags.Instance).SetValue(instance, "private-set");
                    this.ActualValue = instance.PrivateSetterValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(this.Operation));
            }
        }

        protected override void Assert() => this.ActualValue.Should().Be(this.ExpectedValue);
    }

    private sealed class PropertyLookupTest : XUnitTest
    {
        public required string MemberName { get; init; }

        public required BindingFlags BindingFlags { get; init; }

        public required bool ExpectedSuccess { get; init; }

        private bool ActualSuccess { get; set; }

        protected override void Act()
            => this.ActualSuccess = MemberAccessor.TryCreateProperty(typeof(MemberAccessShape), this.MemberName, out _, this.BindingFlags);

        protected override void Assert() => this.ActualSuccess.Should().Be(this.ExpectedSuccess);
    }

    private sealed class CacheIdentityTest : XUnitTest
    {
        public required CacheIdentityOperation Operation { get; init; }

        public required bool ExpectedSame { get; init; }

        private object? First { get; set; }

        private object? Second { get; set; }

        protected override void Act()
        {
            var property = typeof(MemberAccessShape).GetProperty(nameof(MemberAccessShape.PrivateSetterValue))!;
            switch (this.Operation)
            {
                case CacheIdentityOperation.ExactAndPublic:
                    property = typeof(MemberAccessShape).GetProperty(nameof(MemberAccessShape.TextValue))!;
                    this.First = MemberAccessor.Create(property);
                    this.Second = MemberAccessor.CreateProperty(typeof(MemberAccessShape), nameof(MemberAccessShape.TextValue));
                    break;
                case CacheIdentityOperation.ExactAndCombined:
                    this.First = MemberAccessor.Create(property);
                    this.Second = MemberAccessor.CreateProperty(typeof(MemberAccessShape), nameof(MemberAccessShape.PrivateSetterValue), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    break;
                case CacheIdentityOperation.PublicAndNonPublic:
                    this.First = MemberAccessor.CreateProperty(typeof(MemberAccessShape), nameof(MemberAccessShape.PrivateSetterValue), BindingFlags.Public | BindingFlags.Instance);
                    this.Second = MemberAccessor.CreateProperty(typeof(MemberAccessShape), nameof(MemberAccessShape.PrivateSetterValue), BindingFlags.NonPublic | BindingFlags.Instance);
                    break;
                case CacheIdentityOperation.FactoryGetter:
                    property = typeof(MemberAccessShape).GetProperty(nameof(MemberAccessShape.TextValue))!;
                    this.First = MemberAccessorFactory.CreateGetter(property);
                    this.Second = MemberAccessorFactory.CreateGetter(property);
                    break;
                case CacheIdentityOperation.FactoryGenericGetter:
                    property = typeof(MemberAccessShape).GetProperty(nameof(MemberAccessShape.TextValue))!;
                    this.First = MemberAccessorFactory.CreateGetter<MemberAccessShape, string>(property);
                    this.Second = MemberAccessorFactory.CreateGetter<MemberAccessShape, string>(property);
                    break;
                case CacheIdentityOperation.FactoryStaticGetter:
                    property = typeof(MemberAccessShape).GetProperty(nameof(MemberAccessShape.StaticTextValue))!;
                    this.First = MemberAccessorFactory.CreateStaticGetter(property);
                    this.Second = MemberAccessorFactory.CreateStaticGetter(property);
                    break;
                case CacheIdentityOperation.FactorySetterByRef:
                    var field = typeof(Point).GetField(nameof(Point.X))!;
                    this.First = MemberAccessorFactory.CreateSetterByRef<Point, long>(field);
                    this.Second = MemberAccessorFactory.CreateSetterByRef<Point, long>(field);
                    break;
            }
        }

        protected override void Assert()
        {
            if (this.ExpectedSame)
            {
                this.First.Should().BeSameAs(this.Second);
            }
            else
            {
                this.First.Should().NotBeSameAs(this.Second);
            }
        }
    }

    private sealed class CacheConcurrencyTest : XUnitTest
    {
        public required ConcurrentCacheOperation Operation { get; init; }

        private object[]? Identities { get; set; }

        private object?[]? Values { get; set; }

        protected override void Act()
        {
            var start = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var tasks = Enumerable.Range(0, 32)
                .Select(_ => InvokeConcurrentCacheWhenReleased(start.Task, this.Operation))
                .ToArray();
            start.SetResult();
            var results = Task.WhenAll(tasks).GetAwaiter().GetResult();
            this.Identities = results.Select(static result => result.Identity).ToArray();
            this.Values = results.Select(static result => result.Value).ToArray();
        }

        protected override void Assert()
        {
            this.Identities.Should().OnlyContain(identity => ReferenceEquals(identity, this.Identities![0]));
            this.Values.Should().OnlyContain(value => value != null && value.ToString() == ExpectedConcurrentValue(this.Operation));
        }
    }
    #endregion

    #region Theory Data Methods
    private static PropertyVisibilityTest VisibilityCase(string memberName, BindingFlags bindingFlags, bool canRead, bool canWrite) => new()
    {
        Name = $"{nameof(MemberAccessor)} {memberName} with {bindingFlags} has CanRead={canRead} and CanWrite={canWrite}",
        MemberName = memberName,
        BindingFlags = bindingFlags,
        ExpectedCanRead = canRead,
        ExpectedCanWrite = canWrite,
    };

    private static PropertyVisibilityAccessTest VisibilityAccessCase(VisibilityAccessOperation operation, string expectedValue) => new()
    {
        Name = $"{nameof(MemberAccessor)} {operation} succeeds through the permitted accessor",
        Operation = operation,
        ExpectedValue = expectedValue,
    };

    private static CacheIdentityTest IdentityCase(CacheIdentityOperation operation, bool expectedSame) => new()
    {
        Name = $"{nameof(MemberAccessor)} {operation} returns {(expectedSame ? "the same" : "a different")} cached identity",
        Operation = operation,
        ExpectedSame = expectedSame,
    };
    #endregion

    #region Implementation Methods
    private static async Task<(object Identity, object? Value)> InvokeConcurrentCacheWhenReleased
    (
        Task start,
        ConcurrentCacheOperation operation
    )
    {
        await start.ConfigureAwait(false);
        return InvokeConcurrentCache(operation);
    }

    private static (object Identity, object? Value) InvokeConcurrentCache(ConcurrentCacheOperation operation)
    {
        var instanceProperty = typeof(MemberAccessShape).GetProperty(nameof(MemberAccessShape.ConcurrentCacheValue))!;
        var staticProperty = typeof(MemberAccessShape).GetProperty(nameof(MemberAccessShape.StaticConcurrentCacheValue))!;
        var instance = new MemberAccessShape();
        var coercion = new TypeCoercion();

        return operation switch
        {
            ConcurrentCacheOperation.Accessor => AccessorResult
            (
                MemberAccessor.CreateProperty
                (
                    typeof(MemberAccessShape),
                    nameof(MemberAccessShape.ConcurrentAccessorCacheValue)
                ),
                instance
            ),
            ConcurrentCacheOperation.StaticAccessor => StaticAccessorResult
            (
                MemberAccessor.CreateProperty
                (
                    typeof(MemberAccessShape),
                    nameof(MemberAccessShape.StaticConcurrentAccessorCacheValue),
                    BindingFlags.Public | BindingFlags.Static
                )
            ),
            ConcurrentCacheOperation.Getter => DelegateResult(MemberAccessorFactory.CreateGetter(instanceProperty), instance),
            ConcurrentCacheOperation.GenericGetter => DelegateResult(MemberAccessorFactory.CreateGetter<MemberAccessShape, long>(instanceProperty), instance),
            ConcurrentCacheOperation.CoercingGetter => DelegateResult(MemberAccessorFactory.CreateCoercingGetter(instanceProperty), instance, coercion),
            ConcurrentCacheOperation.CoercingGenericGetter => DelegateResult(MemberAccessorFactory.CreateCoercingGetter<MemberAccessShape, string>(instanceProperty), instance, coercion),
            ConcurrentCacheOperation.StaticGetter => DelegateResult(MemberAccessorFactory.CreateStaticGetter(staticProperty)),
            ConcurrentCacheOperation.GenericStaticGetter => DelegateResult(MemberAccessorFactory.CreateStaticGetter<long>(staticProperty)),
            ConcurrentCacheOperation.CoercingStaticGetter => DelegateResult(MemberAccessorFactory.CreateCoercingStaticGetter(staticProperty), coercion),
            ConcurrentCacheOperation.CoercingGenericStaticGetter => DelegateResult(MemberAccessorFactory.CreateCoercingStaticGetter<string>(staticProperty), coercion),
            _ => throw new ArgumentOutOfRangeException(nameof(operation)),
        };
    }

    private static string ExpectedConcurrentValue(ConcurrentCacheOperation operation)
        => operation switch
        {
            ConcurrentCacheOperation.Accessor => "26",
            ConcurrentCacheOperation.StaticAccessor => "27",
            ConcurrentCacheOperation.StaticGetter or
            ConcurrentCacheOperation.GenericStaticGetter or
            ConcurrentCacheOperation.CoercingStaticGetter or
            ConcurrentCacheOperation.CoercingGenericStaticGetter => "25",
            _ => "24",
        };

    private static (object, object?) AccessorResult(MemberAccessor accessor, MemberAccessShape instance) => (accessor, accessor.GetValue(instance));
    private static (object, object?) StaticAccessorResult(MemberAccessor accessor) => (accessor, accessor.GetStaticValue());
    private static (object, object?) DelegateResult(Func<object, object?> getter, object target) => (getter, getter(target));
    private static (object, object?) DelegateResult(Func<MemberAccessShape, long> getter, MemberAccessShape target) => (getter, getter(target));
    private static (object, object?) DelegateResult(Func<object, Type, TypeCoercion, TypeCoercionContext?, object?> getter, object target, TypeCoercion coercion) => (getter, getter(target, typeof(string), coercion, null));
    private static (object, object?) DelegateResult(Func<MemberAccessShape, TypeCoercion, TypeCoercionContext?, string?> getter, MemberAccessShape target, TypeCoercion coercion) => (getter, getter(target, coercion, null));
    private static (object, object?) DelegateResult(Func<object?> getter) => (getter, getter());
    private static (object, object?) DelegateResult(Func<long> getter) => (getter, getter());
    private static (object, object?) DelegateResult(Func<Type, TypeCoercion, TypeCoercionContext?, object?> getter, TypeCoercion coercion) => (getter, getter(typeof(string), coercion, null));
    private static (object, object?) DelegateResult(Func<TypeCoercion, TypeCoercionContext?, string?> getter, TypeCoercion coercion) => (getter, getter(coercion, null));
    #endregion
}
