# XmiSchema.Core

[![NuGet](https://img.shields.io/nuget/v/XmiSchema.Core.svg)](https://www.nuget.org/packages/XmiSchema.Core/)
[![Build Status](https://github.com/xmi-schema/xmi-schema-csharp/workflows/Build,%20Test%20&%20Publish%20NuGet/badge.svg)](https://github.com/xmi-schema/xmi-schema-csharp/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A C# library for building and managing structural engineering data models with automatic relationship inference. XmiSchema.Core exports models to JSON format representing graph structures (nodes and edges) for built environment data.

## Features

- **Graph-Based Architecture**: Represents built environment data as nodes and edges
- **Automatic Relationship Inference**: Automatically creates relationships between entities
- **Structural Engineering Support**: Built-in support for structural members, materials, cross-sections, and more
- **Topological Analysis**: Dependency analysis and cycle detection
- **JSON Export**: Export models to JSON format for interoperability
- **Type-Safe**: Strongly typed entities and relationships
- **.NET 8.0**: Modern .NET with nullable reference types and latest C# features

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package XmiSchema.Core
```

Or via Package Manager Console:

```powershell
Install-Package XmiSchema.Core
```

## Quick Start

```csharp
using XmiSchema.Core.Results;
using XmiSchema.Core.Entities;
using XmiSchema.Core.Geometries;

// Create a model builder
var builder = new XmiSchemaModelBuilder();

// Add entities
var point = new XmiPoint3D(
    id: "P1",
    name: "Point 1",
    x: 0.0,
    y: 0.0,
    z: 0.0
);

var material = new XmiStructuralMaterial(
    id: "MAT1",
    name: "Concrete C30",
    ifcguid: "",
    nativeId: "MAT1",
    description: "Concrete material",
    entityType: "XmiStructuralMaterial"
);

builder.AddEntity(point);
builder.AddEntity(material);

// Build the model (automatically infers relationships)
var model = builder.BuildModel();

// Export to JSON
var json = builder.BuildJsonString();

// Or save to file
builder.ExportJson("output.json");
```

## Core Concepts

### Entities

All entities inherit from `XmiBaseEntity` with common properties:
- `ID`: Unique identifier
- `Name`: Display name
- `IFCGUID`: IFC GUID (if applicable)
- `NativeId`: Native identifier from source system
- `Description`: Entity description
- `EntityType`: Type discriminator

Built-in entities include:
- `XmiStructuralCurveMember`: Structural beams, columns, braces
- `XmiStructuralSurfaceMember`: Structural slabs, walls, plates
- `XmiStructuralPointConnection`: Structural nodes/connections
- `XmiStructuralCrossSection`: Cross-section definitions
- `XmiStructuralMaterial`: Material properties
- `XmiStructuralStorey`: Building levels
- Geometry types: `XmiPoint3D`, `XmiLine3D`, `XmiArc3D`

### Relationships

The library automatically infers relationships from entity properties:
- `XmiHasGeometry`: Entity → Geometry
- `XmiHasStructuralCrossSection`: Member → Cross Section
- `XmiHasStructuralMaterial`: Cross Section/Member → Material
- `XmiHasStructuralNode`: Member → Node
- `XmiHasStructuralStorey`: Entity → Storey
- `XmiHasSegment`: Member → Segment

### Builder Pattern

The `XmiSchemaModelBuilder` provides a fluent API:

```csharp
var builder = new XmiSchemaModelBuilder();

// Add single entity
builder.AddEntity(entity);

// Add multiple entities
builder.AddEntities(entities);

// Build model with automatic relationship inference
var model = builder.BuildModel();

// Get topologically sorted entities (respects dependencies)
var sorted = builder.GetTopologicallySortedEntities();

// Detect circular dependencies
var cycles = builder.GetCycles();
```

### JSON Export

Export format is a graph structure:

```json
{
  "nodes": [
    {
      "XmiStructuralMaterial": {
        "ID": "MAT1",
        "Name": "Concrete C30",
        "EntityType": "XmiStructuralMaterial"
      }
    }
  ],
  "edges": [
    {
      "XmiHasStructuralMaterial": {
        "ID": "REL1",
        "Source": "CS1",
        "Target": "MAT1",
        "UmlType": "Composition"
      }
    }
  ]
}
```

## Advanced Usage

### Dependency Analysis

```csharp
// Check for circular dependencies
var cycles = builder.GetCycles();
if (cycles.Any())
{
    Console.WriteLine("Warning: Circular dependencies detected!");
}

// Get correct serialization order
var orderedEntities = builder.GetTopologicallySortedEntities();
```

### Custom Entities

Create custom entities by inheriting from `XmiBaseEntity`:

```csharp
public class CustomEntity : XmiBaseEntity
{
    public string CustomProperty { get; set; }

    public CustomEntity(string id, string name)
        : base(id, name, "", "", "", nameof(CustomEntity))
    {
    }
}
```

## Architecture

The library follows these patterns:

- **Builder Pattern**: `XmiSchemaModelBuilder` for model construction
- **Manager Pattern**: `EntityManager<T>` and `RelationshipManager<T>` for CRUD operations
- **Extension Methods**: Core processing logic in extension classes
- **Graph Algorithms**: Uses QuikGraph for topological sorting and cycle detection

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Links

- [NuGet Package](https://www.nuget.org/packages/XmiSchema.Core/)
- [GitHub Repository](https://github.com/xmi-schema/xmi-schema-csharp)
- [Issue Tracker](https://github.com/xmi-schema/xmi-schema-csharp/issues)

## Support

For questions, issues, or feature requests, please [open an issue](https://github.com/xmi-schema/xmi-schema-csharp/issues) on GitHub.
