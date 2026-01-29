# XmiHasMaterial

Links entities to the structural material definition applied to them.

## Purpose

`XmiHasMaterial` is a relationship that associates material properties with structural elements.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Source` | `XmiBaseEntity` | Yes | Entity consuming the material |
| `Target` | `XmiMaterial` | Yes | Material entity |

## Usage Example

```csharp
var beam = new XmiBeam(/* ... */);
var concrete = new XmiMaterial(/* ... */);

var hasMaterial = new XmiHasMaterial(
    id: "rel-1",
    source: beam,
    target: concrete
);

model.Relationships.Add(hasMaterial);
```

## Related Classes

- **XmiBaseEntity** - Base class for all entities
- **XmiMaterial** - Material entity
- **XmiBaseRelationship** - Base relationship class
