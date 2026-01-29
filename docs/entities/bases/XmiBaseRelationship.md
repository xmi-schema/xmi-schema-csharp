# XmiBaseRelationship

Base class for all relationships in the XmiSchema graph model.

## Purpose

`XmiBaseRelationship` represents graph edges that connect entities as nodes. Each relationship explicitly defines a typed connection between a source entity and a target entity, enabling rich semantic modeling of building systems.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `ID` | `string` | Yes | Unique identifier for relationship within model |
| `SourceId` | `string` | Yes | ID of the source entity (from node) |
| `TargetId` | `string` | Yes | ID of the target entity (to node) |
| `EntityName` | `string` | Yes | Type discriminator for JSON polymorphic serialization (e.g., "XmiHasMaterial") |
| `UMLStereotype` | `string` | No | UML stereotype for additional type information |

## Graph Architecture

### Nodes and Edges

XmiSchema implements a directed graph where:

- **Entities** are nodes (e.g., beams, materials, points)
- **Relationships** are directed edges connecting nodes

```
[Entity A] ---(XmiHasMaterial)---> [Entity B]
     ↑                                         ↓
     |                                         |
(XmiHasGeometry)                         (XmiHasStorey)
     |                                         ↓
[Entity C] <---(XmiHasCrossSection)---- [Entity D]
```

### Relationship Types

| Relationship | Source Entity | Target Entity | Semantics |
|-------------|---------------|---------------|-----------|
| `XmiHasMaterial` | Beam/Column/Slab | Material | "is made of" |
| `XmiHasCrossSection` | Beam/Column | CrossSection | "has profile of" |
| `XmiHasStorey` | Beam/Column | Storey | "is on level" |
| `XmiHasGeometry` | Beam/Column | Geometry | "has geometry of" |
| `XmiHasPoint3D` | Connection | Point | "located at" |
| `XmiHasLine3D` | Beam | Line | "has axis of" |

## Characteristics

### Explicit Connections

Relationships are first-class objects with their own identity:

- Each relationship has a unique `ID`
- Enables querying and manipulation of graph edges
- Supports multiple relationships between same entity pairs

### Type Safety

The `EntityName` discriminator enables:

- Polymorphic deserialization from JSON
- Type-safe casting to specific relationship types
- Runtime type checking of relationships

### UML Stereotypes

Optional `UMLStereotype` property supports:

- Additional classification of relationships
- UML modeling integration
- Domain-specific relationship subtyping

## Usage Example

```csharp
// Create entities
var beam = manager.CreateXmiBeam(...);
var material = manager.CreateXmiMaterial(...);

// Create relationship
var hasMaterial = new XmiHasMaterial(
    id: "rel-1",
    source: beam,
    target: material
);

// Add to model
model.Relationships.Add(hasMaterial);

// Query relationships
var materialRels = model.Relationships
    .OfType<XmiHasMaterial>()
    .Where(rel => rel.SourceId == beam.ID);
```

## Graph Traversal

### Outgoing Edges

Find all relationships from a source entity:

```csharp
var outgoingRels = model.Relationships
    .Where(rel => rel.SourceId == beam.ID);
```

### Incoming Edges

Find all relationships pointing to a target entity:

```csharp
var incomingRels = model.Relationships
    .Where(rel => rel.TargetId == material.ID);
```

### Specific Relationship Type

Find relationships of a specific type:

```csharp
var materialRels = model.Relationships
    .OfType<XmiHasMaterial>();
```

## Best Practices

1. **Use specific relationship classes** (`XmiHasMaterial`, etc.) instead of base class
2. **Set descriptive IDs** for relationships to aid debugging
3. **Use factory methods** when available for relationship creation
4. **Query efficiently** by relationship type instead of checking `EntityName`
5. **Maintain referential integrity** by validating target IDs exist
6. **Set UMLStereotype** for advanced UML modeling scenarios

## Relationship Categories

### Material & Properties
- `XmiHasMaterial` - Links to material properties
- `XmiHasCrossSection` - Links to cross-section profile

### Geometry
- `XmiHasGeometry` - Generic geometry association
- `XmiHasPoint3D` - Links to point coordinates
- `XmiHasLine3D` - Links to line geometry

### Structural
- `XmiHasStructuralCurveMember` - Physical to analytical connection
- `XmiHasStructuralPointConnection` - Member to node connection

### Hierarchical
- `XmiHasStorey` - Level/Storey association
- `XmiHasSegment` - Curve member to segments

## Related Classes

All relationship classes inherit from `XmiBaseRelationship`:

**Material & Property Relationships**
- **XmiHasMaterial**
- **XmiHasCrossSection**

**Geometry Relationships**
- **XmiHasGeometry**
- **XmiHasPoint3D**
- **XmiHasLine3D**

**Structural Relationships**
- **XmiHasStructuralCurveMember**
- **XmiHasStructuralPointConnection**

**Hierarchical Relationships**
- **XmiHasStorey**
- **XmiHasSegment**

## Related Enums

- **XmiBaseEntityDomainEnum** - Source and target entity domains

## Notes

- Relationships are directed edges (source → target)
- Multiple relationships can exist between same entity pair
- Each relationship is independent and can be modified/deleted
- Relationship integrity is not enforced by the library (caller must verify)
- Use `EntityName` for type-specific operations when working with generic collections
