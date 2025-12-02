# XmiStructuralMaterial

Represents material properties for structural analysis and design.

## Overview

`XmiStructuralMaterial` defines the physical and mechanical properties of structural materials such as concrete, steel, timber, and aluminum. It provides essential material characteristics used in structural analysis calculations.

## Location

`Models/Entities/XmiStructuralMaterial.cs`

## Inheritance

```
XmiBaseEntity → XmiStructuralMaterial
```

## Properties

### Material Classification

- **MaterialType** (`XmiStructuralMaterialTypeEnum`): Type of material (Concrete, Steel, Timber, Aluminum, etc.)

### Strength Properties

- **Grade** (`double`): Material strength grade
  - For concrete: Compressive strength (fck) in MPa
  - For steel: Yield strength (fy) in MPa
  - For timber: Characteristic strength

### Physical Properties

- **UnitWeight** (`double`): Density or unit weight (kN/m³ or lb/ft³)

### Elastic Properties

- **EModulus** (`double`): Young's modulus / Modulus of elasticity (MPa or ksi)
  - Resistance to elastic deformation under axial stress

- **GModulus** (`double`): Shear modulus / Modulus of rigidity (MPa or ksi)
  - Resistance to shear deformation

- **PoissonRatio** (`double`): Poisson's ratio (dimensionless, typically 0.0 - 0.5)
  - Ratio of transverse to axial strain

### Thermal Properties

- **ThermalCoefficient** (`double`): Coefficient of thermal expansion (per °C or per °F)
  - Material expansion/contraction with temperature change

## Constructor

```csharp
public XmiStructuralMaterial(
    string id,
    string name,
    string ifcguid,
    string nativeId,
    string description,
    XmiStructuralMaterialTypeEnum materialType,
    double grade,
    double unitWeight,
    double eModulus,
    double gModulus,
    double poissonRatio,
    double thermalCoefficient
)
```

## Usage Examples

### Concrete Material

```csharp
var concrete = new XmiStructuralMaterial(
    id: "MAT001",
    name: "Concrete C30/37",
    ifcguid: "1Ai2bc$dE4fGH5jKLMNoPq",
    nativeId: "ConcreteType_01",
    description: "Normal weight concrete grade C30/37",
    materialType: XmiStructuralMaterialTypeEnum.Concrete,
    grade: 30.0,              // fck = 30 MPa
    unitWeight: 25.0,         // 25 kN/m³
    eModulus: 32000.0,        // 32 GPa = 32000 MPa
    gModulus: 13333.0,        // G ≈ E / (2(1+ν))
    poissonRatio: 0.2,        // Typical for concrete
    thermalCoefficient: 1.0e-5  // 10 × 10⁻⁶ per °C
);
```

### Steel Material

```csharp
var steel = new XmiStructuralMaterial(
    id: "MAT002",
    name: "Steel S355",
    ifcguid: "2Bj3cd$eF5gHI6kLMNoPqR",
    nativeId: "SteelGrade_S355",
    description: "Structural steel grade S355",
    materialType: XmiStructuralMaterialTypeEnum.Steel,
    grade: 355.0,             // fy = 355 MPa
    unitWeight: 78.5,         // 78.5 kN/m³
    eModulus: 210000.0,       // 210 GPa = 210000 MPa
    gModulus: 81000.0,        // G ≈ E / (2(1+ν))
    poissonRatio: 0.3,        // Typical for steel
    thermalCoefficient: 1.2e-5  // 12 × 10⁻⁶ per °C
);
```

### Timber Material

