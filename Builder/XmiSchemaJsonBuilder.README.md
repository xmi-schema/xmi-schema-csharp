# XmiSchemaJsonBuilder

Alternative builder for constructing XMI schema models with automatic relationship inference.

## Overview

`XmiSchemaJsonBuilder` provides the same functionality as `XmiSchemaModelBuilder`. It implements the builder pattern to construct complete XMI models with automatically inferred relationships and JSON export capabilities.

## Location

`Builder/XmiSchemaJsonBuilder.cs`

## Relationship to XmiSchemaModelBuilder

`XmiSchemaJsonBuilder` and `XmiSchemaModelBuilder` are functionally identical classes with the same API and behavior. Both provide:

- Entity management (AddEntity, AddEntities)
- Model building with automatic relationship inference
- Dependency analysis (topological sorting, cycle detection)
- JSON export (file and string)

**Note:** In most cases, you should use `XmiSchemaModelBuilder` as it is the primary, documented entry point for the library.

## Key Features

- **Fluent API**: Add entities one at a time or in batches
- **Automatic Relationship Inference**: Analyzes entity properties to create relationships
- **Dependency Analysis**: Provides topological sorting and circular dependency detection
- **JSON Export**: Direct export to JSON files or strings
- **Manager Integration**: Uses EntityManager and RelationshipManager internally

## Methods

All methods have identical signatures and behavior to `XmiSchemaModelBuilder`:

### Entity Management
- `AddEntity(XmiBaseEntity entity)` - Add single entity
- `AddEntities(IEnumerable<XmiBaseEntity> entities)` - Add multiple entities

### Model Building
- `BuildModel()` - Build model with automatic relationship inference

### Dependency Analysis
- `GetTopologicallySortedEntities()` - Get entities in dependency order
- `GetCycles()` - Detect circular dependencies

### JSON Export
- `ExportJson(string outputPath)` - Export to JSON file
- `BuildJsonString()` - Get JSON as string

## Usage Example

```csharp
// Initialize builder
var builder = new XmiSchemaJsonBuilder();

// Create entities
var material = new XmiStructuralMaterial(...);
var crossSection = new XmiStructuralCrossSection(...);
var member = new XmiStructuralCurveMember(...);

// Add entities
builder.AddEntities(new[] { material, crossSection, member });

// Build model
var model = builder.BuildModel();

// Export to JSON
builder.ExportJson("output/model.json");

// Or get JSON string
string json = builder.BuildJsonString();
```

## Internal Architecture

Identical to `XmiSchemaModelBuilder`:

```csharp
private readonly EntityManager<XmiBaseEntity> _entityManager = new();
private readonly RelationshipManager<XmiBaseRelationship> _relationshipManager = new();
```

## Processing Pipeline

Same as `XmiSchemaModelBuilder`:

```
AddEntity/AddEntities
   ↓
BuildModel()
   ↓
ExtensionRelationshipExporter.ExportRelationships()
   ↓
XmiModel (entities + relationships)
   ↓
ExportJson() or BuildJsonString()
   ↓
JSON Output
```

## When to Use This Class

**Recommendation:** Use `XmiSchemaModelBuilder` instead, as it is the primary documented builder.

`XmiSchemaJsonBuilder` exists for:
- Legacy compatibility
- Organizational preferences
- Semantic clarity (if the "Json" suffix better communicates intent)

Both classes can be used interchangeably with no functional differences.

## Automatic Relationship Inference

Identical behavior to `XmiSchemaModelBuilder`. The builder automatically creates relationships by analyzing entity properties:

- Member → CrossSection (XmiHasStructuralCrossSection)
- Member → Segments (XmiHasSegment)
- CrossSection → Material (XmiHasStructuralMaterial)
- Point → Geometry (XmiHasGeometry)
- And more...

## Related Classes

- **XmiSchemaModelBuilder**: Primary builder (identical functionality)
- **XmiModel**: The output model structure
- **EntityManager<T>**: Manages entity storage
- **RelationshipManager<T>**: Manages relationship storage
- **ExtensionRelationshipExporter**: Infers relationships from entities
- **ExtensionDependencyAnalyzer**: Analyzes graph dependencies
- **ExtensionNativeJsonBuilder**: Exports model to JSON

## See Also

- [XmiSchemaModelBuilder](XmiSchemaModelBuilder.README.md) - Primary builder (recommended)
- [XmiModel](../Models/Model/XmiModel.README.md) - Model structure
- [ExtensionRelationshipExporter](../Extensions/ExtensionRelationshipExporter.README.md) - Relationship inference
- [ExtensionNativeJsonBuilder](../Extensions/ExtensionNativeJsonBuilder.README.md) - JSON export
