# ExtensionDependencyAnalyzer

Analyzes entity relationship graphs for dependency ordering and circular dependency detection.

## Overview

`ExtensionDependencyAnalyzer` uses graph algorithms to analyze the dependency structure of XMI models. It provides topological sorting for correct serialization order and detects circular dependencies that could cause issues in model processing.

## Location

`Extensions/ExtensionDependencyAnalyzer.cs`

## Key Features

- **Topological Sorting**: Orders entities based on dependencies (dependencies first)
- **Circular Dependency Detection**: Identifies cycles in the relationship graph
- **QuikGraph Integration**: Leverages QuikGraph library for graph algorithms
- **Directed Graph Analysis**: Treats relationships as directed edges

## Constructor

```csharp
public ExtensionDependencyAnalyzer(
    List<XmiBaseEntity> entities,
    List<XmiBaseRelationship> relationships
)
```

**Parameters:**
- `entities`: All entities in the model
- `relationships`: All relationships in the model

**Process:**
1. Creates a directed graph using QuikGraph's `AdjacencyGraph`
2. Adds all entities as vertices
3. Adds all relationships as directed edges (Source → Target)

## Methods

### GetTopologicallySortedEntities

```csharp
public List<XmiBaseEntity> GetTopologicallySortedEntities()
```

Returns entities in topological order (dependencies appear before dependents).

**Returns:** List of entities sorted so that if entity A depends on entity B, then B appears before A in the list

**Behavior:**
- If topological sorting succeeds, returns sorted list
- If circular dependencies exist, catches exception and returns original order
- Prints warning message to console if sorting fails

**Use Cases:**
- Serialization order for export
- Import order for creating entities
- Dependency-aware processing

**Example:**
```csharp
var analyzer = new ExtensionDependencyAnalyzer(model.Entities, model.Relationships);
var sortedEntities = analyzer.GetTopologicallySortedEntities();

// Output order: Materials → CrossSections → Members
foreach (var entity in sortedEntities)
{
    Console.WriteLine($"{entity.EntityType}: {entity.ID}");
}
```

### DetectCycles

```csharp
public List<List<XmiBaseEntity>> DetectCycles()
```

Detects all circular dependencies in the entity graph.

**Returns:** List of cycles, where each cycle is a list of entities forming a circular dependency path

**Algorithm:**
- Depth-first search with stack tracking
- Identifies back edges indicating cycles
- Returns all unique cycles found

**Example:**
```csharp
var cycles = analyzer.DetectCycles();

if (cycles.Any())
{
    Console.WriteLine("WARNING: Circular dependencies detected!");

    foreach (var cycle in cycles)
    {
        var cycleIds = string.Join(" → ", cycle.Select(e => e.ID));
        Console.WriteLine($"Cycle: {cycleIds}");
    }
}
else
{
    Console.WriteLine("No circular dependencies found.");
}
```

## Graph Structure

### Vertices
Each entity becomes a vertex in the graph.

### Edges
Each relationship creates a directed edge:
```
Source Entity ──[Relationship]──> Target Entity
```

### Example Graph

```
Material
   ↑
   │ (XmiHasStructuralMaterial)
   │
CrossSection
   ↑
   │ (XmiHasStructuralCrossSection)
   │
CurveMember
   ↑
   │ (XmiHasSegment)
   │
Segment
```

Topological order: Material, CrossSection, CurveMember, Segment

## Topological Sorting Example

### Input Model

```csharp
var material = new XmiStructuralMaterial("MAT001", ...);
var crossSection = new XmiStructuralCrossSection("CS001", ..., material);
var member = new XmiStructuralCurveMember("M001", ..., crossSection);

var model = new XmiModel
{
    Entities = new List<XmiBaseEntity> { member, crossSection, material },  // Random order
    Relationships = new List<XmiBaseRelationship>
    {
        new XmiHasStructuralMaterial(crossSection, material),
        new XmiHasStructuralCrossSection(member, crossSection)
    }
};
```

### Topological Sort Output

