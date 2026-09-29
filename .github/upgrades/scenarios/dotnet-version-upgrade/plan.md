# .NET Version Upgrade Plan

## Overview

**Target**: Upgrade `BarcodeLib.csproj` from .NET Framework 4.8 to .NET 10 while converting the project to SDK-style.
**Scope**: Single classic class library, about 5.7k LOC, no project-to-project dependencies, and heavy `System.Drawing` usage with a large source-compatibility surface.

### Selected Strategy

**All-At-Once** — single-project upgrade completed in one coordinated pass.
**Rationale**: This is a single .NET Framework project with no dependency chain, so the project format conversion and target framework change can be sequenced safely without cross-project coordination.

## Tasks

### 01-sdk-style-conversion: Convert BarcodeLib to SDK-style on net48

Convert `BarcodeLib.csproj` from the classic project format to SDK-style while keeping the project on .NET Framework 4.8. Preserve the library's existing build behavior, references, and metadata so the project remains stable before the target framework change. This task establishes the modern project structure needed for the .NET 10 upgrade and should surface any project-file issues before the framework jump.

**Done when**: `BarcodeLib.csproj` is SDK-style, still targets net48, and the project restores/builds successfully in its converted form.

---

### 02-upgrade-net10: Upgrade BarcodeLib to .NET 10

Move `BarcodeLib.csproj` from net48 to net10.0 and resolve the source incompatibilities flagged by the assessment, especially the extensive `System.Drawing`-based code paths. This task covers the TFM change and the code cleanup needed to compile cleanly under .NET 10, then validates the upgraded project with a successful build.

**Done when**: `BarcodeLib.csproj` targets net10.0 and the project builds successfully with no compilation errors.
