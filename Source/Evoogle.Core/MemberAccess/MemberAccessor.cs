// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;

using Evoogle.Coercion;
using Evoogle.Extensions;
using Evoogle.MemberAccess.Internal;

namespace Evoogle.MemberAccess;

/// <summary>
///     Provides compiled read and write access to one property or field.
/// </summary>
/// <remarks>
///     Delegates are compiled on first use and shared with <see cref="MemberAccessorFactory"/>.
///     A member can support only reading or only writing. Struct mutation requires a
///     by-reference setter.
/// </remarks>
public sealed class MemberAccessor
{
    #region Types
    [Flags]
    private enum AccessCapabilities
    {
        None = 0,
        Read = 1,
        Write = 2,
    }

    private readonly record struct LookupKey
    (
        string MemberName,
        BindingFlags BindingFlags
    );

    private readonly record struct PropertyLookupResult
    (
        PropertyInfo PropertyInfo,
        AccessCapabilities Capabilities
    );

    private readonly record struct TryLookupState
    (
        Type? DeclaringType,
        string? MemberName,
        BindingFlags BindingFlags
    );

    private sealed class AccessorCacheEntry
    {
        private readonly MemberInfo _memberInfo;
        private readonly Type _declaringType;
        private readonly Type _memberType;
        private readonly bool _isStatic;
        private readonly AccessCapabilities _availableCapabilities;
        private readonly Lazy<MemberAccessor> _readAccessor;
        private readonly Lazy<MemberAccessor> _writeAccessor;
        private readonly Lazy<MemberAccessor> _readWriteAccessor;

        public AccessorCacheEntry(MemberInfo memberInfo)
        {
            _memberInfo = memberInfo;
            (_declaringType, _memberType, _isStatic, _availableCapabilities) = Inspect(memberInfo);
            _readAccessor = new Lazy<MemberAccessor>
            (
                this.CreateReadAccessor,
                LazyThreadSafetyMode.ExecutionAndPublication
            );
            _writeAccessor = new Lazy<MemberAccessor>
            (
                this.CreateWriteAccessor,
                LazyThreadSafetyMode.ExecutionAndPublication
            );
            _readWriteAccessor = new Lazy<MemberAccessor>
            (
                this.CreateReadWriteAccessor,
                LazyThreadSafetyMode.ExecutionAndPublication
            );
        }

        public AccessCapabilities AvailableCapabilities => _availableCapabilities;

        public MemberAccessor GetAccessor(AccessCapabilities capabilities) => capabilities switch
        {
            AccessCapabilities.Read => _readAccessor.Value,
            AccessCapabilities.Write => _writeAccessor.Value,
            AccessCapabilities.Read | AccessCapabilities.Write => _readWriteAccessor.Value,
            _ => throw new MemberAccessException($"Member '{_memberInfo.Name}' has no usable getter or setter."),
        };

        private MemberAccessor CreateReadAccessor() => this.CreateAccessor(AccessCapabilities.Read);

        private MemberAccessor CreateWriteAccessor() => this.CreateAccessor(AccessCapabilities.Write);

        private MemberAccessor CreateReadWriteAccessor() => this.CreateAccessor(AccessCapabilities.Read | AccessCapabilities.Write);

        private MemberAccessor CreateAccessor(AccessCapabilities capabilities)
        {
            if ((_availableCapabilities & capabilities) != capabilities)
            {
                throw new MemberAccessException($"Member '{_memberInfo.Name}' does not support the requested operation.");
            }

            return new MemberAccessor
            (
                _memberInfo,
                _declaringType,
                _memberType,
                _isStatic,
                canRead: capabilities.HasFlag(AccessCapabilities.Read),
                canWrite: capabilities.HasFlag(AccessCapabilities.Write)
            );
        }

        private static (Type DeclaringType, Type MemberType, bool IsStatic, AccessCapabilities Capabilities) Inspect
        (
            MemberInfo memberInfo
        )
        {
            var declaringType = memberInfo.DeclaringType ?? throw new MemberAccessException($"Member '{memberInfo.Name}' has no declaring type.");
            if (declaringType.ContainsGenericParameters)
            {
                throw new MemberAccessException($"Member '{memberInfo.Name}' belongs to an open generic type.");
            }

            return memberInfo switch
            {
                PropertyInfo propertyInfo => InspectProperty(propertyInfo, declaringType),
                FieldInfo fieldInfo => InspectField(fieldInfo, declaringType),
                _ => throw new MemberAccessException($"Member '{memberInfo.Name}' is neither a property nor a field."),
            };
        }
    }

    private sealed class PropertyLookupCacheEntry
    {
        private readonly Type _declaringType;
        private readonly LookupKey _lookup;
        private readonly Lazy<PropertyLookupResult?> _result;

        public PropertyLookupCacheEntry(Type declaringType, LookupKey lookup)
        {
            _declaringType = declaringType;
            _lookup = lookup;
            _result = new Lazy<PropertyLookupResult?>
            (
                this.FindProperty,
                LazyThreadSafetyMode.ExecutionAndPublication
            );
        }

        public PropertyLookupResult? Result => _result.Value;

