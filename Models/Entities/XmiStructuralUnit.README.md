# XmiStructuralUnit

Represents unit of measurement metadata for entity attributes.

## Overview

`XmiStructuralUnit` provides metadata about the units of measurement used for specific attributes of entities in the model. This enables proper interpretation of numerical values and supports unit conversion when exchanging data between systems with different unit conventions.

## Location

`Models/Entities/XmiStructuralUnit.cs`

## Inheritance

```
XmiBaseEntity → XmiStructuralUnit
```

## Properties

### Entity Reference

- **Entity** (`string`): The entity type name this unit applies to
  - Example: "XmiStructuralMaterial", "XmiStructuralCurveMember"

### Attribute Reference

- **Attribute** (`string`): The specific attribute name within the entity
  - Example: "EModulus", "Length", "Thickness"

### Unit Specification

- **Unit** (`XmiUnitEnum`): The unit of measurement
  - Example: Metric, Imperial, SI units

## Constructor

```csharp
public XmiStructuralUnit(
    string id,
    string name,
    string ifcguid,
    string nativeId,
    string description,
    string entity,
    string attribute,
    XmiUnitEnum unit
)
```

## Usage Examples

### Material Property Units

```csharp
// Define units for material properties

// Young's modulus in MPa
var eModulusUnit = new XmiStructuralUnit(
    id: "UNIT_001",
    name: "E-Modulus-Unit",
    ifcguid: "1Ms3ul$wN4pQR5tUvUwYzA",
    nativeId: "UnitDef_EModulus",
    description: "Unit for Young's modulus",
    entity: "XmiStructuralMaterial",
    attribute: "EModulus",
    unit: XmiUnitEnum.Megapascal  // MPa
);

// Unit weight in kN/m³
var unitWeightUnit = new XmiStructuralUnit(
    id: "UNIT_002",
    name: "UnitWeight-Unit",
    ifcguid: "2Nt4vm$xO5qRS6uVwVxZaB",
    nativeId: "UnitDef_UnitWeight",
    description: "Unit for material unit weight",
    entity: "XmiStructuralMaterial",
    attribute: "UnitWeight",
    unit: XmiUnitEnum.KilonewtonPerCubicMeter  // kN/m³
);
```

### Geometric Property Units

```csharp
// Length units for members
var lengthUnit = new XmiStructuralUnit(
    id: "UNIT_003",
    name: "Member-Length-Unit",
    entity: "XmiStructuralCurveMember",
    attribute: "Length",
    unit: XmiUnitEnum.Meter,  // meters
    ...
);

// Thickness units for slabs
var thicknessUnit = new XmiStructuralUnit(
    id: "UNIT_004",
    name: "Slab-Thickness-Unit",
    entity: "XmiStructuralSurfaceMember",
    attribute: "Thickness",
    unit: XmiUnitEnum.Millimeter,  // mm
    ...
);

// Area units for cross-sections
var areaUnit = new XmiStructuralUnit(
    id: "UNIT_005",
    name: "CrossSection-Area-Unit",
    entity: "XmiStructuralCrossSection",
    attribute: "Area",
    unit: XmiUnitEnum.SquareMeter,  // m²
    ...
);
```

### Complete Unit Definition Set

```csharp
var builder = new XmiSchemaModelBuilder();

// Define all material units
var materialUnits = new List<XmiStructuralUnit>
{
    new XmiStructuralUnit("U_MAT_E", "Material-EModulus", ..., "XmiStructuralMaterial", "EModulus", XmiUnitEnum.Megapascal),
    new XmiStructuralUnit("U_MAT_G", "Material-GModulus", ..., "XmiStructuralMaterial", "GModulus", XmiUnitEnum.Megapascal),
    new XmiStructuralUnit("U_MAT_UW", "Material-UnitWeight", ..., "XmiStructuralMaterial", "UnitWeight", XmiUnitEnum.KilonewtonPerCubicMeter),
    new XmiStructuralUnit("U_MAT_GR", "Material-Grade", ..., "XmiStructuralMaterial", "Grade", XmiUnitEnum.Megapascal)
};

// Define geometric units
var geometricUnits = new List<XmiStructuralUnit>
{
    new XmiStructuralUnit("U_LEN", "Length", ..., "XmiStructuralCurveMember", "Length", XmiUnitEnum.Meter),
    new XmiStructuralUnit("U_THK", "Thickness", ..., "XmiStructuralSurfaceMember", "Thickness", XmiUnitEnum.Meter),
    new XmiStructuralUnit("U_AREA", "Area", ..., "XmiStructuralCrossSection", "Area", XmiUnitEnum.SquareMeter)
};

// Add all units to model
builder.AddEntities(materialUnits);
builder.AddEntities(geometricUnits);

var model = builder.BuildModel();
```

