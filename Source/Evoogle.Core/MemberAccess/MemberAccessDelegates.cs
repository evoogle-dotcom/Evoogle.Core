// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.Coercion;

namespace Evoogle.MemberAccess;

/// <summary>Sets a member on the original value-type instance.</summary>
/// <typeparam name="TObject">The value type declaring the member.</typeparam>
/// <typeparam name="TValue">The supplied value type.</typeparam>
/// <param name="target">The instance to modify.</param>
/// <param name="value">The value to assign.</param>
public delegate void MemberSetterByRef<TObject, in TValue>(ref TObject target, TValue value) where TObject : struct;

/// <summary>Sets a member on a value type, coercing the supplied value when needed.</summary>
/// <typeparam name="TObject">The value type declaring the member.</typeparam>
/// <typeparam name="TValue">The supplied value type.</typeparam>
/// <param name="target">The instance to modify.</param>
/// <param name="value">The value to assign.</param>
/// <param name="coercion">The coercion service.</param>
/// <param name="context">The optional coercion context.</param>
public delegate void CoercingMemberSetterByRef<TObject, in TValue>
(
    ref TObject target,
    TValue value,
    TypeCoercion coercion,
    TypeCoercionContext? context = null
) where TObject : struct;
