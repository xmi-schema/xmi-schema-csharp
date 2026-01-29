# XmiPoint3d

Represents a 3D point in the XMI analytical model.

## Purpose

`XmiPoint3d` defines 3D coordinates with tolerance-aware equality for representing locations in space.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `X` | `double` | Yes | X coordinate (meters) |
| `Y` | `double` | Yes | Y coordinate (meters) |
| `Z` | `double` | Yes | Z coordinate (meters) |
| `PointType` | `XmiPoint3dTypeEnum` | Yes | Point type classification |

## Usage Example

```csharp
var point = new XmiPoint3d(
    id: "point-1",
    name: "Point A",
    ifcGuid: "2g0X$0m55u9hX1w2a7x5f",
    nativeId: "PT-A",
    description: "Corner point at (0,0,0)",
    x: 0.0,
    y: 0.0,
    z: 0.0,
    pointType: XmiPoint3dTypeEnum.Start
);

model.AddXmiPoint3d(point);
```

## Coordinate System

Points use a global model coordinate system:

| Axis | Description |
|-------|-------------|
| **X** | Horizontal, typically East-West or building length |
| **Y** | Horizontal, typically North-South or building width |
| **Z** | Vertical, elevation |

## Related Classes

- **XmiStructuralPointConnection** - Analytical node connection
- **XmiHasPoint3D** - Relationship linking entities to points
