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
    private HashSet<ExcludeMember>? _excludeMembersSet;

    private readonly HashSet<ExcludeMember> _excludedMembersSet = [];
    private readonly HashSet<ExcludeMember> _includedMembersSet = [];
    #endregion

    #region XUnitTestBase Properties
    /// <summary>Gets the display name of the test.</summary>
    public required string Name { get; init; }

    /// <summary>Gets the list of members to exclude from equivalency assertions.</summary>
    public List<ExcludeMember>? ExcludeMembers { get; init; }

    /// <summary>Gets the parent test container instance while the test is running.</summary>
    protected XUnitTests? Parent { get; private set; }
    #endregion

    #region XUnitTestBase Constructors
    /// <summary>Initializes a new instance, using the declaring class name as the test name when <paramref name="name"/> is not supplied.</summary>
    /// <param name="name">Optional explicit test name; defaults to the declaring class name.</param>
    protected XUnitTestBase(string? name = null)
    {
        this.Name = name ?? this.GetType().Name;
    }
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

        if (this.ExcludeMembers == null || this.ExcludeMembers.Count == 0)
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
        _excludeMembersSet ??= [.. this.ExcludeMembers!];

        var declaringType = info.DeclaringType;
        var name = info.Name;
        var fullMemberName = $"{declaringType.SafeToName()}.{name.SafeToString()}";
        var excludeMember = new ExcludeMember(declaringType, name);

        var isExcluded = _excludeMembersSet.Contains(excludeMember);
        if (isExcluded)
        {
            if (this._excludedMembersSet.Add(excludeMember))
            {
                this.WriteLine($"Excluded member: {fullMemberName}");
            }
        }
        else
        {
            if (this._includedMembersSet.Add(excludeMember))
            {
                this.WriteLine($"Included member: {fullMemberName}");
            }
        }

        return isExcluded;
    }
    #endregion
}
