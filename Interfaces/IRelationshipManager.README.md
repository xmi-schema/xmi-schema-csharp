# IRelationshipManager&lt;T&gt;

Generic interface defining graph-aware CRUD operations for managing Cross Model Information (XMI) relationships.

## Overview

`IRelationshipManager<T>` is the core interface for relationship (edge) management in the XMI schema's directed graph model. It defines operations for managing connections between entities, enabling graph traversal, dependency analysis, and understanding structural connectivity in built environment data.

Unlike `IEntityManager<T>` which manages graph nodes, this interface provides specialized operations for managing directed edges:
- **FindBySource**: Find all outgoing relationships from an entity (what does this entity reference?)
- **FindByTarget**: Find all incoming relationships to an entity (what references this entity?)

These graph-specific operations enable dependency analysis, topological sorting, impact analysis, and graph visualizations essential for structural engineering workflows.

The interface uses C# generics with a type constraint (`where T : XmiBaseRelationship`) to ensure type safety while allowing management of any relationship type in the XMI schema.

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `AddRelationship(T relationship)` | `bool` | Adds a relationship (directed edge) to the collection |
| `RemoveRelationship(string id)` | `bool` | Removes a relationship by ID |
| `GetRelationship(string id)` | `T?` | Retrieves a relationship by ID |
| `GetAll()` | `IEnumerable<T>` | Returns all relationships in the collection |
| `FindBySource(string sourceId)` | `IEnumerable<T>` | Finds all outgoing relationships from an entity |
| `FindByTarget(string targetId)` | `IEnumerable<T>` | Finds all incoming relationships to an entity |
| `Clear()` | `void` | Removes all relationships from the collection |

### Graph Operations Details

#### FindBySource(string sourceId)
- **Purpose**: Graph traversal - find all edges leaving a node
- **Use Cases**:
  - "What material does this cross-section use?"
  - "What segments compose this member?"
  - "What nodes does this member connect to?"
- **Performance**: O(n) - scans all relationships
- **Direction**: Source → Target (forward traversal)

#### FindByTarget(string targetId)
- **Purpose**: Reverse graph traversal - find all edges entering a node
- **Use Cases**:
  - "What cross-sections use this material?" (impact analysis)
  - "What members are on this storey?"
  - "What elements reference this geometry?"
- **Performance**: O(n) - scans all relationships
- **Direction**: Source ← Target (backward traversal)

## Implementing Classes

### RelationshipManager&lt;T&gt;

The primary implementation of `IRelationshipManager<T>`:

**Location**: `Managers/RelationshipManager.cs`

**Implementation Details**:
- Uses `Dictionary<string, T>` for O(1) lookups by ID
- Uses `Lookup` or filtered queries for source/target searches
- Validates relationships before adding

**Usage**:
```csharp
IRelationshipManager<XmiHasStructuralMaterial> manager =
    new RelationshipManager<XmiHasStructuralMaterial>();
```

## Usage Examples

### Basic Relationship Management

```csharp
using XmiSchema.Core.Entities;
using XmiSchema.Core.Relationships;
using XmiSchema.Core.Interfaces;
using XmiSchema.Core.Managers;

// Create a relationship manager
IRelationshipManager<XmiHasStructuralMaterial> relManager =
    new RelationshipManager<XmiHasStructuralMaterial>();

// Create entities
var material = new XmiStructuralMaterial("MAT001", "Concrete C30", ...);
var crossSection = new XmiStructuralCrossSection("SEC001", "300x600", ...);

// Create relationship
var relationship = new XmiHasStructuralMaterial(crossSection, material);

// Add relationship
bool added = relManager.AddRelationship(relationship);
Console.WriteLine($"Relationship added: {added}"); // true

// Retrieve by ID
var retrieved = relManager.GetRelationship(relationship.ID);
if (retrieved != null)
{
    Console.WriteLine($"{retrieved.Source.ID} → {retrieved.Target.ID}");
}

// Get all relationships
var all = relManager.GetAll();
Console.WriteLine($"Total relationships: {all.Count()}");

// Remove relationship
bool removed = relManager.RemoveRelationship(relationship.ID);
Console.WriteLine($"Removed: {removed}"); // true
```

