// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.Reflection;

/// <summary>
///     Represents the nullability state of a CLR data member (property, field, or collection element)
///     as determined by the .NET nullability reflection API.
/// </summary>
public enum MemberNullability
{
    /// <summary>
    ///     Nullability cannot be determined. This occurs for reference types in assemblies or contexts
    ///     that do not have nullable reference type (NRT) annotations enabled (<c>#nullable enable</c>).
    ///     No confident inference can be made from this state.
    /// </summary>
    Unknown,

    /// <summary>
    ///     The member is nullable. For reference types (e.g. <c>string?</c>) this requires NRT to be enabled.
    ///     For value types this means the type is <see cref="Nullable{T}"/> (e.g. <c>int?</c>).
    /// </summary>
    Nullable,

    /// <summary>
    ///     The member is non-nullable. For reference types this requires NRT to be enabled.
    ///     For value types (<see cref="ValueType"/>) this is always the case unless the type is <see cref="Nullable{T}"/>.
    /// </summary>
    NonNullable,
}
