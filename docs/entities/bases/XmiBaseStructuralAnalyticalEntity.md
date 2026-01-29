# XmiBaseStructuralAnalyticalEntity

Base class for structural analytical elements in XmiSchema library.

## Purpose

`XmiBaseStructuralAnalyticalEntity` extends `XmiBaseEntity` to represent abstract structural analysis elements. These are simplified representations used for structural engineering calculations, finite element analysis (FEA), and structural design workflows.

## Properties

Inherits all properties from `XmiBaseEntity`:

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `ID` | `string` | Yes | Unique identifier for analytical element |
| `Name` | `string` | Yes | Human-readable name |
| `ifcGuid` | `string` | No | IFC GUID for BIM interoperability |
| `NativeId` | `string` | No | Original identifier from source system (e.g., ETABS, SAP2000) |
| `Description` | `string` | No | Detailed description of analytical element |
| `EntityName` | `string` | Yes | Type discriminator (e.g., "XmiStructuralCurveMember", "XmiStructuralPointConnection") |
| `Domain` | `XmiBaseEntityDomainEnum` | Yes | Always set to `StructuralAnalytical` |

## Concrete Implementations

| Class | Description | Typical Use Cases |
|-------|-------------|-------------------|
| `XmiStructuralCurveMember` | 1D analytical member | Beam elements, column elements, bracing members |
| `XmiStructuralSurfaceMember` | 2D analytical element | Shell/plate elements for slabs and walls |
| `XmiStructuralPointConnection` | Analytical node | Connection points where members meet, supports, load application points |

## Characteristics

### Analytical Domain

All analytical entities have `Domain` set to `XmiBaseEntityDomainEnum.StructuralAnalytical`, enabling:

- Clear distinction from physical entities (which represent real building components)
- Filtering entities by analytical vs. physical in workflows
- Type-safe operations on analytical element collections

### Abstract Representation

Analytical entities represent:

- **Simplified models** optimized for structural analysis calculations
- **Finite element meshes** (1D elements, 2D elements, 0D nodes)
- **Centerlines and centerlines** (idealized geometry vs. detailed physical geometry)
- **Load paths** for structural analysis (forces, moments, reactions)

### Structural Analysis Integration

Analytical entities include:

- **Structural properties**: Material, cross-section, stiffness
- **Geometric properties**: Length, area, local axes
- **Boundary conditions**: Supports, fixities, releases
- **Connectivity**: Member-to-member connections, nodes

### Software Integration

Analytical entities typically include:

- **NativeId**: Preserves identifiers from structural analysis software (ETABS, SAP2000, RISA, Robot)
- **ifcGuid**: Optional link to BIM elements for model synchronization
- Enables round-trip workflows between XmiSchema and structural analysis platforms

### Relationship to Physical Entities

Analytical entities connect to physical entities via relationships:

- **XmiHasStructuralCurveMember**: Links physical beam/column to analytical curve member
- **XmiHasStructuralSurfaceMember**: Links physical slab/wall to analytical surface member

This enables dual modeling:
1. **Physical model**: Architectural/structural drawings, BIM, quantity takeoff
2. **Analytical model**: Structural analysis calculations, finite element analysis

## Usage Example

```csharp
// Create analytical curve member (column)
var analyticalColumn = new XmiStructuralCurveMember(
    id: "col-1-analytical",
    name: "Column C-1 (Analytical)",
    ifcGuid: "",
    nativeId: "COL-1_ETABS",
    description: "Analytical column element",
    domain: XmiBaseEntityDomainEnum.StructuralAnalytical,
    material: concreteMaterial,
    crossSection: columnSection,
    storey: groundFloor,
    curveMemberType: XmiStructuralCurveMemberTypeEnum.Column,
    nodes: new List<XmiStructuralPointConnection> { bottomNode, topNode },
    segments: segments,
    positions: positions,
    systemLine: XmiSystemLineEnum.MiddleMiddle,
    beginNode: bottomNode,
    endNode: topNode,
    length: 3.0,
    localAxisX: new XmiAxis(1, 0, 0),
    localAxisY: new XmiAxis(0, 1, 0),
    localAxisZ: new XmiAxis(0, 0, 1),
    endFixityStart: "Fixed",
    endFixityEnd: "Fixed"
);

// Query analytical elements
var allAnalytical = model.Entities
    .Where(e => e.Domain == XmiBaseEntityDomainEnum.StructuralAnalytical);
```

## Analysis Types

### 1D Analysis (Curve Members)

Used for:

- **Axial forces**: Compression, tension
- **Bending moments**: Major and minor axes
- **Shear forces**: Major and minor axes
- **Torsion**: Torsional moments and shears

### 2D Analysis (Surface Members)

Used for:

- **In-plane stresses**: Normal and shear stresses
- **Out-of-plane bending**: Plate bending behavior
- **Membrane action**: Diaphragm behavior

### 0D Analysis (Point Connections)

Used for:

- **Reaction forces**: Support reactions
- **Load application**: Point loads, moments
- **Displacements**: Node translations and rotations

## Best Practices

1. **Use consistent naming** between physical and analytical elements (e.g., "Column C-1" for both)
2. **Include NativeId** for traceability to analysis software (ETABS, SAP2000)
3. **Provide structural properties**: Material, cross-section, stiffness
4. **Define boundary conditions**: Supports, fixities, releases explicitly
5. **Maintain physical-analytical links** for model synchronization
6. **Set local axes** correctly for proper force output interpretation

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
   └─ XmiStructuralCurveMember, XmiStructuralSurfaceMember, XmiStructuralPointConnection
        │
        ├─ Material Properties (E, G, ν, density)
        ├─ Cross-Section Properties (A, Ix, Iy, Sx, Sy)
        ├─ Boundary Conditions (supports, fixities)
        └─ Loads (dead load, live load, wind, seismic)
              │
              ↓
3. Analysis & Design
   └─ FEM analysis, stress checks, code compliance
```

## Related Entities

**Analytical Elements**
- **XmiStructuralCurveMember** - 1D analytical member
- **XmiStructuralSurfaceMember** - 2D analytical element
- **XmiStructuralPointConnection** - Analytical node

**Physical Elements** (linked via relationships)
- **XmiBeam** - Physical horizontal member
- **XmiColumn** - Physical vertical member
- **XmiSlab** - Physical planar element
- **XmiWall** - Physical planar element

**Relationships**
- **XmiHasStructuralCurveMember** - Physical to analytical connection
- **XmiHasStructuralPointConnection** - Member to node connection

**Properties**
- **XmiMaterial** - Material properties (E, G, ν)
- **XmiCrossSection** - Section properties (A, I, S)
- **XmiStorey** - Level/Storey association

## Related Enums

- **XmiBaseEntityDomainEnum** - Domain classification
- **XmiStructuralCurveMemberTypeEnum** - Curve member types
- **XmiStructuralSurfaceMemberTypeEnum** - Surface member types
- **XmiSystemLineEnum** - System line positions

## Notes

- Analytical entities represent simplified models for structural analysis
- Physical entities represent detailed building components
- Use NativeId for traceability to structural analysis software
- Linking physical and analytical enables integrated BIM + analysis workflows
- Analytical models are typically smaller/more simplified than physical models
- Supports multiple analysis types: linear, nonlinear, dynamic, buckling
