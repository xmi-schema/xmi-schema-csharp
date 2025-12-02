# XmiBaseRelationship

Base class for all relationships in the XMI schema model.

## Overview

`XmiBaseRelationship` represents directed edges in the graph structure that connect entities. Relationships define connections between entities in the built environment data model, forming a directed graph that models complex dependencies and associations in structural engineering data.

## Location

`Models/Bases/XmiBaseRelationship.cs`

## Properties

### Graph Structure

- **Source** (`XmiBaseEntity`): The starting point of the directed relationship edge
  - Example: In "Member has CrossSection", the member is the source

- **Target** (`XmiBaseEntity`): The ending point of the directed relationship edge
  - Example: In "Member has CrossSection", the cross-section is the target

### Identifiers

- **ID** (`string`): Unique identifier for the relationship
  - Must be unique within the model
  - Auto-generated as GUID in simplified constructor

### Metadata

- **Name** (`string`): Display name of the relationship
  - Defaults to "Unnamed" if not provided
  - Human-readable description of relationship type

- **Description** (`string`): Textual description of the relationship
  - Provides additional context about the relationship's nature and purpose

- **EntityType** (`string`): Type discriminator for serialization/deserialization
  - Defaults to class name if not provided
  - Used to identify concrete relationship type in JSON output

- **UmlType** (`string`): UML relationship classification
  - Specifies semantic meaning using UML terminology
  - Common values: "Composition", "Association", "Aggregation"
  - Indicates lifecycle and ownership implications

## Constructors

### Full Constructor

```csharp
public XmiBaseRelationship(
    string id,
    XmiBaseEntity source,
    XmiBaseEntity target,
    string name,
    string description,
    string entityType,
    string umlType
)
```

Provides complete control over all relationship properties.

### Simplified Constructor

```csharp
public XmiBaseRelationship(
    XmiBaseEntity source,
    XmiBaseEntity target,
    string entityType,
    string umlType
)
```

- Auto-generates GUID for ID
- Uses entityType as the name
- Simplified for common usage patterns

## UML Relationship Types

### Composition
Strong ownership where the target's lifecycle depends on the source:
- **XmiHasGeometry**: Point owns its geometry
- **XmiHasSegment**: Member owns its segments

### Association
Loose reference without ownership implications:
- **XmiHasStructuralMaterial**: Cross-section references material
- **XmiHasStructuralStorey**: Entity belongs to a storey

### Aggregation
Moderate ownership where target can exist independently:
- **XmiHasStructuralCrossSection**: Member uses cross-section
- **XmiHasStructuralNode**: Member references nodes

## Inheritance Hierarchy

All concrete relationships inherit from `XmiBaseRelationship`:

- `XmiHasGeometry` - Entity to geometry connection
- `XmiHasPoint3D` - Geometry to point connection
- `XmiHasLine3D` - Geometry to line connection
- `XmiHasSegment` - Member to segment connection
- `XmiHasStructuralCrossSection` - Member to cross-section
- `XmiHasStructuralMaterial` - Cross-section to material
- `XmiHasStructuralNode` - Member to node connection
- `XmiHasStructuralStorey` - Entity to storey
- And more...

## Usage Examples

### Creating a Relationship

```csharp
var member = new XmiStructuralCurveMember(...);
var crossSection = new XmiStructuralCrossSection(...);

// Using simplified constructor
var relationship = new XmiBaseRelationship(
    source: member,
    target: crossSection,
    entityType: "XmiHasStructuralCrossSection",
    umlType: "Association"
);

// Using full constructor
var relationship2 = new XmiBaseRelationship(
    id: "REL001",
    source: member,
    target: crossSection,
    name: "Member-CrossSection Link",
    description: "Associates structural member with its cross-section",
    entityType: "XmiHasStructuralCrossSection",
    umlType: "Association"
);
```

### Automatic Relationship Creation

The `ExtensionRelationshipExporter` automatically creates relationships by analyzing entity properties:

```csharp
var builder = new XmiSchemaModelBuilder();
builder.AddEntity(member);
builder.AddEntity(crossSection);

// BuildModel() automatically infers and creates relationships
var model = builder.BuildModel();
// Relationships are automatically added to model.Relationships
```

## JSON Export

Relationships are exported as edges in the graph structure:

```json
{
  "edges": [
    {
      "XmiHasStructuralCrossSection": {
        "ID": "a7f3c8e2-9b41-4d5a-8e1c-6f2d9a3b5c7e",
        "Source": "M001",
        "Target": "CS001",
        "Name": "XmiHasStructuralCrossSection",
        "Description": "",
        "EntityType": "XmiHasStructuralCrossSection",
        "UmlType": "Association"
      }
    }
  ]
}
```

## Graph Representation

Relationships enable graph-based operations:

### Directed Graph
```
Source Entity ──[Relationship]──> Target Entity
```

### Dependency Analysis
- Topological sorting for serialization order
- Circular dependency detection
- Traversal algorithms for querying related entities

### Example Graph
```
StructuralCurveMember ──[HasCrossSection]──> StructuralCrossSection
                                                      │
                                                      └──[HasMaterial]──> StructuralMaterial
      │
      └──[HasGeometry]──> Line3D ──[HasPoint3D]──> Point3D
```

## Design Patterns

### Directed Edge Pattern
Each relationship is a directed edge from source to target, enabling:
- Dependency tracking
- Graph traversal
- Hierarchical modeling

### Type Discriminator Pattern
The `EntityType` property enables polymorphic handling:
- Runtime type identification
- JSON schema validation
- Relationship filtering by type

## Related Classes

- **XmiBaseEntity**: Entities connected by relationships
- **XmiModel**: Container holding all relationships
- **RelationshipManager<T>**: Manages CRUD operations for relationships
- **ExtensionRelationshipExporter**: Automatically infers and creates relationships
- **ExtensionDependencyAnalyzer**: Analyzes relationship graph for dependencies

## See Also

- [XmiBaseEntity](XmiBaseEntity.README.md) - Base class for entities
- [XmiModel](../Model/XmiModel.README.md) - Root container for relationships
- [IRelationshipManager](../../Interfaces/IRelationshipManager.README.md) - Relationship management interface
