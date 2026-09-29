// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.Extensions;

namespace Evoogle.MemberAccess;

public struct Point
{
    public long X;

    public long Y { get; set; }

    public string? Note { get; set; }

    public override readonly string ToString()
    {
        var x = this.X.SafeToString();
        var y = this.Y.SafeToString();
        var note = this.Note.SafeToString();

        return $"{nameof(Point)} {{{nameof(this.X)}={x}, {nameof(this.Y)}={y}, {nameof(this.Note)}={note}}}";
    }
}

public sealed class ScalarsOnly
{
    private long? _nonPublicOptionalNumber;

    private long NonPublicRequiredNumber { get; set; }

    public const string NonPublicOptionalNumberFieldName = nameof(_nonPublicOptionalNumber);

    public const string NonPublicRequiredNumberPropertyName = nameof(NonPublicRequiredNumber);

    public string? OptionalName;
    public long? OptionalNumber;
    public bool? OptionalPredicate;

    public ScalarsOnly()
    {
        this.RequiredName = string.Empty;
    }

    public ScalarsOnly(string requiredName, long requiredNumber, bool requiredPredicate)
    {
        this.RequiredName = requiredName;
        this.RequiredNumber = requiredNumber;
        this.RequiredPredicate = requiredPredicate;
    }

    public ScalarsOnly
    (
        string requiredName,
        long requiredNumber,
        bool requiredPredicate,
        long nonPublicRequiredNumber,
        long? nonPublicOptionalNumber
    ) : this(requiredName, requiredNumber, requiredPredicate)
    {
        this.NonPublicRequiredNumber = nonPublicRequiredNumber;
        this.SetNonPublicOptionalNumber(nonPublicOptionalNumber);
    }

    public long? NonPublicOptionalNumberValue => this._nonPublicOptionalNumber;

    public long NonPublicRequiredNumberValue => this.NonPublicRequiredNumber;

    public void SetNonPublicOptionalNumber(long? value) => this._nonPublicOptionalNumber = value;

    public string RequiredName { get; set; }

    public long RequiredNumber { get; set; }

    public bool RequiredPredicate { get; set; }

    public override string ToString()
    {
        var requiredName = this.RequiredName.SafeToString();
        var requiredNumber = this.RequiredNumber.SafeToString();
        var requiredPredicate = this.RequiredPredicate.SafeToString();
        var optionalName = this.OptionalName.SafeToString();
        var optionalNumber = this.OptionalNumber.SafeToString();
        var optionalPredicate = this.OptionalPredicate.SafeToString();

        return $"{nameof(ScalarsOnly)} {{{nameof(this.RequiredName)}={requiredName}, " +
            $"{nameof(this.RequiredNumber)}={requiredNumber}, " +
            $"{nameof(this.RequiredPredicate)}={requiredPredicate}, " +
            $"{nameof(this.OptionalName)}={optionalName}, " +
            $"{nameof(this.OptionalNumber)}={optionalNumber}, " +
            $"{nameof(this.OptionalPredicate)}={optionalPredicate}}}";
    }
}

public static class StaticScalarsOnly
{
    private static long? _nonPublicOptionalNumber;

    private static long NonPublicRequiredNumber { get; set; }

    public const string NonPublicOptionalNumberFieldName = nameof(_nonPublicOptionalNumber);

    public const string NonPublicRequiredNumberPropertyName = nameof(NonPublicRequiredNumber);

    public static long? NonPublicOptionalNumberValue => _nonPublicOptionalNumber;

    public static long NonPublicRequiredNumberValue => NonPublicRequiredNumber;

    public static void SetNonPublicOptionalNumber(long? value) => _nonPublicOptionalNumber = value;

    public static string? OptionalName;

    public static long? OptionalNumber;

    public static bool? OptionalPredicate;

    public static string RequiredName { get; set; } = string.Empty;

    public static long RequiredNumber { get; set; }