### Graph Traversal - Finding Outgoing Relationships

```csharp
using XmiSchema.Core.Relationships;

// Setup: CrossSection → Material
var material = new XmiStructuralMaterial("MAT001", "Concrete C30", ...);
var crossSection = new XmiStructuralCrossSection(
    id: "SEC001",
    name: "300x600",
    material: material,  // Reference to material
    ...
);

// Create manager
IRelationshipManager<XmiBaseRelationship> relManager =
    new RelationshipManager<XmiBaseRelationship>();

// Add various relationships
relManager.AddRelationship(new XmiHasStructuralMaterial(crossSection, material));

// Find what the cross-section references (outgoing edges)
var outgoing = relManager.FindBySource("SEC001");

Console.WriteLine($"Cross-section SEC001 references:");
foreach (var rel in outgoing)
{
    Console.WriteLine($"  {rel.EntityType}: {rel.Source.ID} → {rel.Target.ID}");
}
// Output:
// Cross-section SEC001 references:
//   XmiHasStructuralMaterial: SEC001 → MAT001
```

### Graph Traversal - Finding Incoming Relationships

```csharp
// Setup: Multiple cross-sections using the same material
var material = new XmiStructuralMaterial("MAT001", "Concrete C30", ...);

var section1 = new XmiStructuralCrossSection("SEC001", "300x600", ...);
var section2 = new XmiStructuralCrossSection("SEC002", "400x800", ...);
var section3 = new XmiStructuralCrossSection("SEC003", "200x400", ...);

IRelationshipManager<XmiHasStructuralMaterial> relManager =
    new RelationshipManager<XmiHasStructuralMaterial>();

// Add relationships
relManager.AddRelationship(new XmiHasStructuralMaterial(section1, material));
relManager.AddRelationship(new XmiHasStructuralMaterial(section2, material));
relManager.AddRelationship(new XmiHasStructuralMaterial(section3, material));

// Find what references the material (incoming edges)
var incoming = relManager.FindByTarget("MAT001");

Console.WriteLine($"Material MAT001 is used by {incoming.Count()} cross-sections:");
foreach (var rel in incoming)
{
    Console.WriteLine($"  {rel.Source.ID} ({rel.Source.Name})");
}
// Output:
// Material MAT001 is used by 3 cross-sections:
//   SEC001 (300x600)
//   SEC002 (400x800)
//   SEC003 (200x400)

// Impact analysis: If we change MAT001, these sections are affected
```

### Managing Multiple Relationship Types

```csharp
// Separate managers for different relationship types
IRelationshipManager<XmiHasStructuralMaterial> materialRelManager =
    new RelationshipManager<XmiHasStructuralMaterial>();

IRelationshipManager<XmiHasGeometry> geometryRelManager =
    new RelationshipManager<XmiHasGeometry>();

IRelationshipManager<XmiHasSegment> segmentRelManager =
    new RelationshipManager<XmiHasSegment>();

// Or use base type for heterogeneous collection
IRelationshipManager<XmiBaseRelationship> allRelManager =
    new RelationshipManager<XmiBaseRelationship>();

// Add different types
allRelManager.AddRelationship(new XmiHasStructuralMaterial(section, material));
allRelManager.AddRelationship(new XmiHasGeometry(connection, point));
allRelManager.AddRelationship(new XmiHasSegment(member, segment));

// Query by type
var materialRels = allRelManager.GetAll()
    .OfType<XmiHasStructuralMaterial>()
    .ToList();
```

### Dependency Analysis

