# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

XmiSchema is a C# library for constructing and serializing Cross Model Information (XMI) graphs representing structural analytical and physical building elements. The library uses a graph-based architecture where entities (nodes) are connected via relationships (edges), all serializable to JSON using Newtonsoft.Json.

## Build and Test Commands

### Solution Structure
- **Main library**: `XmiSchema.csproj` (targets `net8.0`)
- **Unit tests**: `tests/Unit/XmiSchema.Core.Tests/XmiSchema.Tests.csproj` (targets `net8.0`)
  - Uses xUnit 2.9.2 with Microsoft.NET.Test.Sdk 17.12.0
- **Solution file**: `XmiSchema.sln` (includes both library and tests)

### Local Development Workflow

```bash
# Restore dependencies
dotnet restore XmiSchema.sln

# Build library and tests
dotnet build XmiSchema.sln --configuration Debug

# Run all unit tests (builds automatically if needed)
dotnet test XmiSchema.sln

# Run tests without rebuilding
dotnet test XmiSchema.sln --no-build

# Run specific test by filter
dotnet test --filter "FullyQualifiedName~XmiBeamTests"

# Run tests with verbose output
dotnet test XmiSchema.sln --verbosity detailed
```

### Running Examples

```bash
dotnet run --project tests/Examples/StructuralGraphSample/StructuralGraphSample.csproj
```

### Packaging

```bash
# Create NuGet package for Release
dotnet pack XmiSchema.csproj --configuration Release --output ./packages
```

## Architecture

### Namespace Structure

The library is organized into the following namespaces:

- **`XmiSchema.Entities.Bases`** - Base classes for all entities, geometries, and relationships
- **`XmiSchema.Entities.Commons`** - Common domain entities (materials, cross-sections, storeys, units, segments)
- **`XmiSchema.Entities.Physical`** - Physical building elements (beams, columns, slabs, walls)
- **`XmiSchema.Entities.StructuralAnalytical`** - Analytical structural elements (curve members, surface members, point connections)
- **`XmiSchema.Entities.Geometries`** - Geometric primitives (points, lines, arcs)
- **`XmiSchema.Entities.Relationships`** - Graph edges connecting entities
- **`XmiSchema.Enums`** - Domain enumerations with serialization attributes
- **`XmiSchema.Parameters`** - Shape parameter definitions
- **`XmiSchema.Managers`** - XmiModel and XmiManager orchestration classes
- **`XmiSchema.Utils`** - Helper utilities (e.g., ExtensionEnumHelper)

### Core Types

**Base Classes** (`XmiSchema.Entities.Bases`):
- `XmiBaseEntity`: Root for all entities. Provides `ID`, `ifcGuid`, `NativeId`, `Description`, and `EntityType` (polymorphic discriminator for JSON serialization).
- `XmiBaseGeometry`: Extends `XmiBaseEntity` for geometric primitives (points, lines, arcs).
- `XmiBaseRelationship`: Base for all graph edges, holding source/target entity references and UML stereotypes.
- `XmiBasePhysicalEntity`: Intermediate base for physical elements (beams, columns, slabs, walls).
- `XmiBaseStructuralAnalyticalEntity`: Intermediate base for analytical elements (curve members, surface members, point connections).
- `XmiBaseEnum`: Provides `EnumValueAttribute` for enum serialization.

**Common Entities** (`XmiSchema.Entities.Commons`):
- `XmiMaterial`: Material properties with grade, density, and stiffness constants.
- `XmiCrossSection`: Geometric and analytical profile properties (area, torsion, section moduli).
- `XmiStorey`: Level representation with elevation, mass, and reaction info.
- `XmiUnit`: Maps entity attributes to unit types for conversion.
- `XmiSegment`: Sub-span representation along curve members.

**Physical Elements** (`XmiSchema.Entities.Physical`):
- `XmiBeam`, `XmiColumn`, `XmiSlab`, `XmiWall`: Physical building components.

**Analytical Elements** (`XmiSchema.Entities.StructuralAnalytical`):
- `XmiStructuralCurveMember`: Linear analytical members (beams, columns, bracing).
- `XmiStructuralSurfaceMember`: Surface-based analytical elements (slabs, walls, plates).
- `XmiStructuralPointConnection`: Analytical nodes connecting members.

**Geometries** (`XmiSchema.Entities.Geometries`):
- `XmiPoint3D`: 3D point with tolerance-aware equality. Create via `XmiModel.CreatePoint3D()` for automatic deduplication.
- `XmiLine3D`: Straight line between two points.
- `XmiArc3D`: Circular arc with start, end, center, and radius.

**Relationships** (`XmiSchema.Entities.Relationships`):
- `XmiHasGeometry`, `XmiHasLine3D`, `XmiHasPoint3D`: Link entities to geometries.
- `XmiHasMaterial`, `XmiHasCrossSection`: Associate materials and cross-sections.
- `XmiHasSegment`, `XmiHasStorey`: Connect segments and storeys.
- `XmiHasStructuralCurveMember`, `XmiHasStructuralPointConnection`: Relate analytical elements.

**Enumerations** (`XmiSchema.Enums`):
All enums use `EnumValueAttribute` to map to serialized string values:
- `XmiSegmentTypeEnum`, `XmiShapeEnum`, `XmiStructuralCurveMemberTypeEnum`
- `XmiSystemLineEnum`, `XmiMaterialTypeEnum`, `XmiStructuralSurfaceMemberTypeEnum`
- `XmiStructuralSurfaceMemberSpanTypeEnum`, `XmiStructuralSurfaceMemberSystemPlaneEnum`
- `XmiUnitEnum`, `XmiBaseEntityDomainEnum`

