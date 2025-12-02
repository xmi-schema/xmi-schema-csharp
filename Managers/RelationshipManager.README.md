# RelationshipManager<T>

Generic manager for CRUD operations on relationships with source/target querying.

## Overview

`RelationshipManager<T>` provides type-safe management of XMI relationships with efficient lookup and querying capabilities. It implements the `IRelationshipManager<T>` interface and supports querying relationships by source or target entity.

## Location

`Managers/RelationshipManager.cs`

## Generic Type Parameter

- **T**: Must inherit from `XmiBaseRelationship`
  - Enables type-safe operations
  - Allows specialized managers for specific relationship types

## Key Features

- **Type-Safe**: Generic parameter ensures compile-time type checking
- **Efficient Lookup**: Dictionary-based storage with O(1) ID lookup
- **CRUD Operations**: Create (Add), Read (Get), Update (implicit via Add), Delete (Remove)
- **Graph Queries**: Find relationships by source or target entity
- **Null Safety**: Handles null and invalid inputs gracefully

## Internal Storage

```csharp
private readonly Dictionary<string, T> _relationships = new();
```

Relationships are stored in a dictionary keyed by their ID property.

## Methods

### AddRelationship

```csharp
public bool AddRelationship(T relationship)
```

Adds or updates a relationship in the manager.

**Parameters:**
- `relationship`: The relationship to add

**Returns:**
- `true` if relationship was added successfully
- `false` if relationship is null or has null/whitespace ID

**Behavior:**
- If relationship ID already exists, it is overwritten (upsert behavior)
- Null relationships or empty IDs are rejected

**Example:**
```csharp
var manager = new RelationshipManager<XmiBaseRelationship>();

var relationship = new XmiHasStructuralCrossSection(member, crossSection);
bool added = manager.AddRelationship(relationship);  // true

// Update existing (same ID)
manager.AddRelationship(relationship);  // Overwrites
```

### RemoveRelationship

```csharp
public bool RemoveRelationship(string id)
```

Removes a relationship by ID.

**Parameters:**
- `id`: The ID of the relationship to remove

**Returns:**
- `true` if relationship was found and removed
- `false` if relationship with given ID does not exist

**Example:**
```csharp
bool removed = manager.RemoveRelationship("rel-001");  // true if exists
```

### GetRelationship

```csharp
public T? GetRelationship(string id)
```

Retrieves a single relationship by ID.

**Parameters:**
- `id`: The ID of the relationship to retrieve

**Returns:**
- The relationship if found
- `null` if not found

**Example:**
```csharp
var relationship = manager.GetRelationship("rel-001");
if (relationship != null)
{
    Console.WriteLine($"{relationship.Source.ID} → {relationship.Target.ID}");
}
```

### GetAll

```csharp
public IEnumerable<T> GetAll()
```

Retrieves all relationships managed by this manager.

**Returns:** Collection of all relationships

**Example:**
```csharp
var allRelationships = manager.GetAll();
Console.WriteLine($"Total relationships: {allRelationships.Count()}");

foreach (var rel in allRelationships)
{
    Console.WriteLine($"  {rel.Source.ID} → {rel.Target.ID} ({rel.EntityType})");
}
```

### FindBySource

```csharp
public IEnumerable<T> FindBySource(string sourceId)
```

Finds all relationships originating from a specific entity.

**Parameters:**
- `sourceId`: The ID of the source entity

**Returns:** Collection of relationships where `Source.ID == sourceId`

**Use Cases:**
- Find all dependencies of an entity
- Traverse graph from a starting entity
- Analyze what an entity references

**Example:**
```csharp
// Find all relationships from member M001
var memberRelationships = manager.FindBySource("M001");

foreach (var rel in memberRelationships)
{
    Console.WriteLine($"M001 has relationship to {rel.Target.ID}");
}

// Output:
// M001 has relationship to CS001 (cross-section)
// M001 has relationship to SEG001 (segment)
// M001 has relationship to SEG002 (segment)
```

### FindByTarget

```csharp
public IEnumerable<T> FindByTarget(string targetId)
```

Finds all relationships pointing to a specific entity.

**Parameters:**
- `targetId`: The ID of the target entity

**Returns:** Collection of relationships where `Target.ID == targetId`

**Use Cases:**
- Find all entities that reference a specific entity
- Analyze what depends on an entity
- Impact analysis for deletions

**Example:**
```csharp
// Find all relationships to cross-section CS001
var crossSectionUsages = manager.FindByTarget("CS001");

foreach (var rel in crossSectionUsages)
{
    Console.WriteLine($"{rel.Source.ID} uses CS001");
}

// Output:
// M001 uses CS001
// M002 uses CS001
// M003 uses CS001
```

### Clear

```csharp
public void Clear()
```

Removes all relationships from the manager.

**Example:**
```csharp
manager.Clear();
Console.WriteLine($"Relationships remaining: {manager.GetAll().Count()}");  // 0
```

## Usage Examples

