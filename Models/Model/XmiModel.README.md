# XmiModel

The root container representing a complete Cross Model Information (XMI) graph model for built environment data.

## Overview

`XmiModel` is the central data structure in the XMI schema library. It represents an entire structural engineering model as a **directed graph**, where:

- **Nodes** = Entities (structural members, materials, geometry, etc.)
- **Edges** = Relationships (connections between entities)

This graph-based approach provides a flexible and powerful way to represent complex built environment data with explicit relationships, enabling:
- Advanced graph queries and traversals
- Dependency analysis and topological sorting
- Visual representation of structural hierarchies
- Interoperable JSON export format

The model serves as the output of the builder pattern (`XmiSchemaModelBuilder`) and the input to JSON export utilities (`ExtensionNativeJsonBuilder`).

## Properties

| Property | Type | Description |
|----------|------|-------------|
| `Entities` | `List<XmiBaseEntity>` | Collection of all entities (nodes) in the graph |
| `Relationships` | `List<XmiBaseRelationship>` | Collection of all relationships (edges) in the graph |

### Entities Collection

The `Entities` list contains all node types in the model:

**Structural Elements:**
- `XmiStructuralCurveMember` - Beams, columns, bracing
- `XmiStructuralSurfaceMember` - Slabs, walls, footings
- `XmiStructuralPointConnection` - Nodes, supports

**Properties & Definitions:**
- `XmiStructuralMaterial` - Material properties
- `XmiStructuralCrossSection` - Section properties
- `XmiStructuralStorey` - Building levels

**Geometry:**
- `XmiPoint3D` - 3D coordinates
- `XmiLine3D` - Linear geometry
- `XmiArc3D` - Curved geometry

**Components:**
- `XmiSegment` - Geometric segments
- `XmiStructuralUnit` - Unit definitions

### Relationships Collection

The `Relationships` list contains all directed edges:

- `XmiHasGeometry` - Links entities to their geometric representation
- `XmiHasStructuralMaterial` - Links sections/surfaces to materials
- `XmiHasStructuralCrossSection` - Links curve members to cross-sections
- `XmiHasStructuralNode` - Links members/segments to nodes
- `XmiHasSegment` - Links members to segments
- `XmiHasStructuralStorey` - Links entities to building levels

## Usage Example

### Creating a Model Manually

```csharp
using XmiSchema.Core.Models;
using XmiSchema.Core.Entities;
using XmiSchema.Core.Relationships;
using XmiSchema.Core.Enums;

// Create an empty model
var model = new XmiModel();

// Create entities
var material = new XmiStructuralMaterial(
    id: "MAT001",
    name: "Concrete C30",
    ifcguid: "",
    nativeId: "CONC-C30",
    description: "Normal weight concrete",
    materialType: XmiStructuralMaterialTypeEnum.Concrete,
    grade: 30.0,
    unitWeight: 2400.0,
    eModulus: 33000.0,
    gModulus: 13750.0,
    poissonRatio: 0.2,
    thermalCoefficient: 0.00001
);

var crossSection = new XmiStructuralCrossSection(
    id: "SEC001",
    name: "300x600",
    ifcguid: "",
    nativeId: "COL-SEC",
    description: "Column section",
    material: material,
    shape: XmiShapeEnum.Rectangular,
    parameters: new[] { "300", "600" },
    area: 0.18,  // m²
    secondMomentOfAreaXAxis: 0.0054,
    secondMomentOfAreaYAxis: 0.00135,
    radiusOfGyrationXAxis: 0.173,
    radiusOfGyrationYAxis: 0.087,
    elasticModulusXAxis: 0.018,
    elasticModulusYAxis: 0.009,
    plasticModulusXAxis: 0.027,
    plasticModulusYAxis: 0.0135,
    torsionalConstant: 0.00027
);

// Add entities to model
model.Entities.Add(material);
model.Entities.Add(crossSection);

// Create relationship manually
var relationship = new XmiHasStructuralMaterial(crossSection, material);
model.Relationships.Add(relationship);

Console.WriteLine($"Model contains {model.Entities.Count} entities");
Console.WriteLine($"Model contains {model.Relationships.Count} relationships");
```

### Creating a Model with Builder (Recommended)

```csharp
using XmiSchema.Core.Results;

// Use the builder pattern - much easier!
var builder = new XmiSchemaModelBuilder();

// Add entities
builder.AddEntity(material);
builder.AddEntity(crossSection);

// Build model - relationships inferred automatically!
var model = builder.BuildModel();

// Export to JSON
string json = builder.BuildJsonString();
```

### Querying the Model

