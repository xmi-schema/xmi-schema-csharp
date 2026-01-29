# XmiBasePhysicalEntity

Base class for physical building elements in XmiSchema library.

## Purpose

`XmiBasePhysicalEntity` extends `XmiBaseEntity` to represent real-world physical building components such as beams, columns, slabs, and walls. Physical entities represent tangible building elements that exist in the constructed environment.

## Properties

Inherits all properties from `XmiBaseEntity`:

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `ID` | `string` | Yes | Unique identifier for physical element |
| `Name` | `string` | Yes | Human-readable name |
| `ifcGuid` | `string` | No | IFC GUID for BIM interoperability |
| `NativeId` | `string` | No | Original identifier from source system |
| `Description` | `string` | No | Detailed description of physical element |
| `EntityName` | `string` | Yes | Type discriminator (e.g., "XmiBeam", "XmiColumn") |
| `Domain` | `XmiBaseEntityDomainEnum` | Yes | Always set to `Physical` |

## Concrete Implementations

| Class | Description | Typical Use Cases |
|-------|-------------|-------------------|
| `XmiBeam` | Horizontal structural member | Floor beams, roof joists, girders |
| `XmiColumn` | Vertical structural member | Building columns, support posts |
| `XmiSlab` | Horizontal planar element | Floor slabs, roof decks, diaphragms |
| `XmiWall` | Vertical planar element | Load-bearing walls, shear walls, curtain walls |

## Characteristics

### Physical Domain

All physical entities have `Domain` set to `XmiBaseEntityDomainEnum.Physical`, enabling:

- Clear distinction from analytical entities (which are abstract representations)
- Filtering entities by physical vs. analytical in workflows
- Type-safe operations on physical element collections

### Real-World Representation

Physical entities represent:

- **Tangible building components** that can be touched, measured, and quantified
- **Manufactured or constructed elements** with actual dimensions, materials, and properties
- **Elements visible in architectural and structural drawings**
- **Components included in BIM models** for quantity takeoff and construction planning

### BIM Integration

Physical entities typically include:

- **ifcGuid**: Links to IFC elements for BIM authoring software (Revit, Tekla, ArchiCAD)
- **NativeId**: Preserves original identifiers for traceability
- Enables round-trip workflows between XmiSchema and BIM platforms

### Relationship to Analytical Entities

Physical entities connect to analytical entities via relationships:

- **XmiHasStructuralCurveMember**: Links physical beam/column to analytical curve member
- **XmiHasStructuralSurfaceMember**: Links physical slab/wall to analytical surface member

This enables dual modeling:
1. **Physical model**: Architectural/structural drawings, BIM, quantity takeoff
2. **Analytical model**: Structural analysis calculations, finite element analysis

## Usage Example

```csharp
// Create physical beam
var physicalBeam = new XmiBeam(
    id: "beam-1",
    name: "Beam B-1",
    ifcGuid: "2O2Fr$t4X7Zf8NOew3FL9r",
    nativeId: "B-1_Revit",
    description: "Main floor beam in grid line A",
    domain: XmiBaseEntityDomainEnum.Physical
);

// Create corresponding analytical member
var analyticalBeam = new XmiStructuralCurveMember(...);

// Link physical to analytical
var rel = new XmiHasStructuralCurveMember(
    id: "rel-1",
    source: physicalBeam,
    target: analyticalBeam
);
```

## Best Practices

1. **Use consistent naming** between physical and analytical elements (e.g., "Beam B-1" for both)
2. **Include ifcGuid** when working with BIM workflows
3. **Provide descriptive names** for architectural/structural drawings
4. **Maintain physical-analytical links** for analysis-model synchronization
5. **Use NativeId** for traceability to source CAD/BIM files

## Workflow: Physical to Analytical Mapping

Typical workflow in structural engineering:

```
1. BIM/Revit Model (Physical)
   └─ XmiBeam, XmiColumn, XmiSlab, XmiWall
        │
        └─ XmiHasStructuralCurveMember
              │
              ↓
2. Structural Analysis Model (Analytical)
   └─ XmiStructuralCurveMember, XmiStructuralSurfaceMember
        │
        └─ Calculations (FEM, loads, results)
```

## Related Entities

**Physical Elements**
- **XmiBeam** - Horizontal structural member
- **XmiColumn** - Vertical structural member
- **XmiSlab** - Horizontal planar element
- **XmiWall** - Vertical planar element

**Analytical Elements** (linked via relationships)
- **XmiStructuralCurveMember** - 1D analytical member
- **XmiStructuralSurfaceMember** - 2D analytical element

**Relationships**
- **XmiHasStructuralCurveMember** - Physical to analytical connection
- **XmiHasStructuralSurfaceMember** - Physical to analytical connection

**Properties**
- **XmiMaterial** - Material properties
- **XmiCrossSection** - Section properties
- **XmiStorey** - Level/Storey association

## Related Enums

- **XmiBaseEntityDomainEnum** - Domain classification

## Notes

- Physical entities represent real-world building components
- Analytical entities are abstract representations for structural analysis
- Physical and analytical models can exist independently
- Linking them enables integrated BIM + analysis workflows
- Physical entities may have geometry, materials, and cross-sections directly
- Use ifcGuid for BIM interoperability (Revit, Tekla, etc.)
