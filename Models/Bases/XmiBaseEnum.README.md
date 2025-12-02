# EnumValueAttribute

Custom attribute for controlling enum serialization to human-readable strings in JSON exports.

## Overview

`EnumValueAttribute` is a fundamental infrastructure component of the Cross Model Information (XMI) schema. It enables enumerations to serialize as descriptive string values rather than numeric integers, making the exported JSON data more readable and interoperable with external systems.

This attribute is applied to enum fields throughout the XMI schema to define how each enum value should appear in JSON exports. Without this attribute, enum values would serialize as integers (0, 1, 2, etc.), which are meaningless to human readers and incompatible with systems expecting specific string values.

## Why This Pattern Exists

### Problem: Default Enum Serialization

By default, C# enums serialize to integers:

```csharp
public enum MaterialType { Concrete, Steel, Timber }
var material = MaterialType.Concrete;
// Default JSON: { "MaterialType": 0 }  // Not human-readable!
```

### Solution: String-Based Serialization

With `EnumValueAttribute`, we get readable string values:

```csharp
public enum MaterialType
{
    [EnumValue("Concrete")] Concrete,
    [EnumValue("Steel")] Steel,
    [EnumValue("Timber")] Timber
}
// With attribute: { "MaterialType": "Concrete" }  // Human-readable!
```

### Benefits

1. **Human Readability**: JSON exports are understandable without documentation
2. **Interoperability**: External systems can parse string values reliably
3. **Versioning**: Adding new enum values doesn't break existing integrations
4. **Documentation**: String values serve as self-documenting field values
5. **Standards Compliance**: Aligns with industry terminology and conventions

## Properties

| Property | Type | Description |
|----------|------|-------------|
| `Value` | `string` | The string representation to use in JSON serialization |

## Usage Example

### Defining an Enum with String Values

```csharp
using XmiSchema.Core.Enums;

/// <summary>
/// Types of structural materials used in construction.
/// </summary>
public enum XmiStructuralMaterialTypeEnum
{
    /// <summary>Concrete material (cement-based composite)</summary>
    [EnumValue("Concrete")]
    Concrete,

    /// <summary>Structural steel material</summary>
    [EnumValue("Steel")]
    Steel,

    /// <summary>Timber/wood material</summary>
    [EnumValue("Timber")]
    Timber,

    /// <summary>Aluminum material</summary>
    [EnumValue("Aluminum")]
    Aluminum,

    /// <summary>Other or unknown material type</summary>
    [EnumValue("Other")]
    Other
}
```

### Using Enums in Entities

```csharp
using XmiSchema.Core.Entities;
using XmiSchema.Core.Enums;

var material = new XmiStructuralMaterial(
    id: "MAT001",
    name: "Concrete C30",
    // ... other parameters
    materialType: XmiStructuralMaterialTypeEnum.Concrete,  // Enum value
    // ... more parameters
);
```

### JSON Serialization Result

When the model is exported to JSON:

```json
{
  "nodes": [
    {
      "XmiStructuralMaterial": {
        "ID": "MAT001",
        "Name": "Concrete C30",
        "MaterialType": "Concrete",  // String value, not integer!
        "Grade": 30.0,
        "EModulus": 33000.0
      }
    }
  ]
}
```

### Reading Enum Values Programmatically

The `ExtensionEnumHelper` class provides utilities to work with enum string values:

```csharp
using XmiSchema.Core.Extensions;
using XmiSchema.Core.Enums;

// Get string value from enum
var materialType = XmiStructuralMaterialTypeEnum.Concrete;
string stringValue = ExtensionEnumHelper.GetEnumValue(materialType);
// Result: "Concrete"

// Parse string back to enum
string jsonValue = "Steel";
var parsedEnum = ExtensionEnumHelper.ParseEnum<XmiStructuralMaterialTypeEnum>(jsonValue);
// Result: XmiStructuralMaterialTypeEnum.Steel
```

## Pattern Consistency

### All XMI Enums Follow This Pattern

Every enumeration in the XMI schema uses `EnumValueAttribute`:

