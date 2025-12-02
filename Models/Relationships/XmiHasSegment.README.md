# XmiHasSegment

Relationship linking structural members to their geometric segments.

## Overview

`XmiHasSegment` represents an association between structural members (curve or surface) and their constituent segments. This relationship defines the geometric composition of members, particularly useful for curved or multi-segment elements.

## Location

`Models/Relationships/XmiHasSegment.cs`

## Inheritance

```
XmiBaseRelationship → XmiHasSegment
```

## UML Type

**Association** - Represents a composition relationship where members are composed of one or more segments.

## Constructors

### Full Constructor

```csharp
public XmiHasSegment(
    string id,
    XmiBaseEntity source,
    XmiBaseEntity target,
    string name,
    string description,
    string entityName,
    string umlType
)
```

### Simplified Constructor (Auto-generated ID)

```csharp
public XmiHasSegment(
    XmiBaseEntity source,
    XmiBaseEntity target
)
```

## Automatic Creation

This relationship is automatically created by `ExtensionRelationshipExporter` for:

### 1. Curve Member Segments

```csharp
if (entity is XmiStructuralCurveMember curveWithSegments)
{
    foreach (var segment in curveWithSegments.Segments)
    {
        _relationshipManager.AddRelationship(
            new XmiHasSegment(curveWithSegments, segment)
        );
    }
}
```

### 2. Surface Member Segments

```csharp
if (entity is XmiStructuralSurfaceMember surfaceWithSegments)
{
    foreach (var segment in surfaceWithSegments.Segments)
    {
        _relationshipManager.AddRelationship(
            new XmiHasSegment(surfaceWithSegments, segment)
        );
    }
}
```

## Usage Examples

### Straight Beam (Single Segment)

```csharp
// Create nodes
var node1 = new XmiStructuralPointConnection("N1", ...);
var node2 = new XmiStructuralPointConnection("N2", ...);

// Create linear segment
var segment = new XmiSegment(
    id: "SEG001",
    name: "Beam-Segment",
    beginNode: node1,
    endNode: node2,
    geometry: new XmiLine3D(...),
    ...
);

// Create beam with segment
var beam = new XmiStructuralCurveMember(
    id: "B001",
    name: "Beam-1",
    segments: new List<XmiBaseEntity> { segment },
    ...
);

// Add to model
var builder = new XmiSchemaModelBuilder();
builder.AddEntities(new[] { node1, node2 });
builder.AddEntity(segment);
builder.AddEntity(beam);

var model = builder.BuildModel();
// Relationship automatically created: beam → segment
```

### Curved Member (Multiple Segments)

```csharp
// Create three segments forming a curved beam
var segment1 = new XmiSegment("SEG001", "Segment-1", ...,
    geometry: new XmiLine3D(...), ...);

var segment2 = new XmiSegment("SEG002", "Segment-2", ...,
    geometry: new XmiArc3D(...), ...);  // Curved segment

var segment3 = new XmiSegment("SEG003", "Segment-3", ...,
    geometry: new XmiLine3D(...), ...);

// Create curved member
var curvedBeam = new XmiStructuralCurveMember(
    id: "B002",
    name: "Curved-Beam",
    segments: new List<XmiBaseEntity> { segment1, segment2, segment3 },
    ...
);

builder.AddEntities(new[] { segment1, segment2, segment3 });
builder.AddEntity(curvedBeam);

model = builder.BuildModel();
// Relationships automatically created:
// - curvedBeam → segment1
// - curvedBeam → segment2
// - curvedBeam → segment3
```

### Slab with Edge Segments

```csharp
// Create segments defining slab perimeter
var edge1 = new XmiSegment("EDGE1", "Edge-1", ...);
var edge2 = new XmiSegment("EDGE2", "Edge-2", ...);
var edge3 = new XmiSegment("EDGE3", "Edge-3", ...);
var edge4 = new XmiSegment("EDGE4", "Edge-4", ...);

// Create slab
var slab = new XmiStructuralSurfaceMember(
    id: "SLB001",
    name: "Floor-Slab",
    segments: new List<XmiSegment> { edge1, edge2, edge3, edge4 },
    ...
);

builder.AddEntities(new[] { edge1, edge2, edge3, edge4 });
builder.AddEntity(slab);

model = builder.BuildModel();
// Relationships automatically created for each edge
```

## Graph Representations

### Single Segment Member

```
XmiStructuralCurveMember ──[XmiHasSegment]──> XmiSegment
```

### Multi-Segment Member

```
                    XmiSegment (1)
                        ↑
                        │ [XmiHasSegment]
XmiStructuralCurveMember ──┼──> XmiSegment (2)
                        │
                        └──> XmiSegment (3)
```

### Complete Segment Hierarchy

```
XmiStructuralCurveMember
        │
        │ [XmiHasSegment]
        ↓
XmiSegment
        │
        ├──[XmiHasGeometry]──> XmiLine3D/XmiArc3D
        │
        └──[XmiHasStructuralNode]──> XmiStructuralPointConnection
```

## JSON Export Example

```json
{
  "XmiHasSegment": {
    "ID": "rel-004",
    "Source": "B001",
    "Target": "SEG001",
    "Name": "XmiHasSegment",
    "Description": "",
    "EntityType": "XmiHasSegment",
    "UmlType": "Association"
  }
}
```

## Use Cases

### 1. Curved Members
Representing arches, curved beams, or circular columns using multiple arc and line segments.

### 2. Tapered Members
Members with varying cross-sections along their length, with each segment having different properties.

### 3. Complex Geometry
Complex member shapes that cannot be represented by a single geometric primitive.

### 4. Surface Boundaries
Defining the perimeter of surface elements (slabs, walls) using edge segments.

## Segment Ordering

Segments should be ordered sequentially along the member:

```csharp
// Correct: segments ordered along member path
segments: new[] { segment1, segment2, segment3 }
// segment1.EndNode == segment2.BeginNode
// segment2.EndNode == segment3.BeginNode
```

## Related Classes

- **XmiBaseRelationship**: Base class
- **XmiSegment**: Target entity (geometric segment)
- **XmiStructuralCurveMember**: Source entity (beam, column, brace)
- **XmiStructuralSurfaceMember**: Source entity (slab, wall)
- **ExtensionRelationshipExporter**: Creates this relationship automatically

## See Also

- [XmiSegment](../Entities/XmiSegment.README.md) - Segment definition
- [XmiStructuralCurveMember](../Entities/XmiStructuralCurveMember.README.md) - Curve member
- [XmiStructuralSurfaceMember](../Entities/XmiStructuralSurfaceMember.README.md) - Surface member
- [XmiBaseRelationship](../Bases/XmiBaseRelationship.README.md) - Base class
