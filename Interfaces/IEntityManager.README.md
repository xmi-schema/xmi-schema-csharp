# IEntityManager&lt;T&gt;

Generic interface defining CRUD operations for managing Cross Model Information (XMI) entities.

## Overview

`IEntityManager<T>` is the core interface for entity collection management in the XMI schema library. It defines a standard contract for Create, Read, Update, and Delete (CRUD) operations on entities representing structural elements, materials, geometry, and other built environment data.

The interface uses C# generics with a type constraint (`where T : XmiBaseEntity`) to ensure type safety while allowing management of any entity type in the XMI schema. This pattern enables strongly-typed collections while maintaining a consistent API across all entity types.

The manager pattern separates data structure concerns from business logic, making entity management operations testable, maintainable, and reusable across the codebase.

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `AddEntity(T entity)` | `bool` | Adds an entity to the collection. Returns `true` if successful, `false` if duplicate ID or null |
| `RemoveEntity(string id)` | `bool` | Removes an entity by ID. Returns `true` if found and removed, `false` otherwise |
| `GetEntity(string id)` | `T?` | Retrieves an entity by ID. Returns the entity or `null` if not found |
| `GetAllEntities()` | `IEnumerable<T>` | Returns all entities in the collection |
| `FindByName(string name)` | `IEnumerable<T>` | Finds entities by partial name match (case-insensitive) |
| `Clear()` | `void` | Removes all entities from the collection |

### Method Details

#### AddEntity(T entity)
- **Purpose**: Add a new entity to the managed collection
- **Validation**: Rejects null entities and duplicate IDs
- **Performance**: O(1) for dictionary-based implementations
- **Thread Safety**: Not guaranteed by interface (implementation-dependent)

#### RemoveEntity(string id)
- **Purpose**: Remove an entity from the collection
- **Side Effects**: Does NOT automatically remove related relationships
- **Return Behavior**: Silent failure (returns `false`) if ID not found

#### GetEntity(string id)
- **Purpose**: Direct lookup by unique identifier
- **Performance**: O(1) for optimized implementations
- **Nullability**: Returns `null` if not found (nullable reference type)

#### GetAllEntities()
- **Purpose**: Enumerate all managed entities
- **Return Type**: `IEnumerable<T>` for LINQ support and deferred execution
- **Warning**: Modifying collection during iteration may cause exceptions

#### FindByName(string name)
- **Purpose**: Search entities by name substring
- **Matching**: Case-insensitive partial match
- **Performance**: O(n) linear search

#### Clear()
- **Purpose**: Empty the entire collection
- **Warning**: Operation cannot be undone
- **Side Effects**: Does NOT clear relationships

## Implementing Classes

### EntityManager&lt;T&gt;

The primary implementation of `IEntityManager<T>` in the XMI schema library:

**Location**: `Managers/EntityManager.cs`

**Implementation Details**:
- Uses `Dictionary<string, T>` for O(1) lookups by ID
- Validates entities before adding (null checks, ID uniqueness)
- Thread-safe operations (implementation-specific)

**Usage**:
```csharp
IEntityManager<XmiStructuralMaterial> manager = new EntityManager<XmiStructuralMaterial>();
```

## Usage Examples

### Basic CRUD Operations

```csharp
using XmiSchema.Core.Entities;
using XmiSchema.Core.Interfaces;
using XmiSchema.Core.Managers;
using XmiSchema.Core.Enums;

// Create a manager for structural materials
IEntityManager<XmiStructuralMaterial> materialManager = new EntityManager<XmiStructuralMaterial>();

// Create entities
var concrete = new XmiStructuralMaterial(
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

var steel = new XmiStructuralMaterial(
    id: "MAT002",
    name: "Steel S355",
    ifcGuid: "",
    nativeId: "STEEL-355",
    description: "Structural steel",
    materialType: XmiStructuralMaterialTypeEnum.Steel,
    grade: 355.0,
    unitWeight: 7850.0,
    eModulus: 210000.0,
    gModulus: 81000.0,
    poissonRatio: 0.3,
    thermalCoefficient: 0.000012
);

// Add entities
bool added1 = materialManager.AddEntity(concrete);
bool added2 = materialManager.AddEntity(steel);

Console.WriteLine($"Concrete added: {added1}"); // true
Console.WriteLine($"Steel added: {added2}");    // true

// Try adding duplicate
bool duplicate = materialManager.AddEntity(concrete);
Console.WriteLine($"Duplicate added: {duplicate}"); // false

// Get by ID
var retrieved = materialManager.GetEntity("MAT001");
if (retrieved != null)
{
    Console.WriteLine($"Found: {retrieved.Name}"); // "Concrete C30"
}

// Get all entities
var allMaterials = materialManager.GetAllEntities();
Console.WriteLine($"Total materials: {allMaterials.Count()}"); // 2

// Find by name
var concretes = materialManager.FindByName("Concrete");
foreach (var material in concretes)
{
    Console.WriteLine($"  {material.ID}: {material.Name}");
}
// Output: MAT001: Concrete C30

// Remove entity
bool removed = materialManager.RemoveEntity("MAT001");
Console.WriteLine($"Removed: {removed}"); // true

// Verify removal
var afterRemoval = materialManager.GetAllEntities();
Console.WriteLine($"Remaining: {afterRemoval.Count()}"); // 1

// Clear all
materialManager.Clear();
Console.WriteLine($"After clear: {materialManager.GetAllEntities().Count()}"); // 0
```

