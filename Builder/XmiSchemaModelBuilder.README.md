# XmiSchemaModelBuilder

Primary entry point for building XMI schema models with automatic relationship inference.

## Overview

`XmiSchemaModelBuilder` implements the builder pattern to construct complete XMI models. It manages the entire workflow from adding entities to building models with automatically inferred relationships and exporting to JSON format.

## Location

`Builder/XmiSchemaModelBuilder.cs`

## Key Features

- **Fluent API**: Add entities one at a time or in batches
- **Automatic Relationship Inference**: Analyzes entity properties to create relationships
- **Dependency Analysis**: Provides topological sorting and circular dependency detection
- **JSON Export**: Direct export to JSON files or strings
- **Manager Integration**: Uses EntityManager and RelationshipManager internally

## Methods

### Entity Management

#### AddEntity
```csharp
public void AddEntity(XmiBaseEntity entity)
```
Adds a single entity to the builder.

**Parameters:**
- `entity`: The entity to add to the model

**Example:**
```csharp
var builder = new XmiSchemaModelBuilder();
var member = new XmiStructuralCurveMember(...);
builder.AddEntity(member);
```

#### AddEntities
```csharp
public void AddEntities(IEnumerable<XmiBaseEntity> entities)
```
Adds multiple entities in a single call.

**Parameters:**
- `entities`: Collection of entities to add

**Example:**
```csharp
var members = new List<XmiStructuralCurveMember> { member1, member2, member3 };
builder.AddEntities(members);
```

### Model Building

#### BuildModel
```csharp
public XmiModel BuildModel()
```
Constructs the complete XMI model with automatic relationship inference.

**Returns:** `XmiModel` containing all entities and their inferred relationships

**Process:**
1. Creates XmiModel with all added entities
2. Uses ExtensionRelationshipExporter to infer relationships from entity properties
3. Populates model.Relationships with inferred relationships
4. Returns complete model

**Example:**
```csharp
var model = builder.BuildModel();
Console.WriteLine($"Entities: {model.Entities.Count}");
Console.WriteLine($"Relationships: {model.Relationships.Count}");
```

### Dependency Analysis

#### GetTopologicallySortedEntities
```csharp
public List<XmiBaseEntity> GetTopologicallySortedEntities()
```
Returns entities sorted in dependency order (dependencies first).

**Returns:** List of entities in topological order

**Use Cases:**
- Correct serialization order
- Import/export operations
- Dependency-aware processing

**Example:**
```csharp
var sortedEntities = builder.GetTopologicallySortedEntities();
// Materials, then CrossSections, then Members
```

#### GetCycles
```csharp
public List<List<XmiBaseEntity>> GetCycles()
```
Detects circular dependencies in entity relationships.

**Returns:** List of cycles, where each cycle is a list of entities forming a circular dependency

**Example:**
```csharp
var cycles = builder.GetCycles();
if (cycles.Any())
{
    Console.WriteLine("Warning: Circular dependencies detected!");
    foreach (var cycle in cycles)
    {
        Console.WriteLine($"Cycle: {string.Join(" -> ", cycle.Select(e => e.ID))}");
    }
}
```

### JSON Export

#### ExportJson
```csharp
public void ExportJson(string outputPath)
```
Builds the model and exports directly to a JSON file.

**Parameters:**
- `outputPath`: File path where JSON should be saved

**Example:**
```csharp
builder.ExportJson("output/model.json");
```

#### BuildJsonString
```csharp
public string BuildJsonString()
```
Builds the model and returns JSON as a string (without writing to disk).

**Returns:** JSON string representation of the model

**Example:**
```csharp
string json = builder.BuildJsonString();
Console.WriteLine(json);
// or send via HTTP, etc.
```

## Internal Architecture

### Manager Components

```csharp
private readonly EntityManager<XmiBaseEntity> _entityManager = new();
private readonly RelationshipManager<XmiBaseRelationship> _relationshipManager = new();
```

