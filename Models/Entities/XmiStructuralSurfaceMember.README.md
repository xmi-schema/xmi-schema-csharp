# XmiStructuralSurfaceMember

Represents 2D structural elements such as slabs, walls, and shells in a structural model.

## Overview

`XmiStructuralSurfaceMember` models planar or curved structural surface elements that carry loads through membrane and bending action. It includes geometric, material, and connectivity information for walls, slabs, shells, and other surface elements.

## Location

`Models/Entities/XmiStructuralSurfaceMember.cs`

## Inheritance

```
XmiBaseEntity → XmiStructuralSurfaceMember
```

## Properties

### Material Properties

- **Material** (`XmiStructuralMaterial`): Material assigned to this surface member
  - Direct material reference (unlike curve members which use cross-section)

### Type and Classification

- **SurfaceMemberType** (`XmiStructuralSurfaceMemberTypeEnum`): Type of surface element
  - Slab, Wall, Shell, etc.

- **SystemPlane** (`XmiStructuralSurfaceMemberSystemPlaneEnum`): Reference plane position
  - Top, Center, Bottom of surface thickness

### Geometric Properties

- **Thickness** (`double`): Thickness of the surface element (in meters or inches)

- **Area** (`double`): Total surface area

- **Height** (`double`): Height dimension (particularly for walls)

- **ZOffset** (`double`): Vertical offset from reference plane

### Local Coordinate System

- **LocalAxisX** (`string`): Local X-axis orientation (in-plane)
- **LocalAxisY** (`string`): Local Y-axis orientation (in-plane)
- **LocalAxisZ** (`string`): Local Z-axis orientation (normal to surface)

### Connectivity

- **Nodes** (`List<XmiStructuralPointConnection>`): Boundary nodes defining the surface perimeter
  - Ordered list forming closed polygon
  - Typically 3+ nodes

- **Segments** (`List<XmiSegment>`): Edges of the surface element
  - Connect consecutive nodes
  - Define surface boundary

### Hierarchical Organization

- **Storey** (`XmiStructuralStorey`): Building level this surface belongs to

## Constructor

```csharp
public XmiStructuralSurfaceMember(
    string id,
    string name,
    string ifcguid,
    string nativeId,
    string description,
    XmiStructuralMaterial material,
    XmiStructuralSurfaceMemberTypeEnum surfaceMemberType,
    double thickness,
    XmiStructuralSurfaceMemberSystemPlaneEnum systemPlane,
    List<XmiStructuralPointConnection> nodes,
    XmiStructuralStorey storey,
    List<XmiSegment> segments,
    double area,
    double zOffset,
    string localAxisX,
    string localAxisY,
    string localAxisZ,
    double height
)
```

## Automatic Relationships

When added to a model via `XmiSchemaModelBuilder`, the following relationships are automatically created:

- **XmiHasStructuralMaterial**: Links to Material
- **XmiHasStructuralStorey**: Links to Storey (if not null)
- **XmiHasSegment**: Links to each Segment
- **XmiHasStructuralNode**: Links to each Node

## Usage Examples

### Concrete Slab

```csharp
// Create material
var concrete = new XmiStructuralMaterial(
    id: "MAT001",
    name: "Concrete C30",
    materialType: XmiStructuralMaterialTypeEnum.Concrete,
    grade: 30.0,
    ...
);

// Create storey
var level1 = new XmiStructuralStorey("L01", "Level 1", ..., 3.5, ...);

// Create corner nodes
var node1 = new XmiStructuralPointConnection("N1", "Node-1", ..., level1,
    new XmiPoint3D("P1", "P1", 0.0, 0.0, 3.5, ...));
var node2 = new XmiStructuralPointConnection("N2", "Node-2", ..., level1,
    new XmiPoint3D("P2", "P2", 6.0, 0.0, 3.5, ...));
var node3 = new XmiStructuralPointConnection("N3", "Node-3", ..., level1,
    new XmiPoint3D("P3", "P3", 6.0, 4.0, 3.5, ...));
var node4 = new XmiStructuralPointConnection("N4", "Node-4", ..., level1,
    new XmiPoint3D("P4", "P4", 0.0, 4.0, 3.5, ...));

// Create slab
var slab = new XmiStructuralSurfaceMember(
    id: "SLB001",
    name: "Floor-Slab-1",
    ifcguid: "9Kq1sj$uL2nOP3rTsTuWxY",
    nativeId: "SlabId_001",
    description: "150mm concrete floor slab",
    material: concrete,
    surfaceMemberType: XmiStructuralSurfaceMemberTypeEnum.Slab,
    thickness: 0.15,  // 150mm
    systemPlane: XmiStructuralSurfaceMemberSystemPlaneEnum.Center,
    nodes: new List<XmiStructuralPointConnection> { node1, node2, node3, node4 },
    storey: level1,
    segments: new List<XmiSegment> { /* edge segments */ },
    area: 24.0,  // 6m × 4m
    zOffset: 0.0,
    localAxisX: "1,0,0",
    localAxisY: "0,1,0",
    localAxisZ: "0,0,1",
    height: 0.0  // N/A for slabs
);
```

### Concrete Shear Wall

