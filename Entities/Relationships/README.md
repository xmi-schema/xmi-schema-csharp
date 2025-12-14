# Entities.Relationships Namespace

The `XmiSchema.Entities.Relationships` namespace contains relationship classes that connect entities as edges in the XMI graph structure. All relationships inherit from `XmiBaseRelationship`.

## Core Relationship Classes

### Geometry Relationships
- **XmiHasGeometry** - Generic geometry association
- **XmiHasPoint3D** - Point geometry association
- **XmiHasLine3D** - Line geometry association
- **XmiHasArc3D** - Arc geometry association

### Property Relationships
- **XmiHasMaterial** - Material assignment
- **XmiHasCrossSection** - Cross-section assignment
- **XmiHasSegment** - Segment association with positioning
- **XmiHasStorey** - Storey/level assignment

### Structural Relationships
- **XmiHasStructuralCurveMember** - Curve member reference
- **XmiHasStructuralPointConnection** - Point connection reference

## Constructor Patterns

### Dual Constructor Design
All relationship classes provide two constructors:

```csharp
// Fully described constructor
public XmiHasMaterial(string id, XmiBaseEntity source, XmiBaseEntity target, ...)

// Shorthand constructor with auto-generated ID
public XmiHasMaterial(XmiBaseEntity source, XmiBaseEntity target, ...)
```

### Usage Examples

```csharp
// Using shorthand constructor (recommended)
var beam = model.CreateXmiBeam(...);
var material = model.CreateXmiMaterial(...);
var hasMaterial = new XmiHasMaterial(beam, material);
model.AddRelationship(hasMaterial);

// Using fully described constructor
var hasMaterial2 = new XmiHasMaterial("rel_001", beam, material);
```

## Relationship Properties

### Common Properties
- **Source** - The originating entity
- **Target** - The destination entity
- **ID** - Unique relationship identifier
- **Description** - Optional description
- **Stereotypes** - UML stereotypes for semantic enrichment

### Specialized Properties
- **XmiHasSegment.Position** - Position along parent element
- **XmiHasSegment.Index** - Segment index ordering

## Graph Architecture

### Directed Edges
Relationships are directed edges connecting:
- **Physical Elements** → **Geometries**
- **Analytical Elements** → **Geometries**
- **Elements** → **Materials/Cross-Sections**
- **Elements** → **Storeys/Segments**

### Common Source → Target Patterns
| Relationship | Purpose | Common Source → Target |
| --- | --- | --- |
| `XmiHasGeometry` | Binds an entity to its geometry | Member/Surface → `XmiBaseGeometry` |
| `XmiHasLine3D` | Specialized line reference | `XmiStructuralCurveMember` → `XmiLine3D` |
| `XmiHasPoint3D` | Point connection coordinates | `XmiStructuralPointConnection` → `XmiPoint3D` |
| `XmiHasSegment` | Curve member segments | `XmiStructuralCurveMember` → `XmiSegment` |
| `XmiHasMaterial` | Material assignment | Cross section/member → `XmiMaterial` |
| `XmiHasStructuralPointConnection` | Analytical node dependency | `XmiStructuralCurveMember` → `XmiStructuralPointConnection` |
| `XmiHasCrossSection` | Cross-section specification | `XmiStructuralCurveMember` → `XmiCrossSection` |
| `XmiHasStorey` | Storey level placement | Point connection → `XmiStorey` |

## Usage Patterns

### Connecting Elements to Geometries
```csharp
var beam = model.CreateXmiBeam(...);
var line = model.CreateLine3D(startPoint, endPoint);
var hasLine3D = new XmiHasLine3D(beam, line);
model.AddRelationship(hasLine3D);
```

### Assigning Properties
```csharp
var column = model.CreateXmiColumn(...);
var material = model.CreateXmiMaterial(...);
var crossSection = model.CreateXmiCrossSection(...);

model.AddRelationship(new XmiHasMaterial(column, material));
model.AddRelationship(new XmiHasCrossSection(column, crossSection));
```

### Segment Positioning
```csharp
var curveMember = model.CreateXmiStructuralCurveMember(...);
var segment = model.CreateXmiSegment(...);

var hasSegment = new XmiHasSegment(curveMember, segment)
{
    Position = 0.5, // Mid-span position
    Index = 0        // First segment
};
model.AddRelationship(hasSegment);
```

## Design Principles

1. **Type Safety** - Strongly typed source/target relationships
2. **Semantic Clarity** - Relationship names express intent
3. **Graph Integrity** - Relationships maintain graph structure
4. **Serialization** - Proper JSON serialization with discriminators
5. **Extensibility** - Clear patterns for custom relationships

## Integration with Other Namespaces

- **Entities.Bases** - Inherits from `XmiBaseRelationship`
- **Entities.* (all)** - Connects entities within the graph
- **Managers** - Managed through `XmiModel.AddRelationship()`
- **Serialization** - All relationships serialize to JSON

## Best Practices

1. **Use shorthand constructors** for automatic ID generation
2. **Add relationships to model** immediately after creation
3. **Follow naming conventions** - `XmiHas{TargetType}` pattern
4. **Document complex relationships** with descriptions
5. **Test graph integrity** when creating complex structures

## Testing

Unit tests for relationships should live in `tests/XmiSchema.Core.Tests/Relationships` and cover:
- Constructor behavior
- Serialization/deserialization
- Graph navigation
- Edge cases and error handling
