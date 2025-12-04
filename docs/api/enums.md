---
layout: default
title: Enums
---

# Enums

Enumerations in this folder standardize string values flowing through the Cross Model Information schema. Each enum member is annotated with `EnumValueAttribute`, ensuring serialized payloads use the correct labels (e.g., `"Beam"`, `"Rectangular"`).

## Domain Enums

### XmiBaseEntityDomainEnum

Defines the domain classification for all XMI entities.

**Values:**
- `Physical` - Physical building elements
- `StructuralAnalytical` - Structural analysis model entities
- `Geometry` - Spatial primitives
- `Functional` - Functional/logical elements

## Structural Enums

### XmiStructuralCurveMemberTypeEnum

Classification of curve members.

**Values:**
- `Beam` - Horizontal structural member
- `Column` - Vertical structural member
- `Bracing` - Diagonal bracing member
- `Cable` - Cable element
- `Rigid` - Rigid link
- `Unknown` - Unclassified member

### XmiStructuralCurveMemberSystemLineEnum

Location of the analytical line relative to the physical profile.

**Values:**
- `TopLeft`, `TopMiddle`, `TopRight`
- `MiddleLeft`, `MiddleMiddle`, `MiddleRight`
- `BottomLeft`, `BottomMiddle`, `BottomRight`
- `Unknown`

### XmiStructuralSurfaceMemberTypeEnum

Slab/wall/panel classifications.

**Values:**
- `Slab` - Horizontal surface
- `Wall` - Vertical surface
- `Panel` - Generic surface panel
- `Shell` - Curved shell element
- `Unknown`

### XmiStructuralSurfaceMemberSpanTypeEnum

Span behavior of surface members.

**Values:**
- `OneWay` - One-way spanning
- `TwoWay` - Two-way spanning
- `Unknown`

### XmiStructuralSurfaceMemberSystemPlaneEnum

Analytical plane location for surface elements.

**Values:**
- `Top` - Top surface
- `Middle` - Mid-plane
- `Bottom` - Bottom surface
- `Unknown`

## Material Enums

### XmiStructuralMaterialTypeEnum

Material categories.

**Values:**
- `Concrete` - Concrete material
- `Steel` - Structural steel
- `Timber` - Wood/timber
- `Masonry` - Brick/block masonry
- `Aluminum` - Aluminum alloy
- `Glass` - Glass material
- `Other` - Other materials
- `Unknown`

## Geometry Enums

### XmiSegmentTypeEnum

Geometry families for `XmiSegment`.

**Values:**
- `Line` - Straight line segment
- `CircularArc` - Circular arc
- `Spline` - Spline curve
- `Parabolic` - Parabolic curve
- `Bezier` - Bezier curve
- `Unknown`

### XmiShapeEnum

Profile shapes for `XmiCrossSection`.

**Values:**
- `Circular` - Circular solid
- `Rectangular` - Rectangular solid
- `LShape` - L-shaped angle
- `TShape` - T-shaped section
- `IShape` - I-beam/wide flange
- `CShape` - C-shaped channel
- `CircularHollow` - Circular hollow section
- `SquareHollow` - Square hollow section
- `RectangularHollow` - Rectangular hollow section
- `TaperedFlangeChannel` - Tapered channel
- `ParallelFlangeChannel` - Parallel flange channel
- `PlainChannel` - Plain channel
- `LippedChannel` - Lipped channel
- `ZPurlin` - Z-shaped purlin
- `EqualAngle` - Equal leg angle
- `UnequalAngle` - Unequal leg angle
- `FlatBar` - Flat bar
- `SquareBar` - Square bar
- `DeformedBar` - Deformed reinforcing bar
- `RoundBar` - Round bar
- `Trapezium` - Trapezoidal section
- `Parallelogram` - Parallelogram section
- `Polygon` - Polygonal section
- `Elbow` - Elbow section
- `LInverted` - Inverted L
- `TInverted` - Inverted T
- `Others` - Custom/undefined
- `Unknown`

## Unit Enums

### XmiUnitEnum

Units assigned to entity attributes.

**Values:**
- `Meter`, `Millimeter`, `Centimeter`, `Inch`, `Foot`
- `SquareMeter`, `SquareMillimeter`
- `CubicMeter`, `CubicMillimeter`
- `Radian`, `Degree`
- `Second`, `Minute`, `Hour`
- `Newton`, `Kilonewton`, `PoundForce`
- `Pascal`, `Kilopascal`, `Megapascal`, `PSI`
- `Kilogram`, `Ton`, `Pound`
- `Unknown`

## Using Enums

### With EnumValueAttribute

```csharp
public enum XmiShapeEnum
{
    [EnumValue("Rectangular")]
    Rectangular,

    [EnumValue("Circular")]
    Circular
}
```

### Parsing from Strings

```csharp
using XmiSchema.Core.Utils;

var shape = ExtensionEnumHelper.FromEnumValue<XmiShapeEnum>("Rectangular");
// Returns: XmiShapeEnum.Rectangular
```

### Serialization

Enums automatically serialize to their `EnumValue` strings when using JSON serialization:

```json
{
  "Shape": "Rectangular",
  "CurveMemberType": "Beam",
  "MaterialType": "Steel"
}
```

## Adding New Enum Values

When adding new enum values:

1. Add the enum member with `[EnumValue("StringValue")]` attribute
2. Update `Utils/ExtensionEnumHelper` tests to verify round-trip conversions
3. Confirm serialized strings align with schema specification
4. Document the new value in this reference

[Back to API Reference](.)
