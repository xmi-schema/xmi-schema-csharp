# XmiSchema Library

[![Build](https://github.com/xmi-schema/xmi-schema-csharp/actions/workflows/build.yml/badge.svg)](https://github.com/ximlib-foundation/XmiSchema.Core/actions)
[![NuGet Version](https://img.shields.io/nuget/v/XmiSchema.Core.svg)](https://www.nuget.org/packages/XmiSchema.Core/)
[![License](https://img.shields.io/github/license/ximlib-foundation/XmiSchema.Core.svg)](LICENSE)
[![Downloads](https://img.shields.io/nuget/dt/XmiSchema.Core.svg)](https://www.nuget.org/packages/XmiSchema.Core/)

## Overview
XmiSchema.Core is a .NET 8 class library that models Cross Model Information (XMI) graphs for built-environment data. The package exposes strongly-typed entities (materials, storeys, geometries, relationships) plus the `XmiManager` helper so downstream projects or NuGet consumers can build, query, and export graph payloads consistently.

## Installation
```bash
dotnet add package XmiSchema.Core
```
The package targets .NET 8.0 and ships as a reusable dependency for desktop, web, or service workloads.

## Quick Start
```csharp
using System.Collections.Generic;
using XmiSchema.Core.Enums;
using XmiSchema.Core.Entities;
using XmiSchema.Core.Manager;
using XmiSchema.Core.Models;
using XmiSchema.Core.Parameters;

var manager = new XmiManager();
manager.Models.Add(new XmiModel());

var storey = manager.CreateStorey(0, "Storey-1", "Level 1", "ifc-guid", "LEVEL_1", "Ground floor", 0, 900, "Fx", "Fy", "Fz");
var point = manager.CreatePoint3D(0, "pt-1", "Grid A/1", "pt-guid", "PT_A1", "Column base", 0, 0, 0);
var connection = manager.CreateStructuralPointConnection(0, "pc-1", "Node", "pc-guid", "PC_A1", "Column node", storey, point);

var material = manager.CreateMaterial(0, "mat-1", "Steel", "mat-guid", "MAT_S355", "Sample material", XmiMaterialTypeEnum.Steel, 50, 78.5, "210000", "81000", "0.3", 1.2);
var section = manager.CreateCrossSection(
    0,
    "sec-1",
    "IPE300",
    "sec-guid",
    "SEC_IPE300",
    "Beam section",
    material,
    XmiShapeEnum.IShape,
    new IShapeParameters(0.3, 0.15, 0.02, 0.012, 0.008),
    0.0009,
    0.00012,
    0.00035,
    0.045,
    0.09,
    0.0003,
    0.0004,
    0.0005,
    0.0006,
    0.00007);

var curveMember = manager.CreateStructuralCurveMember(0, "cur-1", "Beam A", "cur-guid", "BEAM_A", "Sample beam", section, storey, XmiStructuralCurveMemberTypeEnum.Beam,
    new List<XmiStructuralPointConnection> { connection, connection }, new List<XmiSegment> { new("seg-1", "Segment", "seg-guid", "SEG_1", "Segment", 0f, XmiSegmentTypeEnum.Line) },
    XmiSystemLineEnum.MiddleMiddle, connection, connection, 5, "1,0,0", "0,1,0", "0,0,1", 0, 0, 0, 0, 0, 0, "Fixed", "Pinned");

var json = manager.BuildJson(0);
Console.WriteLine(json);
```
For a full runnable sample see `tests/Examples/StructuralGraphSample`.

## Folder Layout
```
xmi-schema-csharp/
├── IXmiManager.cs / XmiManager.cs      # Public orchestration API
├── Models/                             # Bases, entities, geometries, relationships, and XmiModel helpers
├── Utils/                              # Shared utilities (enum converters, extensions)
├── tests/
│   ├── Unit/XmiSchema.Core.Tests       # xUnit project with one fixture per production class
│   └── Examples/StructuralGraphSample  # Console sample showing library usage
├── AGENTS.md / PLAN.md / README.md     # Contributor docs
└── xmi-schema-Csharp.Core.csproj       # Library project file
```
Each subdirectory within `Models/` includes its own README that documents constructor expectations and naming conventions.

## Testing & CI
- **Local workflow:**
  ```bash
  dotnet restore xmi-schema-Csharp.Core.sln
  dotnet test xmi-schema-Csharp.Core.sln --configuration Release
  ```
  Run these commands before every commit or pull request so the xUnit suite (one fixture per production class) stays green. Builders under `tests/Unit/XmiSchema.Core.Tests/Support` provide deterministic sample models for the tests.
- **Examples:** run `dotnet run --project tests/Examples/StructuralGraphSample/StructuralGraphSample.csproj` to see how `XmiManager` composes a graph; treat it as an integration sanity check when touching graph construction logic.
- **GitHub Actions:**
  - `Pull Request Tests` restores, builds, and tests the solution on every pull request targeting any branch.
  - `Release Build & Publish` runs after pushes to `main`, repeats restore/build/test, and then packs/pushes the NuGet package (using the README captured in the `.csproj`).
  Both workflows rely on the same commands shown above, so matching them locally avoids surprises in CI.

## Contributing
1. `dotnet restore`
2. Update or add XML documentation whenever a public API changes.
3. Add or update tests in `tests/Unit/XmiSchema.Core.Tests` and ensure `dotnet test` passes.
4. Document new behavior in the relevant README (folder-level or root).

See `AGENTS.md` for detailed contributor guidelines and the long-term plan in `PLAN.md`.

## License
Distributed under the terms of the project [LICENSE](LICENSE).
