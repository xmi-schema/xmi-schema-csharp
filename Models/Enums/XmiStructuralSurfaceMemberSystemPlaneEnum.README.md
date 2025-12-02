# XmiStructuralSurfaceMemberSystemPlaneEnum

Enumeration defining the reference plane position within a surface member's thickness.

## Overview

`XmiStructuralSurfaceMemberSystemPlaneEnum` specifies where the reference plane is located through the thickness of a surface member. This determines how the surface is offset from its defining nodes.

## Location

`Models/Enums/XmiStructuralSurfaceMemberSystemPlaneEnum.cs`

## Values

| Enum Value | JSON Value | Description |
|------------|-----------|-------------|
| `Bottom` | "Bottom" | Reference at bottom surface (thickness extends upward) |
| `Top` | "Top" | Reference at top surface (thickness extends downward) |
| `Middle` | "Middle" | Reference at mid-thickness (typical for analysis) |
| `Left` | "Left" | Reference at left edge (for vertical surfaces) |
| `Right` | "Right" | Reference at right edge (for vertical surfaces) |
| `Unknown` | "Unknown" | Position not determined |

## Usage Examples

### Floor Slab - Top Reference

```csharp
var slab = new XmiStructuralSurfaceMember(
    id: "SLB001",
    name: "Floor-Slab",
    systemPlane: XmiStructuralSurfaceMemberSystemPlaneEnum.Top,
    thickness: 0.15,  // 150mm extends downward from reference
    // Top surface at defined elevation
    ...
);
```

### Floor Slab - Middle Reference

```csharp
var slab = new XmiStructuralSurfaceMember(
    id: "SLB002",
    name: "Roof-Slab",
    systemPlane: XmiStructuralSurfaceMemberSystemPlaneEnum.Middle,
    thickness: 0.20,  // 200mm, ±100mm from reference
    // Mid-thickness at defined elevation (typical for analysis)
    ...
);
```

### Foundation - Bottom Reference

```csharp
var foundation = new XmiStructuralSurfaceMember(
    id: "FTG001",
    name: "Pad-Footing",
    systemPlane: XmiStructuralSurfaceMemberSystemPlaneEnum.Bottom,
    thickness: 0.60,  // 600mm extends upward from reference
    // Bottom at bearing level
    ...
);
```

### Wall - Middle Reference

```csharp
var wall = new XmiStructuralSurfaceMember(
    id: "WALL001",
    name: "Shear-Wall",
    systemPlane: XmiStructuralSurfaceMemberSystemPlaneEnum.Middle,
    thickness: 0.25,  // 250mm, ±125mm from centerline
    // Centerline at defined position
    ...
);
```

## JSON Serialization

```json
{
  "SystemPlane": "Middle"
}
```

## Visual Reference

### Horizontal Surface (Slab) - Section View

```
Top:    ═══════════════════  ← Reference plane
        ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  ↓ Thickness extends down
        ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓
        ═══════════════════

Middle: ───────────────────
        ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  ↑ Thickness extends up
        ═══════════════════  ← Reference plane
        ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  ↓ Thickness extends down
        ───────────────────

Bottom: ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  ↑ Thickness extends up
        ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓
        ═══════════════════  ← Reference plane
```

### Vertical Surface (Wall) - Plan View

```
Left:   ║ ← Reference
        ║▒▒▒▒▒▒▒▒▒  → Thickness
        ║

Middle:   ║
        ▒▒║▒▒  ← Reference at center
          ║

Right:  ▒▒▒▒▒▒▒▒▒║ ← Reference
                 ║
```

## Common Usage Patterns

### Floor Slabs
```csharp
systemPlane: XmiStructuralSurfaceMemberSystemPlaneEnum.Top
// Top surface matches floor elevation
// Useful for coordination with finishes
```

### Structural Analysis
```csharp
systemPlane: XmiStructuralSurfaceMemberSystemPlaneEnum.Middle
// Reference at neutral axis
// Standard for analysis models
```

### Foundations
```csharp
systemPlane: XmiStructuralSurfaceMemberSystemPlaneEnum.Bottom
// Bottom at bearing elevation
// Matches geotechnical model
```

### Walls
```csharp
systemPlane: XmiStructuralSurfaceMemberSystemPlaneEnum.Middle
// Centerline reference
// Standard for walls in plan
```

## Effect on Z-Coordinates

For a slab at elevation 3.5m with 150mm thickness:

```csharp
// Top reference
systemPlane: Top
// Top surface: 3.5m
// Bottom surface: 3.35m

// Middle reference
systemPlane: Middle
// Top surface: 3.575m
// Mid-surface: 3.5m
// Bottom surface: 3.425m

// Bottom reference
systemPlane: Bottom
// Top surface: 3.65m
// Bottom surface: 3.5m
```

## Coordination with Other Elements

### Slab + Beam Integration
```csharp
// Slab with top reference at 3.5m
slab.SystemPlane = Top
slab.Elevation = 3.5

// Beam with top reference at 3.5m
beam.SystemLine = TopMiddle
beam.Elevation = 3.5

// Both elements align at top
```

## Related Classes

- **XmiStructuralSurfaceMember**: Uses this enum for system plane definition
- **EnumValueAttribute**: Provides JSON string values

## See Also

- [XmiStructuralSurfaceMember](../Entities/XmiStructuralSurfaceMember.README.md) - Uses this enum
- [XmiStructuralCurveMemberSystemLineEnum](XmiStructuralCurveMemberSystemLineEnum.README.md) - Similar concept for curve members
- [XmiBaseEnum](../Bases/XmiBaseEnum.README.md) - Enum attribute system
