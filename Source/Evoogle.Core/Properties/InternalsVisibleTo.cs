// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Runtime.CompilerServices;

// Allow framework assemblies to be friend assemblies of this assembly

// Allow unit test assemblies to be friend assemblies of this assembly
[assembly: InternalsVisibleTo("Evoogle.Core.Tests")]
