# XmiHasStructuralMaterial

Relationship linking structural elements to their material definitions.

## Overview

`XmiHasStructuralMaterial` represents an association between structural elements and their material property definitions. This relationship connects cross-sections and surface members to materials, providing the physical and mechanical properties needed for analysis.

## Location

`Models/Relationships/XmiHasStructralMaterial.cs`

**Note**: There is a typo in the filename ("Structral" instead of "Structural"), but the class name is correct.

## Inheritance

```
XmiBaseRelationship → XmiHasStructuralMaterial
```

## UML Type

**Association** - Represents a reference relationship where structural elements use material definitions.

## Constructors

### Full Constructor

```csharp
public XmiHasStructuralMaterial(
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
public XmiHasStructuralMaterial(
    XmiBaseEntity source,
    XmiBaseEntity target
)
```

## Automatic Creation

This relationship is automatically created by `ExtensionRelationshipExporter` for:

### 1. Cross-Section to Material

```csharp
if (entity is XmiStructuralCrossSection crossSection && crossSection.Material != null)
{
    _relationshipManager.AddRelationship(
        new XmiHasStructuralMaterial(crossSection, crossSection.Material)
    );
}
```

### 2. Surface Member to Material

```csharp
if (entity is XmiStructuralSurfaceMember surface && surface.Material != null)
{
    _relationshipManager.AddRelationship(
        new XmiHasStructuralMaterial(surface, surface.Material)
    );
}
```

## Usage Examples

### Cross-Section with Material

```csharp
// Create material
var steel = new XmiStructuralMaterial(
    id: "MAT001",
    name: "Steel S355",
    materialType: XmiStructuralMaterialTypeEnum.Steel,
    grade: 355.0,
    ...
);

// Create cross-section with material
var crossSection = new XmiStructuralCrossSection(
    id: "CS001",
    name: "IPE 300",
    material: steel,  // Reference to material
    ...
);

// Add to model
var builder = new XmiSchemaModelBuilder();
builder.AddEntity(steel);
builder.AddEntity(crossSection);

var model = builder.BuildModel();
// Relationship automatically created: crossSection → steel
```

### Surface Member with Material

```csharp
// Create material
var concrete = new XmiStructuralMaterial(
    id: "MAT002",
    name: "Concrete C30",
    materialType: XmiStructuralMaterialTypeEnum.Concrete,
    ...
);

// Create slab with material
var slab = new XmiStructuralSurfaceMember(
    id: "SLB001",
    name: "Floor-Slab",
    material: concrete,  // Direct material reference
    thickness: 0.15,
    ...
);

builder.AddEntity(concrete);
builder.AddEntity(slab);

model = builder.BuildModel();
// Relationship automatically created: slab → concrete
```

## Graph Representations

### Via Cross-Section (Indirect)

```
XmiStructuralCurveMember
        │
        │ [XmiHasStructuralCrossSection]
        ↓
XmiStructuralCrossSection ──[XmiHasStructuralMaterial]──> XmiStructuralMaterial
```

### Direct (Surface Members)

```
XmiStructuralSurfaceMember ──[XmiHasStructuralMaterial]──> XmiStructuralMaterial
```

## JSON Export Example

```json
{
  "XmiHasStructuralMaterial": {
    "ID": "rel-002",
    "Source": "CS001",
    "Target": "MAT001",
    "Name": "XmiHasStructuralMaterial",
    "Description": "",
    "EntityType": "XmiHasStructuralMaterial",
    "UmlType": "Association"
  }
}
```

## Common Patterns

### Material Reuse

Multiple cross-sections/elements can reference the same material:

```csharp
var steel = new XmiStructuralMaterial("MAT001", "Steel S355", ...);

var cs1 = new XmiStructuralCrossSection("CS001", "IPE 300", material: steel, ...);
var cs2 = new XmiStructuralCrossSection("CS002", "IPE 400", material: steel, ...);
var cs3 = new XmiStructuralCrossSection("CS003", "HEB 200", material: steel, ...);

// All create separate relationships to the same material
// cs1 → steel
// cs2 → steel
// cs3 → steel
```

## Related Classes

- **XmiBaseRelationship**: Base class
- **XmiStructuralMaterial**: Target entity (material definition)
- **XmiStructuralCrossSection**: Source entity (for curve members)
- **XmiStructuralSurfaceMember**: Source entity (for surface elements)
- **ExtensionRelationshipExporter**: Creates this relationship automatically

## See Also

- [XmiStructuralMaterial](../Entities/XmiStructuralMaterial.README.md) - Material definition
- [XmiStructuralCrossSection](../Entities/XmiStructuralCrossSection.README.md) - Uses material
- [XmiStructuralSurfaceMember](../Entities/XmiStructuralSurfaceMember.README.md) - Uses material
- [XmiBaseRelationship](../Bases/XmiBaseRelationship.README.md) - Base class
