// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.XUnit;

/// <summary>Identifies a member on a specific type to exclude from equivalency assertions.</summary>
/// <param name="DeclaringType">The type that declares the member to exclude.</param>
/// <param name="Name">The name of the member to exclude.</param>
public readonly record struct ExcludeMember(Type DeclaringType, string Name);
