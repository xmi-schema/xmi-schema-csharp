# XmiBaseEnum

Base class for enumeration types with XML documentation attributes in XmiSchema library.

## Purpose

`XmiBaseEnum` provides a common base for domain-specific enumerations that require custom XML serialization attributes. It enables control over how enum values are serialized to and deserialized from JSON.

## Characteristics

### XML Serialization Control

Domain enums in XmiSchema use custom serialization to:

- **Map enum names to specific string values** during JSON serialization
- **Parse string values back to enums** during JSON deserialization
- **Support legacy naming conventions** while maintaining modern enum design
- **Enable backward compatibility** with existing data formats

### EnumValueAttribute

The `EnumValueAttribute` marks enum members with their serialized string value:

```csharp
public enum XmiMaterialTypeEnum
{
    [EnumValue("Concrete")]
    Concrete = 0,

    [EnumValue("Steel")]
    Steel = 1,

    [EnumValue("Timber")]
    Timber = 2
}
```

During serialization:
- `XmiMaterialTypeEnum.Concrete` → `"Concrete"` (string)
- `"Steel"` → `XmiMaterialTypeEnum.Steel` (enum)

## Enum Categories

### Material Types

| Enum | Purpose | Values |
|-------|---------|---------|
| `XmiMaterialTypeEnum` | Material classification | Concrete, Steel, Timber, Aluminum, Masonry, etc. |

### Structural Member Types

| Enum | Purpose | Values |
|-------|---------|---------|
| `XmiStructuralCurveMemberTypeEnum` | 1D member classification | Beam, Column, Brace, etc. |
| `XmiStructuralSurfaceMemberTypeEnum` | 2D member classification | Slab, Wall, Plate, etc. |
| `XmiSegmentTypeEnum` | Segment geometry type | Line, Arc, etc. |
| `XmiSystemLineEnum` | System line position | TopTop, MiddleMiddle, BottomBottom, etc. |

### Cross-Section Types

| Enum | Purpose | Values |
|-------|---------|---------|
| `XmiShapeEnum` | Profile shape type | Rectangular, Circular, IShape, TShape, LShape, CShape, etc. |
| `XmiStructuralSurfaceMemberSpanTypeEnum` | Span type for surface members | OneWay, TwoWay, etc. |
| `XmiStructuralSurfaceMemberSystemPlaneEnum` | Plane orientation for surface members | XY, XZ, YZ |

### Unit Types

| Enum | Purpose | Values |
|-------|---------|---------|
| `XmiUnitEnum` | Unit classification for conversion | Length, Force, Stress, etc. |

### Domain Classification

| Enum | Purpose | Values |
|-------|---------|---------|
| `XmiBaseEntityDomainEnum` | Entity domain classification | Physical, StructuralAnalytical, Geometry, Functional, Shared |

## Usage Example

```csharp
// Define enum with EnumValueAttribute
public enum XmiMaterialTypeEnum
{
    [EnumValue("Concrete")]
    Concrete = 0,

    [EnumValue("Steel")]
    Steel = 1
}

// Create material using enum
var material = new XmiMaterial(
    id: "mat-1",
    name: "Concrete C40",
    materialType: XmiMaterialTypeEnum.Concrete  // Uses EnumValue
);

// Serialize to JSON
var json = JsonConvert.SerializeObject(material);
// Output: { ..., "MaterialType": "Concrete", ... }  // Not "0"

// Deserialize from JSON
var parsedMaterial = JsonConvert.DeserializeObject<XmiMaterial>(json);
// parsedMaterial.MaterialType == XmiMaterialTypeEnum.Concrete
```

## ExtensionEnumHelper

The `ExtensionEnumHelper` utility class provides:

```csharp
// Convert enum to string
string stringValue = ExtensionEnumHelper.ToEnumValue(XmiMaterialTypeEnum.Concrete);
// Result: "Concrete"

// Parse string to enum
var enumValue = ExtensionEnumHelper.FromEnumValue<XmiMaterialTypeEnum>("Steel");
// Result: XmiMaterialTypeEnum.Steel
```

## Best Practices

1. **Use EnumValueAttribute** on all enum members
2. **Keep enum values descriptive** (e.g., "Concrete" vs. "MAT_CONC")
3. **Test round-trip serialization** to ensure correctness
4. **Document enum values** in code comments
5. **Use ExtensionEnumHelper** for parsing when needed

## Enum Design Patterns

### Semantic Naming

Use meaningful enum names:

```csharp
// Good
public enum XmiMaterialTypeEnum
{
    [EnumValue("ReinforcedConcrete")]
    ReinforcedConcrete = 0
}

// Avoid
public enum XmiMaterialTypeEnum
{
    [EnumValue("RC")]
    ReinforcedConcrete = 0  // Too cryptic
}
```

### Default Values

Define default value (typically 0):

```csharp
public enum XmiStructuralCurveMemberTypeEnum
{
    [EnumValue("Beam")]
    Beam = 0,  // Default

    [EnumValue("Column")]
    Column = 1
}
```

### Backward Compatibility

EnumValue enables maintaining old format:

```csharp
// Legacy API used "BEAM"
// New API uses "Beam" (improved naming)
public enum XmiStructuralCurveMemberTypeEnum
{
    [EnumValue("BEAM")]  // Maps to legacy format
    Beam = 0
}
```

## Related Classes

- **ExtensionEnumHelper** - Utility for enum/string conversion
- **Newtonsoft.Json** - JSON serialization with custom converters

## Related Enums

All domain enums in `XmiSchema.Enums` namespace:

**Materials & Sections**
- `XmiMaterialTypeEnum`
- `XmiShapeEnum`

**Structural Members**
- `XmiStructuralCurveMemberTypeEnum`
- `XmiStructuralSurfaceMemberTypeEnum`
- `XmiSegmentTypeEnum`

**Geometry & Properties**
- `XmiSystemLineEnum`
- `XmiStructuralSurfaceMemberSpanTypeEnum`
- `XmiStructuralSurfaceMemberSystemPlaneEnum`

**Units**
- `XmiUnitEnum`

**Classification**
- `XmiBaseEntityDomainEnum`

## Notes

- All domain enums should inherit or be marked with `EnumValueAttribute`
- Enum values are case-sensitive in serialization
- Use `ExtensionEnumHelper` for robust string-to-enum conversion
- Test enum serialization in `ExtensionEnumHelperTests.cs`
- Enum values in JSON are human-readable strings, not numeric values
