# XmiHasLine3D

Links entities to line geometries in the XMI graph.

## Purpose

`XmiHasLine3D` is a relationship that associates straight line geometries with structural elements.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Source` | `XmiBaseEntity` | Yes | Entity using the line geometry |
| `Target` | `XmiLine3d` | Yes | Line geometry entity |

## Usage Example

```csharp
var beam = new XmiBeam(/* ... */);
var line = new XmiLine3d(/* ... */);

var hasLine3D = new XmiHasLine3D(
    id: "rel-1",
    source: beam,
    target: line
);

model.Relationships.Add(hasLine3D);
```

## Related Classes

- **XmiBaseEntity** - Base class for all entities
- **XmiLine3d** - Line geometry entity
- **XmiBaseRelationship** - Base relationship class
