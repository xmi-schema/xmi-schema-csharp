# XmiHasLine3D

Relationship linking entities to linear 3D geometry.

## Overview

`XmiHasLine3D` represents a specific association between entities and `XmiLine3D` geometry objects. This is a specialized form of the more general `XmiHasGeometry` relationship, specifically for linear geometric primitives.

## Location

`Models/Relationships/XmiHasLine3D.cs`

## Inheritance

```
XmiBaseRelationship → XmiHasLine3D
```

## UML Type

**Association** - Represents a reference to linear geometry.

## Constructors

### Full Constructor

```csharp
public XmiHasLine3D(
    string id,
    XmiBaseEntity source,
    XmiBaseEntity target,
    string name,
    string description,
    string entityName,
    string umlType
)
```

### Simplified Constructor (Auto-generated ID)

```csharp
public XmiHasLine3D(
    XmiBaseEntity source,
    XmiBaseEntity target
)
```

## Usage Context

**Note**: This relationship type is currently **not automatically created** by `ExtensionRelationshipExporter`. The exporter uses the more general `XmiHasGeometry` relationship for all geometry types including `XmiLine3D`.

This relationship can be created manually when you need to explicitly distinguish linear geometry relationships from other geometry types.

## Manual Usage Example

```csharp
// Create line geometry
var line = new XmiLine3D(
    id: "LINE001",
    name: "Line-1",
    startPoint3D: new XmiPoint3D(..., 0.0, 0.0, 0.0, ...),
    endPoint3D: new XmiPoint3D(..., 6.0, 0.0, 0.0, ...),
    ...
);

// Create segment
var segment = new XmiSegment(
    id: "SEG001",
    name: "Segment-1",
    geometry: line,
    ...
);

// Manually create specific Line3D relationship
var relationship = new XmiHasLine3D(segment, line);

var relationshipManager = new RelationshipManager<XmiBaseRelationship>();
relationshipManager.AddRelationship(relationship);
```

## Comparison with XmiHasGeometry

### XmiHasGeometry (General)

Used by the system for all geometry types:

```csharp
segment → XmiLine3D   (via XmiHasGeometry)
segment → XmiArc3D    (via XmiHasGeometry)
segment → XmiPoint3D  (via XmiHasGeometry)
```

### XmiHasLine3D (Specific)

Can be used when you need type-specific handling:

```csharp
segment → XmiLine3D   (via XmiHasLine3D - explicit)
```

## Graph Representation

```
XmiSegment ──[XmiHasLine3D]──> XmiLine3D
```

## JSON Export Example

```json
{
  "XmiHasLine3D": {
    "ID": "rel-006",
    "Source": "SEG001",
    "Target": "LINE001",
    "Name": "XmiHasLine3D",
    "Description": "",
    "EntityType": "XmiHasLine3D",
    "UmlType": "Association"
  }
}
```

## When to Use

Use `XmiHasLine3D` when you need to:

1. **Explicitly identify linear geometry** in queries or analysis
2. **Type-specific processing** for linear elements
3. **Custom relationship handling** in extended applications

## Typical Pattern

For most use cases, the automatically-created `XmiHasGeometry` relationship is sufficient. Use `XmiHasLine3D` only when you need explicit type differentiation:

```csharp
// Automatic (preferred for most cases)
var model = builder.BuildModel();
// Creates XmiHasGeometry for all geometry types

// Manual (for type-specific needs)
var lineRelationship = new XmiHasLine3D(segment, lineGeometry);
relationshipManager.AddRelationship(lineRelationship);
```

## Related Classes

- **XmiBaseRelationship**: Base class
- **XmiLine3D**: Target geometry (linear)
- **XmiHasGeometry**: General geometry relationship
- **XmiSegment**: Common source entity

## See Also

- [XmiHasGeometry](XmiHasGeometry.README.md) - General geometry relationship
- [XmiLine3D](../Geometries/XmiLine3D.README.md) - Linear geometry
- [XmiSegment](../Entities/XmiSegment.README.md) - Segment entity
- [XmiBaseRelationship](../Bases/XmiBaseRelationship.README.md) - Base class
