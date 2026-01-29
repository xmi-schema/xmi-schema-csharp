# XmiStructuralPointConnection

Represents a discrete analytical point that connects members, nodes, or storey definitions.

## Purpose

`XmiStructuralPointConnection` defines analytical nodes where structural members intersect or terminate.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Point` | `XmiPoint3d` | No | 3D coordinate of connection |
| `Storey` | `XmiStorey` | No | Storey containing this connection |

## Usage Example

```csharp
var node = new XmiStructuralPointConnection(
    id: "node-1",
    name: "Column Base Node N1",
    ifcGuid: "2g0X$0m55u9hX1w2a7x5f",
    nativeId: "NODE-N1",
    description: "Base of column C1 at foundation"
);

model.AddXmiStructuralPointConnection(node);

var hasGeometry = new XmiHasPoint3D(node, point3D);
var hasStorey = new XmiHasStorey(node, storey1);
```

## Related Classes

- **XmiPoint3d** - 3D coordinate geometry
- **XmiStructuralCurveMember** - 1D member connected to nodes
- **XmiStructuralSurfaceMember** - 2D member connected to nodes
- **XmiHasPoint3D** - Geometry relationship
- **XmiStorey** - Storey relationship
