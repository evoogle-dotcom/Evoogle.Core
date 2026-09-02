// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Evoogle.Extension;

/// <summary>
///     Base class that implements <see cref="IExtensible"/> and can freeze its extensions for concurrent reads.
/// </summary>
/// <remarks>
///     Freezing publishes deterministic insertion-order enumeration with frozen lookup and
///     permanently rejects all extension mutation operations.
/// </remarks>
public abstract class ExtensibleBase : IExtensible
{
    #region Fields
    private readonly OrderedDictionary<Type, object> _mutableExtensions = [];
    private IReadOnlyDictionary<Type, object>? _frozenExtensions;
    #endregion

    #region IReadOnlyExtensible Properties
    /// <inheritdoc />
    public long ExtensionCount => this.Extensions.Count;

    /// <inheritdoc />
    public IReadOnlyDictionary<Type, object> Extensions =>
        this._frozenExtensions ?? new OrderedExtensionView(this._mutableExtensions);
    #endregion

    #region Properties
    /// <summary>
    ///     Gets a value indicating whether extension mutation has been permanently disabled.
    /// </summary>
    protected bool AreExtensionsFrozen => this._frozenExtensions != null;
    #endregion

    #region IExtensible Methods
    /// <inheritdoc />
    public void AttachExtension(Type extensionType, object extension)
    {
        ArgumentNullException.ThrowIfNull(extensionType);
        ArgumentNullException.ThrowIfNull(extension);

        this.ThrowIfExtensionsFrozen();
        this._mutableExtensions.Add(extensionType, extension);
    }

    /// <inheritdoc />
    public object? DetachExtension(Type extensionType)
    {
        ArgumentNullException.ThrowIfNull(extensionType);

        this.ThrowIfExtensionsFrozen();
        return this._mutableExtensions.Remove(extensionType, out var extension) ? extension : null;
    }

    /// <inheritdoc />
    public bool TryGetExtension(Type extensionType, [NotNullWhen(true)] out object? extension)
    {
        ArgumentNullException.ThrowIfNull(extensionType);
        return this.Extensions.TryGetValue(extensionType, out extension);
    }
    #endregion

    #region Methods
    /// <summary>
    ///     Permanently freezes the currently attached extensions for lock-free concurrent reads.
    /// </summary>
    protected void FreezeExtensions() => this.FreezeExtensions(this._mutableExtensions);

    /// <summary>
    ///     Permanently replaces the attached extensions with the supplied ordered snapshot and freezes them.
    /// </summary>
    /// <param name="extensions">The ordered extension snapshot to publish.</param>
    protected void FreezeExtensions(IEnumerable<KeyValuePair<Type, object>> extensions)
    {
        ArgumentNullException.ThrowIfNull(extensions);
        this.ThrowIfExtensionsFrozen();

        this._frozenExtensions = new FrozenExtensionDictionary(extensions);
        this._mutableExtensions.Clear();
    }
    #endregion

    #region Implementation Methods
    private void ThrowIfExtensionsFrozen()
    {
        if (this.AreExtensionsFrozen)
        {
            throw new InvalidOperationException("Extensions are frozen and cannot be modified.");
        }
    }

    internal void EnsureExtensionsMutable() => this.ThrowIfExtensionsFrozen();
    #endregion

    #region Types
    private sealed class OrderedExtensionView(OrderedDictionary<Type, object> extensions) : IReadOnlyDictionary<Type, object>
    {
        public int Count => extensions.Count;

        public IEnumerable<Type> Keys => extensions.Keys;

        public IEnumerable<object> Values => extensions.Values;

        public object this[Type key] => extensions[key];

        public bool ContainsKey(Type key) => extensions.ContainsKey(key);

        public IEnumerator<KeyValuePair<Type, object>> GetEnumerator() => extensions.GetEnumerator();

        public bool TryGetValue(Type key, [MaybeNullWhen(false)] out object value) =>
            extensions.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    private sealed class FrozenExtensionDictionary : IReadOnlyDictionary<Type, object>
    {
        private readonly ImmutableArray<KeyValuePair<Type, object>> _entries;
        private readonly FrozenDictionary<Type, object> _lookup;

        public FrozenExtensionDictionary(IEnumerable<KeyValuePair<Type, object>> extensions)
        {
            this._entries = [.. extensions];
            this._lookup = this._entries.ToFrozenDictionary(pair => pair.Key, pair => pair.Value);
        }

        public int Count => this._entries.Length;

        public IEnumerable<Type> Keys => this._entries.Select(pair => pair.Key);

        public IEnumerable<object> Values => this._entries.Select(pair => pair.Value);

        public object this[Type key] => this._lookup[key];

        public bool ContainsKey(Type key) => this._lookup.ContainsKey(key);

        public IEnumerator<KeyValuePair<Type, object>> GetEnumerator() =>
            ((IEnumerable<KeyValuePair<Type, object>>)this._entries).GetEnumerator();

        public bool TryGetValue(Type key, [MaybeNullWhen(false)] out object value) =>
            this._lookup.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
    #endregion
}