```csharp
// Wall nodes (vertical surface)
var nodeBase1 = new XmiStructuralPointConnection("NW1", "Wall-Base-1", ...,
    new XmiPoint3D("PW1", "PW1", 0.0, 0.0, 0.0, ...));
var nodeBase2 = new XmiStructuralPointConnection("NW2", "Wall-Base-2", ...,
    new XmiPoint3D("PW2", "PW2", 4.0, 0.0, 0.0, ...));
var nodeTop1 = new XmiStructuralPointConnection("NW3", "Wall-Top-1", ...,
    new XmiPoint3D("PW3", "PW3", 0.0, 0.0, 3.5, ...));
var nodeTop2 = new XmiStructuralPointConnection("NW4", "Wall-Top-2", ...,
    new XmiPoint3D("PW4", "PW4", 4.0, 0.0, 3.5, ...));

// Create wall
var wall = new XmiStructuralSurfaceMember(
    id: "WALL001",
    name: "Shear-Wall-1",
    ifcguid: "0Lr2tk$vM3oQP4sTuTvXyZ",
    nativeId: "WallId_001",
    description: "200mm reinforced concrete shear wall",
    material: concrete,
    surfaceMemberType: XmiStructuralSurfaceMemberTypeEnum.Wall,
    thickness: 0.20,  // 200mm
    systemPlane: XmiStructuralSurfaceMemberSystemPlaneEnum.Center,
    nodes: new List<XmiStructuralPointConnection> { nodeBase1, nodeBase2, nodeTop2, nodeTop1 },
    storey: level1,
    segments: new List<XmiSegment> { /* edge segments */ },
    area: 14.0,  // 4m × 3.5m
    zOffset: 0.0,
    localAxisX: "1,0,0",  // Horizontal in-plane
    localAxisY: "0,0,1",  // Vertical in-plane
    localAxisZ: "0,1,0",  // Normal to wall
    height: 3.5  // Wall height
);
```

### Shell Structure

```csharp
var shell = new XmiStructuralSurfaceMember(
    id: "SHELL001",
    name: "Curved-Shell",
    material: concrete,
    surfaceMemberType: XmiStructuralSurfaceMemberTypeEnum.Shell,
    thickness: 0.10,  // 100mm thin shell
    systemPlane: XmiStructuralSurfaceMemberSystemPlaneEnum.Center,
    nodes: shellNodes,  // Multiple nodes defining curved surface
    segments: shellSegments,  // Curved edge segments
    area: 85.5,
    ...
);
```

## JSON Export Example

```json
{
  "XmiStructuralSurfaceMember": {
    "ID": "SLB001",
    "Name": "Floor-Slab-1",
    "IFCGUID": "9Kq1sj$uL2nOP3rTsTuWxY",
    "NativeId": "SlabId_001",
    "Description": "150mm concrete floor slab",
    "EntityType": "XmiStructuralSurfaceMember",
    "Material": "MAT001",
    "SurfaceMemberType": "Slab",
    "Thickness": 0.15,
    "SystemPlane": "Center",
    "Nodes": ["N1", "N2", "N3", "N4"],
    "Storey": "L01",
    "Segments": ["SEG1", "SEG2", "SEG3", "SEG4"],
    "Area": 24.0,
    "ZOffset": 0.0,
    "LocalAxisX": "1,0,0",
    "LocalAxisY": "0,1,0",
    "LocalAxisZ": "0,0,1",
    "Height": 0.0
  }
}
```

## Surface Member Types

Common `SurfaceMemberType` values:

- **Slab**: Horizontal floor/roof slabs
- **Wall**: Vertical walls
- **Shell**: Curved shell structures
- **Plate**: General plate elements
- **Membrane**: Membrane-only elements (no bending)

(See `XmiStructuralSurfaceMemberTypeEnum` for complete list)

## System Plane Options

The `SystemPlane` defines the reference plane:

- **Top**: Reference at top surface (thickness extends downward)
- **Center**: Reference at mid-thickness (typical for analysis)
- **Bottom**: Reference at bottom surface (thickness extends upward)

## Node Ordering

Nodes should be ordered to form a closed polygon:
```
For rectangular slab:
node1 (0,0) → node2 (L,0) → node3 (L,W) → node4 (0,W) → back to node1
```

Counter-clockwise ordering when viewed from positive Z direction.

## Thickness Conventions

### Typical Thicknesses

```csharp
// Residential floor slab
thickness: 0.15  // 150mm

// Parking slab
thickness: 0.20  // 200mm

// Residential wall
thickness: 0.15  // 150mm

// Shear wall
thickness: 0.25  // 250mm

// Shell roof
thickness: 0.10  // 100mm
```

## Local Coordinate System

For horizontal slabs:
```csharp
localAxisX: "1,0,0"  // Global X
localAxisY: "0,1,0"  // Global Y
localAxisZ: "0,0,1"  // Normal (upward)
```

For vertical walls (in XZ plane):
```csharp
localAxisX: "1,0,0"  // Horizontal in-plane
localAxisY: "0,0,1"  // Vertical in-plane
localAxisZ: "0,1,0"  // Normal (perpendicular to wall)
```

## Relationship Graph

```
XmiStructuralMaterial
        ↑
        │ (XmiHasStructuralMaterial)
        │
XmiStructuralSurfaceMember ──[XmiHasStructuralStorey]──> XmiStructuralStorey
        │
        ├──[XmiHasSegment]──> XmiSegment
        │
        └──[XmiHasStructuralNode]──> XmiStructuralPointConnection
```

## Design Patterns

### Composite Pattern
Surfaces compose from nodes and segments.

### Direct Material Association
Unlike curve members, surface members reference material directly (not through cross-section).

## Related Classes

- **XmiBaseEntity**: Base class
- **XmiStructuralMaterial**: Material properties
- **XmiStructuralPointConnection**: Boundary nodes
- **XmiSegment**: Edge segments
- **XmiStructuralStorey**: Building level

## See Also

- [XmiStructuralMaterial](XmiStructuralMaterial.README.md) - Material properties
- [XmiStructuralPointConnection](XmiStructuralPointConnection.README.md) - Nodes
- [XmiSegment](XmiSegment.README.md) - Edge segments
- [XmiStructuralStorey](XmiStructuralStorey.README.md) - Building levels
- [XmiBaseEntity](../Bases/XmiBaseEntity.README.md) - Base class
