# XmiPoint3D

Represents a point in 3D Cartesian space within the Cross Model Information (XMI) schema.

## Overview

`XmiPoint3D` is the fundamental geometric primitive in the XMI built environment data model. It represents a location in three-dimensional space using Cartesian coordinates (X, Y, Z).

Points are the building blocks of all geometry in structural engineering models, defining:
- **Positions** of structural nodes and connections
- **Endpoints** of linear members (beams, columns, bracing)
- **Control points** for curved geometry
- **Reference locations** for any spatial entity

As a subclass of `XmiBaseGeometry`, points are first-class entities in the graph model, existing independently and referenced by multiple structural elements through `XmiHasGeometry` relationships.

## Properties

| Property | Type | Description |
|----------|------|-------------|
| `X` | `double` | X-coordinate (typically East-West direction) |
| `Y` | `double` | Y-coordinate (typically North-South direction) |
| `Z` | `double` | Z-coordinate (typically vertical elevation) |

### Inherited Properties

From `XmiBaseEntity`:
- `ID`: Unique identifier
- `Name`: Display name (e.g., "Grid A-1", "N101")
- `IFCGUID`: IFC GUID for BIM interoperability
- `NativeId`: Original identifier from source system
- `Description`: Textual description
- `EntityType`: Automatically set to "XmiPoint3D"

## Coordinate System

The XMI schema uses a **right-handed Cartesian coordinate system**:

- **X-axis**: East-West direction (horizontal)
- **Y-axis**: North-South direction (horizontal)
- **Z-axis**: Vertical direction (elevation)

**Units**: Should be consistent across the model (typically meters or millimeters for structural engineering).

**Precision**: Double-precision floating-point (15-17 decimal digits) - sufficient for 0.1mm to 1mm engineering precision.

## Relationships

### Incoming Relationships

- **XmiHasGeometry**: Referenced by:
  - `XmiStructuralPointConnection` - Links nodes to 3D positions
  - `XmiLine3D` - Start and end points of linear geometry
  - `XmiArc3D` - Start, end, and center points of curved geometry

## Usage Examples

### Creating Points

```csharp
using XmiSchema.Core.Geometries;

// Origin point
var origin = new XmiPoint3D(
    id: "PT001",
    name: "Origin",
    ifcGuid: "",
    nativeId: "ORIGIN",
    description: "Coordinate system origin",
    x: 0.0,
    y: 0.0,
    z: 0.0
);

// Column base at grid intersection
var columnBase = new XmiPoint3D(
    id: "PT_A1_L0",
    name: "Grid A-1 @ Ground",
    ifcGuid: "2eQ8vn9fD9QQvN8O_70D$8",
    nativeId: "ETABS_N101",
    description: "Column base at grid intersection A-1",
    x: 6.0,   // 6m east
    y: 8.0,   // 8m north
    z: 0.0    // ground level
);

// Column top (3.5m above base)
var columnTop = new XmiPoint3D(
    id: "PT_A1_L1",
    name: "Grid A-1 @ 1st Floor",
    ifcGuid: "",
    nativeId: "ETABS_N102",
    description: "Column top at first floor level",
    x: 6.0,
    y: 8.0,
    z: 3.5   // 3.5m elevation
);
```

### Points for Structural Nodes

```csharp
using XmiSchema.Core.Entities;

// Create point geometry
var point = new XmiPoint3D(
    id: "PT001",
    name: "Support Point",
    ifcGuid: "",
    nativeId: "",
    description: "Fixed support location",
    x: 0.0,
    y: 0.0,
    z: 0.0
);

// Create structural node referencing the point
var connection = new XmiStructuralPointConnection(
    id: "NODE001",
    name: "Column Base",
    ifcGuid: "",
    nativeId: "",
    description: "Fixed support at column base",
    storey: null,
    point: point  // Reference to Point3D geometry
);

// Add to model
var builder = new XmiSchemaModelBuilder();
builder.AddEntity(point);
builder.AddEntity(connection);

var model = builder.BuildModel();
// XmiHasGeometry relationship created automatically: NODE001 → PT001
```

### Points for Linear Geometry

```csharp
// Create endpoint points
var startPoint = new XmiPoint3D("PT001", "Start", "", "", "", 0.0, 0.0, 0.0);
var endPoint = new XmiPoint3D("PT002", "End", "", "", "", 0.0, 0.0, 3.5);

// Create line between points
var line = new XmiLine3D(
    id: "LN001",
    name: "Column Centerline",
    ifcGuid: "",
    nativeId: "",
    description: "Vertical line from base to top",
    startPoint3D: startPoint,
    endPoint3D: endPoint
);
```

### Multi-Story Building Grid

