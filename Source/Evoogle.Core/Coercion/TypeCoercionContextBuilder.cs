// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Globalization;

namespace Evoogle.Coercion;

/// <summary>
///     Builds immutable <see cref="TypeCoercionContext"/> instances.
/// </summary>
/// <remarks>
///     Builder instances are mutable and are not safe for concurrent use.
/// </remarks>
public sealed class TypeCoercionContextBuilder
{
    #region Fields
    private readonly Dictionary<Type, string> _formatMapping = TypeCoercionContext.CreateDefaultFormatMapping();
    private readonly Dictionary<Type, IFormatProvider> _formatProviderMapping = [];
    private readonly Dictionary<Type, DateTimeStyles> _dateTimeStylesMapping =
        TypeCoercionContext.CreateDefaultDateTimeStylesMapping();
    #endregion

    #region Methods
    /// <summary>
    ///     Sets the text format for a CLR type.
    /// </summary>
    public TypeCoercionContextBuilder SetFormat(Type type, string format)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(format);
        _formatMapping[type] = format;
        return this;
    }

    /// <summary>
    ///     Removes the text format for a CLR type.
    /// </summary>
    public TypeCoercionContextBuilder RemoveFormat(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        _formatMapping.Remove(type);
        return this;
    }

    /// <summary>
    ///     Removes all text formats.
    /// </summary>
    public TypeCoercionContextBuilder ClearFormats()
    {
        _formatMapping.Clear();
        return this;
    }

    /// <summary>
    ///     Sets the format provider for a CLR type.
    /// </summary>
    /// <remarks>
    ///     Culture providers are cloned into read-only instances when built. Other providers must be immutable or thread-safe.
    /// </remarks>
    public TypeCoercionContextBuilder SetFormatProvider(Type type, IFormatProvider formatProvider)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(formatProvider);
        _formatProviderMapping[type] = formatProvider;
        return this;
    }

    /// <summary>
    ///     Removes the format provider for a CLR type.
    /// </summary>
    public TypeCoercionContextBuilder RemoveFormatProvider(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        _formatProviderMapping.Remove(type);
        return this;
    }

    /// <summary>
    ///     Removes all format providers.
    /// </summary>
    public TypeCoercionContextBuilder ClearFormatProviders()
    {
        _formatProviderMapping.Clear();
        return this;
    }

    /// <summary>
    ///     Sets the date/time styles for a CLR type.
    /// </summary>
    public TypeCoercionContextBuilder SetDateTimeStyles(Type type, DateTimeStyles dateTimeStyles)
    {
        ArgumentNullException.ThrowIfNull(type);
        _dateTimeStylesMapping[type] = dateTimeStyles;
        return this;
    }

    /// <summary>
    ///     Removes the date/time styles for a CLR type.
    /// </summary>
    public TypeCoercionContextBuilder RemoveDateTimeStyles(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        _dateTimeStylesMapping.Remove(type);
        return this;
    }

    /// <summary>
    ///     Removes all date/time styles.
    /// </summary>
    public TypeCoercionContextBuilder ClearDateTimeStyles()
    {
        _dateTimeStylesMapping.Clear();
        return this;
    }

    /// <summary>
    ///     Creates an immutable context from defensive snapshots of the current mappings.
    /// </summary>
    public TypeCoercionContext Build() => new
    (
        _formatMapping,
        _formatProviderMapping,
        _dateTimeStylesMapping
    );
    #endregion
}
