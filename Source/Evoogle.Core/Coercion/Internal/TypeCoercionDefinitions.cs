// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.Reflection;

namespace Evoogle.Coercion.Internal;

/// <summary>
///     This API supports the Evoogle.Core infrastructure and is not intended to be used directly from your code.
///     This API may change or be removed in future releases.
/// </summary>
internal partial class TypeCoercion : ITypeCoercion
{
    #region Fields
    private static readonly ITypeCoercionDefinition[] BuiltInDefinitions =
    {
            // Simple Types /////////////////////////////////////////////

            // Bool To XXX
            new TypeCoercionDefinitionFunc<bool, bool>((input,    context) => input),
            new TypeCoercionDefinitionFunc<bool, byte>((input,    context) => Convert.ToByte(input)),
            new TypeCoercionDefinitionFunc<bool, char>((input,    context) => input ? (char)1 : (char)0),
            new TypeCoercionDefinitionFunc<bool, decimal>((input, context) => Convert.ToDecimal(input)),
            new TypeCoercionDefinitionFunc<bool, double>((input,  context) => Convert.ToDouble(input)),
            new TypeCoercionDefinitionFunc<bool, float>((input,   context) => Convert.ToSingle(input)),
            new TypeCoercionDefinitionFunc<bool, int>((input,     context) => Convert.ToInt32(input)),
            new TypeCoercionDefinitionFunc<bool, long>((input,    context) => Convert.ToInt64(input)),
            new TypeCoercionDefinitionFunc<bool, sbyte>((input,   context) => Convert.ToSByte(input)),
            new TypeCoercionDefinitionFunc<bool, short>((input,   context) => Convert.ToInt16(input)),
            new TypeCoercionDefinitionFunc<bool, string>((input,  context) => Convert.ToString(input, context.GetFormatProvider(typeof(bool))).ToLowerInvariant()),
            new TypeCoercionDefinitionFunc<bool, uint>((input,    context) => Convert.ToUInt32(input)),
            new TypeCoercionDefinitionFunc<bool, ulong>((input,   context) => Convert.ToUInt64(input)),
            new TypeCoercionDefinitionFunc<bool, ushort>((input,  context) => Convert.ToUInt16(input)),

            // Byte To XXX
            new TypeCoercionDefinitionFunc<byte, bool>((input,    context) => Convert.ToBoolean(input)),
            new TypeCoercionDefinitionFunc<byte, byte>((input,    context) => input),
            new TypeCoercionDefinitionFunc<byte, char>((input,    context) => Convert.ToChar(input)),
            new TypeCoercionDefinitionFunc<byte, decimal>((input, context) => Convert.ToDecimal(input)),
            new TypeCoercionDefinitionFunc<byte, double>((input,  context) => Convert.ToDouble(input)),
            new TypeCoercionDefinitionFunc<byte, float>((input,   context) => Convert.ToSingle(input)),
            new TypeCoercionDefinitionFunc<byte, int>((input,     context) => Convert.ToInt32(input)),
            new TypeCoercionDefinitionFunc<byte, long>((input,    context) => Convert.ToInt64(input)),
            new TypeCoercionDefinitionFunc<byte, sbyte>((input,   context) => Convert.ToSByte(input)),
            new TypeCoercionDefinitionFunc<byte, short>((input,   context) => Convert.ToInt16(input)),
            new TypeCoercionDefinitionFunc<byte, string>((input,  context) => Convert.ToString(input, context.GetFormatProvider(typeof(bool)))),
            new TypeCoercionDefinitionFunc<byte, uint>((input,    context) => Convert.ToUInt32(input)),
            new TypeCoercionDefinitionFunc<byte, ulong>((input,   context) => Convert.ToUInt64(input)),
            new TypeCoercionDefinitionFunc<byte, ushort>((input,  context) => Convert.ToUInt16(input)),

            // ByteArray To XXX
            new TypeCoercionDefinitionFunc<byte[], byte[]>((input, context) => input),
            new TypeCoercionDefinitionFunc<byte[], Guid>((input,   context) => new Guid(input)),
            new TypeCoercionDefinitionFunc<byte[], string>((input, context) => Convert.ToBase64String(input)),

            // Char To XXX
            new TypeCoercionDefinitionFunc<char, bool>((input,    context) => Convert.ToBoolean(Convert.ToInt32(input))),
            new TypeCoercionDefinitionFunc<char, byte>((input,    context) => Convert.ToByte(input)),
            new TypeCoercionDefinitionFunc<char, char>((input,    context) => input),
            new TypeCoercionDefinitionFunc<char, decimal>((input, context) => Convert.ToDecimal(Convert.ToInt32(input))),
            new TypeCoercionDefinitionFunc<char, double>((input,  context) => Convert.ToDouble(Convert.ToInt32(input))),
            new TypeCoercionDefinitionFunc<char, float>((input,   context) => Convert.ToSingle(Convert.ToInt32(input))),
            new TypeCoercionDefinitionFunc<char, int>((input,     context) => Convert.ToInt32(input)),
            new TypeCoercionDefinitionFunc<char, long>((input,    context) => Convert.ToInt64(input)),
            new TypeCoercionDefinitionFunc<char, sbyte>((input,   context) => Convert.ToSByte(input)),
            new TypeCoercionDefinitionFunc<char, short>((input,   context) => Convert.ToInt16(input)),
            new TypeCoercionDefinitionFunc<char, string>((input,  context) => Convert.ToString(input, context.GetFormatProvider(typeof(char)))),
            new TypeCoercionDefinitionFunc<char, uint>((input,    context) => Convert.ToUInt32(input)),
            new TypeCoercionDefinitionFunc<char, ulong>((input,   context) => Convert.ToUInt64(input)),
            new TypeCoercionDefinitionFunc<char, ushort>((input,  context) => Convert.ToUInt16(input)),

            // DateTime To XXX
            new TypeCoercionDefinitionFunc<DateTime, DateTime>((input,       context) => input),
            new TypeCoercionDefinitionFunc<DateTime, DateTimeOffset>((input, context) => (DateTimeOffset)input),
            new TypeCoercionDefinitionFunc<DateTime, string>(ConvertDateTimeToString),

            // DateTimeOffset To XXX
            new TypeCoercionDefinitionFunc<DateTimeOffset, DateTime>((input,       context) => input.DateTime),
            new TypeCoercionDefinitionFunc<DateTimeOffset, DateTimeOffset>((input, context) => input),
            new TypeCoercionDefinitionFunc<DateTimeOffset, string>(ConvertDateTimeOffsetToString),

            // Decimal To XXX
            new TypeCoercionDefinitionFunc<decimal, bool>((input,    context) => Convert.ToBoolean(input)),
            new TypeCoercionDefinitionFunc<decimal, byte>((input,    context) => Convert.ToByte(input)),
            new TypeCoercionDefinitionFunc<decimal, char>((input,    context) => (char)input),
            new TypeCoercionDefinitionFunc<decimal, decimal>((input, context) => input),
            new TypeCoercionDefinitionFunc<decimal, double>((input,  context) => Convert.ToDouble(input)),
            new TypeCoercionDefinitionFunc<decimal, float>((input,   context) => Convert.ToSingle(input)),
            new TypeCoercionDefinitionFunc<decimal, int>((input,     context) => Convert.ToInt32(input)),
            new TypeCoercionDefinitionFunc<decimal, long>((input,    context) => Convert.ToInt64(input)),
            new TypeCoercionDefinitionFunc<decimal, sbyte>((input,   context) => Convert.ToSByte(input)),
            new TypeCoercionDefinitionFunc<decimal, short>((input,   context) => Convert.ToInt16(input)),
            new TypeCoercionDefinitionFunc<decimal, string>((input,  context) => Convert.ToString(input, context.GetFormatProvider(typeof(decimal)))),
            new TypeCoercionDefinitionFunc<decimal, uint>((input,    context) => Convert.ToUInt32(input)),
            new TypeCoercionDefinitionFunc<decimal, ulong>((input,   context) => Convert.ToUInt64(input)),
            new TypeCoercionDefinitionFunc<decimal, ushort>((input,  context) => Convert.ToUInt16(input)),

            // Double To XXX
            new TypeCoercionDefinitionFunc<double, bool>((input,    context) => Convert.ToBoolean(input)),
            new TypeCoercionDefinitionFunc<double, byte>((input,    context) => Convert.ToByte(input)),
            new TypeCoercionDefinitionFunc<double, char>((input,    context) => (char)input),
            new TypeCoercionDefinitionFunc<double, decimal>((input, context) => Convert.ToDecimal(input)),
            new TypeCoercionDefinitionFunc<double, double>((input,  context) => input),
            new TypeCoercionDefinitionFunc<double, float>((input,   context) => Convert.ToSingle(input)),
            new TypeCoercionDefinitionFunc<double, int>((input,     context) => Convert.ToInt32(input)),
            new TypeCoercionDefinitionFunc<double, long>((input,    context) => Convert.ToInt64(input)),
            new TypeCoercionDefinitionFunc<double, sbyte>((input,   context) => Convert.ToSByte(input)),
            new TypeCoercionDefinitionFunc<double, short>((input,   context) => Convert.ToInt16(input)),
            new TypeCoercionDefinitionFunc<double, string>((input,  context) => Convert.ToString(input, context.GetFormatProvider(typeof(double)))),
            new TypeCoercionDefinitionFunc<double, uint>((input,    context) => Convert.ToUInt32(input)),
            new TypeCoercionDefinitionFunc<double, ulong>((input,   context) => Convert.ToUInt64(input)),
            new TypeCoercionDefinitionFunc<double, ushort>((input,  context) => Convert.ToUInt16(input)),

            // Float To XXX
            new TypeCoercionDefinitionFunc<float, bool>((input,    context) => Convert.ToBoolean(input)),
            new TypeCoercionDefinitionFunc<float, byte>((input,    context) => Convert.ToByte(input)),
            new TypeCoercionDefinitionFunc<float, char>((input,    context) => (char)input),
            new TypeCoercionDefinitionFunc<float, decimal>((input, context) => Convert.ToDecimal(input)),
            new TypeCoercionDefinitionFunc<float, double>((input,  context) => Convert.ToDouble(input)),
            new TypeCoercionDefinitionFunc<float, float>((input,   context) => input),
            new TypeCoercionDefinitionFunc<float, int>((input,     context) => Convert.ToInt32(input)),
            new TypeCoercionDefinitionFunc<float, long>((input,    context) => Convert.ToInt64(input)),
            new TypeCoercionDefinitionFunc<float, sbyte>((input,   context) => Convert.ToSByte(input)),
            new TypeCoercionDefinitionFunc<float, short>((input,   context) => Convert.ToInt16(input)),
            new TypeCoercionDefinitionFunc<float, string>((input,  context) => Convert.ToString(input, context.GetFormatProvider(typeof(float)))),
            new TypeCoercionDefinitionFunc<float, uint>((input,    context) => Convert.ToUInt32(input)),
            new TypeCoercionDefinitionFunc<float, ulong>((input,   context) => Convert.ToUInt64(input)),
            new TypeCoercionDefinitionFunc<float, ushort>((input,  context) => Convert.ToUInt16(input)),

            // Guid To XXX
            new TypeCoercionDefinitionFunc<Guid, byte[]>((input, context) => input.ToByteArray()),
            new TypeCoercionDefinitionFunc<Guid, Guid>((input,   context) => input),
            new TypeCoercionDefinitionFunc<Guid, string>(ConvertGuidToString),
            new TypeCoercionDefinitionFunc<Guid, Ulid>((input, context) => new Ulid(input)),

            // Int To XXX
            new TypeCoercionDefinitionFunc<int, bool>((input,    context) => Convert.ToBoolean(input)),
            new TypeCoercionDefinitionFunc<int, byte>((input,    context) => Convert.ToByte(input)),
            new TypeCoercionDefinitionFunc<int, char>((input,    context) => Convert.ToChar(input)),
            new TypeCoercionDefinitionFunc<int, decimal>((input, context) => Convert.ToDecimal(input)),
            new TypeCoercionDefinitionFunc<int, double>((input,  context) => Convert.ToDouble(input)),
            new TypeCoercionDefinitionFunc<int, float>((input,   context) => Convert.ToSingle(input)),
            new TypeCoercionDefinitionFunc<int, int>((input,     context) => input),
            new TypeCoercionDefinitionFunc<int, long>((input,    context) => Convert.ToInt64(input)),
            new TypeCoercionDefinitionFunc<int, sbyte>((input,   context) => Convert.ToSByte(input)),
            new TypeCoercionDefinitionFunc<int, short>((input,   context) => Convert.ToInt16(input)),
            new TypeCoercionDefinitionFunc<int, string>((input,  context) => Convert.ToString(input, context.GetFormatProvider(typeof(int)))),
            new TypeCoercionDefinitionFunc<int, uint>((input,    context) => Convert.ToUInt32(input)),
            new TypeCoercionDefinitionFunc<int, ulong>((input,   context) => Convert.ToUInt64(input)),
            new TypeCoercionDefinitionFunc<int, ushort>((input,  context) => Convert.ToUInt16(input)),

            // Long To XXX
            new TypeCoercionDefinitionFunc<long, bool>((input,    context) => Convert.ToBoolean(input)),
            new TypeCoercionDefinitionFunc<long, byte>((input,    context) => Convert.ToByte(input)),
            new TypeCoercionDefinitionFunc<long, char>((input,    context) => Convert.ToChar(input)),
            new TypeCoercionDefinitionFunc<long, decimal>((input, context) => Convert.ToDecimal(input)),
            new TypeCoercionDefinitionFunc<long, double>((input,  context) => Convert.ToDouble(input)),
            new TypeCoercionDefinitionFunc<long, float>((input,   context) => Convert.ToSingle(input)),
            new TypeCoercionDefinitionFunc<long, int>((input,     context) => Convert.ToInt32(input)),
            new TypeCoercionDefinitionFunc<long, long>((input,    context) => input),
            new TypeCoercionDefinitionFunc<long, sbyte>((input,   context) => Convert.ToSByte(input)),
            new TypeCoercionDefinitionFunc<long, short>((input,   context) => Convert.ToInt16(input)),
            new TypeCoercionDefinitionFunc<long, string>((input,  context) => Convert.ToString(input, context.GetFormatProvider(typeof(long)))),
            new TypeCoercionDefinitionFunc<long, uint>((input,    context) => Convert.ToUInt32(input)),
            new TypeCoercionDefinitionFunc<long, ulong>((input,   context) => Convert.ToUInt64(input)),
            new TypeCoercionDefinitionFunc<long, ushort>((input,  context) => Convert.ToUInt16(input)),

            // SByte To XXX
            new TypeCoercionDefinitionFunc<sbyte, bool>((input,    context) => Convert.ToBoolean(input)),
            new TypeCoercionDefinitionFunc<sbyte, byte>((input,    context) => Convert.ToByte(input)),
            new TypeCoercionDefinitionFunc<sbyte, char>((input,    context) => Convert.ToChar(input)),
            new TypeCoercionDefinitionFunc<sbyte, decimal>((input, context) => Convert.ToDecimal(input)),
            new TypeCoercionDefinitionFunc<sbyte, double>((input,  context) => Convert.ToDouble(input)),
            new TypeCoercionDefinitionFunc<sbyte, float>((input,   context) => Convert.ToSingle(input)),
            new TypeCoercionDefinitionFunc<sbyte, int>((input,     context) => Convert.ToInt32(input)),
            new TypeCoercionDefinitionFunc<sbyte, long>((input,    context) => Convert.ToInt64(input)),
            new TypeCoercionDefinitionFunc<sbyte, sbyte>((input,   context) => input),
            new TypeCoercionDefinitionFunc<sbyte, short>((input,   context) => Convert.ToInt16(input)),
            new TypeCoercionDefinitionFunc<sbyte, string>((input,  context) => Convert.ToString(input, context.GetFormatProvider(typeof(sbyte)))),
            new TypeCoercionDefinitionFunc<sbyte, uint>((input,    context) => Convert.ToUInt32(input)),
            new TypeCoercionDefinitionFunc<sbyte, ulong>((input,   context) => Convert.ToUInt64(input)),
            new TypeCoercionDefinitionFunc<sbyte, ushort>((input,  context) => Convert.ToUInt16(input)),

            // Short To XXX
            new TypeCoercionDefinitionFunc<short, bool>((input,    context) => Convert.ToBoolean(input)),
            new TypeCoercionDefinitionFunc<short, byte>((input,    context) => Convert.ToByte(input)),
            new TypeCoercionDefinitionFunc<short, char>((input,    context) => Convert.ToChar(input)),
            new TypeCoercionDefinitionFunc<short, decimal>((input, context) => Convert.ToDecimal(input)),
            new TypeCoercionDefinitionFunc<short, double>((input,  context) => Convert.ToDouble(input)),
            new TypeCoercionDefinitionFunc<short, float>((input,   context) => Convert.ToSingle(input)),
            new TypeCoercionDefinitionFunc<short, int>((input,     context) => Convert.ToInt32(input)),
            new TypeCoercionDefinitionFunc<short, long>((input,    context) => Convert.ToInt64(input)),
            new TypeCoercionDefinitionFunc<short, sbyte>((input,   context) => Convert.ToSByte(input)),
            new TypeCoercionDefinitionFunc<short, short>((input,   context) => input),
            new TypeCoercionDefinitionFunc<short, string>((input,  context) => Convert.ToString(input, context.GetFormatProvider(typeof(short)))),
            new TypeCoercionDefinitionFunc<short, uint>((input,    context) => Convert.ToUInt32(input)),
            new TypeCoercionDefinitionFunc<short, ulong>((input,   context) => Convert.ToUInt64(input)),
            new TypeCoercionDefinitionFunc<short, ushort>((input,  context) => Convert.ToUInt16(input)),

            // String To XXX
            new TypeCoercionDefinitionFunc<string, bool>((input,           context) => Convert.ToBoolean(input.ToLowerInvariant(), context.GetFormatProvider(typeof(bool)))),
            new TypeCoercionDefinitionFunc<string, byte>((input,           context) => Convert.ToByte(input, context.GetFormatProvider(typeof(byte)))),
            new TypeCoercionDefinitionFunc<string, byte[]>((input,         context) => Convert.FromBase64String(input)),
            new TypeCoercionDefinitionFunc<string, char>((input,           context) => Convert.ToChar(input, context.GetFormatProvider(typeof(char)))),
            new TypeCoercionDefinitionFunc<string, DateTime>(ConvertStringToDateTime),
            new TypeCoercionDefinitionFunc<string, DateTimeOffset>(ConvertStringToDateTimeOffset),
            new TypeCoercionDefinitionFunc<string, decimal>((input,        context) => Convert.ToDecimal(input, context.GetFormatProvider(typeof(decimal)))),
            new TypeCoercionDefinitionFunc<string, double>((input,         context) => Convert.ToDouble(input, context.GetFormatProvider(typeof(double)))),
            new TypeCoercionDefinitionFunc<string, float>((input,          context) => Convert.ToSingle(input, context.GetFormatProvider(typeof(float)))),
            new TypeCoercionDefinitionFunc<string, Guid>(ConvertStringToGuid),
            new TypeCoercionDefinitionFunc<string, int>((input,            context) => Convert.ToInt32(input, context.GetFormatProvider(typeof(int)))),
            new TypeCoercionDefinitionFunc<string, long>((input,           context) => Convert.ToInt64(input, context.GetFormatProvider(typeof(long)))),
            new TypeCoercionDefinitionFunc<string, sbyte>((input,          context) => Convert.ToSByte(input, context.GetFormatProvider(typeof(sbyte)))),
            new TypeCoercionDefinitionFunc<string, short>((input,          context) => Convert.ToInt16(input, context.GetFormatProvider(typeof(short)))),
            new TypeCoercionDefinitionFunc<string, string>((input,         context) => input),
            new TypeCoercionDefinitionFunc<string, TimeSpan>(ConvertStringToTimeSpan),
            new TypeCoercionDefinitionFunc<string, Type>(ConvertStringToType),
            new TypeCoercionDefinitionFunc<string, uint>((input,           context) => Convert.ToUInt32(input, context.GetFormatProvider(typeof(uint)))),
            new TypeCoercionDefinitionFunc<string, Ulid>(ConvertStringToUlid),
            new TypeCoercionDefinitionFunc<string, ulong>((input,          context) => Convert.ToUInt64(input, context.GetFormatProvider(typeof(ulong)))),
            new TypeCoercionDefinitionFunc<string, Uri>((input,            context) => new Uri(input, UriKind.RelativeOrAbsolute)),
            new TypeCoercionDefinitionFunc<string, ushort>((input,         context) => Convert.ToUInt16(input, context.GetFormatProvider(typeof(ushort)))),

            // TimeSpan To XXX
            new TypeCoercionDefinitionFunc<TimeSpan, string>(ConvertTimeSpanToString),
            new TypeCoercionDefinitionFunc<TimeSpan, TimeSpan>((input, context) => input),

            // Type To XXX
            new TypeCoercionDefinitionFunc<Type, string>(ConvertTypeToString),
            new TypeCoercionDefinitionFunc<Type, Type>((input, context) => input),

            // UInt To XXX
            new TypeCoercionDefinitionFunc<uint, bool>((input,    context) => Convert.ToBoolean(input)),
            new TypeCoercionDefinitionFunc<uint, byte>((input,    context) => Convert.ToByte(input)),
            new TypeCoercionDefinitionFunc<uint, char>((input,    context) => Convert.ToChar(input)),
            new TypeCoercionDefinitionFunc<uint, decimal>((input, context) => Convert.ToDecimal(input)),
            new TypeCoercionDefinitionFunc<uint, double>((input,  context) => Convert.ToDouble(input)),
            new TypeCoercionDefinitionFunc<uint, float>((input,   context) => Convert.ToSingle(input)),
            new TypeCoercionDefinitionFunc<uint, int>((input,     context) => Convert.ToInt32(input)),
            new TypeCoercionDefinitionFunc<uint, long>((input,    context) => Convert.ToInt64(input)),
            new TypeCoercionDefinitionFunc<uint, sbyte>((input,   context) => Convert.ToSByte(input)),
            new TypeCoercionDefinitionFunc<uint, short>((input,   context) => Convert.ToInt16(input)),
            new TypeCoercionDefinitionFunc<uint, string>((input,  context) => Convert.ToString(input, context.GetFormatProvider(typeof(uint)))),
            new TypeCoercionDefinitionFunc<uint, uint>((input,    context) => input),
            new TypeCoercionDefinitionFunc<uint, ulong>((input,   context) => Convert.ToUInt64(input)),
            new TypeCoercionDefinitionFunc<uint, ushort>((input,  context) => Convert.ToUInt16(input)),

            // Ulid To XXX
            new TypeCoercionDefinitionFunc<Ulid, byte[]>((input, context) => input.ToByteArray()),
            new TypeCoercionDefinitionFunc<Ulid, Guid>((input, context) => input.ToGuid()),
            new TypeCoercionDefinitionFunc<Ulid, Ulid>((input,   context) => input),
            new TypeCoercionDefinitionFunc<Ulid, string>(ConvertUlidToString),

            // ULong To XXX
            new TypeCoercionDefinitionFunc<ulong, bool>((input,    context) => Convert.ToBoolean(input)),
            new TypeCoercionDefinitionFunc<ulong, byte>((input,    context) => Convert.ToByte(input)),
            new TypeCoercionDefinitionFunc<ulong, char>((input,    context) => Convert.ToChar(input)),
            new TypeCoercionDefinitionFunc<ulong, decimal>((input, context) => Convert.ToDecimal(input)),
            new TypeCoercionDefinitionFunc<ulong, double>((input,  context) => Convert.ToDouble(input)),
            new TypeCoercionDefinitionFunc<ulong, float>((input,   context) => Convert.ToSingle(input)),
            new TypeCoercionDefinitionFunc<ulong, int>((input,     context) => Convert.ToInt32(input)),
            new TypeCoercionDefinitionFunc<ulong, long>((input,    context) => Convert.ToInt64(input)),
            new TypeCoercionDefinitionFunc<ulong, sbyte>((input,   context) => Convert.ToSByte(input)),
            new TypeCoercionDefinitionFunc<ulong, short>((input,   context) => Convert.ToInt16(input)),
            new TypeCoercionDefinitionFunc<ulong, string>((input,  context) => Convert.ToString(input, context.GetFormatProvider(typeof(ulong)))),
            new TypeCoercionDefinitionFunc<ulong, uint>((input,    context) => Convert.ToUInt32(input)),
            new TypeCoercionDefinitionFunc<ulong, ulong>((input,   context) => input),
            new TypeCoercionDefinitionFunc<ulong, ushort>((input,  context) => Convert.ToUInt16(input)),

            // Uri To XXX
            new TypeCoercionDefinitionFunc<Uri, string>((input, context) => input.ToString()),
            new TypeCoercionDefinitionFunc<Uri, Uri>((input,    context) => input),

            // UShort To XXX
            new TypeCoercionDefinitionFunc<ushort, bool>((input,    context) => Convert.ToBoolean(input)),
            new TypeCoercionDefinitionFunc<ushort, byte>((input,    context) => Convert.ToByte(input)),
            new TypeCoercionDefinitionFunc<ushort, char>((input,    context) => Convert.ToChar(input)),
            new TypeCoercionDefinitionFunc<ushort, decimal>((input, context) => Convert.ToDecimal(input)),
            new TypeCoercionDefinitionFunc<ushort, double>((input,  context) => Convert.ToDouble(input)),
            new TypeCoercionDefinitionFunc<ushort, float>((input,   context) => Convert.ToSingle(input)),
            new TypeCoercionDefinitionFunc<ushort, int>((input,     context) => Convert.ToInt32(input)),
            new TypeCoercionDefinitionFunc<ushort, long>((input,    context) => Convert.ToInt64(input)),
            new TypeCoercionDefinitionFunc<ushort, sbyte>((input,   context) => Convert.ToSByte(input)),
            new TypeCoercionDefinitionFunc<ushort, short>((input,   context) => Convert.ToInt16(input)),
            new TypeCoercionDefinitionFunc<ushort, string>((input,  context) => Convert.ToString(input, context.GetFormatProvider(typeof(ushort)))),
            new TypeCoercionDefinitionFunc<ushort, uint>((input,    context) => Convert.ToUInt32(input)),
            new TypeCoercionDefinitionFunc<ushort, ulong>((input,   context) => Convert.ToUInt64(input)),
            new TypeCoercionDefinitionFunc<ushort, ushort>((input,  context) => input)
        };
    #endregion

