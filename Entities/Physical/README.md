# Physical Namespace

The `XmiSchema.Entities.Physical` namespace contains entities that represent real-world building components. These physical elements are the tangible objects that make up a building structure, such as beams, columns, slabs, and walls.

## Overview

Physical entities represent the actual construction elements that would be found on a building site. They are typically linked to analytical elements through relationships for structural analysis workflows.

## Entity Classes

### XmiBeam

Represents horizontal structural members that primarily resist bending and shear forces.

```csharp
public class XmiBeam : XmiBasePhysicalEntity
{
    public XmiSystemLineEnum SystemLine { get; set; }
    public double Length { get; set; }
}
```

**Key Properties:**
- `SystemLine`: Defines the geometric reference line for the beam
- `Length`: Total length of the beam

**Common Usage:**
- Floor beams and joists
- Roof beams and rafters
- Horizontal framing members
- Girders and purlins

**Example:**
```csharp
var beam = model.CreateXmiBeam(
    "beam-1", "Main Floor Beam", "", "BEAM_1", "",
    material, segments, positions, XmiSystemLineEnum.MiddleMiddle, 6.0,
    axisX, axisY, axisZ, 0, 0, 0, 0, 0, 0);
```

### XmiColumn

Represents vertical structural members that primarily resist axial compression forces.

```csharp
public class XmiColumn : XmiBasePhysicalEntity
{
    public XmiSystemLineEnum SystemLine { get; set; }
    public double Length { get; set; }
}
```

**Key Properties:**
- `SystemLine`: Defines the geometric reference line for the column
- `Length`: Total height of the column

**Common Usage:**
- Building columns and posts
- Vertical support members
- Load-bearing vertical elements
- Foundation piles and piers

**Example:**
```csharp
var column = model.CreateXmiColumn(
    "col-1", "Main Column", "", "COL_1", "",
    material, segments, positions, XmiSystemLineEnum.MiddleMiddle, 3.0,
    axisX, axisY, axisZ, 0, 0, 0, 0, 0, 0);
```

### XmiSlab

Represents horizontal planar elements that provide floor, roof, or wall surfaces.

```csharp
public class XmiSlab : XmiBasePhysicalEntity
{
    public double Thickness { get; set; }
}
```

**Key Properties:**
- `Thickness`: Uniform thickness of the slab

**Common Usage:**
- Floor slabs and decks
- Roof slabs and panels
- Wall panels and cladding
- Foundation mats

**Example:**
```csharp
var slab = model.CreateXmiSlab(
    "slab-1", "Ground Floor", "", "SLAB_1", "",
    material, segments, positions, 0.0,
    axisX, axisY, axisZ, 0.2);
```

### XmiWall

Represents vertical planar elements that enclose space or resist lateral loads.

```csharp
public class XmiWall : XmiBasePhysicalEntity
{
    public double Height { get; set; }
}
```

**Key Properties:**
- `Height`: Total height of the wall

**Common Usage:**
- Exterior walls and partitions
- Shear walls and load-bearing walls
- Retaining walls and basement walls
- Curtain walls and partitions

**Example:**
```csharp
var wall = model.CreateXmiWall(
    "wall-1", "Exterior Wall", "", "WALL_1", "",
    material, segments, positions, 0.0,
    axisX, axisY, axisZ, 3.0);
```

## Design Patterns

### Base Class Inheritance

All physical entities inherit from `XmiBasePhysicalEntity`, which provides:

```csharp
public abstract class XmiBasePhysicalEntity : XmiBaseEntity
{
    public XmiSystemLineEnum SystemLine { get; set; }
    public XmiAxis LocalAxisX { get; set; }
    public XmiAxis LocalAxisY { get; set; }
    public XmiAxis LocalAxisZ { get; set; }
    public double BeginNodeXOffset { get; set; }
    public double EndNodeXOffset { get; set; }
    public double BeginNodeYOffset { get; set; }
    public double EndNodeYOffset { get; set; }
    public double BeginNodeZOffset { get; set; }
    public double EndNodeZOffset { get; set; }
}
```

### Common Properties

**SystemLine**: Defines the geometric reference line for the physical element
- `MiddleMiddle`: Center line through element centroid
- `TopCenter`: Top center line
- `BottomCenter`: Bottom center line
- `LeftCenter`, `RightCenter`: Side center lines

**Local Axes**: Define the element's local coordinate system
- Used for proper orientation of loads and results
- Essential for structural analysis integration

**Offsets**: Define geometric offsets from reference points
- Allow for connection eccentricities
- Support complex connection geometries

## Factory Method Integration

Physical entities are created through `XmiModel` factory methods:

```csharp
// Beam creation
var beam = model.CreateXmiBeam(
    id, name, ifcGuid, nativeId, description,
    material, segments, positions, systemLine, length,
    localAxisX, localAxisY, localAxisZ,
    beginNodeXOffset, endNodeXOffset,
    beginNodeYOffset, endNodeYOffset,
    beginNodeZOffset, endNodeZOffset);

// Column creation
var column = model.CreateXmiColumn(/* similar parameters */);

// Slab creation
var slab = model.CreateXmiSlab(
    id, name, ifcGuid, nativeId, description,
    material, segments, positions, zOffset,
    localAxisX, localAxisY, localAxisZ, thickness);

// Wall creation
var wall = model.CreateXmiWall(
    id, name, ifcGuid, nativeId, description,
    material, segments, positions, zOffset,
    localAxisX, localAxisY, localAxisZ, height);
```

