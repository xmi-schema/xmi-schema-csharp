# XmiHasStructuralCrossSection

Relationship linking structural members to their cross-section definitions.

## Overview

`XmiHasStructuralCrossSection` represents an association between structural curve members (beams, columns, braces) and their cross-sectional property definitions. This relationship is essential for defining the structural properties of linear members.

## Location

`Models/Relationships/XmiHasStructuralCrossSection.cs`

## Inheritance

```
XmiBaseRelationship → XmiHasStructuralCrossSection
```

## UML Type

**Association** - Represents a reference relationship where members use cross-section definitions for their geometric and material properties.

## Constructors

### Full Constructor

```csharp
public XmiHasStructuralCrossSection(
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
public XmiHasStructuralCrossSection(
    XmiBaseEntity source,
    XmiBaseEntity target
)
```

## Automatic Creation

This relationship is automatically created by `ExtensionRelationshipExporter` when a `XmiStructuralCurveMember` has a `CrossSection` property set:

```csharp
if (entity is XmiStructuralCurveMember curve && curve.CrossSection != null)
{
    _relationshipManager.AddRelationship(
        new XmiHasStructuralCrossSection(curve, curve.CrossSection)
    );
}
```

## Usage Example

```csharp
// Create cross-section
var crossSection = new XmiStructuralCrossSection(
    id: "CS001",
    name: "300x600",
    material: material,
    shape: XmiShapeEnum.Rectangular,
    ...
);

// Create member with cross-section
var beam = new XmiStructuralCurveMember(
    id: "B001",
    name: "Beam-1",
    crossSection: crossSection,  // Reference to cross-section
    ...
);

// Add to model
var builder = new XmiSchemaModelBuilder();
builder.AddEntity(crossSection);
builder.AddEntity(beam);

var model = builder.BuildModel();
// Relationship automatically created: beam → crossSection
```

## Graph Representation

```
XmiStructuralCurveMember ──[XmiHasStructuralCrossSection]──> XmiStructuralCrossSection
```

## Dependency Chain

```
XmiStructuralCurveMember
        │
        │ [XmiHasStructuralCrossSection]
        ↓
XmiStructuralCrossSection
        │
        │ [XmiHasStructuralMaterial]
        ↓
XmiStructuralMaterial
```

## JSON Export Example

```json
{
  "XmiHasStructuralCrossSection": {
    "ID": "rel-001",
    "Source": "B001",
    "Target": "CS001",
    "Name": "XmiHasStructuralCrossSection",
    "Description": "",
    "EntityType": "XmiHasStructuralCrossSection",
    "UmlType": "Association"
  }
}
```

## Related Classes

- **XmiBaseRelationship**: Base class
- **XmiStructuralCurveMember**: Source entity (beams, columns, braces)
- **XmiStructuralCrossSection**: Target entity (cross-section definition)
- **ExtensionRelationshipExporter**: Creates this relationship automatically

## See Also

- [XmiStructuralCurveMember](../Entities/XmiStructuralCurveMember.README.md) - Source entity
- [XmiStructuralCrossSection](../Entities/XmiStructuralCrossSection.README.md) - Target entity
- [XmiBaseRelationship](../Bases/XmiBaseRelationship.README.md) - Base class
