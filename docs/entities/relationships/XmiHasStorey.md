# XmiHasStorey

Connects storeys (building levels) to structural entities.

## Purpose

`XmiHasStorey` is a relationship that associates building storey levels with elements.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Source` | `XmiBaseEntity` | Yes | Entity belonging to the storey |
| `Target` | `XmiStorey` | Yes | Storey entity |

## Usage Example

```csharp
var slab = new XmiSlab(/* ... */);
var storey2 = new XmiStorey(/* ... */);

var hasStorey = new XmiHasStorey(
    id: "rel-1",
    source: slab,
    target: storey2
);

model.Relationships.Add(hasStorey);
```

## Related Classes

- **XmiBaseEntity** - Base class for all entities
- **XmiStorey** - Storey entity
- **XmiBaseRelationship** - Base relationship class