```csharp
/// <summary>
/// Analyzes dependencies for a given entity.
/// </summary>
void AnalyzeDependencies(string entityId, IRelationshipManager<XmiBaseRelationship> relManager)
{
    // What does this entity depend on? (outgoing)
    var dependencies = relManager.FindBySource(entityId);

    Console.WriteLine($"\nEntity {entityId} depends on:");
    foreach (var dep in dependencies)
    {
        Console.WriteLine($"  → {dep.Target.ID} ({dep.Target.Name}) [{dep.EntityType}]");
    }

    // What depends on this entity? (incoming)
    var dependents = relManager.FindByTarget(entityId);

    Console.WriteLine($"\nEntity {entityId} is depended on by:");
    foreach (var dep in dependents)
    {
        Console.WriteLine($"  ← {dep.Source.ID} ({dep.Source.Name}) [{dep.EntityType}]");
    }

    // Check if orphaned (no incoming or outgoing)
    if (!dependencies.Any() && !dependents.Any())
    {
        Console.WriteLine($"\nWARNING: Entity {entityId} is orphaned (no connections)");
    }
}

// Usage
AnalyzeDependencies("SEC001", relationshipManager);
```

### Integration with Model Builder

```csharp
using XmiSchema.Core.Results;

var builder = new XmiSchemaModelBuilder();

// Add entities
builder.AddEntity(material);
builder.AddEntity(crossSection);
builder.AddEntity(member);

// Build model - relationships inferred automatically
var model = builder.BuildModel();

// Access relationships
Console.WriteLine($"Model has {model.Relationships.Count} relationships");

// Analyze with relationship manager
IRelationshipManager<XmiBaseRelationship> relManager =
    new RelationshipManager<XmiBaseRelationship>();

foreach (var rel in model.Relationships)
{
    relManager.AddRelationship(rel);
}

// Now perform graph queries
var crossSectionDeps = relManager.FindBySource("SEC001");
```

### Cleaning Up Relationships When Removing Entities

```csharp
/// <summary>
/// Safely removes an entity and all its relationships.
/// </summary>
bool RemoveEntityWithRelationships(
    string entityId,
    IEntityManager<XmiBaseEntity> entityManager,
    IRelationshipManager<XmiBaseRelationship> relManager)
{
    // Find all relationships involving this entity
    var outgoing = relManager.FindBySource(entityId);
    var incoming = relManager.FindByTarget(entityId);
    var allRelationships = outgoing.Concat(incoming).ToList();

    // Remove all relationships first
    foreach (var rel in allRelationships)
    {
        relManager.RemoveRelationship(rel.ID);
    }

    Console.WriteLine($"Removed {allRelationships.Count} relationships");

    // Then remove the entity
    bool removed = entityManager.RemoveEntity(entityId);

    if (removed)
    {
        Console.WriteLine($"Entity {entityId} and its relationships removed successfully");
    }

    return removed;
}
```

## Related Classes

### Base Classes
- **`XmiBaseRelationship`**: Base class for all relationships that can be managed by this interface

### Implementing Classes
- **`RelationshipManager<T>`**: Concrete implementation providing dictionary-based relationship storage

### Related Interfaces
- **`IEntityManager<T>`**: Manages entities (graph nodes)
- **`IModelManager`**: High-level model operations combining entities and relationships

### Relationship Types (Examples)
All relationship types can be managed with this interface:
- **`XmiHasStructuralMaterial`**: CrossSection/Surface → Material
- **`XmiHasStructuralCrossSection`**: CurveMember → CrossSection
- **`XmiHasGeometry`**: Entity → Geometry (Point3D, Line3D, Arc3D)
- **`XmiHasSegment`**: CurveMember → Segment
- **`XmiHasStructuralNode`**: Member/Segment → PointConnection
- **`XmiHasStructuralStorey`**: Entity → Storey

### Analysis Classes
- **`ExtensionDependencyAnalyzer`**: Uses relationship data for topological sorting and cycle detection
- **`ExtensionRelationshipExporter`**: Automatically infers relationships from entity properties

