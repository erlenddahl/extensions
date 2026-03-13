# .NET 8.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET 8.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 8.0 upgrade.
3. Upgrade ByteUtilities\ByteUtilities.csproj
4. Upgrade SqliteExtensions\SqliteExtensions.csproj
5. Upgrade NpgsqlExtensions\NpgsqlExtensions.csproj
6. Upgrade ConsoleUtilities\ConsoleUtilities.csproj
7. Upgrade DataflowUtilities\DataflowUtilities.csproj
8. Upgrade ConsoleTester\ConsoleTester.csproj
9. Upgrade ByteUtilities.Tests\ByteUtilities.Tests.csproj
10. Upgrade ConsoleUtilities.Tests\ConsoleUtilities.Tests.csproj
11. Upgrade NpgsqlExtensions.Tests\NpgsqlExtensions.Tests.csproj
12. Run unit tests to validate upgrade in the projects listed below:
    - Extensions.Tests\Extensions.Tests.csproj
    - ByteUtilities.Tests\ByteUtilities.Tests.csproj
    - ConsoleUtilities.Tests\ConsoleUtilities.Tests.csproj
    - NpgsqlExtensions.Tests\NpgsqlExtensions.Tests.csproj

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

Table below contains projects that do belong to the dependency graph for selected projects and should not be included in the upgrade.

| Project name                                   | Description                                                    |
|:-----------------------------------------------|:---------------------------------------------------------------|
| Extensions\Extensions.csproj                   | Already targets net8.0;netstandard2.0                          |
| LeafletHelpers\LeafletHelpers.csproj           | Already targets net8.0;netstandard2.0                          |
| Extensions.Tests\Extensions.Tests.csproj       | Already targets net8.0                                         |

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name                                   | Current Version | New Version | Description                                   |
|:-----------------------------------------------|:---------------:|:-----------:|:----------------------------------------------|
| EntityFramework                                |   6.4.4         |             | Remove - not compatible with .NET 8           |
| Microsoft.Bcl.AsyncInterfaces                  |   7.0.0         |             | Remove - included in .NET 8                   |
| Microsoft.Bcl.HashCode                         |   1.1.1         |             | Remove - included in .NET 8                   |
| Microsoft.Extensions.Logging.Abstractions      |   7.0.0         |  8.0.0      | Update for .NET 8                             |
| Microsoft.NET.Test.Sdk                         |   17.5.0        |  17.9.0     | Update for .NET 8                             |
| MSTest.TestAdapter                             |   3.0.2         |  3.2.0      | Update for .NET 8                             |
| MSTest.TestFramework                           |   3.0.2         |  3.2.0      | Update for .NET 8                             |
| Npgsql                                         |   7.0.2         |  8.0.2      | Update for .NET 8                             |
| Stub.System.Data.SQLite.Core.NetFramework      |   1.0.117.0     |             | Remove - replace with Microsoft.Data.Sqlite   |
| System.Data.SQLite.EF6                         |   1.0.117.0     |             | Remove - not compatible with .NET 8           |
| System.Data.SQLite.Linq                        |   1.0.117.0     |             | Remove - not compatible with .NET 8           |
| Microsoft.Data.Sqlite                          |                 |  8.0.2      | New - replacement for SQLite on .NET 8        |
| coverlet.collector                             |   3.2.0         |  6.0.1      | Update for .NET 8                             |

### Project upgrade details
This section contains details about each project upgrade and modifications that need to be done in the project.

#### ByteUtilities\ByteUtilities.csproj modifications

Project properties changes:
  - Convert from non-SDK style to SDK-style project
  - Target frameworks should be changed from `net472` to `net8.0;netstandard2.0`

NuGet packages changes:
  - No changes discovered

Other changes:
  - Remove AssemblyInfo.cs (attributes will be auto-generated)

#### SqliteExtensions\SqliteExtensions.csproj modifications

Project properties changes:
  - Convert from non-SDK style to SDK-style project
  - Target frameworks should be changed from `net472` to `net8.0;netstandard2.0`

