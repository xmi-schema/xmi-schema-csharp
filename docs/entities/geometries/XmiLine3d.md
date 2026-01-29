# XmiLine3d

Represents a straight line between two points in the XMI analytical model.

## Purpose

`XmiLine3d` defines 3D linear geometric primitives for representing straight edges.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `BeginPoint` | `XmiPoint3d` | Yes | Starting point of line |
| `EndPoint` | `XmiPoint3d` | Yes | Ending point of line |

## Usage Example

```csharp
var startPoint = new XmiPoint3d(/* ... */);
var endPoint = new XmiPoint3d(/* ... */);

var line = new XmiLine3d(
    id: "line-1",
    name: "Line from A to B",
    ifcGuid: "2g0X$0m55u9hX1w2a7x5f",
    nativeId: "LINE-AB",
    description: "Straight edge from point A to point B",
    beginPoint: startPoint,
    endPoint: endPoint
);

model.AddXmiLine3d(line);
```

## Related Classes

- **XmiPoint3d** - 3D coordinate points
- **XmiHasLine3D** - Relationship linking entities to lines
