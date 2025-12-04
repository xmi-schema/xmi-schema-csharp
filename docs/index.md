---
layout: default
title: Home
---

# XMI Schema C#

A .NET library for structural engineering data exchange using the Cross Model Information (XMI) standard.

## Quick Start

Install via NuGet:

```bash
dotnet add package xmi-schema-csharp
```

Create your first model:

```csharp
using XmiSchema.Core.Models;
using XmiSchema.Core.Models.Entities.Physical;

var model = new XmiModel();
var beam = new XmiBeam(
    "beam-1",
    "Steel Beam",
    "ifc-guid",
    "BEAM-1",
    "Main support beam",
    XmiStructuralCurveMemberSystemLineEnum.MiddleMiddle,
    5.0,
    "1,0,0", "0,1,0", "0,0,1",
    0, 0, 0, 0, 0, 0,
    "Fixed", "Pinned"
);
model.AddXmiBeam(beam);
```

## Documentation

### API Reference

- [Base Types](api/base-types) - Core abstractions and base classes
- [Entities](api/entities) - Business objects representing structural elements
- [Physical Elements](api/physical) - Beams, columns, slabs, and walls
- [Structural Analytical](api/structural-analytical) - Analytical model entities
- [Relationships](api/relationships) - Graph edges connecting entities
- [Enums](api/enums) - Enumeration types and constants
- [Geometries](api/geometries) - Spatial primitives

### Reference Guides

- [Shape Parameters](reference/shape-parameters) - Cross-section parameter definitions

### Examples

- [Usage Examples](examples/usage) - Common scenarios and patterns

## Features

- **Domain-Driven Design**: Separate Physical, StructuralAnalytical, Geometry, and Functional domains
- **Type-Safe**: Strongly typed C# entities with validation
- **JSON Serialization**: Built-in support for Cross Model Information format
- **Relationship Graph**: Model connections between structural elements
- **Extensible**: Easy to add custom entities and relationships

## License

See LICENSE file for details.

## Contributing

Contributions are welcome! Please see the main repository for guidelines.
