// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Collections.Frozen;
using System.Globalization;

namespace Evoogle.Coercion;

/// <summary>
///     Immutable type coercion runtime context used when converting from one type to another.
/// </summary>
/// <remarks>
///     Mappings use frozen lookup tables and are safe for concurrent reads.
///     Non-culture format providers supplied by callers must themselves be immutable or thread-safe.
/// </remarks>
public sealed class TypeCoercionContext
{
    #region Fields
    /// <summary>
    ///     The default immutable type coercion context.
    /// </summary>
    public static readonly TypeCoercionContext Default = new();
    #endregion

    #region Constructors
    /// <summary>
    ///     Initializes a new context with the built-in mappings.
    /// </summary>
    public TypeCoercionContext()
        : this(CreateDefaultFormatMapping(), [], CreateDefaultDateTimeStylesMapping())
    {
    }

    internal TypeCoercionContext
    (
        IEnumerable<KeyValuePair<Type, string>> formatMapping,
        IEnumerable<KeyValuePair<Type, IFormatProvider>> formatProviderMapping,
        IEnumerable<KeyValuePair<Type, DateTimeStyles>> dateTimeStylesMapping
    )
    {
        this.FormatMapping = formatMapping.ToFrozenDictionary();
        this.FormatProviderMapping = formatProviderMapping.ToFrozenDictionary(
            pair => pair.Key,
            pair => NormalizeFormatProvider(pair.Value));
        this.DateTimeStylesMapping = dateTimeStylesMapping.ToFrozenDictionary();
    }
    #endregion

    #region Properties
    /// <summary>
    ///     Gets the format string on a per CLR type to use when coercing from or to text.
    /// </summary>
    public IReadOnlyDictionary<Type, string> FormatMapping { get; }

    /// <summary>
    ///     Gets the format provider on a per CLR type to use when coercing from or to text.
    /// </summary>
    public IReadOnlyDictionary<Type, IFormatProvider> FormatProviderMapping { get; }

    /// <summary>
    ///     Gets the date/time styles on a per CLR type to use when coercing from or to text.
    /// </summary>
    public IReadOnlyDictionary<Type, DateTimeStyles> DateTimeStylesMapping { get; }
    #endregion

    #region Methods
    /// <summary>
    ///     Retrieves the format string associated with the specified type.
    /// </summary>
    /// <param name="type">The CLR type for which to retrieve the format string.</param>
    /// <returns>The format string if found, otherwise <c>null</c>.</returns>
    public string? GetFormat(Type type) => this.FormatMapping.TryGetValue(type, out var format) ? format : null;

    /// <summary>
    ///     Retrieves the format provider associated with the specified type.
    /// </summary>
    /// <param name="type">The CLR type for which to retrieve the format provider.</param>
    /// <returns>The format provider if found, otherwise <c>null</c>.</returns>
    public IFormatProvider? GetFormatProvider(Type type) =>
        this.FormatProviderMapping.TryGetValue(type, out var formatProvider) ? formatProvider : null;

    /// <summary>
    ///     Retrieves the DateTime style associated with the specified type.
    /// </summary>
    /// <param name="type">The CLR type for which to retrieve the DateTime style.</param>
    /// <returns>The DateTime style if found, otherwise <see cref="DateTimeStyles.None"/>.</returns>
    public DateTimeStyles GetDateTimeStyles(Type type) =>
        this.DateTimeStylesMapping.TryGetValue(type, out var dateTimeStyles)
            ? dateTimeStyles
            : DateTimeStyles.None;
    #endregion

    #region Implementation Methods
    internal static Dictionary<Type, string> CreateDefaultFormatMapping() => new()
    {
        { typeof(DateTime), "O" },
        { typeof(DateTimeOffset), "O" },
        { typeof(Guid), "D" },
        { typeof(TimeSpan), "c" },
    };

    internal static Dictionary<Type, DateTimeStyles> CreateDefaultDateTimeStylesMapping() => new()
    {
        { typeof(DateTime), DateTimeStyles.RoundtripKind },
        { typeof(DateTimeOffset), DateTimeStyles.AssumeUniversal },
    };

    private static IFormatProvider NormalizeFormatProvider(IFormatProvider formatProvider)
    {
        ArgumentNullException.ThrowIfNull(formatProvider);

        if (formatProvider is not CultureInfo cultureInfo)
        {
            return formatProvider;
        }

        return CultureInfo.ReadOnly((CultureInfo)cultureInfo.Clone());
    }
    #endregion
}
