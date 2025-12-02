# XmiStructuralCurveMemberSystemLineEnum

Enumeration defining the reference line position within a curve member's cross-section.

## Overview

`XmiStructuralCurveMemberSystemLineEnum` specifies where the reference line (system line) is located within the cross-section of a structural member. This affects how the member geometry is positioned relative to its defining nodes.

## Location

`Models/Enums/XmiStructuralCurveMemberSystemLineEnum.cs`

## Values

| Enum Value | JSON Value | Vertical Position | Horizontal Position |
|------------|-----------|------------------|---------------------|
| `TopMiddle` | "TopMiddle" | Top of section | Centerline |
| `TopLeft` | "TopLeft" | Top of section | Left edge |
| `TopRight` | "TopRight" | Top of section | Right edge |
| `MiddleMiddle` | "MiddleMiddle" | Centroid | Centerline |
| `MiddleLeft` | "MiddleLeft" | Centroid | Left edge |
| `MiddleRight` | "MiddleRight" | Centroid | Right edge |
| `BottomLeft` | "BottomLeft" | Bottom of section | Left edge |
| `BottomMiddle` | "BottomMiddle" | Bottom of section | Centerline |
| `BottomRight` | "BottomRight" | Bottom of section | Right edge |
| `Unknown` | "Unknown" | Position not determined | - |

## Usage Examples

### Beam with Top Reference Line

```csharp
var beam = new XmiStructuralCurveMember(
    id: "B001",
    name: "Beam-1",
    systemLine: XmiStructuralCurveMemberSystemLineEnum.TopMiddle,
    // Reference line at top center of cross-section
    // Useful when beam aligns with top of slab
    ...
);
```

### Beam with Centroid Reference Line

```csharp
var beam = new XmiStructuralCurveMember(
    id: "B002",
    name: "Beam-2",
    systemLine: XmiStructuralCurveMemberSystemLineEnum.MiddleMiddle,
    // Reference line at centroid (most common for analysis)
    ...
);
```

### Column with Bottom Reference Line

```csharp
var column = new XmiStructuralCurveMember(
    id: "C001",
    name: "Column-1",
    systemLine: XmiStructuralCurveMemberSystemLineEnum.BottomMiddle,
    // Reference line at bottom center
    // Useful for columns starting from foundation
    ...
);
```

## JSON Serialization

```json
{
  "SystemLine": "MiddleMiddle"
}
```

## Visual Reference

### Cross-Section System Line Positions

```
     TopLeft ─── TopMiddle ─── TopRight
        │            │            │
        │            │            │
  MiddleLeft ─ MiddleMiddle ─ MiddleRight
        │            │            │
        │            │            │
   BottomLeft ─ BottomMiddle ─ BottomRight
```

## Common Usage Patterns

### Analysis Models
```csharp
systemLine: XmiStructuralCurveMemberSystemLineEnum.MiddleMiddle
// Reference at centroid for accurate structural analysis
```

### Beams at Slab Level
```csharp
systemLine: XmiStructuralCurveMemberSystemLineEnum.TopMiddle
// Top of beam aligns with top of slab
```

### Foundation Columns
```csharp
systemLine: XmiStructuralCurveMemberSystemLineEnum.BottomMiddle
// Bottom of column at foundation level
```

### Edge Beams
```csharp
systemLine: XmiStructuralCurveMemberSystemLineEnum.MiddleLeft or MiddleRight
// Beam aligned to building edge
```

## Coordinate System

- **Vertical (First Position)**:
  - Top: Upper face of cross-section
  - Middle: Centroid
  - Bottom: Lower face of cross-section

- **Horizontal (Second Position)**:
  - Left: Left edge of cross-section
  - Middle: Centerline
  - Right: Right edge of cross-section

## Effect on Geometry

The system line determines how the cross-section is positioned:

```csharp
// TopMiddle: Cross-section extends downward from reference line
// MiddleMiddle: Cross-section centered on reference line
// BottomMiddle: Cross-section extends upward from reference line
```

## Related Classes

- **XmiStructuralCurveMember**: Uses this enum for system line definition
- **EnumValueAttribute**: Provides JSON string values

## See Also

- [XmiStructuralCurveMember](../Entities/XmiStructuralCurveMember.README.md) - Uses this enum
- [XmiBaseEnum](../Bases/XmiBaseEnum.README.md) - Enum attribute system
