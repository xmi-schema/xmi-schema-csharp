# XmiStructuralPointConnection

Represents a structural node or connection point in a structural model.

## Overview

`XmiStructuralPointConnection` models discrete connection points in the structural system. These nodes define locations where structural members connect, boundary conditions are applied, and loads can be assigned. Each node has a spatial location (via Point3D) and can be associated with a building storey.

## Location

`Models/Entities/XmiStructuralPointConnection.cs`

## Inheritance

```
XmiBaseEntity → XmiStructuralPointConnection
```

## Properties

### Spatial Definition

- **Point** (`XmiPoint3D`): The 3D spatial coordinates of this connection point
  - Defines X, Y, Z position in the structural model
  - Reference to a `XmiPoint3D` geometry object

### Hierarchical Organization

- **Storey** (`XmiStructuralStorey`): The building level this node belongs to
  - Can be null for nodes not associated with a specific storey
  - Enables organization by building level

## Constructor

```csharp
public XmiStructuralPointConnection(
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiStructuralStorey storey,
    XmiPoint3D point
)
```

## Automatic Relationships

When added to a model via `XmiSchemaModelBuilder`, the following relationships are automatically created:

- **XmiHasGeometry**: Links to Point (XmiPoint3D)
- **XmiHasStructuralStorey**: Links to Storey (if not null)

## Usage Examples

### Basic Node Creation

```csharp
// Create 3D point
var point3D = new XmiPoint3D(
    id: "P001",
    name: "Point-1",
    coordinateX: 0.0,
    coordinateY: 0.0,
    coordinateZ: 3.5,
    ...
);

// Create structural node
var node = new XmiStructuralPointConnection(
    id: "NODE001",
    name: "Column-Base-Node",
    ifcGuid: "6Hn8pg$rI9kLM0oQpRrTsUv",
    nativeId: "NodeId_001",
    description: "Base node for column at grid A1",
    storey: null,  // Ground level, no storey assigned
    point: point3D
);
```

### Node with Storey Assignment

```csharp
// Create storey
var level1 = new XmiStructuralStorey(
    id: "L01",
    name: "Level 1",
    storeyElevation: 3.5,
    ...
);

// Create point at level 1
var point = new XmiPoint3D(
    id: "P002",
    name: "Point-2",
    coordinateX: 6.0,
    coordinateY: 0.0,
    coordinateZ: 3.5,  // Matches storey elevation
    ...
);

// Create node with storey
var node = new XmiStructuralPointConnection(
    id: "NODE002",
    name: "L1-Node-A1",
    ifcGuid: "7Io9qh$sJ0lMN1pRqStUvW",
    nativeId: "NodeId_002",
    description: "Level 1 node at grid A1",
    storey: level1,
    point: point
);
```

### Building a Frame System

```csharp
var builder = new XmiSchemaModelBuilder();

// Create storey
var level1 = new XmiStructuralStorey("L01", "Level 1", ..., storeyElevation: 3.5, ...);

// Create nodes at corners of a bay
var node1 = new XmiStructuralPointConnection("N1", "Node-1", ..., level1,
    new XmiPoint3D("P1", "P1", 0.0, 0.0, 3.5, ...));

var node2 = new XmiStructuralPointConnection("N2", "Node-2", ..., level1,
    new XmiPoint3D("P2", "P2", 6.0, 0.0, 3.5, ...));

var node3 = new XmiStructuralPointConnection("N3", "Node-3", ..., level1,
    new XmiPoint3D("P3", "P3", 6.0, 6.0, 3.5, ...));

var node4 = new XmiStructuralPointConnection("N4", "Node-4", ..., level1,
    new XmiPoint3D("P4", "P4", 0.0, 6.0, 3.5, ...));

// Create beams connecting nodes
var beam1 = new XmiStructuralCurveMember(
    id: "B1",
    name: "Beam-1",
    beginNode: node1,
    endNode: node2,
    nodes: new List<XmiStructuralPointConnection> { node1, node2 },
    ...
);

var beam2 = new XmiStructuralCurveMember(
    id: "B2",
    name: "Beam-2",
    beginNode: node2,
    endNode: node3,
    nodes: new List<XmiStructuralPointConnection> { node2, node3 },
    ...
);

// Add to model
builder.AddEntity(level1);
builder.AddEntities(new[] { node1, node2, node3, node4 });
builder.AddEntities(new[] { beam1, beam2 });

var model = builder.BuildModel();
```

## JSON Export Example

```json
{
  "XmiStructuralPointConnection": {
    "ID": "NODE001",
    "Name": "Column-Base-Node",
    "IFCGUID": "6Hn8pg$rI9kLM0oQpRrTsUv",
    "NativeId": "NodeId_001",
    "Description": "Base node for column at grid A1",
    "EntityType": "XmiStructuralPointConnection",
    "Storey": "L01",
    "Point": "P001"
  }
}
```

## Relationship Graph

```
XmiStructuralStorey
        ↑
        │ (XmiHasStructuralStorey)
        │
XmiStructuralPointConnection ──[XmiHasGeometry]──> XmiPoint3D
        ↑
        │ (XmiHasStructuralNode)
        │
XmiStructuralCurveMember / XmiSegment
```

## Usage in Structural Analysis

Nodes serve multiple purposes in structural analysis:

### Connection Points
- Where structural members meet and connect
- Transfer forces between members

### Boundary Conditions
- Support locations (pins, rollers, fixed supports)
- Restraints against translation/rotation

### Load Application Points
- Point loads
- Concentrated moments
- Reactions

### Degrees of Freedom
Each node typically has 6 degrees of freedom:
- 3 translations (X, Y, Z)
- 3 rotations (about X, Y, Z axes)

## Common Node Configurations

### Column Base (Fixed Support)
```csharp
var baseNode = new XmiStructuralPointConnection(
    id: "N_BASE",
    name: "Column-Base",
    storey: null,  // Foundation level
    point: new XmiPoint3D(..., 0.0, 0.0, 0.0, ...)  // Ground level
);
// Typically fully restrained (all DOF = 0)
```

### Beam-Column Joint
```csharp
var jointNode = new XmiStructuralPointConnection(
    id: "N_JOINT",
    name: "Beam-Column-Joint",
    storey: level2,
    point: new XmiPoint3D(..., 3.0, 4.0, 7.0, ...)
);
// Connects multiple members, typically rigid
```

### Mid-Span Node
```csharp
var midNode = new XmiStructuralPointConnection(
    id: "N_MID",
    name: "Beam-Midspan",
    storey: level1,
    point: new XmiPoint3D(..., 3.0, 0.0, 3.5, ...)
);
// For load application or member subdivision
```

## Design Patterns

### Value Object Pattern
The Point3D geometry defines the immutable spatial position.

### Association Pattern
Links to both geometric (Point3D) and organizational (Storey) hierarchies.

## Related Classes

- **XmiBaseEntity**: Base class
- **XmiPoint3D**: 3D spatial coordinates
- **XmiStructuralStorey**: Building level organization
- **XmiStructuralCurveMember**: References nodes for connectivity
- **XmiSegment**: Uses nodes as begin/end points

## See Also

- [XmiPoint3D](../Geometries/XmiPoint3D.README.md) - 3D coordinates
- [XmiStructuralStorey](XmiStructuralStorey.README.md) - Building levels
- [XmiStructuralCurveMember](XmiStructuralCurveMember.README.md) - Uses nodes
- [XmiSegment](XmiSegment.README.md) - Uses nodes
- [XmiBaseEntity](../Bases/XmiBaseEntity.README.md) - Base class
