# XmiHasStructuralStorey

Relationship linking structural elements to their building storey/level.

## Overview

`XmiHasStructuralStorey` represents an association between structural elements and the building storey (floor level) they belong to. This relationship enables vertical organization of the building model and storey-based analysis.

## Location

`Models/Relationships/XmiHasStructuralStorey.cs`

## Inheritance

```
XmiBaseRelationship → XmiHasStructuralStorey
```

## UML Type

**Association** - Represents organizational membership of elements within a building level.

## Constructors

### Full Constructor

```csharp
public XmiHasStructuralStorey(
    string id,
    XmiBaseEntity source,
    XmiBaseEntity target,
    string name,
    string description,
    string entityName,
    string umlType
)
```

### Simplified Constructor (Auto-generated ID)

```csharp
public XmiHasStructuralStorey(
    XmiBaseEntity source,
    XmiBaseEntity target
)
```

## Automatic Creation

This relationship is automatically created by `ExtensionRelationshipExporter` for:

### 1. Point Connections with Storey

```csharp
if (entity is XmiStructuralPointConnection spc && spc.Storey is XmiStructuralStorey storey)
{
    _relationshipManager.AddRelationship(
        new XmiHasStructuralStorey(spc, storey)
    );
}
```

### 2. Curve Members with Storey

```csharp
if (entity is XmiStructuralCurveMember curve && curve.Storey != null)
{
    _relationshipManager.AddRelationship(
        new XmiHasStructuralStorey(curve, curve.Storey)
    );
}
```

### 3. Surface Members with Storey

```csharp
if (entity is XmiStructuralSurfaceMember surface && surface.Storey != null)
{
    _relationshipManager.AddRelationship(
        new XmiHasStructuralStorey(surface, surface.Storey)
    );
}
```

## Usage Examples

### Beam Assigned to Storey

```csharp
// Create storey
var level1 = new XmiStructuralStorey(
    id: "L01",
    name: "Level 1",
    storeyElevation: 3.5,
    ...
);

// Create beam on Level 1
var beam = new XmiStructuralCurveMember(
    id: "B001",
    name: "Beam-1",
    storey: level1,  // Assign to Level 1
    ...
);

// Add to model
var builder = new XmiSchemaModelBuilder();
builder.AddEntity(level1);
builder.AddEntity(beam);

var model = builder.BuildModel();
// Relationship automatically created: beam → level1
```

### Multi-Storey Building

```csharp
// Create storeys
var ground = new XmiStructuralStorey("L00", "Ground", ..., 0.0, ...);
var level1 = new XmiStructuralStorey("L01", "Level 1", ..., 3.5, ...);
var level2 = new XmiStructuralStorey("L02", "Level 2", ..., 7.0, ...);

// Create elements on different levels
var groundSlab = new XmiStructuralSurfaceMember(
    id: "SLAB_G",
    name: "Ground-Slab",
    storey: ground,
    ...
);

var level1Slab = new XmiStructuralSurfaceMember(
    id: "SLAB_1",
    name: "Level1-Slab",
    storey: level1,
    ...
);

var level1Column = new XmiStructuralCurveMember(
    id: "COL_1_A1",
    name: "Column-L1-A1",
    storey: level1,
    ...
);

// Add to model
builder.AddEntities(new[] { ground, level1, level2 });
builder.AddEntities(new[] { groundSlab, level1Slab, level1Column });

var model = builder.BuildModel();
// Relationships automatically created:
// - groundSlab → ground
// - level1Slab → level1
// - level1Column → level1
```

### Nodes with Storey

