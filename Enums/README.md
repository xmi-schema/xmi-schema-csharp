# Enums Namespace

The `XmiSchema.Enums` namespace contains enumeration types that define domain-specific values used throughout the XMI schema. All enums use `EnumValueAttribute` for string serialization.

## Core Enumerations

### Geometric Enums
- **XmiPoint3dTypeEnum** - Point type classifications
- **XmiSystemLineEnum** - Line system definitions

### Structural Member Enums
- **XmiStructuralCurveMemberTypeEnum** - Curve member types (beam, column, brace)
- **XmiStructuralSurfaceMemberTypeEnum** - Surface member types (slab, wall, plate)
- **XmiStructuralSurfaceMemberSpanTypeEnum** - Surface member span configurations
- **XmiStructuralSurfaceMemberSystemPlaneEnum** - Surface member system plane orientations

### Material and Section Enums
- **XmiStructuralMaterialTypeEnum** - Structural material classifications
- **XmiShapeEnum** - Cross-section shape profiles
- **XmiShapeEnumParameters** - Parameter definitions for each shape type

### Segment and Domain Enums
- **XmiSegmentTypeEnum** - Segment type classifications
- **XmiBaseEntityDomainEnum** - Entity domain categories
- **XmiUnitEnum** - Unit type definitions

## EnumValueAttribute Pattern

All enum members use the `EnumValueAttribute` for serialization:

```csharp
public enum XmiStructuralCurveMemberTypeEnum
{
    [EnumValue("BEAM")]
    Beam,
    
    [EnumValue("COLUMN")]
    Column,
    
    [EnumValue("BRACE")]
    Brace
}
```

### Serialization Benefits
- **String-based** - Human-readable JSON values
- **Version-stable** - Enum reordering doesn't break serialization
- **Interoperability** - Compatible with external systems

## Usage Patterns

### Enum Parsing
Use `ExtensionEnumHelper.FromEnumValue<T>()` for safe parsing:

```csharp
// Parse from string value
var memberType = ExtensionEnumHelper.FromEnumValue<XmiStructuralCurveMemberTypeEnum>("BEAM");

// Convert to string value
var stringValue = memberType.ToEnumValue(); // Returns "BEAM"
```

### Switch Pattern Matching
Modern C# pattern matching works seamlessly:

```csharp
string GetDescription(XmiStructuralCurveMemberTypeEnum type) => type switch
{
    XmiStructuralCurveMemberTypeEnum.Beam => "Horizontal structural member",
    XmiStructuralCurveMemberTypeEnum.Column => "Vertical structural member",
    XmiStructuralCurveMemberTypeEnum.Brace => "Diagonal support member",
    _ => "Unknown member type"
};
```

### Default Values
Enums have sensible defaults for new entities:

```csharp
public XmiBeam(...) : base(...)
{
    StructuralCurveMemberType = XmiStructuralCurveMemberTypeEnum.Beam; // Default
}
```

## Shape Parameter System

### XmiShapeEnumParameters
Defines canonical parameter keys for each shape type:

```csharp
public static class XmiShapeEnumParameters
{
    public const string RectangularWidth = "width";
    public const string RectangularHeight = "height";
    public const string CircularRadius = "radius";
    public const string ISectionWidth = "width";
    public const string ISectionHeight = "height";
    public const string ISectionWebThickness = "web_thickness";
    // ... more parameters
}
```

### Usage in Shape Parameters
```csharp
var rectangularParams = new Dictionary<string, double>
{
    [XmiShapeEnumParameters.RectangularWidth] = 300.0,
    [XmiShapeEnumParameters.RectangularHeight] = 500.0
};
```

## Domain Classification

### XmiBaseEntityDomainEnum
Categorizes entities into logical domains:
- **STRUCTURAL_ANALYTICAL** - Analytical model elements
- **PHYSICAL** - Physical building elements
- **GEOMETRY** - Geometric primitives
- **COMMON** - Shared domain entities

## Unit System

### XmiUnitEnum
Defines supported unit types for unit conversion:
- **LENGTH** - Linear measurements
- **AREA** - Cross-sectional areas
- **VOLUME** - Volumetric measurements
- **FORCE** - Force and load units
- **STRESS** - Material stress units

## Testing Enum Behavior

### Round-trip Testing
Ensure enum serialization works correctly:

```csharp
[Test]
public void Enum_Should_SerializeAndDeserialize_Correctly()
{
    var original = XmiStructuralCurveMemberTypeEnum.Column;
    var serialized = original.ToEnumValue();
    var deserialized = ExtensionEnumHelper.FromEnumValue<XmiStructuralCurveMemberTypeEnum>(serialized);
    
    Assert.That(deserialized, Is.EqualTo(original));
}
```

### Parameter Validation
Test shape parameter completeness:

```csharp
[Test]
public void ShapeParameters_Should_ContainAllRequiredKeys()
{
    var params = new RectangularShapeParameters { Width = 300, Height = 500 };
    var dict = params.ToDictionary();
    
    Assert.That(dict.ContainsKey(XmiShapeEnumParameters.RectangularWidth));
    Assert.That(dict.ContainsKey(XmiShapeEnumParameters.RectangularHeight));
}
```

## Best Practices

1. **Use EnumValueAttribute** on all enum members
2. **Parse with ExtensionEnumHelper** for robustness
3. **Document enum values** in XML comments
4. **Test round-trip serialization** for all enums
5. **Use shape parameter constants** for consistency
6. **Provide sensible defaults** in entity constructors

## Integration with Other Namespaces

- **Entities.* (all)** - Use enums for type classification
- **Parameters** - Shape enums define parameter structures
- **Utils** - ExtensionEnumHelper provides parsing utilities
- **Serialization** - EnumValueAttribute enables JSON serialization

## Testing

When adding new enum values, always update `Utils/ExtensionEnumHelper` tests to verify round-trip conversions and confirm that serialized strings align with the schema specification.
