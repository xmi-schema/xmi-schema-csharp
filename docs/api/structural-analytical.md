---
layout: default
title: Structural Analytical Elements
---

# Structural Analytical Elements

Structural analytical elements represent the mathematical model used for structural analysis. All analytical entities inherit from `XmiBaseStructuralAnalyticalEntity` and have their `Type` automatically set to `StructuralAnalytical`.

## XmiStructuralCurveMember

Beam/column/bracing equivalent with offsets, system line, and fixities. This is the analytical representation of linear structural members.

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `CurveMemberType` | `XmiStructuralCurveMemberTypeEnum` | Classification (Beam, Column, Bracing) |
| `SystemLine` | `XmiStructuralCurveMemberSystemLineEnum` | Location of analytical line relative to profile |
| `Length` | `double` | Analytical length |
| `LocalAxisX` | `string` | Local X axis orientation |
| `LocalAxisY` | `string` | Local Y axis orientation |
| `LocalAxisZ` | `string` | Local Z axis orientation |
| `BeginNodeXOffset` | `double` | X offset at start node |
| `EndNodeXOffset` | `double` | X offset at end node |
| `BeginNodeYOffset` | `double` | Y offset at start node |
| `EndNodeYOffset` | `double` | Y offset at end node |
| `BeginNodeZOffset` | `double` | Z offset at start node |
| `EndNodeZOffset` | `double` | Z offset at end node |
| `EndFixityStart` | `string` | Boundary condition at start |
| `EndFixityEnd` | `string` | Boundary condition at end |

### Example

```csharp
var curveMember = new XmiStructuralCurveMember(
    "cur-1",
    "Analytical Beam",
    "ifc-guid",
    "CUR-1",
    "Analytical curve member",
    XmiStructuralCurveMemberTypeEnum.Beam,
    XmiStructuralCurveMemberSystemLineEnum.MiddleMiddle,
    5.0,
    "1,0,0", "0,1,0", "0,0,1",
    0.1, 0.1, 0, 0, 0, 0,
    "Fixed", "Pinned"
);
```

## XmiStructuralSurfaceMember

Plate, slab, or wall element capturing thickness, axes, and plane metadata. Use to model surface-based analytical elements.

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `SurfaceMemberType` | `XmiStructuralSurfaceMemberTypeEnum` | Classification (Slab, Wall, Panel) |
| `Thickness` | `double` | Element thickness |
| `SystemPlane` | `XmiStructuralSurfaceMemberSystemPlaneEnum` | Analytical plane location |
| `Area` | `double` | Surface area |
| `Eccentricity` | `double` | Offset from reference plane |
| `LocalAxisX` | `string` | Local X axis orientation |
| `LocalAxisY` | `string` | Local Y axis orientation |
| `LocalAxisZ` | `string` | Local Z axis orientation |
| `SpanDirection` | `double` | Primary span direction |

### Example

```csharp
var surfaceMember = new XmiStructuralSurfaceMember(
    "surf-1",
    "Floor Slab",
    "ifc-guid",
    "SURF-1",
    "Analytical surface",
    XmiStructuralSurfaceMemberTypeEnum.Slab,
    0.2,
    XmiStructuralSurfaceMemberSystemPlaneEnum.Middle,
    12.5,
    0.0,
    "1,0,0", "0,1,0", "0,0,1",
    0.3
);
```

## XmiStructuralPointConnection

Analytical node that ties members and storeys together. Create through `XmiModel.CreateStructurePointConnection` to automatically reuse identical coordinates.

### Properties

Inherits base properties from `XmiBaseStructuralAnalyticalEntity`:
- `Id`, `Name`, `IfcGuid`, `NativeId`, `Description`, `EntityType`, `Type`

### Example

```csharp
var pointConnection = new XmiStructuralPointConnection(
    "pc-1",
    "Node 1",
    "ifc-guid",
    "PC-1",
    "Support node"
);

// Link to geometry
var point = new XmiPoint3D("pt-1", "Point", "guid", "PT-1", "coords", 0, 0, 0);
var hasPoint = new XmiHasPoint3D(pointConnection, point);
```

## XmiStructuralStorey

Represents a level with elevation, mass, and reaction info. Link point connections or surfaces to storeys for vertical organization.

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `Elevation` | `double` | Height above reference datum |
| `MassPerArea` | `double` | Distributed mass |
| `ReactionX` | `string` | X-direction reaction forces |
| `ReactionY` | `string` | Y-direction reaction forces |
| `ReactionZ` | `string` | Z-direction reaction forces |

### Example

```csharp
var storey = new XmiStorey(
    "str-1",
    "Level 1",
    "ifc-guid",
    "STR-1",
    "Ground floor level",
    0.0,
    1000,
    "Fx", "Fy", "Fz"
);
```

[Back to API Reference](.)
