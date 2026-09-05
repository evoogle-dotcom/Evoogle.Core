[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Invoke-DotNet
{
    param(
        [Parameter(Mandatory)]
        [string[]] $dotnetArguments)

    & dotnet @dotnetArguments

    if ($LASTEXITCODE -ne 0)
    {
        throw "dotnet $($dotnetArguments -join ' ') failed with exit code $LASTEXITCODE."
    }
}

function Test-PackageContents
{
    param(
        [Parameter(Mandatory)]
        [string] $packagePath,

        [Parameter(Mandatory)]
        [string] $packageId)

    if (-not (Test-Path -LiteralPath $packagePath))
    {
        throw "Packaging completed without creating '$packagePath'."
    }

    $packageArchive = [System.IO.Compression.ZipFile]::OpenRead($packagePath)

    try
    {
        $packageEntryNames = @($packageArchive.Entries | ForEach-Object { $_.FullName })
        $requiredPackageEntries = @(
            'ReadMe.md',
            "lib/net10.0/$packageId.dll",
            "lib/net10.0/$packageId.pdb",
            "lib/net10.0/$packageId.xml")
        $missingPackageEntries = @(
            $requiredPackageEntries | Where-Object { $_ -notin $packageEntryNames })

        if ($missingPackageEntries.Count -gt 0)
        {
            $message = "The package '$packagePath' is missing required entries: " + `
                ($missingPackageEntries -join ', ')
            throw $message
        }
    }
    finally
    {
        $packageArchive.Dispose()
    }
}

function Update-ApiFrameworkPackageVersions
{
    param(
        [Parameter(Mandatory)]
        [string] $coreRepositoryRoot,

        [Parameter(Mandatory)]
        [string] $packageVersion)

    $platformRoot = Split-Path -Parent $coreRepositoryRoot
    $apiFrameworkRepositoryRoot = Join-Path $platformRoot 'evoogle-apiframework'
    $apiFrameworkDirectoryBuildPropsPath = Join-Path `
        $apiFrameworkRepositoryRoot 'Directory.Build.props'

    if (-not (Test-Path -LiteralPath $apiFrameworkDirectoryBuildPropsPath))
    {
        Write-Host 'ApiFramework checkout was not found.'
        Write-Host 'No ApiFramework package-version pin was updated.'
        return
    }

    $apiFrameworkPackageVersionsPath = Join-Path `
        $apiFrameworkRepositoryRoot 'Directory.Build.evoogle-core.local.props'
    $packageVersionsContents = @"
<Project>
  <PropertyGroup>
    <EvoogleCorePackageVersion>$packageVersion</EvoogleCorePackageVersion>
    <EvoogleXUnitPackageVersion>$packageVersion</EvoogleXUnitPackageVersion>
  </PropertyGroup>
</Project>
"@ -replace "`r?`n", "`r`n"
    $utf8BomEncoding = [System.Text.UTF8Encoding]::new($true)

    [System.IO.File]::WriteAllText(
        $apiFrameworkPackageVersionsPath,
        $packageVersionsContents,
        $utf8BomEncoding)

    Write-Host "Updated ApiFramework package-version pin: $apiFrameworkPackageVersionsPath"
}

$repositoryRoot = Split-Path -Parent $PSScriptRoot
$solutionPath = Join-Path $repositoryRoot 'Evoogle.Core.sln'
$coreProjectPath = Join-Path $repositoryRoot 'Source\Evoogle.Core\Evoogle.Core.csproj'
$xUnitProjectPath = Join-Path $repositoryRoot 'Source\Evoogle.XUnit\Evoogle.XUnit.csproj'
$localApplicationDataPath = [Environment]::GetFolderPath(
    [Environment+SpecialFolder]::LocalApplicationData)
$feedPath = Join-Path $localApplicationDataPath 'Evoogle\NuGetFeed'
$versionSuffix = "local.$([DateTime]::UtcNow.ToString('yyyyMMddHHmmssfff'))"
$packageVersion = "0.1.0-$versionSuffix"
$corePackagePath = Join-Path $feedPath "Evoogle.Core.$packageVersion.nupkg"
$xUnitPackagePath = Join-Path $feedPath "Evoogle.XUnit.$packageVersion.nupkg"

New-Item -ItemType Directory -Path $feedPath -Force | Out-Null

if ((Test-Path -LiteralPath $corePackagePath) -or (Test-Path -LiteralPath $xUnitPackagePath))
{
    $message = "A package for version '$packageVersion' already exists. " + `
        'Run the script again to create a new version.'
    throw $message
}

Invoke-DotNet -dotnetArguments @('test', $solutionPath, '--configuration', 'Release')
Invoke-DotNet -dotnetArguments @(
    'pack',
    $coreProjectPath,
    '--configuration',
    'Release',
    '--no-restore',
    "--property:PackageVersion=$packageVersion",
    '--output',
    $feedPath)
Add-Type -AssemblyName System.IO.Compression.FileSystem
Test-PackageContents -packagePath $corePackagePath -packageId 'Evoogle.Core'
Invoke-DotNet -dotnetArguments @(
    'pack',
    $xUnitProjectPath,
    '--configuration',
    'Release',
    '--no-restore',
    "--property:PackageVersion=$packageVersion",
    '--output',
    $feedPath)
Test-PackageContents -packagePath $xUnitPackagePath -packageId 'Evoogle.XUnit'
Update-ApiFrameworkPackageVersions -coreRepositoryRoot $repositoryRoot -packageVersion $packageVersion

Write-Host "Created Evoogle.Core and Evoogle.XUnit. Version: $packageVersion"
Write-Host "Local NuGet feed: $feedPath"
