# XmiStructuralCurveMember

Represents 1D structural elements such as beams, columns, and braces in a structural model.

## Overview

`XmiStructuralCurveMember` models linear structural members that carry loads through bending, axial, and torsional forces. It includes geometric, material, and connectivity information essential for structural analysis.

## Location

`Models/Entities/XmiStructuralCurveMember.cs`

## Inheritance

```
XmiBaseEntity → XmiStructuralCurveMember
```

## Properties

### References to Other Entities

- **CrossSection** (`XmiStructuralCrossSection`): The cross-section profile of the member
- **Storey** (`XmiStructuralStorey`): The building level this member belongs to
- **Nodes** (`List<XmiStructuralPointConnection>`): Collection of nodes along the member
- **Segments** (`List<XmiBaseEntity>`): Geometric segments defining the member's shape
- **BeginNode** (`XmiStructuralPointConnection`): Starting node of the member
- **EndNode** (`XmiStructuralPointConnection`): Ending node of the member

### Type and Classification

- **CurvememberType** (`XmiStructuralCurveMemberTypeEnum`): Type of member (Beam, Column, Brace, etc.)
- **SystemLine** (`XmiStructuralCurveMemberSystemLineEnum`): Reference line position (Top, Center, Bottom, etc.)

### Geometric Properties

- **Length** (`double`): Total length of the member
- **LocalAxisX** (`string`): Local X-axis orientation
- **LocalAxisY** (`string`): Local Y-axis orientation
- **LocalAxisZ** (`string`): Local Z-axis orientation

### Node Offsets

- **BeginNodeXOffset** (`double`): X-offset at start node
- **BeginNodeYOffset** (`double`): Y-offset at start node
- **BeginNodeZOffset** (`double`): Z-offset at start node
- **EndNodeXOffset** (`double`): X-offset at end node
- **EndNodeYOffset** (`double`): Y-offset at end node
- **EndNodeZOffset** (`double`): Z-offset at end node

### Boundary Conditions

- **EndFixityStart** (`string`): End fixity condition at start (Fixed, Pinned, etc.)
- **EndFixityEnd** (`string`): End fixity condition at end (Fixed, Pinned, etc.)

## Constructor

```csharp
public XmiStructuralCurveMember(
    string id,
    string name,
    string ifcguid,
    string nativeId,
    string description,
    XmiStructuralCrossSection crossSection,
    XmiStructuralStorey storey,
    XmiStructuralCurveMemberTypeEnum curvememberType,
    List<XmiStructuralPointConnection> nodes,
    List<XmiBaseEntity> segments,
    XmiStructuralCurveMemberSystemLineEnum systemLine,
    XmiStructuralPointConnection beginNode,
    XmiStructuralPointConnection endNode,
    double length,
    string localAxisX,
    string localAxisY,
    string localAxisZ,
    double beginNodeXOffset,
    double endNodeXOffset,
    double beginNodeYOffset,
    double endNodeYOffset,
    double beginNodeZOffset,
    double endNodeZOffset,
    string endFixityStart,
    string endFixityEnd
)
```

## Automatic Relationships

When added to a model via `XmiSchemaModelBuilder`, the following relationships are automatically created:

- **XmiHasStructuralCrossSection**: Links to CrossSection
- **XmiHasStructuralStorey**: Links to Storey (if not null)
- **XmiHasSegment**: Links to each Segment in Segments list
- **XmiHasStructuralNode**: Links to each Node in Nodes list
- **XmiHasStructuralNode**: Links to BeginNode (if not null)
- **XmiHasStructuralNode**: Links to EndNode (if not null)

## Usage Example

