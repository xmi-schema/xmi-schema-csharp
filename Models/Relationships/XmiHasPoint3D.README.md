# XmiHasPoint3D

Relationship linking entities to 3D point geometry.

## Overview

`XmiHasPoint3D` represents a specific association between entities and `XmiPoint3D` geometry objects. This is a specialized form of the more general `XmiHasGeometry` relationship, specifically for point geometric primitives.

## Location

`Models/Relationships/XmiHasPoint3D.cs`

## Inheritance

```
XmiBaseRelationship → XmiHasPoint3D
```

## UML Type

**Association** - Represents a reference to point geometry.

## Constructors

### Full Constructor

```csharp
public XmiHasPoint3D(
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
public XmiHasPoint3D(
    XmiBaseEntity source,
    XmiBaseEntity target
)
```

## Usage Context

**Note**: This relationship type is currently **not automatically created** by `ExtensionRelationshipExporter`. The exporter uses the more general `XmiHasGeometry` relationship for all geometry types including `XmiPoint3D`.

This relationship can be created manually when you need to explicitly distinguish point geometry relationships from other geometry types.

## Manual Usage Example

```csharp
// Create 3D point
var point = new XmiPoint3D(
    id: "P001",
    name: "Point-1",
    coordinateX: 0.0,
    coordinateY: 0.0,
    coordinateZ: 3.5,
    ...
);

// Create point connection
var node = new XmiStructuralPointConnection(
    id: "NODE001",
    name: "Node-1",
    point: point,
    ...
);

// Manually create specific Point3D relationship
var relationship = new XmiHasPoint3D(node, point);

var relationshipManager = new RelationshipManager<XmiBaseRelationship>();
relationshipManager.AddRelationship(relationship);
```

## Comparison with XmiHasGeometry

### XmiHasGeometry (General)

Used by the system for all geometry types:

```csharp
pointConnection → XmiPoint3D  (via XmiHasGeometry)
segment → XmiLine3D           (via XmiHasGeometry)
segment → XmiArc3D            (via XmiHasGeometry)
```

### XmiHasPoint3D (Specific)

Can be used when you need type-specific handling:

```csharp
pointConnection → XmiPoint3D  (via XmiHasPoint3D - explicit)
arc3D → XmiPoint3D            (via XmiHasPoint3D - endpoints)
line3D → XmiPoint3D           (via XmiHasPoint3D - endpoints)
```

## Common Source Entities

### Point Connections
```csharp
XmiStructuralPointConnection → XmiPoint3D
```

### Geometry Endpoints
```csharp
XmiLine3D → XmiPoint3D (startPoint, endPoint)
XmiArc3D → XmiPoint3D (startPoint, endPoint, centrePoint)
```

## Graph Representations

### Node to Point
```
XmiStructuralPointConnection ──[XmiHasPoint3D]──> XmiPoint3D
```

### Line Endpoints
```
XmiLine3D ──[XmiHasPoint3D]──> XmiPoint3D (StartPoint)
          └─[XmiHasPoint3D]──> XmiPoint3D (EndPoint)
```

### Arc Points
```
XmiArc3D ──[XmiHasPoint3D]──> XmiPoint3D (StartPoint)
         ├─[XmiHasPoint3D]──> XmiPoint3D (EndPoint)
         └─[XmiHasPoint3D]──> XmiPoint3D (CentrePoint)
```

## JSON Export Example

```json
{
  "XmiHasPoint3D": {
    "ID": "rel-007",
    "Source": "NODE001",
    "Target": "P001",
    "Name": "XmiHasPoint3D",
    "Description": "",
    "EntityType": "XmiHasPoint3D",
    "UmlType": "Association"
  }
}
```

## When to Use

Use `XmiHasPoint3D` when you need to:

1. **Explicitly identify point geometry** in queries
2. **Point-specific processing** or validation
3. **Endpoint analysis** for lines and arcs
4. **Custom relationship handling** in extended applications

## Typical Pattern

For most use cases, the automatically-created `XmiHasGeometry` relationship is sufficient. Use `XmiHasPoint3D` only when you need explicit type differentiation:

```csharp
// Automatic (preferred for most cases)
var model = builder.BuildModel();
// Creates XmiHasGeometry for all geometry types

// Manual (for type-specific needs)
var pointRelationship = new XmiHasPoint3D(node, pointGeometry);
relationshipManager.AddRelationship(pointRelationship);
```

## Point-Specific Queries

If using `XmiHasPoint3D` explicitly:

```csharp
// Find all entities that reference a specific point
var pointId = "P001";

var referencingEntities = relationshipManager
    .FindByTarget(pointId)
    .Where(r => r is XmiHasPoint3D)
    .Select(r => r.Source);

Console.WriteLine($"Entities using point {pointId}:");
foreach (var entity in referencingEntities)
{
    Console.WriteLine($"  - {entity.ID}: {entity.EntityType}");
}

// Find all points used as line endpoints
var lineEndpoints = relationshipManager
    .GetAll()
    .Where(r => r is XmiHasPoint3D && r.Source is XmiLine3D)
    .Select(r => r.Target as XmiPoint3D);
```

## Related Classes

- **XmiBaseRelationship**: Base class
- **XmiPoint3D**: Target geometry (3D point)
- **XmiHasGeometry**: General geometry relationship
- **XmiStructuralPointConnection**: Common source entity
- **XmiLine3D**: Uses points for endpoints
- **XmiArc3D**: Uses points for endpoints and center

## See Also

- [XmiHasGeometry](XmiHasGeometry.README.md) - General geometry relationship
- [XmiPoint3D](../Geometries/XmiPoint3D.README.md) - Point geometry
- [XmiStructuralPointConnection](../Entities/XmiStructuralPointConnection.README.md) - Node entity
- [XmiLine3D](../Geometries/XmiLine3D.README.md) - Uses point endpoints
- [XmiArc3D](../Geometries/XmiArc3D.README.md) - Uses point endpoints
- [XmiBaseRelationship](../Bases/XmiBaseRelationship.README.md) - Base class
