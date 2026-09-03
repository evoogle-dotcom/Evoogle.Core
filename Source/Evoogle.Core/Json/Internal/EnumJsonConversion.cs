// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Reflection;
using System.Runtime.Serialization;

namespace Evoogle.Json.Internal;

/// <summary>
///     This API supports the Evoogle.Core infrastructure and is not intended to be used directly from your code.
///     This API may change or be removed in future releases.
/// </summary>
internal static class EnumJsonConversion<TEnum>
    where TEnum : struct, Enum
{
    #region Fields
    private static readonly bool _hasFlags = typeof(TEnum).IsDefined(typeof(FlagsAttribute), inherit: false);

    private static readonly string[] _enumNames = Enum.GetNames<TEnum>();

    private static readonly Dictionary<string, string> _enumNamesByJsonName = CreateEnumNamesByJsonName();

    private static readonly Dictionary<string, string> _jsonNamesByEnumName = CreateJsonNamesByEnumName();
    #endregion

    #region Methods
    public static bool TryParse(string value, out TEnum enumeration, out EnumJsonParseFailure failure)
    {
        if (_hasFlags)
        {
            var parts = value.Split(',').Select(static part => part.Trim()).ToArray();
            if (!parts.All(TryGetEnumName))
            {
                enumeration = default;
                failure = EnumJsonParseFailure.InvalidFlagsValue;
                return false;
            }

            var enumNames = parts.Select(static part => _enumNamesByJsonName[part]);
            return TryParseEnum(string.Join(", ", enumNames), out enumeration, out failure);
        }

        if (value.Contains(','))
        {
            enumeration = default;
            failure = EnumJsonParseFailure.CommaInNonFlagsValue;
            return false;
        }

        if (!TryGetEnumName(value))
        {
            enumeration = default;
            failure = EnumJsonParseFailure.InvalidValue;
            return false;
        }

        return TryParseEnum(_enumNamesByJsonName[value], out enumeration, out failure);
    }

    public static string Format(TEnum value)
    {
        var enumText = value.ToString();
        if (!_hasFlags || !enumText.Contains(','))
        {
            return FormatSingleName(enumText);
        }

        var parts = enumText.Split(',').Select(static part => FormatSingleName(part.Trim()));
        return string.Join(", ", parts);
    }
    #endregion

    #region Implementation Methods
    private static Dictionary<string, string> CreateEnumNamesByJsonName()
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var enumName in _enumNames)
        {
            result[enumName] = enumName;

            var jsonName = GetJsonName(enumName);
            if (!string.IsNullOrWhiteSpace(jsonName))
            {
                result[jsonName] = enumName;
            }
        }

        return result;
    }

    private static Dictionary<string, string> CreateJsonNamesByEnumName()
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);

        foreach (var enumName in _enumNames)
        {
            result[enumName] = GetJsonName(enumName) ?? enumName;
        }

        return result;
    }

    private static string FormatSingleName(string enumName)
    {
        return _jsonNamesByEnumName.TryGetValue(enumName, out var jsonName) ? jsonName : enumName;
    }

    private static string? GetJsonName(string enumName)
    {
        var member = typeof(TEnum).GetMember(enumName).FirstOrDefault();
        var attribute = member?.GetCustomAttribute<EnumMemberAttribute>();
        return string.IsNullOrWhiteSpace(attribute?.Value) ? null : attribute.Value;
    }

    private static bool TryGetEnumName(string jsonName) => _enumNamesByJsonName.ContainsKey(jsonName);

    private static bool TryParseEnum(string value, out TEnum enumeration, out EnumJsonParseFailure failure)
    {
        if (Enum.TryParse<TEnum>(value, ignoreCase: true, out enumeration))
        {
            failure = EnumJsonParseFailure.None;
            return true;
        }

        failure = EnumJsonParseFailure.ParseFailure;
        return false;
    }
    #endregion
}
