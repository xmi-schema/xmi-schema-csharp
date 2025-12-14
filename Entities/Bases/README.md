# Entities.Bases Namespace

The `XmiSchema.Entities.Bases` namespace contains the foundational base classes that provide common functionality for all XMI entities, geometries, and relationships.

## Core Base Classes

### XmiBaseEntity
The root base class for all entities in the XMI schema. Provides:
- `ID` - Unique identifier
- `ifcGuid` - IFC GUID for interoperability
- `NativeId` - Native application identifier
- `Description` - Entity description
- `EntityName` - Polymorphic discriminator for JSON serialization
- `Domain` - Domain classification

### XmiBaseGeometry
Extends `XmiBaseEntity` for geometric primitives:
- [`XmiPoint3D`](XmiPoint3D.md) - 3D point with tolerance-aware equality
- [`XmiLine3D`](XmiLine3D.md) - Straight line between two points
- [`XmiArc3D`](XmiArc3D.md) - Circular arc with start, end, center, and radius

### XmiBaseRelationship
Base class for all graph edges connecting entities:
- Holds source/target entity references
- Manages UML stereotypes
- Provides relationship metadata
- Typically exposes two constructors: fully described and shorthand with auto-generated ID

### XmiBasePhysicalEntity
Intermediate base for physical building elements:
- [`XmiBeam`](../Physical/XmiBeam.md)
- [`XmiColumn`](../Physical/XmiColumn.md)
- [`XmiSlab`](../Physical/XmiSlab.md)
- [`XmiWall`](../Physical/XmiWall.md)

### XmiBaseStructuralAnalyticalEntity
Intermediate base for analytical structural elements:
- [`XmiStructuralCurveMember`](../StructuralAnalytical/XmiStructuralCurveMember.md)
- [`XmiStructuralSurfaceMember`](../StructuralAnalytical/XmiStructuralSurfaceMember.md)
- [`XmiStructuralPointConnection`](../StructuralAnalytical/XmiStructuralPointConnection.md)

### XmiBaseEnum
Provides enum serialization functionality:
- Uses `EnumValueAttribute` for string serialization
- Supports parsing via `ExtensionEnumHelper.FromEnumValue`

## Usage Patterns

### Creating Custom Entities
When extending the schema, inherit from the appropriate base class:

```csharp
public class XmiCustomEntity : XmiBaseEntity
{
    public XmiCustomEntity(string id, string name, string ifcGuid, string nativeId, string description)
        : base("XmiCustomEntity", id, name, ifcGuid, nativeId, description)
    {
    }
}
```

### EntityName Discriminator
All entity constructors must pass the entity type name to ensure correct JSON serialization:

```csharp
public XmiBeam(...) : base("XmiBeam", id, name, ifcGuid, nativeId, description, domain)
```

### Relationship Constructors
Provide two constructors for relationships:
```csharp
// Fully described
public XmiHasGeometry(string id, XmiBaseEntity source, XmiBaseEntity target, ...)

// Shorthand with auto-generated ID
public XmiHasGeometry(XmiBaseEntity source, XmiBaseEntity target, ...)
```

## Key Design Principles

1. **Polymorphic Serialization** - `EntityName` enables proper JSON deserialization
2. **Graph Architecture** - Relationships connect entities as edges in a graph
3. **IFC Interoperability** - `ifcGuid` enables integration with IFC models
4. **Type Safety** - Strong typing throughout the inheritance hierarchy
5. **Extensibility** - Clear inheritance patterns for custom entities

## Related Namespaces

- [`XmiSchema.Entities.Commons`](../Commons/README.md) - Common domain entities
- [`XmiSchema.Entities.Physical`](../Physical/README.md) - Physical building elements
- [`XmiSchema.Entities.StructuralAnalytical`](../StructuralAnalytical/README.md) - Analytical elements
- [`XmiSchema.Entities.Geometries`](../Geometries/README.md) - Geometric primitives
- [`XmiSchema.Entities.Relationships`](../Relationships/README.md) - Graph relationships
