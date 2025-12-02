# IModelManager

High-level interface for managing complete Cross Model Information (XMI) graph models with coordinated entity and relationship operations.

## Overview

`IModelManager` provides a unified, simplified interface for managing both entities (graph nodes) and relationships (graph edges) within a complete XMI model. It serves as a higher-level abstraction that coordinates operations across the entire graph structure.

While `IEntityManager<T>` and `IRelationshipManager<T>` provide detailed, type-specific control over individual collections, `IModelManager` offers a more streamlined API for working with the complete model. This interface is ideal for scenarios where you need to manage the entire graph without worrying about type-specific operations.

### Key Characteristics

- **Non-Generic**: Works with base types (`XmiBaseEntity`, `XmiBaseRelationship`) rather than specific types
- **Unified Interface**: Combines entity and relationship management in one place
- **Simplified API**: Fewer methods with straightforward signatures
- **Heterogeneous Collections**: Manages all entity and relationship types together

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `AddEntity(XmiBaseEntity entity)` | `void` | Adds any entity type to the model |
| `CreateRelationship(XmiBaseRelationship relationship)` | `void` | Creates any relationship type in the model |
| `GetAllEntities()` | `List<XmiBaseEntity>` | Returns all entities in the model |
| `GetAllRelationships()` | `List<XmiBaseRelationship>` | Returns all relationships in the model |

### Method Comparison with Lower-Level Managers

| Feature | IModelManager | IEntityManager&lt;T&gt; / IRelationshipManager&lt;T&gt; |
|---------|---------------|----------------------------------------------------------|
| Type Safety | Base types only | Generic, type-specific |
| Return Types | `void` or `List` | `bool`, `IEnumerable`, nullable types |
| Error Handling | May throw exceptions | Returns `false` for failures |
| Use Case | Complete model operations | Detailed, type-specific operations |

## When to Use

### Use IModelManager When:

1. **Building Complete Models**: Adding multiple entity and relationship types together
2. **Simple Workflows**: You don't need fine-grained control over success/failure
3. **Rapid Prototyping**: Quick model assembly without type constraints
4. **Integration Points**: Implementing high-level model operations

### Use IEntityManager&lt;T&gt; / IRelationshipManager&lt;T&gt; When:

1. **Type-Specific Operations**: Managing a single entity or relationship type
2. **Error Handling Required**: Need to check success/failure of each operation
3. **Performance-Critical Code**: Direct access to optimized operations
4. **Complex Queries**: Using FindBySource, FindByTarget, FindByName, etc.

### Prefer XmiSchemaModelBuilder When:

For most use cases, prefer `XmiSchemaModelBuilder` over direct use of any manager interface:
- Fluent API for easy model construction
- Automatic relationship inference
- Built-in validation
- Topological sorting and cycle detection

## Implementing Classes

### ModelManager

The concrete implementation of `IModelManager`:

**Location**: `Managers/ModelManager.cs`

**Implementation Details**:
- Uses internal `EntityManager<XmiBaseEntity>` for entities
- Uses internal `RelationshipManager<XmiBaseRelationship>` for relationships
- Delegates operations to specialized managers

## Usage Examples

### Basic Model Management

```csharp
using XmiSchema.Core.Entities;
using XmiSchema.Core.Relationships;
using XmiSchema.Core.Interfaces;
using XmiSchema.Core.Managers;
using XmiSchema.Core.Enums;

// Create a model manager
IModelManager modelManager = new ModelManager();

// Create entities
var material = new XmiStructuralMaterial(
    id: "MAT001",
    name: "Concrete C30",
    ifcGuid: "",
    nativeId: "CONC-30",
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
    name: "300x600 Column",
    ifcGuid: "",
    nativeId: "COL-SEC",
    description: "Rectangular column section",
    material: material,
    shape: XmiShapeEnum.Rectangular,
    parameters: new[] { "300", "600" },
    area: 0.18,
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
modelManager.AddEntity(material);
modelManager.AddEntity(crossSection);

// Create relationship
var relationship = new XmiHasStructuralMaterial(crossSection, material);
modelManager.CreateRelationship(relationship);

// Retrieve model contents
var entities = modelManager.GetAllEntities();
var relationships = modelManager.GetAllRelationships();

Console.WriteLine($"Model contains {entities.Count} entities");
Console.WriteLine($"Model contains {relationships.Count} relationships");
```

### Building a Complete Structural Model