```csharp
var timber = new XmiStructuralMaterial(
    id: "MAT003",
    name: "Glulam GL24h",
    ifcguid: "3Ck4de$fG6hIJ7lMNoPqRs",
    nativeId: "TimberClass_GL24h",
    description: "Glued laminated timber GL24h",
    materialType: XmiStructuralMaterialTypeEnum.Timber,
    grade: 24.0,              // Characteristic strength
    unitWeight: 4.2,          // 4.2 kN/m³
    eModulus: 11500.0,        // 11.5 GPa = 11500 MPa
    gModulus: 720.0,          // Shear modulus
    poissonRatio: 0.3,        // Typical for timber
    thermalCoefficient: 5.0e-6  // 5 × 10⁻⁶ per °C
);
```

## JSON Export Example

```json
{
  "XmiStructuralMaterial": {
    "ID": "MAT001",
    "Name": "Concrete C30/37",
    "IFCGUID": "1Ai2bc$dE4fGH5jKLMNoPq",
    "NativeId": "ConcreteType_01",
    "Description": "Normal weight concrete grade C30/37",
    "EntityType": "XmiStructuralMaterial",
    "MaterialType": "Concrete",
    "Grade": 30.0,
    "UnitWeight": 25.0,
    "EModulus": 32000.0,
    "GModulus": 13333.0,
    "PoissonRatio": 0.2,
    "ThermalCoefficient": 0.00001
  }
}
```

## Material Type Values

Common `MaterialType` enum values:

- **Concrete**: Concrete materials
- **Steel**: Steel materials
- **Timber**: Wood and engineered wood products
- **Aluminum**: Aluminum alloys
- **Other**: Other material types

(See `XmiStructuralMaterialTypeEnum` for complete list)

## Typical Material Properties

### Concrete (C30/37)
- Grade: 30 MPa
- Unit Weight: 25 kN/m³
- E: 32,000 MPa
- ν: 0.2

### Steel (S355)
- Grade: 355 MPa
- Unit Weight: 78.5 kN/m³
- E: 210,000 MPa
- ν: 0.3

### Timber (GL24h)
- Grade: 24 MPa
- Unit Weight: 4.2 kN/m³
- E: 11,500 MPa
- ν: 0.3

### Aluminum (6061-T6)
- Grade: 240 MPa
- Unit Weight: 27 kN/m³
- E: 69,000 MPa
- ν: 0.33

## Relationship to Other Entities

Materials are referenced by:
- **XmiStructuralCrossSection**: Cross-sections use materials
- **XmiStructuralSurfaceMember**: Surface elements can directly reference materials

```
XmiStructuralMaterial
        ↑
        │ (XmiHasStructuralMaterial)
        │
XmiStructuralCrossSection
        ↑
        │ (XmiHasStructuralCrossSection)
        │
XmiStructuralCurveMember
```

## Elastic Relationships

For isotropic materials, the elastic constants are related:

```
G = E / (2(1 + ν))
```

Where:
- G = Shear modulus (GModulus)
- E = Young's modulus (EModulus)
- ν = Poisson's ratio (PoissonRatio)

## Usage in Model

```csharp
// Create material
var material = new XmiStructuralMaterial(...);

// Assign to cross-section
var crossSection = new XmiStructuralCrossSection(
    ...,
    material: material,
    ...
);

// Use cross-section in member
var member = new XmiStructuralCurveMember(
    ...,
    crossSection: crossSection,
    ...
);

// Add to model
var builder = new XmiSchemaModelBuilder();
builder.AddEntity(material);
builder.AddEntity(crossSection);
builder.AddEntity(member);

var model = builder.BuildModel();
// Relationships: member → crossSection → material
```

## Related Classes

- **XmiBaseEntity**: Base class
- **XmiStructuralCrossSection**: Uses material properties
- **XmiStructuralSurfaceMember**: Can directly reference material
- **XmiStructuralMaterialTypeEnum**: Material type enumeration

## See Also

- [XmiStructuralCrossSection](XmiStructuralCrossSection.README.md) - Uses materials
- [XmiStructuralCurveMember](XmiStructuralCurveMember.README.md) - Uses cross-sections with materials
- [XmiBaseEntity](../Bases/XmiBaseEntity.README.md) - Base class