## JSON Export Example

```json
{
  "XmiStructuralUnit": {
    "ID": "UNIT_001",
    "Name": "E-Modulus-Unit",
    "IFCGUID": "1Ms3ul$wN4pQR5tUvUwYzA",
    "NativeId": "UnitDef_EModulus",
    "Description": "Unit for Young's modulus",
    "EntityType": "XmiStructuralUnit",
    "Entity": "XmiStructuralMaterial",
    "Attribute": "EModulus",
    "Unit": "MPa"
  }
}
```

## Common Unit Definitions

### Length Units
```csharp
unit: XmiUnitEnum.Meter        // m
unit: XmiUnitEnum.Millimeter   // mm
unit: XmiUnitEnum.Foot         // ft
unit: XmiUnitEnum.Inch         // in
```

### Force Units
```csharp
unit: XmiUnitEnum.Newton       // N
unit: XmiUnitEnum.Kilonewton   // kN
unit: XmiUnitEnum.Pound        // lb
unit: XmiUnitEnum.Kip          // kip
```

### Stress/Pressure Units
```csharp
unit: XmiUnitEnum.Pascal       // Pa
unit: XmiUnitEnum.Megapascal   // MPa
unit: XmiUnitEnum.PSI          // psi
unit: XmiUnitEnum.KSI          // ksi
```

### Area Units
```csharp
unit: XmiUnitEnum.SquareMeter      // m²
unit: XmiUnitEnum.SquareMillimeter // mm²
unit: XmiUnitEnum.SquareFoot       // ft²
unit: XmiUnitEnum.SquareInch       // in²
```

## Use Cases

### 1. Data Exchange

When exporting/importing models between systems:
```csharp
// System A uses metric units
var materialA = new XmiStructuralMaterial(..., eModulus: 210000.0, ...);
var unitA = new XmiStructuralUnit(..., "XmiStructuralMaterial", "EModulus", XmiUnitEnum.Megapascal);

// System B can read unit definition and convert if needed
// 210000 MPa = 210 GPa = 30457 ksi
```

### 2. Display and Reporting

```csharp
// Query unit for display purposes
var unit = GetUnitForAttribute("XmiStructuralMaterial", "EModulus");
Console.WriteLine($"Young's Modulus: {material.EModulus} {unit.Unit}");
// Output: "Young's Modulus: 210000 MPa"
```

### 3. Validation

```csharp
// Ensure values are in expected ranges for given units
if (unit.Unit == XmiUnitEnum.Megapascal)
{
    if (material.EModulus < 1000 || material.EModulus > 500000)
    {
        Console.WriteLine("Warning: E-Modulus value seems out of range for MPa");
    }
}
```

### 4. Unit Consistency Checking

```csharp
// Verify all properties use consistent unit system
var units = model.Entities.OfType<XmiStructuralUnit>();

var metricUnits = units.Where(u => IsMetric(u.Unit));
var imperialUnits = units.Where(u => IsImperial(u.Unit));

if (metricUnits.Any() && imperialUnits.Any())
{
    Console.WriteLine("Warning: Mixed unit systems detected");
}
```

## Best Practices

### 1. Define Units at Model Start
```csharp
// Define all units before adding entities
var builder = new XmiSchemaModelBuilder();
DefineAllUnits(builder);
AddEntities(builder);
```

### 2. Use Consistent Unit System
```csharp
// Prefer all metric or all imperial
// Metric: m, kN, MPa, kg
// Imperial: ft, kip, ksi, lb
```

### 3. Document Assumptions
```csharp
var unit = new XmiStructuralUnit(
    ...,
    description: "Stress in MPa - assumes all material properties in SI units",
    ...
);
```

## Design Patterns

### Metadata Pattern
Provides metadata about data values without changing the values themselves.

### Registry Pattern
Acts as a registry mapping entity/attribute pairs to units.

## Related Classes

- **XmiBaseEntity**: Base class
- **XmiUnitEnum**: Unit enumeration
- **XmiStructuralMaterial**: Uses units for material properties
- **XmiStructuralCrossSection**: Uses units for geometric properties

## See Also

- [XmiStructuralMaterial](XmiStructuralMaterial.README.md) - Uses unit definitions
- [XmiStructuralCrossSection](XmiStructuralCrossSection.README.md) - Uses unit definitions
- [XmiBaseEntity](../Bases/XmiBaseEntity.README.md) - Base class