        private PropertyLookupResult? FindProperty() => FindPropertyCore(_declaringType, _lookup);
    }

    private sealed class FieldLookupCacheEntry
    {
        private readonly Type _declaringType;
        private readonly LookupKey _lookup;
        private readonly Lazy<FieldInfo?> _fieldInfo;

        public FieldLookupCacheEntry(Type declaringType, LookupKey lookup)
        {
            _declaringType = declaringType;
            _lookup = lookup;
            _fieldInfo = new Lazy<FieldInfo?>
            (
                this.FindField,
                LazyThreadSafetyMode.ExecutionAndPublication
            );
        }

        public FieldInfo? FieldInfo => _fieldInfo.Value;

        private FieldInfo? FindField() => _declaringType.GetField(_lookup.MemberName, _lookup.BindingFlags);
    }

    private sealed class TypeLookupCache(Type declaringType)
    {
        private readonly Type _declaringType = declaringType;
        private readonly ConcurrentDictionary<MemberInfo, AccessorCacheEntry> _accessors = new();
        private readonly ConcurrentDictionary<LookupKey, PropertyLookupCacheEntry> _properties = new();
        private readonly ConcurrentDictionary<LookupKey, FieldLookupCacheEntry> _fields = new();

        public AccessorCacheEntry GetAccessor(MemberInfo memberInfo)
            => _accessors.GetOrAdd(memberInfo, static member => new AccessorCacheEntry(member));

        public PropertyLookupResult? FindProperty(LookupKey lookup)
        {
            var entry = _properties.GetOrAdd
            (
                lookup,
                static (key, declaringType) => new PropertyLookupCacheEntry(declaringType, key),
                _declaringType
            );

            try
            {
                var result = entry.Result;
                if (result is null)
                {
                    _properties.TryRemove(lookup, out _);
                }

                return result;
            }
            catch (Exception)
            {
                _properties.TryRemove(lookup, out _);
                throw;
            }
        }

        public FieldInfo? FindField(LookupKey lookup)
        {
            var entry = _fields.GetOrAdd
            (
                lookup,
                static (key, declaringType) => new FieldLookupCacheEntry(declaringType, key),
                _declaringType
            );

            try
            {
                var fieldInfo = entry.FieldInfo;
                if (fieldInfo is null)
                {
                    _fields.TryRemove(lookup, out _);
                }

                return fieldInfo;
            }
            catch (Exception)
            {
                _fields.TryRemove(lookup, out _);
                throw;
            }
        }
    }
    #endregion

    #region Fields
    private const BindingFlags _defaultFlags = BindingFlags.Public | BindingFlags.Instance;

    private static readonly ConditionalWeakTable<Type, TypeLookupCache> _lookups = [];

    private Func<object, object?>? _getter;
    private Func<object?>? _staticGetter;
    private Func<object, Type, TypeCoercion, TypeCoercionContext?, object?>? _coercingGetter;
    private Func<Type, TypeCoercion, TypeCoercionContext?, object?>? _coercingStaticGetter;

    private Action<object, object?>? _setter;
    private Action<object?>? _staticSetter;
    private Action<object, object?, TypeCoercion, TypeCoercionContext?>? _coercingSetter;
    private Action<object?, TypeCoercion, TypeCoercionContext?>? _coercingStaticSetter;
    #endregion

    #region Constructors
    private MemberAccessor
    (
        MemberInfo memberInfo,
        Type declaringType,
        Type memberType,
        bool isStatic,
        bool canRead,
        bool canWrite
    )
    {
        this.MemberInfo = memberInfo;
        this.DeclaringType = declaringType;
        this.MemberType = memberType;
        this.IsProperty = memberInfo is PropertyInfo;
        this.IsField = memberInfo is FieldInfo;
        this.IsStatic = isStatic;
        this.CanRead = canRead;
        this.CanWrite = canWrite;
    }
    #endregion

    #region Properties
    /// <summary>Gets the exact reflected property or field.</summary>
    public MemberInfo MemberInfo { get; }

    /// <summary>Gets the member's declaring type.</summary>
    public Type DeclaringType { get; }

    /// <summary>Gets the member's value type.</summary>
    public Type MemberType { get; }

    /// <summary>Gets whether the member is a property.</summary>
    public bool IsProperty { get; }

    /// <summary>Gets whether the member is a field.</summary>
    public bool IsField { get; }

    /// <summary>Gets whether the member is static.</summary>
    public bool IsStatic { get; }

    /// <summary>Gets whether the member has a getter.</summary>
    public bool CanRead { get; }

    /// <summary>Gets whether the member has a runtime setter.</summary>
    public bool CanWrite { get; }
    #endregion

    #region Computed Properties
    /// <summary>Gets the member's name.</summary>
    public string MemberName => this.MemberInfo.Name;

    /// <summary>Gets the exact reflected property.</summary>
    /// <exception cref="MemberAccessException">Thrown if the member is not a property.</exception>
    public PropertyInfo PropertyInfo => this.MemberInfo as PropertyInfo ?? throw new MemberAccessException("The member is not a property.");