## Design Patterns

### Directed Graph Pattern

Relationships represent directed edges in a graph:

```
        ┌─────────────┐
        │   Source    │
        │  (Entity)   │
        └──────┬──────┘
               │
               │ Relationship
               │ (Directed Edge)
               ▼
        ┌─────────────┐
        │   Target    │
        │  (Entity)   │
        └─────────────┘
```

**Properties of the graph**:
- **Directed**: Relationships have direction (Source → Target)
- **Labeled**: Each edge has a type (XmiHasStructuralMaterial, XmiHasGeometry, etc.)
- **Typed**: Edges have properties beyond source/target (Name, Description, UmlType)
- **Multi-graph**: Multiple edges can exist between the same two nodes (different types)

### Adjacency Operations

The interface provides adjacency list operations:

| Method | Graph Operation | Description |
|--------|----------------|-------------|
| `FindBySource(id)` | Out-edges | adj[source] → [targets] |
| `FindByTarget(id)` | In-edges | reverse_adj[target] → [sources] |
| `GetAll()` | Edge list | All edges in graph |

### Separation of Nodes and Edges

Entities (nodes) and relationships (edges) are managed separately:

```
┌────────────────────┐        ┌──────────────────────┐
│  IEntityManager    │        │ IRelationshipManager │
│  (Manages Nodes)   │        │  (Manages Edges)     │
└────────────────────┘        └──────────────────────┘
         │                              │
         │                              │
         ▼                              ▼
    ┌─────────┐                    ┌─────────┐
    │ Entities│                    │Relations│
    └─────────┘                    └─────────┘
                    Combined by
                         │
                         ▼
                  ┌─────────────┐
                  │  XmiModel   │
                  │   (Graph)   │
                  └─────────────┘
```

**Benefits**:
- Independent lifecycle management
- Easier to rebuild relationships without affecting entities
- Clear separation of graph structure from node data

### Repository Pattern for Graphs

Similar to `IEntityManager`, this implements a repository pattern specialized for graph edges:

- **Add/Remove**: Edge persistence operations
- **Get/GetAll**: Edge query operations
- **FindBySource/FindByTarget**: Graph-specific queries

## Engineering Notes

### Performance Characteristics

Assuming dictionary-based implementation:

| Operation | Time Complexity | Notes |
|-----------|----------------|-------|
| AddRelationship | O(1) | Dictionary insert |
| RemoveRelationship | O(1) | Dictionary removal |
| GetRelationship | O(1) | Dictionary lookup |
| GetAll | O(n) | Enumerate all edges |
| FindBySource | O(n) | Linear scan of all relationships |
| FindByTarget | O(n) | Linear scan of all relationships |
| Clear | O(n) | Dictionary clear |

### Optimization Strategies

For large graphs with frequent source/target queries:

1. **Adjacency List Index**:
   ```csharp
   // Maintain separate indexes
   Dictionary<string, List<T>> _sourceIndex;  // sourceId → relationships
   Dictionary<string, List<T>> _targetIndex;  // targetId → relationships

   // FindBySource becomes O(1)
   public IEnumerable<T> FindBySource(string sourceId)
   {
       return _sourceIndex.TryGetValue(sourceId, out var rels)
           ? rels
           : Enumerable.Empty<T>();
   }
   ```

2. **ILookup for Grouping**:
   ```csharp
   var sourceIndex = relationships.ToLookup(r => r.Source.ID);
   var targetIndex = relationships.ToLookup(r => r.Target.ID);
   ```

### Memory Usage

- Each relationship: ~100-200 bytes
- Dictionary overhead: ~40 bytes per entry
- Total for 1000 relationships: ~140-240 KB

For graphs with N entities and E edges:
- Sparse graph: E ≈ N (tree-like)
- Dense graph: E ≈ N² (highly connected)

