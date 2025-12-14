# Commons Namespace

The `XmiSchema.Entities.Commons` namespace contains shared entities that are used across both physical and structural analytical models. These entities represent fundamental building and structural components that can be referenced by multiple element types.

## Overview

Common entities provide the foundational data structures for:
- **Material Properties**: Physical and mechanical properties of construction materials
- **Cross-Section Profiles**: Geometric and analytical properties of structural profiles
- **Building Levels**: Storey/elevation information for multi-story structures
- **Coordinate Systems**: Local axis definitions for element orientation
- **Segmentation**: Logical divisions along structural members
- **Unit Systems**: Measurement unit definitions and conversions

## Entity Classes

### XmiMaterial

Represents material properties used throughout the structure.

```csharp
public class XmiMaterial : XmiBaseEntity
{
    public XmiStructuralMaterialTypeEnum MaterialType { get; set; }
    public double Grade { get; set; }
    public double UnitWeight { get; set; }
    public double EModulus { get; set; }
    public double GModulus { get; set; }
    public double PoissonRatio { get; set; }
    public double ThermalCoefficient { get; set; }
}
```

**Key Properties:**
- `MaterialType`: Enum defining material category (Concrete, Steel, Timber, etc.)
- `Grade`: Material strength or quality designation
- `UnitWeight`: Weight per unit volume (kN/m³ or lb/ft³)
- `EModulus`: Young's modulus of elasticity
- `GModulus`: Shear modulus of elasticity
- `PoissonRatio`: Poisson's ratio for lateral deformation
- `ThermalCoefficient`: Coefficient of thermal expansion

**Usage:**
```csharp
var concrete = model.CreateXmiMaterial(
    "mat-concrete", "C40 Concrete", "", "MAT_C40", "",
    XmiStructuralMaterialTypeEnum.Concrete, 40.0, 24.0, 33000.0, 13000.0, 0.2, 1.0);
```

### XmiCrossSection

Defines geometric and analytical properties of structural cross-sections.

```csharp
public class XmiCrossSection : XmiBaseEntity
{
    public XmiShapeEnum Shape { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
    public double Area { get; set; }
    public double TorsionConstant { get; set; }
    public double SectionModulusX { get; set; }
    public double SectionModulusY { get; set; }
    public double SectionModulusZ { get; set; }
}
```

**Key Properties:**
- `Shape`: Cross-section shape type (Rectangular, Circular, I-Shape, etc.)
- `Parameters`: Shape-specific parameters (width, height, thickness, etc.)
- `Area`: Cross-sectional area
- `TorsionConstant`: Torsional constant (J)
- `SectionModulus*`: Elastic section moduli about principal axes

**Supported Shapes:**
- Rectangular, Circular, I-Shape, T-Shape, L-Shape, C-Shape
- Hollow Rectangular (Box), Hollow Circular (Pipe)

**Usage:**
```csharp
var section = model.CreateXmiCrossSection(
    "sec-beam", "W16x36", "", "SEC_W16x36", "",
    XmiShapeEnum.IShape, iShapeParameters, 135.0, 0.0005, 472.0, 82.8, 82.8);
```

### XmiStorey

Represents a building level or storey with elevation and mass information.

```csharp
public class XmiStorey : XmiBaseEntity
{
    public double StoreyElevation { get; set; }
    public double StoreyMass { get; set; }
    public double XReaction { get; set; }
    public double YReaction { get; set; }
    public double ZReaction { get; set; }
}
```

**Key Properties:**
- `StoreyElevation`: Height above reference datum (typically ground level)
- `StoreyMass`: Total mass of the storey
- `*Reaction`: Support reactions at the storey level

**Usage:**
```csharp
var groundFloor = model.CreateXmiStorey(
    "storey-1", "Ground Floor", "", "LEVEL_1", "",
    0.0, 15000.0, 0.0, 0.0, 0.0);
```

### XmiAxis

Defines a local coordinate system for element orientation.

```csharp
public class XmiAxis
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
}
```

**Usage:**
```csharp
var localAxis = new XmiAxis(1.0, 0.0, 0.0); // X-axis aligned with element
```

### XmiUnit

Maps entity attributes to unit types for proper conversion.

```csharp
public class XmiUnit : XmiBaseEntity
{
    public string AttributeName { get; set; }
    public XmiUnitEnum UnitType { get; set; }
}
```

### XmiSegment

Represents a logical segment along a structural member.

```csharp
public class XmiSegment : XmiBaseEntity
{
    public XmiSegmentTypeEnum SegmentType { get; set; }
}
```

**Key Features:**
- **Position Handling**: Segment positions managed through `XmiHasSegment` relationships
- **Static Validation**: Methods for sequence validation and boundary checking
- **Geometric Types**: Line, CircularArc, and other segment classifications

