# XmiStructuralSurfaceMemberTypeEnum

Enumeration defining types of structural surface members (2D elements).

## Overview

`XmiStructuralSurfaceMemberTypeEnum` classifies surface structural elements by their primary function and location in the building structure.

## Location

`Models/Enums/XmiStructuralSurfaceMemberTypeEnum.cs`

## Values

| Enum Value | JSON Value | Description |
|------------|-----------|-------------|
| `Slab` | "Slab" | Horizontal floor or roof slab |
| `Wall` | "Wall" | Vertical wall element |
| `PadFooting` | "PadFooting" | Individual column footing |
| `StripFooting` | "StripFooting" | Continuous wall footing |
| `Pilecap` | "Pilecap" | Cap over pile group |
| `RoofPanel` | "RoofPanel" | Roof panel or deck |
| `WallPanel` | "WallPanel" | Prefabricated wall panel |
| `Raft` | "Raft" | Raft foundation (mat foundation) |
| `Unknown` | "Unknown" | Type not determined |

## Usage Examples

### Floor Slab

```csharp
var floorSlab = new XmiStructuralSurfaceMember(
    id: "SLB001",
    name: "Floor-Slab-L1",
    surfaceMemberType: XmiStructuralSurfaceMemberTypeEnum.Slab,
    thickness: 0.15,  // 150mm
    ...
);
```

### Shear Wall

```csharp
var shearWall = new XmiStructuralSurfaceMember(
    id: "WALL001",
    name: "Shear-Wall-1",
    surfaceMemberType: XmiStructuralSurfaceMemberTypeEnum.Wall,
    thickness: 0.20,  // 200mm
    ...
);
```

### Pad Footing

```csharp
var padFooting = new XmiStructuralSurfaceMember(
    id: "FTG001",
    name: "Footing-C1",
    surfaceMemberType: XmiStructuralSurfaceMemberTypeEnum.PadFooting,
    thickness: 0.60,  // 600mm
    ...
);
```

### Raft Foundation

```csharp
var raft = new XmiStructuralSurfaceMember(
    id: "RAFT001",
    name: "Raft-Foundation",
    surfaceMemberType: XmiStructuralSurfaceMemberTypeEnum.Raft,
    thickness: 0.80,  // 800mm
    ...
);
```

## JSON Serialization

```json
{
  "SurfaceMemberType": "Slab"
}
```

## Member Type Characteristics

### Slab
- **Orientation**: Horizontal
- **Primary Load**: Bending from gravity loads
- **Typical Thickness**: 100-300mm
- **Location**: Floors, flat roofs

### Wall
- **Orientation**: Vertical
- **Primary Load**: Axial, shear, bending
- **Typical Thickness**: 150-300mm
- **Purpose**: Lateral resistance, vertical support

### PadFooting
- **Orientation**: Horizontal (below grade)
- **Primary Load**: Bearing pressure from column
- **Typical Thickness**: 400-1000mm
- **Purpose**: Distribute column load to soil

### StripFooting
- **Orientation**: Horizontal (below grade)
- **Primary Load**: Distributed load from wall
- **Typical Thickness**: 300-600mm
- **Purpose**: Support load-bearing walls

### Pilecap
- **Orientation**: Horizontal (below grade)
- **Primary Load**: Transfer load to pile group
- **Typical Thickness**: 600-1500mm
- **Purpose**: Connect column to multiple piles

### RoofPanel
- **Orientation**: Horizontal or inclined
- **Primary Load**: Gravity, wind, snow
- **Typical Thickness**: 75-150mm
- **Purpose**: Roof structure

### WallPanel
- **Orientation**: Vertical
- **Primary Load**: Self-weight, lateral loads
- **Typical Thickness**: 150-250mm
- **Purpose**: Prefabricated wall system

### Raft
- **Orientation**: Horizontal (below grade)
- **Primary Load**: Distributed building load
- **Typical Thickness**: 500-1500mm
- **Purpose**: Continuous mat foundation

## Common Applications

### Above Grade
```csharp
// Floor slabs
SurfaceMemberType: Slab

// Load-bearing walls
SurfaceMemberType: Wall

// Roof structure
SurfaceMemberType: RoofPanel
```

### Below Grade (Foundations)
```csharp
// Individual column footings
SurfaceMemberType: PadFooting

// Wall foundations
SurfaceMemberType: StripFooting

// Pile caps
SurfaceMemberType: Pilecap

// Mat foundations
SurfaceMemberType: Raft
```

## Related Classes

- **XmiStructuralSurfaceMember**: Uses this enum for type classification
- **EnumValueAttribute**: Provides JSON string values

## See Also

- [XmiStructuralSurfaceMember](../Entities/XmiStructuralSurfaceMember.README.md) - Uses this enum
- [XmiBaseEnum](../Bases/XmiBaseEnum.README.md) - Enum attribute system
