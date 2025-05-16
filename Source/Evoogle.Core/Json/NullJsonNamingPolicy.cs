// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;

namespace Evoogle.Json;

/// <summary>
///     A null object implementation of <see cref="JsonNamingPolicy"/> that performs no conversion on property names.
///     This class can be used when no naming policy is desired, effectively leaving property names unchanged during JSON serialization and deserialization.
/// </summary>
/// <remarks>
///     This policy is useful in scenarios where the default naming behavior is desired, or when integrating with systems that expect property names to match exactly.
///     It can also serve as a fallback or default policy when no specific naming transformation is required.
/// </remarks>
public class NullJsonNamingPolicy : JsonNamingPolicy
{
    #region JsonNamingPolicy Methods
    /// <summary>
    ///     Overrides the <see cref="JsonNamingPolicy.ConvertName"/> method to return the input name without any conversion.
    ///     This ensures that property names remain unchanged during JSON operations.
    /// </summary>
    /// <param name="name">The name to convert.</param>
    /// <returns>The original name, unchanged.</returns>    
    public override string ConvertName(string name)
    {
        return name;
    }
    #endregion
}