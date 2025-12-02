# XmiLine3D

Represents a straight line segment in 3D space defined by two endpoints.

## Overview

`XmiLine3D` represents linear geometry in the Cross Model Information (XMI) schema. It defines straight-line segments used for centerlines of structural members, reference lines, and edges of geometric shapes. The line is defined by two `XmiPoint3D` endpoints with implicit direction from start to end.

## Properties

| Property | Type | Description |
|----------|------|-------------|
| `StartPoint3D` | `XmiPoint3D` | Starting point of the line segment |
| `EndPoint3D` | `XmiPoint3D` | Ending point of the line segment |

Inherited from `XmiBaseGeometry`: ID, Name, IFCGUID, NativeId, Description, EntityType (= "XmiLine3D")

## Relationships

**Incoming**: `XmiHasGeometry` from `XmiSegment` (defines segment geometry)
**Outgoing**: `XmiHasGeometry` to `StartPoint3D` and `EndPoint3D` (endpoints)

## Usage Examples

### Creating Lines

```csharp
// Vertical column centerline
var base = new XmiPoint3D("PT001", "Base", "", "", "", 0, 0, 0);
var top = new XmiPoint3D("PT002", "Top", "", "", "", 0, 0, 3.5);

var columnLine = new XmiLine3D(
    id: "LN001",
    name: "Column Centerline",
    ifcGuid: "",
    nativeId: "",
    description: "Vertical column axis",
    startPoint3D: base,
    endPoint3D: top
);

// Horizontal beam
var beamStart = new XmiPoint3D("PT003", "Beam Start", "", "", "", 0, 0, 3.5);
var beamEnd = new XmiPoint3D("PT004", "Beam End", "", "", "", 6, 0, 3.5);

var beamLine = new XmiLine3D(
    id: "LN002",
    name: "Beam Centerline",
    ifcGuid: "",
    nativeId: "",
    description: "Horizontal beam axis",
    startPoint3D: beamStart,
    endPoint3D: beamEnd
);
```

### Line Length Calculation

```csharp
double GetLineLength(XmiLine3D line)
{
    double dx = line.EndPoint3D.X - line.StartPoint3D.X;
    double dy = line.EndPoint3D.Y - line.StartPoint3D.Y;
    double dz = line.EndPoint3D.Z - line.StartPoint3D.Z;
    return Math.Sqrt(dx * dx + dy * dy + dz * dz);
}

double length = GetLineLength(columnLine);
Console.WriteLine($"Length: {length}m"); // 3.5m
```

## JSON Representation

```json
{
  "XmiLine3D": {
    "ID": "LN001",
    "Name": "Column Centerline",
    "EntityType": "XmiLine3D",
    "StartPoint3D": { "ID": "PT001" },
    "EndPoint3D": { "PT002" }
  }
}
```

## Engineering Notes

- **Direction**: Start → End defines line direction (may define local axes)
- **Straight Only**: No curvature; use `XmiArc3D` for curved paths
- **BIM Mapping**: Maps to IfcPolyline, IfcLine, IfcTrimmedCurve

## See Also

- [XmiPoint3D.README.md](./XmiPoint3D.README.md) - 3D point geometry
- [XmiArc3D.README.md](./XmiArc3D.README.md) - Curved geometry
- [XmiSegment.README.md](../Entities/XmiSegment.README.md) - Segments using lines