### Managing Different Entity Types

```csharp
using XmiSchema.Core.Entities;
using XmiSchema.Core.Geometries;

// Manager for materials
IEntityManager<XmiStructuralMaterial> materialManager = new EntityManager<XmiStructuralMaterial>();

// Manager for cross-sections
IEntityManager<XmiStructuralCrossSection> sectionManager = new EntityManager<XmiStructuralCrossSection>();

// Manager for 3D points
IEntityManager<XmiPoint3D> pointManager = new EntityManager<XmiPoint3D>();

// Each manager handles a specific entity type
materialManager.AddEntity(concrete);
sectionManager.AddEntity(crossSection);
pointManager.AddEntity(point);

// Type safety enforced by generics
// materialManager.AddEntity(crossSection); // Compile error!
```

### Working with LINQ

```csharp
// Filter entities with LINQ
var allMaterials = materialManager.GetAllEntities();

// Find concrete materials
var concreteMaterials = allMaterials
    .Where(m => m.MaterialType == XmiStructuralMaterialTypeEnum.Concrete)
    .ToList();

// Find high-grade materials
var highGradeMaterials = allMaterials
    .Where(m => m.Grade > 40.0)
    .OrderBy(m => m.Grade)
    .ToList();

// Group by material type
var materialGroups = allMaterials
    .GroupBy(m => m.MaterialType)
    .Select(g => new
    {
        Type = g.Key,
        Count = g.Count(),
        Materials = g.ToList()
    });

foreach (var group in materialGroups)
{
    Console.WriteLine($"{group.Type}: {group.Count} materials");
}
```

### Integration with Model Builder

```csharp
using XmiSchema.Core.Results;

// The builder uses entity managers internally
var builder = new XmiSchemaModelBuilder();

// Add entities (uses EntityManager<XmiBaseEntity> internally)
builder.AddEntity(concrete);
builder.AddEntity(steel);

// Access entities if needed
var entities = builder.GetEntities(); // Returns all added entities

// Build model
var model = builder.BuildModel();

// The model contains the entities that were managed
Console.WriteLine($"Model has {model.Entities.Count} entities");
```

## Related Classes

### Base Classes
- **`XmiBaseEntity`**: Base class for all entities that can be managed by this interface

### Implementing Classes
- **`EntityManager<T>`**: Concrete implementation providing dictionary-based entity storage

### Related Interfaces
- **`IRelationshipManager<T>`**: Manages relationships between entities
- **`IModelManager`**: High-level model operations combining entities and relationships

### Entity Types (Examples)
All entity types can be managed with this interface:
- **`XmiStructuralMaterial`**: Material definitions
- **`XmiStructuralCrossSection`**: Section properties
- **`XmiStructuralCurveMember`**: Beams, columns, bracing
- **`XmiStructuralSurfaceMember`**: Slabs, walls, footings
- **`XmiPoint3D`**, **`XmiLine3D`**, **`XmiArc3D`**: Geometry entities
- And all other classes derived from `XmiBaseEntity`

### Builder Pattern
- **`XmiSchemaModelBuilder`**: Uses entity managers for model construction

## Design Patterns

### Generic Interface Pattern

The interface uses C# generics with constraints to provide type safety:

```csharp
public interface IEntityManager<T> where T : XmiBaseEntity
```

**Benefits**:
- **Type Safety**: Compile-time checking prevents adding wrong entity types
- **Code Reuse**: Single interface definition for all entity types
- **IntelliSense**: Better IDE support with specific type information
- **LINQ Support**: Strongly-typed queries without casting

### Manager Pattern

The manager pattern separates data storage from business logic:

```
┌─────────────────┐
│   Client Code   │
└────────┬────────┘
         │ uses
         ▼
┌─────────────────────┐
│ IEntityManager<T>   │ ◄─── Interface (contract)
└─────────────────────┘
         △
         │ implements
         │
┌─────────────────────┐
│ EntityManager<T>    │ ◄─── Implementation
└─────────────────────┘
         │ manages
         ▼
┌─────────────────────┐
│ Dictionary<ID, T>   │ ◄─── Storage
└─────────────────────┘
```

**Benefits**:
- **Separation of Concerns**: Storage logic separate from entity definitions
- **Testability**: Easy to mock for unit tests
- **Flexibility**: Can swap implementations without changing client code
- **Consistency**: Uniform API across all entity types

### Repository Pattern Similarity

This interface resembles the **Repository Pattern** from Domain-Driven Design (DDD):

- **Add/Remove**: Persistence operations
- **GetEntity/GetAllEntities**: Query operations
- **FindByName**: Query with criteria

However, it's simpler than a full repository:
- In-memory storage (no database)
- No Unit of Work pattern
- No change tracking
- No lazy loading

