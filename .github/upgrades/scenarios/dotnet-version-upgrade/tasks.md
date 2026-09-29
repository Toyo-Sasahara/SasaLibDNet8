# .NET Version Upgrade Progress

## Overview

Upgrade `BarcodeLib.csproj` from .NET Framework 4.8 to .NET 10, starting by converting the project file to SDK-style and then updating the target framework and source compatibility issues. The project is a single classic class library with heavy `System.Drawing` usage, so the upgrade will be validated with a clean build after each major change.
**Progress**: 2/2 tasks complete <progress value="100" max="100"></progress> 100%
**Progress**: 0/2 tasks complete <progress value="0" max="100"></progress> 0%

## Tasks
- ✅ 01-sdk-style-conversion: Convert BarcodeLib to SDK-style on net48 ([Content](tasks/01-sdk-style-conversion/task.md), [Progress](tasks/01-sdk-style-conversion/progress-details.md))
- 🔲 01-sdk-style-conversion: Convert BarcodeLib to SDK-style on net48
- ✅ 02-upgrade-net10: Upgrade BarcodeLib to .NET 10 ([Content](tasks/02-upgrade-net10/task.md), [Progress](tasks/02-upgrade-net10/progress-details.md))
