# XmiCrossSection

Represents cross-section profile geometry and section properties for structural members in XmiSchema library.

## Purpose

`XmiCrossSection` defines the geometric shape and mechanical properties of member cross-sections. Section properties are essential for structural analysis, design calculations, and determining member capacity.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `ID` | `string` | Yes | Unique identifier for cross-section |
| `Name` | `string` | Yes | Human-readable section name |
| `ifcGuid` | `string` | No | IFC GUID for BIM interoperability |
| `NativeId` | `string` | No | Original identifier from source system |
| `Description` | `string` | No | Detailed description of section |
| `EntityName` | `string` | Yes | Always "XmiCrossSection" |
| `Domain` | `XmiBaseEntityDomainEnum` | Yes | Always "Shared" |
| `Shape` | `XmiShapeEnum` | Yes | Section shape type (Rectangular, IShape, etc.) |
| `Area` | `double` | Yes | Cross-sectional area (mm²) |
| `Torsion` | `double` | Yes | Torsional constant J (mm⁴) |
| `SectionModulusStart` | `double` | Yes | Elastic section modulus S_start (mm³) |
| `SectionModulusEnd` | `double` | Yes | Elastic section modulus S_end (mm³) |
| `InertiaStart` | `double` | Yes | Moment of inertia I_start (mm⁴) |
| `InertiaEnd` | `double` | Yes | Moment of inertia I_end (mm⁴) |
| `ShearAreaStart` | `double` | Yes | Shear area A_v,start (mm²) |
| `ShearAreaEnd` | `double` | Yes | Shear area A_v,end (mm²) |
| `Parameters` | `IDictionary<string, object>` | Yes | Shape-specific parametric definition |

## Section Shapes

Supported shapes via `XmiShapeEnum`:

| Shape | Description | Common Applications |
|-------|-------------|-------------------|
| **Rectangular** | Solid rectangle | Concrete beams, columns, walls |
| **Circular** | Solid circle | Concrete columns, steel pipes |
| **IShape** | I-section / Wide flange | Steel beams, columns |
| **TShape** | T-section | Steel beams, composite sections |
| **LShape** | Angle section | Bracing, connections |
| **CShape** | Channel / C-section | Bracing, purlins |
| **RectangularHollow** | Hollow rectangle (Box) | Steel tubes, box columns |
| **CircularHollow** | Hollow circle (Pipe/Tube) | Steel pipes, CHS |

## Properties Explained

### Geometric Properties

**Area (A)**:
- Cross-sectional area (mm²)
- Used for axial stress calculation: σ = P/A
- Used for axial stiffness: EA

**Torsion (J)**:
- Torsional constant (mm⁴)
- Resists twisting of member
- Used for torsional stiffness: GJ

### Section Modulus (S)**

**SectionModulusStart** and **SectionModulusEnd**:
- Elastic section modulus (mm³)
- Bending stress: σ = M/S
- Different values for major vs. minor axis
- Used for flexural capacity checks

**For symmetric sections** (e.g., I-shape):
- SectionModulusStart = SectionModulusEnd
- Different only for non-symmetric shapes (T-shape, L-shape)

### Moment of Inertia (I)**

**InertiaStart** and **InertiaEnd**:
- Second moment of area (mm⁴)
- Bending stiffness: EI
- Different values for major vs. minor axis

**For I-beam example**:
- InertiaStart (I_y): Major axis inertia (strong axis)
- InertiaEnd (I_z): Minor axis inertia (weak axis)

### Shear Area (A_v)**

**ShearAreaStart** and **ShearAreaEnd**:
- Effective area resisting shear (mm²)
- Shear stress: τ = V/A_v
- Different values for different shear planes

## Parametric Definition

The `Parameters` dictionary stores shape-specific dimensions:

### Rectangular Shape
```csharp
{
    "width": 400,      // mm
    "height": 600      // mm
}
```

### I-Shape (Wide Flange)
```csharp
{
    "depth": 500,       // mm
    "flangeWidth": 200, // mm
    "flangeThickness": 16,  // mm
    "webThickness": 10,    // mm
    "radius": 20          // mm (fillet radius)
}
```

### Circular Shape
```csharp
{
    "diameter": 400      // mm
}
```

## Usage Example

