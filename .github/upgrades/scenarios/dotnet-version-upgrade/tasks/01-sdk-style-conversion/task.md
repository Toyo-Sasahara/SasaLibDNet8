# 01-sdk-style-conversion: Convert BarcodeLib to SDK-style on net48

Convert `BarcodeLib.csproj` from the classic project format to SDK-style while keeping the project on .NET Framework 4.8. Preserve the library's existing build behavior, references, and metadata so the project remains stable before the target framework change. This task establishes the modern project structure needed for the .NET 10 upgrade and should surface any project-file issues before the framework jump.

**Done when**: `BarcodeLib.csproj` is SDK-style, still targets net48, and the project restores/builds successfully in its converted form.

## Research Notes

### Current project shape
- Classic .NET Framework 4.8 class library (`<TargetFrameworkVersion>v4.8</TargetFrameworkVersion>`) with legacy `Microsoft.CSharp.targets` import.
- No `packages.config` file is present, so package migration is not part of this conversion.
- The project includes explicit framework references to `System`, `System.Data`, `System.Data.DataSetExtensions`, `System.Drawing`, and `System.Xml`.

### Important files to preserve or review
- `BarcodeLib.csproj` — legacy project structure, bootstrapper settings, compiler/linker settings, and the project item list.
- `Properties/AssemblyInfo.cs` — assembly metadata plus `AssemblyInformationalVersion("BarcodeLib 1.22.08.xx [.NET Framework 4.8]")`.
- `..\..\SasaLib.AssemblyVersion\SasaLib.AssemblyVersion.projitems` — shared versioning import used by the project.
- `BarcodeXML.xsd` / `BarcodeXML.Designer.cs` — dataset-style generated files that should continue to build correctly after conversion.
- `BarcodeLib.nuspec`, `nugetpack.cmd`, `Release Notes.txt`, `LocalVersion.txt` — packaging/versioning artifacts that may need to remain referenced correctly after the project-format change.

### Notes from assessment / project inspection
- Assessment shows the project has 378 source incompatibilities when moving to .NET 10, with `System.Drawing` accounting for almost all of them; those issues belong to the next task, not this conversion task.
- The project contains 29 source/content files and no project references, so the conversion is structurally simple but must preserve all existing items and build metadata.
- Because the project is old-style, SDK conversion must be validated with a direct project build before the target framework upgrade starts.