```csharp
using XmiSchema.Core.Geometries;

IModelManager modelManager = new ModelManager();

// Step 1: Add materials
var concrete = new XmiStructuralMaterial("MAT001", "Concrete C30", ...);
var steel = new XmiStructuralMaterial("MAT002", "Steel S355", ...);

modelManager.AddEntity(concrete);
modelManager.AddEntity(steel);

// Step 2: Add cross-sections
var columnSection = new XmiStructuralCrossSection("SEC001", "300x600", ...);
var beamSection = new XmiStructuralCrossSection("SEC002", "200x400", ...);

modelManager.AddEntity(columnSection);
modelManager.AddEntity(beamSection);

// Step 3: Add geometry
var point1 = new XmiPoint3D("PT001", "Base Point", "", "", "", 0, 0, 0);
var point2 = new XmiPoint3D("PT002", "Top Point", "", "", "", 0, 0, 3.5);

modelManager.AddEntity(point1);
modelManager.AddEntity(point2);

// Step 4: Add structural members
var column = new XmiStructuralCurveMember(
    id: "COL001",
    name: "Ground Floor Column",
    crossSection: columnSection,
    // ... other parameters
);

modelManager.AddEntity(column);

// Step 5: Create relationships
modelManager.CreateRelationship(new XmiHasStructuralMaterial(columnSection, concrete));
modelManager.CreateRelationship(new XmiHasStructuralCrossSection(column, columnSection));

// Verify model
Console.WriteLine($"Complete model: {modelManager.GetAllEntities().Count} entities, " +
                  $"{modelManager.GetAllRelationships().Count} relationships");
```

### Querying Model Contents

```csharp
// Get all entities
var allEntities = modelManager.GetAllEntities();

// Filter by type using LINQ
var materials = allEntities.OfType<XmiStructuralMaterial>().ToList();
var crossSections = allEntities.OfType<XmiStructuralCrossSection>().ToList();
var members = allEntities.OfType<XmiStructuralCurveMember>().ToList();
var geometry = allEntities.OfType<XmiPoint3D>().ToList();

Console.WriteLine("Model Statistics:");
Console.WriteLine($"  Materials: {materials.Count}");
Console.WriteLine($"  Cross-Sections: {crossSections.Count}");
Console.WriteLine($"  Members: {members.Count}");
Console.WriteLine($"  Geometry: {geometry.Count}");

// Get all relationships
var allRelationships = modelManager.GetAllRelationships();

// Filter relationships by type
var materialLinks = allRelationships.OfType<XmiHasStructuralMaterial>().ToList();
var sectionLinks = allRelationships.OfType<XmiHasStructuralCrossSection>().ToList();
var geometryLinks = allRelationships.OfType<XmiHasGeometry>().ToList();

Console.WriteLine("\nRelationship Statistics:");
Console.WriteLine($"  Material Links: {materialLinks.Count}");
Console.WriteLine($"  Section Links: {sectionLinks.Count}");
Console.WriteLine($"  Geometry Links: {geometryLinks.Count}");
```

### Converting to XmiModel

```csharp
using XmiSchema.Core.Models;

// After building with IModelManager
IModelManager modelManager = new ModelManager();

// ... add entities and relationships ...

// Convert to XmiModel for export
var xmiModel = new XmiModel
{
    Entities = modelManager.GetAllEntities(),
    Relationships = modelManager.GetAllRelationships()
};

// Export to JSON
var jsonBuilder = new ExtensionNativeJsonBuilder(xmiModel);
string json = jsonBuilder.BuildJson();

// Save to file
jsonBuilder.Save("structural_model.json");
```

### Comparison: ModelManager vs. XmiSchemaModelBuilder

```csharp
// Using IModelManager (explicit relationship management)
var modelManager = new ModelManager();
modelManager.AddEntity(material);
modelManager.AddEntity(crossSection);
modelManager.CreateRelationship(new XmiHasStructuralMaterial(crossSection, material));

var entities1 = modelManager.GetAllEntities();
var relationships1 = modelManager.GetAllRelationships();

// Using XmiSchemaModelBuilder (automatic relationship inference)
var builder = new XmiSchemaModelBuilder();
builder.AddEntity(material);
builder.AddEntity(crossSection);
// No explicit relationship creation needed!

var model = builder.BuildModel();
var entities2 = model.Entities;
var relationships2 = model.Relationships;

// Both approaches yield the same result
// Builder approach is recommended for most use cases
```

## Related Classes

### Related Interfaces
- **`IEntityManager<T>`**: Type-specific entity management with detailed operations
- **`IRelationshipManager<T>`**: Type-specific relationship management with graph queries

### Implementing Classes
- **`ModelManager`**: Concrete implementation of this interface

### Higher-Level Classes
- **`XmiSchemaModelBuilder`**: Fluent builder with automatic relationship inference (recommended)

### Data Structures
- **`XmiModel`**: The data container holding entities and relationships
- **`XmiBaseEntity`**: Base class for all entities
- **`XmiBaseRelationship`**: Base class for all relationships

### Export Classes
- **`ExtensionNativeJsonBuilder`**: Exports XmiModel to JSON format

## Design Patterns

### Facade Pattern

`IModelManager` implements the **Facade Pattern** by providing a simplified interface over the more complex subsystems:

```
┌─────────────────────┐
│   IModelManager     │ ◄──── Simplified facade
│   (Facade)          │
└──────────┬──────────┘
           │ delegates to
           │
    ┌──────┴─────────┐
    │                │
    ▼                ▼
┌─────────────┐  ┌──────────────────┐
│ EntityMgr<T>│  │RelationshipMgr<T>│ ◄──── Complex subsystems
└─────────────┘  └──────────────────┘
```

**Benefits**:
- **Simplified API**: Fewer methods to learn
- **Reduced Complexity**: Hides internal manager coordination
- **Easier Integration**: Single interface for model operations

