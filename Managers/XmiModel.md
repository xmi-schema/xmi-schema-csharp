---
title: "XmiModel Factory Methods"
layout: default
parent: "API Reference"
nav_order: 2
---

# XmiModel Factory Methods

The `XmiModel` class provides factory methods for creating all types of entities in the Cross Model Information graph.

## Physical Element Factories

### CreateXmiBeam

Creates a beam physical element and optionally links material and segments.

```csharp
public XmiBeam CreateXmiBeam(
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiMaterial? material,
    List<XmiSegment>? segments,
    List<int>? positions,
    XmiSystemLineEnum systemLine,
    double length,
    XmiAxis localAxisX,
    XmiAxis localAxisY,
    XmiAxis localAxisZ,
    double beginNodeXOffset,
    double endNodeXOffset,
    double beginNodeYOffset,
    double endNodeYOffset,
    double beginNodeZOffset,
    double endNodeZOffset
)
```

### CreateXmiColumn

Creates a column physical element and optionally links material and segments.

```csharp
public XmiColumn CreateXmiColumn(
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiMaterial? material,
    List<XmiSegment>? segments,
    List<int>? positions,
    XmiSystemLineEnum systemLine,
    double length,
    XmiAxis localAxisX,
    XmiAxis localAxisY,
    XmiAxis localAxisZ,
    double beginNodeXOffset,
    double endNodeXOffset,
    double beginNodeYOffset,
    double endNodeYOffset,
    double beginNodeZOffset,
    double endNodeZOffset
)
```

### CreateXmiSlab

Creates a slab physical element and optionally links material and segments.

```csharp
public XmiSlab CreateXmiSlab(
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiMaterial? material,
    List<XmiSegment>? segments,
    List<int>? positions,
    double zOffset,
    XmiAxis localAxisX,
    XmiAxis localAxisY,
    XmiAxis localAxisZ,
    double thickness
)
```

### CreateXmiWall

Creates a wall physical element and optionally links material and segments.

```csharp
public XmiWall CreateXmiWall(
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiMaterial? material,
    List<XmiSegment>? segments,
    List<int>? positions,
    double zOffset,
    XmiAxis localAxisX,
    XmiAxis localAxisY,
    XmiAxis localAxisZ,
    double height
)
```

## Structural Analytical Element Factories

### CreateXmiStructuralCurveMember

Creates a structural curve member (beam, column, bracing) and optionally links material, cross-section, storey, and segments.

```csharp
public XmiStructuralCurveMember CreateXmiStructuralCurveMember(
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiMaterial? material,
    XmiCrossSection? crossSection,
    XmiStorey? storey,
    XmiStructuralCurveMemberTypeEnum curveMemberType,
    List<XmiStructuralPointConnection> nodes,
    List<XmiSegment>? segments,
    List<int>? positions,
    XmiSystemLineEnum systemLine,
    XmiStructuralPointConnection beginNode,
    XmiStructuralPointConnection endNode,
    double length,
    XmiAxis localAxisX,
    XmiAxis localAxisY,
    XmiAxis localAxisZ,
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

### CreateXmiStructuralSurfaceMember

Creates a structural surface member (slab, wall, plate) and optionally links material, storey, and segments.

```csharp
public XmiStructuralSurfaceMember CreateXmiStructuralSurfaceMember(
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiMaterial? material,
    XmiStructuralSurfaceMemberTypeEnum surfaceMemberType,
    double thickness,
    XmiStructuralSurfaceMemberSystemPlaneEnum systemPlane,
    List<XmiStructuralPointConnection> nodes,
    XmiStorey? storey,
    List<XmiSegment> segments,
    List<int> positions,
    double area,
    double zOffset,
    XmiAxis localAxisX,
    XmiAxis localAxisY,
    XmiAxis localAxisZ,
    double height
)
```

### CreateXmiStructuralPointConnection

Creates a structural point connection (node) in the model.

```csharp
public XmiStructuralPointConnection CreateXmiStructuralPointConnection(
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiPoint3d point
)
```

## Geometry Factories

### CreateXmiPoint3D

Creates a 3D point with automatic deduplication based on coordinates.

```csharp
public XmiPoint3d CreateXmiPoint3D(
    double x,
    double y,
    double z,
    string? id = null,
    string? name = null,
    string? ifcGuid = null,
    string? nativeId = null,
    string? description = null
)
```

### CreateXmiLine3D

Creates a 3D line between two points with automatic deduplication.

```csharp
public XmiLine3d CreateXmiLine3d(
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiPoint3d startPoint,
    XmiPoint3d endPoint
)
```

### CreateXmiArc3D

Creates a 3D circular arc with automatic deduplication.

```csharp
public XmiArc3d CreateXmiArc3d(
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiPoint3d startPoint,
    XmiPoint3d endPoint,
    XmiPoint3d centerPoint,
    float radius
)
```

## Segment Factories

### CreateXmiLineSegment

Creates a line segment with associated geometry and point relationships.

```csharp
public XmiSegment CreateXmiLineSegment(
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiLine3d line
)
```

### CreateXmiArcSegment

Creates an arc segment with associated geometry and point relationships.

```csharp
public XmiSegment CreateXmiArcSegment(
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiArc3d arc
)
```

## Usage Guidelines

### Segments and Positions

When providing segments to factory methods, you must also provide a corresponding `positions` array:

```csharp
var segments = new List<XmiSegment>
{
    new XmiSegment("seg-1", "Segment 1", "", "native-1", "", XmiSegmentTypeEnum.Line),
    new XmiSegment("seg-2", "Segment 2", "", "native-2", "", XmiSegmentTypeEnum.Line)
};

var positions = new List<int> { 0, 1 }; // Must match segments count

var beam = model.CreateXmiBeam(
    "beam-1", "Beam 1", "", "native-beam-1", "", 
    material, segments, positions, XmiSystemLineEnum.MiddleMiddle, 5.0,
    axisX, axisY, axisZ, 0, 0, 0, 0, 0, 0);
```

### Automatic Deduplication

- **Points**: `CreateXmiPoint3D` automatically reuses existing points with same coordinates
- **Lines**: `CreateXmiLine3d` automatically reuses existing lines with same endpoints
- **Arcs**: `CreateXmiArc3d` automatically reuses existing arcs with same properties

### Relationship Creation

Factory methods automatically create necessary relationships:
- `XmiHasMaterial` when material is provided
- `XmiHasCrossSection` when cross-section is provided
- `XmiHasStorey` when storey is provided
- `XmiHasSegment` when segments are provided
- `XmiHasLine3d`/`XmiHasArc3d` for segment geometry
- `XmiHasPoint3d` for segment endpoints