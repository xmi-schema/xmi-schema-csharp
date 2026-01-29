# XmiHasGeometry

Links entities to geometric representations in the XMI graph.

## Purpose

`XmiHasGeometry` is a general relationship that associates geometric primitives with structural elements.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Source` | `XmiBaseEntity` | Yes | Entity using the geometry |
| `Target` | `XmiBaseGeometry` | Yes | Geometry entity (point, line, arc, or polygon) |

## Usage Example

```csharp
var slab = new XmiSlab(/* ... */);
var polygon = new XmiPolygon3d(/* ... */);

var hasGeometry = new XmiHasGeometry(
    id: "rel-1",
    source: slab,
    target: polygon
);

model.Relationships.Add(hasGeometry);
```

## Related Classes

- **XmiBaseEntity** - Base class for all entities
- **XmiBaseGeometry** - Base class for all geometries
- **XmiBaseRelationship** - Base relationship class
