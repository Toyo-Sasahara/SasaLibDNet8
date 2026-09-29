# .NET Version Upgrade

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net10.0

## Build Tool Decisions
- **Barcode_src_1.0.0.21/BarcodeLib/BarcodeLib.csproj**: dotnet build (SDK-style project targeting net10.0; validated successfully with the .NET 10 SDK)

## Strategy
**Selected**: All-At-Once
**Rationale**: Single .NET Framework project with no project-to-project dependencies; the upgrade can be completed in one coordinated pass while keeping SDK-style conversion separate from the target framework change.

### Execution Constraints
- Convert the classic project to SDK-style before changing the target framework.
- Keep SDK-style conversion and .NET 10 target framework upgrade as separate concerns during execution.
- Treat System.Drawing-heavy source incompatibilities as the main code-fix area during the upgrade task.
- Validate the project with a clean build after each major change and again at the end.

## Source Control
- **Source Branch**: master
- **Working Branch**: upgrade-dotnet-10
- **Commit Strategy**: After Each Task
- **Branch Sync**: Auto (Merge)
