# XmiStorey

Represents a building level or storey in XmiSchema library.

## Purpose

`XmiStorey` defines horizontal levels within a building structure, such as ground floor, first floor, roof level, etc. Storeys are used for organizing building elements vertically and for mass and load calculations.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `ID` | `string` | Yes | Unique identifier for storey |
| `Name` | `string` | Yes | Human-readable level name (e.g., "Level 1", "Ground Floor") |
| `ifcGuid` | `string` | No | IFC GUID for BIM interoperability |
| `NativeId` | No | Original identifier from source system |
| `Description` | `string` | No | Detailed description of level |
| `EntityName` | `string` | Yes | Always "XmiStorey" |
| `Domain` | `XmiBaseEntityDomainEnum` | Yes | Always "Functional" |
| `Elevation` | `double` | Yes | Vertical elevation from reference level (mm) |
| `Mass` | `double` | Yes | Total mass of elements on this level (kN) |
| `IsGroundStorey` | `bool` | Yes | Indicates if this is ground/lowest level |
| `ReactionSumX` | `double` | No | Sum of X-direction reactions at supports (kN) |
| `ReactionSumY` | `double` | No | Sum of Y-direction reactions at supports (kN) |
| `ReactionSumZ` | `double` | No | Sum of Z-direction reactions at supports (kN) |
| `ReactionSumMX` | `double` | No | Sum of X-axis moments at supports (kN·m) |
| `ReactionSumMY` | `double` | No | Sum of Y-axis moments at supports (kN·m) |
| `ReactionSumMZ` | `double` | No | Sum of Z-axis moments at supports (kN·m) |

## Properties Explained

### Geometric Properties

**Elevation**:
- Vertical distance from reference level (mm)
- Typically measured from ground level (±0.00)
- Positive for levels above reference, negative for basements

**Example Storey Elevations**:
- Basement Level: -3000 mm
- Ground Floor: 0 mm
- Level 1: 3500 mm
- Level 2: 7000 mm
- Roof: 10500 mm

### Structural Properties

**Mass**:
- Total mass of all structural elements on this level (kN)
- Used for seismic load calculations
- Used for total building mass calculations

**IsGroundStorey**:
- Boolean flag indicating if this is the lowest level
- Typically ground floor or basement level
- Used as reference for base shear calculations

### Reaction Sums

Reaction properties capture support reactions from structural analysis:

**Force Reactions** (kN):
- `ReactionSumX`: Horizontal X-direction reaction
- `ReactionSumY`: Horizontal Y-direction reaction
- `ReactionSumZ`: Vertical Z-direction reaction

**Moment Reactions** (kN·m):
- `ReactionSumMX`: Moment about X-axis
- `ReactionSumMY`: Moment about Y-axis
- `ReactionSumMZ`: Moment about Z-axis

## Usage Example

```csharp
// Create ground floor
var groundFloor = new XmiStorey(
    id: "storey-0",
    name: "Ground Floor",
    ifcGuid: "5$SmSh4fn5$vQIE9d8M0L9",
    nativeId: "LEVEL_00",
    description: "Ground level, reference elevation ±0.00",
    elevation: 0.0,
    mass: 850.0,
    isGroundStorey: true,
    reactionSumX: 0.0,
    reactionSumY: 0.0,
    reactionSumZ: 850.0,
    reactionSumMX: 0.0,
    reactionSumMY: 0.0,
    reactionSumMZ: 0.0
);

// Create first floor
var firstFloor = new XmiStorey(
    id: "storey-1",
    name: "Level 1",
    ifcGuid: "",
    nativeId: "LEVEL_01",
    description: "First floor, elevation +3.50m",
    elevation: 3500.0,
    mass: 820.0,
    isGroundStorey: false,
    reactionSumX: 12.5,
    reactionSumY: 8.3,
    reactionSumZ: 820.0,
    reactionSumMX: 0.0,
    reactionSumMY: 0.0,
    reactionSumMZ: 0.0
);

// Create roof level
var roof = new XmiStorey(
    id: "storey-5",
    name: "Roof",
    ifcGuid: "",
    nativeId: "LEVEL_ROOF",
    description: "Roof level",
    elevation: 10500.0,
    mass: 420.0,
    isGroundStorey: false,
    reactionSumX: 0.0,
    reactionSumY: 0.0,
    reactionSumZ: 420.0,
    reactionSumMX: 0.0,
    reactionSumMY: 0.0,
    reactionSumMZ: 0.0
);
```