### Coordinated Management

Unlike individual managers that operate independently, `IModelManager` coordinates operations:

- Ensures entities are available before relationships are created
- Provides unified access to both collections
- Simplifies common operations that span entities and relationships

### Base Type Pattern

Works with base types rather than generics:

```csharp
// Generic managers (type-specific)
IEntityManager<XmiStructuralMaterial> materialManager;
IRelationshipManager<XmiHasStructuralMaterial> materialRelManager;

// Model manager (base types, heterogeneous)
IModelManager modelManager;  // Handles ALL entity and relationship types
```

**Trade-offs**:
- **Pro**: Can handle any entity/relationship type
- **Pro**: Simpler method signatures
- **Con**: Less compile-time type safety
- **Con**: May require runtime type checking (casting)

## Engineering Notes

### Error Handling

Unlike `IEntityManager<T>` and `IRelationshipManager<T>` which return `bool`:

```csharp
// Lower-level managers return bool
bool success = entityManager.AddEntity(material);
if (!success)
{
    Console.WriteLine("Failed to add entity");
}

// IModelManager returns void (may throw exceptions)
try
{
    modelManager.AddEntity(material);
}
catch (Exception ex)
{
    Console.WriteLine($"Failed to add entity: {ex.Message}");
}
```

The exact exception behavior depends on the `ModelManager` implementation.

### Return Type Differences

| Interface | Get Method Return Type | Notes |
|-----------|------------------------|-------|
| IEntityManager&lt;T&gt; | `IEnumerable<T>` | Deferred execution, read-only |
| IRelationshipManager&lt;T&gt; | `IEnumerable<T>` | Deferred execution, read-only |
| IModelManager | `List<T>` | Materialized list, can be modified |

The `List` return type means you get a concrete collection you can modify without affecting the original model (implementation-dependent).

### Performance Considerations

- **GetAllEntities()** and **GetAllRelationships()** may create new lists (O(n) copy)
- For read-only iteration, lower-level managers with `IEnumerable` may be more efficient
- For bulk operations, the simplified API may reduce overall code complexity

### When NOT to Use

**Don't use IModelManager when**:

1. **You need fine-grained error handling**: Use `IEntityManager<T>` / `IRelationshipManager<T>` for `bool` return values
2. **You need graph traversal**: Use `IRelationshipManager<T>` for `FindBySource` / `FindByTarget`
3. **You need type-specific queries**: Use `IEntityManager<T>` for `FindByName` and typed operations
4. **You're building models**: Use `XmiSchemaModelBuilder` for automatic relationship inference

**IModelManager is best for**:
- Simple add/get operations
- Integration layers
- Rapid prototyping
- When you don't need the advanced features of lower-level managers

### Architectural Position

```
User Application Code
        │
        ▼
┌──────────────────────────────┐
│  XmiSchemaModelBuilder       │ ◄──── Recommended for most use cases
│  (Fluent builder + auto rel) │
└──────────────────────────────┘
        │ uses
        ▼
┌──────────────────────────────┐
│  IModelManager               │ ◄──── Unified, simplified interface
│  (High-level facade)         │
└──────────────────────────────┘
        │ delegates to
        ▼
┌──────────────────────────────┐
│  IEntityManager<T>           │ ◄──── Detailed, type-specific control
│  IRelationshipManager<T>     │
│  (Low-level managers)        │
└──────────────────────────────┘
        │ stores in
        ▼
┌──────────────────────────────┐
│  Dictionary<string, T>       │ ◄──── Storage layer
│  (Data structures)           │
└──────────────────────────────┘
```

### Best Practices

**DO**:
- Use IModelManager for simple model assembly
- Prefer `XmiSchemaModelBuilder` over direct IModelManager usage
- Filter results by type using LINQ when needed
- Handle exceptions appropriately

**DON'T**:
- Don't use IModelManager when you need graph traversal operations
- Don't use IModelManager when you need fine-grained error handling
- Don't assume `void` methods always succeed (they may throw)
- Don't modify returned lists if you expect changes to affect the model

### Migration Path

If you start with `IModelManager` and need more features:

```csharp
// Start simple
IModelManager modelManager = new ModelManager();
modelManager.AddEntity(entity);

// Need more control? Switch to typed managers
var entityManager = new EntityManager<XmiStructuralMaterial>();
bool success = entityManager.AddEntity(material);  // Can check success

// Need automatic relationships? Use builder
var builder = new XmiSchemaModelBuilder();
builder.AddEntity(material);
builder.AddEntity(crossSection);
var model = builder.BuildModel();  // Relationships inferred automatically
```

## See Also

- [IEntityManager.README.md](./IEntityManager.README.md) - Type-specific entity management
- [IRelationshipManager.README.md](./IRelationshipManager.README.md) - Type-specific relationship management
- [ModelManager.README.md](../Managers/ModelManager.README.md) - Concrete implementation
- [XmiSchemaModelBuilder.README.md](../Builder/XmiSchemaModelBuilder.README.md) - Recommended builder pattern
- [XmiModel.README.md](../Models/Model/XmiModel.README.md) - The data model container
