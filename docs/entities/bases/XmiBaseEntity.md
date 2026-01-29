# XmiBaseEntity

Base class for all entities in the XmiSchema library. Provides core identity and classification properties.

## Purpose

`XmiBaseEntity` is the root base class from which all domain entities inherit. It provides essential properties for entity identification, classification, and interoperability with external systems.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `ID` | `string` | Yes | Unique identifier for the entity within the model |
| `Name` | `string` | Yes | Human-readable name for the entity |
| `ifcGuid` | `string` | No | IFC (Industry Foundation Classes) GUID for BIM interoperability |
| `NativeId` | `string` | No | Original identifier from source system (e.g., Revit, ETABS, Tekla) |
| `Description` | `string` | No | Detailed description of the entity |
| `EntityName` | `string` | Yes | Type discriminator for JSON polymorphic serialization (e.g., "XmiBeam", "XmiMaterial") |
| `Domain` | `XmiBaseEntityDomainEnum` | Yes | Domain classification (Physical, StructuralAnalytical, Geometry, Functional, Shared) |

## Inheritance Hierarchy

```
XmiBaseEntity
├── XmiBasePhysicalEntity (beams, columns, slabs, walls)
├── XmiBaseStructuralAnalyticalEntity (curve members, surface members, point connections)
├── XmiBaseGeometry (points, lines, arcs)
└── Domain Entities (materials, cross-sections, storeys, units, segments)
```

## Key Characteristics

### Identity Management
- **ID**: Primary identifier within the XmiSchema model
- **ifcGuid**: Enables round-trip with IFC-based BIM workflows
- **NativeId**: Preserves original identifiers from source systems

### Classification
- **EntityName**: Used by JSON serializer to determine concrete type during deserialization
- **Domain**: Enables filtering and routing of entities by semantic category

### Interoperability
- **ifcGuid**: Links to IFC elements for BIM integration
- **NativeId**: Traceability to original source system

## Usage Example

```csharp
// Create a beam (inherits from XmiBaseEntity)
var beam = new XmiBeam(
    id: "beam-1",
    name: "Beam B-1",
    ifcGuid: "2O2Fr$t4X7Zf8NOew3FL9r",
    nativeId: "B-1_Revit",
    description: "Main floor beam in grid line A",
    domain: XmiBaseEntityDomainEnum.Physical
);

// Access base properties
Console.WriteLine(beam.ID);          // "beam-1"
Console.WriteLine(beam.Name);        // "Beam B-1"
Console.WriteLine(beam.EntityName);   // "XmiBeam"
Console.WriteLine(beam.Domain);      // Physical
```

## Best Practices

1. **Always use unique IDs** within a model to prevent conflicts
2. **Include ifcGuid** when working with BIM workflows that require IFC integration
3. **Preserve NativeId** to maintain traceability to source systems
4. **Use meaningful names** for human readability
5. **Set appropriate Domain** for proper entity classification

## Related Entities

- **XmiBeam**, **XmiColumn**, **XmiSlab**, **XmiWall** - Physical entities
- **XmiStructuralCurveMember**, **XmiStructuralSurfaceMember**, **XmiStructuralPointConnection** - Analytical entities
- **XmiMaterial**, **XmiCrossSection** - Shared domain entities

## Related Enums

- **XmiBaseEntityDomainEnum** - Domain classification values

## Notes

- All entities must inherit directly or indirectly from `XmiBaseEntity`
- The `EntityName` property is critical for JSON polymorphic deserialization
- Domain values are used for routing and filtering in workflow systems
