# XmiSegmentTypeEnum

Enumeration defining types of geometric segments.

## Overview

`XmiSegmentTypeEnum` classifies the geometric shape of segments used to define member geometry, from simple straight lines to complex curves.

## Location

`Models/Enums/XmiSegmentTypeEnum.cs`

## Values

| Enum Value | JSON Value | Description |
|------------|-----------|-------------|
| `Line` | "Line" | Straight line segment |
| `CircularArc` | "Circular Arc" | Circular arc segment |
| `ParabolicArc` | "Parabolic Arc" | Parabolic arc segment |
| `Bezier` | "Bezier" | Bezier curve segment |
| `Spline` | "Spline" | Spline curve segment |
| `Others` | "Others" | Other segment types |
| `Unknown` | "Unknown" | Segment type not determined |

## Usage Examples

### Linear Segment

```csharp
var lineSegment = new XmiSegment(
    id: "SEG001",
    name: "Line-Segment-1",
    segmentType: XmiSegmentTypeEnum.Line,
    geometry: new XmiLine3D(...),
    ...
);
```

### Circular Arc Segment

```csharp
var arcSegment = new XmiSegment(
    id: "SEG002",
    name: "Arc-Segment-1",
    segmentType: XmiSegmentTypeEnum.CircularArc,
    geometry: new XmiArc3D(...),
    ...
);
```

### Curved Member with Multiple Segment Types

```csharp
// Straight approach
var segment1 = new XmiSegment(
    segmentType: XmiSegmentTypeEnum.Line,
    ...
);

// Curved middle section
var segment2 = new XmiSegment(
    segmentType: XmiSegmentTypeEnum.CircularArc,
    ...
);

// Straight departure
var segment3 = new XmiSegment(
    segmentType: XmiSegmentTypeEnum.Line,
    ...
);

var member = new XmiStructuralCurveMember(
    segments: new[] { segment1, segment2, segment3 },
    ...
);
```

## JSON Serialization

```json
{
  "SegmentType": "Line"
}
```

## Segment Type Characteristics

### Line
- **Geometry**: Straight line between two points
- **Representation**: Start point, end point
- **Use**: Most common, simple members
- **Examples**: Straight beams, vertical columns

### Circular Arc
- **Geometry**: Arc of a circle
- **Representation**: Start point, end point, center point or radius
- **Use**: Curved members, arches
- **Examples**: Arched bridges, curved beams

### Parabolic Arc
- **Geometry**: Parabolic curve
- **Representation**: Parametric equation or control points
- **Use**: Suspension cables, arches
- **Examples**: Cable-stayed bridges, parabolic arches

### Bezier
- **Geometry**: Bezier curve defined by control points
- **Representation**: Control points
- **Use**: Complex curved geometry
- **Examples**: Freeform architectural elements

### Spline
- **Geometry**: Smooth curve through multiple points
- **Representation**: Control points, knot vector
- **Use**: Complex smooth curves
- **Examples**: Complex roof geometry, sculptural elements

## Common Applications

### Straight Members
```csharp
segmentType: XmiSegmentTypeEnum.Line
// - Beams
// - Columns
// - Bracing
```

### Arches and Vaults
```csharp
segmentType: XmiSegmentTypeEnum.CircularArc or ParabolicArc
// - Arch bridges
// - Vaulted ceilings
// - Curved roof beams
```

### Cables and Suspension Structures
```csharp
segmentType: XmiSegmentTypeEnum.ParabolicArc
// - Suspension bridge cables
// - Cable-stayed structures
```

### Freeform Geometry
```csharp
segmentType: XmiSegmentTypeEnum.Bezier or Spline
// - Sculptural structures
// - Complex architectural forms
```

## Related Classes

- **XmiSegment**: Uses this enum for segment classification
- **XmiLine3D**: Geometry for Line segments
- **XmiArc3D**: Geometry for CircularArc segments
- **EnumValueAttribute**: Provides JSON string values

## See Also

- [XmiSegment](../Entities/XmiSegment.README.md) - Uses this enum
- [XmiLine3D](../Geometries/XmiLine3D.README.md) - Line geometry
- [XmiArc3D](../Geometries/XmiArc3D.README.md) - Arc geometry
- [XmiBaseEnum](../Bases/XmiBaseEnum.README.md) - Enum attribute system