    public static bool RequiredPredicate { get; set; }
}

public class MemberAccessBaseShape
{
    public string AmbiguousValue { get; set; } = "base";

    public string InheritedValue { get; set; } = "inherited";
}

public sealed class MemberAccessShape : MemberAccessBaseShape
{
    private string _writeOnlyValue = string.Empty;
    private int _getterInvocationCount;

    public const long ConstantField = 11;

    public long FieldValue = 12;

    public long? FieldNullableValue;

    public string? FieldNullableString;

    public readonly long ReadonlyField = 12;

    public long ReadonlyProperty { get; } = 12;

    private readonly string _privateReadonlyField = "private-readonly";

    private string _privateField = "private-field";

    private string PrivateFieldValue => _privateField;

    public new long AmbiguousValue { get; set; } = 13;

    public long CacheValue { get; set; } = 14;

    public long? CacheNullableValue { get; set; }

    public long ConcurrentCacheValue { get; set; } = 24;

    public long ConcurrentAccessorCacheValue { get; set; } = 26;

    public DateTime DateValue { get; set; } = new(2025, 6, 7, 8, 9, 10, DateTimeKind.Utc);

    public string InitOnlyValue { get; init; } = "init";

    public int GetterInvocationCount => _getterInvocationCount;

    public long InvocationTrackedValue
    {
        get
        {
            _getterInvocationCount++;
            return 42;
        }
    }

    public string PrivateGetterValue { private get; set; } = "private-getter";

    public string PrivateSetterValue { get; private set; } = "private-setter";

    private string PrivateValue { get; set; } = "private-property";

    public string ReadOnlyValue => "read-only";

    public string TextValue { get; set; } = "text";

    public string? TextNullableValue { get; set; } = "text";

    public string WriteOnlyValue
    {
        set => _writeOnlyValue = value;
    }

    public string WrittenValue => _writeOnlyValue;

    public string this[int index] => index.ToString();

    public static long StaticCacheValue { get; set; } = 15;

    public static long StaticConcurrentCacheValue { get; set; } = 25;

    public static long StaticConcurrentAccessorCacheValue { get; set; } = 27;

    public static int StaticGetterInvocationCount { get; private set; }

    public static long StaticInvocationTrackedValue
    {
        get
        {
            StaticGetterInvocationCount++;
            return 43;
        }
    }

    public static void ResetStaticGetterInvocationCount() => StaticGetterInvocationCount = 0;

    public static DateTime StaticDateValue { get; set; } =
        new(2025, 6, 7, 8, 9, 10, DateTimeKind.Utc);

#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static string StaticTextField = "static-field";
#pragma warning restore CA2211 // Non-constant fields should not be visible

    public static string StaticTextValue { get; set; } = "static-text";

    private static readonly string _privateStaticReadonlyField = "private-static-readonly";

    private static string? _secretToken;

    // Write-only static property
    public static string SecretToken
    {
        set { _secretToken = value; } // No "get" accessor allowed here
    }

    public MemberAccessShape()
    {
        if (_privateReadonlyField == "private-readonly" && _privateStaticReadonlyField != null)
        {
            // Do something with the private static readonly field
            _privateField = "modified-private-field";
        }
    }

    public void Foo()
    {
        _privateField = "modified-private-field";

        if (_privateReadonlyField == "private-readonly" && _privateStaticReadonlyField != null)
        {
            // Do something with the private static readonly field
            _privateField = "modified-private-field";
        }
    }
}

public sealed class NullableValueMembers
{
    public long RequiredField;

    public long? OptionalField;

#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static long StaticRequiredField;

    public static long? StaticOptionalField;
#pragma warning restore CA2211 // Non-constant fields should not be visible

    public long RequiredProperty { get; set; }

    public long? OptionalProperty { get; set; }

    public static long StaticRequiredProperty { get; set; }

    public static long? StaticOptionalProperty { get; set; }
}