**Parameters** (`XmiSchema.Parameters`):
- `IXmiShapeParameters`: Interface for shape parameter implementations.
- Shape parameter classes (e.g., `RectangularShapeParameters`, `IShapeParameters`) that serialize to dictionaries.
- See `Enums/XmiShapeEnumParameters.cs` for parameter key definitions.

### XmiModel and XmiManager

**XmiModel** (`XmiSchema.Managers.XmiModel`):
- Holds `List<XmiBaseEntity> Entities` and `List<XmiBaseRelationship> Relationships`
- Provides `Add*` methods (e.g., `AddXmiMaterial`, `AddXmiStructuralCurveMember`)
- Provides factory methods like `CreatePoint3D` that enforce deduplication
- Provides query methods like `GetXmiEntitiesOfType<T>()` and `FindMatchingXmiStructuralPointConnectionByPoint3D`

**XmiManager** (`XmiSchema.Managers.XmiManager`):
- Manages a collection of `XmiModel` instances (`List<XmiModel> Models`)
- Provides higher-level orchestration methods (e.g., `AddXmiMaterialToModel(int modelIndex, XmiMaterial material)`)
- Validates model indices before delegating to underlying `XmiModel` instances

## Coding Conventions

### Constructor Signatures
Maintain consistent parameter order across all new entities:
```
[id, name, ifcGuid, nativeId, description, ...domain-specific-args]
```

### EntityType Discriminator
All entity constructors must pass the entity type name to the base constructor:
```csharp
public XmiBeam(...) : base("XmiBeam", id, name, ifcGuid, nativeId, description, domain)
```
This ensures JSON serialization emits the correct `"EntityType"` discriminator for polymorphic deserialization.

### Relationship Constructors
Provide two constructors:
1. Fully described: `(string id, XmiBaseEntity source, XmiBaseEntity target, ...)`
2. Shorthand with auto-generated ID: `(XmiBaseEntity source, XmiBaseEntity target, ...)`

### Enum Usage
- Decorate all enum members with `[EnumValue("SerializedString")]`
- Use `ExtensionEnumHelper.FromEnumValue<T>(string value)` for parsing
- Test round-trip conversions in `Utils/ExtensionEnumHelperTests.cs`

### Test Organization
- Tests are located in `tests/Unit/XmiSchema.Core.Tests/`
- Use `TestModelFactory` in `Support/` for consistent test fixtures
- Add XML doc comments to test classes describing their intent
- Test namespace pattern: `XmiSchema.Tests.<category>` (e.g., `XmiSchema.Tests.Models.Entities`)

## File Organization

```
Entities/
  Bases/              - XmiBaseEntity, XmiBaseEnum, XmiBaseGeometry, XmiBaseRelationship,
                        XmiBasePhysicalEntity, XmiBaseStructuralAnalyticalEntity
  Commons/            - XmiMaterial, XmiCrossSection, XmiStorey, XmiUnit, XmiSegment
  Physical/           - XmiBeam, XmiColumn, XmiSlab, XmiWall
  StructuralAnalytical/ - XmiStructuralCurveMember, XmiStructuralSurfaceMember, XmiStructuralPointConnection
  Geometries/         - XmiPoint3D, XmiLine3D, XmiArc3D
  Relationships/      - XmiHas* relationship classes

Enums/                - All domain enums with EnumValueAttribute

Parameters/           - IXmiShapeParameters, XmiShapeParametersBase, ShapeParameterSets

Managers/             - XmiModel, XmiManager, IXmiManager

Utils/                - ExtensionEnumHelper for enum parsing

Docs/                 - Documentation files
  api/
  examples/
  reference/

tests/
  Unit/XmiSchema.Core.Tests/ - xUnit tests (266 tests)
    Models/           - Entity and relationship tests
    Parameters/       - Shape parameter tests
    Serialization/    - JSON serialization tests
    Support/          - TestModelFactory helper
    Utils/            - Utility tests

  Examples/StructuralGraphSample/ - Runnable sample demonstrating API usage
```

## CI/CD

### Pull Request Tests
- Workflow: `.github/workflows/pull-request-tests.yml`
- Triggers on all PRs
- Runs: restore → build → test with .NET 8.0.416

### Release Pipeline
- Workflow: `.github/workflows/release.yml`
- Triggers on push to `main`
- Steps:
  1. Generate semantic version tag using commit messages
  2. Update `<Version>` in all `.csproj` files and commit
  3. Build and test
  4. Pack and publish to NuGet
  5. Create GitHub release with enhanced changelog

**Note**: The release workflow references legacy file names (`xmi-schema-Csharp.Core.sln` and `xmi-schema-Csharp.Core.csproj`) which may need updating to match the current naming (`XmiSchema.sln` and `XmiSchema.csproj`).

## Important Notes

- **JSON Serialization**: Uses Newtonsoft.Json. Polymorphic entities rely on the `EntityType` discriminator.
- **Point Deduplication**: Always create points via `XmiModel.CreatePoint3D()` to avoid duplicate coordinates in the graph.
- **Shape Parameters**: Reference `Enums/XmiShapeEnumParameters.cs` for the canonical list of parameter keys per shape type.
- **Target Framework**: `net8.0` for both library and tests.
- **Test Integration**: Tests are included in `XmiSchema.sln` and run automatically with `dotnet test XmiSchema.sln`.
- **266 Unit Tests**: All tests must pass before committing changes.
