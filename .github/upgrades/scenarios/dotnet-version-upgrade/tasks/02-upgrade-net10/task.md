# 02-upgrade-net10: Upgrade BarcodeLib to .NET 10

Move `BarcodeLib.csproj` from net48 to net10.0 and resolve the source incompatibilities flagged by the assessment, especially the extensive `System.Drawing`-based code paths. This task covers the TFM change and the code cleanup needed to compile cleanly under .NET 10, then validates the upgraded project with a successful build.

**Done when**: `BarcodeLib.csproj` targets net10.0 and the project builds successfully with no compilation errors.

## Research Notes

### Current compatibility findings
- The converted project is still using the legacy shared version import from `..\..\SasaLib.AssemblyVersion\SasaLib.AssemblyVersion.projitems`, so version metadata remains centrally managed.
- Other projects in this workspace already use `System.Drawing.Common` 8.0.10 on modern .NET (`SasaLibDNet8.Barcode` and `SasaLibDNet8`), which is a likely reference point if the BarcodeLib code needs an explicit drawing package after retargeting.
- The project also relies on `System.Data.DataSetExtensions`, so the post-retarget build should confirm that the SDK and target framework still resolve this reference cleanly.

### Files likely to be touched
- `Barcode_src_1.0.0.21/BarcodeLib/BarcodeLib.csproj` — retarget to `net10.0` and add any framework/package references needed for the new TFM.

### Validation plan
- Build the project after the TFM change.
- If the compiler reports missing drawing or dataset-extension types, add the minimal package/reference adjustments required and rebuild.