    /// <summary>Gets the exact reflected field.</summary>
    /// <exception cref="MemberAccessException">Thrown if the member is not a field.</exception>
    public FieldInfo FieldInfo => this.MemberInfo as FieldInfo ?? throw new MemberAccessException("The member is not a field.");
    #endregion

    #region Factory Methods
    /// <summary>Creates an accessor for an exact reflected property or field.</summary>
    /// <param name="memberInfo">The member to access.</param>
    /// <returns>The accessor.</returns>
    public static MemberAccessor Create(MemberInfo memberInfo)
    {
        ArgumentNullException.ThrowIfNull(memberInfo);

        var declaringType = memberInfo.DeclaringType ?? throw new MemberAccessException($"Member '{memberInfo.Name}' has no declaring type.");
        var cache = _lookups.GetValue(declaringType, static type => new TypeLookupCache(type));
        var entry = cache.GetAccessor(memberInfo);
        return entry.GetAccessor(entry.AvailableCapabilities);
    }

    /// <summary>Looks up a property and creates its accessor.</summary>
    /// <param name="declaringType">The type to search.</param>
    /// <param name="memberName">The case-sensitive property name.</param>
    /// <param name="bindingFlags">Reflection lookup flags. The default is public instance.</param>
    /// <returns>The accessor.</returns>
    public static MemberAccessor CreateProperty
    (
        Type declaringType,
        string memberName,
        BindingFlags bindingFlags = _defaultFlags
    )
    {
        ArgumentNullException.ThrowIfNull(declaringType);
        ArgumentException.ThrowIfNullOrWhiteSpace(memberName);

        var key = new LookupKey(memberName, bindingFlags);
        try
        {
            var result = FindProperty(declaringType, key) ?? throw new MemberAccessException($"Property '{memberName}' was not found on '{declaringType}'.");
            return Create(result.PropertyInfo, result.Capabilities);
        }
        catch (AmbiguousMatchException exception)
        {
            throw new MemberAccessException($"Property '{memberName}' is ambiguous on '{declaringType}'.", exception);
        }
    }

    /// <summary>Looks up a field and creates its accessor.</summary>
    /// <param name="declaringType">The type to search.</param>
    /// <param name="memberName">The case-sensitive field name.</param>
    /// <param name="bindingFlags">Reflection lookup flags. The default is public instance.</param>
    /// <returns>The accessor.</returns>
    public static MemberAccessor CreateField
    (
        Type declaringType,
        string memberName,
        BindingFlags bindingFlags = _defaultFlags
    )
    {
        ArgumentNullException.ThrowIfNull(declaringType);
        ArgumentException.ThrowIfNullOrWhiteSpace(memberName);

        var key = new LookupKey(memberName, bindingFlags);
        try
        {
            var fieldInfo = FindField(declaringType, key) ?? throw new MemberAccessException($"Field '{memberName}' was not found on '{declaringType}'.");
            return Create(fieldInfo);
        }
        catch (AmbiguousMatchException exception)
        {
            throw new MemberAccessException($"Field '{memberName}' is ambiguous on '{declaringType}'.", exception);
        }
    }

    /// <summary>Attempts to create an accessor for an exact reflected member.</summary>
    /// <param name="memberInfo">The member to access.</param>
    /// <param name="accessor">The created accessor when successful.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreate(MemberInfo? memberInfo, out MemberAccessor? accessor)
        => TryCreateCore(memberInfo, static member => Create(member!), out accessor);

    /// <summary>Attempts to look up and create a property accessor.</summary>
    /// <param name="declaringType">The type to search.</param>
    /// <param name="memberName">The case-sensitive property name.</param>
    /// <param name="accessor">The created accessor when successful.</param>
    /// <param name="bindingFlags">Reflection lookup flags.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateProperty
    (
        Type? declaringType,
        string? memberName,
        out MemberAccessor? accessor,
        BindingFlags bindingFlags = _defaultFlags
    ) => TryCreateCore
    (
        new TryLookupState(declaringType, memberName, bindingFlags),
        static state => CreateProperty
        (
            state.DeclaringType!,
            state.MemberName!,
            state.BindingFlags
        ),
        out accessor
    );

    /// <summary>Attempts to look up and create a field accessor.</summary>
    /// <param name="declaringType">The type to search.</param>
    /// <param name="memberName">The case-sensitive field name.</param>
    /// <param name="accessor">The created accessor when successful.</param>
    /// <param name="bindingFlags">Reflection lookup flags.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateField
    (
        Type? declaringType,
        string? memberName,
        out MemberAccessor? accessor,
        BindingFlags bindingFlags = _defaultFlags
    ) => TryCreateCore
    (
        new TryLookupState(declaringType, memberName, bindingFlags),
        static state => CreateField
        (
            state.DeclaringType!,
            state.MemberName!,
            state.BindingFlags
        ),
        out accessor
    );
    #endregion