**Static Methods:**
- `ValidateSequence()`: Validates proper position sequencing
- `SortByPosition()`: Sorts segments by position
- `CanFormClosedBoundary()`: Checks if segments can form closed loop

**Usage:**
```csharp
var segments = new List<XmiSegment>
{
    new XmiSegment("seg-1", "Segment 1", "", "seg-1", "", XmiSegmentTypeEnum.Line),
    new XmiSegment("seg-2", "Segment 2", "", "seg-2", "", XmiSegmentTypeEnum.Line)
};
var positions = new List<int> { 0, 1 };

// Validate sequence
bool isValid = XmiSegment.ValidateSequence(segments, positions);
```

## Design Patterns

### Factory Method Integration

Common entities are typically created through `XmiModel` factory methods:

```csharp
// Material creation
var material = model.CreateXmiMaterial(id, name, ifcGuid, nativeId, description, 
    materialType, grade, unitWeight, eModulus, gModulus, poissonRatio, thermalCoefficient);

// Cross-section creation
var section = model.CreateXmiCrossSection(id, name, ifcGuid, nativeId, description,
    material, shape, parameters, area, torsionConstant, sectionModulusX, sectionModulusY, sectionModulusZ);

// Storey creation
var storey = model.CreateXmiStorey(id, name, ifcGuid, nativeId, description,
    storeyElevation, storeyMass, xReaction, yReaction, zReaction);
```

### Relationship Management

Common entities are linked to other elements through relationship classes:

- `XmiHasMaterial`: Links elements to materials
- `XmiHasCrossSection`: Links curve members to cross-sections
- `XmiHasStorey`: Associates elements with building levels
- `XmiHasSegment`: Positions segments along curve members

### Validation and Error Handling

Common entities include validation in factory methods:

```csharp
// Required parameters
if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));

// Range validation
if (unitWeight <= 0) throw new ArgumentException("Unit weight must be positive", nameof(unitWeight));
```

## Best Practices

### 1. Material Management
- Use consistent material definitions across the entire model
- Include complete mechanical properties for analysis
- Link materials to IFC standards via `ifcGuid`

### 2. Cross-Section Definition
- Use appropriate shape types for structural profiles
- Provide accurate geometric parameters
- Include analytical section properties for design

### 3. Storey Organization
- Define clear elevation hierarchy
- Include mass information for dynamic analysis
- Use consistent naming conventions

### 4. Segment Usage
- Provide positions array when creating segments
- Validate segment sequences before use
- Use appropriate segment types for geometry

### 5. Unit Consistency
- Define units for all measurable attributes
- Use `XmiUnit` entities for unit conversion
- Maintain consistent unit systems across the model

## Integration Examples

### Complete Structural Model

```csharp
// Create common entities
var material = model.CreateXmiMaterial("mat-1", "Steel", "", "MAT_1", "",
    XmiStructuralMaterialTypeEnum.Steel, 7850.0, 210000.0, 81000.0, 0.3, 1.2);

var section = model.CreateXmiCrossSection("sec-1", "W16x36", "", "SEC_1", "",
    material, XmiShapeEnum.IShape, iShapeParams, 135.0, 0.0005, 472.0, 82.8, 82.8);

var storey = model.CreateXmiStorey("storey-1", "Level 1", "", "LEVEL_1", "",
    0.0, 15000.0, 0.0, 0.0, 0.0);

// Use in structural elements
var beam = model.CreateXmiStructuralCurveMember("beam-1", "Main Beam", "", "BEAM_1", "",
    material, section, storey, XmiStructuralCurveMemberTypeEnum.Beam,
    nodes, segments, positions, XmiSystemLineEnum.MiddleMiddle,
    startNode, endNode, 6.0, axisX, axisY, axisZ,
    0, 0, 0, 0, 0, 0, "Fixed", "Pinned");
```

## File Organization

```
Entities/Commons/
├── XmiAxis.cs           # Local coordinate system
├── XmiCrossSection.cs   # Profile geometry and properties
├── XmiMaterial.cs       # Material properties
├── XmiSegment.cs        # Member segmentation
├── XmiStorey.cs         # Building levels
└── XmiUnit.cs           # Unit definitions
```

## Related Namespaces

- **XmiSchema.Entities.Physical**: Physical building elements using commons
- **XmiSchema.Entities.StructuralAnalytical**: Analytical elements using commons
- **XmiSchema.Entities.Relationships**: Relationship classes linking commons to other entities
- **XmiSchema.Managers**: Factory methods for creating commons

## See Also

- [API Reference](../api/) for detailed method documentation
- [Examples](../examples/) for complete usage samples
- [Relationships](../Relationships/) for connection patterns
- [Physical Elements](../Physical/) for usage in building components
- [Structural Analytical](../StructuralAnalytical/) for usage in analysis models