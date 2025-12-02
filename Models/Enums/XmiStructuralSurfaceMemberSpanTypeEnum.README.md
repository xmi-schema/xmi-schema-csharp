# XmiStructuralSurfaceMemberSpanTypeEnum

Enumeration defining the spanning behavior of surface members.

## Overview

`XmiStructuralSurfaceMemberSpanTypeEnum` classifies how a surface member spans between its supports, which significantly affects its structural behavior and design.

## Location

`Models/Enums/XmiStructuralSurfaceMemberSpanTypeEnum.cs`

## Values

| Enum Value | JSON Value | Description |
|------------|-----------|-------------|
| `OneWay` | "OneWay" | Spans primarily in one direction |
| `TwoWay` | "TwoWay" | Spans in both directions |
| `Unknown` | "Unknown" | Span type not determined |

## Usage Examples

### One-Way Slab

```csharp
var oneWaySlab = new XmiStructuralSurfaceMember(
    id: "SLB001",
    name: "One-Way-Slab",
    surfaceMemberType: XmiStructuralSurfaceMemberTypeEnum.Slab,
    spanType: XmiStructuralSurfaceMemberSpanTypeEnum.OneWay,
    // Spans between parallel beams
    // Reinforcement primarily in spanning direction
    ...
);
```

### Two-Way Slab

```csharp
var twoWaySlab = new XmiStructuralSurfaceMember(
    id: "SLB002",
    name: "Two-Way-Slab",
    surfaceMemberType: XmiStructuralSurfaceMemberTypeEnum.Slab,
    spanType: XmiStructuralSurfaceMemberSpanTypeEnum.TwoWay,
    // Supported on all four sides
    // Load distributed in both directions
    ...
);
```

## JSON Serialization

```json
{
  "SpanType": "OneWay"
}
```

## Span Type Characteristics

### One-Way Spanning

**Definition**: Load is primarily carried in one direction to parallel supports.

**Typical Conditions**:
- Long/short ratio > 2.0
- Supported on two opposite edges
- Continuous over parallel beams or walls

**Structural Behavior**:
- Bending primarily in one direction
- Main reinforcement in spanning direction
- Distribution reinforcement perpendicular
- Acts similar to a series of adjacent beams

**Example Geometry**:
```
Support ═══════════════════════════ Support
        ║                         ║
        ║    One-Way Span →       ║
        ║                         ║
Wall    ═══════════════════════════ Wall
```

**Applications**:
- Parking garage slabs
- Floor slabs with closely-spaced beams
- Corridor slabs
- Bridge decks

### Two-Way Spanning

**Definition**: Load is carried in both horizontal directions to supports on all sides.

**Typical Conditions**:
- Long/short ratio ≤ 2.0
- Supported on all four edges (or three)
- Column-supported flat slabs

**Structural Behavior**:
- Bending in both X and Y directions
- Reinforcement required in both directions
- More efficient use of material
- Load distribution more complex

**Example Geometry**:
```
Beam ═══════════════════════════ Beam
     ║                         ║
     ║    Two-Way Spanning     ║  ← Load to sides
     ║         ↓↓↓             ║
     ║    Load to top/bottom   ║
Beam ═══════════════════════════ Beam
```

**Applications**:
- Square or nearly square bays
- Flat plate slabs (column-supported)
- Waffle slabs
- Residential floor slabs

## Design Implications

### One-Way Design

```csharp
// One-way slab
// Design strip per meter width as beam
// Moment: M = wL²/8 (for simple span)
// Main reinforcement in span direction
// Temperature/shrinkage reinforcement perpendicular

spanType: XmiStructuralSurfaceMemberSpanTypeEnum.OneWay
```

**Reinforcement Pattern**:
```
Main bars (Spanning Direction):
║ ║ ║ ║ ║ ║ ║ ║ ║ ║

Distribution bars (Perpendicular):
════════════════════
```

### Two-Way Design

```csharp
// Two-way slab
// Analyze for moments in both directions
// Yield-line or finite element analysis
// Reinforcement in both X and Y directions

spanType: XmiStructuralSurfaceMemberSpanTypeEnum.TwoWay
```

**Reinforcement Pattern**:
```
Grid pattern:
║ ║ ║ ║ ║ ║ ║ ║
══════════════════
║ ║ ║ ║ ║ ║ ║ ║
══════════════════
```

## Determining Span Type

### Length-to-Width Ratio Method

```csharp
double length = 8.0;  // meters
double width = 3.0;   // meters
double ratio = length / width;  // 2.67

XmiStructuralSurfaceMemberSpanTypeEnum spanType;

if (ratio > 2.0)
{
    spanType = XmiStructuralSurfaceMemberSpanTypeEnum.OneWay;
}
else
{
    spanType = XmiStructuralSurfaceMemberSpanTypeEnum.TwoWay;
}
```

### Support Configuration

```csharp
// Supported on two opposite edges → One-Way
// Supported on three or four edges → Two-Way
// Column-supported (no beams) → Two-Way
```

## Common Configurations

### One-Way Slabs
- Parking garage floor slabs (spans between beams)
- Bridge decks (spans between girders)
- Precast plank systems

### Two-Way Slabs
- Residential building floors
- Office building floors (typical bays)
- Flat plate systems
- Waffle slab systems

## Related Classes

- **XmiStructuralSurfaceMember**: Uses this enum for span behavior classification
- **EnumValueAttribute**: Provides JSON string values

## See Also

- [XmiStructuralSurfaceMember](../Entities/XmiStructuralSurfaceMember.README.md) - Uses this enum
- [XmiStructuralSurfaceMemberTypeEnum](XmiStructuralSurfaceMemberTypeEnum.README.md) - Surface member types
- [XmiBaseEnum](../Bases/XmiBaseEnum.README.md) - Enum attribute system