The builder uses managers internally to:
- Store and organize entities
- Store inferred relationships
- Provide CRUD operations

### Processing Pipeline

```
1. AddEntity/AddEntities
   ↓
2. BuildModel()
   ↓
3. ExtensionRelationshipExporter.ExportRelationships()
   ↓
4. XmiModel (complete with entities + relationships)
   ↓
5. ExportJson() or BuildJsonString()
   ↓
6. ExtensionNativeJsonBuilder
   ↓
7. JSON Output
```

## Complete Usage Example

```csharp
// Initialize builder
var builder = new XmiSchemaModelBuilder();

// Create entities
var material = new XmiStructuralMaterial(
    id: "MAT001",
    name: "Concrete C30",
    ...
);

var crossSection = new XmiStructuralCrossSection(
    id: "CS001",
    name: "300x600",
    material: material,
    ...
);

var point1 = new XmiStructuralPointConnection(...);
var point2 = new XmiStructuralPointConnection(...);

var member = new XmiStructuralCurveMember(
    id: "M001",
    name: "Beam-1",
    crossSection: crossSection,
    segments: new[] { new XmiSegment(point1, point2) },
    ...
);

// Add entities to builder
builder.AddEntities(new[] { material, crossSection, point1, point2, member });

// Check for circular dependencies
var cycles = builder.GetCycles();
if (cycles.Any())
{
    throw new InvalidOperationException("Model contains circular dependencies");
}

// Get topologically sorted entities
var sortedEntities = builder.GetTopologicallySortedEntities();
Console.WriteLine("Entity order:");
foreach (var entity in sortedEntities)
{
    Console.WriteLine($"  {entity.ID} ({entity.EntityType})");
}

// Build complete model
var model = builder.BuildModel();
Console.WriteLine($"Built model with {model.Entities.Count} entities and {model.Relationships.Count} relationships");

// Export to JSON
builder.ExportJson("output/structural_model.json");

// Or get JSON string
string jsonString = builder.BuildJsonString();
```

## Automatic Relationship Inference

The builder automatically creates relationships based on entity property references:

### Detected Relationships

| Entity Property | Inferred Relationship |
|----------------|----------------------|
| CurveMember.CrossSection | XmiHasStructuralCrossSection |
| CurveMember.Segments | XmiHasSegment (multiple) |
| CrossSection.Material | XmiHasStructuralMaterial |
| PointConnection.Point3D | XmiHasGeometry |
| Segment.StartNode, EndNode | XmiHasStructuralNode |
| Entity.Storey | XmiHasStructuralStorey |
| Arc3D/Line3D endpoints | XmiHasPoint3D |

**Note:** This is handled by `ExtensionRelationshipExporter` internally.

## Design Patterns

### Builder Pattern
- Fluent interface for constructing complex models
- Separates construction from representation
- Supports incremental entity addition

### Facade Pattern
- Simplifies interaction with multiple subsystems
- Hides complexity of managers and exporters
- Provides unified interface for model building

### Dependency Injection Ready
- Uses concrete managers internally
- Can be extended to accept manager interfaces via constructor

## Related Classes

- **XmiModel**: The output model structure
- **EntityManager<T>**: Manages entity storage
- **RelationshipManager<T>**: Manages relationship storage
- **ExtensionRelationshipExporter**: Infers relationships from entities
- **ExtensionDependencyAnalyzer**: Analyzes graph dependencies
- **ExtensionNativeJsonBuilder**: Exports model to JSON

## See Also

- [XmiSchemaJsonBuilder](XmiSchemaJsonBuilder.README.md) - Alternative builder (similar functionality)
- [XmiModel](../Models/Model/XmiModel.README.md) - Model structure
- [ExtensionRelationshipExporter](../Extensions/ExtensionRelationshipExporter.README.md) - Relationship inference logic
- [ExtensionNativeJsonBuilder](../Extensions/ExtensionNativeJsonBuilder.README.md) - JSON export logic
