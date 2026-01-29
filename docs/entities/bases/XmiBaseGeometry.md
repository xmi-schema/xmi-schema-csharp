# XmiBaseGeometry

Base class for geometric primitives in the XmiSchema library.

## Purpose

`XmiBaseGeometry` extends `XmiBaseEntity` to provide a common base for all geometric primitives including points, lines, and arcs. It enables type-safe handling of geometry entities within the graph model.

## Properties

Inherits all properties from `XmiBaseEntity`:

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `ID` | `string` | Yes | Unique identifier for geometry within model |
| `Name` | `string` | Yes | Human-readable name for geometry |
| `ifcGuid` | `string` | No | IFC GUID for BIM interoperability |
| `NativeId` | `string` | No | Original identifier from source system |
| `Description` | `string` | No | Detailed description of geometry |
| `EntityName` | `string` | Yes | Type discriminator (e.g., "XmiPoint3D", "XmiLine3D") |
| `Domain` | `XmiBaseEntityDomainEnum` | Yes | Always set to `Geometry` |

## Concrete Implementations

| Class | Description | Usage |
|-------|-------------|--------|
| `XmiPoint3D` | 3D point with X, Y, Z coordinates | Node locations, corner points |
| `XmiLine3D` | Straight line between two points | Beam axes, wall edges |
| `XmiArc3D` | Circular arc with center and radius | Curved beams, arches |

## Characteristics

### Geometry Domain
All geometric primitives have `Domain` set to `XmiBaseEntityDomainEnum.Geometry`, enabling:

- Filtering entities by geometric vs. domain entities
- Type-safe operations on geometry collections
- Separate handling for geometry in serialization

### Tolerance-Aware Equality
Geometric primitives implement tolerance-based equality comparison:

- **XmiPoint3D**: Compares coordinates within configurable tolerance (default 0.001mm)
- Enables automatic deduplication of identical points
- Prevents floating-point precision issues

### Graph Integration
Geometries are connected to domain entities via relationships:

- **XmiHasPoint3D**: Links entities to point coordinates
- **XmiHasLine3D**: Associates entities with line geometry
- **XmiHasGeometry**: Generic geometry association

## Usage Example

```csharp
// Create a point
var point = model.CreatePoint3D(1000.0, 2000.0, 3000.0, "pt-1");

// Create a line
var line = new XmiLine3D(
    id: "line-1",
    name: "Axis Line",
    ifcGuid: "",
    nativeId: "",
    description: "Beam centerline",
    startPoint: point1,
    endPoint: point2
);

// Query all geometry in model
var allGeometry = model.Entities
    .Where(e => e.Domain == XmiBaseEntityDomainEnum.Geometry);
```

## Deduplication

Points, lines, and arcs are automatically deduplicated via factory methods:

```csharp
// These return the same instance due to deduplication
var point1 = model.CreatePoint3D(1000.0, 2000.0, 3000.0, "pt-1");
var point2 = model.CreatePoint3D(1000.0, 2000.0, 3000.0, "pt-2");
// point1 == point2 (same coordinates, same instance)
```

## Best Practices

1. **Use factory methods** (`CreatePoint3D`, etc.) for automatic deduplication
2. **Link geometries** to entities via relationship classes
3. **Set descriptive names** for geometry to aid debugging
4. **Provide ifcGuid** when integrating with BIM workflows
5. **Use tolerance-aware** comparison when checking geometry equality

## Related Entities

- **XmiPoint3D** - 3D point geometry
- **XmiLine3D** - Line geometry
- **XmiArc3D** - Arc geometry
- **XmiHasGeometry**, **XmiHasPoint3D**, **XmiHasLine3D** - Geometry relationships

## Related Enums

- **XmiBaseEntityDomainEnum** - Domain classification

## Notes

- All geometry entities inherit from `XmiBaseGeometry`
- Geometry is separate from domain entities for reusability
- Multiple domain entities can share the same geometry instance
- Geometric tolerance is configurable at model level