```csharp
var analyzer = new ExtensionDependencyAnalyzer(model.Entities, model.Relationships);
var sorted = analyzer.GetTopologicallySortedEntities();

// Result: [material, crossSection, member]
// Dependencies first: material has no deps, crossSection depends on material, member depends on crossSection
```

## Circular Dependency Detection

### Example Circular Dependency

```csharp
// Entity A references Entity B
// Entity B references Entity A
// This creates a cycle

var cycles = analyzer.DetectCycles();
// Returns: [[EntityA, EntityB]]
```

### Complex Cycle Example

```
Member1 → CrossSection1 → Material1 → Member1
```

This creates a 3-entity cycle that would be detected:
```csharp
cycles = [[Member1, CrossSection1, Material1]]
```

## Integration with Builder

The analyzer is used automatically in `XmiSchemaModelBuilder`:

```csharp
public List<XmiBaseEntity> GetTopologicallySortedEntities()
{
    var model = BuildModel();
    var analyzer = new ExtensionDependencyAnalyzer(model.Entities, model.Relationships);
    return analyzer.GetTopologicallySortedEntities();
}

public List<List<XmiBaseEntity>> GetCycles()
{
    var model = BuildModel();
    var analyzer = new ExtensionDependencyAnalyzer(model.Entities, model.Relationships);
    return analyzer.DetectCycles();
}
```

## Complete Usage Example

```csharp
// Build model
var builder = new XmiSchemaModelBuilder();
builder.AddEntity(material);
builder.AddEntity(crossSection);
builder.AddEntity(member);

var model = builder.BuildModel();

// Analyze dependencies
var analyzer = new ExtensionDependencyAnalyzer(model.Entities, model.Relationships);

// Check for cycles first
var cycles = analyzer.DetectCycles();
if (cycles.Any())
{
    throw new InvalidOperationException(
        $"Model contains {cycles.Count} circular dependency cycles"
    );
}

// Get sorted entities for export
var sortedEntities = analyzer.GetTopologicallySortedEntities();

// Export in dependency order
foreach (var entity in sortedEntities)
{
    ExportEntity(entity);  // Materials first, then cross-sections, then members
}
```

## Algorithm Details

### Topological Sort
Uses QuikGraph's `TopologicalSortAlgorithm`:
- Kahn's algorithm or DFS-based approach
- Time complexity: O(V + E) where V = vertices, E = edges
- Fails if cycles exist

### Cycle Detection
Custom DFS implementation:
- Maintains visited set and current path stack
- Detects back edges (edges to ancestors in DFS tree)
- Time complexity: O(V + E)

## Dependencies

- **QuikGraph 2.5.0**: Graph data structures and algorithms
  - `AdjacencyGraph<TVertex, TEdge>`: Directed graph representation
  - `TopologicalSortAlgorithm<TVertex, TEdge>`: Topological sorting
  - `Edge<TVertex>`: Directed edge representation

## Error Handling

### Topological Sort Failure
When circular dependencies exist:
```csharp
try
{
    // Attempt topological sort
}
catch
{
    Console.WriteLine("Topological sorting fails (circular dependencies may exist), returning to the original order");
    return _graph.Vertices.ToList();
}
```

Returns entities in their original insertion order as fallback.

## Design Patterns

### Facade Pattern
Simplifies interaction with QuikGraph library.

### Adapter Pattern
Adapts XMI entities and relationships to QuikGraph's graph structures.

### Strategy Pattern
Different algorithms for different analysis tasks (sorting vs. cycle detection).

## Related Classes

- **XmiSchemaModelBuilder**: Uses this analyzer for dependency analysis
- **XmiBaseEntity**: Vertices in the graph
- **XmiBaseRelationship**: Edges in the graph
- **XmiModel**: Provides entities and relationships for analysis

## See Also

- [XmiSchemaModelBuilder](../Builder/XmiSchemaModelBuilder.README.md) - Uses this analyzer
- [ExtensionRelationshipExporter](ExtensionRelationshipExporter.README.md) - Creates relationships analyzed by this class
- [XmiModel](../Models/Model/XmiModel.README.md) - Model structure
