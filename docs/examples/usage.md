---
layout: default
title: Usage Examples
---

# Usage Examples

This page demonstrates how to use the XmiSchema.Core library in common scenarios.

## Quick Start

### Creating a Simple Model

```csharp
using XmiSchema.Core.Models;
using XmiSchema.Core.Manager;

// Create a new model
var model = new XmiModel();

// Create a material
var material = new XmiMaterial(
    "mat-1",
    "Steel A992",
    "ifc-guid",
    "MAT-1",
    "Structural steel",
    XmiMaterialTypeEnum.Steel,
    345,       // Yield strength (MPa)
    78.5,      // Density (kN/m³)
    "200000",  // Young's modulus (MPa)
    "80000",   // Shear modulus (MPa)
    "0.3",     // Poisson's ratio
    1.2        // Thermal expansion (×10⁻⁵/°C)
);
model.AddXmiMaterial(material);

// Create a cross-section
var crossSection = new XmiCrossSection(
    "sec-1",
    "W12x26",
    "ifc-guid",
    "SEC-1",
    "Wide flange beam section",
    XmiShapeEnum.IShape,
    new IShapeParameters(0.31, 0.165, 0.0094, 0.00635, 0.0095),
    0.00497,   // Area (m²)
    0.0000234, // Torsional constant
    0.0000567, // Moment of inertia Y
    0.000891,  // Moment of inertia Z
    0.000234,  // Section modulus Y
    0.00567,   // Section modulus Z
    0.0034,    // Radius of gyration Y
    0.134,     // Radius of gyration Z
    0.0000012  // Warping constant
);
model.AddXmiStructuralCrossSection(crossSection);
```

## Creating Physical Elements

### Beam Example

```csharp
using XmiSchema.Core.Models.Entities.Physical;

var beam = new XmiBeam(
    "beam-1",
    "Main Support Beam",
    "ifc-guid-beam",
    "BEAM-1",
    "Primary structural beam",
    XmiStructuralCurveMemberSystemLineEnum.MiddleMiddle,
    5.0,       // Length (m)
    "1,0,0",   // Local X axis
    "0,1,0",   // Local Y axis
    "0,0,1",   // Local Z axis
    0.1,       // Begin node X offset
    0.1,       // End node X offset
    0,         // Begin node Y offset
    0,         // End node Y offset
    0,         // Begin node Z offset
    0,         // End node Z offset
    "Fixed",   // Start fixity
    "Pinned"   // End fixity
);
model.AddXmiBeam(beam);
```

### Column Example

```csharp
var column = new XmiColumn(
    "col-1",
    "Ground Floor Column",
    "ifc-guid-col",
    "COL-1",
    "Concrete support column",
    XmiStructuralCurveMemberSystemLineEnum.MiddleMiddle,
    3.5,       // Length (m)
    "1,0,0",   // Local X axis
    "0,1,0",   // Local Y axis
    "0,0,1",   // Local Z axis
    0, 0, 0, 0, 0, 0,  // No offsets
    "Fixed",   // Start fixity
    "Fixed"    // End fixity
);
model.AddXmiColumn(column);
```

### Slab and Wall Examples

```csharp
var slab = new XmiSlab(
    "slab-1",
    "Floor Slab",
    "ifc-guid-slab",
    "SLAB-1",
    "200mm concrete slab"
);
model.AddXmiSlab(slab);

var wall = new XmiWall(
    "wall-1",
    "Shear Wall",
    "ifc-guid-wall",
    "WALL-1",
    "Structural shear wall"
);
model.AddXmiWall(wall);
```

## Creating Analytical Elements

### Structural Curve Member

```csharp
using XmiSchema.Core.Models.Entities.StructuralAnalytical;

var curveMember = new XmiStructuralCurveMember(
    "cur-1",
    "Analytical Beam",
    "ifc-guid-cur",
    "CUR-1",
    "Analytical representation",
    XmiStructuralCurveMemberTypeEnum.Beam,
    XmiStructuralCurveMemberSystemLineEnum.MiddleMiddle,
    5.0,       // Length
    "1,0,0",   // Local X
    "0,1,0",   // Local Y
    "0,0,1",   // Local Z
    0.1, 0.1, 0, 0, 0, 0,  // Offsets
    "Fixed",   // Start fixity
    "Pinned"   // End fixity
);
model.AddXmiStructuralCurveMember(curveMember);
```

### Point Connections and Geometry

```csharp
using XmiSchema.Core.Geometries;

// Create points
var startPoint = model.CreatePoint3D(0, 0, 0);
var endPoint = model.CreatePoint3D(5, 0, 0);

// Create line geometry
var line = new XmiLine3D(
    "line-1",
    "Member Centerline",
    "ifc-guid-line",
    "LINE-1",
    "Beam centerline",
    startPoint,
    endPoint
);
model.AddXmiLine3D(line);

// Create point connections
var pointConn1 = new XmiStructuralPointConnection(
    "pc-1",
    "Node 1",
    "ifc-guid-pc1",
    "PC-1",
    "Start node"
);
model.AddXmiStructuralPointConnection(pointConn1);
```