NuGet packages changes:
  - Remove EntityFramework 6.4.4 (not compatible with .NET 8)
  - Remove Stub.System.Data.SQLite.Core.NetFramework 1.0.117.0
  - Remove System.Data.SQLite.EF6 1.0.117.0
  - Remove System.Data.SQLite.Linq 1.0.117.0
  - Add Microsoft.Data.Sqlite 8.0.2

Other changes:
  - Remove AssemblyInfo.cs (attributes will be auto-generated)
  - Code changes may be required to migrate from Entity Framework 6 to EF Core or Microsoft.Data.Sqlite

#### NpgsqlExtensions\NpgsqlExtensions.csproj modifications

Project properties changes:
  - Convert from non-SDK style to SDK-style project
  - Target frameworks should be changed from `net472` to `net8.0;netstandard2.0`

NuGet packages changes:
  - Update Npgsql from 7.0.2 to 8.0.2
  - Remove Microsoft.Bcl.AsyncInterfaces 7.0.0 (included in .NET 8)
  - Remove Microsoft.Bcl.HashCode 1.1.1 (included in .NET 8)
  - Update Microsoft.Extensions.Logging.Abstractions from 7.0.0 to 8.0.0

Other changes:
  - Remove AssemblyInfo.cs (attributes will be auto-generated)
  - Remove packages.config

#### ConsoleUtilities\ConsoleUtilities.csproj modifications

Project properties changes:
  - Target frameworks should be changed from `net8.0` to `net8.0;netstandard2.0`

NuGet packages changes:
  - No changes discovered

Other changes:
  - None

#### DataflowUtilities\DataflowUtilities.csproj modifications

Project properties changes:
  - Target frameworks should be changed from `net8.0` to `net8.0;netstandard2.0`

NuGet packages changes:
  - No changes discovered

Other changes:
  - None

#### ConsoleTester\ConsoleTester.csproj modifications

Project properties changes:
  - Convert from non-SDK style to SDK-style project
  - Target framework should be changed from `net472` to `net8.0` (executable project)

NuGet packages changes:
  - No changes discovered

Other changes:
  - Remove AssemblyInfo.cs (attributes will be auto-generated)

#### ByteUtilities.Tests\ByteUtilities.Tests.csproj modifications

Project properties changes:
  - Convert from non-SDK style to SDK-style project
  - Target framework should be changed from `net472` to `net8.0` (test project)

NuGet packages changes:
  - Add Microsoft.NET.Test.Sdk 17.9.0
  - Add MSTest.TestAdapter 3.2.0
  - Add MSTest.TestFramework 3.2.0
  - Add coverlet.collector 6.0.1

Other changes:
  - Remove AssemblyInfo.cs (attributes will be auto-generated)
  - Migrate from Microsoft.VisualStudio.QualityTools.UnitTestFramework to MSTest

#### ConsoleUtilities.Tests\ConsoleUtilities.Tests.csproj modifications

Project properties changes:
  - Convert from non-SDK style to SDK-style project
  - Target framework should be changed from `net472` to `net8.0` (test project)

NuGet packages changes:
  - Add Microsoft.NET.Test.Sdk 17.9.0
  - Add MSTest.TestAdapter 3.2.0
  - Add MSTest.TestFramework 3.2.0
  - Add coverlet.collector 6.0.1

Other changes:
  - Remove AssemblyInfo.cs (attributes will be auto-generated)
  - Migrate from Microsoft.VisualStudio.QualityTools.UnitTestFramework to MSTest

#### NpgsqlExtensions.Tests\NpgsqlExtensions.Tests.csproj modifications

Project properties changes:
  - Convert from non-SDK style to SDK-style project
  - Target framework should be changed from `net472` to `net8.0` (test project)

NuGet packages changes:
  - Add Microsoft.NET.Test.Sdk 17.9.0
  - Update MSTest.TestAdapter from 3.0.2 to 3.2.0
  - Update MSTest.TestFramework from 3.0.2 to 3.2.0
  - Add coverlet.collector 6.0.1

Other changes:
  - Remove AssemblyInfo.cs (attributes will be auto-generated)
  - Remove packages.config