```csharp
// Create grid points at different elevations
var gridPoints = new List<XmiPoint3D>();

string[] gridX = { "A", "B", "C", "D" };
double[] xCoords = { 0.0, 6.0, 12.0, 18.0 };
double[] zElevations = { 0.0, 3.5, 7.0, 10.5 }; // Ground, 1st, 2nd, 3rd floors

for (int i = 0; i < gridX.Length; i++)
{
    for (int z = 0; z < zElevations.Length; z++)
    {
        var point = new XmiPoint3D(
            id: $"PT_{gridX[i]}_L{z}",
            name: $"Grid {gridX[i]} @ Level {z}",
            ifcGuid: "",
            nativeId: $"GRID_{gridX[i]}_{z}",
            description: $"Grid intersection at {gridX[i]} on level {z}",
            x: xCoords[i],
            y: 0.0,
            z: zElevations[z]
        );
        gridPoints.Add(point);
    }
}

Console.WriteLine($"Created {gridPoints.Count} grid points"); // 16 points
```

## JSON Representation

```json
{
  "nodes": [
    {
      "XmiPoint3D": {
        "ID": "PT_A1_L0",
        "Name": "Grid A-1 @ Ground",
        "IFCGUID": "2eQ8vn9fD9QQvN8O_70D$8",
        "NativeId": "ETABS_N101",
        "Description": "Column base at grid intersection A-1 on ground level",
        "EntityType": "XmiPoint3D",
        "X": 6.0,
        "Y": 8.0,
        "Z": 0.0
      }
    }
  ]
}
```

## Related Classes

### Base Classes
- **`XmiBaseGeometry`**: Abstract base for all geometry
- **`XmiBaseEntity`**: Root entity class

### Derived Geometry
- **`XmiLine3D`**: Linear geometry using two Point3D endpoints
- **`XmiArc3D`**: Curved geometry using Point3D for start, end, center

### Related Entities
- **`XmiStructuralPointConnection`**: Structural nodes positioned at Point3D locations

### Relationships
- **`XmiHasGeometry`**: Links structural entities to their Point3D geometry

## Engineering Notes

### Precision and Tolerance

**Double Precision**: ~15-17 significant decimal digits
- For meters: precise to ~1e-15 meters (far beyond engineering needs)
- For millimeters: precise to ~1e-12 millimeters

**Practical Tolerance**:
- Structural engineering: ±1mm is typical
- Point equality: Use tolerance-based comparison

```csharp
bool PointsEqual(XmiPoint3D p1, XmiPoint3D p2, double tolerance = 0.001)
{
    return Math.Abs(p1.X - p2.X) < tolerance &&
           Math.Abs(p1.Y - p2.Y) < tolerance &&
           Math.Abs(p1.Z - p2.Z) < tolerance;
}
```

### Coordinate System Conventions

**Global vs. Local**:
- XmiPoint3D represents **global coordinates**
- Local member coordinates require transformation

**Elevation Reference**:
- Z = 0 typically represents a reference datum (ground level, sea level, etc.)
- Positive Z = above reference
- Negative Z = below reference (basements, foundations)

### Common Point Operations

**Distance Between Points**:
```csharp
double Distance(XmiPoint3D p1, XmiPoint3D p2)
{
    double dx = p2.X - p1.X;
    double dy = p2.Y - p1.Y;
    double dz = p2.Z - p1.Z;
    return Math.Sqrt(dx * dx + dy * dy + dz * dz);
}
```

**Midpoint**:
```csharp
XmiPoint3D Midpoint(XmiPoint3D p1, XmiPoint3D p2)
{
    return new XmiPoint3D(
        id: $"MID_{p1.ID}_{p2.ID}",
        name: "Midpoint",
        ifcGuid: "",
        nativeId: "",
        description: $"Midpoint between {p1.Name} and {p2.Name}",
        x: (p1.X + p2.X) / 2.0,
        y: (p1.Y + p2.Y) / 2.0,
        z: (p1.Z + p2.Z) / 2.0
    );
}
```

### BIM Interoperability

**IFC Mapping**:
- XmiPoint3D ↔ IfcCartesianPoint
- Use `IFCGUID` property for round-trip mapping
- IFC uses meters by default (ensure unit consistency)

**Coordinate System Transformation**:
- May need to transform between different BIM software coordinate systems
- Consider using transformation matrices for batch operations

### Performance Considerations

**Memory Usage**:
- Per point: ~72 bytes (3 doubles + object overhead)
- 10,000 points: ~720 KB

**Optimization for Large Models**:
- Consider spatial indexing (octrees, KD-trees) for proximity queries
- Reuse point instances when multiple entities reference the same location

## Best Practices

**DO**:
- Use consistent units throughout the model
- Provide meaningful names (grid references, node labels)
- Include IFC GUIDs for BIM interoperability
- Use tolerance-based equality for floating-point comparison

**DON'T**:
- Don't mix units (meters and millimeters) in the same model
- Don't assume exact floating-point equality
- Don't create duplicate points at the same location (reuse instances)

## See Also

- [XmiBaseGeometry.README.md](../Bases/XmiBaseGeometry.README.md) - Base geometry class
- [XmiLine3D.README.md](./XmiLine3D.README.md) - Linear geometry using points
- [XmiArc3D.README.md](./XmiArc3D.README.md) - Curved geometry using points
- [XmiStructuralPointConnection.README.md](../Entities/XmiStructuralPointConnection.README.md) - Structural nodes
- [XmiHasGeometry.README.md](../Relationships/XmiHasGeometry.README.md) - Geometry relationships
