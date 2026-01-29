# XmiHasCrossSection

Associates cross-section properties with structural entities.

## Purpose

`XmiHasCrossSection` is a relationship that links geometric and analytical section properties to structural members.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Source` | `XmiBaseEntity` | Yes | Entity using the cross-section |
| `Target` | `XmiCrossSection` | Yes | Cross-section entity |

## Usage Example

```csharp
var column = new XmiColumn(/* ... */);
var ipe300 = new XmiCrossSection(/* ... */);

var hasCrossSection = new XmiHasCrossSection(
    id: "rel-1",
    source: column,
    target: ipe300
);

model.Relationships.Add(hasCrossSection);
```

## Related Classes

- **XmiBaseEntity** - Base class for all entities
- **XmiCrossSection** - Cross-section entity
- **XmiBaseRelationship** - Base relationship class
