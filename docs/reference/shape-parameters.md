---
layout: default
title: Shape Parameters
---

# XmiShapeEnum Parameter Reference

`XmiCrossSection.Parameters` stores the shape inputs through strongly typed classes (e.g., `RectangularShapeParameters`, `IShapeParameters`). Each class serializes into a dictionary so every symbol (H, B, T, etc.) is explicitly paired with its numeric value. This ensures consumers can reason about the payload without tracking array indexes.

## Example

Example for a rectangular column:

```json
{
  "Shape": "Rectangular",
  "Parameters": {
    "H": 0.4,
    "B": 0.4
  }
}
```

## Usage in Code

```csharp
var crossSection = new XmiCrossSection(
    "sec-1",
    "Rectangular 400x400",
    "ifc-guid",
    "SEC-1",
    "Concrete column section",
    XmiShapeEnum.Rectangular,
    new RectangularShapeParameters(0.4, 0.4),  // H, B
    0.16,      // Area
    0.002,     // Torsional constant
    0.003,     // Moment of inertia Y
    0.01,      // Moment of inertia Z
    0.02,      // Section modulus Y
    0.0005,    // Section modulus Z
    0.0006,    // Radius of gyration Y
    0.0007,    // Radius of gyration Z
    0.0008     // Warping constant
);
```

## Parameter Reference

The table below lists all parameter keys for each `XmiShapeEnum`. Values are numeric magnitudes (double precision in code, JSON numbers on export).

| Shape | Parameter Keys | Notes / Alternate Representations |
| --- | --- | --- |
| `Circular` | D | Diameter |
| `Rectangular` | H, B | Height, Width |
| `LShape` | H, B, T, t | Height, Width, Flange thickness, Web thickness |
| `TShape` | H, B, T, t | Height, Width, Flange thickness, Web thickness |
| | | **Alternate:** d, B, T, t, r |
| `LInverted` | H, B, T, t | Height, Width, Flange thickness, Web thickness |
| `TInverted` | H, B, T, t | Height, Width, Flange thickness, Web thickness |
| `CShape` | H, B, T1, T2, t | Height, Width, Flange thickness 1, Flange thickness 2, Web thickness |
| `Elbow` | B1, B2, T, a | Width 1, Width 2, Thickness, Angle |
| `Trapezium` | H, BTop, BBot | Height, Top width, Bottom width |
| `Parallelogram` | B, L, a | Width, Length, Angle |
| `Polygon` | N, R | Number of sides, Radius |
| `IShape` | D, B, T, t, r | Depth, Flange width, Flange thickness, Web thickness, Fillet radius |
| `CircularHollow` | D, t | Outside diameter, Wall thickness |
| `SquareHollow` | D, t | Outside dimension, Wall thickness |
| `RectangularHollow` | D, B, t | Depth, Width, Wall thickness |
| `TaperedFlangeChannel` | D, B, T, t | Depth, Flange width, Flange thickness, Web thickness |
| `ParallelFlangeChannel` | D, B, T, t, r1 | Depth, Flange width, Flange thickness, Web thickness, Root radius |
| `PlainChannel` | D, B, t | Depth, Flange width, Thickness |
| `LippedChannel` | D, B, C, t, r1, r2 | Depth, Flange width, Lip length, Thickness, Root radius 1, Root radius 2 |
| `ZPurlin` | D, E, F, L, t | Depth, Top flange, Bottom flange, Lip, Thickness |
| | | **Constraint:** E > F |
| `EqualAngle` | A, t, r1, r2 | Leg length, Thickness, Root radius, Toe radius |
| | | **Constraint:** r1 > r2 |
| `UnequalAngle` | A, B, t, r1, r2 | Leg length A, Leg length B, Thickness, Root radius, Toe radius |
| `FlatBar` | B, t | Width, Thickness |
| `SquareBar` | a | Side length |
| `DeformedBar` | D | Nominal diameter |
| `RoundBar` | D | Diameter |
| `Others` | (any) | Provide any key/value pairs required by the authoring tool |
| `Unknown` | – | Reserved for placeholder sections |

## Creating Shape Parameters

Each shape has a corresponding parameter class:

```csharp
// Rectangular section
var rectParams = new RectangularShapeParameters(height: 0.4, width: 0.4);

// I-Shape section
var iParams = new IShapeParameters(
    depth: 0.31,
    flangeWidth: 0.165,
    flangeThickness: 0.0094,
    webThickness: 0.00635,
    filletRadius: 0.0095
);

// Circular hollow section
var circHollowParams = new CircularHollowShapeParameters(
    diameter: 0.2,
    wallThickness: 0.01
);
```

## Serialization

When serializing to JSON via `XmiManager`, parameter dictionaries are emitted under the `Parameters` property:

```json
{
  "Id": "sec-1",
  "Name": "W12x26",
  "Shape": "IShape",
  "Parameters": {
    "D": 0.31,
    "B": 0.165,
    "T": 0.0094,
    "t": 0.00635,
    "r": 0.0095
  },
  "Area": 0.00497,
  "TorsionalConstant": 0.0000234
}
```

## Custom Shapes

For custom or uncommon shapes, use `XmiShapeEnum.Others` and provide any key/value pairs:

```csharp
var customParams = new Dictionary<string, double>
{
    ["CustomParam1"] = 0.5,
    ["CustomParam2"] = 1.2,
    ["SpecialDimension"] = 0.75
};

// Note: You may need to create a custom parameter class
// or use the generic parameter dictionary approach
```

## Notes

- All dimensional values should use consistent units (typically meters)
- Parameter keys are case-sensitive
- The parameter dictionary mirrors the content of the official Cross Model Information specification
- Some shapes have alternate representations (e.g., T-Shape can use different parameter sets)
- Constraints noted in the table must be satisfied for valid geometry

[Back to Documentation Home](../)
