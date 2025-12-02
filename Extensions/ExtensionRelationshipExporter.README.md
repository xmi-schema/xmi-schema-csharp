# ExtensionRelationshipExporter

Automatically infers and creates relationships between entities by analyzing their properties.

## Overview

`ExtensionRelationshipExporter` is the core engine for automatic relationship inference in XMI models. It traverses all entities in a model and examines their properties to identify references to other entities, then creates appropriate relationship objects to represent these connections in the graph structure.

## Location

`Extensions/ExtensionRelationshipExporter.cs`

## Key Features

- **Automatic Inference**: No manual relationship creation required
- **Property Inspection**: Analyzes entity properties using reflection
- **Multiple Relationship Types**: Creates specific relationship types based on context
- **Dependency Graph Construction**: Builds directed graph of entity dependencies

## Constructor

```csharp
public ExtensionRelationshipExporter(IRelationshipManager<XmiBaseRelationship> relationshipManager)
```

**Parameters:**
- `relationshipManager`: Manager where inferred relationships will be stored

## Methods

### ExportRelationships

```csharp
public void ExportRelationships(XmiModel xmiModel)
```

Main method that analyzes all entities and creates relationships.

**Parameters:**
- `xmiModel`: The model containing entities to analyze

**Process:**
1. Iterates through all entities in the model
2. Type-checks each entity for known patterns
3. Examines entity properties for references to other entities
4. Creates appropriate relationship objects
5. Adds relationships to the relationship manager

## Inferred Relationships

The exporter automatically creates the following relationships:

### Structural Point Connections

| Source | Target Property | Relationship Type |
|--------|----------------|-------------------|
| XmiStructuralPointConnection | Point3D | XmiHasGeometry |
| XmiStructuralPointConnection | Storey | XmiHasStructuralStorey |

### Cross Sections

| Source | Target Property | Relationship Type |
|--------|----------------|-------------------|
| XmiStructuralCrossSection | Material | XmiHasStructuralMaterial |

### Curve Members (Beams/Columns)

| Source | Target Property | Relationship Type |
|--------|----------------|-------------------|
| XmiStructuralCurveMember | CrossSection | XmiHasStructuralCrossSection |
| XmiStructuralCurveMember | Storey | XmiHasStructuralStorey |
| XmiStructuralCurveMember | Segments (collection) | XmiHasSegment (multiple) |
| XmiStructuralCurveMember | Nodes (collection) | XmiHasStructuralNode (multiple) |
| XmiStructuralCurveMember | BeginNode | XmiHasStructuralNode |
| XmiStructuralCurveMember | EndNode | XmiHasStructuralNode |

### Surface Members (Slabs/Walls)

| Source | Target Property | Relationship Type |
|--------|----------------|-------------------|
| XmiStructuralSurfaceMember | Material | XmiHasStructuralMaterial |
| XmiStructuralSurfaceMember | Storey | XmiHasStructuralStorey |
| XmiStructuralSurfaceMember | Segments (collection) | XmiHasSegment (multiple) |
| XmiStructuralSurfaceMember | Nodes (collection) | XmiHasStructuralNode (multiple) |

### Segments

| Source | Target Property | Relationship Type |
|--------|----------------|-------------------|
| XmiSegment | Geometry | XmiHasGeometry |
| XmiSegment | BeginNode | XmiHasStructuralNode |
| XmiSegment | EndNode | XmiHasStructuralNode |

### 3D Geometry

| Source | Target Property | Relationship Type |
|--------|----------------|-------------------|
| XmiArc3D | StartPoint | XmiHasGeometry |
| XmiArc3D | EndPoint | XmiHasGeometry |
| XmiArc3D | CentrePoint | XmiHasGeometry |
| XmiLine3D | StartPoint3D | XmiHasGeometry |
| XmiLine3D | EndPoint3D | XmiHasGeometry |

## Code Example

