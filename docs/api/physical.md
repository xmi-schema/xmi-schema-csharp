---
layout: default
title: Physical Elements
---

# Physical Elements

Physical elements represent real-world building components in the built environment. All physical entities inherit from `XmiBasePhysicalEntity` and have their `Type` automatically set to `Physical`.

## XmiBeam

Represents a physical beam element - typically a horizontal structural member.

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `SystemLine` | `XmiSystemLineEnum` | Relative position of the analytical line inside the physical profile |
| `Length` | `double` | Physical length of the beam |
| `LocalAxisX` | `string` | Serialized orientation of local X axis |
| `LocalAxisY` | `string` | Serialized orientation of local Y axis |
| `LocalAxisZ` | `string` | Serialized orientation of local Z axis |
| `BeginNodeXOffset` | `double` | X offset applied to the start node |
| `EndNodeXOffset` | `double` | X offset applied to the end node |
| `BeginNodeYOffset` | `double` | Y offset applied to the start node |
| `EndNodeYOffset` | `double` | Y offset applied to the end node |
| `BeginNodeZOffset` | `double` | Z offset applied to the start node |
| `EndNodeZOffset` | `double` | Z offset applied to the end node |
| `EndFixityStart` | `string` | Boundary condition definition at the start |
| `EndFixityEnd` | `string` | Boundary condition definition at the end |

### Example

```csharp
var beam = new XmiBeam(
    "beam-1",
    "Steel Beam",
    "ifc-guid",
    "BEAM-1",
    "Main support beam",
    XmiSystemLineEnum.MiddleMiddle,
    5.0,
    "1,0,0", "0,1,0", "0,0,1",
    0.1, 0.1, 0, 0, 0, 0,
    "Fixed", "Pinned"
);
```

## XmiColumn

Represents a physical column element - typically a vertical structural member.

### Properties

Same properties as `XmiBeam`.

### Example

```csharp
var column = new XmiColumn(
    "col-1",
    "Concrete Column",
    "ifc-guid",
    "COL-1",
    "Ground floor column",
    XmiSystemLineEnum.MiddleMiddle,
    3.5,
    "1,0,0", "0,1,0", "0,0,1",
    0, 0, 0, 0, 0, 0,
    "Fixed", "Fixed"
);
```

## XmiSlab

Represents a physical slab element - typically a horizontal plate.

### Properties

Inherits base properties from `XmiBasePhysicalEntity`:
- `Id`, `Name`, `ifcGuid`, `NativeId`, `Description`, `EntityType`, `Type`

### Example

```csharp
var slab = new XmiSlab(
    "slab-1",
    "Concrete Slab",
    "ifc-guid",
    "SLAB-1",
    "Floor slab"
);
```

## XmiWall

Represents a physical wall element - typically a vertical plate.

### Properties

Inherits base properties from `XmiBasePhysicalEntity`:
- `Id`, `Name`, `ifcGuid`, `NativeId`, `Description`, `EntityType`, `Type`

### Example

```csharp
var wall = new XmiWall(
    "wall-1",
    "Concrete Wall",
    "ifc-guid",
    "WALL-1",
    "Shear wall"
);
```

## Linking to Analytical Elements

Physical elements can be linked to their analytical counterparts using relationships:

```csharp
// Link beam to analytical curve member
var relationship = new XmiHasStructuralCurveMember(beam, curveMember);
model.AddXmiHasStructuralCurveMember(relationship);
```

[Back to API Reference](.)