## Relationship Management

Physical entities are typically linked to:

- **Materials**: Via `XmiHasMaterial` relationships
- **Cross-Sections**: Via `XmiHasCrossSection` relationships
- **Storeys**: Via `XmiHasStorey` relationships
- **Segments**: Via `XmiHasSegment` relationships
- **Analytical Elements**: Via `XmiHasStructuralCurveMember` or `XmiHasStructuralSurfaceMember`

## Usage Patterns

### Complete Building Model

```csharp
// Create materials
var concrete = model.CreateXmiMaterial("mat-conc", "Concrete", "", "MAT_1", "",
    XmiStructuralMaterialTypeEnum.Concrete, 30.0, 24.0, 25000.0, 12500.0, 0.2, 1.0);

var steel = model.CreateXmiMaterial("mat-steel", "Steel", "", "MAT_2", "",
    XmiStructuralMaterialTypeEnum.Steel, 50.0, 78.5, 200000.0, 81000.0, 0.3, 1.2);

// Create cross-sections
var columnSection = model.CreateXmiCrossSection("cs-col", "Column Section", "", "CS_1", "",
    concrete, XmiShapeEnum.Rectangular, rectParams, 0.4, 0.0005, 0.0005, 0.0005);

var beamSection = model.CreateXmiCrossSection("cs-beam", "Beam Section", "", "CS_2", "",
    steel, XmiShapeEnum.IShape, iShapeParams, 135.0, 0.005, 472.0, 82.8, 82.8);

// Create physical elements
var columns = new List<XmiColumn>
{
    model.CreateXmiColumn("col-1", "Column 1", "", "COL_1", "", concrete, null, null,
        XmiSystemLineEnum.MiddleMiddle, 3.0, axisX, axisY, axisZ, 0, 0, 0, 0, 0, 0),
    model.CreateXmiColumn("col-2", "Column 2", "", "COL_2", "", concrete, null, null,
        XmiSystemLineEnum.MiddleMiddle, 3.0, axisX, axisY, axisZ, 0, 0, 0, 0, 0, 0)
};

var beams = new List<XmiBeam>
{
    model.CreateXmiBeam("beam-1", "Floor Beam 1", "", "BEAM_1", "", steel, null, null,
        XmiSystemLineEnum.MiddleMiddle, 6.0, axisX, axisY, axisZ, 0, 0, 0, 0, 0, 0),
    model.CreateXmiBeam("beam-2", "Floor Beam 2", "", "BEAM_2", "", steel, null, null,
        XmiSystemLineEnum.MiddleMiddle, 6.0, axisX, axisY, axisZ, 0, 0, 0, 0, 0, 0)
};

var slabs = new List<XmiSlab>
{
    model.CreateXmiSlab("slab-1", "Ground Floor", "", "SLAB_1", "", concrete, null, null,
        0.0, axisX, axisY, axisZ, 0.2),
    model.CreateXmiSlab("slab-2", "Second Floor", "", "SLAB_2", "", concrete, null, null,
        3.0, axisX, axisY, axisZ, 0.15)
};
```

## Best Practices

### 1. Material Assignment
- Use consistent material definitions across the model
- Include complete mechanical properties for analysis
- Link to IFC standards via `ifcGuid` when available

### 2. Cross-Section Definition
- Use appropriate shape types for structural profiles
- Provide accurate geometric parameters
- Include analytical section properties for design

### 3. Geometric Modeling
- Define proper local coordinate systems
- Use appropriate system lines for element geometry
- Include offsets for connection eccentricities

### 4. Segment Usage
- Provide positions array when segments are used
- Ensure positions array length matches segments count
- Use appropriate segment types for element geometry

### 5. Performance Considerations
- Reuse materials and cross-sections where possible
- Use automatic point deduplication for shared coordinates
- Consider model complexity for large structures

## File Organization

```
Entities/Physical/
├── XmiBeam.cs          # Horizontal structural members
├── XmiColumn.cs        # Vertical structural members  
├── XmiSlab.cs          # Horizontal planar elements
├── XmiWall.cs          # Vertical planar elements
└── README.md            # This file
```

## Related Namespaces

- **XmiSchema.Entities.Commons**: Shared entities (materials, sections, etc.)
- **XmiSchema.Entities.StructuralAnalytical**: Analytical representations
- **XmiSchema.Entities.Relationships**: Connection classes
- **XmiSchema.Managers**: Factory methods and model management

## See Also

- [Commons](../Commons/) for shared entities used by physical elements
- [StructuralAnalytical](../StructuralAnalytical/) for analytical element counterparts
- [Relationships](../Relationships/) for connection patterns
- [API Reference](../../api/) for detailed method documentation