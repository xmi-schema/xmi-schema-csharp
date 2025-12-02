# EntityManager<T>

Generic manager for CRUD operations on entities with dictionary-based storage.

## Overview

`EntityManager<T>` provides type-safe management of XMI entities with efficient lookup by ID. It implements the `IEntityManager<T>` interface and uses a dictionary for O(1) entity retrieval.

## Location

`Managers/EntityManager.cs`

## Generic Type Parameter

- **T**: Must inherit from `XmiBaseEntity`
  - Enables type-safe operations
  - Allows specialized managers for specific entity types

## Key Features

- **Type-Safe**: Generic parameter ensures compile-time type checking
- **Efficient Lookup**: Dictionary-based storage with O(1) ID lookup
- **CRUD Operations**: Create (Add), Read (Get), Update (implicit via Add), Delete (Remove)
- **Query Methods**: Find entities by name
- **Null Safety**: Handles null and invalid inputs gracefully

## Internal Storage

```csharp
private readonly Dictionary<string, T> _entities = new();
```

Entities are stored in a dictionary keyed by their ID property.

## Methods

### AddEntity

```csharp
public bool AddEntity(T entity)
```

Adds or updates an entity in the manager.

**Parameters:**
- `entity`: The entity to add

**Returns:**
- `true` if entity was added successfully
- `false` if entity ID is null or whitespace

**Behavior:**
- If entity ID already exists, it is overwritten (upsert behavior)
- Empty or null IDs are rejected

**Example:**
```csharp
var manager = new EntityManager<XmiStructuralCurveMember>();

var member = new XmiStructuralCurveMember("M001", "Beam-1", ...);
bool added = manager.AddEntity(member);  // true

// Update existing entity (same ID)
member.Name = "Updated Beam-1";
manager.AddEntity(member);  // Overwrites previous
```

### RemoveEntity

```csharp
public bool RemoveEntity(string id)
```

Removes an entity by ID.

**Parameters:**
- `id`: The ID of the entity to remove

**Returns:**
- `true` if entity was found and removed
- `false` if entity with given ID does not exist

**Example:**
```csharp
bool removed = manager.RemoveEntity("M001");  // true if exists
bool removed2 = manager.RemoveEntity("NONEXISTENT");  // false
```

### GetEntity

```csharp
public T? GetEntity(string id)
```

Retrieves a single entity by ID.

**Parameters:**
- `id`: The ID of the entity to retrieve

**Returns:**
- The entity if found
- `null` if not found

**Example:**
```csharp
var member = manager.GetEntity("M001");
if (member != null)
{
    Console.WriteLine($"Found member: {member.Name}");
}
else
{
    Console.WriteLine("Member not found");
}
```

### GetAllEntities

```csharp
public IEnumerable<T> GetAllEntities()
```

Retrieves all entities managed by this manager.

**Returns:** Collection of all entities

**Example:**
```csharp
var allMembers = manager.GetAllEntities();
Console.WriteLine($"Total members: {allMembers.Count()}");

foreach (var member in allMembers)
{
    Console.WriteLine($"  {member.ID}: {member.Name}");
}
```

### FindByName

```csharp
public IEnumerable<T> FindByName(string name)
```

Finds all entities with a specific name.

**Parameters:**
- `name`: The exact name to search for (case-sensitive)

**Returns:** Collection of entities with matching name

**Note:** Uses exact string equality, not partial matching

**Example:**
```csharp
var beams = manager.FindByName("Beam-1");
foreach (var beam in beams)
{
    Console.WriteLine($"Found beam with ID: {beam.ID}");
}
```

### Clear

```csharp
public void Clear()
```

Removes all entities from the manager.

**Example:**
```csharp
manager.Clear();
Console.WriteLine($"Entities remaining: {manager.GetAllEntities().Count()}");  // 0
```

## Usage Examples

### Basic CRUD Operations

