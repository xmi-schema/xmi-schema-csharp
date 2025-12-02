# XmiStructuralStorey

Represents a building level or storey in a structural model.

## Overview

`XmiStructuralStorey` models horizontal building levels (floors) in a multi-storey structure. It organizes structural elements by elevation and can store storey-level properties such as mass and reaction data for seismic and dynamic analysis.

## Location

`Models/Entities/XmiStructuralStorey.cs`

## Inheritance

```
XmiBaseEntity → XmiStructuralStorey
```

## Properties

### Geometric Properties

- **StoreyElevation** (`double`): Vertical elevation of the storey from a reference datum (typically in meters or feet)
  - Positive values above datum
  - Used to organize elements vertically

### Structural Properties

- **StoreyMass** (`double`): Total mass assigned to this storey level
  - Used for seismic analysis
  - Includes dead loads, live loads, and mass from structural elements
  - Typically in kg or tons

### Reaction Data

- **StoreyHorizontalReactionX** (`string`): Horizontal reaction force in X direction
  - Can store analysis results or prescribed values
  - String format allows for various representations

- **StoreyHorizontalReactionY** (`string`): Horizontal reaction force in Y direction
  - For lateral load analysis
  - Wind and seismic reactions

- **StoreyVerticalReaction** (`string`): Vertical reaction force
  - Gravity load reactions
  - Total vertical force at storey level

## Constructor

```csharp
public XmiStructuralStorey(
    string id,
    string name,
    string ifcguid,
    string nativeId,
    string description,
    double storeyElevation,
    double storeyMass,
    string storeyHorizontalReactionX,
    string storeyHorizontalReactionY,
    string storeyVerticalReaction
)
```

## Usage Examples

### Basic Storey Definition

```csharp
var level1 = new XmiStructuralStorey(
    id: "L01",
    name: "Level 1",
    ifcguid: "8Jp0ri$tK1mNO2qSrTuVwX",
    nativeId: "StoreyId_L1",
    description: "First floor level at +3.5m",
    storeyElevation: 3.5,        // 3.5 meters above datum
    storeyMass: 50000.0,         // 50 tons
    storeyHorizontalReactionX: "0.0",
    storeyHorizontalReactionY: "0.0",
    storeyVerticalReaction: "0.0"
);
```

### Multi-Storey Building

```csharp
var builder = new XmiSchemaModelBuilder();

// Ground floor
var ground = new XmiStructuralStorey(
    id: "L00",
    name: "Ground Floor",
    ifcguid: "...",
    nativeId: "L00",
    description: "Ground level at +0.0m",
    storeyElevation: 0.0,
    storeyMass: 60000.0,
    storeyHorizontalReactionX: "0.0",
    storeyHorizontalReactionY: "0.0",
    storeyVerticalReaction: "0.0"
);

// First floor
var level1 = new XmiStructuralStorey(
    id: "L01",
    name: "Level 1",
    storeyElevation: 3.5,
    storeyMass: 50000.0,
    ...
);

// Second floor
var level2 = new XmiStructuralStorey(
    id: "L02",
    name: "Level 2",
    storeyElevation: 7.0,
    storeyMass: 50000.0,
    ...
);

// Roof
var roof = new XmiStructuralStorey(
    id: "LRF",
    name: "Roof",
    storeyElevation: 10.5,
    storeyMass: 30000.0,
    ...
);

// Add to model
builder.AddEntities(new[] { ground, level1, level2, roof });
```

### Storey with Seismic Reactions

```csharp
// After seismic analysis, reactions can be stored
var level1WithReactions = new XmiStructuralStorey(
    id: "L01",
    name: "Level 1",
    storeyElevation: 3.5,
    storeyMass: 50000.0,
    storeyHorizontalReactionX: "125.5",  // 125.5 kN
    storeyHorizontalReactionY: "98.2",   // 98.2 kN
    storeyVerticalReaction: "490.5"      // 490.5 kN
);
```

### Organizing Members by Storey

```csharp
var level1 = new XmiStructuralStorey("L01", "Level 1", ..., 3.5, ...);

// Assign members to storey
var column = new XmiStructuralCurveMember(
    id: "C001",
    name: "Column-1",
    storey: level1,  // Belongs to Level 1
    ...
);

var beam = new XmiStructuralCurveMember(
    id: "B001",
    name: "Beam-1",
    storey: level1,  // Belongs to Level 1
    ...
);

var slab = new XmiStructuralSurfaceMember(
    id: "S001",
    name: "Slab-1",
    storey: level1,  // Belongs to Level 1
    ...
);

// Add to model
builder.AddEntity(level1);
builder.AddEntities(new[] { column, beam, slab });

var model = builder.BuildModel();
// Relationships: column → level1, beam → level1, slab → level1
```