```csharp
// Create rectangular concrete section
var rectSection = new XmiCrossSection(
    id: "sec-1",
    name: "Rect-400x600",
    ifcGuid: "4$RmSh4fn5$vQIE9d8M0L9",
    nativeId: "SEC_RECT_400_600",
    description: "Concrete beam section",
    shape: XmiShapeEnum.Rectangular,
    area: 240000,
    torsion: 7200000000,
    sectionModulusStart: 24000000,
    sectionModulusEnd: 16000000,
    inertiaStart: 7200000000,
    inertiaEnd: 3200000000,
    shearAreaStart: 200000,
    shearAreaEnd: 240000,
    parameters: new Dictionary<string, object>
    {
        { "width", 400 },
        { "height", 600 }
    }
);

// Create I-section steel beam
var ibeamSection = new XmiCrossSection(
    id: "sec-2",
    name: "IPE500",
    ifcGuid: "",
    nativeId: "IPE500",
    description: "European I-beam 500",
    shape: XmiShapeEnum.IShape,
    area: 11550,
    torsion: 2670000,
    sectionModulusStart: 2194000,
    sectionModulusEnd: 214000,
    inertiaStart: 482000000,
    inertiaEnd: 21420000,
    shearAreaStart: 4800,
    shearAreaEnd: 4800,
    parameters: new Dictionary<string, object>
    {
        { "depth", 500 },
        { "flangeWidth", 200 },
        { "flangeThickness", 16 },
        { "webThickness", 10 },
        { "radius", 20 }
    }
);

// Assign to structural member
var beam = new XmiStructuralCurveMember(
    // ... other parameters
    crossSection: ibeamSection
);
```

## Linking to Entities

Cross-sections are linked to structural elements via `XmiHasCrossSection` relationships:

```csharp
// Create relationship
var hasSection = new XmiHasCrossSection(
    id: "rel-2",
    source: beam,
    target: ibeamSection
);

model.Relationships.Add(hasSection);
```

## Section Properties Calculation

### Rectangular Section
```
Area = b × h
I_y = (b × h³) / 12
S_y = (b × h²) / 6
```

### Circular Section
```
Area = π × d² / 4
I = π × d⁴ / 64
S = π × d³ / 32
```

### I-Shape Section
(Approximate, excluding fillets)
```
Area = 2 × (b_f × t_f) + (h_w × t_w)
I_y ≈ 2 × [(b_f × h³)/12] + [t_w × h_w³/12]
```

## Analysis Applications

### Flexural Design

Section properties used for:

- **Bending capacity**: M_r = f × S
- **Deflection**: δ = (5 × w × L⁴) / (384 × E × I)
- **Bending stiffness**: EI

### Shear Design

Section properties used for:

- **Shear capacity**: V_r = f_v × A_v
- **Shear deformation**: δ_v = V × L / (G × A_v)

### Torsional Design

Section properties used for:

- **Torsional capacity**: T_r = τ × J / r
- **Torsional stiffness**: GJ
- **St. Venant torsion**: θ = (T × L) / (G × J)

## Best Practices

1. **Use standard section names** (IPE500, HEB300) for design code lookup
2. **Provide accurate section properties** calculated from parameters
3. **Set correct Shape** type for appropriate default behavior
4. **Include Parameters** for section dimensions and reusability
5. **Distinguish major/minor axes** for I_start vs. I_end
6. **Use consistent units** (mm for dimensions, mm² for area, mm⁴ for inertia)
7. **Document standard references** (EN 10365, AISC, JIS) in description

## Standard Section Libraries

### European I-Beams (EN 10365)
- IPE 100-600
- HEA 100-1000
- HEB 100-1000
- HEM 100-1000

### American Wide Flange (AISC)
- W8-W44 series
- Depth range: 8-44 inches

### Universal Beams (BS 4)
- UB 127×76×13 to UB 914×419×388
- Depth range: 127-914 mm

## Related Classes

- **XmiHasCrossSection** - Relationship linking elements to cross-sections
- **XmiStructuralCurveMember** - 1D member using cross-section
- **XmiStructuralSurfaceMember** - 2D element using cross-section

## Related Enums

- **XmiShapeEnum** - Section shape classification

## Notes

- Section properties are shared across multiple structural members
- Start/End properties distinguish major vs. minor axes
- For symmetric sections, Start = End properties
- Parameters enable parametric section definition and reusability
- Use ifcGuid for BIM interoperability with section libraries
- All units in mm for consistency (mm, mm², mm³, mm⁴)
- Shear areas are effective areas, not gross areas