    #region XXX To String Methods
    private static string ConvertDateTimeToString(DateTime input, TypeCoercionContext context)
    {
        var format = context.GetFormat(typeof(DateTime));
        var formatProvider = context.GetFormatProvider(typeof(DateTime));

        return input.ToString(format, formatProvider);
    }

    private static string ConvertDateTimeOffsetToString(DateTimeOffset input, TypeCoercionContext context)
    {
        var format = context.GetFormat(typeof(DateTimeOffset));
        var formatProvider = context.GetFormatProvider(typeof(DateTimeOffset));

        return input.ToString(format, formatProvider);
    }

    private static string ConvertGuidToString(Guid input, TypeCoercionContext context)
    {
        var format = context.GetFormat(typeof(Guid));
        var formatProvider = context.GetFormatProvider(typeof(Guid));

        return input.ToString(format, formatProvider);
    }

    private static string ConvertTimeSpanToString(TimeSpan input, TypeCoercionContext context)
    {
        var format = context.GetFormat(typeof(TimeSpan));
        var formatProvider = context.GetFormatProvider(typeof(TimeSpan));

        return input.ToString(format, formatProvider);
    }

    private static string ConvertTypeToString(Type input, TypeCoercionContext context)
    {
        return TypeReflection.GetCompactQualifiedName(input);
    }