The following code shows how the exporter identifies a relationship:

```csharp
// Check if entity is a CurveMember with a CrossSection
if (entity is XmiStructuralCurveMember curve && curve.CrossSection != null)
{
    // Create and add relationship
    _relationshipManager.AddRelationship(
        new XmiHasStructuralCrossSection(curve, curve.CrossSection)
    );
}
```

## Usage in Build Pipeline

The exporter is automatically invoked by `XmiSchemaModelBuilder`:

```csharp
public XmiModel BuildModel()
{
    var model = new XmiModel
    {
        Entities = _entityManager.GetAllEntities().ToList()
    };

    // Automatic relationship inference happens here
    var exporter = new ExtensionRelationshipExporter(_relationshipManager);
    exporter.ExportRelationships(model);

    model.Relationships = _relationshipManager.GetAll().ToList();
    return model;
}
```

## Complete Example

```csharp
// Create entities with references
var material = new XmiStructuralMaterial("MAT001", "Concrete C30", ...);

var crossSection = new XmiStructuralCrossSection(
    id: "CS001",
    name: "300x600",
    material: material,  // Reference to material
    ...
);

var point1 = new XmiStructuralPointConnection(...);
var point2 = new XmiStructuralPointConnection(...);
var segment = new XmiSegment(point1, point2);

var member = new XmiStructuralCurveMember(
    id: "M001",
    name: "Beam-1",
    crossSection: crossSection,  // Reference to cross-section
    segments: new[] { segment },  // Collection reference
    ...
);

// Create model and add entities
var model = new XmiModel
{
    Entities = new List<XmiBaseEntity> { material, crossSection, point1, point2, segment, member }
};

// Automatic relationship inference
var relationshipManager = new RelationshipManager<XmiBaseRelationship>();
var exporter = new ExtensionRelationshipExporter(relationshipManager);
exporter.ExportRelationships(model);

// Result: Relationships automatically created:
// 1. member → crossSection (XmiHasStructuralCrossSection)
// 2. member → segment (XmiHasSegment)
// 3. crossSection → material (XmiHasStructuralMaterial)
// 4. segment → point1 (XmiHasStructuralNode)
// 5. segment → point2 (XmiHasStructuralNode)
```

## Design Patterns

### Visitor Pattern
Iterates through entities and applies relationship inference logic based on entity type.

### Strategy Pattern
Different relationship creation strategies based on entity type and property configuration.

### Type Inspection Pattern
Uses C# pattern matching (`is` and type checks) to identify entity types and their properties.

## Performance Considerations

- **Single Pass**: Examines each entity only once
- **Null Checks**: Skips properties that are null
- **Linear Complexity**: O(n) where n is the number of entities
- **Collection Iteration**: Additional O(m) for collections like Segments and Nodes

## Extensibility

To support new entity types or relationships:

1. Add new entity type check in `ExportRelationships`
2. Examine relevant properties
3. Create and add appropriate relationship instance

Example:
```csharp
// Add support for new entity type
if (entity is MyNewEntityType newEntity && newEntity.RelatedEntity != null)
{
    _relationshipManager.AddRelationship(
        new MyNewRelationshipType(newEntity, newEntity.RelatedEntity)
    );
}
```

## Related Classes

- **XmiSchemaModelBuilder**: Uses this exporter to build complete models
- **RelationshipManager<T>**: Stores inferred relationships
- **XmiBaseRelationship**: Base class for all relationships
- **XmiModel**: Contains entities and relationships
- **ExtensionDependencyAnalyzer**: Analyzes the resulting relationship graph

## See Also

- [XmiSchemaModelBuilder](../Builder/XmiSchemaModelBuilder.README.md) - Uses this exporter
- [IRelationshipManager](../Interfaces/IRelationshipManager.README.md) - Relationship storage interface
- [XmiBaseRelationship](../Models/Bases/XmiBaseRelationship.README.md) - Base relationship class