Typical XMI models are **sparse**: E ≈ 3-5×N

### Graph Integrity

The interface does **not enforce** graph integrity:
- Does NOT validate that source/target entities exist
- Does NOT prevent dangling references
- Does NOT maintain referential integrity

Validation should be done at higher levels:
- `XmiSchemaModelBuilder`: Validates during model construction
- `ModelManager`: Enforces integrity across entities and relationships

### Relationship Identity

Relationships are identified by their `ID` property. Additionally:
- Each relationship has a `Source` (entity reference)
- Each relationship has a `Target` (entity reference)
- Multiple relationships can exist between the same source/target (different types)

### Common Graph Queries

**1. Find all dependencies of an entity**:
```csharp
var dependencies = relManager.FindBySource(entityId)
    .Select(r => r.Target)
    .ToList();
```

**2. Find all dependents of an entity**:
```csharp
var dependents = relManager.FindByTarget(entityId)
    .Select(r => r.Source)
    .ToList();
```

**3. Check if entity has any connections**:
```csharp
bool isConnected = relManager.FindBySource(entityId).Any() ||
                   relManager.FindByTarget(entityId).Any();
```

**4. Get degree (number of connections)**:
```csharp
int outDegree = relManager.FindBySource(entityId).Count();
int inDegree = relManager.FindByTarget(entityId).Count();
int totalDegree = outDegree + inDegree;
```

**5. Find all entities connected to an entity**:
```csharp
var outgoing = relManager.FindBySource(entityId).Select(r => r.Target);
var incoming = relManager.FindByTarget(entityId).Select(r => r.Source);
var allConnected = outgoing.Concat(incoming).Distinct();
```

### Best Practices

**DO**:
- Use specific relationship types when possible: `IRelationshipManager<XmiHasStructuralMaterial>`
- Check return values from `AddRelationship()` and `RemoveRelationship()`
- Clean up relationships before removing entities
- Use `FindBySource`/`FindByTarget` for graph traversal

**DON'T**:
- Don't assume relationships exist (check collections for empty)
- Don't create relationships with null source or target
- Don't forget to remove relationships when removing entities
- Don't rely on order of `GetAll()` results

### Common Pitfalls

1. **Orphaned relationships** (source/target entity doesn't exist):
   ```csharp
   // BAD - entity removed but relationships remain
   entityManager.RemoveEntity("MAT001");
   // Relationships still reference MAT001!

   // GOOD - clean up first
   CleanupRelationships("MAT001");
   entityManager.RemoveEntity("MAT001");
   ```

2. **Not checking for null**:
   ```csharp
   // BAD
   var rel = relManager.GetRelationship("REL999");
   Console.WriteLine(rel.Source.ID); // NullReferenceException!

   // GOOD
   var rel = relManager.GetRelationship("REL999");
   if (rel != null)
   {
       Console.WriteLine(rel.Source.ID);
   }
   ```

3. **Assuming FindBySource/FindByTarget return results**:
   ```csharp
   // BAD - assumes results exist
   var first = relManager.FindBySource("SEC001").First(); // Exception if empty!

   // GOOD - check first
   var relationships = relManager.FindBySource("SEC001");
   if (relationships.Any())
   {
       var first = relationships.First();
   }
   ```

## See Also

- [XmiBaseRelationship.README.md](../Models/Bases/XmiBaseRelationship.README.md) - Base relationship class
- [RelationshipManager.README.md](../Managers/RelationshipManager.README.md) - Concrete implementation
- [IEntityManager.README.md](./IEntityManager.README.md) - Entity management
- [IModelManager.README.md](./IModelManager.README.md) - High-level model operations
- [ExtensionDependencyAnalyzer.README.md](../Extensions/ExtensionDependencyAnalyzer.README.md) - Graph analysis
- [ExtensionRelationshipExporter.README.md](../Extensions/ExtensionRelationshipExporter.README.md) - Automatic relationship inference
