# XmiStructuralCurveMemberTypeEnum

Enumeration defining types of structural curve members (1D elements).

## Overview

`XmiStructuralCurveMemberTypeEnum` classifies linear structural members by their primary structural function in a building or structure.

## Location

`Models/Enums/XmiStructuralCurveMemberTypeEnum.cs`

## Values

| Enum Value | JSON Value | Description |
|------------|-----------|-------------|
| `Beam` | "Beam" | Horizontal load-bearing member |
| `Column` | "Column" | Vertical load-bearing member |
| `Bracing` | "Bracing" | Diagonal bracing element for lateral stability |
| `Other` | "Other" | Other types of curve members |
| `Unknown` | "Unknown" | Type not determined or specified |

## Usage Example

```csharp
// Create a beam
var beam = new XmiStructuralCurveMember(
    id: "B001",
    name: "Beam-1",
    curvememberType: XmiStructuralCurveMemberTypeEnum.Beam,
    ...
);

// Create a column
var column = new XmiStructuralCurveMember(
    id: "C001",
    name: "Column-1",
    curvememberType: XmiStructuralCurveMemberTypeEnum.Column,
    ...
);

// Create diagonal bracing
var brace = new XmiStructuralCurveMember(
    id: "BR001",
    name: "Brace-1",
    curvememberType: XmiStructuralCurveMemberTypeEnum.Bracing,
    ...
);
```

## JSON Serialization

The enum uses `EnumValueAttribute` for JSON export:

```json
{
  "CurvememberType": "Beam"
}
```

## Member Type Characteristics

### Beam
- **Orientation**: Typically horizontal or near-horizontal
- **Primary Load**: Bending from gravity and lateral loads
- **Common Sections**: Rectangular, I-beam, T-beam

### Column
- **Orientation**: Typically vertical
- **Primary Load**: Axial compression (with some bending)
- **Common Sections**: Circular, rectangular, I-section

### Bracing
- **Orientation**: Typically diagonal
- **Primary Load**: Axial tension/compression
- **Common Sections**: Angles, tubes, rods
- **Purpose**: Lateral stability, wind/seismic resistance

## Related Classes

- **XmiStructuralCurveMember**: Uses this enum for member classification
- **EnumValueAttribute**: Provides JSON string values

## See Also

- [XmiStructuralCurveMember](../Entities/XmiStructuralCurveMember.README.md) - Uses this enum
- [XmiBaseEnum](../Bases/XmiBaseEnum.README.md) - Enum attribute system
