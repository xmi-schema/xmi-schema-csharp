# XmiUnitEnum

Enumeration defining units of measurement for physical quantities.

## Overview

`XmiUnitEnum` specifies the units of measurement used for various attributes in the XMI schema, enabling proper interpretation and conversion of numerical values.

## Location

`Models/Enums/XmiUnitEnum.cs`

## Values

| Enum Value | JSON Value | Quantity Type | Description |
|------------|-----------|---------------|-------------|
| `Meter3` | "m^3" | Volume | Cubic meters |
| `Meter2` | "m^2" | Area | Square meters |
| `Meter` | "m" | Length | Meters |
| `Meter4` | "m^4" | Second moment of area | Meters to fourth power |
| `Millimeter4` | "mm^4" | Second moment of area | Millimeters to fourth power |
| `Millimeter` | "mm" | Length | Millimeters |
| `Centimeter` | "cm" | Length | Centimeters |
| `Millimeter3` | "mm^3" | Section modulus | Cubic millimeters |
| `Millimeter2` | "mm^2" | Area | Square millimeters |
| `Second` | "sec" | Time | Seconds |
| `Unknown` | "Unknown" | - | Unit not determined |

## Usage Examples

### Material Properties

```csharp
// Young's modulus in MPa
var eModulusUnit = new XmiStructuralUnit(
    id: "UNIT_E",
    name: "E-Modulus-Unit",
    entity: "XmiStructuralMaterial",
    attribute: "EModulus",
    unit: XmiUnitEnum.Megapascal  // Note: Not in current enum list
);

// Unit weight in kN/m³
var unitWeightUnit = new XmiStructuralUnit(
    id: "UNIT_UW",
    name: "UnitWeight-Unit",
    entity: "XmiStructuralMaterial",
    attribute: "UnitWeight",
    unit: XmiUnitEnum.KilonewtonPerCubicMeter  // Note: Not in current enum list
);
```

### Geometric Properties

```csharp
// Length in meters
var lengthUnit = new XmiStructuralUnit(
    entity: "XmiStructuralCurveMember",
    attribute: "Length",
    unit: XmiUnitEnum.Meter,  // m
    ...
);

// Cross-sectional area in square meters
var areaUnit = new XmiStructuralUnit(
    entity: "XmiStructuralCrossSection",
    attribute: "Area",
    unit: XmiUnitEnum.Meter2,  // m²
    ...
);

// Second moment of area in m⁴
var inertiaUnit = new XmiStructuralUnit(
    entity: "XmiStructuralCrossSection",
    attribute: "SecondMomentOfAreaXAxis",
    unit: XmiUnitEnum.Meter4,  // m⁴
    ...
);
```

### Dimension Conversion

```csharp
// Cross-section dimensions in millimeters
var widthUnit = new XmiStructuralUnit(
    entity: "XmiStructuralCrossSection",
    attribute: "Parameters[0]",  // Width
    unit: XmiUnitEnum.Millimeter,  // mm
    ...
);
```

## JSON Serialization

```json
{
  "Unit": "m^2"
}
```

## Common Unit Combinations

### Length Measurements
```csharp
unit: XmiUnitEnum.Meter        // 1 m
unit: XmiUnitEnum.Centimeter   // 0.01 m
unit: XmiUnitEnum.Millimeter   // 0.001 m
```

### Area Measurements
```csharp
unit: XmiUnitEnum.Meter2       // 1 m²
unit: XmiUnitEnum.Millimeter2  // 0.000001 m²
```

### Volume Measurements
```csharp
unit: XmiUnitEnum.Meter3       // 1 m³
unit: XmiUnitEnum.Millimeter3  // 1×10⁻⁹ m³
```

### Second Moment of Area
```csharp
unit: XmiUnitEnum.Meter4       // 1 m⁴
unit: XmiUnitEnum.Millimeter4  // 1×10⁻¹² m⁴
```

## Unit Conversions

### Length
```
1 m = 100 cm = 1000 mm
```

### Area
```
1 m² = 10,000 cm² = 1,000,000 mm²
```

### Volume
```
1 m³ = 1,000,000 cm³ = 1,000,000,000 mm³
```

### Second Moment of Area (Inertia)
```
1 m⁴ = 100,000,000 cm⁴ = 1,000,000,000,000 mm⁴
```

## Typical Applications

### Structural Geometry (Meters)
```csharp
// Member lengths
Length: Meter

// Span dimensions
Span: Meter

// Coordinates
X, Y, Z: Meter
```

### Cross-Section Properties (Mixed)
```csharp
// Overall dimensions
Width, Height: Millimeter or Meter

// Area
Area: Meter2

// Moments of inertia
Ixx, Iyy: Meter4 or Millimeter4

// Section modulus
Sx, Sy: Millimeter3 or Meter3
```

### Material Properties (SI Units)
```csharp
// Moduli (not in current enum)
E-Modulus: Megapascal (MPa)
G-Modulus: Megapascal (MPa)

// Density (not in current enum)
UnitWeight: KilonewtonPerCubicMeter (kN/m³)
```

## Note on Missing Units

**The current enum implementation is limited.** For a complete structural analysis library, consider adding:

- Force units: N, kN, kip, lb
- Stress/Pressure units: Pa, kPa, MPa, GPa, psi, ksi
- Density units: kg/m³, kN/m³, lb/ft³
- Angle units: radians, degrees

## Related Classes

- **XmiStructuralUnit**: Uses this enum for unit specification
- **EnumValueAttribute**: Provides JSON string values

## See Also

- [XmiStructuralUnit](../Entities/XmiStructuralUnit.README.md) - Unit metadata entity
- [XmiBaseEnum](../Bases/XmiBaseEnum.README.md) - Enum attribute system
