// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.Extensions;

using FluentAssertions;
using FluentAssertions.Equivalency;

namespace Evoogle.XUnit;

/// <summary>
///     Common base class for <see cref="XUnitTest" /> and <see cref="XUnitTestAsync" /> respectively.
/// </summary>
public abstract class XUnitTestBase
{
    #region XUnitTestBase Fields
    private HashSet<(Type DeclaringType, string Name)>? _excludeMembersSet;
    #endregion

    #region XUnitTestBase Properties
    /// <summary>Gets the display name of the test.</summary>
    public required string Name { get; init; }

    /// <summary>Gets the members to exclude from equivalency assertions.</summary>
    public IEnumerable<ExcludeMember>? ExcludeMembers { get; init; }

    /// <summary>
    ///     Gets a value indicating whether excluded members are written to the test output.
    ///     Defaults to <see langword="false"/>; opt in to enable this level of logging.
    /// </summary>
    public bool LogExcludedMembers { get; init; }

    /// <summary>
    ///     Gets a value indicating whether included members are written to the test output.
    ///     Defaults to <see langword="false"/>; opt in to enable this level of logging.
    /// </summary>
    public bool LogIncludedMembers { get; init; }

    /// <summary>Gets the parent test container instance while the test is running.</summary>
    protected XUnitTests? Parent { get; private set; }
    #endregion

    #region Object Methods
    /// <inheritdoc />
    public override string ToString()
    {
        return this.Name;
    }
    #endregion

    #region XUnitTestBase Methods
    /// <summary>Asserts that <paramref name="actual"/> is deeply equivalent to <paramref name="expected"/>, optionally excluding members specified by <see cref="ExcludeMembers"/>.</summary>
    /// <typeparam name="T">The type being compared.</typeparam>
    /// <param name="actual">The actual value produced by the test.</param>
    /// <param name="expected">The expected value to compare against.</param>
    protected void AssertBeEquivalentTo<T>(T? actual, T? expected)
    {
        if (expected == null)
        {
            actual.Should().BeNull();
            return;
        }

        if (this.ExcludeMembers == null)
        {
            actual.Should().BeEquivalentTo(expected, opt => opt.PreferringRuntimeMemberTypes());
            return;
        }

        actual.Should().BeEquivalentTo(
            expected,
            opt => opt
                .PreferringRuntimeMemberTypes()
                .Excluding(info => this.IsMemberExcluded(info)));
    }

    /// <summary>Writes a message to the test text output.</summary>
    protected void WriteLine(string? message = null) => this.Parent?.WriteLine(message ?? string.Empty);

    /// <summary>Writes a dashed line to the test text output.</summary>
    protected void WriteDashedLine() => this.Parent?.WriteDashedLine();

    /// <summary>Writes a double dashed line to the test text output.</summary>
    protected void WriteDoubleDashedLine() => this.Parent?.WriteDoubleDashedLine();

    internal void SetParent(XUnitTests parent) => this.Parent = parent;

    private bool IsMemberExcluded(IMemberInfo info)
    {
        _excludeMembersSet ??= this.ExcludeMembers!.Select(e => (e.DeclaringType, e.Name)).ToHashSet();

        var declaringType = info.DeclaringType;
        var name = info.Name;
        var isExcluded = _excludeMembersSet.Contains((declaringType, name));

        if (isExcluded && this.LogExcludedMembers)
        {
            this.WriteLine($"Excluded member: {declaringType.SafeToName()}.{name.SafeToString()}");
        }
        else if (!isExcluded && this.LogIncludedMembers)
        {
            this.WriteLine($"Included member: {declaringType.SafeToName()}.{name.SafeToString()}");
        }

        return isExcluded;
    }
    #endregion
}
