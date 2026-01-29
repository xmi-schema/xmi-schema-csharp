# XmiHasSegment

Connects segments to structural entities.

## Purpose

`XmiHasSegment` is a relationship that associates segment definitions with curve members or other entities.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Source` | `XmiBaseEntity` | Yes | Entity using the segment |
| `Target` | `XmiSegment` | Yes | Segment entity |
| `Position` | `int` | Yes | Segment position along curve |

## Usage Example

```csharp
var member = new XmiStructuralCurveMember(/* ... */);
var segment = new XmiSegment(/* ... */);

var hasSegment = new XmiHasSegment(
    id: "rel-1",
    source: member,
    target: segment,
    position: 1  // First segment
);

model.Relationships.Add(hasSegment);
```

## Related Classes

- **XmiBaseEntity** - Base class for all entities
- **XmiSegment** - Segment entity
- **XmiBaseRelationship** - Base relationship class
