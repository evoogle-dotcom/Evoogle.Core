// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.Extensions;

namespace Evoogle.MemberAccess;

public struct Point
{
    public long X;

    public long Y { get; set; }

    public string? Note { get; set; }

    public override readonly string ToString()
    {
        var x = this.X.SafeToString();
        var y = this.Y.SafeToString();
        var note = this.Note.SafeToString();

        return $"{nameof(Point)} {{{nameof(this.X)}={x}, {nameof(this.Y)}={y}, " +
            $"{nameof(this.Note)}={note}}}";
    }
}

public sealed class ScalarsOnly
{
    public string? OptionalName;
    public long? OptionalNumber;
    public bool? OptionalPredicate;

    public ScalarsOnly()
    {
        this.RequiredName = string.Empty;
    }

    public ScalarsOnly(string requiredName, long requiredNumber, bool requiredPredicate)
    {
        this.RequiredName = requiredName;
        this.RequiredNumber = requiredNumber;
        this.RequiredPredicate = requiredPredicate;
    }

    public string RequiredName { get; set; }

    public long RequiredNumber { get; set; }

    public bool RequiredPredicate { get; set; }

    public override string ToString()
    {
        var requiredName = this.RequiredName.SafeToString();
        var requiredNumber = this.RequiredNumber.SafeToString();
        var requiredPredicate = this.RequiredPredicate.SafeToString();
        var optionalName = this.OptionalName.SafeToString();
        var optionalNumber = this.OptionalNumber.SafeToString();
        var optionalPredicate = this.OptionalPredicate.SafeToString();

        return $"{nameof(ScalarsOnly)} {{{nameof(this.RequiredName)}={requiredName}, " +
            $"{nameof(this.RequiredNumber)}={requiredNumber}, " +
            $"{nameof(this.RequiredPredicate)}={requiredPredicate}, " +
            $"{nameof(this.OptionalName)}={optionalName}, " +
            $"{nameof(this.OptionalNumber)}={optionalNumber}, " +
            $"{nameof(this.OptionalPredicate)}={optionalPredicate}}}";
    }
}

public static class StaticScalarsOnly
{
    public static string? OptionalName;

    public static long? OptionalNumber;

    public static bool? OptionalPredicate;

    public static string RequiredName { get; set; } = string.Empty;

    public static long RequiredNumber { get; set; }

    public static bool RequiredPredicate { get; set; }
}
