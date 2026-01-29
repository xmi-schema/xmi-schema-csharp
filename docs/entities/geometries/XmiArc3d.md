# XmiArc3d

Represents a circular arc with start, end, center, and radius in the XMI analytical model.

## Purpose

`XmiArc3d` defines 3D circular geometric primitives for representing curved edges.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `StartPoint` | `XmiPoint3d` | Yes | Starting point of arc |
| `EndPoint` | `XmiPoint3d` | Yes | Ending point of arc |
| `CenterPoint` | `XmiPoint3d` | Yes | Center point of arc |
| `Radius` | `double` | Yes | Arc radius (meters) |

## Usage Example

```csharp
var startPoint = new XmiPoint3d(/* ... */);
var endPoint = new XmiPoint3d(/* ... */);
var centerPoint = new XmiPoint3d(/* ... */);

var arc = new XmiArc3d(
    id: "arc-1",
    name: "Arc from A to B",
    ifcGuid: "2g0X$0m55u9hX1w2a7x5f",
    nativeId: "ARC-AB",
    description: "Circular arc from point A to point B with center C",
    startPoint: startPoint,
    endPoint: endPoint,
    centerPoint: centerPoint,
    radius: 5.0
);

model.AddXmiArc3d(arc);
```

## Related Classes

- **XmiPoint3d** - 3D coordinate points
- **XmiHasArc3D** - Relationship linking entities to arcs
