# XmiHasArc3D

Links entities to arc geometries in the XMI graph.

## Purpose

`XmiHasArc3D` is a relationship that associates circular arc geometries with structural elements.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Source` | `XmiBaseEntity` | Yes | Entity using the arc geometry |
| `Target` | `XmiArc3d` | Yes | Arc geometry entity |

## Usage Example

```csharp
var element = new XmiWall(/* ... */);
var arc = new XmiArc3d(/* ... */);

var hasArc3D = new XmiHasArc3D(
    id: "rel-1",
    source: element,
    target: arc
);

model.Relationships.Add(hasArc3D);
```

## Related Classes

- **XmiBaseEntity** - Base class for all entities
- **XmiArc3d** - Arc geometry entity
- **XmiBaseRelationship** - Base relationship class
