# XmiHasStructuralCurveMember

Links physical elements to analytical curve members.

## Purpose

`XmiHasStructuralCurveMember` is a relationship that associates physical representations with their analytical counterparts.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Source` | `XmiBaseEntity` | Yes | Physical element (beam, column) |
| `Target` | `XmiStructuralCurveMember` | Yes | Analytical member |

## Usage Example

```csharp
var physicalBeam = new XmiBeam(/* ... */);
var analyticalMember = new XmiStructuralCurveMember(/* ... */);

var hasMember = new XmiHasStructuralCurveMember(
    id: "rel-1",
    source: physicalBeam,
    target: analyticalMember
);

model.Relationships.Add(hasMember);
```

## Related Classes

- **XmiBaseEntity** - Base class for all entities
- **XmiBeam** / **XmiColumn** - Physical element
- **XmiStructuralCurveMember** - Analytical member
- **XmiBaseRelationship** - Base relationship class
