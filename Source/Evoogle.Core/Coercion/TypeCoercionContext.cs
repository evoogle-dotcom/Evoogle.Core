// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Globalization;

using Evoogle.Cloneable;

namespace Evoogle.Coercion;

/// <summary>
///     Type coercion runtime context when converting from one type to another type.
/// </summary>
public class TypeCoercionContext : DeepCloneable<TypeCoercionContext>
{
    #region Fields
    /// <summary>
    ///     A default type coercion context when needed.
    /// </summary>
    public static readonly TypeCoercionContext Default = new TypeCoercionContext();
    #endregion

    #region Properties
    /// <summary>
    ///     Gets the format string on a per CLR type to use when coercing from/to text.
    /// </summary>
    public Dictionary<Type, string> FormatMapping { get; set; } = new Dictionary<Type, string>
    {
        {typeof(DateTime), "O"},
        {typeof(DateTimeOffset), "O"},
        {typeof(Guid), "D"},
        {typeof(TimeSpan), "c"},
    };

    /// <summary>
    ///     Gets the format provider on a per CLR type to use when coercing from/to text.
    /// </summary>
    public Dictionary<Type, IFormatProvider> FormatProviderMapping { get; set; } = [];

    /// <summary>
    ///     Gets the date/time styles on a per CLR type to use when coercing from/to text.
    /// </summary>
    public Dictionary<Type, DateTimeStyles> DateTimeStylesMapping { get; set; } = new Dictionary<Type, DateTimeStyles>
    {
        {typeof(DateTime), DateTimeStyles.RoundtripKind},
        {typeof(DateTimeOffset), DateTimeStyles.AssumeUniversal},
    };
    #endregion

    #region Methods
    public string? GetFormat(Type type)
    {
        return this.FormatMapping.TryGetValue(type, out var format) ? format : null;
    }

    public IFormatProvider? GetFormatProvider(Type type)
    {
        return this.FormatProviderMapping.TryGetValue(type, out var formatProvider) ? formatProvider : null;
    }

    public DateTimeStyles GetDateTimeStyles(Type type)
    {
        return this.DateTimeStylesMapping.TryGetValue(type, out var dateTimeStyles) ? dateTimeStyles : DateTimeStyles.None;
    }
    #endregion
}