    #region Get Methods
    /// <summary>Reads an instance member, optionally converting its value.</summary>
    /// <param name="target">The object containing the member.</param>
    /// <param name="valueType">The optional requested result type.</param>
    /// <param name="coercion">The optional conversion service.</param>
    /// <param name="context">The optional conversion context.</param>
    /// <returns>The member value.</returns>
    public object? GetValue
    (
        object target,
        Type? valueType = null,
        TypeCoercion? coercion = null,
        TypeCoercionContext? context = null
    )
    {
        ArgumentNullException.ThrowIfNull(target);

        try
        {
            this.RequireInstance();
            if (valueType is not null && coercion is not null)
            {
                return this.GetCoercingObjectGetter()(target, valueType, coercion, context);
            }

            ValidateResultType(this.MemberType, valueType);
            var getter = this.GetObjectGetter();
            return getter(target);
        }
        catch (Exception exception)
        {
            throw new MemberAccessException($"Could not access member '{this.MemberInfo.Name}'.", exception);
        }
    }

    /// <summary>Reads an instance member using the requested generic types.</summary>
    /// <typeparam name="TObject">The target type.</typeparam>
    /// <typeparam name="TValue">The requested result type.</typeparam>
    /// <param name="target">The object containing the member.</param>
    /// <param name="coercion">The optional conversion service.</param>
    /// <param name="context">The optional conversion context.</param>
    /// <returns>The member value.</returns>
    public TValue? GetValue<TObject, TValue>
    (
        TObject target,
        TypeCoercion? coercion = null,
        TypeCoercionContext? context = null
    )
    {
        ArgumentNullException.ThrowIfNull(target);

        try
        {
            this.RequireInstance();
            if (coercion is null)
            {
                var getter = MemberAccessCompiler.Get<Func<TObject, TValue?>>(this, AccessOperation.Get);
                return getter(target);
            }

            var coercingGetter = MemberAccessCompiler.Get<Func<TObject, TypeCoercion, TypeCoercionContext?, TValue?>>(this, AccessOperation.CoercingGet);
            return coercingGetter(target, coercion, context);
        }
        catch (Exception exception)
        {
            throw new MemberAccessException($"Could not access member '{this.MemberInfo.Name}'.", exception);
        }
    }

    /// <summary>Reads a static member, optionally converting its value.</summary>
    /// <param name="valueType">The optional requested result type.</param>
    /// <param name="coercion">The optional conversion service.</param>
    /// <param name="context">The optional conversion context.</param>
    /// <returns>The member value.</returns>
    public object? GetStaticValue
    (
        Type? valueType = null,
        TypeCoercion? coercion = null,
        TypeCoercionContext? context = null
    )
    {
        try
        {
            this.RequireStatic();
            if (valueType is not null && coercion is not null)
            {
                var coercingGetter = this.GetCoercingStaticObjectGetter();
                return coercingGetter(valueType, coercion, context);
            }

            ValidateResultType(this.MemberType, valueType);
            var getter = this.GetStaticObjectGetter();
            return getter();
        }
        catch (Exception exception)
        {
            throw new MemberAccessException($"Could not access member '{this.MemberInfo.Name}'.", exception);
        }
    }

    /// <summary>Reads a static member using a requested generic result type.</summary>
    /// <typeparam name="TValue">The requested result type.</typeparam>
    /// <param name="coercion">The optional conversion service.</param>
    /// <param name="context">The optional conversion context.</param>
    /// <returns>The member value.</returns>
    public TValue? GetStaticValue<TValue>
    (
        TypeCoercion? coercion = null,
        TypeCoercionContext? context = null
    )
    {
        try
        {
            this.RequireStatic();
            if (coercion is null)
            {
                var getter = MemberAccessCompiler.Get<Func<TValue?>>(this, AccessOperation.Get);
                return getter();
            }

            var coercingGetter = MemberAccessCompiler.Get<Func<TypeCoercion, TypeCoercionContext?, TValue?>>(this, AccessOperation.CoercingGet);
            return coercingGetter(coercion, context);
        }
        catch (Exception exception)
        {
            throw new MemberAccessException($"Could not access member '{this.MemberInfo.Name}'.", exception);
        }
    }
    #endregion

    #region Set Methods
    /// <summary>Writes an instance member, optionally converting the supplied value.</summary>
    /// <param name="target">The object containing the member.</param>
    /// <param name="value">The value to assign.</param>
    /// <param name="coercion">The optional conversion service.</param>
    /// <param name="context">The optional conversion context.</param>
    public void SetValue
    (
        object target,
        object? value,
        TypeCoercion? coercion = null,
        TypeCoercionContext? context = null
    )
    {
        ArgumentNullException.ThrowIfNull(target);

        try
        {
            this.RequireInstance();
            if (target is ValueType)
            {
                throw new MemberAccessException("A boxed struct cannot be changed through SetValue; use SetValueByRef.");
            }

            if (coercion is null)
            {
                var setter = this.GetObjectSetter();
                setter(target, value);
            }
            else
            {
                var setter = this.GetCoercingObjectSetter();
                setter(target, value, coercion, context);
            }
        }
        catch (Exception exception)
        {
            throw new MemberAccessException($"Could not access member '{this.MemberInfo.Name}'.", exception);
        }
    }

