# XmiHasGeometry

Relationship linking entities to their geometric representations.

## Overview

`XmiHasGeometry` represents an association between structural entities and their geometric definitions. This relationship connects entities like `XmiStructuralPointConnection` or `XmiSegment` to geometry objects such as `XmiPoint3D`, `XmiLine3D`, or `XmiArc3D`.

## Location

`Models/Relationships/XmiHasGeometry.cs`

## Inheritance

```
XmiBaseRelationship → XmiHasGeometry
```

## UML Type

**Association** - Represents a loose reference relationship where the geometry defines the spatial representation of the entity.

## Constructors

### Full Constructor

```csharp
public XmiHasGeometry(
    string id,
    XmiBaseEntity source,
    XmiBaseGeometry target,
    string name,
    string description,
    string entityName,
    string umlType
)
```

### Simplified Constructor (Auto-generated ID)

```csharp
public XmiHasGeometry(
    XmiBaseEntity source,
    XmiBaseGeometry target
)
```

- Automatically generates GUID for ID
- Sets entity type to "XmiHasGeometry"
- Sets UML type to "Association"

## Automatic Creation

This relationship is automatically created by `ExtensionRelationshipExporter` when:

1. **XmiStructuralPointConnection has Point3D**:
   ```csharp
   pointConnection → point3D
   ```

2. **XmiSegment has Geometry**:
   ```csharp
   segment → line3D or arc3D
   ```

3. **XmiArc3D has Points**:
   ```csharp
   arc3D → startPoint
   arc3D → endPoint
   arc3D → centrePoint
   ```

4. **XmiLine3D has Points**:
   ```csharp
   line3D → startPoint3D
   line3D → endPoint3D
   ```

## Usage Examples

### Point Connection to Geometry

```csharp
var point3D = new XmiPoint3D("P001", "Point-1", 0.0, 0.0, 0.0, ...);

var pointConnection = new XmiStructuralPointConnection(
    id: "NODE001",
    name: "Node-1",
    point: point3D,
    ...
);

// Relationship automatically created
var builder = new XmiSchemaModelBuilder();
builder.AddEntity(pointConnection);
var model = builder.BuildModel();

// Result: XmiHasGeometry(pointConnection, point3D)
```

### Segment to Line Geometry

```csharp
var line = new XmiLine3D("LINE001", "Line-1", startPoint, endPoint, ...);

var segment = new XmiSegment(
    id: "SEG001",
    name: "Segment-1",
    geometry: line,
    ...
);

// Relationship automatically created
builder.AddEntity(segment);
model = builder.BuildModel();

// Result: XmiHasGeometry(segment, line)
```

### Manual Relationship Creation

```csharp
// If needed, can create manually
var geometry = new XmiPoint3D(...);
var entity = new XmiStructuralPointConnection(...);

var relationship = new XmiHasGeometry(entity, geometry);

var relationshipManager = new RelationshipManager<XmiBaseRelationship>();
relationshipManager.AddRelationship(relationship);
```

## Graph Representation

```
XmiStructuralPointConnection ──[XmiHasGeometry]──> XmiPoint3D

XmiSegment ──[XmiHasGeometry]──> XmiLine3D

XmiSegment ──[XmiHasGeometry]──> XmiArc3D

XmiArc3D ──[XmiHasGeometry]──> XmiPoint3D (start, end, centre)

XmiLine3D ──[XmiHasGeometry]──> XmiPoint3D (start, end)
```

## JSON Export Example

```json
{
  "XmiHasGeometry": {
    "ID": "a7f3c8e2-9b41-4d5a-8e1c-6f2d9a3b5c7e",
    "Source": "NODE001",
    "Target": "P001",
    "Name": "XmiHasGeometry",
    "Description": "",
    "EntityType": "XmiHasGeometry",
    "UmlType": "Association"
  }
}
```

## Common Source Entities

- **XmiStructuralPointConnection**: Node → Point3D
- **XmiSegment**: Segment → Line3D or Arc3D
- **XmiArc3D**: Arc → Point3D (endpoints, center)
- **XmiLine3D**: Line → Point3D (endpoints)

## Common Target Geometries

- **XmiPoint3D**: 3D point coordinates
- **XmiLine3D**: Linear geometry
- **XmiArc3D**: Circular arc geometry

## Related Classes

- **XmiBaseRelationship**: Base class
- **XmiBaseGeometry**: Target geometry base class
- **XmiPoint3D**: Point geometry
- **XmiLine3D**: Line geometry
- **XmiArc3D**: Arc geometry
- **ExtensionRelationshipExporter**: Creates this relationship automatically

## See Also

- [XmiBaseRelationship](../Bases/XmiBaseRelationship.README.md) - Base relationship class
- [XmiPoint3D](../Geometries/XmiPoint3D.README.md) - Point geometry
- [XmiLine3D](../Geometries/XmiLine3D.README.md) - Line geometry
- [XmiArc3D](../Geometries/XmiArc3D.README.md) - Arc geometry
