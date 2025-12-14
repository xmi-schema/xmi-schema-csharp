# XMI Schema - Cross Model Information

[![NuGet](https://img.shields.io/nuget/v/XmiSchema.svg)](https://www.nuget.org/packages/XmiSchema)
[![Build Status](https://github.com/xmi-schema/xmi-schema-csharp/actions/workflows/pull-request-tests.yml/badge.svg)](https://github.com/xmi-schema/xmi-schema-csharp/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

**XMI (Cross Model Information)** is an open-source file format schema designed to represent built environment data. This C# library provides a complete implementation for constructing, validating, and serializing XMI graphs representing structural analytical and physical building elements.

## What is Cross Model Information?

Cross Model Information (XMI) is a graph-based data schema specifically designed for the Architecture, Engineering, and Construction (AEC) industry. It enables seamless exchange of structural and physical building data between different software platforms, analysis tools, and BIM (Building Information Modeling) systems.

### Key Characteristics

- **Graph-Based Architecture**: Entities (nodes) are connected via typed relationships (edges), enabling rich semantic connections between building elements
- **Domain-Specific**: Purpose-built for structural engineering and physical building representation
- **Interoperable**: Supports IFC GUID references for integration with Industry Foundation Classes (IFC) workflows
- **Lightweight**: JSON serialization for human-readable, easily parseable output
- **Extensible**: Modular entity and relationship system allows for domain-specific extensions

## Relationship to Industry Standards

### Industry Foundation Classes (IFC)

XMI is designed to complement and integrate with [IFC (Industry Foundation Classes)](https://www.buildingsmart.org/standards/bsi-standards/industry-foundation-classes/), the ISO-certified open standard for BIM data exchange:

| Aspect | IFC | XMI |
|--------|-----|-----|
| **Scope** | Comprehensive BIM (architecture, MEP, structural, etc.) | Focused on structural/analytical models |
| **Format** | STEP/EXPRESS, IFC-XML, IFC-JSON | JSON (graph-based) |
| **Complexity** | 900+ entity types | Streamlined entity set |
| **Use Case** | Full building lifecycle | Structural analysis exchange |
| **Interoperability** | Native `ifcGuid` field support | References IFC GUIDs |

XMI entities include an `ifcGuid` property, allowing direct mapping to IFC elements for round-trip workflows between BIM authoring tools and structural analysis software.

### Schema Design Principles

Following industry best practices for AEC data schemas:

1. **Unique Identification**: Every entity has a stable `id`, optional `ifcGuid` for IFC interoperability, and `nativeId` for source system traceability
2. **Polymorphic Serialization**: `EntityName` discriminator enables type-safe deserialization of heterogeneous entity collections
3. **Domain Classification**: Entities include a `Domain` field (Physical, StructuralAnalytical, Geometry, Functional, Shared) to make payload routing explicit
4. **Explicit Relationships**: Graph edges (relationships) are first-class citizens with their own identity and metadata
5. **Unit Awareness**: `XmiUnit` entities define measurement units for proper unit conversion across systems
6. **Tolerance-Aware Geometry**: Point deduplication with configurable geometric tolerance prevents duplicate coordinates

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package XmiSchema
```

Or via Package Manager Console:

```powershell
Install-Package XmiSchema
```

## Quick Start

```csharp
using XmiSchema.Managers;
using XmiSchema.Entities.Commons;
using XmiSchema.Entities.StructuralAnalytical;
using XmiSchema.Entities.Geometries;
using XmiSchema.Enums;

// Initialize manager and model
var manager = new XmiManager();
manager.Models.Add(new XmiModel());

// Create a storey
var storey = manager.CreateXmiStorey(
    modelIndex: 0,
    id: "storey-1",
    name: "Level 1",
    ifcGuid: "2O2Fr$t4X7Zf8NOew3FL9r",
    nativeId: "LEVEL_1",
    description: "Ground floor",
    storeyElevation: 0,
    storeyMass: 800);

// Create a material
var material = manager.CreateXmiMaterial(
    modelIndex: 0,
    id: "mat-1",
    name: "Concrete C40",
    ifcGuid: "3$HnSh4fn5$vQIE9d8M0L9",
    nativeId: "MAT_C40",
    description: "High-strength concrete",
    materialType: XmiMaterialTypeEnum.Concrete,
    grade: 40,
    unitWeight: 24,
    eModulus: "33000",
    gModulus: "13000",
    poissonRatio: "0.2",
    thermalCoefficient: 1.0);

// Create points with automatic deduplication
var point1 = manager.CreateXmiPoint3D(0.0, 0.0, 0.0, "pt-1");
var point2 = manager.CreateXmiPoint3D(0.0, 0.0, 3.0, "pt-2");

// Create structural nodes
var node1 = manager.CreateXmiStructuralPointConnection(
    modelIndex: 0, id: "node-1", name: "Node 1", 
    ifcGuid: "", nativeId: "", description: "", point: point1);

var node2 = manager.CreateXmiStructuralPointConnection(
    modelIndex: 0, id: "node-2", name: "Node 2", 
    ifcGuid: "", nativeId: "", description: "", point: point2);

// Create segments with positions
var segments = new List<XmiSegment>
{
    new XmiSegment("seg-1", "Segment 1", "", "seg-1", "", XmiSegmentTypeEnum.Line)
};
var positions = new List<int> { 0 };

// Create a structural curve member
var column = manager.CreateXmiStructuralCurveMember(
    modelIndex: 0,
    id: "col-1", name: "Column 1", ifcGuid: "", nativeId: "col-1", description: "",
    material: material, crossSection: null, storey: storey,
    curveMemberType: XmiStructuralCurveMemberTypeEnum.Column,
    nodes: new List<XmiStructuralPointConnection> { node1, node2 },
    segments: segments, positions: positions,
    systemLine: XmiSystemLineEnum.MiddleMiddle,
    beginNode: node1, endNode: node2, length: 3.0,
    localAxisX: new XmiAxis(1, 0, 0), localAxisY: new XmiAxis(0, 1, 0), localAxisZ: new XmiAxis(0, 0, 1),
    beginNodeXOffset: 0, endNodeXOffset: 0, beginNodeYOffset: 0, endNodeYOffset: 0,
    beginNodeZOffset: 0, endNodeZOffset: 0,
    endFixityStart: "Fixed", endFixityEnd: "Fixed");

// Serialize to JSON
var json = manager.BuildJson(0);
Console.WriteLine(json);
```

### Key API Changes

- **Segment Positioning**: Segment positions are now handled through `XmiHasSegment` relationships with a `positions` array
- **Automatic Deduplication**: Points, lines, and arcs with identical coordinates/geometries are automatically reused
- **Required Parameters**: Factory methods now require `positions` parameter when `segments` are provided
- **End Fixity**: Structural curve members require both `endFixityStart` and `endFixityEnd` parameters

## Entity Types

### Physical Elements

Physical building components that represent real-world objects:

| Entity | Description |
|--------|-------------|
| `XmiBeam` | Horizontal structural member |
| `XmiColumn` | Vertical structural member |
| `XmiSlab` | Horizontal planar element (floors, roofs) |
| `XmiWall` | Vertical planar element |

### Analytical Elements

Abstract representations for structural analysis:

| Entity | Description |
|--------|-------------|
| `XmiStructuralCurveMember` | 1D analytical member (beams, columns, braces) |
| `XmiStructuralSurfaceMember` | 2D analytical member (slabs, walls, plates) |
| `XmiStructuralPointConnection` | Analytical node connecting members |

### Common Entities

| Entity | Description |
|--------|-------------|
| `XmiMaterial` | Material properties (modulus, density, grade) |
| `XmiCrossSection` | Profile geometry and section properties |
| `XmiStorey` | Building level with elevation and mass |
| `XmiSegment` | Sub-span along curve members |
| `XmiUnit` | Unit definitions for attribute conversion |

### Geometry

| Entity | Description |
|--------|-------------|
| `XmiPoint3d` | 3D coordinate with tolerance-aware equality |
| `XmiLine3d` | Straight line between two points |
| `XmiArc3d` | Circular arc with center and radius |

## Relationships

XMI uses explicit relationship entities to connect nodes in the graph:

| Relationship | Description |
|--------------|-------------|
| `XmiHasMaterial` | Links entity to material |
| `XmiHasCrossSection` | Links entity to cross-section profile |
| `XmiHasStorey` | Associates entity with building level |
| `XmiHasPoint3d` | Links entity to geometric point |
| `XmiHasLine3d` | Links entity to line geometry |
| `XmiHasGeometry` | Generic geometry association |
| `XmiHasStructuralCurveMember` | Connects physical to analytical curve |
| `XmiHasStructuralPointConnection` | Links members to nodes |
| `XmiHasSegment` | Associates curve member with segments |

## Supported Cross-Section Shapes

XMI supports parametric cross-section definitions:

- Rectangular
- Circular
- I-Shape (Wide Flange)
- T-Shape
- L-Shape (Angle)
- C-Shape (Channel)
- Hollow Rectangular (Box)
- Hollow Circular (Pipe)

## JSON Output Format

XMI serializes to a clean JSON structure:

```json
{
  "Entities": [
    {
      "id": "mat-1",
      "Name": "Concrete C40",
      "ifcGuid": "3$HnSh4fn5$vQIE9d8M0L9",
      "NativeId": "MAT_C40",
      "Description": "High-strength concrete",
      "EntityName": "XmiMaterial",
      "Domain": "Shared",
      "MaterialType": "Concrete",
      "Grade": 40,
      "UnitWeight": 24
    }
  ],
  "Relationships": [
    {
      "id": "rel-1",
      "EntityName": "XmiHasMaterial",
      "SourceId": "col-1",
      "TargetId": "mat-1"
    }
  ]
}
```

## Building from Source

### Prerequisites

- .NET 8.0 SDK

### Build and Test

```bash
# Clone the repository
git clone https://github.com/xmi-schema/xmi-schema-csharp.git
cd xmi-schema-csharp

# Restore dependencies
dotnet restore XmiSchema.sln

# Build
dotnet build XmiSchema.sln

# Run tests (266 unit tests)
dotnet test XmiSchema.sln

# Run specific test
dotnet test --filter "FullyQualifiedName~XmiBeamTests"
```

### Run the Example

```bash
dotnet run --project tests/Examples/StructuralGraphSample/StructuralGraphSample.csproj
```

## Resources

- **Website**: [https://xmi-schema.com](https://xmi-schema.com/)
- **NuGet Package**: [XmiSchema on NuGet](https://www.nuget.org/packages/XmiSchema)
- **IFC Standard**: [buildingSMART IFC](https://www.buildingsmart.org/standards/bsi-standards/industry-foundation-classes/)

## Contributing

Contributions are welcome! Please feel free to submit issues and pull requests.

## License

This project is open source. See the repository for license details.
