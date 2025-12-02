# XmiBaseGeometry

Abstract base class for all 3D geometry entities in the Cross Model Information (XMI) schema.

## Overview

`XmiBaseGeometry` serves as the foundation for all geometric primitives in the built environment data model. This abstract class provides common infrastructure for representing spatial information in 3D space.

Geometry entities in the XMI schema are first-class entities that exist independently and can be referenced by structural elements. This design allows multiple structural entities to share the same geometry, promoting data efficiency and consistency.

All concrete geometry classes (`XmiPoint3D`, `XmiLine3D`, `XmiArc3D`) inherit from this base class and can be linked to structural entities through the `XmiHasGeometry` relationship.

## Class Hierarchy

```
XmiBaseEntity (root)
    └── XmiBaseGeometry (abstract)
            ├── XmiPoint3D (3D coordinates)
            ├── XmiLine3D (linear segments)
            └── XmiArc3D (curved segments)
```

## Properties

This class inherits all properties from `XmiBaseEntity`:

| Property | Type | Description |
|----------|------|-------------|
| `ID` | `string` | Unique identifier for the geometry entity |
| `Name` | `string` | Display name (defaults to ID if not provided) |
| `IFCGUID` | `string` | IFC GUID for BIM interoperability |
| `NativeId` | `string` | Original identifier from source system |
| `Description` | `string` | Textual description of the geometry |
| `EntityType` | `string` | Type discriminator (set to "XmiBaseGeometry" or derived type name) |

### No Additional Properties

`XmiBaseGeometry` does not add any new properties beyond those inherited from `XmiBaseEntity`. Derived classes add geometry-specific properties like coordinates, radii, etc.

## Relationships

### Outgoing Relationships
None - geometry entities don't typically reference other entities.

### Incoming Relationships
- **XmiHasGeometry**: Referenced by:
  - `XmiStructuralPointConnection` (links nodes to Point3D geometry)
  - `XmiSegment` (links segments to Line3D/Arc3D geometry)
  - Potentially other structural entities that need spatial definition

## Usage Example

### Deriving from XmiBaseGeometry

Since `XmiBaseGeometry` is abstract, you cannot instantiate it directly. Instead, use one of the concrete geometry classes:

```csharp
using XmiSchema.Core.Geometries;

// Create a 3D point (concrete implementation)
var point = new XmiPoint3D(
    id: "PT001",
    name: "Node at Origin",
    ifcGuid: "1a2b3c4d-5678-90ab-cdef-1234567890ab",
    nativeId: "NATIVE-PT001",
    description: "Origin point of coordinate system",
    x: 0.0,
    y: 0.0,
    z: 0.0
);

// Create a 3D line (concrete implementation)
var line = new XmiLine3D(
    id: "LN001",
    name: "Vertical Line",
    ifcGuid: "2b3c4d5e-6789-01bc-def0-234567890abc",
    nativeId: "NATIVE-LN001",
    description: "Line from origin to point above",
    startPoint3D: point,
    endPoint3D: new XmiPoint3D("PT002", "Top Point", "", "", "", 0, 0, 3.5)
);
```

### Working with Geometry in the Model

```csharp
using XmiSchema.Core.Results;
using XmiSchema.Core.Entities;

var builder = new XmiSchemaModelBuilder();

// Add geometry entities
builder.AddEntity(point);
builder.AddEntity(line);

// Create a structural element that references geometry
var connection = new XmiStructuralPointConnection(
    id: "NODE001",
    name: "Support Node",
    ifcGuid: "...",
    nativeId: "...",
    description: "Fixed support at column base",
    storey: null,
    point: point  // Reference to Point3D geometry
);

builder.AddEntity(connection);

// Build model - XmiHasGeometry relationship created automatically
var model = builder.BuildModel();

// Export to JSON
string json = builder.BuildJsonString();
```

## JSON Representation

Geometry entities appear as nodes in the JSON graph export. Since `XmiBaseGeometry` is abstract, you'll see concrete types like `XmiPoint3D`:

```json
{
  "nodes": [
    {
      "XmiPoint3D": {
        "ID": "PT001",
        "Name": "Node at Origin",
        "IFCGUID": "1a2b3c4d-5678-90ab-cdef-1234567890ab",
        "NativeId": "NATIVE-PT001",
        "Description": "Origin point of coordinate system",
        "EntityType": "XmiPoint3D",
        "X": 0.0,
        "Y": 0.0,
        "Z": 0.0
      }
    }
  ],
  "edges": [
    {
      "XmiHasGeometry": {
        "ID": "rel-001",
        "Source": "NODE001",
        "Target": "PT001",
        "Name": "XmiHasGeometry",
        "EntityType": "XmiHasGeometry",
        "UmlType": "Composition"
      }
    }
  ]
}
```

## Related Classes

### Base Classes
- **`XmiBaseEntity`**: Parent class providing common entity properties

### Derived Classes
- **`XmiPoint3D`**: Represents 3D coordinates (X, Y, Z)
- **`XmiLine3D`**: Represents linear geometry between two points
- **`XmiArc3D`**: Represents curved geometry with start, end, and center points

### Related Relationships
- **`XmiHasGeometry`**: Links structural entities to their geometric representation

### Related Entities
- **`XmiStructuralPointConnection`**: Uses Point3D for spatial positioning
- **`XmiSegment`**: Uses Line3D or Arc3D for geometric definition

## Design Patterns

### Independent Geometry Pattern

Geometries are designed as independent entities rather than embedded properties. This pattern provides several benefits:

1. **Reusability**: Multiple structural elements can reference the same geometry
2. **Graph Representation**: Geometries appear as separate nodes in the graph model
3. **Relationship Tracking**: Explicit relationships show which entities use which geometries
4. **Data Integrity**: Centralized geometry management reduces duplication

### Abstract Base Pattern

`XmiBaseGeometry` is abstract, enforcing that only specific geometry types (Point3D, Line3D, Arc3D) can be instantiated. This ensures type safety and clear semantics in the model.

## Engineering Notes

### Coordinate System

The XMI schema uses a right-handed 3D Cartesian coordinate system:
- **X-axis**: Typically represents the East-West direction
- **Y-axis**: Typically represents the North-South direction
- **Z-axis**: Typically represents the vertical (elevation) direction

Units should be consistent across the model (typically meters or millimeters).

### Geometry Sharing

Multiple structural elements can reference the same geometry entity:

```csharp
// Two segments sharing endpoint geometry
var sharedPoint = new XmiPoint3D(...);
var segment1 = new XmiSegment(..., endNode: connection1);
var segment2 = new XmiSegment(..., beginNode: connection1);
// Both connections reference the same Point3D
```

### BIM Interoperability

The `IFCGUID` property enables mapping geometry entities to IFC geometric representations, supporting round-trip data exchange with BIM applications like Revit, ArchiCAD, and Tekla.

## See Also

- [XmiBaseEntity.README.md](./XmiBaseEntity.README.md) - Parent class documentation
- [XmiPoint3D.README.md](../Geometries/XmiPoint3D.README.md) - 3D point implementation
- [XmiLine3D.README.md](../Geometries/XmiLine3D.README.md) - Linear geometry implementation
- [XmiArc3D.README.md](../Geometries/XmiArc3D.README.md) - Curved geometry implementation
- [XmiHasGeometry.README.md](../Relationships/XmiHasGeometry.README.md) - Geometry relationship