- **XmiShapeEnum**: `[EnumValue("Rectangular")]`, `[EnumValue("I Shape")]`
- **XmiStructuralMaterialTypeEnum**: `[EnumValue("Concrete")]`, `[EnumValue("Steel")]`
- **XmiStructuralCurveMemberTypeEnum**: `[EnumValue("Beam")]`, `[EnumValue("Column")]`
- **XmiSegmentTypeEnum**: `[EnumValue("Line")]`, `[EnumValue("CircularArc")]`
- And all others...

### Naming Conventions

String values in the XMI schema follow these conventions:

1. **PascalCase or Space-Separated**: `"Concrete"` or `"I Shape"`
2. **Descriptive**: Match industry terminology
3. **Consistent**: Similar concepts use similar naming patterns
4. **No Abbreviations**: Full words for clarity (exceptions for standard abbreviations like "3D")

## Related Classes

### Helper Classes
- **`ExtensionEnumHelper`**: Utility methods for enum/string conversion
- **`ExtensionNativeJsonBuilder`**: Uses enum string values during JSON serialization

### Enum Classes Using This Attribute
- **`XmiShapeEnum`**: Cross-section shapes
- **`XmiStructuralMaterialTypeEnum`**: Material types
- **`XmiStructuralCurveMemberTypeEnum`**: Linear member types
- **`XmiStructuralSurfaceMemberTypeEnum`**: Surface member types
- **`XmiSegmentTypeEnum`**: Geometry segment types
- **`XmiUnitEnum`**: Units of measurement
- And more...

## Technical Implementation

### Attribute Target

```csharp
[AttributeUsage(AttributeTargets.Field)]
public class EnumValueAttribute : Attribute
```

The `AttributeTargets.Field` specification means this attribute can only be applied to enum fields, enforcing correct usage.

### Reflection-Based Retrieval

The serialization system uses reflection to retrieve the string value:

```csharp
// Simplified example of how ExtensionEnumHelper works
var field = typeof(YourEnum).GetField(enumValue.ToString());
var attribute = field?.GetCustomAttribute<EnumValueAttribute>();
string stringValue = attribute?.Value ?? enumValue.ToString();
```

## Best Practices

### When Defining New Enums

1. **Always Use EnumValueAttribute**: Every enum field should have the attribute
2. **Provide Descriptive Values**: Use industry-standard terminology
3. **Add XML Documentation**: Document each enum value's meaning
4. **Consider Backward Compatibility**: Don't change existing string values

### Example of Well-Documented Enum

```csharp
/// <summary>
/// Defines the types of structural curve members in the built environment.
/// </summary>
public enum XmiStructuralCurveMemberTypeEnum
{
    /// <summary>
    /// Horizontal member primarily resisting bending moments.
    /// Typically supports loads from slabs or other members.
    /// </summary>
    [EnumValue("Beam")]
    Beam,

    /// <summary>
    /// Vertical member primarily resisting axial compression.
    /// Transfers loads from beams/slabs to foundations.
    /// </summary>
    [EnumValue("Column")]
    Column,

    /// <summary>
    /// Diagonal member providing lateral stability.
    /// Resists horizontal loads from wind or seismic forces.
    /// </summary>
    [EnumValue("Bracing")]
    Bracing
}
```

## Engineering Notes

### JSON Schema Compatibility

The string-based enum serialization makes XMI schema JSON compatible with:
- JSON Schema validation
- OpenAPI/Swagger specifications
- TypeScript/JavaScript type definitions
- GraphQL enum types

### Database Integration

When storing XMI data in databases:
- String values can be stored directly in VARCHAR columns
- No need for lookup tables or integer mapping
- Queries can filter by readable values: `WHERE MaterialType = 'Concrete'`

### Internationalization Considerations

For international use, consider:
- String values are currently in English
- External translation layers can map to localized terms
- Internal values remain stable across languages
- Documentation should explain standard English terms used

## See Also

- [ExtensionEnumHelper.README.md](../../Extensions/ExtensionEnumHelper.README.md) - Enum conversion utilities
- [ExtensionNativeJsonBuilder.README.md](../../Extensions/ExtensionNativeJsonBuilder.README.md) - JSON serialization
- [XmiShapeEnum.README.md](../Enums/XmiShapeEnum.README.md) - Example enum implementation
- [XmiStructuralMaterialTypeEnum.README.md](../Enums/XmiStructuralMaterialTypeEnum.README.md) - Example enum implementation
