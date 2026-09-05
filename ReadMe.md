# Repository

This repository hosts the **Evoogle** projects, maintained by [Evoogle](https://github.com/evoogle-dotcom) and licensed under the [MIT License](License.txt).

### Projects Included

* [Evoogle.Core](#evooglecore)
* [Evoogle.XUnit](#evooglexunit)

## Evoogle.Core

**Evoogle.Core** is a foundational library offering C# extensions and core components. It includes:

* **Cloneable**: A deep cloning framework with the following built-in implementation:
  * JSON-based cloning via serialization/deserialization of the source object.
* **Coercion**: A library for implicit type coercion. `TypeCoercionContextBuilder` creates immutable contexts backed by frozen lookup maps; the built-in coercion definitions are frozen for concurrent lookup.
* **Extension**: A dynamic extension framework with separate read-only and mutable interfaces.
  `ExtensibleBase` can publish a deterministic insertion-ordered frozen snapshot and permanently reject mutation while retaining lock-free lookup.
* **Extensions**: A set of helpful extension methods for core .NET types.
* **Json**: A utility library and set of extensions that simplify working with the `System.Text.Json` library.
* **Logging**: A flexible and extensible logging utility that enables developers to log messages to multiple destinations simultaneously, making it ideal for both runtime applications and diagnostics during development or testing.
* **NTree**: An N-ary tree implementation with read-only structural interfaces,
  optional named nodes, interface-driven traversal, and a mutable node base class.
* **Reflection**: Utility classes to streamline working with .NET reflection APIs.

## Evoogle.XUnit

**Evoogle.XUnit** is a unit testing framework built on [xUnit v3](https://github.com/xunit/xunit). It standardizes unit test structure by providing abstractions and base classes that break tests down into the classic unit test phases:

* *Arrange*
* *Act*
* *Assert*

## Local NuGet Packages

`Evoogle.Core` and `Evoogle.XUnit` packages are built into a machine-local NuGet folder feed.
They are not uploaded to GitHub Packages or another cloud registry, and each developer builds
their own packages.

### One-Time Feed Setup

Run the following commands in PowerShell once for each developer account:

```powershell
$feedPath = Join-Path $env:LOCALAPPDATA 'Evoogle\NuGetFeed'
New-Item -ItemType Directory -Path $feedPath -Force
dotnet nuget add source $feedPath --name EvoogleLocal
```

The source is registered in the user-level NuGet configuration. It contains no credentials and
is not stored in this repository. The feed is available only on that development machine unless
another machine independently builds a package or a shared feed is introduced later.

### Building a Local Package

From the repository root, run:

```powershell
.\scripts\Pack-EvoogleCore.ps1
```

The script runs the Release test suite, then packs both `Evoogle.Core` and `Evoogle.XUnit` into
`%LOCALAPPDATA%\Evoogle\NuGetFeed` with the same unique `0.1.0-local.<UTC timestamp>` version.
This avoids stale-package results from NuGet's global package cache. Use the exact version
printed by the script when adding either package to a consumer.

### Source-Level Debugging

Each local package includes a portable PDB alongside its DLL. The PDB retains paths to the source
checkout used to build the package, so Visual Studio can open that local source when stepping into
Core or XUnit without downloading it from a symbol server. Keep the checkout available and rebuild
the packages after changing source. If Visual Studio skips a library frame, confirm that its symbols
loaded in the Modules window, then disable Just My Code for that debugging session.