### Basic CRUD Operations

```csharp
// Create manager
var manager = new RelationshipManager<XmiBaseRelationship>();

// Create entities
var member = new XmiStructuralCurveMember("M001", ...);
var crossSection = new XmiStructuralCrossSection("CS001", ...);

// Create relationship
var relationship = new XmiHasStructuralCrossSection(member, crossSection);

// Add relationship
manager.AddRelationship(relationship);

// Read relationship
var retrieved = manager.GetRelationship(relationship.ID);

// Delete relationship
manager.RemoveRelationship(relationship.ID);
```

### Graph Traversal

```csharp
// Find all entities that member M001 depends on
var dependencies = manager.FindBySource("M001");
var dependencyIds = dependencies.Select(r => r.Target.ID).ToList();
Console.WriteLine($"M001 depends on: {string.Join(", ", dependencyIds)}");

// Find all entities that depend on cross-section CS001
var dependents = manager.FindByTarget("CS001");
var dependentIds = dependents.Select(r => r.Source.ID).ToList();
Console.WriteLine($"Entities using CS001: {string.Join(", ", dependentIds)}");
```

### Impact Analysis

```csharp
// Analyze impact of deleting cross-section CS001
var impactedEntities = manager.FindByTarget("CS001");

if (impactedEntities.Any())
{
    Console.WriteLine($"Cannot delete CS001 - used by {impactedEntities.Count()} entities:");
    foreach (var rel in impactedEntities)
    {
        Console.WriteLine($"  - {rel.Source.ID} ({rel.Source.Name})");
    }
}
else
{
    Console.WriteLine("Safe to delete CS001 - no dependencies");
}
```

### Type-Specific Managers

```csharp
// Manager for specific relationship type
var geometryManager = new RelationshipManager<XmiHasGeometry>();

var point = new XmiStructuralPointConnection(...);
var geometry = new XmiPoint3D(...);

var geometryRel = new XmiHasGeometry(point, geometry);
geometryManager.AddRelationship(geometryRel);  // Type-safe

// Compile error - type mismatch
// geometryManager.AddRelationship(new XmiHasStructuralMaterial(...));
```

### Relationship Statistics

```csharp
var allRels = manager.GetAll().ToList();

// Count relationships by type
var relsByType = allRels
    .GroupBy(r => r.EntityType)
    .Select(g => new { Type = g.Key, Count = g.Count() });

foreach (var stat in relsByType)
{
    Console.WriteLine($"{stat.Type}: {stat.Count} relationships");
}

// Find entities with most outgoing relationships
var mostDependencies = allRels
    .GroupBy(r => r.Source.ID)
    .OrderByDescending(g => g.Count())
    .First();

Console.WriteLine($"Entity {mostDependencies.Key} has {mostDependencies.Count()} dependencies");
```

## Integration with Builder

Used internally by `XmiSchemaModelBuilder`:

```csharp
public class XmiSchemaModelBuilder
{
    private readonly RelationshipManager<XmiBaseRelationship> _relationshipManager = new();

    public XmiModel BuildModel()
    {
        var model = new XmiModel
        {
            Entities = _entityManager.GetAllEntities().ToList()
        };

        // Relationship inference
        var exporter = new ExtensionRelationshipExporter(_relationshipManager);
        exporter.ExportRelationships(model);

        model.Relationships = _relationshipManager.GetAll().ToList();
        return model;
    }
}
```

## Performance Characteristics

| Operation | Time Complexity | Notes |
|-----------|----------------|-------|
| AddRelationship | O(1) | Dictionary insertion/update |
| RemoveRelationship | O(1) | Dictionary removal |
| GetRelationship | O(1) | Dictionary lookup |
| GetAll | O(n) | Returns all values |
| FindBySource | O(n) | Linear search through values |
| FindByTarget | O(n) | Linear search through values |
| Clear | O(n) | Dictionary clear |

**Note:** For high-performance source/target queries, consider maintaining additional indexes.

## Design Patterns

### Generic Repository Pattern
Provides type-safe data access layer for relationships.

### Dictionary-Based Storage
Uses dictionary for efficient key-based lookups.

### Interface Implementation
Implements `IRelationshipManager<T>` for dependency injection and testing.

## Related Classes

- **IRelationshipManager<T>**: Interface implemented by this class
- **XmiBaseRelationship**: Base type constraint for T
- **ExtensionRelationshipExporter**: Uses RelationshipManager to store inferred relationships
- **ModelManager**: Higher-level model management

## See Also

- [IRelationshipManager](../Interfaces/IRelationshipManager.README.md) - Interface definition
- [EntityManager](EntityManager.README.md) - Manages entities
- [ModelManager](ModelManager.README.md) - High-level model operations
- [XmiBaseRelationship](../Models/Bases/XmiBaseRelationship.README.md) - Base relationship class
- [ExtensionRelationshipExporter](../Extensions/ExtensionRelationshipExporter.README.md) - Relationship inference
