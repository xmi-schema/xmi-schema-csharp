# XmiUnit

Represents unit definitions for attribute conversion in XmiSchema library.

## Purpose

`XmiUnit` defines measurement units for entity attributes, enabling proper unit conversion between different measurement systems and ensuring data consistency across structural models.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `ID` | `string` | Yes | Unique identifier for unit |
| `Name` | `string` | Yes | Human-readable unit name (e.g., "Length-Millimeter") |
| `ifcGuid` | `string` | No | IFC GUID for BIM interoperability |
| `NativeId` | `string` | No | Original identifier from source system |
| `Description` | `string` | No | Detailed description of unit |
| `EntityName` | `string` | Yes | Always "XmiUnit" |
| `Domain` | `XmiBaseEntityDomainEnum` | Yes | Always "Functional" |
| `UnitType` | `XmiUnitEnum` | Yes | Unit category (Length, Force, Stress, etc.) |

## Unit Types

Supported unit types via `XmiUnitEnum`:

| Unit Type | Description | Common Units |
|-----------|-------------|----------------|
| **Length** | Distance measurements | mm, m, ft, in |
| **Area** | Area measurements | mm², m², ft² |
| **Volume** | Volume measurements | mm³, m³ |
| **Force** | Force measurements | N, kN, lb, kip |
| **Moment** | Bending moment measurements | N·mm, kN·m, ft·kip |
| **Stress** | Stress measurements | MPa, psi, ksf |
| **Mass** | Mass measurements | kg, ton, lb |
| **Angle** | Angular measurements | deg, rad |

## Common Unit Systems

### Metric (SI) Units

| Quantity | Unit | Symbol | Conversion |
|----------|------|---------|------------|
| Length | Millimeter | mm | 1 mm = 0.001 m |
| Length | Meter | m | 1 m = 1000 mm |
| Force | Kilonewton | kN | 1 kN = 1000 N |
| Stress | Megapascal | MPa | 1 MPa = 1 N/mm² |
| Moment | Kilonewton-meter | kN·m | 1 kN·m = 1,000,000 N·mm |

### Imperial (US Customary) Units

| Quantity | Unit | Symbol | Conversion to SI |
|----------|------|---------|-----------------|
| Length | Inch | in | 1 in = 25.4 mm |
| Length | Foot | ft | 1 ft = 304.8 mm |
| Force | Pound | lb | 1 lb = 4.448 N |
| Force | Kip | kip | 1 kip = 4448 N |
| Stress | Pounds per sq. inch | psi | 1 psi = 0.00689 MPa |
| Moment | Foot-kip | ft·kip | 1 ft·kip = 1.356 kN·m |

## Usage Example

```csharp
// Define length unit (millimeter)
var mmUnit = new XmiUnit(
    id: "unit-length-mm",
    name: "Length-Millimeter",
    ifcGuid: "",
    nativeId: "LENGTH_MM",
    description: "Length measurement in millimeters (SI)",
    unitType: XmiUnitEnum.Length
);

// Define force unit (kilonewton)
var kNUnit = new XmiUnit(
    id: "unit-force-kn",
    name: "Force-Kilonewton",
    ifcGuid: "",
    nativeId: "FORCE_KN",
    description: "Force measurement in kilonewtons (SI)",
    unitType: XmiUnitEnum.Force
);

// Define stress unit (megapascal)
var mpaUnit = new XmiUnit(
    id: "unit-stress-mpa",
    name: "Stress-Megapascal",
    ifcGuid: "",
    nativeId: "STRESS_MPA",
    description: "Stress measurement in megapascals (SI)",
    unitType: XmiUnitEnum.Stress
);

// Define moment unit (kilonewton-meter)
var kNmUnit = new XmiUnit(
    id: "unit-moment-knm",
    name: "Moment-KilonewtonMeter",
    ifcGuid: "",
    nativeId: "MOMENT_KNM",
    description: "Moment measurement in kilonewton-meters (SI)",
    unitType: XmiUnitEnum.Moment
);
```

## Unit Conversion

### Length Conversion

```csharp
// Convert inches to millimeters
double lengthInInches = 12.0;        // 1 foot
double lengthInMm = lengthInInches * 25.4;  // 304.8 mm
```

