# XmiSlab

Represents a physical slab element in the built environment.

## Purpose

`XmiSlab` defines horizontal planar elements that form floors, roofs, or platforms. Slabs are physical representations of structural elements that can be linked to analytical surface members for analysis purposes.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `ZOffset` | `double` | Yes | Offset along Z axis relative to host (meters) |
| `LocalAxisX` | `XmiAxis` | Yes | Unit direction of local X axis |
| `LocalAxisY` | `XmiAxis` | Yes | Unit direction of local Y axis |
| `LocalAxisZ` | `XmiAxis` | Yes | Unit direction of local Z axis |
| `Thickness` | `double` | Yes | Physical thickness of slab (meters) |

## Usage Example

```csharp
// Create a concrete floor slab
var slab = new XmiSlab(
    id: "slab-1",
    name: "Floor Slab Level 2",
    ifcGuid: "2g0X$0m55u9hX1w2a7x5f",
    nativeId: "SLAB-L2",
    description: "Reinforced concrete floor slab spanning 8x8m",
    zOffset: 0.0,
    localAxisX: new XmiAxis(1, 0, 0),    // Along length
    localAxisY: new XmiAxis(0, 1, 0),    // Along width
    localAxisZ: new XmiAxis(0, 0, 1),    // Vertical
    thickness: 0.20  // 200mm thick
);

// Add to model
model.AddXmiSlab(slab);

// Link to material and geometry
var hasMaterial = new XmiHasMaterial(slab, concreteMaterial);
var hasGeometry = new XmiHasGeometry(slab, polygonGeometry);
```

## Common Slab Types

- **Floor Slabs** - Support live loads from occupants and equipment
- **Roof Slabs** - Support roof loads (snow, wind, equipment)
- **Mezzanine Slabs** - Intermediate level within building
- **Podium Slabs** - Larger floor area at building base

## Related Classes

- **XmiStructuralSurfaceMember** - Analytical member linked to physical slab
- **XmiStorey** - Storey hosting slab
- **XmiHasMaterial** - Material relationship
- **XmiHasGeometry** - Geometry relationship