## JSON Export Example

```json
{
  "XmiStructuralStorey": {
    "ID": "L01",
    "Name": "Level 1",
    "IFCGUID": "8Jp0ri$tK1mNO2qSrTuVwX",
    "NativeId": "StoreyId_L1",
    "Description": "First floor level at +3.5m",
    "EntityType": "XmiStructuralStorey",
    "StoreyElevation": 3.5,
    "StoreyMass": 50000.0,
    "StoreyHorizontalReactionX": "0.0",
    "StoreyHorizontalReactionY": "0.0",
    "StoreyVerticalReaction": "0.0"
  }
}
```

## Storey Elevation Guidelines

### Typical Building Elevations

```csharp
// Basement levels (negative elevations)
var basement1 = new XmiStructuralStorey(..., storeyElevation: -3.0, ...);
var basement2 = new XmiStructuralStorey(..., storeyElevation: -6.0, ...);

// Ground floor (datum)
var ground = new XmiStructuralStorey(..., storeyElevation: 0.0, ...);

// Typical office floors (3.5m - 4.0m floor-to-floor)
var level1 = new XmiStructuralStorey(..., storeyElevation: 3.5, ...);
var level2 = new XmiStructuralStorey(..., storeyElevation: 7.0, ...);
var level3 = new XmiStructuralStorey(..., storeyElevation: 10.5, ...);

// Mechanical/service floor (higher floor-to-floor)
var mechanical = new XmiStructuralStorey(..., storeyElevation: 14.5, ...);

// Roof
var roof = new XmiStructuralStorey(..., storeyElevation: 18.0, ...);
```

## Storey Mass Calculation

Storey mass typically includes:

1. **Dead Loads**:
   - Self-weight of structural elements
   - Permanent fixtures and finishes
   - MEP equipment

2. **Live Loads** (portion for seismic):
   - Occupancy loads (reduced percentage)
   - Storage loads
   - Equipment loads

3. **Calculation Example**:
```csharp
double floorArea = 900.0;  // m²
double deadLoad = 5.0;      // kN/m²
double liveLoad = 2.5;      // kN/m²
double seismicLiveLoadFactor = 0.25;  // 25% of live load

double storeyMass = (floorArea * (deadLoad + liveLoad * seismicLiveLoadFactor)) / 9.81;
// Convert from kN to mass (kg), then to tons

var storey = new XmiStructuralStorey(
    ...,
    storeyMass: storeyMass,
    ...
);
```

## Relationship Graph

```
XmiStructuralStorey
        ↑
        │ (XmiHasStructuralStorey)
        │
XmiStructuralCurveMember / XmiStructuralSurfaceMember / XmiStructuralPointConnection
```

## Common Storey Organizations

### Residential Building
- Basement (-3.0m)
- Ground (0.0m)
- Levels 1-10 (3.0m spacing)
- Roof (30.0m)

### Office Building
- Basement 2 (-6.0m)
- Basement 1 (-3.0m)
- Ground (0.0m)
- Levels 1-20 (4.0m spacing)
- Mechanical (84.0m)
- Roof (88.0m)

### Parking Structure
- Levels P1-P5 (3.5m spacing)
- Clear heights for vehicles

## Design Patterns

### Composite Pattern
Storeys organize collections of structural elements.

### Layer Pattern
Provides vertical layering of building model.

## Related Classes

- **XmiBaseEntity**: Base class
- **XmiStructuralCurveMember**: Can be assigned to storey
- **XmiStructuralSurfaceMember**: Can be assigned to storey
- **XmiStructuralPointConnection**: Can be assigned to storey

## See Also

- [XmiStructuralCurveMember](XmiStructuralCurveMember.README.md) - Uses storey
- [XmiStructuralSurfaceMember](XmiStructuralSurfaceMember.README.md) - Uses storey
- [XmiStructuralPointConnection](XmiStructuralPointConnection.README.md) - Uses storey
- [XmiBaseEntity](../Bases/XmiBaseEntity.README.md) - Base class