```csharp
// Create dependencies
var material = new XmiStructuralMaterial(
    id: "MAT001",
    name: "Concrete C30",
    materialType: XmiStructuralMaterialTypeEnum.Concrete,
    grade: 30.0,
    ...
);

var crossSection = new XmiStructuralCrossSection(
    id: "CS001",
    name: "300x600",
    material: material,
    shape: XmiShapeEnum.Rectangular,
    area: 0.18,
    ...
);

var storey = new XmiStructuralStorey(
    id: "L01",
    name: "Level 1",
    elevation: 0.0,
    ...
);

var startNode = new XmiStructuralPointConnection(...);
var endNode = new XmiStructuralPointConnection(...);

// Create curve member (beam)
var beam = new XmiStructuralCurveMember(
    id: "B001",
    name: "Beam-1",
    ifcguid: "2Dj4jc$nD2hBU9kSEOLXyH",
    nativeId: "RevitId_12345",
    description: "Main support beam",
    crossSection: crossSection,
    storey: storey,
    curvememberType: XmiStructuralCurveMemberTypeEnum.Beam,
    nodes: new List<XmiStructuralPointConnection> { startNode, endNode },
    segments: new List<XmiBaseEntity> { /* segment objects */ },
    systemLine: XmiStructuralCurveMemberSystemLineEnum.Center,
    beginNode: startNode,
    endNode: endNode,
    length: 6.0,
    localAxisX: "1,0,0",
    localAxisY: "0,1,0",
    localAxisZ: "0,0,1",
    beginNodeXOffset: 0.0,
    endNodeXOffset: 0.0,
    beginNodeYOffset: 0.0,
    endNodeYOffset: 0.0,
    beginNodeZOffset: 0.0,
    endNodeZOffset: 0.0,
    endFixityStart: "Fixed",
    endFixityEnd: "Pinned"
);

// Add to model
var builder = new XmiSchemaModelBuilder();
builder.AddEntity(material);
builder.AddEntity(crossSection);
builder.AddEntity(storey);
builder.AddEntity(startNode);
builder.AddEntity(endNode);
builder.AddEntity(beam);

var model = builder.BuildModel();
// Relationships automatically created
```

## JSON Export Example

```json
{
  "XmiStructuralCurveMember": {
    "ID": "B001",
    "Name": "Beam-1",
    "IFCGUID": "2Dj4jc$nD2hBU9kSEOLXyH",
    "NativeId": "RevitId_12345",
    "Description": "Main support beam",
    "EntityType": "XmiStructuralCurveMember",
    "CrossSection": "CS001",
    "Storey": "L01",
    "CurvememberType": "Beam",
    "Nodes": ["NODE001", "NODE002"],
    "Segments": ["SEG001"],
    "SystemLine": "Center",
    "BeginNode": "NODE001",
    "EndNode": "NODE002",
    "Length": 6.0,
    "LocalAxisX": "1,0,0",
    "LocalAxisY": "0,1,0",
    "LocalAxisZ": "0,0,1",
    "BeginNodeXOffset": 0.0,
    "EndNodeXOffset": 0.0,
    "BeginNodeYOffset": 0.0,
    "EndNodeYOffset": 0.0,
    "BeginNodeZOffset": 0.0,
    "EndNodeZOffset": 0.0,
    "EndFixityStart": "Fixed",
    "EndFixityEnd": "Pinned"
  }
}
```

## Member Types

Common values for `CurvememberType`:

- **Beam**: Horizontal load-bearing member
- **Column**: Vertical load-bearing member
- **Brace**: Diagonal bracing member
- And more (see `XmiStructuralCurveMemberTypeEnum`)

## System Lines

The `SystemLine` property defines the reference line position:

- **Top**: Reference line at top of cross-section
- **Center**: Reference line at centroid
- **Bottom**: Reference line at bottom of cross-section
- And more (see `XmiStructuralCurveMemberSystemLineEnum`)

## End Fixity

Common end fixity conditions:

- **Fixed**: Fully restrained (no translation or rotation)
- **Pinned**: Restrained translation, free rotation
- **Free**: No restraints
- **Custom**: User-defined fixity

## Local Coordinate System

The local axes define the member's orientation:

- **LocalAxisX**: Longitudinal axis (along member length)
- **LocalAxisY**: Major axis (typically vertical for beams)
- **LocalAxisZ**: Minor axis (perpendicular to X and Y)

Format: "X,Y,Z" (e.g., "1,0,0" for X-axis pointing in global X direction)

## Node Offsets

Offsets allow precise positioning of connections:

```
Offset > 0: Moves connection away from node in positive axis direction
Offset < 0: Moves connection toward node in negative axis direction
Offset = 0: Connection at node position
```

## Related Classes

- **XmiBaseEntity**: Base class
- **XmiStructuralCrossSection**: Cross-section definition
- **XmiStructuralMaterial**: Material properties (via cross-section)
- **XmiStructuralPointConnection**: Node connections
- **XmiStructuralStorey**: Building level
- **XmiSegment**: Geometric segments

## See Also

- [XmiStructuralCrossSection](XmiStructuralCrossSection.README.md) - Cross-section properties
- [XmiStructuralMaterial](XmiStructuralMaterial.README.md) - Material properties
- [XmiStructuralPointConnection](XmiStructuralPointConnection.README.md) - Node definitions
- [XmiBaseEntity](../Bases/XmiBaseEntity.README.md) - Base class