## Creating Relationships

### Linking Physical to Analytical

```csharp
using XmiSchema.Core.Models.Relationships;

// Link beam to analytical curve member
var hasStructuralCurve = new XmiHasStructuralCurveMember(beam, curveMember);
model.AddXmiHasStructuralCurveMember(hasStructuralCurve);
```

### Assigning Materials and Sections

```csharp
// Assign material to cross-section
var hasMaterial = new XmiHasMaterial(crossSection, material);
model.AddXmiHasMaterial(hasMaterial);

// Assign cross-section to curve member
var hasSection = new XmiHasCrossSection(curveMember, crossSection);
model.AddXmiHasCrossSection(hasSection);
```

### Linking Geometry

```csharp
// Link geometry to curve member
var hasGeometry = new XmiHasLine3D(curveMember, line);
model.AddXmiHasLine3D(hasGeometry);

// Link point to point connection
var hasPoint = new XmiHasPoint3D(pointConn1, startPoint);
model.AddXmiHasPoint3D(hasPoint);
```

### Assigning to Storeys

```csharp
// Create a storey
var storey = new XmiStorey(
    "str-1",
    "Level 1",
    "ifc-guid-str",
    "STR-1",
    "Ground floor",
    0.0,       // Elevation
    1000,      // Mass per area
    "Fx",      // Reaction X
    "Fy",      // Reaction Y
    "Fz"       // Reaction Z
);
model.AddXmiStorey(storey);

// Link point connection to storey
var hasStorey = new XmiHasStorey(pointConn1, storey);
model.AddXmiHasStorey(hasStorey);
```

## Working with XmiManager

### Building and Exporting JSON

```csharp
using XmiSchema.Core.Manager;

// Create manager and add model
var manager = new XmiManager();
manager.Models.Add(model);

// Build JSON representation
string json = manager.BuildJson();

// Save to file
System.IO.File.WriteAllText("output.json", json);
```

### Deserializing from JSON

```csharp
string json = System.IO.File.ReadAllText("input.json");
var manager = XmiManager.FromJson(json);

// Access models
foreach (var model in manager.Models)
{
    Console.WriteLine($"Model has {model.XmiBeams.Count} beams");
    Console.WriteLine($"Model has {model.XmiColumns.Count} columns");
}
```

## Complete Example: Simple Frame

```csharp
using XmiSchema.Core.Models;
using XmiSchema.Core.Models.Entities.Physical;
using XmiSchema.Core.Models.Entities.StructuralAnalytical;
using XmiSchema.Core.Manager;

// Initialize
var model = new XmiModel();

// Create material
var steel = new XmiMaterial(
    "mat-steel", "Steel", "guid", "STEEL", "A992",
    XmiMaterialTypeEnum.Steel,
    345, 78.5, "200000", "80000", "0.3", 1.2
);
model.AddXmiMaterial(steel);

// Create section
var wSection = new XmiCrossSection(
    "sec-w12", "W12x26", "guid", "W12",
    "Wide flange", XmiShapeEnum.IShape,
    new IShapeParameters(0.31, 0.165, 0.0094, 0.00635, 0.0095),
    0.00497, 0.0000234, 0.0000567, 0.000891,
    0.000234, 0.00567, 0.0034, 0.134, 0.0000012
);
model.AddXmiStructuralCrossSection(wSection);

// Create beam
var beam = new XmiBeam(
    "beam-1", "Floor Beam", "guid", "BEAM-1", "Main beam",
    XmiStructuralCurveMemberSystemLineEnum.MiddleMiddle,
    6.0, "1,0,0", "0,1,0", "0,0,1",
    0, 0, 0, 0, 0, 0, "Fixed", "Fixed"
);
model.AddXmiBeam(beam);

// Create analytical member
var analytical = new XmiStructuralCurveMember(
    "cur-1", "Analytical", "guid", "CUR-1", "Analysis",
    XmiStructuralCurveMemberTypeEnum.Beam,
    XmiStructuralCurveMemberSystemLineEnum.MiddleMiddle,
    6.0, "1,0,0", "0,1,0", "0,0,1",
    0, 0, 0, 0, 0, 0, "Fixed", "Fixed"
);
model.AddXmiStructuralCurveMember(analytical);

// Link everything
model.AddXmiHasStructuralCurveMember(
    new XmiHasStructuralCurveMember(beam, analytical));
model.AddXmiHasMaterial(
    new XmiHasMaterial(wSection, steel));
model.AddXmiHasCrossSection(
    new XmiHasCrossSection(analytical, wSection));

// Export
var manager = new XmiManager();
manager.Models.Add(model);
string json = manager.BuildJson();

Console.WriteLine("Model created successfully!");
Console.WriteLine($"JSON size: {json.Length} characters");
```

## Running the Example Project

A complete runnable example is available in the repository:

```bash
dotnet run --project tests/Examples/StructuralGraphSample/StructuralGraphSample.csproj
```

This demonstrates building a full XmiManager instance with storeys, materials, structural members, and prints the resulting graph JSON.

[Back to Documentation Home](../)
