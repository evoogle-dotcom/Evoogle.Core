// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.MemberAccess;

/// <summary>Represents a failure to create or invoke a property or field accessor.</summary>
public sealed class MemberAccessException : Exception
{
    #region Constructors
    /// <summary>Creates an exception with an explanatory message.</summary>
    /// <param name="message">The failure description.</param>
    public MemberAccessException(string message) : base(message)
    {
    }

    /// <summary>Creates an exception with an explanatory message and cause.</summary>
    /// <param name="message">The failure description.</param>
    /// <param name="innerException">The original failure.</param>
    public MemberAccessException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
    #endregion
}
