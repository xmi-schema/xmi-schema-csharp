# XmiSegment

Represents a sub-span along a curve member in XmiSchema library.

## Purpose

`XmiSegment` divides curve members (beams, columns, braces) into smaller sections or spans. Segments enable modeling of:

- Variable cross-sections along member length
- Material changes along member
- Connection points between members
- Analytical refinement for long members

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `ID` | `string` | Yes | Unique identifier for segment |
| `Name` | `string` | Yes | Human-readable segment name (e.g., "Segment 1") |
| `ifcGuid` | `string` | No | IFC GUID for BIM interoperability |
| `NativeId` | `string` | No | Original identifier from source system |
| `Description` | `string` | No | Detailed description of segment |
| `EntityName` | `string` | Yes | Always "XmiSegment" |
| `Domain` | `XmiBaseEntityDomainEnum` | Yes | Always "Shared" |
| `SegmentType` | `XmiSegmentTypeEnum` | Yes | Geometry type (Line, Arc) |
| `Position` | `int` | No | Position index along member (used via relationship) |

## Segment Types

Supported types via `XmiSegmentTypeEnum`:

| Type | Description | Typical Use Cases |
|-------|-------------|-------------------|
| **Line** | Straight line segment | Beam sections between intermediate points |
| **Arc** | Curved arc segment | Curved beams, arches, circular braces |

## Properties Explained

### Segment Type

**Line**:
- Straight segment between two points
- Used for most beam/column sections
- Simplest geometry, easy analysis

**Arc**:
- Curved segment defined by center, radius, and angular span
- Used for curved beams, arches, circular braces
- More complex geometry, requires additional parameters

### Position

**Position** index:
- Defines order of segments along member
- Starts at 0 for first segment
- Increments by 1 for each subsequent segment
- Managed via `XmiHasSegment.Position` relationship

**Example Positions**:
- Member divided into 3 segments: positions 0, 1, 2
- Each segment spans portion of member length

## Usage Example

```csharp
// Create line segment (straight section)
var lineSegment = new XmiSegment(
    id: "seg-1",
    name: "Segment 1 - Straight",
    ifcGuid: "",
    nativeId: "SEG_LINE_1",
    description: "Straight section of beam between points A and B",
    segmentType: XmiSegmentTypeEnum.Line,
    position: 0
);

// Create arc segment (curved section)
var arcSegment = new XmiSegment(
    id: "seg-2",
    name: "Segment 2 - Curved",
    ifcGuid: "",
    nativeId: "SEG_ARC_1",
    description: "Curved arch section with radius 5000mm",
    segmentType: XmiSegmentTypeEnum.Arc,
    position: 1
);

// Create multiple segments for one member
var segments = new List<XmiSegment>
{
    lineSegment,   // Position 0: First half (straight)
    arcSegment,    // Position 1: Middle (arched)
    lineSegment    // Position 2: Second half (straight)
};
```

## Linking to Members

Segments are linked to curve members via `XmiHasSegment` relationships with `Position` property:

```csharp
// Link segments to member
var hasSegment1 = new XmiHasSegment(
    id: "rel-seg-1",
    source: beam,
    target: segments[0],
    position: 0
);

var hasSegment2 = new XmiHasSegment(
    id: "rel-seg-2",
    source: beam,
    target: segments[1],
    position: 1
);

var hasSegment3 = new XmiHasSegment(
    id: "rel-seg-3",
    source: beam,
    target: segments[2],
    position: 2
);

model.Relationships.AddRange(new List<XmiHasSegment>
{
    hasSegment1,
    hasSegment2,
    hasSegment3
});
```

## Member with Segments

### Structural Curve Member Factory

When creating a curve member with segments:

```csharp
// Create segments
var segments = new List<XmiSegment>
{
    new XmiSegment("seg-1", "Segment 1", ...),
    new XmiSegment("seg-2", "Segment 2", ...)
};

// Define positions array (order of segments)
var positions = new List<int> { 0, 1 };

// Create curve member with segments
var beam = manager.CreateXmiStructuralCurveMember(
    modelIndex: 0,
    id: "beam-1",
    name: "Beam B-1",
    // ... other parameters
    segments: segments,
    positions: positions  // Required when segments provided
);
```

### Visual Representation