    /// <summary>Writes an instance member using the requested generic types.</summary>
    /// <typeparam name="TObject">The target type.</typeparam>
    /// <typeparam name="TValue">The supplied value type.</typeparam>
    /// <param name="target">The object containing the member.</param>
    /// <param name="value">The value to assign.</param>
    /// <param name="coercion">The optional conversion service.</param>
    /// <param name="context">The optional conversion context.</param>
    public void SetValue<TObject, TValue>
    (
        TObject target,
        TValue value,
        TypeCoercion? coercion = null,
        TypeCoercionContext? context = null
    )
    {
        ArgumentNullException.ThrowIfNull(target);

        try
        {
            this.RequireInstance();
            if (coercion is null)
            {
                var setter = MemberAccessCompiler.Get<Action<TObject, TValue?>>(this, AccessOperation.Set);
                setter(target, value);
            }
            else
            {
                var setter = MemberAccessCompiler.Get<Action<TObject, TValue?, TypeCoercion, TypeCoercionContext?>>(this, AccessOperation.CoercingSet);
                setter(target, value, coercion, context);
            }
        }
        catch (Exception exception)
        {
            throw new MemberAccessException($"Could not access member '{this.MemberInfo.Name}'.", exception);
        }
    }

    /// <summary>Writes a static member, optionally converting the supplied value.</summary>
    /// <param name="value">The value to assign.</param>
    /// <param name="coercion">The optional conversion service.</param>
    /// <param name="context">The optional conversion context.</param>
    public void SetStaticValue
    (
        object? value,
        TypeCoercion? coercion = null,
        TypeCoercionContext? context = null
    )
    {
        try
        {
            this.RequireStatic();
            if (coercion is null)
            {
                var setter = this.GetStaticObjectSetter();
                setter(value);
            }
            else
            {
                var setter = this.GetCoercingStaticObjectSetter();
                setter(value, coercion, context);
            }
        }
        catch (Exception exception)
        {
            throw new MemberAccessException($"Could not access member '{this.MemberInfo.Name}'.", exception);
        }
    }

    /// <summary>Writes a static member using a generic value type.</summary>
    /// <typeparam name="TValue">The supplied value type.</typeparam>
    /// <param name="value">The value to assign.</param>
    /// <param name="coercion">The optional conversion service.</param>
    /// <param name="context">The optional conversion context.</param>
    public void SetStaticValue<TValue>
    (
        TValue value,
        TypeCoercion? coercion = null,
        TypeCoercionContext? context = null
    )
    {
        try
        {
            this.RequireStatic();
            if (coercion is null)
            {
                var setter = MemberAccessCompiler.Get<Action<TValue?>>(this, AccessOperation.Set);
                setter(value);
            }
            else
            {
                var setter = MemberAccessCompiler.Get<Action<TValue?, TypeCoercion, TypeCoercionContext?>>(this, AccessOperation.CoercingSet);
                setter(value, coercion, context);
            }
        }
        catch (Exception exception)
        {
            throw new MemberAccessException($"Could not access member '{this.MemberInfo.Name}'.", exception);
        }
    }

    /// <summary>Writes a struct member without changing a boxed copy.</summary>
    /// <typeparam name="TObject">The struct type.</typeparam>
    /// <typeparam name="TValue">The supplied value type.</typeparam>
    /// <param name="target">The original struct.</param>
    /// <param name="value">The value to assign.</param>
    /// <param name="coercion">The optional conversion service.</param>
    /// <param name="context">The optional conversion context.</param>
    public void SetValueByRef<TObject, TValue>
    (
        ref TObject target,
        TValue value,
        TypeCoercion? coercion = null,
        TypeCoercionContext? context = null
    ) where TObject : struct
    {
        try
        {
            this.RequireInstance();
            if (coercion is null)
            {
                var setter = MemberAccessCompiler.Get<MemberSetterByRef<TObject, TValue>>(this, AccessOperation.Set);
                setter(ref target, value);
            }
            else
            {
                var setter = MemberAccessCompiler.Get<CoercingMemberSetterByRef<TObject, TValue>>(this, AccessOperation.CoercingSet);
                setter(ref target, value, coercion, context);
            }
        }
        catch (Exception exception)
        {
            throw new MemberAccessException($"Could not set member '{this.MemberInfo.Name}'.", exception);
        }
    }
    #endregion

    #region TryGet Methods
    /// <summary>Attempts to read an instance member.</summary>
    /// <param name="target">The object containing the member.</param>
    /// <param name="value">The retrieved value when successful.</param>
    /// <param name="valueType">The optional requested result type.</param>
    /// <param name="coercion">The optional conversion service.</param>
    /// <param name="context">The optional conversion context.</param>
    /// <returns>Whether reading succeeded.</returns>
    public bool TryGetValue
    (
        object? target,
        out object? value,
        Type? valueType = null,
        TypeCoercion? coercion = null,
        TypeCoercionContext? context = null
    )
    {
        try
        {
            value = this.GetValue(target!, valueType, coercion, context);
            return true;
        }
        catch (Exception)
        {
            value = null;
            return false;
        }
    }

