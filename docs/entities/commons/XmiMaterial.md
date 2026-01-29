# XmiMaterial

Represents material properties for structural elements in XmiSchema library.

## Purpose

`XmiMaterial` defines physical and mechanical properties of construction materials used in building structures. Materials are shared across multiple structural elements and include properties necessary for structural analysis and design.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `ID` | `string` | Yes | Unique identifier for material |
| `Name` | `string` | Yes | Human-readable material name |
| `ifcGuid` | `string` | No | IFC GUID for BIM interoperability |
| `NativeId` | `string` | No | Original identifier from source system |
| `Description` | `string` | No | Detailed description of material |
| `EntityName` | `string` | Yes | Always "XmiMaterial" |
| `Domain` | `XmiBaseEntityDomainEnum` | Yes | Always "Shared" |
| `MaterialType` | `XmiMaterialTypeEnum` | Yes | Material classification (Concrete, Steel, Timber, etc.) |
| `Grade` | `int?` | No | Material grade or strength rating |
| `UnitWeight` | `double?` | No | Unit weight (density) in kN/m³ |
| `EModulus` | `string` | No | Young's modulus of elasticity in MPa |
| `GModulus` | `string` | No | Shear modulus of elasticity in MPa |
| `PoissonRatio` | `string` | No | Poisson's ratio (dimensionless) |
| `ThermalCoefficient` | `double?` | No | Coefficient of thermal expansion (×10⁻⁶/°C) |

## Material Types

Supported material types via `XmiMaterialTypeEnum`:

| Type | Typical Grade | Unit Weight | E Modulus (GPa) | Poisson's Ratio |
|------|---------------|--------------|-------------------|-----------------|
| **Concrete** | C20, C30, C40, C50 | 24-25 | 25-40 | 0.15-0.20 |
| **Steel** | S235, S275, S355 | 77-78 | 200-210 | 0.30 |
| **Timber** | C14, C16, C24 | 4-8 | 8-12 | 0.30-0.40 |
| **Aluminum** | 6061-T6, 6063-T5 | 27 | 69 | 0.33 |
| **Masonry** | Clay brick, Concrete block | 18-20 | 5-20 | 0.15-0.25 |

## Properties Explained

### Structural Properties

**EModulus (Young's Modulus)**:
- Measures material stiffness in tension/compression
- Higher values = stiffer material
- Used for calculating deflections and stress

**GModulus (Shear Modulus)**:
- Measures material stiffness in shear
- Related to E modulus via: G = E / (2 × (1 + ν))
- Used for shear deformation calculations

**Poisson's Ratio (ν)**:
- Ratio of transverse to axial strain
- Typical values: 0.15-0.35 for construction materials
- Used for 3D stress-strain relationships

### Physical Properties

**UnitWeight (Density)**:
- Mass per unit volume (kN/m³)
- Used for dead load calculations
- Concrete: ~24 kN/m³, Steel: ~78 kN/m³

**Grade**:
- Material strength classification
- Examples: C40 (concrete strength), S355 (steel yield strength)
- Used for design code checks

### Thermal Properties

**ThermalCoefficient**:
- Rate of thermal expansion (×10⁻⁶/°C)
- Used for thermal stress calculations
- Steel: ~12, Concrete: ~10, Aluminum: ~23

## Usage Example

```csharp
// Create concrete material
var concreteC40 = new XmiMaterial(
    id: "mat-1",
    name: "Concrete C40",
    ifcGuid: "3$HnSh4fn5$vQIE9d8M0L9",
    nativeId: "MAT_C40",
    description: "High-strength concrete for columns",
    materialType: XmiMaterialTypeEnum.Concrete,
    grade: 40,
    unitWeight: 24.0,
    eModulus: "33000",
    gModulus: "13750",
    poissonRatio: "0.20",
    thermalCoefficient: 10.0
);

// Create steel material
var steelS355 = new XmiMaterial(
    id: "mat-2",
    name: "Steel S355",
    ifcGuid: "",
    nativeId: "MAT_S355",
    description: "Structural steel grade S355",
    materialType: XmiMaterialTypeEnum.Steel,
    grade: 355,
    unitWeight: 77.0,
    eModulus: "210000",
    gModulus: "81000",
    poissonRatio: "0.30",
    thermalCoefficient: 12.0
);

// Assign to structural member
var column = new XmiStructuralCurveMember(
    // ... other parameters
    material: concreteC40
);
```

## Linking to Entities

Materials are linked to structural elements via `XmiHasMaterial` relationships:

```csharp
// Create relationship
var hasMaterial = new XmiHasMaterial(
    id: "rel-1",
    source: beam,
    target: concreteC40
);

model.Relationships.Add(hasMaterial);
```

## Analysis Applications

### Concrete Design

Material properties used for:

- **Axial capacity**: P_c = 0.85 × f'_c × A × β_1
- **Modulus of rupture**: Used for crack control
- **Creep & shrinkage**: Time-dependent deformation

### Steel Design

Material properties used for:

- **Yield strength**: f_y (from Grade)
- **Elastic modulus**: For buckling calculations
- **Shear modulus**: For shear deformation

### Dynamic Analysis

Material properties used for:

- **Mass**: From UnitWeight × Volume
- **Stiffness**: From E and G moduli
- **Natural frequencies**: √(k/m)

## Best Practices

1. **Use standard material grades** (C30, S355) for design code compliance
2. **Provide realistic E and G values** for accurate structural analysis
3. **Include UnitWeight** for dead load calculations
4. **Set MaterialType correctly** for appropriate default values
5. **Use consistent material definitions** across project elements
6. **Document material standards** (ASTM, EN, BS) in description

## Common Material Libraries

### Concrete (EN 206 / ACI 318)
- C20/25: f'_ck = 20 MPa, f'_c = 25 MPa
- C30/37: f'_ck = 30 MPa, f'_c = 37 MPa
- C40/50: f'_ck = 40 MPa, f'_c = 50 MPa

### Structural Steel (EN 10025 / AISC)
- S235: f_y = 235 MPa
- S275: f_y = 275 MPa
- S355: f_y = 355 MPa
- S460: f_y = 460 MPa

### Timber (EN 14081)
- C14: f_m = 14 N/mm²
- C16: f_m = 16 N/mm²
- C24: f_m = 24 N/mm²

## Related Classes

- **XmiHasMaterial** - Relationship linking elements to materials
- **XmiStructuralCurveMember** - 1D member using material
- **XmiStructuralSurfaceMember** - 2D element using material

## Related Enums

- **XmiMaterialTypeEnum** - Material type classification

## Notes

- Material properties are shared across multiple structural elements
- E and G modulus values should be in MPa for consistency
- UnitWeight in kN/m³ is standard for structural engineering
- Grade is optional but recommended for design checks
- Thermal coefficient is used for temperature load calculations
- Use ifcGuid for BIM interoperability with material libraries
