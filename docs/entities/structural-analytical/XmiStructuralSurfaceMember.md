# XmiStructuralSurfaceMember

Models plates, slabs, and other surface members in XMI analytical model.

## Purpose

`XmiStructuralSurfaceMember` defines 2D analytical elements used for structural analysis.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `SurfaceMemberType` | `XmiStructuralSurfaceMemberTypeEnum` | Yes | Surface type classification |
| `Thickness` | `double` | Yes | Member thickness (meters) |
| `SystemPlane` | `XmiStructuralSurfaceMemberSystemPlaneEnum` | Yes | Plane orientation relative to model |
| `Area` | `double` | Yes | Planar area (m²) |
| `ZOffset` | `double` | Yes | Offset along Z axis (meters) |
| `LocalAxisX` | `XmiAxis` | Yes | Unit direction of local X axis |
| `LocalAxisY` | `XmiAxis` | Yes | Unit direction of local Y axis |
| `LocalAxisZ` | `XmiAxis` | Yes | Unit direction of local Z axis |
| `Height` | `double` | Yes | Total extrusion height (meters) |

## Usage Example

```csharp
var surface = new XmiStructuralSurfaceMember(
    id: "surf-1",
    name: "Analytical Floor Slab",
    ifcGuid: "2g0X$0m55u9hX1w2a7x5f",
    nativeId: "SURF-F1",
    description: "Finite element model of floor slab",
    surfaceMemberType: XmiStructuralSurfaceMemberTypeEnum.Slab,
    thickness: 0.20,
    systemPlane: XmiStructuralSurfaceMemberSystemPlaneEnum.Horizontal,
    area: 64.0,  // 8m x 8m
    zOffset: 0.0,
    localAxisX: new XmiAxis(1, 0, 0),
    localAxisY: new XmiAxis(0, 1, 0),
    localAxisZ: new XmiAxis(0, 0, 1),
    height: 0.20
);

model.AddXmiStructuralSurfaceMember(surface);
```

## Related Classes

- **XmiSlab** / **XmiWall** - Physical element linked to analytical surface
- **XmiStructuralPointConnection** - Node connections defining corners
- **XmiHasGeometry** - Geometry relationship
- **XmiStructuralSurfaceMemberTypeEnum** - Surface type classification
