# XmiShapeEnum

Enumeration defining cross-section shape types for structural members.

## Overview

`XmiShapeEnum` classifies the geometric shape of structural cross-sections, determining how sectional properties are calculated and how the section behaves structurally.

## Location

`Models/Enums/XmiShapeEnum.cs`

## Values

| Enum Value | JSON Value | Description |
|------------|-----------|-------------|
| `Rectangular` | "Rectangular" | Solid rectangular section |
| `Circular` | "Circular" | Solid circular section |
| `LShape` | "L Shape" | Angle section (L-profile) |
| `TShape` | "T Shape" | T-section |
| `CShape` | "C Shape" | Channel section (C-profile) |
| `IShape` | "I Shape" | I-beam or H-section |
| `SquareHollow` | "Square Hollow" | Hollow square tube |
| `RectangularHollow` | "Rectangular Hollow" | Hollow rectangular tube |
| `Others` | "Others" | Other shape types |
| `Unknown` | "Unknown" | Shape not determined |

## Usage Example

```csharp
// Rectangular concrete section
var concretSection = new XmiStructuralCrossSection(
    id: "CS001",
    name: "300x600",
    shape: XmiShapeEnum.Rectangular,
    parameters: new[] { "300", "600" },  // width x height
    ...
);

// I-beam steel section
var steelBeam = new XmiStructuralCrossSection(
    id: "CS002",
    name: "IPE-300",
    shape: XmiShapeEnum.IShape,
    parameters: new[] { "150", "10.7", "300", "7.1" },  // flange width, flange thick, height, web thick
    ...
);

// Circular column
var circularColumn = new XmiStructuralCrossSection(
    id: "CS003",
    name: "CHS-200",
    shape: XmiShapeEnum.Circular,
    parameters: new[] { "200" },  // diameter
    ...
);
```

## JSON Serialization

```json
{
  "Shape": "Rectangular"
}
```

## Shape Parameters

Different shapes require different parameter definitions:

### Rectangular
```csharp
shape: XmiShapeEnum.Rectangular
parameters: ["width", "height"]
```

### Circular
```csharp
shape: XmiShapeEnum.Circular
parameters: ["diameter"]
```

### I-Shape
```csharp
shape: XmiShapeEnum.IShape
parameters: ["flangeWidth", "flangeThickness", "webHeight", "webThickness"]
```

### Square Hollow
```csharp
shape: XmiShapeEnum.SquareHollow
parameters: ["outerWidth", "thickness"]
```

### Rectangular Hollow
```csharp
shape: XmiShapeEnum.RectangularHollow
parameters: ["outerWidth", "outerHeight", "thickness"]
```

## Material-Shape Combinations

### Concrete (typically solid)
- Rectangular
- Circular
- T-Shape (beams)

### Steel (various profiles)
- I-Shape (beams, columns)
- Rectangular Hollow (columns)
- Circular (columns, piles)
- L-Shape (angles, bracing)
- C-Shape (channels, purlins)

### Timber (typically solid)
- Rectangular
- Circular (poles, piles)

## Related Classes

- **XmiStructuralCrossSection**: Uses this enum for shape classification
- **EnumValueAttribute**: Provides JSON string values

## See Also

- [XmiStructuralCrossSection](../Entities/XmiStructuralCrossSection.README.md) - Uses this enum
- [XmiBaseEnum](../Bases/XmiBaseEnum.README.md) - Enum attribute system
