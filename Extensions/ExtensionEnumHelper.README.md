# ExtensionEnumHelper

Utility class for converting between string values and enum types using EnumValueAttribute.

## Overview

`ExtensionEnumHelper` provides reverse lookup functionality for enums decorated with `EnumValueAttribute`. It enables converting from custom string values back to their corresponding enum values, supporting JSON deserialization and user input parsing.

## Location

`Extensions/ExtensionEnumHelper.cs`

## Key Features

- **String to Enum Conversion**: Converts custom string values to enum types
- **Attribute-Based Lookup**: Uses `EnumValueAttribute` for mapping
- **Case-Insensitive**: Matches string values ignoring case
- **Null Safety**: Returns nullable enum for failed lookups
- **Generic**: Works with any enum type

## Methods

### FromEnumValue

```csharp
public static TEnum? FromEnumValue<TEnum>(string value) where TEnum : struct, Enum
```

Converts a string value to its corresponding enum value.

**Type Parameters:**
- `TEnum`: The enum type to convert to (must be a struct and Enum)

**Parameters:**
- `value`: The string value to look up (matches `EnumValueAttribute.Value`)

**Returns:**
- Enum value if match found
- `null` if no match found

**Matching:**
- Case-insensitive comparison
- Uses `EnumValueAttribute.Value` for comparison

## Usage Examples

### Basic Conversion

```csharp
// Enum definition
public enum XmiStructuralCurveMemberTypeEnum
{
    [EnumValue("Beam")]
    Beam,

    [EnumValue("Column")]
    Column,

    [EnumValue("Brace")]
    Brace
}

// Convert string to enum
var memberType = ExtensionEnumHelper.FromEnumValue<XmiStructuralCurveMemberTypeEnum>("Beam");
// Result: XmiStructuralCurveMemberTypeEnum.Beam

var columnType = ExtensionEnumHelper.FromEnumValue<XmiStructuralCurveMemberTypeEnum>("column");
// Result: XmiStructuralCurveMemberTypeEnum.Column (case-insensitive)

var invalidType = ExtensionEnumHelper.FromEnumValue<XmiStructuralCurveMemberTypeEnum>("Wall");
// Result: null (no match)
```

### With Null Handling

```csharp
var memberTypeString = "Beam";
var memberType = ExtensionEnumHelper.FromEnumValue<XmiStructuralCurveMemberTypeEnum>(memberTypeString);

if (memberType.HasValue)
{
    Console.WriteLine($"Member type: {memberType.Value}");
}
else
{
    Console.WriteLine($"Invalid member type: {memberTypeString}");
}
```

### JSON Deserialization Context

```csharp
// Parse JSON node
var jsonMemberType = jsonObject["MemberType"]?.ToString();  // "Beam"

// Convert to enum
var memberType = ExtensionEnumHelper.FromEnumValue<XmiStructuralCurveMemberTypeEnum>(jsonMemberType);

if (memberType.HasValue)
{
    member.MemberType = memberType.Value;
}
else
{
    throw new JsonException($"Invalid MemberType value: {jsonMemberType}");
}
```

### User Input Parsing

```csharp
// Get user input
Console.WriteLine("Enter member type (Beam/Column/Brace):");
var input = Console.ReadLine();

// Parse input
var memberType = ExtensionEnumHelper.FromEnumValue<XmiStructuralCurveMemberTypeEnum>(input);

if (memberType.HasValue)
{
    var member = new XmiStructuralCurveMember(...)
    {
        MemberType = memberType.Value
    };
}
else
{
    Console.WriteLine("Invalid member type. Please use Beam, Column, or Brace.");
}
```

## Relationship to EnumValueAttribute

### Forward (Enum → String)

Handled by `ExtensionNativeJsonBuilder`:
```csharp
// In ExtensionNativeJsonBuilder.GetAttributes()
if (type.IsEnum)
{
    var enumName = value.ToString();
    var field = type.GetField(enumName);
    var enumValueAttr = field.GetCustomAttribute<EnumValueAttribute>();
    dict[prop.Name] = enumValueAttr?.Value ?? enumName;
}
```

### Reverse (String → Enum)

Handled by `ExtensionEnumHelper`:
```csharp
var enumValue = ExtensionEnumHelper.FromEnumValue<MyEnum>("custom-string-value");
```

## Implementation Details

### Reflection Process

1. Get all public static fields of the enum type
2. For each field, retrieve `EnumValueAttribute`
3. Compare attribute value with input string (case-insensitive)
4. Return enum value if match found
5. Return null if no matches

### Code Flow

```csharp
foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
{
    var attribute = field.GetCustomAttribute<EnumValueAttribute>();

    if (attribute != null &&
        attribute.Value.Equals(value, StringComparison.OrdinalIgnoreCase))
    {
        var enumValue = field.GetValue(null);
        if (enumValue is TEnum typedValue)
        {
            return typedValue;
        }
    }
}

return null;
```

## Performance Considerations

- **Reflection Overhead**: Uses reflection to inspect enum fields
- **Linear Search**: O(n) where n is number of enum values
- **No Caching**: Performs lookup each time (consider caching for high-frequency use)

### Optimization Opportunity

For high-performance scenarios, consider creating a cached dictionary:

```csharp
private static readonly Dictionary<string, XmiStructuralCurveMemberTypeEnum> _cachedLookup
    = BuildLookupCache<XmiStructuralCurveMemberTypeEnum>();

private static Dictionary<string, TEnum> BuildLookupCache<TEnum>() where TEnum : struct, Enum
{
    var cache = new Dictionary<string, TEnum>(StringComparer.OrdinalIgnoreCase);

    foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
    {
        var attribute = field.GetCustomAttribute<EnumValueAttribute>();
        if (attribute != null && field.GetValue(null) is TEnum value)
        {
            cache[attribute.Value] = value;
        }
    }

    return cache;
}
```

## Common Enum Types

The helper works with all XMI enum types:

- `XmiStructuralCurveMemberTypeEnum` - Beam, Column, Brace
- `XmiStructuralSurfaceMemberTypeEnum` - Slab, Wall, Shell
- `XmiStructuralMaterialTypeEnum` - Concrete, Steel, Timber
- `XmiShapeEnum` - Rectangular, Circular, I-Section
- `XmiUnitEnum` - Metric, Imperial
- And more...

## Error Handling

```csharp
var value = ExtensionEnumHelper.FromEnumValue<MyEnum>("invalid");

if (!value.HasValue)
{
    // Handle invalid input
    throw new ArgumentException($"Invalid enum value: 'invalid'");
}
```

## Design Patterns

### Static Utility Pattern
Pure static class with no state.

### Generic Method Pattern
Type-safe conversion with compile-time type checking.

### Nullable Pattern
Returns nullable to indicate success/failure without exceptions.

## Related Classes

- **EnumValueAttribute**: Attribute class for custom enum string values
- **ExtensionNativeJsonBuilder**: Uses EnumValueAttribute for serialization (forward direction)
- **XmiBaseEnum**: Contains enum types with EnumValueAttribute decorations

## See Also

- [XmiBaseEnum](../Models/Bases/XmiBaseEnum.README.md) - Enum types and EnumValueAttribute
- [ExtensionNativeJsonBuilder](ExtensionNativeJsonBuilder.README.md) - JSON serialization
