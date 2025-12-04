---
layout: default
title: Entities
---

# Entities

This folder contains the business objects that describe the built-environment graph. They all inherit from `XmiBaseEntity` and can be serialized into the Cross Model Information payload.

## Entity Overview

| Class | Description | Typical Usage |
| --- | --- | --- |
| `XmiSegment` | Represents a sub-span along a curve member with a positional offset and segment type. | Attach to `XmiStructuralCurveMember` via `XmiHasSegment` when detailed geometry is required. |
| `XmiCrossSection` | Holds all geometric and analytical properties for a profile (area, torsion, section moduli). | Assign to curve or surface members to standardise their section definition. |
| `XmiStructuralCurveMember` | Beam/column/bracing equivalent with offsets, system line, and fixities. | Use when modeling linear members extracted from authoring tools. |
| `XmiMaterial` | Material definition with grade, density, and stiffness constants. | Shared across sections or members so consumers can look up properties. |
| `XmiStructuralPointConnection` | Analytical node that ties members and storeys together. | Create through `XmiModel.CreateStructurePointConnection` to automatically reuse identical coordinates. |
| `XmiStructuralStorey` | Represents a level with elevation, mass, and reaction info. | Link point connections or surfaces to storeys for vertical organization. |
| `XmiStructuralSurfaceMember` | Plate, slab, or wall element capturing thickness, axes, and plane metadata. | Use to model surface-based analytical elements. |
| `XmiStructuralUnit` | Maps entity attributes to `XmiUnitEnum` units for conversion. | Persist the measurement standard for downstream validation. |

## Physical Elements

Physical elements represent real-world building components:

- **[XmiBeam](physical)** - Horizontal structural member
- **[XmiColumn](physical)** - Vertical structural member
- **[XmiSlab](physical)** - Horizontal plate element
- **[XmiWall](physical)** - Vertical plate element

## Structural Analytical Elements

Analytical elements represent the structural analysis model:

- **[XmiStructuralCurveMember](structural-analytical)** - Linear analytical member
- **[XmiStructuralSurfaceMember](structural-analytical)** - Surface analytical member
- **[XmiStructuralPointConnection](structural-analytical)** - Nodal connection point

## Cross Section Parameters

`XmiCrossSection.Parameters` wraps an `IXmiShapeParameters` implementation (e.g., `RectangularShapeParameters`). Each class exposes a typed constructor but serializes to the same dictionary shown in the [Shape Parameters Reference](../reference/shape-parameters).

## Adding New Entities

When adding new entities:

1. Extend the appropriate base class (`XmiBasePhysicalEntity`, `XmiBaseStructuralAnalyticalEntity`, or `XmiBaseGeometry`)
2. Add comprehensive XML documentation
3. Keep constructor arguments ordered: `[id, name, ifcGuid, nativeId, description, domain-specific args]`
4. Implement `IEquatable<T>` based on `NativeId`
5. Add regression tests under `tests/XmiSchema.Core.Tests/Entities/<ClassName>Tests.cs`

[Back to API Reference](.)