```csharp
// Find all materials
var materials = model.Entities
    .OfType<XmiStructuralMaterial>()
    .ToList();

// Find all relationships where a specific entity is the source
var entityId = "SEC001";
var outgoingRelationships = model.Relationships
    .Where(r => r.Source.ID == entityId)
    .ToList();

// Find what material a cross-section uses
var sectionId = "SEC001";
var materialRelationship = model.Relationships
    .OfType<XmiHasStructuralMaterial>()
    .FirstOrDefault(r => r.Source.ID == sectionId);
var usedMaterial = materialRelationship?.Target as XmiStructuralMaterial;

// Count entities by type
var entityCounts = model.Entities
    .GroupBy(e => e.EntityType)
    .Select(g => new { Type = g.Key, Count = g.Count() })
    .ToList();
```

### Exporting to JSON

```csharp
using XmiSchema.Core.Handlers;

// Export using ExtensionNativeJsonBuilder
var jsonBuilder = new ExtensionNativeJsonBuilder(model);

// Get JSON string
string jsonString = jsonBuilder.BuildJson();

// Or save to file
jsonBuilder.Save("model.json");
```

## JSON Representation

The model exports to a graph JSON format with separate nodes and edges arrays:

```json
{
  "nodes": [
    {
      "XmiStructuralMaterial": {
        "ID": "MAT001",
        "Name": "Concrete C30",
        "EntityType": "XmiStructuralMaterial",
        "MaterialType": "Concrete",
        "Grade": 30.0,
        "UnitWeight": 2400.0,
        "EModulus": 33000.0,
        "GModulus": 13750.0,
        "PoissonRatio": 0.2,
        "ThermalCoefficient": 0.00001
      }
    },
    {
      "XmiStructuralCrossSection": {
        "ID": "SEC001",
        "Name": "300x600",
        "EntityType": "XmiStructuralCrossSection",
        "Shape": "Rectangular",
        "Area": 0.18
      }
    }
  ],
  "edges": [
    {
      "XmiHasStructuralMaterial": {
        "ID": "rel-001",
        "Source": "SEC001",
        "Target": "MAT001",
        "Name": "XmiHasStructuralMaterial",
        "EntityType": "XmiHasStructuralMaterial",
        "UmlType": "Composition"
      }
    }
  ]
}
```

## Advanced Features

### Topological Sorting

Get entities in dependency order using `ExtensionDependencyAnalyzer`:

```csharp
using XmiSchema.Core.Handlers;

var analyzer = new ExtensionDependencyAnalyzer(
    model.Entities,
    model.Relationships
);

// Get entities sorted by dependencies
// (Materials before cross-sections, cross-sections before members, etc.)
var sortedEntities = analyzer.GetTopologicallySortedEntities();
```

### Circular Dependency Detection

```csharp
var analyzer = new ExtensionDependencyAnalyzer(
    model.Entities,
    model.Relationships
);

// Detect cycles in the dependency graph
var cycles = analyzer.DetectCycles();

if (cycles.Any())
{
    Console.WriteLine("Warning: Circular dependencies detected!");
    foreach (var cycle in cycles)
    {
        var cycleIds = string.Join(" → ", cycle.Select(e => e.ID));
        Console.WriteLine($"Cycle: {cycleIds}");
    }
}
```

### Model Statistics

```csharp
// Get model statistics
Console.WriteLine($"Total entities: {model.Entities.Count}");
Console.WriteLine($"Total relationships: {model.Relationships.Count}");

// Count by entity type
var entityTypes = model.Entities
    .GroupBy(e => e.GetType().Name)
    .OrderByDescending(g => g.Count());

foreach (var group in entityTypes)
{
    Console.WriteLine($"  {group.Key}: {group.Count()}");
}

// Count by relationship type
var relationshipTypes = model.Relationships
    .GroupBy(r => r.GetType().Name)
    .OrderByDescending(g => g.Count());

foreach (var group in relationshipTypes)
{
    Console.WriteLine($"  {group.Key}: {group.Count()}");
}
```

## Related Classes

### Builder Pattern
- **`XmiSchemaModelBuilder`**: Fluent builder for creating models with automatic relationship inference

### Export/Serialization
- **`ExtensionNativeJsonBuilder`**: Exports model to graph JSON format
- **`ExtensionRelationshipExporter`**: Automatically infers and creates relationships

### Analysis
- **`ExtensionDependencyAnalyzer`**: Topological sorting and cycle detection using QuikGraph

### Managers
- **`EntityManager<T>`**: CRUD operations for entities
- **`RelationshipManager<T>`**: CRUD operations for relationships
- **`ModelManager`**: High-level model operations

