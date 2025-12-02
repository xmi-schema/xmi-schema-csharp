# ModelManager

High-level manager for XMI model operations with direct list-based storage.

## Overview

`ModelManager` provides a simplified interface for managing entities and relationships within an `XmiModel`. Unlike `EntityManager` and `RelationshipManager` which use dictionary-based storage, `ModelManager` operates directly on the model's entity and relationship lists.

## Location

`Managers/ModelManager.cs`

## Key Features

- **Direct Model Access**: Works directly with `XmiModel` lists
- **Simple Interface**: Minimal API for basic operations
- **List-Based Storage**: No dictionary overhead
- **Interface Implementation**: Implements `IModelManager`

## Constructor

```csharp
public ModelManager(XmiModel model)
```

**Parameters:**
- `model`: The XMI model to manage

Creates a manager instance that operates on the provided model.

## Methods

### AddEntity

```csharp
public void AddEntity(XmiBaseEntity entity)
```

Adds an entity to the model's entity list.

**Parameters:**
- `entity`: The entity to add

**Behavior:**
- Null entities are silently ignored
- No duplicate checking - same entity can be added multiple times
- No dictionary lookup - direct list append

**Example:**
```csharp
var model = new XmiModel();
var manager = new ModelManager(model);

var member = new XmiStructuralCurveMember("M001", ...);
manager.AddEntity(member);

Console.WriteLine($"Entities: {model.Entities.Count}");  // 1
```

### CreateRelationship

```csharp
public void CreateRelationship(XmiBaseRelationship relationship)
```

Adds a relationship to the model's relationship list.

**Parameters:**
- `relationship`: The relationship to add

**Behavior:**
- Null relationships are silently ignored
- No duplicate checking
- No validation of source/target existence
- Direct list append

**Example:**
```csharp
var crossSection = new XmiStructuralCrossSection(...);
var material = new XmiStructuralMaterial(...);

manager.AddEntity(crossSection);
manager.AddEntity(material);

var relationship = new XmiHasStructuralMaterial(crossSection, material);
manager.CreateRelationship(relationship);

Console.WriteLine($"Relationships: {model.Relationships.Count}");  // 1
```

### GetAllEntities

```csharp
public List<XmiBaseEntity> GetAllEntities()
```

Returns the model's entity list.

**Returns:** Direct reference to the model's entity list

**Example:**
```csharp
var entities = manager.GetAllEntities();
foreach (var entity in entities)
{
    Console.WriteLine($"{entity.EntityType}: {entity.ID}");
}
```

### GetAllRelationships

```csharp
public List<XmiBaseRelationship> GetAllRelationships()
```

Returns the model's relationship list.

**Returns:** Direct reference to the model's relationship list

**Example:**
```csharp
var relationships = manager.GetAllRelationships();
foreach (var rel in relationships)
{
    Console.WriteLine($"{rel.Source.ID} → {rel.Target.ID}");
}
```

## Usage Examples

### Basic Model Management

```csharp
// Create model and manager
var model = new XmiModel();
var manager = new ModelManager(model);

// Add entities
var material = new XmiStructuralMaterial("MAT001", "Concrete C30", ...);
var crossSection = new XmiStructuralCrossSection("CS001", "300x600", ...);
var member = new XmiStructuralCurveMember("M001", "Beam-1", ...);

manager.AddEntity(material);
manager.AddEntity(crossSection);
manager.AddEntity(member);

// Create relationships
manager.CreateRelationship(new XmiHasStructuralMaterial(crossSection, material));
manager.CreateRelationship(new XmiHasStructuralCrossSection(member, crossSection));

// Access data
Console.WriteLine($"Model contains {manager.GetAllEntities().Count} entities");
Console.WriteLine($"Model contains {manager.GetAllRelationships().Count} relationships");
```

### Manual Model Construction