### CRUD Operations

Standard Create, Read, Update, Delete operations:

| Operation | Methods |
|-----------|---------|
| **Create** | `AddEntity()` |
| **Read** | `GetEntity()`, `GetAllEntities()`, `FindByName()` |
| **Update** | Remove + Add (no direct update method) |
| **Delete** | `RemoveEntity()`, `Clear()` |

Note: There's no explicit `UpdateEntity()` method. To update an entity:
1. Remove the old version
2. Add the updated version

Or modify the entity directly (entities are reference types).

## Engineering Notes

### Performance Characteristics

Assuming dictionary-based implementation:

| Operation | Time Complexity | Notes |
|-----------|----------------|-------|
| AddEntity | O(1) | Dictionary insert |
| RemoveEntity | O(1) | Dictionary removal |
| GetEntity | O(1) | Dictionary lookup |
| GetAllEntities | O(n) | Enumerate all values |
| FindByName | O(n) | Linear search |
| Clear | O(n) | Dictionary clear |

### Memory Usage

- Each entity: ~200-500 bytes depending on type
- Dictionary overhead: ~40 bytes per entry
- Total for 1000 entities: ~250-550 KB

### Thread Safety

The interface does **not guarantee** thread safety. Thread safety is implementation-dependent.

For concurrent access:
- Use `ConcurrentDictionary<TKey, TValue>` in the implementation
- Add locking mechanisms
- Or use immutable collections

### Null Handling

- **AddEntity**: Rejects `null` entities (returns `false`)
- **GetEntity**: Returns `null` if not found (C# 8.0+ nullable reference types: `T?`)
- **FindByName**: Returns empty collection for no matches (never `null`)
- **GetAllEntities**: Returns empty collection for empty manager (never `null`)

### Entity Identity

Entities are identified by their `ID` property (string). The manager:
- Uses ID for uniqueness checking
- Uses ID for lookup operations
- Does NOT allow duplicate IDs

IDs should be:
- Unique within the entity type
- Non-empty strings
- Immutable (don't change after creation)

### Relationship Management

**Important**: This interface manages **only entities**, not relationships.

When removing entities:
- Relationships referencing the entity are NOT automatically removed
- Use `IRelationshipManager<T>` to clean up relationships
- Or use `IModelManager` for coordinated operations

Example of proper cleanup:

```csharp
// Remove entity
entityManager.RemoveEntity("MAT001");

// Also remove relationships
var relationshipsToRemove = relationshipManager
    .FindBySourceId("MAT001")
    .Concat(relationshipManager.FindByTargetId("MAT001"));

foreach (var rel in relationshipsToRemove)
{
    relationshipManager.RemoveRelationship(rel.ID);
}
```

### Generic Type Constraint

The constraint `where T : XmiBaseEntity` means:
- Only types derived from `XmiBaseEntity` can be used
- Ensures all entities have `ID`, `Name`, `EntityType`, etc.
- Enables polymorphic storage in `XmiModel.Entities` (which is `List<XmiBaseEntity>`)

### Best Practices

**DO**:
- Use specific entity types for managers: `IEntityManager<XmiStructuralMaterial>`
- Check return values from `AddEntity()` and `RemoveEntity()`
- Check for `null` when using `GetEntity()`
- Use `GetAllEntities()` with LINQ for filtering

**DON'T**:
- Don't assume `AddEntity()` always succeeds (check return value)
- Don't modify entities during `GetAllEntities()` enumeration
- Don't rely on order of `GetAllEntities()` (dictionary order is not guaranteed)
- Don't forget to clean up relationships when removing entities

### Common Pitfalls

1. **Forgetting to check null**:
   ```csharp
   // BAD
   var material = manager.GetEntity("MAT999");
   Console.WriteLine(material.Name); // NullReferenceException!

   // GOOD
   var material = manager.GetEntity("MAT999");
   if (material != null)
   {
       Console.WriteLine(material.Name);
   }
   ```

2. **Not handling duplicate IDs**:
   ```csharp
   // BAD
   manager.AddEntity(material1);
   manager.AddEntity(material2); // Assumes success

   // GOOD
   if (manager.AddEntity(material1))
   {
       Console.WriteLine("Added successfully");
   }
   else
   {
       Console.WriteLine("Failed - duplicate ID or null");
   }
   ```

3. **Orphaned relationships**:
   ```csharp
   // BAD - relationships still reference removed entity
   entityManager.RemoveEntity("MAT001");

   // GOOD - clean up relationships first
   CleanupRelationships("MAT001");
   entityManager.RemoveEntity("MAT001");
   ```

## See Also

- [XmiBaseEntity.README.md](../Models/Bases/XmiBaseEntity.README.md) - Base entity class
- [EntityManager.README.md](../Managers/EntityManager.README.md) - Concrete implementation
- [IRelationshipManager.README.md](./IRelationshipManager.README.md) - Relationship management
- [IModelManager.README.md](./IModelManager.README.md) - High-level model operations
- [XmiSchemaModelBuilder.README.md](../Builder/XmiSchemaModelBuilder.README.md) - Model construction