### Force Conversion

```csharp
// Convert kips to kilonewtons
double forceInKips = 10.0;           // 10 kips
double forceInKN = forceInKips * 4.448;  // 44.48 kN
```

### Stress Conversion

```csharp
// Convert psi to MPa
double stressInPsi = 29000.0;       // 29 ksi
double stressInMPa = stressInPsi * 0.006894;  // 199.93 MPa
```

## Unit Assignment to Attributes

### Direct Property Values

Some entity properties implicitly use specific units:

| Entity Property | Unit | Example Value |
|---------------|-------|---------------|
| `XmiPoint3D.X`, `Y`, `Z` | mm | (1000.0, 2000.0, 3000.0) mm |
| `XmiMaterial.UnitWeight` | kN/m³ | 24.0 kN/m³ |
| `XmiCrossSection.Area` | mm² | 240000 mm² |
| `XmiCrossSection.Inertia` | mm⁴ | 7200000000 mm⁴ |

### Unit Definitions

Define unit expectations in model:

```csharp
// Define expected units for model
var lengthUnit = new XmiUnit("unit-len", "Length-Millimeter", ...);
var forceUnit = new XmiUnit("unit-force", "Force-Kilonewton", ...);
var stressUnit = new XmiUnit("unit-stress", "Stress-Megapascal", ...);

// Add unit definitions to model
model.Entities.AddRange(new List<XmiBaseEntity>
{
    lengthUnit,
    forceUnit,
    stressUnit
});
```

## Standard Units by Region

### Europe (Metric/SI)
- **Length**: mm, m
- **Force**: kN, N
- **Stress**: MPa
- **Moment**: kN·m
- **Mass**: kg, ton

### United States (Imperial)
- **Length**: in, ft
- **Force**: lb, kip
- **Stress**: psi, ksi
- **Moment**: ft·kip, in·kip
- **Mass**: lb

### Australia/Asia (Mixed)
- **Length**: mm
- **Force**: kN
- **Stress**: MPa
- **Mass**: tonne (metric ton)

## Best Practices

1. **Use consistent units** throughout the model (all SI or all imperial)
2. **Define unit expectations** explicitly in model metadata
3. **Document unit assumptions** in description fields
4. **Perform unit conversions** at data import/export boundaries
5. **Test unit conversions** with known reference values
6. **Use standard unit names** (e.g., "Length-Millimeter")
7. **Include unit in property names** when documentation (e.g., "Length_mm")

## Unit Conversion Factors

### Common Conversions

| From | To | Multiply By |
|-------|-----|-------------|
| in → mm | 25.4 |
| ft → mm | 304.8 |
| lb → N | 4.448 |
| kip → N | 4448 |
| psi → MPa | 0.006894 |
| ksi → MPa | 6.894 |
| ft·kip → kN·m | 1.356 |
| in·kip → kN·m | 0.113 |

## BIM Interoperability

### IFC Unit Definitions

`XmiUnit` with `ifcGuid` enables IFC integration:

- IFC uses explicit unit definitions (`IfcSIUnit`, `IfcConversionBasedUnit`)
- Different IFC tools may use different unit preferences
- Unit definitions ensure correct interpretation across systems

### Example IFC Integration

```csharp
var lengthUnit = new XmiUnit(
    // ... other properties
    ifcGuid: "1$qHnSh4fn5$vQIE9d8M0L9",
    unitType: XmiUnitEnum.Length
);

// When exporting to IFC:
// - Use ifcGuid to reference IFC unit definition
// - Apply conversions if IFC uses different units
```

## Related Classes

- Entity properties (e.g., `XmiPoint3D`, `XmiMaterial`, `XmiCrossSection`)
- Properties have implicit unit expectations (mm, kN, MPa, etc.)

## Related Enums

- **XmiUnitEnum** - Unit type classification

## Notes

- Units define measurement system for all quantitative properties
- Different regions use different unit systems (SI vs. Imperial)
- XmiSchema internally uses SI units (mm, kN, MPa) by convention
- Unit definitions enable data exchange across different unit systems
- Use ifcGuid for BIM interoperability with unit libraries
- Maintain unit consistency within a single model
- Document unit assumptions clearly in model metadata