## Linking to Entities

Storeys are linked to structural elements via `XmiHasStorey` relationships:

```csharp
// Link beam to first floor
var hasStorey = new XmiHasStorey(
    id: "rel-1",
    source: beam,
    target: firstFloor
);

model.Relationships.Add(hasStorey);
```

Multiple elements can reference the same storey:

```csharp
// All these beams are on first floor
model.Relationships.AddRange(new List<XmiHasStorey>
{
    new XmiHasStorey("rel-2", beam1, firstFloor),
    new XmiHasStorey("rel-3", beam2, firstFloor),
    new XmiHasStorey("rel-4", beam3, firstFloor),
    new XmiHasStorey("rel-5", column1, firstFloor),
    new XmiHasStorey("rel-6", column2, firstFloor)
});
```

## Analysis Applications

### Seismic Analysis

Storey mass is used for:

- **Base shear**: V_b = Σ (Mass_i × a_i)
- **Total building mass**: M_total = Σ Mass_i
- **Seismic load distribution**: F_i = Mass_i × a_i

**Example**:
```
Ground Floor:  850 kN
Level 1:      820 kN
Level 2:      780 kN
Level 3:      750 kN
Level 4:      700 kN
Roof:         420 kN
Total:        4320 kN
```

### Vertical Load Distribution

Storey mass is used for:

- **Gravity loads**: Dead load, Live load
- **Load path**: Downward load distribution through structure
- **Foundation design**: Total loads to foundations

### Support Reactions

Reaction sums are used for:

- **Equilibrium checks**: ΣForces = 0, ΣMoments = 0
- **Foundation design**: Load transfer to ground
- **Global stability**: Checking overturning and sliding

**Equilibrium Check Example**:
```
Total Load = 4320 kN
ReactionSumZ = 4320 kN
Difference = 0 kN ✓ (Equilibrium satisfied)
```

## Building Organization

### Multi-Story Buildings

Storeys organize building elements vertically:

```
┌─────────────────────────┐  Roof (Elev: 10500mm)
│                       │
├─────────────────────────┤  Level 4 (Elev: 7000mm)
│                       │
├─────────────────────────┤  Level 3 (Elev: 5250mm)
│                       │
├─────────────────────────┤  Level 2 (Elev: 3500mm)
│                       │
├─────────────────────────┤  Level 1 (Elev: 1750mm)
│                       │
├─────────────────────────┤  Ground Floor (Elev: 0mm)
│                       │
└─────────────────────────┘  Basement (Elev: -3000mm)
```

### Element Assignment

Each structural element is assigned to one storey:

- **Beams**: Typically on floor level (e.g., Level 1 beams)
- **Columns**: Span between storeys (e.g., Ground → Level 1)
- **Slabs**: Typically on floor level (e.g., Level 1 slab)
- **Walls**: Can span multiple storeys

## Best Practices

1. **Set Elevation** in millimeters for consistency
2. **Name levels clearly** (e.g., "Level 1" vs. "L1" vs. "1F")
3. **Calculate Mass** accurately from element weights
4. **Set IsGroundStorey** correctly for reference level
5. **Use descriptive names** for building documentation
6. **Maintain consistent elevations** across all storeys
7. **Include NativeId** for traceability to BIM models
8. **Calculate ReactionSums** after structural analysis

## Naming Conventions

### Recommended
- "Ground Floor"
- "Level 1", "Level 2", ...
- "Roof"
- "Basement", "Basement 1", ...

### Avoid
- "L1", "L2", "L3" (use "Level" prefix)
- "00", "01", "02" (use descriptive names)
- "1F", "2F", "3F" (use "Floor" suffix)

## Related Classes

- **XmiHasStorey** - Relationship linking elements to storeys
- **XmiBeam**, **XmiColumn** - Elements assigned to storeys
- **XmiSlab**, **XmiWall** - Elements assigned to storeys

## Related Enums

- **XmiBaseEntityDomainEnum** - Domain classification (Functional)

## Notes

- Storeys organize building elements vertically
- Mass values are cumulative for all elements on level
- Elevation is measured from reference level (usually ground)
- Ground floor typically has elevation 0.0 mm
- Reaction sums are calculated after structural analysis
- IsGroundStorey is used as reference for seismic base shear
- Use ifcGuid for BIM interoperability with level definitions
- All elements on a storey reference the same XmiStorey instance