### Entity Types
- **`XmiBaseEntity`**: Base class for all entities (nodes)
- **`XmiBaseRelationship`**: Base class for all relationships (edges)

## Design Patterns

### Graph Data Structure

The model implements a property graph pattern:
- **Nodes** have properties (entity attributes)
- **Edges** have properties (relationship metadata)
- Both nodes and edges have unique IDs
- Edges are directed (source → target)

This pattern is commonly used in:
- Graph databases (Neo4j, AWS Neptune)
- Data analysis platforms
- Visualization tools (D3.js, Cytoscape)

### Separation of Structure and Behavior

`XmiModel` is a pure data container with no behavior methods. This follows the **anemic domain model** pattern, where:
- Data structures (`XmiModel`) are simple DTOs
- Behavior is in separate classes (`XmiSchemaModelBuilder`, `ExtensionNativeJsonBuilder`)

Benefits:
- Easy serialization
- Clear separation of concerns
- Testable components

## Best Practices

### DO: Use the Builder Pattern

```csharp
// ✅ GOOD: Use builder for automatic relationship inference
var builder = new XmiSchemaModelBuilder();
builder.AddEntity(material);
builder.AddEntity(crossSection);
var model = builder.BuildModel();
```

### DON'T: Manually manage relationships

```csharp
// ❌ BAD: Manual relationship management is error-prone
var model = new XmiModel();
model.Entities.Add(material);
model.Entities.Add(crossSection);
// Easy to forget relationships or create invalid ones!
model.Relationships.Add(new XmiHasStructuralMaterial(...));
```

### DO: Validate Entity IDs

```csharp
// ✅ GOOD: Ensure unique IDs
var entityIds = new HashSet<string>();
foreach (var entity in model.Entities)
{
    if (!entityIds.Add(entity.ID))
    {
        throw new InvalidOperationException($"Duplicate entity ID: {entity.ID}");
    }
}
```

### DO: Check for Cycles

```csharp
// ✅ GOOD: Detect circular dependencies before processing
var analyzer = new ExtensionDependencyAnalyzer(model.Entities, model.Relationships);
var cycles = analyzer.DetectCycles();
if (cycles.Any())
{
    // Handle error
}
```

## Performance Considerations

### Memory Usage

- Each entity: ~200-500 bytes depending on type
- Each relationship: ~100-200 bytes
- Typical model (100 members): ~50KB
- Large model (10,000 members): ~5MB

### Lookup Performance

For efficient entity lookups, consider creating a dictionary:

```csharp
// One-time indexing for fast lookups
var entityIndex = model.Entities.ToDictionary(e => e.ID);

// O(1) lookup instead of O(n)
var entity = entityIndex["MAT001"];
```

### Relationship Queries

For complex relationship queries, consider creating an adjacency list:

```csharp
// Build adjacency list for fast traversal
var outgoing = model.Relationships
    .ToLookup(r => r.Source.ID, r => r);

// Fast query for all outgoing relationships
var relationships = outgoing["SEC001"].ToList();
```

## Engineering Notes

### Compatibility with Graph Databases

The XmiModel structure maps naturally to graph databases:

**Neo4j Cypher:**
```cypher
// Entities become nodes
CREATE (mat:XmiStructuralMaterial {ID: 'MAT001', Name: 'Concrete C30'})
CREATE (sec:XmiStructuralCrossSection {ID: 'SEC001', Name: '300x600'})

// Relationships become edges
CREATE (sec)-[:HAS_MATERIAL]->(mat)
```

**AWS Neptune/Gremlin:**
```groovy
// Add vertices (entities)
g.addV('XmiStructuralMaterial').property('ID', 'MAT001')

// Add edges (relationships)
g.V().has('ID', 'SEC001')
  .addE('HAS_MATERIAL')
  .to(g.V().has('ID', 'MAT001'))
```

### Extensibility

The model can be extended with:
- Custom entity types (inherit from `XmiBaseEntity`)
- Custom relationship types (inherit from `XmiBaseRelationship`)
- Additional collections (e.g., `LoadCases`, `AnalysisResults`)

## See Also

- [XmiSchemaModelBuilder.README.md](../../Builder/XmiSchemaModelBuilder.README.md) - Recommended way to create models
- [XmiBaseEntity.README.md](../Bases/XmiBaseEntity.README.md) - Entity base class
- [XmiBaseRelationship.README.md](../Bases/XmiBaseRelationship.README.md) - Relationship base class
- [ExtensionNativeJsonBuilder.README.md](../../Extensions/ExtensionNativeJsonBuilder.README.md) - JSON export
- [ExtensionDependencyAnalyzer.README.md](../../Extensions/ExtensionDependencyAnalyzer.README.md) - Graph analysis
