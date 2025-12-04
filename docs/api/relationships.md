---
layout: default
title: Relationships
---

# Relationships

Relationship entities inherit from `XmiBaseRelationship` and form the edges of the Cross Model Information graph. Each relationship pairs a source entity with a target entity, exposing the association through a UML type string.

## Overview

| Relationship | Purpose | Common Source → Target |
| --- | --- | --- |
| `XmiHasGeometry` | Binds an entity to its geometry (line, arc, etc.). | Member/Surface → `XmiBaseGeometry` |
| `XmiHasLine3D` | Specialised helper for line references. | `XmiStructuralCurveMember` → `XmiLine3D` |
| `XmiHasPoint3D` | Connects point connections to coordinates. | `XmiStructuralPointConnection` → `XmiPoint3D` |
| `XmiHasSegment` | Links curve members to their `XmiSegment` definitions. | `XmiStructuralCurveMember` → `XmiSegment` |
| `XmiHasStructuralMaterial` | Assigns materials to consuming entities. | Cross section / member → `XmiMaterial` |
| `XmiHasStructuralNode` | Declares a member's analytical node dependency. | `XmiStructuralCurveMember` → `XmiStructuralPointConnection` |
| `XmiHasCrossSection` | Specifies which cross-section a member uses. | `XmiStructuralCurveMember` → `XmiCrossSection` |
| `XmiHasStorey` | Places an entity on a storey level. | Point connection → `XmiStructuralStorey` |
| `XmiHasStructuralCurveMember` | Links physical elements to analytical curve members. | `XmiBasePhysicalEntity` → `XmiBaseStructuralAnalyticalEntity` |

## XmiHasStructuralCurveMember

Links physical beam or column elements to their structural analytical representations.

### Example

```csharp
var beam = new XmiBeam(...);
var curveMember = new XmiStructuralCurveMember(...);

var relationship = new XmiHasStructuralCurveMember(beam, curveMember);
model.AddXmiHasStructuralCurveMember(relationship);
```

### Type Constraints

- **Source**: Must be `XmiBasePhysicalEntity` (e.g., `XmiBeam`, `XmiColumn`)
- **Target**: Must be `XmiBaseStructuralAnalyticalEntity` (e.g., `XmiStructuralCurveMember`)

## XmiHasCrossSection

Assigns a cross-section definition to a structural member.

### Example

```csharp
var crossSection = new XmiCrossSection(
    "sec-1",
    "W12x26",
    "ifc-guid",
    "SEC-1",
    "Wide flange section",
    XmiShapeEnum.IShape,
    new IShapeParameters(0.31, 0.165, 0.0094, 0.00635, 0.0095),
    0.00497,
    // ... other properties
);

var relationship = new XmiHasCrossSection(curveMember, crossSection);
```

## XmiHasStructuralMaterial

Assigns material properties to sections or members.

### Example

```csharp
var material = new XmiMaterial(
    "mat-1",
    "Steel A992",
    "ifc-guid",
    "MAT-1",
    "Structural steel",
    XmiStructuralMaterialTypeEnum.Steel,
    345, // Yield strength
    78.5, // Density
    "200000", // Young's modulus
    "80000", // Shear modulus
    "0.3", // Poisson's ratio
    1.2 // Thermal expansion
);

var relationship = new XmiHasStructuralMaterial(crossSection, material);
```

## XmiHasGeometry

Links entities to their geometric representations.

### Example

```csharp
var line = new XmiLine3D(
    "line-1",
    "Member Line",
    "ifc-guid",
    "LINE-1",
    "Centerline geometry",
    startPoint,
    endPoint
);

var relationship = new XmiHasGeometry(curveMember, line);
```

## XmiHasStructuralNode

Connects structural members to their nodal connections.

### Example

```csharp
var pointConnection = new XmiStructuralPointConnection(...);

var hasStartNode = new XmiHasStructuralNode(curveMember, pointConnection);
var hasEndNode = new XmiHasStructuralNode(curveMember, pointConnection);
```

## XmiHasStorey

Places entities on a specific building level.

### Example

```csharp
var storey = new XmiStorey(...);
var pointConnection = new XmiStructuralPointConnection(...);

var relationship = new XmiHasStorey(pointConnection, storey);
```

## Adding New Relationships

When adding new entity types, introduce matching relationships so consuming systems can navigate the graph:

1. Inherit from `XmiBaseRelationship`
2. Keep constructors consistent with the `<source, target>` signature
3. Add appropriate type constraints
4. Cover with unit tests in `tests/XmiSchema.Core.Tests/Relationships`

### Example Template

```csharp
public class XmiHasNewRelationship : XmiBaseRelationship
{
    public XmiHasNewRelationship(
        XmiSourceEntity source,
        XmiTargetEntity target)
        : base(source, target, nameof(XmiHasNewRelationship), "Association")
    {
    }
}
```

[Back to API Reference](.)
