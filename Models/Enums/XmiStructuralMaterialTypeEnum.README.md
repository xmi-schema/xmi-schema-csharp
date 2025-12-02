# XmiStructuralMaterialTypeEnum

Enumeration defining types of structural materials.

## Overview

`XmiStructuralMaterialTypeEnum` classifies materials used in structural engineering by their basic material family, determining their fundamental behavior and properties.

## Location

`Models/Enums/XmiStructuralMaterialTypeEnum.cs`

## Values

| Enum Value | JSON Value | Description |
|------------|-----------|-------------|
| `Concrete` | "Concrete" | Concrete materials (plain, reinforced, pre-stressed) |
| `Steel` | "Steel" | Structural steel and steel alloys |
| `Timber` | "Timber" | Wood and engineered wood products |
| `Aluminium` | "Aluminium" | Aluminum and aluminum alloys |
| `Composite` | "Composite" | Composite materials (steel-concrete, FRP, etc.) |
| `Masonry` | "Masonry" | Brick, block, and stone masonry |
| `Rebar` | "Rebar" | Reinforcing steel bars |
| `Tendon` | "Tendon" | Pre-stressing tendons/cables |
| `Others` | "Others" | Other material types |
| `Unknown` | "Unknown" | Material type not determined |

## Usage Examples

### Concrete Material

```csharp
var concrete = new XmiStructuralMaterial(
    id: "MAT001",
    name: "Concrete C30/37",
    materialType: XmiStructuralMaterialTypeEnum.Concrete,
    grade: 30.0,  // fck in MPa
    unitWeight: 25.0,  // kN/m³
    eModulus: 32000.0,  // MPa
    ...
);
```

### Steel Material

```csharp
var steel = new XmiStructuralMaterial(
    id: "MAT002",
    name: "Steel S355",
    materialType: XmiStructuralMaterialTypeEnum.Steel,
    grade: 355.0,  // fy in MPa
    unitWeight: 78.5,  // kN/m³
    eModulus: 210000.0,  // MPa
    ...
);
```

### Timber Material

```csharp
var timber = new XmiStructuralMaterial(
    id: "MAT003",
    name: "Glulam GL24h",
    materialType: XmiStructuralMaterialTypeEnum.Timber,
    grade: 24.0,  // Characteristic strength
    unitWeight: 4.2,  // kN/m³
    eModulus: 11500.0,  // MPa
    ...
);
```

### Reinforcement

```csharp
var rebar = new XmiStructuralMaterial(
    id: "MAT004",
    name: "Rebar B500B",
    materialType: XmiStructuralMaterialTypeEnum.Rebar,
    grade: 500.0,  // fy in MPa
    unitWeight: 78.5,  // kN/m³
    eModulus: 200000.0,  // MPa
    ...
);
```

## JSON Serialization

```json
{
  "MaterialType": "Concrete"
}
```

## Material Type Characteristics

### Concrete
- **Nature**: Brittle, high compression strength, low tension strength
- **E-Modulus**: ~20,000-40,000 MPa
- **Unit Weight**: ~24-25 kN/m³
- **Applications**: Beams, columns, slabs, walls

### Steel
- **Nature**: Ductile, high strength in tension and compression
- **E-Modulus**: ~200,000-210,000 MPa
- **Unit Weight**: ~78.5 kN/m³
- **Applications**: Beams, columns, connections, bracing

### Timber
- **Nature**: Orthotropic, varies with grain direction
- **E-Modulus**: ~8,000-14,000 MPa (parallel to grain)
- **Unit Weight**: ~3-6 kN/m³
- **Applications**: Beams, columns, trusses

### Aluminium
- **Nature**: Lightweight, corrosion-resistant
- **E-Modulus**: ~70,000 MPa
- **Unit Weight**: ~27 kN/m³
- **Applications**: Lightweight structures, facades

### Composite
- **Nature**: Combined properties of constituent materials
- **Properties**: Variable based on composition
- **Applications**: Steel-concrete composite beams, FRP strengthening

### Masonry
- **Nature**: Compressive strength, low tensile strength
- **E-Modulus**: Variable (typically 5,000-20,000 MPa)
- **Unit Weight**: ~18-22 kN/m³
- **Applications**: Load-bearing walls, arches

### Rebar
- **Nature**: High tensile strength steel bars
- **E-Modulus**: ~200,000 MPa
- **Applications**: Concrete reinforcement

### Tendon
- **Nature**: High-strength steel cables/strands
- **E-Modulus**: ~190,000-200,000 MPa
- **Applications**: Pre-stressed/post-tensioned concrete

## Related Classes

- **XmiStructuralMaterial**: Uses this enum for material classification
- **EnumValueAttribute**: Provides JSON string values

## See Also

- [XmiStructuralMaterial](../Entities/XmiStructuralMaterial.README.md) - Uses this enum
- [XmiBaseEnum](../Bases/XmiBaseEnum.README.md) - Enum attribute system
