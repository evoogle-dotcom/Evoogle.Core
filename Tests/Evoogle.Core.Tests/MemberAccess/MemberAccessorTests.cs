// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Reflection;

using Evoogle.Coercion;
using Evoogle.Extensions;
using Evoogle.XUnit;

using FluentAssertions;

namespace Evoogle.MemberAccess;

public partial class MemberAccessorTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Base Tests
    private abstract class TryTestBase : XUnitTest
    {
        #region User Supplied Properties
        public required Type DeclaringType { get; init; }

        public required string MemberName { get; init; }

        public required BindingFlags BindingFlags { get; init; }

        public bool ShouldCoerce { get; init; }
        #endregion

        #region Calculated Properties
        protected MemberAccessor? Accessor { get; set; }

        protected TypeCoercion? Coercion { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            ValidateBindingFlags(this.BindingFlags);

            var memberInfo = (MemberInfo?)this.DeclaringType.GetProperty(this.MemberName, this.BindingFlags)
                ?? this.DeclaringType.GetField(this.MemberName, this.BindingFlags)
                ?? throw new InvalidOperationException(
                    $"Member '{this.MemberName}' was not found on '{this.DeclaringType}'.");

            this.Accessor = MemberAccessor.Create(memberInfo);
            this.Coercion = this.ShouldCoerce ? new TypeCoercion() : null;

            this.WriteLine($"DeclaringType: {this.DeclaringType.SafeToName()}");
            this.WriteLine($"MemberName:    {this.MemberName.SafeToString()}");
            this.WriteLine($"BindingFlags:  {this.BindingFlags.SafeToString()}");
            this.WriteLine($"ShouldCoerce:  {this.ShouldCoerce.SafeToString()}");
            this.WriteLine();
        }
        #endregion
    }

    private abstract class TryGetTestBase : TryTestBase
    {
        #region User Supplied Properties
        public required bool ExpectedSuccess { get; init; }
        #endregion

        #region Calculated Properties
        protected bool? ActualSuccess { get; set; }
        #endregion
    }

    private abstract class TrySetTestBase : TryTestBase
    {
        #region User Supplied Properties
        public required bool ExpectedTrySetSuccess { get; set; }
        #endregion

        #region Calculated Properties
        protected bool? ActualTrySetSuccess { get; set; }
        #endregion
    }

    private abstract class StaticTryTestBase : XUnitTest
    {
        #region User Supplied Properties
        public required Type DeclaringType { get; init; }

        public required string MemberName { get; init; }

        public required BindingFlags BindingFlags { get; init; }

        public bool ShouldCoerce { get; init; }

        public object? InitialClrValue { get; init; }
        #endregion

        #region Calculated Properties
        protected MemberAccessor? Accessor { get; set; }

        protected TypeCoercion? Coercion { get; set; }

        private MemberInfo? MemberInfo { get; set; }

        private object? MemberTarget { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            ValidateBindingFlags(this.BindingFlags);
            var isStaticMember = this.BindingFlags.HasFlag(BindingFlags.Static);

            this.MemberInfo = (MemberInfo?)
                this.DeclaringType.GetProperty(this.MemberName, this.BindingFlags)
                ?? this.DeclaringType.GetField(this.MemberName, this.BindingFlags)
                ?? throw new InvalidOperationException(
                    $"Member '{this.MemberName}' was not found on '{this.DeclaringType}'.");

            this.MemberTarget = isStaticMember
                ? null
                : Activator.CreateInstance(this.DeclaringType);
            this.SetMemberValue(this.InitialClrValue);

            this.Accessor = MemberAccessor.Create(this.MemberInfo);
            this.Coercion = this.ShouldCoerce ? new TypeCoercion() : null;

            this.WriteLine($"DeclaringType:   {this.DeclaringType.SafeToName()}");
            this.WriteLine($"MemberName:      {this.MemberName.SafeToString()}");
            this.WriteLine($"BindingFlags:    {this.BindingFlags.SafeToString()}");
            this.WriteLine($"ShouldCoerce:    {this.ShouldCoerce.SafeToString()}");
            this.WriteLine($"InitialClrValue: {this.InitialClrValue.SafeToString()}");
            this.WriteLine();
        }
        #endregion

        #region Implementation Methods
        protected object? GetMemberValue()
            => this.MemberInfo switch
            {
                PropertyInfo propertyInfo => propertyInfo.GetValue(this.MemberTarget),
                FieldInfo fieldInfo => fieldInfo.GetValue(this.MemberTarget),
                _ => throw new InvalidOperationException("The test member is unavailable.")
            };

        private void SetMemberValue(object? value)
        {
            switch (this.MemberInfo)
            {
                case PropertyInfo propertyInfo:
                    propertyInfo.SetValue(this.MemberTarget, value);
                    break;

                case FieldInfo fieldInfo:
                    fieldInfo.SetValue(this.MemberTarget, value);
                    break;

                default:
                    throw new InvalidOperationException("The test member is unavailable.");
            }
        }
        #endregion
    }

    private abstract class StaticTryGetTestBase : StaticTryTestBase
    {
        #region User Supplied Properties
        public required bool ExpectedSuccess { get; init; }
        #endregion

        #region Calculated Properties
        protected bool? ActualSuccess { get; set; }
        #endregion
    }

    private abstract class StaticTrySetTestBase : StaticTryTestBase
    {
        #region User Supplied Properties
        public required bool ExpectedSuccess { get; init; }

        public object? ExpectedClrValue { get; init; }
        #endregion

        #region Calculated Properties
        protected bool? ActualSuccess { get; set; }

        private object? ActualClrValue { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            base.Arrange();

            this.WriteLine($"ExpectedSuccess:  {this.ExpectedSuccess.SafeToString()}");
            this.WriteLine($"ExpectedClrValue: {this.ExpectedClrValue.SafeToString()}");
        }

        protected override void Act()
        {
            this.ActualSuccess = this.TrySetValue();
            this.ActualClrValue = this.GetMemberValue();

            this.WriteLine($"ActualSuccess:  {this.ActualSuccess.SafeToString()}");
            this.WriteLine($"ActualClrValue: {this.ActualClrValue.SafeToString()}");
        }

        protected override void Assert()
        {
            this.ActualSuccess.Should().Be(this.ExpectedSuccess);
            this.ActualClrValue.Should().BeEquivalentTo(
                this.ExpectedSuccess ? this.ExpectedClrValue : this.InitialClrValue);
        }
        #endregion

        #region Abstract Methods
        protected abstract bool TrySetValue();
        #endregion
    }
    #endregion

    #region Validation Methods
    private static void ValidateBindingFlags(BindingFlags bindingFlags)
    {
        var includesInstance = bindingFlags.HasFlag(BindingFlags.Instance);
        var includesStatic = bindingFlags.HasFlag(BindingFlags.Static);
        if (includesInstance == includesStatic)
        {
            throw new InvalidOperationException(
                $"Binding flags must include exactly one of {nameof(BindingFlags.Instance)} or " +
                $"{nameof(BindingFlags.Static)}.");
        }
    }
    #endregion

    #region Factory Tests
    private sealed class FactoryTest : XUnitTest
    {
        #region User Supplied Properties
        public required Type DeclaringType { get; init; }
        public required Type MemberType { get; init; }
        public required string MemberName { get; init; }
        public BindingFlags? BindingFlags { get; init; }
        public bool? IsProperty { get; init; }
        public bool? IsField { get; init; }
        public bool? IsStatic { get; init; }
        public bool? CanRead { get; init; }
        public bool? CanWrite { get; init; }
        public Type? ExceptionType { get; init; }
        #endregion

        #region Calculated Properties
        private MemberAccessor? ExactAccessor { get; set; }
        private MemberAccessor? NamedAccessor { get; set; }
        private MemberAccessor? TryNamedAccessor { get; set; }
        private Type? ActualExceptionType { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            this.WriteLine($"DeclaringType: {this.DeclaringType.SafeToName()}");
            this.WriteLine($"MemberType:    {this.MemberType.SafeToName()}");
            this.WriteLine($"MemberName:    {this.MemberName.SafeToString()}");
            this.WriteLine($"BindingFlags:  {this.BindingFlags.SafeToString()}");
            this.WriteLine($"IsProperty:    {this.IsProperty.SafeToString()}");
            this.WriteLine($"IsField:       {this.IsField.SafeToString()}");
            this.WriteLine($"IsStatic:      {this.IsStatic.SafeToString()}");
            this.WriteLine($"CanRead:       {this.CanRead.SafeToString()}");
            this.WriteLine($"CanWrite:      {this.CanWrite.SafeToString()}");
            this.WriteLine($"ExceptionType: {this.ExceptionType.SafeToName()}");
            this.WriteLine();
        }

        protected override void Act()
        {
            try
            {
                var bindingFlags = this.BindingFlags ??
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;

                if (this.IsField.HasValue && this.IsField.Value == true)
                {
                    var propertyAccessor = MemberAccessor.CreateField(
                        this.DeclaringType!,
                        this.MemberName!,
                        bindingFlags);
                    var tryFieldAccessor = MemberAccessor.TryCreateField(
                        this.DeclaringType!,
                        this.MemberName!,
                        out var tryNamedAccessor,
                        bindingFlags)
                        ? tryNamedAccessor
                        : null;
                }

                if (this.IsProperty.HasValue && this.IsProperty.Value == true)
                {
                    var propertyAccessor = MemberAccessor.CreateProperty(
                        this.DeclaringType!,
                        this.MemberName!,
                        bindingFlags);
                    var tryPropertyAccessor = MemberAccessor.TryCreateProperty(
                        this.DeclaringType!,
                        this.MemberName!,
                        out var tryNamedAccessor,
                        bindingFlags)
                        ? tryNamedAccessor
                        : null;
                }

                var fieldInfo = (MemberInfo?)Field(this.DeclaringType, this.MemberName, bindingFlags);
                var propertyInfo = (MemberInfo?)Property(this.DeclaringType, this.MemberName, bindingFlags);
                var methodInfo = (MemberInfo?)Method(this.DeclaringType, this.MemberName, bindingFlags);
                var memberInfo = fieldInfo ?? propertyInfo ?? methodInfo;

                this.ExactAccessor = MemberAccessor.Create(memberInfo!);
                if (fieldInfo != null)
                {
                    this.NamedAccessor = MemberAccessor.Create(fieldInfo);
                    this.TryNamedAccessor = MemberAccessor.TryCreate(fieldInfo, out var tryNamedAccessor)
                        ? tryNamedAccessor
                        : null;
                }
                else if (propertyInfo != null)
                {
                    this.NamedAccessor = MemberAccessor.Create(propertyInfo);
                    this.TryNamedAccessor = MemberAccessor.TryCreate(propertyInfo, out var tryNamedAccessor)
                        ? tryNamedAccessor
                        : null;
                }
            }
            catch (Exception ex)
            {
                this.ActualExceptionType = ex.GetType();
            }
        }

        protected override void Assert()
        {
            if (this.ExceptionType == null)
            {
                this.ActualExceptionType.Should().BeNull();

                // Expected factory methods to have succeeded
                this.ExactAccessor.Should().NotBeNull();
                this.NamedAccessor.Should().NotBeNull();
                this.TryNamedAccessor.Should().NotBeNull();

                // Exact, Named, and TryNamed accessors should refer to the same underlying object.
                this.ExactAccessor.Should().BeSameAs(this.NamedAccessor);
                this.ExactAccessor.Should().BeSameAs(this.TryNamedAccessor);

                // Exact, Named, and TryNamed accessors should have the expected metadata.
                this.ExactAccessor.DeclaringType.Should().Be(this.DeclaringType);
                this.ExactAccessor.MemberType.Should().Be(this.MemberType);
                this.ExactAccessor.MemberName.Should().Be(this.MemberName);

                if (this.IsProperty.HasValue)
                {
                    this.ExactAccessor.IsProperty.Should().Be(this.IsProperty.Value);
                }
                if (this.IsField.HasValue)
                {
                    this.ExactAccessor.IsField.Should().Be(this.IsField.Value);
                }
                if (this.IsStatic.HasValue)
                {
                    this.ExactAccessor.IsStatic.Should().Be(this.IsStatic.Value);
                }
                if (this.CanRead.HasValue)
                {
                    this.ExactAccessor.CanRead.Should().Be(this.CanRead.Value);
                }
                if (this.CanWrite.HasValue)
                {
                    this.ExactAccessor.CanWrite.Should().Be(this.CanWrite.Value);
                }
            }
            else
            {
                this.ActualExceptionType.Should().NotBeNull();
                this.ActualExceptionType.Should().Be(this.ExceptionType);
            }
        }
        #endregion

        #region Reflection Methods
        private static FieldInfo? Field(Type declaringType, string memberName, BindingFlags bindingFlags)
            => declaringType.GetField(memberName, bindingFlags);

        private static PropertyInfo? Property(Type declaringType, string memberName, BindingFlags bindingFlags)
            => declaringType.GetProperty(memberName, bindingFlags);

        private static MethodInfo? Method(Type declaringType, string memberName, BindingFlags bindingFlags)
            => declaringType.GetMethod(memberName, bindingFlags);
        #endregion
    }
    #endregion

    #region TryGet Tests
    private sealed class TryGetGenericTest<TObject, TValue> : TryGetTestBase
    {
        #region User Supplied Properties
        public TObject? ClrObject { get; init; }

        public TValue? ExpectedClrValue { get; init; }
        #endregion

        #region Calculated Properties
        private TValue? ActualClrValue { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            base.Arrange();

            this.WriteLine($"ClrObject: {this.ClrObject.SafeToString()}");
            this.WriteLine();
            this.WriteLine($"ExpectedSuccess:  {this.ExpectedSuccess.SafeToString()}");
            this.WriteLine($"ExpectedClrValue: {this.ExpectedClrValue.SafeToString()}");
        }

        protected override void Act()
        {
            this.ActualSuccess = this.Accessor!.TryGetValue<TObject, TValue>
            (
                this.ClrObject!,
                out var clrValue,
                this.Coercion
            );
            this.ActualClrValue = clrValue;

            this.WriteLine();
            this.WriteLine($"ActualSuccess:  {this.ActualSuccess.SafeToString()}");
            this.WriteLine($"ActualClrValue: {this.ActualClrValue.SafeToString()}");
        }

        protected override void Assert()
        {
            this.ActualSuccess.Should().Be(this.ExpectedSuccess);
            if (this.ExpectedSuccess)
            {
                this.ActualClrValue.Should().BeEquivalentTo(this.ExpectedClrValue);
            }
            else
            {
                this.ActualClrValue.Should().Be(default(TValue));
            }
        }
        #endregion
    }

    private sealed class TryGetNonGenericTest : TryGetTestBase
    {
        #region User Supplied Properties
        public object? ClrObject { get; init; }

        public Type? ClrValueType { get; init; }

        public object? ExpectedClrValue { get; init; }
        #endregion

        #region Calculated Properties
        private object? ActualClrValue { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            base.Arrange();

            this.WriteLine($"ClrObject:    {this.ClrObject.SafeToString()}");
            this.WriteLine($"ClrValueType: {this.ClrValueType.SafeToName()}");
            this.WriteLine();
            this.WriteLine($"ExpectedSuccess:  {this.ExpectedSuccess.SafeToString()}");
            this.WriteLine($"ExpectedClrValue: {this.ExpectedClrValue.SafeToString()}");
        }

        protected override void Act()
        {
            this.ActualSuccess = this.Accessor!.TryGetValue
            (
                this.ClrObject,
                out var clrValue,
                this.ClrValueType,
                this.Coercion
            );
            this.ActualClrValue = clrValue;

            this.WriteLine();
            this.WriteLine($"ActualSuccess:  {this.ActualSuccess.SafeToString()}");
            this.WriteLine($"ActualClrValue: {this.ActualClrValue.SafeToString()}");
        }

        protected override void Assert()
        {
            this.ActualSuccess.Should().Be(this.ExpectedSuccess);
            if (this.ExpectedSuccess)
            {
                this.ActualClrValue.Should().BeEquivalentTo(this.ExpectedClrValue);
            }
            else
            {
                this.ActualClrValue.Should().BeNull();
            }
        }
        #endregion
    }
    #endregion

    #region TrySet Tests
    private sealed class TrySetGenericTest<TObject, TValue> : TrySetTestBase
    {
        #region User Supplied Properties
        public TObject? ClrObject { get; init; }

        public TValue? ClrValue { get; init; }

        public TObject? ExpectedTrySetClrObject { get; init; }
        #endregion

        #region Calculated Properties
        private TObject? ActualTrySetClrObject { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            base.Arrange();

            this.WriteLine($"ClrObject: {this.ClrObject.SafeToString()}");
            this.WriteLine($"ClrValue:  {this.ClrValue.SafeToString()}");
            this.WriteLine();
            this.WriteLine($"ExpectedTrySetSuccess:   {this.ExpectedTrySetSuccess.SafeToString()}");
            this.WriteLine($"ExpectedTrySetClrObject: {this.ExpectedTrySetClrObject.SafeToString()}");
        }

        protected override void Act()
        {
            this.ActualTrySetSuccess = this.Accessor!.TrySetValue
            (
                this.ClrObject!,
                this.ClrValue,
                this.Coercion
            );
            this.ActualTrySetClrObject = this.ClrObject;

            this.WriteLine();
            this.WriteLine($"ActualTrySetSuccess:   {this.ActualTrySetSuccess.SafeToString()}");
            this.WriteLine($"ActualTrySetClrObject: {this.ActualTrySetClrObject.SafeToString()}");
        }

        protected override void Assert()
        {
            this.ActualTrySetSuccess.Should().Be(this.ExpectedTrySetSuccess);
            this.ActualTrySetClrObject.Should().BeEquivalentTo(this.ExpectedTrySetClrObject);
        }
        #endregion
    }

    private sealed class TrySetByRefGenericTest<TObject, TValue> : TrySetTestBase
        where TObject : struct
    {
        #region User Supplied Properties
        public TObject ClrObject { get; init; }

        public TValue? ClrValue { get; init; }

        public TObject? ExpectedTrySetClrObject { get; init; }
        #endregion

        #region Calculated Properties
        private TObject? ActualTrySetClrObject { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            base.Arrange();

            this.WriteLine($"ClrObject: {this.ClrObject.SafeToString()}");
            this.WriteLine($"ClrValue:  {this.ClrValue.SafeToString()}");
            this.WriteLine();
            this.WriteLine($"ExpectedTrySetSuccess:   {this.ExpectedTrySetSuccess.SafeToString()}");
            this.WriteLine($"ExpectedTrySetClrObject: {this.ExpectedTrySetClrObject.SafeToString()}");
        }

        protected override void Act()
        {
            var local = this.ClrObject;
            this.ActualTrySetSuccess = this.Accessor!.TrySetValueByRef
            (
                ref local,
                this.ClrValue,
                this.Coercion
            );
            this.ActualTrySetClrObject = local;

            this.WriteLine();
            this.WriteLine($"ActualTrySetSuccess:   {this.ActualTrySetSuccess.SafeToString()}");
            this.WriteLine($"ActualTrySetClrObject: {this.ActualTrySetClrObject.SafeToString()}");
        }

        protected override void Assert()
        {
            this.ActualTrySetSuccess.Should().Be(this.ExpectedTrySetSuccess);
            if (this.ExpectedTrySetSuccess)
            {
                this.ActualTrySetClrObject.Should().BeEquivalentTo(this.ExpectedTrySetClrObject);
            }
            else
            {
                this.ActualTrySetClrObject.Should().BeEquivalentTo(this.ClrObject);
            }
        }
        #endregion
    }

    private sealed class TrySetNonGenericTest : TrySetTestBase
    {
        #region User Supplied Properties
        public object? ClrObject { get; init; }

        public object? ClrValue { get; init; }

        public object? ExpectedTrySetClrObject { get; init; }
        #endregion

        #region Calculated Properties
        private object? ActualTrySetClrObject { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            base.Arrange();

            this.WriteLine($"ClrObject: {this.ClrObject.SafeToString()}");
            this.WriteLine($"ClrValue:  {this.ClrValue.SafeToString()}");
            this.WriteLine();
            this.WriteLine($"ExpectedTrySetSuccess:   {this.ExpectedTrySetSuccess.SafeToString()}");
            this.WriteLine($"ExpectedTrySetClrObject: {this.ExpectedTrySetClrObject.SafeToString()}");
        }

        protected override void Act()
        {
            this.ActualTrySetSuccess = this.Accessor!.TrySetValue
            (
                this.ClrObject,
                this.ClrValue,
                this.Coercion
            );
            this.ActualTrySetClrObject = this.ClrObject;

            this.WriteLine();
            this.WriteLine($"ActualTrySetSuccess:   {this.ActualTrySetSuccess.SafeToString()}");
            this.WriteLine($"ActualTrySetClrObject: {this.ActualTrySetClrObject.SafeToString()}");
        }

        protected override void Assert()
        {
            this.ActualTrySetSuccess.Should().Be(this.ExpectedTrySetSuccess);
            this.ActualTrySetClrObject.Should().BeEquivalentTo(this.ExpectedTrySetClrObject);
        }
        #endregion
    }
    #endregion

    #region Static Try Tests
    private sealed class TryGetStaticGenericTest<TValue> : StaticTryGetTestBase
    {
        #region User Supplied Properties
        public TValue? ExpectedClrValue { get; init; }
        #endregion

        #region Calculated Properties
        private TValue? ActualClrValue { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            base.Arrange();

            this.WriteLine($"ExpectedSuccess:  {this.ExpectedSuccess.SafeToString()}");
            this.WriteLine($"ExpectedClrValue: {this.ExpectedClrValue.SafeToString()}");
        }

        protected override void Act()
        {
            this.ActualSuccess = this.Accessor!.TryGetStaticValue(out TValue? clrValue, this.Coercion);
            this.ActualClrValue = clrValue;

            this.WriteLine($"ActualSuccess:  {this.ActualSuccess.SafeToString()}");
            this.WriteLine($"ActualClrValue: {this.ActualClrValue.SafeToString()}");
        }

        protected override void Assert()
        {
            this.ActualSuccess.Should().Be(this.ExpectedSuccess);
            this.ActualClrValue.Should().BeEquivalentTo(
                this.ExpectedSuccess ? this.ExpectedClrValue : default);
        }
        #endregion
    }

    private sealed class TryGetStaticNonGenericTest : StaticTryGetTestBase
    {
        #region User Supplied Properties
        public Type? ClrValueType { get; init; }

        public object? ExpectedClrValue { get; init; }
        #endregion

        #region Calculated Properties
        private object? ActualClrValue { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            base.Arrange();

            this.WriteLine($"ExpectedSuccess:  {this.ExpectedSuccess.SafeToString()}");
            this.WriteLine($"ExpectedClrValue: {this.ExpectedClrValue.SafeToString()}");
        }

        protected override void Act()
        {
            this.ActualSuccess = this.Accessor!.TryGetStaticValue(
                out var clrValue,
                this.ClrValueType,
                this.Coercion);
            this.ActualClrValue = clrValue;

            this.WriteLine($"ActualSuccess:  {this.ActualSuccess.SafeToString()}");
            this.WriteLine($"ActualClrValue: {this.ActualClrValue.SafeToString()}");
        }

        protected override void Assert()
        {
            this.ActualSuccess.Should().Be(this.ExpectedSuccess);
            this.ActualClrValue.Should().BeEquivalentTo(
                this.ExpectedSuccess ? this.ExpectedClrValue : null);
        }
        #endregion
    }

    private sealed class TrySetStaticGenericTest<TValue> : StaticTrySetTestBase
    {
        #region User Supplied Properties
        public TValue? ClrValue { get; init; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            base.Arrange();
            this.WriteLine($"ClrValue: {this.ClrValue.SafeToString()}");
        }
        #endregion

        #region Abstract Methods
        protected override bool TrySetValue()
            => this.Accessor!.TrySetStaticValue(this.ClrValue, this.Coercion);
        #endregion
    }

    private sealed class TrySetStaticNonGenericTest : StaticTrySetTestBase
    {
        #region User Supplied Properties
        public object? ClrValue { get; init; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            base.Arrange();
            this.WriteLine($"ClrValue: {this.ClrValue.SafeToString()}");
        }
        #endregion

        #region Abstract Methods
        protected override bool TrySetValue()
            => this.Accessor!.TrySetStaticValue(this.ClrValue, this.Coercion);
        #endregion
    }
    #endregion

    #region Test Data
    public const string TestGuidString = "86d5d1a9-ec14-4730-8d9a-41812e5a117a";

    public static Guid TestGuid { get; } = Guid.Parse(TestGuidString);

    public const string TestUlidString = "46TQ8TKV0M8WR8V6J1G4Q5M4BT";

    public static Ulid TestUlid { get; } = Ulid.Parse(TestUlidString);
    #endregion
}
