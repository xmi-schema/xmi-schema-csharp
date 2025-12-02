# XmiSegment

Represents a geometric segment that defines portions of structural members.

## Overview

`XmiSegment` models a linear segment between two nodes with associated geometry. Segments are used to define the shape of structural members, particularly for curved or multi-segment members. Each segment connects two point connections and has a geometric representation (line, arc, etc.).

## Location

`Models/Entities/XmiSegment.cs`

## Inheritance

```
XmiBaseEntity → XmiSegment
```

## Properties

### Geometric Definition

- **Geometry** (`XmiBaseGeometry`): The geometric shape of the segment
  - Can be `XmiLine3D`, `XmiArc3D`, or other geometry types
  - Defines the actual path between begin and end nodes

### Position

- **Position** (`float`): Position parameter along the member
  - Typically 0.0 to 1.0 representing relative position
  - Used for segmentation and analysis

### Node Connections

- **BeginNode** (`XmiStructuralPointConnection`): Start point of the segment
- **EndNode** (`XmiStructuralPointConnection`): End point of the segment

### Classification

- **SegmentType** (`XmiSegmentTypeEnum`): Type of segment (Line, Arc, Polyline, etc.)

## Constructor

```csharp
public XmiSegment(
    string id,
    string name,
    string ifcguid,
    string nativeId,
    string description,
    XmiBaseGeometry geometry,
    float position,
    XmiStructuralPointConnection beginNode,
    XmiStructuralPointConnection endNode,
    XmiSegmentTypeEnum segmentType
)
```

## Automatic Relationships

When added to a model via `XmiSchemaModelBuilder`, the following relationships are automatically created:

- **XmiHasGeometry**: Links to Geometry
- **XmiHasStructuralNode**: Links to BeginNode
- **XmiHasStructuralNode**: Links to EndNode

## Usage Examples

### Linear Segment

```csharp
// Create nodes
var startNode = new XmiStructuralPointConnection(
    id: "NODE001",
    name: "Node-1",
    point: new XmiPoint3D("P001", "Point-1", 0.0, 0.0, 0.0, ...),
    ...
);

var endNode = new XmiStructuralPointConnection(
    id: "NODE002",
    name: "Node-2",
    point: new XmiPoint3D("P002", "Point-2", 6.0, 0.0, 0.0, ...),
    ...
);

// Create line geometry
var lineGeometry = new XmiLine3D(
    id: "LINE001",
    name: "Line-1",
    startPoint3D: startNode.Point,
    endPoint3D: endNode.Point,
    ...
);

// Create segment
var segment = new XmiSegment(
    id: "SEG001",
    name: "Segment-1",
    ifcguid: "4Fl6ne$pG7iJK8mNoPqRsT",
    nativeId: "SegmentId_001",
    description: "Linear segment between nodes 1 and 2",
    geometry: lineGeometry,
    position: 0.0,
    beginNode: startNode,
    endNode: endNode,
    segmentType: XmiSegmentTypeEnum.Line
);
```

### Curved Segment (Arc)

```csharp
// Create arc geometry
var arcGeometry = new XmiArc3D(
    id: "ARC001",
    name: "Arc-1",
    startPoint: startNode.Point,
    endPoint: endNode.Point,
    centrePoint: new XmiPoint3D("P003", "Center", 3.0, 1.0, 0.0, ...),
    ...
);

// Create arc segment
var arcSegment = new XmiSegment(
    id: "SEG002",
    name: "Arc-Segment-1",
    ifcguid: "5Gm7of$qH8jKL9nOpQrStU",
    nativeId: "SegmentId_002",
    description: "Curved arc segment",
    geometry: arcGeometry,
    position: 0.5,
    beginNode: startNode,
    endNode: endNode,
    segmentType: XmiSegmentTypeEnum.Arc
);
```

### Multi-Segment Member

```csharp
// Create multiple segments for a curved member
var segment1 = new XmiSegment(...);  // First segment
var segment2 = new XmiSegment(...);  // Second segment
var segment3 = new XmiSegment(...);  // Third segment

// Assign to member
var member = new XmiStructuralCurveMember(
    id: "M001",
    name: "Curved-Beam",
    segments: new List<XmiBaseEntity> { segment1, segment2, segment3 },
    ...
);

// Add to model
var builder = new XmiSchemaModelBuilder();
builder.AddEntity(startNode);
builder.AddEntity(endNode);
builder.AddEntity(segment1);
builder.AddEntity(segment2);
builder.AddEntity(segment3);
builder.AddEntity(member);

var model = builder.BuildModel();
// Relationships: member → segments, segments → nodes, segments → geometry
```

## JSON Export Example

```json
{
  "XmiSegment": {
    "ID": "SEG001",
    "Name": "Segment-1",
    "IFCGUID": "4Fl6ne$pG7iJK8mNoPqRsT",
    "NativeId": "SegmentId_001",
    "Description": "Linear segment between nodes 1 and 2",
    "EntityType": "XmiSegment",
    "Geometry": "LINE001",
    "Position": 0.0,
    "BeginNode": "NODE001",
    "EndNode": "NODE002",
    "SegmentType": "Line"
  }
}
```

## Segment Types

Common `SegmentType` enum values:

- **Line**: Straight line segment
- **Arc**: Circular arc segment
- **Polyline**: Multi-segment polyline
- **Spline**: Curved spline segment

(See `XmiSegmentTypeEnum` for complete list)

## Position Parameter

The `Position` property can be used to:
- Define segment location along member (0.0 = start, 1.0 = end)
- Support parametric modeling
- Enable interpolation of properties along member

## Relationship Graph

```
XmiStructuralPointConnection (BeginNode)
        ↑
        │ (XmiHasStructuralNode)
        │
XmiSegment ──[XmiHasGeometry]──> XmiLine3D/XmiArc3D
        │
        │ (XmiHasStructuralNode)
        ↓
XmiStructuralPointConnection (EndNode)
```

## Usage in Members

Segments are referenced by structural members:

```
XmiStructuralCurveMember
        │
        │ (XmiHasSegment)
        ↓
XmiSegment → XmiLine3D/XmiArc3D
```

## Design Patterns

### Composite Pattern
Segments compose complex member geometries from simple primitives.

### Bridge Pattern
Separates geometric representation (Line3D, Arc3D) from structural connectivity (nodes).

## Related Classes

- **XmiBaseEntity**: Base class
- **XmiBaseGeometry**: Geometric shape (Line3D, Arc3D)
- **XmiLine3D**: Linear geometry
- **XmiArc3D**: Curved geometry
- **XmiStructuralPointConnection**: Node connections
- **XmiStructuralCurveMember**: Uses segments

## See Also

- [XmiLine3D](../Geometries/XmiLine3D.README.md) - Linear geometry
- [XmiArc3D](../Geometries/XmiArc3D.README.md) - Arc geometry
- [XmiStructuralPointConnection](XmiStructuralPointConnection.README.md) - Nodes
- [XmiStructuralCurveMember](XmiStructuralCurveMember.README.md) - Uses segments
- [XmiBaseGeometry](../Bases/XmiBaseGeometry.README.md) - Base geometry class
