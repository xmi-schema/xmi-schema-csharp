# XmiBaseEntity

Base class for all entities in the XMI schema model.

## Overview

`XmiBaseEntity` serves as the foundation for all entity types in the built environment data model. It provides common properties and functionality that are shared across all structural engineering entities, ensuring consistency throughout the system.

## Location

`Models/Bases/XmiBaseEntity.cs`

## Properties

### Core Identifiers

- **ID** (`string`): Unique identifier for the entity
  - Primary identifier used throughout the system
  - Must be unique within the model
  - Used for establishing relationships

- **Name** (`string`): Display name of the entity
  - Human-readable name
  - Defaults to ID if not provided

- **NativeId** (`string`): Original identifier from the source system
  - Preserves traceability to source data
  - Enables round-tripping with external systems

### Interoperability

- **IFCGUID** (`string`): IFC (Industry Foundation Classes) GUID
  - Used for interoperability with IFC-based systems and BIM applications
  - Can be empty if entity is not mapped to an IFC element

### Metadata

- **Description** (`string`): Textual description providing additional context
- **EntityType** (`string`): Type discriminator for serialization/deserialization
  - Defaults to class name if not provided
  - Used to identify concrete entity type in JSON output

## Constructor

```csharp
public XmiBaseEntity(
    string id,
    string name,
    string ifcguid,
    string nativeId,
    string description,
    string entityType
)
```

### Parameters

- **id**: Required unique identifier
- **name**: Optional display name (defaults to id)
- **ifcguid**: Optional IFC GUID for BIM interoperability
- **nativeId**: Optional source system identifier
- **description**: Optional textual description
- **entityType**: Optional type discriminator (defaults to "XmiBaseEntity")

## Inheritance Hierarchy

All concrete entities inherit from `XmiBaseEntity`:

- `XmiStructuralCurveMember` - Beam/column elements
- `XmiStructuralSurfaceMember` - Slab/wall elements
- `XmiStructuralPointConnection` - Node/connection points
- `XmiStructuralCrossSection` - Cross-section definitions
- `XmiStructuralMaterial` - Material definitions
- `XmiStructuralStorey` - Building level/storey
- `XmiSegment` - Member segments
- And more...

## Usage in the System

### Entity Creation

```csharp
// Direct instantiation (typically done in derived classes)
var entity = new XmiBaseEntity(
    id: "ENT001",
    name: "My Entity",
    ifcguid: "2Dj4jc$nD2hBU9kSEOLXyH",
    nativeId: "RevitId_12345",
    description: "A structural element",
    entityType: "XmiBaseEntity"
);
```

### Relationship References

Entities are referenced in relationships through their base type:

```csharp
public class XmiBaseRelationship
{
    public XmiBaseEntity Source { get; set; }  // Any entity type
    public XmiBaseEntity Target { get; set; }  // Any entity type
}
```

### JSON Export

During JSON export, entities are serialized with their EntityType as the key:

```json
{
  "XmiStructuralCurveMember": {
    "ID": "M001",
    "Name": "Beam-1",
    "IFCGUID": "2Dj4jc$nD2hBU9kSEOLXyH",
    "NativeId": "RevitId_12345",
    "Description": "Main support beam",
    "EntityType": "XmiStructuralCurveMember"
  }
}
```

## Design Patterns

### Type Discriminator Pattern

The `EntityType` property implements the type discriminator pattern, enabling:
- Polymorphic serialization/deserialization
- Runtime type identification in graph structures
- JSON schema validation

### Default Value Pattern

The constructor provides sensible defaults:
- `Name` defaults to `ID` if not provided
- `EntityType` defaults to class name
- Empty strings are acceptable for optional properties

## Related Classes

- **XmiBaseRelationship**: Connects entities in directed graph structure
- **XmiModel**: Container holding all entities and relationships
- **EntityManager<T>**: Manages CRUD operations for entities
- **ExtensionNativeJsonBuilder**: Serializes entities to JSON

## See Also

- [XmiBaseRelationship](XmiBaseRelationship.README.md) - Base class for relationships
- [XmiModel](../Model/XmiModel.README.md) - Root container for entities
- [IEntityManager](../../Interfaces/IEntityManager.README.md) - Entity management interface
