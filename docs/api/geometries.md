---
layout: default
title: Geometries
---

# Geometries

Geometric primitives inherit from `XmiBaseGeometry` and can be linked to analytical entities through relationships. All geometry entities have their `Type` automatically set to `Geometry`.

## XmiPoint3D

A single 3D coordinate with tolerance-aware equality.

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `X` | `double` | X coordinate |
| `Y` | `double` | Y coordinate |
| `Z` | `double` | Z coordinate |

### Example

```csharp
var point = new XmiPoint3D(
    "pt-1",
    "Support Point",
    "ifc-guid",
    "PT-1",
    "Node coordinate",
    1.5,  // X
    2.0,  // Y
    3.0   // Z
);
```

### Best Practice

Prefer creating points via `XmiModel.CreatePoint3D` so duplicates are automatically reused:

```csharp
var model = new XmiModel();
var point1 = model.CreatePoint3D(0, 0, 0);
var point2 = model.CreatePoint3D(0, 0, 0); // Returns same instance as point1
```

### Tolerance-Aware Equality

Points are considered equal if coordinates are within a small tolerance (1e-9):

```csharp
var p1 = new XmiPoint3D("pt-1", "Point", "guid", "PT-1", "desc", 1.0, 2.0, 3.0);
var p2 = new XmiPoint3D("pt-2", "Point", "guid", "PT-2", "desc", 1.0, 2.0, 3.0000000001);
// p1.Equals(p2) returns true due to tolerance
```

## XmiLine3D

Straight line segment defined by two endpoints.

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `StartPoint` | `XmiPoint3D` | Line start point |
| `EndPoint` | `XmiPoint3D` | Line end point |

### Example

```csharp
var startPoint = new XmiPoint3D("pt-start", "Start", "guid", "PT-1", "desc", 0, 0, 0);
var endPoint = new XmiPoint3D("pt-end", "End", "guid", "PT-2", "desc", 5, 0, 0);

var line = new XmiLine3D(
    "line-1",
    "Beam Centerline",
    "ifc-guid",
    "LINE-1",
    "Member geometry",
    startPoint,
    endPoint
);
```

### Linking to Members

Associate with curve members using `XmiHasLine3D` or `XmiHasGeometry`:

```csharp
var curveMember = new XmiStructuralCurveMember(...);
var relationship = new XmiHasLine3D(curveMember, line);
model.AddXmiHasLine3D(relationship);
```

## XmiArc3D

Circular arc defined by start point, end point, center point, and radius.

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `StartPoint` | `XmiPoint3D` | Arc start point |
| `EndPoint` | `XmiPoint3D` | Arc end point |
| `CenterPoint` | `XmiPoint3D` | Arc center point |
| `Radius` | `float` | Arc radius |

### Example

```csharp
var startPoint = new XmiPoint3D("pt-start", "Start", "guid", "PT-1", "desc", 0, 0, 0);
var endPoint = new XmiPoint3D("pt-end", "End", "guid", "PT-2", "desc", 5, 5, 0);
var centerPoint = new XmiPoint3D("pt-center", "Center", "guid", "PT-3", "desc", 2.5, 2.5, 0);

var arc = new XmiArc3D(
    "arc-1",
    "Curved Member",
    "ifc-guid",
    "ARC-1",
    "Arc geometry",
    startPoint,
    endPoint,
    centerPoint,
    3.5f  // Radius
);
```

### Use Cases

Useful when exporting curved beams/walls and preserving curvature:

```csharp
var relationship = new XmiHasGeometry(curvedMember, arc);
```

## Creating New Geometries

When creating new geometry classes:

1. Inherit from `XmiBaseGeometry`
2. Expose immutable positional data
3. Call the base constructor with the entity type name for serialization
4. Implement equality comparison if needed
5. Add tests covering equality and serialization edge cases in `tests/XmiSchema.Core.Tests/Geometries`

### Template

```csharp
using XmiSchema.Core.Entities;

namespace XmiSchema.Core.Geometries;

public class XmiCustomGeometry : XmiBaseGeometry
{
    public double Property1 { get; set; }
    public double Property2 { get; set; }

    public XmiCustomGeometry(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        double property1,
        double property2)
        : base(id, name, ifcGuid, nativeId, description, nameof(XmiCustomGeometry))
    {
        Property1 = property1;
        Property2 = property2;
    }
}
```

## Geometry Best Practices

1. **Reuse Points**: Use `XmiModel.CreatePoint3D` to avoid duplicate coordinates
2. **Tolerance**: Remember that point equality uses geometric tolerance
3. **Relationships**: Always link geometries to entities via relationships
4. **Immutability**: Treat geometry objects as immutable after creation
5. **Testing**: Include edge cases like zero-length lines, degenerate arcs

[Back to API Reference](.)