    /// <summary>Attempts to read an instance member with generic types.</summary>
    /// <typeparam name="TObject">The target type.</typeparam>
    /// <typeparam name="TValue">The requested result type.</typeparam>
    /// <param name="target">The object containing the member.</param>
    /// <param name="value">The retrieved value when successful.</param>
    /// <param name="coercion">The optional conversion service.</param>
    /// <param name="context">The optional conversion context.</param>
    /// <returns>Whether reading succeeded.</returns>
    public bool TryGetValue<TObject, TValue>
    (
        TObject target,
        out TValue? value,
        TypeCoercion? coercion = null,
        TypeCoercionContext? context = null
    )
    {
        try
        {
            value = this.GetValue<TObject, TValue>(target, coercion, context);
            return true;
        }
        catch (Exception)
        {
            value = default;
            return false;
        }
    }

    /// <summary>Attempts to read a static member.</summary>
    /// <param name="value">The retrieved value when successful.</param>
    /// <param name="valueType">The optional requested result type.</param>
    /// <param name="coercion">The optional conversion service.</param>
    /// <param name="context">The optional conversion context.</param>
    /// <returns>Whether reading succeeded.</returns>
    public bool TryGetStaticValue
    (
        out object? value,
        Type? valueType = null,
        TypeCoercion? coercion = null,
        TypeCoercionContext? context = null
    )
    {
        try
        {
            value = this.GetStaticValue(valueType, coercion, context);
            return true;
        }
        catch (Exception)
        {
            value = null;
            return false;
        }
    }

    /// <summary>Attempts to read a static member with a generic result type.</summary>
    /// <typeparam name="TValue">The requested result type.</typeparam>
    /// <param name="value">The retrieved value when successful.</param>
    /// <param name="coercion">The optional conversion service.</param>
    /// <param name="context">The optional conversion context.</param>
    /// <returns>Whether reading succeeded.</returns>
    public bool TryGetStaticValue<TValue>
    (
        out TValue? value,
        TypeCoercion? coercion = null,
        TypeCoercionContext? context = null
    )
    {
        try
        {
            value = this.GetStaticValue<TValue>(coercion, context);
            return true;
        }
        catch (Exception)
        {
            value = default;
            return false;
        }
    }
    #endregion

