// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle.NTree;

/// <summary>
///     Represents guidance on how to continue visiting the 1-N tree.
/// </summary>
public enum VisitResult
{
    /// <summary>
    ///     Visiting continues.
    /// </summary>
    Continue,

    /// <summary>
    ///     Visiting is done and should stop immediately.
    /// </summary>
    Done
}
