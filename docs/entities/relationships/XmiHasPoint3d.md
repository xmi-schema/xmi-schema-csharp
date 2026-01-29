# XmiHasPoint3D

Links entities to point geometries in the XMI graph.

## Purpose

`XmiHasPoint3D` is a relationship that associates 3D point coordinates with structural elements.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Source` | `XmiBaseEntity` | Yes | Entity using the point geometry |
| `Target` | `XmiPoint3d` | Yes | Point geometry entity |

## Usage Example

```csharp
var node = new XmiStructuralPointConnection(/* ... */);
var point = new XmiPoint3d(/* ... */);

var hasPoint3D = new XmiHasPoint3D(
    id: "rel-1",
    source: node,
    target: point
);

model.Relationships.Add(hasPoint3D);
```

## Related Classes

- **XmiBaseEntity** - Base class for all entities
- **XmiPoint3d** - Point geometry entity
- **XmiBaseRelationship** - Base relationship class
