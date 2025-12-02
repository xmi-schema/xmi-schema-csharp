# XmiStructuralCrossSection

Represents the cross-sectional properties of structural members.

## Overview

`XmiStructuralCrossSection` defines the geometric and material properties of a cross-section used by structural members. It includes shape information, sectional properties (area, moment of inertia, etc.), and material assignment.

## Location

`Models/Entities/XmiStructuralCrossSection.cs`

## Inheritance

```
XmiBaseEntity → XmiStructuralCrossSection
```

## Properties

### Material Assignment

- **Material** (`XmiStructuralMaterial`): Material assigned to this cross-section

### Geometric Definition

- **Shape** (`XmiShapeEnum`): Shape type (Rectangular, Circular, I-Section, etc.)
- **Parameters** (`string[]`): Shape-specific dimensional parameters
  - For Rectangular: [width, height]
  - For I-Section: [flange width, flange thickness, web height, web thickness]
  - For Circular: [diameter]

### Section Properties

#### Area Properties
- **Area** (`double`): Cross-sectional area (m² or in²)

#### Moment of Inertia
- **SecondMomentOfAreaXAxis** (`double`): Ixx - Second moment of area about X-axis
- **SecondMomentOfAreaYAxis** (`double`): Iyy - Second moment of area about Y-axis

#### Radius of Gyration
- **RadiusOfGyrationXAxis** (`double`): rx - Radius of gyration about X-axis
- **RadiusOfGyrationYAxis** (`double`): ry - Radius of gyration about Y-axis

#### Section Modulus
- **ElasticModulusXAxis** (`double`): Sx - Elastic section modulus about X-axis
- **ElasticModulusYAxis** (`double`): Sy - Elastic section modulus about Y-axis
- **PlasticModulusXAxis** (`double`): Zx - Plastic section modulus about X-axis
- **PlasticModulusYAxis** (`double`): Zy - Plastic section modulus about Y-axis

#### Torsion
- **TorsionalConstant** (`double`): J - Torsional constant

## Constructor

```csharp
public XmiStructuralCrossSection(
    string id,
    string name,
    string ifcguid,
    string nativeId,
    string description,
    XmiStructuralMaterial material,
    XmiShapeEnum shape,
    string[] parameters,
    double area,
    double secondMomentOfAreaXAxis,
    double secondMomentOfAreaYAxis,
    double radiusOfGyrationXAxis,
    double radiusOfGyrationYAxis,
    double elasticModulusXAxis,
    double elasticModulusYAxis,
    double plasticModulusXAxis,
    double plasticModulusYAxis,
    double torsionalConstant
)
```

## Automatic Relationships

When added to a model via `XmiSchemaModelBuilder`, the following relationship is automatically created:

- **XmiHasStructuralMaterial**: Links to Material

## Usage Example

```csharp
// Create material first
var material = new XmiStructuralMaterial(
    id: "MAT001",
    name: "Steel S355",
    materialType: XmiStructuralMaterialTypeEnum.Steel,
    grade: 355.0,
    eModulus: 210000.0,
    ...
);

// Create rectangular cross-section
var crossSection = new XmiStructuralCrossSection(
    id: "CS001",
    name: "300x600",
    ifcguid: "3Ek5md$oE3kCV0mTFPMYzI",
    nativeId: "RevitId_67890",
    description: "Rectangular concrete section 300x600mm",
    material: material,
    shape: XmiShapeEnum.Rectangular,
    parameters: new[] { "300", "600" },  // width x height (mm)
    area: 0.18,                          // 0.18 m²
    secondMomentOfAreaXAxis: 0.0054,     // Ixx
    secondMomentOfAreaYAxis: 0.00135,    // Iyy
    radiusOfGyrationXAxis: 0.173,        // rx
    radiusOfGyrationYAxis: 0.087,        // ry
    elasticModulusXAxis: 0.018,          // Sx
    elasticModulusYAxis: 0.009,          // Sy
    plasticModulusXAxis: 0.027,          // Zx
    plasticModulusYAxis: 0.0135,         // Zy
    torsionalConstant: 0.00081           // J
);

// Add to model
var builder = new XmiSchemaModelBuilder();
builder.AddEntity(material);
builder.AddEntity(crossSection);

var model = builder.BuildModel();
// Relationship crossSection → material automatically created
```

## Common Cross-Section Shapes

### Rectangular Section

```csharp
shape: XmiShapeEnum.Rectangular
parameters: new[] { "width", "height" }

// Example: 300x600 mm
parameters: new[] { "300", "600" }
```

### I-Section (Wide Flange)

```csharp
shape: XmiShapeEnum.ISection
parameters: new[] { "flangeWidth", "flangeThickness", "webHeight", "webThickness" }

// Example: W24x84
parameters: new[] { "229", "19", "603", "11" }
```

### Circular Section

```csharp
shape: XmiShapeEnum.Circular
parameters: new[] { "diameter" }

// Example: 300mm diameter
parameters: new[] { "300" }
```

## JSON Export Example

```json
{
  "XmiStructuralCrossSection": {
    "ID": "CS001",
    "Name": "300x600",
    "IFCGUID": "3Ek5md$oE3kCV0mTFPMYzI",
    "NativeId": "RevitId_67890",
    "Description": "Rectangular concrete section 300x600mm",
    "EntityType": "XmiStructuralCrossSection",
    "Material": "MAT001",
    "Shape": "Rectangular",
    "Parameters": ["300", "600"],
    "Area": 0.18,
    "SecondMomentOfAreaXAxis": 0.0054,
    "SecondMomentOfAreaYAxis": 0.00135,
    "RadiusOfGyrationXAxis": 0.173,
    "RadiusOfGyrationYAxis": 0.087,
    "ElasticModulusXAxis": 0.018,
    "ElasticModulusYAxis": 0.009,
    "PlasticModulusXAxis": 0.027,
    "PlasticModulusYAxis": 0.0135,
    "TorsionalConstant": 0.00081
  }
}
```

## Sectional Properties Explained

### Area
Total cross-sectional area used for axial stress calculations.

### Second Moment of Area (I)
Resistance to bending about an axis. Larger values indicate greater bending stiffness.

### Radius of Gyration (r)
Distribution of area relative to an axis. Calculated as: r = √(I/A)

### Elastic Section Modulus (S)
Resistance to elastic bending stress. S = I / c (where c is distance to extreme fiber)

### Plastic Section Modulus (Z)
Resistance to plastic bending stress. Used for ultimate strength calculations.

### Torsional Constant (J)
Resistance to torsion (twisting). For circular sections: J = πd⁴/32

## Relationship Graph

```
XmiStructuralMaterial
        ↑
        │ (XmiHasStructuralMaterial)
        │
XmiStructuralCrossSection
        ↑
        │ (XmiHasStructuralCrossSection)
        │
XmiStructuralCurveMember
```

## Related Classes

- **XmiBaseEntity**: Base class
- **XmiStructuralMaterial**: Material properties
- **XmiStructuralCurveMember**: Uses this cross-section
- **XmiShapeEnum**: Shape type enumeration

## See Also

- [XmiStructuralMaterial](XmiStructuralMaterial.README.md) - Material properties
- [XmiStructuralCurveMember](XmiStructuralCurveMember.README.md) - Uses cross-sections
- [XmiBaseEntity](../Bases/XmiBaseEntity.README.md) - Base class
