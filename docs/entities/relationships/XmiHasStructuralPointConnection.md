# XmiHasStructuralPointConnection

Links entities to analytical point connections.

## Purpose

`XmiHasStructuralPointConnection` is a relationship that associates elements with analytical node connections.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Source` | `XmiBaseEntity` | Yes | Entity using the point connection |
| `Target` | `XmiStructuralPointConnection` | Yes | Point connection entity |

## Usage Example

```csharp
var member = new XmiStructuralCurveMember(/* ... */);
var node = new XmiStructuralPointConnection(/* ... */);

var hasConnection = new XmiHasStructuralPointConnection(
    id: "rel-1",
    source: member,
    target: node
);

model.Relationships.Add(hasConnection);
```

## Related Classes

- **XmiBaseEntity** - Base class for all entities
- **XmiStructuralPointConnection** - Point connection entity
- **XmiBaseRelationship** - Base relationship class