```csharp
// Create Level 1
var level1 = new XmiStructuralStorey("L01", "Level 1", ..., 3.5, ...);

// Create nodes at Level 1
var node1 = new XmiStructuralPointConnection(
    id: "N1",
    name: "Node-1",
    storey: level1,
    point: new XmiPoint3D("P1", "P1", 0.0, 0.0, 3.5, ...),  // Z matches storey elevation
    ...
);

var node2 = new XmiStructuralPointConnection(
    id: "N2",
    name: "Node-2",
    storey: level1,
    point: new XmiPoint3D("P2", "P2", 6.0, 0.0, 3.5, ...),
    ...
);

builder.AddEntity(level1);
builder.AddEntities(new[] { node1, node2 });

model = builder.BuildModel();
// Relationships automatically created:
// - node1 → level1
// - node2 → level1
```

## Graph Representation

```
XmiStructuralStorey
        ↑
        │ [XmiHasStructuralStorey]
        │
XmiStructuralCurveMember / XmiStructuralSurfaceMember / XmiStructuralPointConnection
```

## Storey-Based Queries

This relationship enables storey-based filtering:

```csharp
// Find all elements on a specific storey
var storeyId = "L01";

var elementsOnStorey = relationshipManager
    .FindByTarget(storeyId)
    .Where(r => r is XmiHasStructuralStorey)
    .Select(r => r.Source);

Console.WriteLine($"Elements on storey {storeyId}:");
foreach (var element in elementsOnStorey)
{
    Console.WriteLine($"  - {element.ID}: {element.Name} ({element.EntityType})");
}

// Group elements by storey
var elementsByStorey = relationshipManager
    .GetAll()
    .OfType<XmiHasStructuralStorey>()
    .GroupBy(r => r.Target.ID)
    .Select(g => new
    {
        StoreyId = g.Key,
        ElementCount = g.Count(),
        Elements = g.Select(r => r.Source.ID).ToList()
    });

foreach (var group in elementsByStorey)
{
    Console.WriteLine($"Storey {group.StoreyId}: {group.ElementCount} elements");
}
```

## JSON Export Example

```json
{
  "XmiHasStructuralStorey": {
    "ID": "rel-005",
    "Source": "B001",
    "Target": "L01",
    "Name": "XmiHasStructuralStorey",
    "Description": "",
    "EntityType": "XmiHasStructuralStorey",
    "UmlType": "Association"
  }
}
```

## Use Cases

### 1. Storey-Based Filtering
Isolate and analyze elements on specific building levels.

### 2. Vertical Organization
Organize building model hierarchically by levels.

### 3. Load Calculation
Calculate storey-level loads for seismic analysis.

### 4. Visualization
Display building model by storey/level in viewer applications.

### 5. Quantity Takeoff
Generate material quantities by building level.

## Best Practices

### Elevation Consistency
Ensure element elevations match their assigned storey:

```csharp
var level1 = new XmiStructuralStorey(..., storeyElevation: 3.5, ...);

var node = new XmiStructuralPointConnection(
    storey: level1,
    point: new XmiPoint3D(..., z: 3.5, ...)  // Z matches storey elevation
);
```

### Null Storey Handling
Elements without storey assignment will not create this relationship:

```csharp
var beam = new XmiStructuralCurveMember(
    storey: null,  // No storey assigned
    ...
);
// No XmiHasStructuralStorey relationship created
```

## Related Classes

- **XmiBaseRelationship**: Base class
- **XmiStructuralStorey**: Target entity (building level)
- **XmiStructuralCurveMember**: Source entity (beams, columns)
- **XmiStructuralSurfaceMember**: Source entity (slabs, walls)
- **XmiStructuralPointConnection**: Source entity (nodes)
- **ExtensionRelationshipExporter**: Creates this relationship automatically

## See Also

- [XmiStructuralStorey](../Entities/XmiStructuralStorey.README.md) - Storey definition
- [XmiStructuralCurveMember](../Entities/XmiStructuralCurveMember.README.md) - Curve member
- [XmiStructuralSurfaceMember](../Entities/XmiStructuralSurfaceMember.README.md) - Surface member
- [XmiStructuralPointConnection](../Entities/XmiStructuralPointConnection.README.md) - Node
- [XmiBaseRelationship](../Bases/XmiBaseRelationship.README.md) - Base class
