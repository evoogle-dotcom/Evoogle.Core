// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Globalization;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Text.Json.Serialization;

using Evoogle.Reflection;
using Evoogle.XUnit;

namespace Evoogle.Coercion;

[DynamicLinqType]
public class CoerceTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Types
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(BaseClass), typeDiscriminator: nameof(BaseClass))]
    [JsonDerivedType(typeof(DerivedClass), typeDiscriminator: nameof(DerivedClass))]
    public interface IInterface
    {
        string Name { get; }
    }

    public class BaseClass : IInterface
    {
        public string Name { get; set; }

        public BaseClass(string? name = default)
        {
            this.Name = name ?? nameof(BaseClass);
        }

        public override string ToString() { return this.Name; }
    }

    public class DerivedClass : BaseClass
    {
        public DerivedClass()
            : base(nameof(DerivedClass))
        {
        }
    }
    #endregion

    #region Test Data
    public const string DefaultDateTimeFormat = "O";
    public const string FullDateTimeFormat = "F";
    public static readonly IFormatProvider SpanishMexicoCulture = new CultureInfo("es-MX");

    public static readonly DateTime TestDateTime = new(1968, 5, 20, 20, 2, 42, 0, DateTimeKind.Utc);
    public static readonly string TestDateTimeString = TestDateTime.ToString(DefaultDateTimeFormat);
    public static readonly string TestDateTimeStringWithFormat = TestDateTime.ToString(FullDateTimeFormat);
    public static readonly string TestDateTimeStringWithFormatAndFormatProvider = TestDateTime.ToString(FullDateTimeFormat, SpanishMexicoCulture);

    public static readonly DateTimeOffset TestDateTimeOffset = new(1968, 5, 20, 20, 2, 42, 0, TimeSpan.Zero);
    public static readonly string TestDateTimeOffsetString = TestDateTimeOffset.ToString(DefaultDateTimeFormat);
    public static readonly string TestDateTimeOffsetStringWithFormat = TestDateTimeOffset.ToString(FullDateTimeFormat);
    public static readonly string TestDateTimeOffsetStringWithFormatAndFormatProvider = TestDateTimeOffset.ToString(FullDateTimeFormat, SpanishMexicoCulture);

    public const string DefaultTimeSpanFormat = "c";
    public const string GeneralShortTimeSpanFormat = "g";
    public static readonly IFormatProvider FrenchFranceCulture = new CultureInfo("fr-FR");

    public static readonly TimeSpan TestTimeSpan = new(42, 12, 24, 36, 123);
    public static readonly string TestTimeSpanString = TestTimeSpan.ToString(DefaultTimeSpanFormat);
    public static readonly string TestTimeSpanStringWithFormat = TestTimeSpan.ToString(GeneralShortTimeSpanFormat);
    public static readonly string TestTimeSpanStringWithFormatAndFormatProvider = TestTimeSpan.ToString(GeneralShortTimeSpanFormat, FrenchFranceCulture);

    public const string TestGuidString = "86d5d1a9-ec14-4730-8d9a-41812e5a117a";
    public static readonly Guid TestGuid = Guid.Parse(TestGuidString);
    public static readonly byte[] TestGuidByteArray = TestGuid.ToByteArray();

    public const string TestUriString = "https://api.example.com:8002/api/en-us/articles/42";
    public static readonly Uri TestUri = new(TestUriString);

    public static readonly byte[] TestByteArray = { 42, 24, 48, 84, 12, 21, 68, 86 };
    public const string TestByteArrayString = "KhgwVAwVRFY=";

    public static readonly Type TestType = typeof(CoerceTests);
    public static readonly string TestTypeString = TypeReflection.GetCompactQualifiedName(TestType)!;

    public const int TestRedOrdinal = 0;
    public const int TestGreenOrdinal = 1;
    public const int TestBlueOrdinal = 2;
    public const string TestBlueString = "Blue";
    public const string TestBlueLowercaseString = "blue";
    public const string IntegerEnumFormat = "D";
    public const string TestBlueOrdinalAsString = "2";

    public enum PrimaryColor
    {
        Red = TestRedOrdinal,
        Green = TestGreenOrdinal,
        Blue = TestBlueOrdinal
    };

    public static readonly BaseClass TestBaseClass = new();
    public static readonly DerivedClass TestDerivedClass = new();

    public const string TestUlidString = "46TQ8TKV0M8WR8V6J1G4Q5M4BT";
    public static readonly Ulid TestUlid = Ulid.Parse(TestUlidString);
    public static readonly byte[] TestUlidByteArray = TestUlid.ToByteArray();
    #endregion

    #region Test Methods
    public static TypeCoercionContext CreateTestDateTimeContextWithFormat()
    {
        var context = new TypeCoercionContext()
        {
            FormatMapping = new Dictionary<Type, string> { { typeof(DateTime), FullDateTimeFormat } },
            DateTimeStylesMapping = new Dictionary<Type, DateTimeStyles> { { typeof(DateTime), DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal } },
        };
        return context;
    }

    public static TypeCoercionContext CreateTestDateTimeContextWithFormatAndFormatProvider()
    {
        var context = new TypeCoercionContext()
        {
            FormatMapping = new Dictionary<Type, string> { { typeof(DateTime), FullDateTimeFormat } },
            FormatProviderMapping = new Dictionary<Type, IFormatProvider> { { typeof(DateTime), SpanishMexicoCulture } },
            DateTimeStylesMapping = new Dictionary<Type, DateTimeStyles> { { typeof(DateTime), DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal } },
        };
        return context;
    }

    public static TypeCoercionContext CreateTestDateTimeOffsetContextWithFormat()
    {
        var context = new TypeCoercionContext()
        {
            FormatMapping = new Dictionary<Type, string> { { typeof(DateTimeOffset), FullDateTimeFormat } },
            DateTimeStylesMapping = new Dictionary<Type, DateTimeStyles> { { typeof(DateTimeOffset), DateTimeStyles.AssumeUniversal } },
        };
        return context;
    }

    public static TypeCoercionContext CreateTestDateTimeOffsetContextWithFormatAndFormatProvider()
    {
        var context = new TypeCoercionContext()
        {
            FormatMapping = new Dictionary<Type, string> { { typeof(DateTimeOffset), FullDateTimeFormat } },
            FormatProviderMapping = new Dictionary<Type, IFormatProvider> { { typeof(DateTimeOffset), SpanishMexicoCulture } },
            DateTimeStylesMapping = new Dictionary<Type, DateTimeStyles> { { typeof(DateTimeOffset), DateTimeStyles.AssumeUniversal } },
        };
        return context;
    }

    public static TypeCoercionContext CreateTestTimeSpanContextWithFormat()
    {
        var context = new TypeCoercionContext()
        {
            FormatMapping = new Dictionary<Type, string> { { typeof(TimeSpan), GeneralShortTimeSpanFormat } },
        };
        return context;
    }

    public static TypeCoercionContext CreateTestTimeSpanContextWithFormatAndFormatProvider()
    {
        var context = new TypeCoercionContext()
        {
            FormatMapping = new Dictionary<Type, string> { { typeof(TimeSpan), GeneralShortTimeSpanFormat } },
            FormatProviderMapping = new Dictionary<Type, IFormatProvider> { { typeof(TimeSpan), FrenchFranceCulture } },
        };
        return context;
    }
    #endregion
}
