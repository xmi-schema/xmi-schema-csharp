# Entities.Geometries Namespace

The `XmiSchema.Entities.Geometries` namespace contains geometric primitives that represent spatial elements in the XMI schema. All geometry classes inherit from `XmiBaseGeometry`.

## Core Geometric Classes

### XmiPoint3D
Represents a 3D point with tolerance-aware equality comparison.
- **Properties**: `X`, `Y`, `Z` coordinates
- **Key Feature**: Automatic deduplication when created via `XmiModel.CreatePoint3D()`
- **Usage**: Primary building block for all geometric constructions

### XmiLine3D
Represents a straight line segment between two points.
- **Properties**: `StartPoint`, `EndPoint`
- **Creation**: Use `XmiModel.CreateLine3D(startPoint, endPoint)`
- **Applications**: Centerlines, edges, alignment references

### XmiArc3D
Represents a circular arc in 3D space.
- **Properties**: `StartPoint`, `EndPoint`, `CenterPoint`, `Radius`
- **Creation**: Use `XmiModel.CreateArc3D(startPoint, endPoint, centerPoint)`
- **Applications**: Curved beams, arches, rounded corners

## Usage Patterns

### Point Deduplication
Always create points through the model to avoid duplicates:

```csharp
// Correct - automatic deduplication
var point1 = model.CreatePoint3D(0, 0, 0);
var point2 = model.CreatePoint3D(0, 0, 0); // Returns same instance

// Incorrect - creates duplicates
var point3 = new XmiPoint3D("p3", 0, 0, 0);
```

### Building Geometric Relationships
Connect geometries to entities using relationship classes:

```csharp
var beam = model.CreateXmiBeam(...);
var line = model.CreateLine3D(startPoint, endPoint);
var hasGeometry = new XmiHasLine3D(beam, line);
model.AddRelationship(hasGeometry);
```

### Geometric Tolerance
Points use tolerance-based equality for robust comparison:
- Default tolerance handles floating-point precision
- Essential for geometric consistency in large models

## Design Considerations

1. **Immutable Properties** - Geometric properties are set at creation
2. **Reference Sharing** - Points are shared to reduce redundancy
3. **Type Safety** - Strong typing prevents geometric errors
4. **Serialization** - All geometries serialize to JSON with proper discriminators

## Integration with Other Namespaces

- **Entities.Bases** - Inherits from `XmiBaseGeometry`
- **Entities.Relationships** - Connected via `XmiHas*` relationship classes
- **Managers** - Created and managed through `XmiModel` factory methods
- **Entities.Physical** - Physical elements reference geometries
- **Entities.StructuralAnalytical** - Analytical elements reference geometries

## Common Use Cases

1. **Structural Layouts** - Define centerlines and reference points
2. **Building Geometry** - Create walls, slabs, and openings
3. **Analysis Models** - Define analytical member geometry
4. **Coordinate Systems** - Establish reference frames and origins

## Testing

Tests covering equality and serialization edge cases should live in `tests/XmiSchema.Core.Tests/Geometries`.
