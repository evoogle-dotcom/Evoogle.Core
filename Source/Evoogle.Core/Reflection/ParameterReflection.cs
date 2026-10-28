// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Reflection;

using Evoogle.Reflection.Internal;

namespace Evoogle.Reflection;

/// <summary>
///     Reflection methods for the .NET <see cref="ParameterInfo"/> class.
/// </summary>
public static class ParameterReflection
{
    #region Methods
    /// <summary>
    ///     Gets nullability information for a method parameter or return parameter, including
    ///     recursive collection and element nullability.
    /// </summary>
    /// <param name="parameterInfo">The method parameter or return parameter to analyze.</param>
    /// <returns>The reflected parameter nullability information.</returns>
    public static MemberNullableInfo GetNullabilityInfo(ParameterInfo parameterInfo)
    {
        ArgumentNullException.ThrowIfNull(parameterInfo);

        var context = new NullabilityInfoContext();
        var nullabilityInfo = context.Create(parameterInfo);
        return NullableInfoReflection.GetNullabilityInfo(parameterInfo.ParameterType, nullabilityInfo);
    }
    #endregion
}