```csharp
// Create manager for specific entity type
var memberManager = new EntityManager<XmiStructuralCurveMember>();

// Create entities
var beam1 = new XmiStructuralCurveMember("M001", "Beam-1", ...);
var beam2 = new XmiStructuralCurveMember("M002", "Beam-2", ...);
var column1 = new XmiStructuralCurveMember("M003", "Column-1", ...);

// Add entities
memberManager.AddEntity(beam1);
memberManager.AddEntity(beam2);
memberManager.AddEntity(column1);

// Read entity
var retrievedBeam = memberManager.GetEntity("M001");
Console.WriteLine(retrievedBeam?.Name);  // "Beam-1"

// Update entity (via Add with same ID)
beam1.Name = "Updated Beam-1";
memberManager.AddEntity(beam1);

// Delete entity
memberManager.RemoveEntity("M002");

// Query entities
var allMembers = memberManager.GetAllEntities();
Console.WriteLine($"Total members: {allMembers.Count()}");  // 2
```

### Type-Specific Managers

```csharp
// Different managers for different entity types
var memberManager = new EntityManager<XmiStructuralCurveMember>();
var materialManager = new EntityManager<XmiStructuralMaterial>();
var crossSectionManager = new EntityManager<XmiStructuralCrossSection>();

// Type safety enforced at compile time
var member = new XmiStructuralCurveMember(...);
memberManager.AddEntity(member);  // OK

// materialManager.AddEntity(member);  // Compile error - type mismatch
```

### Finding Entities by Name

```csharp
// Add multiple entities with same name
var beam1 = new XmiStructuralCurveMember("M001", "Standard Beam", ...);
var beam2 = new XmiStructuralCurveMember("M002", "Standard Beam", ...);

manager.AddEntity(beam1);
manager.AddEntity(beam2);

// Find all with same name
var standardBeams = manager.FindByName("Standard Beam");
Console.WriteLine($"Found {standardBeams.Count()} standard beams");
```

### Polymorphic Usage with Base Type

```csharp
// Manager for any entity type
var entityManager = new EntityManager<XmiBaseEntity>();

// Can store any entity type
entityManager.AddEntity(new XmiStructuralCurveMember(...));
entityManager.AddEntity(new XmiStructuralMaterial(...));
entityManager.AddEntity(new XmiStructuralCrossSection(...));

// All stored as XmiBaseEntity
var allEntities = entityManager.GetAllEntities();
foreach (var entity in allEntities)
{
    Console.WriteLine($"{entity.EntityType}: {entity.ID}");
}
```

## Integration with Builder

Used internally by `XmiSchemaModelBuilder`:

```csharp
public class XmiSchemaModelBuilder
{
    private readonly EntityManager<XmiBaseEntity> _entityManager = new();

    public void AddEntity(XmiBaseEntity entity)
    {
        _entityManager.AddEntity(entity);
    }

    public XmiModel BuildModel()
    {
        var model = new XmiModel
        {
            Entities = _entityManager.GetAllEntities().ToList()
        };
        // ...
    }
}
```

## Performance Characteristics

| Operation | Time Complexity | Notes |
|-----------|----------------|-------|
| AddEntity | O(1) | Dictionary insertion/update |
| RemoveEntity | O(1) | Dictionary removal |
| GetEntity | O(1) | Dictionary lookup |
| GetAllEntities | O(n) | Returns all values |
| FindByName | O(n) | Linear search through values |
| Clear | O(n) | Dictionary clear |

## Design Patterns

### Generic Repository Pattern
Provides type-safe data access layer for entities.

### Dictionary-Based Storage
Uses dictionary for efficient key-based lookups.

### Interface Implementation
Implements `IEntityManager<T>` for dependency injection and testing.

## Related Classes

- **IEntityManager<T>**: Interface implemented by this class
- **XmiBaseEntity**: Base type constraint for T
- **XmiSchemaModelBuilder**: Uses EntityManager internally
- **ModelManager**: Higher-level model management

## See Also

- [IEntityManager](../Interfaces/IEntityManager.README.md) - Interface definition
- [RelationshipManager](RelationshipManager.README.md) - Manages relationships
- [ModelManager](ModelManager.README.md) - High-level model operations
- [XmiBaseEntity](../Models/Bases/XmiBaseEntity.README.md) - Base entity class