    #region TrySet Methods
    /// <summary>Attempts to write an instance member.</summary>
    /// <param name="target">The object containing the member.</param>
    /// <param name="value">The value to assign.</param>
    /// <param name="coercion">The optional conversion service.</param>
    /// <param name="context">The optional conversion context.</param>
    /// <returns>Whether writing succeeded.</returns>
    public bool TrySetValue
    (
        object? target,
        object? value,
        TypeCoercion? coercion = null,
        TypeCoercionContext? context = null
    )
    {
        try
        {
            this.SetValue(target!, value, coercion, context);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>Attempts to write an instance member with generic types.</summary>
    /// <typeparam name="TObject">The target type.</typeparam>
    /// <typeparam name="TValue">The supplied value type.</typeparam>
    /// <param name="target">The object containing the member.</param>
    /// <param name="value">The value to assign.</param>
    /// <param name="coercion">The optional conversion service.</param>
    /// <param name="context">The optional conversion context.</param>
    /// <returns>Whether writing succeeded.</returns>
    public bool TrySetValue<TObject, TValue>
    (
        TObject target,
        TValue value,
        TypeCoercion? coercion = null,
        TypeCoercionContext? context = null
    )
    {
        try
        {
            this.SetValue(target, value, coercion, context);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>Attempts to write a static member.</summary>
    /// <param name="value">The value to assign.</param>
    /// <param name="coercion">The optional conversion service.</param>
    /// <param name="context">The optional conversion context.</param>
    /// <returns>Whether writing succeeded.</returns>
    public bool TrySetStaticValue
    (
        object? value,
        TypeCoercion? coercion = null,
        TypeCoercionContext? context = null
    )
    {
        try
        {
            this.SetStaticValue(value, coercion, context);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>Attempts to write a static member with a generic value type.</summary>
    /// <typeparam name="TValue">The supplied value type.</typeparam>
    /// <param name="value">The value to assign.</param>
    /// <param name="coercion">The optional conversion service.</param>
    /// <param name="context">The optional conversion context.</param>
    /// <returns>Whether writing succeeded.</returns>
    public bool TrySetStaticValue<TValue>
    (
        TValue value,
        TypeCoercion? coercion = null,
        TypeCoercionContext? context = null
    )
    {
        try
        {
            this.SetStaticValue(value, coercion, context);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>Attempts to write a struct member by reference.</summary>
    /// <typeparam name="TObject">The struct type.</typeparam>
    /// <typeparam name="TValue">The supplied value type.</typeparam>
    /// <param name="target">The original struct.</param>
    /// <param name="value">The value to assign.</param>
    /// <param name="coercion">The optional conversion service.</param>
    /// <param name="context">The optional conversion context.</param>
    /// <returns>Whether writing succeeded.</returns>
    public bool TrySetValueByRef<TObject, TValue>
    (
        ref TObject target,
        TValue value,
        TypeCoercion? coercion = null,
        TypeCoercionContext? context = null
    ) where TObject : struct
    {
        try
        {
            this.SetValueByRef(ref target, value, coercion, context);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    #endregion

    #region Object Methods
    /// <inheritdoc/>
    public override string ToString()
    {
        var declaringType = this.DeclaringType.SafeToName();
        var memberType = this.MemberType.SafeToName();
        var memberName = this.MemberName.SafeToString();
        var isProperty = this.IsProperty;
        var isField = this.IsField;
        var isStatic = this.IsStatic;
        var canRead = this.CanRead;
        var canWrite = this.CanWrite;

        return $"{nameof(MemberAccessor)} {{{nameof(this.DeclaringType)}={declaringType}, {nameof(this.MemberType)}={memberType}, {nameof(this.MemberName)}={memberName}, {nameof(this.IsProperty)}={isProperty}, {nameof(this.IsField)}={isField}, {nameof(this.IsStatic)}={isStatic}, {nameof(this.CanRead)}={canRead}, {nameof(this.CanWrite)}={canWrite}}}";
    }
    #endregion

    #region Factory Implementation Methods
    private static MemberAccessor Create(MemberInfo memberInfo, AccessCapabilities capabilities)
    {
        var declaringType = memberInfo.DeclaringType ?? throw new MemberAccessException($"Member '{memberInfo.Name}' has no declaring type.");
        var cache = _lookups.GetValue(declaringType, static type => new TypeLookupCache(type));
        var entry = cache.GetAccessor(memberInfo);
        return entry.GetAccessor(capabilities);
    }

    private static (Type DeclaringType, Type MemberType, bool IsStatic, AccessCapabilities Capabilities) InspectField
    (
        FieldInfo fieldInfo,
        Type declaringType
    )
    {
        var capabilities = AccessCapabilities.Read;
        if (!fieldInfo.IsInitOnly && !fieldInfo.IsLiteral)
        {
            capabilities |= AccessCapabilities.Write;
        }

        return
        (
            declaringType,
            fieldInfo.FieldType,
            fieldInfo.IsStatic,
            capabilities
        );
    }

    private static (Type DeclaringType, Type MemberType, bool IsStatic, AccessCapabilities Capabilities) InspectProperty
    (
        PropertyInfo propertyInfo,
        Type declaringType
    )
    {
        if (propertyInfo.GetIndexParameters().Length != 0)
        {
            throw new MemberAccessException($"Indexer property '{propertyInfo.Name}' is not supported.");
        }

        var getter = propertyInfo.GetGetMethod(nonPublic: true);
        var setter = propertyInfo.GetSetMethod(nonPublic: true);
        var method = getter ?? setter ?? throw new MemberAccessException($"Property '{propertyInfo.Name}' has no getter or setter.");
        if (getter is not null && setter is not null && getter.IsStatic != setter.IsStatic)
        {
            throw new MemberAccessException($"Property '{propertyInfo.Name}' has inconsistent accessor methods.");
        }

        var isInitOnly = setter?.ReturnParameter
            .GetRequiredCustomModifiers()
            .Contains(typeof(IsExternalInit)) == true;

        var capabilities = AccessCapabilities.None;
        if (getter is not null)
        {
            capabilities |= AccessCapabilities.Read;
        }
        if (setter is not null && !isInitOnly)
        {
            capabilities |= AccessCapabilities.Write;
        }

        return
        (
            declaringType,
            propertyInfo.PropertyType,
            method.IsStatic,
            capabilities
        );
    }

    private static PropertyLookupResult? FindProperty(Type declaringType, LookupKey key)
    {
        var cache = _lookups.GetValue(declaringType, static type => new TypeLookupCache(type));
        return cache.FindProperty(key);
    }

    private static FieldInfo? FindField(Type declaringType, LookupKey key)
    {
        var cache = _lookups.GetValue(declaringType, static type => new TypeLookupCache(type));
        return cache.FindField(key);
    }

    private static PropertyLookupResult? FindPropertyCore(Type declaringType, LookupKey lookup)
    {
        var visibilityFlags = lookup.BindingFlags & (BindingFlags.Public | BindingFlags.NonPublic);
        if (visibilityFlags == 0)
        {
            return null;
        }

        var comparison = lookup.BindingFlags.HasFlag(BindingFlags.IgnoreCase)
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        var enumerationFlags = lookup.BindingFlags | BindingFlags.Public | BindingFlags.NonPublic;
        PropertyLookupResult? result = null;

        foreach (var propertyInfo in declaringType.GetProperties(enumerationFlags))
        {
            if (!string.Equals(propertyInfo.Name, lookup.MemberName, comparison))
            {
                continue;
            }

            var capabilities = GetVisibleCapabilities(propertyInfo, visibilityFlags);
            if (capabilities == AccessCapabilities.None)
            {
                continue;
            }

            if (result is not null)
            {
                throw new AmbiguousMatchException();
            }

            result = new PropertyLookupResult(propertyInfo, capabilities);
        }

        return result;
    }

    private static AccessCapabilities GetVisibleCapabilities
    (
        PropertyInfo propertyInfo,
        BindingFlags visibilityFlags
    )
    {
        var capabilities = AccessCapabilities.None;
        var getter = propertyInfo.GetGetMethod(nonPublic: true);
        var setter = propertyInfo.GetSetMethod(nonPublic: true);

        if (getter is not null && IsVisible(getter, visibilityFlags))
        {
            capabilities |= AccessCapabilities.Read;
        }

        var isInitOnly = setter?.ReturnParameter
            .GetRequiredCustomModifiers()
            .Contains(typeof(IsExternalInit)) == true;
        if (setter is not null && !isInitOnly && IsVisible(setter, visibilityFlags))
        {
            capabilities |= AccessCapabilities.Write;
        }

        return capabilities;
    }

    private static bool IsVisible(MethodInfo methodInfo, BindingFlags visibilityFlags)
    {
        return methodInfo.IsPublic
            ? visibilityFlags.HasFlag(BindingFlags.Public)
            : visibilityFlags.HasFlag(BindingFlags.NonPublic);
    }

    private static bool TryCreateCore<TState>
    (
        TState state,
        Func<TState, MemberAccessor> create,
        out MemberAccessor? accessor
    )
    {
        try
        {
            accessor = create(state);
            return true;
        }
        catch (Exception)
        {
            accessor = null;
            return false;
        }
    }
    #endregion

    #region Get Implementation Methods
    private Func<object, object?> GetObjectGetter()
    {
        var getter = Volatile.Read(ref _getter);
        if (getter is not null)
        {
            return getter;
        }

        var created = MemberAccessCompiler.Get<Func<object, object?>>(this, AccessOperation.Get);
        return Interlocked.CompareExchange(ref _getter, created, null) ?? created;
    }

    private Func<object?> GetStaticObjectGetter()
    {
        var getter = Volatile.Read(ref _staticGetter);
        if (getter is not null)
        {
            return getter;
        }

        var created = MemberAccessCompiler.Get<Func<object?>>(this, AccessOperation.Get);
        return Interlocked.CompareExchange(ref _staticGetter, created, null) ?? created;
    }

    private Func<object, Type, TypeCoercion, TypeCoercionContext?, object?> GetCoercingObjectGetter()
    {
        var getter = Volatile.Read(ref _coercingGetter);
        if (getter is not null)
        {
            return getter;
        }

        var created = MemberAccessCompiler.Get<Func<object, Type, TypeCoercion, TypeCoercionContext?, object?>>(this, AccessOperation.CoercingGet);
        return Interlocked.CompareExchange(ref _coercingGetter, created, null) ?? created;
    }

    private Func<Type, TypeCoercion, TypeCoercionContext?, object?> GetCoercingStaticObjectGetter()
    {
        var getter = Volatile.Read(ref _coercingStaticGetter);
        if (getter is not null)
        {
            return getter;
        }

        var created = MemberAccessCompiler.Get<Func<Type, TypeCoercion, TypeCoercionContext?, object?>>(this, AccessOperation.CoercingGet);
        return Interlocked.CompareExchange(ref _coercingStaticGetter, created, null) ?? created;
    }
    #endregion

    #region Set Implementation Methods
    private Action<object, object?> GetObjectSetter()
    {
        var setter = Volatile.Read(ref _setter);
        if (setter is not null)
        {
            return setter;
        }

        var created = MemberAccessCompiler.Get<Action<object, object?>>(this, AccessOperation.Set);
        return Interlocked.CompareExchange(ref _setter, created, null) ?? created;
    }

    private Action<object?> GetStaticObjectSetter()
    {
        var setter = Volatile.Read(ref _staticSetter);
        if (setter is not null)
        {
            return setter;
        }

        var created = MemberAccessCompiler.Get<Action<object?>>(this, AccessOperation.Set);
        return Interlocked.CompareExchange(ref _staticSetter, created, null) ?? created;
    }

    private Action<object, object?, TypeCoercion, TypeCoercionContext?> GetCoercingObjectSetter()
    {
        var setter = Volatile.Read(ref _coercingSetter);
        if (setter is not null)
        {
            return setter;
        }

        var created = MemberAccessCompiler.Get<Action<object, object?, TypeCoercion, TypeCoercionContext?>>(this, AccessOperation.CoercingSet);
        return Interlocked.CompareExchange(ref _coercingSetter, created, null) ?? created;
    }

    private Action<object?, TypeCoercion, TypeCoercionContext?>
        GetCoercingStaticObjectSetter()
    {
        var setter = Volatile.Read(ref _coercingStaticSetter);
        if (setter is not null)
        {
            return setter;
        }

        var created = MemberAccessCompiler.Get<Action<object?, TypeCoercion, TypeCoercionContext?>>(this, AccessOperation.CoercingSet);
        return Interlocked.CompareExchange(ref _coercingStaticSetter, created, null) ?? created;
    }
    #endregion

    #region Implementation Methods
    private void RequireInstance()
    {
        if (this.IsStatic)
        {
            throw new MemberAccessException($"Member '{this.MemberInfo.Name}' is static; use a static access method.");
        }
    }

    private void RequireStatic()
    {
        if (!this.IsStatic)
        {
            throw new MemberAccessException($"Member '{this.MemberInfo.Name}' is an instance member.");
        }
    }

    private static void ValidateResultType(Type memberType, Type? valueType)
    {
        if (valueType is null)
        {
            return;
        }

        if (!valueType.IsAssignableFrom(memberType))
        {
            throw new MemberAccessException($"Member type '{memberType}' cannot be returned as '{valueType}' without coercion.");
        }
    }
    #endregion
}
