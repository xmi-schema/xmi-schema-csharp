---
title: "XmiSegment Class"
layout: default
parent: "API Reference"
nav_order: 1
---

# XmiSegment Class

Represents a logical segment within a structural curve member, including its shape classification.

## Properties

| Property | Type | Description |
|----------|------|-------------|
| `SegmentType` | `XmiSegmentTypeEnum` | Geometric definition for downstream consumers |

## Constructor

```csharp
public XmiSegment(
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiSegmentTypeEnum segmentType
)
```

### Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `id` | `string` | Unique identifier inside the Cross Model Information graph |
| `name` | `string` | Readable label; uses `id` when omitted |
| `ifcGuid` | `string` | IFC GUID that links to the originating BIM element |
| `nativeId` | `string` | Source identifier from the authoring system |
| `description` | `string` | Free-form notes about the segment |
| `segmentType` | `XmiSegmentTypeEnum` | Geometric definition for downstream consumers |

## Static Methods

### ValidateSequence

Validates that a collection of segments has proper position sequencing. Segments should be ordered by position from 0, 1, 2, etc. without gaps.

```csharp
public static bool ValidateSequence(
    List<XmiSegment> segments, 
    List<int> positions
)
```

### SortByPosition

Sorts a collection of segments by their position in ascending order.

```csharp
public static List<XmiSegment> SortByPosition(
    List<XmiSegment> segments, 
    List<int> positions
)
```

### CanFormClosedBoundary

Validates that segments can form a closed boundary for a surface member. For a closed boundary, we typically need at least 3 segments.

```csharp
public static bool CanFormClosedBoundary(
    List<XmiSegment> segments, 
    List<int> positions
)
```

## Usage Notes

- **Position Handling**: Segment positions are now handled through the `XmiHasSegment` relationship, not as a direct property on `XmiSegment`
- **Geometry Association**: Segments are associated with geometry (lines, arcs) through relationships like `XmiHasLine3d` and `XmiHasArc3d`
- **Validation**: Use static methods to validate segment sequences and boundary formation

## Example

```csharp
// Create segments
var segments = new List<XmiSegment>
{
    new XmiSegment("seg-1", "Segment 1", "", "native-1", "", XmiSegmentTypeEnum.Line),
    new XmiSegment("seg-2", "Segment 2", "", "native-2", "", XmiSegmentTypeEnum.CircularArc)
};

// Validate sequence
var positions = new List<int> { 0, 1 };
bool isValid = XmiSegment.ValidateSequence(segments, positions);
```