```csharp
// Alternative to using XmiSchemaModelBuilder
var model = new XmiModel();
var manager = new ModelManager(model);

// Add all entities
var entities = CreateStructuralEntities();
foreach (var entity in entities)
{
    manager.AddEntity(entity);
}

// Manually create relationships (no automatic inference)
manager.CreateRelationship(new XmiHasGeometry(pointConnection, point3D));
manager.CreateRelationship(new XmiHasStructuralCrossSection(member, crossSection));
// ... etc.

// Export model
var jsonBuilder = new ExtensionNativeJsonBuilder(model);
jsonBuilder.Save("output/manual_model.json");
```

### Direct List Manipulation

```csharp
var manager = new ModelManager(model);

// Add entities
manager.AddEntity(entity1);
manager.AddEntity(entity2);

// Direct list access (returns reference, not copy)
var entities = manager.GetAllEntities();

// Can modify list directly
entities.Add(entity3);  // Modifies model.Entities directly
entities.RemoveAt(0);   // Modifies model.Entities directly

Console.WriteLine($"Model entities: {model.Entities.Count}");  // Reflects changes
```

## Comparison with Other Managers

### ModelManager vs EntityManager

| Feature | ModelManager | EntityManager |
|---------|-------------|---------------|
| Storage | List-based | Dictionary-based |
| Lookup by ID | O(n) | O(1) |
| Duplicate prevention | No | Yes (overwrites) |
| Query capabilities | None | FindByName |
| Use case | Simple models | Complex models with lookups |

### ModelManager vs XmiSchemaModelBuilder

| Feature | ModelManager | XmiSchemaModelBuilder |
|---------|-------------|----------------------|
| Relationship inference | No | Yes (automatic) |
| Dependency analysis | No | Yes |
| JSON export | No | Yes |
| Complexity | Low | High |
| Use case | Manual construction | Automatic construction |

## When to Use ModelManager

**Use ModelManager when:**
- Building simple models manually
- You don't need automatic relationship inference
- You don't need efficient ID-based lookups
- You want direct control over the model lists

**Use XmiSchemaModelBuilder when:**
- You want automatic relationship inference
- You need dependency analysis
- You want direct JSON export
- You're building complex models

**Use EntityManager/RelationshipManager when:**
- You need efficient lookups by ID
- You need query capabilities
- You're building custom tools or extensions

## Design Patterns

### Facade Pattern
Provides simplified interface to XmiModel's lists.

### Direct Access Pattern
Returns direct references to model lists, not copies.

### Minimal API Pattern
Deliberately simple API for straightforward use cases.

## Important Notes

### No Validation

ModelManager does not validate:
- Entity ID uniqueness
- Relationship source/target existence
- Null checks (beyond basic null rejection)

### Direct List References

`GetAllEntities()` and `GetAllRelationships()` return direct references to the model's lists, not copies:

```csharp
var entities = manager.GetAllEntities();
entities.Add(newEntity);  // Modifies model.Entities directly

// Both return the same reference
Assert.AreSame(model.Entities, manager.GetAllEntities());
```

### No Automatic Relationship Inference

Unlike `XmiSchemaModelBuilder`, ModelManager does not infer relationships:

```csharp
// Manual relationship creation required
var member = new XmiStructuralCurveMember(..., crossSection: cs);
manager.AddEntity(member);
// Relationship to crossSection is NOT automatically created

// Must manually create relationship
manager.CreateRelationship(new XmiHasStructuralCrossSection(member, cs));
```

## Related Classes

- **IModelManager**: Interface implemented by this class
- **XmiModel**: The model being managed
- **EntityManager<T>**: Dictionary-based entity management
- **RelationshipManager<T>**: Dictionary-based relationship management
- **XmiSchemaModelBuilder**: High-level builder with automatic inference

## See Also

- [IModelManager](../Interfaces/IModelManager.README.md) - Interface definition
- [XmiModel](../Models/Model/XmiModel.README.md) - Model structure
- [EntityManager](EntityManager.README.md) - Dictionary-based entity manager
- [RelationshipManager](RelationshipManager.README.md) - Dictionary-based relationship manager
- [XmiSchemaModelBuilder](../Builder/XmiSchemaModelBuilder.README.md) - Automatic model builder