```
Curve Member (Beam B-1)
┌─────────────────────────────────────┐
│ Point A    Point B    Point C       │
│            │            │            │
│            │            │            │
│   Seg 0    Seg 1      Seg 2      │
│ (Line)    (Arc)       (Line)       │
│   Pos 0     Pos 1       Pos 2       │
└─────────────────────────────────────┘
```

## Segment Applications

### Variable Cross-Sections

Different segments can have different cross-sections:

**Use Case**: Tapered beam or spliced beam

```csharp
// Segment 1: IPE 300 (first half)
var seg1 = new XmiSegment("seg-1", ...);

// Segment 2: IPE 500 (second half)
var seg2 = new XmiSegment("seg-2", ...);

// Link to different cross-sections via geometry relationships
```

### Material Changes

Different segments can have different materials:

**Use Case**: Composite beam or spliced member

```csharp
// Segment 1: Steel section
var steelSeg = new XmiSegment("seg-1", ...);

// Segment 2: Concrete section (composite action)
var concreteSeg = new XmiSegment("seg-2", ...);
```

### Intermediate Connections

Segments can define connection points:

**Use Case**: Beam with intermediate brace connection

```
Beam (3 segments)
┌─────────────────────────────────┐
│ Seg 0    Seg 1    Seg 2     │
│                   │            │
│                   ↑            │
│              Brace connection │
└─────────────────────────────────┘
```

### Curved Members

Arc segments model curved geometry:

**Use Case**: Arch bridge or arched beam

```csharp
// Arch beam with one arc segment
var arcSegment = new XmiSegment(
    id: "seg-arc",
    name: "Main Arch",
    segmentType: XmiSegmentTypeEnum.Arc
);

// Link to arc geometry via XmiHasArc3D relationship
var hasArc = new XmiHasArc3D(
    id: "rel-arc",
    source: arcSegment,
    target: arcGeometry
);
```

## Analysis Considerations

### Finite Element Analysis

Segments enable refined meshing:

- **More segments = More elements**
- **Smaller segments = Better stress resolution**
- **Line segments = 1D beam/column elements**
- **Arc segments = Curved beam elements**

**Example**:
```
Beam divided into 4 segments:
- Each segment becomes 1 FE element
- Result: 4 elements along beam length
- Improved accuracy for distributed loads
```

### Load Distribution

Segments distribute loads along member:

- **Point loads**: Applied at segment boundaries
- **Distributed loads**: Applied across segments
- **Moment loads**: Applied at segment positions

## Best Practices

1. **Order segments** correctly via position indices (0, 1, 2, ...)
2. **Use descriptive names** (e.g., "Segment 1 - Start Section")
3. **Match segment types** to actual geometry (Line vs. Arc)
4. **Ensure positions array** length matches segments length
5. **Use segments** for variable properties (cross-section, material)
6. **Document segment purposes** in description field
7. **Link to appropriate geometries** (XmiHasLine3D or XmiHasArc3D)
8. **Test position ordering** to ensure correct member geometry

## Naming Conventions

### Recommended
- "Segment 1", "Segment 2", "Segment 3"
- "Seg-A", "Seg-B", "Seg-C"
- "Start Section", "Middle Section", "End Section"

### Avoid
- "S1", "S2", "S3" (use descriptive names)
- "1", "2", "3" (include "Segment" prefix)

## Relationship Types

Segments are linked to geometries:

| Relationship | Used When |
|-------------|-----------|
| `XmiHasLine3D` | SegmentType = Line |
| `XmiHasArc3D` | SegmentType = Arc |

Segments are linked to members:

| Relationship | Purpose |
|-------------|---------|
| `XmiHasSegment` | Links segment to curve member with position index |

## Related Classes

- **XmiHasSegment** - Relationship linking segment to member with position
- **XmiStructuralCurveMember** - Member using segments
- **XmiLine3D** - Line geometry for Line-type segments
- **XmiArc3D** - Arc geometry for Arc-type segments

## Related Enums

- **XmiSegmentTypeEnum** - Segment type classification (Line, Arc)

## Notes

- Segments divide curve members into smaller sections
- Position index defines order of segments along member
- Positions array is required when creating members with segments
- Each segment references its own geometry (Line3D or Arc3D)
- Use segments for variable cross-sections, materials, or curved geometry
- Segment enables refined finite element modeling
- Use ifcGuid for BIM interoperability with segment definitions
