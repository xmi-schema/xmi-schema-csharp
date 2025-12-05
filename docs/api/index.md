---
layout: default
title: API Reference
---

# API Reference

Complete reference documentation for the XMI Schema C# library.

## Core Concepts

### [Base Types](base-types)

Foundation classes and abstractions that all entities inherit from:

- **XmiBaseEntity** - Root entity class with identifiers and domain type
- **XmiBasePhysicalEntity** - Base for physical building elements
- **XmiBaseStructuralAnalyticalEntity** - Base for analytical model entities
- **XmiBaseGeometry** - Base for spatial primitives
- **XmiBaseRelationship** - Base for graph edges

### Domain Architecture

The library uses a domain-driven architecture with four primary domains:

- **Physical** - Real-world building components (beams, columns, slabs, walls)
- **StructuralAnalytical** - Mathematical analysis model
- **Geometry** - Spatial primitives (points, lines, arcs)
- **Functional** - Logical/functional groupings

## Entities

### [Physical Elements](physical)

Physical building components:

- **XmiBeam** - Horizontal structural members
- **XmiColumn** - Vertical structural members
- **XmiSlab** - Horizontal plate elements
- **XmiWall** - Vertical plate elements

### [Structural Analytical Elements](structural-analytical)

Analytical model entities for structural analysis:

- **XmiStructuralCurveMember** - Linear analytical members (beams, columns, bracing)
- **XmiStructuralSurfaceMember** - Surface analytical members (slabs, walls, shells)
- **XmiStructuralPointConnection** - Nodal connection points
- **XmiStorey** - Building levels with elevation and mass properties

### [Other Entities](entities)

Additional business objects:

- **XmiCrossSection** - Profile geometry and section properties
- **XmiMaterial** - Material definitions with mechanical properties
- **XmiSegment** - Sub-spans along curve members
- **XmiUnit** - Unit mappings for attributes

## [Geometries](geometries)

Spatial primitives for defining geometry:

- **XmiPoint3D** - 3D coordinates with tolerance-aware equality
- **XmiLine3D** - Straight line segments
- **XmiArc3D** - Circular arcs

## [Relationships](relationships)

Graph edges connecting entities:

- **XmiHasStructuralCurveMember** - Link physical to analytical curve members
- **XmiHasCrossSection** - Assign sections to members
- **XmiHasMaterial** - Assign materials to sections/members
- **XmiHasGeometry** - Link entities to geometry
- **XmiHasStructuralNode** - Connect members to nodes
- **XmiHasStorey** - Place entities on building levels
- **XmiHasSegment** - Link curve members to segments

## [Enums](enums)

Enumeration types and constants:

### Domain & Classification
- **XmiBaseEntityDomainEnum** - Entity domain types
- **XmiStructuralCurveMemberTypeEnum** - Curve member classifications
- **XmiStructuralSurfaceMemberTypeEnum** - Surface member classifications

### Geometry & Position
- **XmiSystemLineEnum** - Analytical line positions
- **XmiStructuralSurfaceMemberSystemPlaneEnum** - Analytical plane positions
- **XmiSegmentTypeEnum** - Geometry families

### Materials & Shapes
- **XmiMaterialTypeEnum** - Material categories
- **XmiShapeEnum** - Cross-section shapes

### Units
- **XmiUnitEnum** - Measurement units

## Reference Guides

### [Shape Parameters](../reference/shape-parameters)

Detailed parameter definitions for all cross-section shapes, including:
- Parameter keys for each shape type
- Usage examples
- Serialization format
- Custom shape handling

## Quick Navigation

| Topic | Description |
| --- | --- |
| [Getting Started](../) | Installation and quick start guide |
| [Examples](../examples/usage) | Common usage scenarios and patterns |
| [Base Types](base-types) | Foundation classes |
| [Physical Elements](physical) | Beams, columns, slabs, walls |
| [Analytical Elements](structural-analytical) | Analysis model entities |
| [Geometries](geometries) | Points, lines, arcs |
| [Relationships](relationships) | Connecting entities |
| [Enums](enums) | Constants and classifications |

## Model Management

The **XmiModel** class is the primary container for all entities and relationships:

```csharp
var model = new XmiModel();

// Add entities
model.AddXmiBeam(beam);
model.AddXmiColumn(column);
model.AddXmiStructuralCurveMember(curveMember);

// Add relationships
model.AddXmiHasStructuralCurveMember(relationship);
```

The **XmiManager** class manages multiple models and handles serialization:

```csharp
var manager = new XmiManager();
manager.Models.Add(model);

// Export to JSON
string json = manager.BuildJson();

// Import from JSON
var loadedManager = XmiManager.FromJson(json);
```

---

[Back to Documentation Home](../)
