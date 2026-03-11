// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Reflection;

namespace Evoogle.XUnit;

/// <summary>Identifies a member on a specific type to exclude from equivalency assertions.</summary>
public readonly record struct ExcludeMember
{
    #region Properties
    /// <summary>Gets the type that declares the member to exclude.</summary>
    /// <remarks>
    ///     This must be the <em>exact</em> declaring type as reported by
    ///     <see cref="FluentAssertions.Equivalency.IMemberInfo.DeclaringType"/>: the type on which the member is
    ///     originally declared, not any derived type. When a member is hidden in a subclass via the
    ///     <see langword="new"/> keyword, each shadowing declaration has its own declaring type; specify the type
    ///     that introduces the particular member you want to exclude.
    /// </remarks>
    public Type DeclaringType { get; }

    /// <summary>Gets the name of the member to exclude.</summary>
    public string Name { get; }
    #endregion

    #region Constructors
    /// <summary>Initializes a new <see cref="ExcludeMember"/> identifying the member to exclude.</summary>
    /// <param name="declaringType">The type that declares the member to exclude.</param>
    /// <param name="name">The name of the member to exclude.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="declaringType"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Thrown when <paramref name="name"/> is <see langword="null"/> or empty, or does not correspond to any
    ///     member declared on <paramref name="declaringType"/>.
    /// </exception>
    public ExcludeMember(Type declaringType, string name)
    {
        ArgumentNullException.ThrowIfNull(declaringType);
        ArgumentException.ThrowIfNullOrEmpty(name);

        const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        if (declaringType.GetMember(name, flags).Length == 0)
        {
            throw new ArgumentException($"Member '{name}' does not exist on type '{declaringType.FullName}'.", nameof(name));
        }

        this.DeclaringType = declaringType;
        this.Name = name;
    }
    #endregion
}
