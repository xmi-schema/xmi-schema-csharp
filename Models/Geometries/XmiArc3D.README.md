# XmiArc3D

Represents a circular arc segment in 3D space defined by start/end points, center, and radius.

## Overview

`XmiArc3D` represents curved geometry in the XMI schema. It defines circular arc segments used for curved structural members (arches, curved beams), rounded edges, and architectural curved elements. The arc is fully defined by three `XmiPoint3D` instances (start, end, center) and a radius value.

## Properties

| Property | Type | Description |
|----------|------|-------------|
| `StartPoint` | `XmiPoint3D` | Starting point of the arc |
| `EndPoint` | `XmiPoint3D` | Ending point of the arc |
| `CentrePoint` | `XmiPoint3D` | Center point of the circular path |
| `Radius` | `float` | Radius of the arc |

Inherited from `XmiBaseGeometry`: ID, Name, IFCGUID, NativeId, Description, EntityType (= "XmiArc3D")

## Usage Example

```csharp
// Define points for a 90-degree arc
var startPt = new XmiPoint3D("PT001", "Arc Start", "", "", "", 5.0, 0.0, 0.0);
var endPt = new XmiPoint3D("PT002", "Arc End", "", "", "", 0.0, 5.0, 0.0);
var centerPt = new XmiPoint3D("PT003", "Arc Center", "", "", "", 0.0, 0.0, 0.0);

var arc = new XmiArc3D(
    id: "ARC001",
    name: "Quarter Circle",
    ifcGuid: "",
    nativeId: "",
    description: "90-degree arc in XY plane",
    startPoint: startPt,
    endPoint: endPt,
    centrePoint: centerPt,
    radius: 5.0f
);
```

## JSON Representation

```json
{
  "XmiArc3D": {
    "ID": "ARC001",
    "Name": "Quarter Circle",
    "EntityType": "XmiArc3D",
    "StartPoint": { "ID": "PT001" },
    "EndPoint": { "ID": "PT002" },
    "CentrePoint": { "ID": "PT003" },
    "Radius": 5.0
  }
}
```

## Engineering Notes

- **Arc Direction**: Determined by spatial arrangement of start, end, and center points
- **Radius Consistency**: Center should be equidistant from start/end points
- **Units**: Radius units should match model units (meters or millimeters)
- **BIM Mapping**: Maps to IfcCircle, IfcTrimmedCurve in IFC

## See Also

- [XmiPoint3D.README.md](./XmiPoint3D.README.md) - 3D point geometry
- [XmiLine3D.README.md](./XmiLine3D.README.md) - Linear geometry
- [XmiSegment.README.md](../Entities/XmiSegment.README.md) - Segments using arcs