    private static string ConvertUlidToString(Ulid input, TypeCoercionContext context)
    {
        var format = context.GetFormat(typeof(Ulid));
        var formatProvider = context.GetFormatProvider(typeof(Ulid));

        return input.ToString(format, formatProvider);
    }
    #endregion

    #region String To XXX Methods
    private static DateTime ConvertStringToDateTime(string input, TypeCoercionContext context)
    {
        var format = context.GetFormat(typeof(DateTime));
        var formatProvider = context.GetFormatProvider(typeof(DateTime));
        var dateTimeStyles = context.GetDateTimeStyles(typeof(DateTime));

        return format == null
            ? DateTime.Parse(input, formatProvider, dateTimeStyles)
            : DateTime.ParseExact(input, format, formatProvider, dateTimeStyles);
    }

    private static DateTimeOffset ConvertStringToDateTimeOffset(string input, TypeCoercionContext context)
    {
        var format = context.GetFormat(typeof(DateTimeOffset));
        var formatProvider = context.GetFormatProvider(typeof(DateTimeOffset));
        var dateTimeOffsetStyles = context.GetDateTimeStyles(typeof(DateTimeOffset));

        return format == null
            ? DateTimeOffset.Parse(input, formatProvider, dateTimeOffsetStyles)
            : DateTimeOffset.ParseExact(input, format, formatProvider, dateTimeOffsetStyles);
    }

    private static Guid ConvertStringToGuid(string input, TypeCoercionContext context)
    {
        var formatProvider = context.GetFormatProvider(typeof(Guid));

        return Guid.Parse(input, formatProvider);
    }

    private static TimeSpan ConvertStringToTimeSpan(string input, TypeCoercionContext context)
    {
        var format = context.GetFormat(typeof(TimeSpan));
        var formatProvider = context.GetFormatProvider(typeof(TimeSpan));

        return format == null
            ? TimeSpan.Parse(input, formatProvider)
            : TimeSpan.ParseExact(input, format, formatProvider);
    }

    private static Type ConvertStringToType(string input, TypeCoercionContext context)
    {
        var type = Type.GetType(input, true);
        return type!;
    }

    private static Ulid ConvertStringToUlid(string input, TypeCoercionContext context)
    {
        var formatProvider = context.GetFormatProvider(typeof(Ulid));

        return Ulid.Parse(input, formatProvider);
    }
    #endregion
}
