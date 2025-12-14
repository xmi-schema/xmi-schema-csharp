# Utils Namespace

The `XmiSchema.Utils` namespace contains utility classes and extension methods that provide common functionality across the XMI schema library.

## Core Utilities

### ExtensionEnumHelper
Provides extension methods for enum serialization and parsing with `EnumValueAttribute` support.

#### Key Methods
```csharp
// Convert enum to its string value
public static string ToEnumValue<T>(this T enumValue) where T : Enum

// Parse string to enum value
public static T FromEnumValue<T>(string value) where T : Enum

// Check if string value is valid for enum
public static bool IsValidEnumValue<T>(string value) where T : Enum
```

#### Usage Examples
```csharp
// Convert enum to string
var memberType = XmiStructuralCurveMemberTypeEnum.Beam;
var stringValue = memberType.ToEnumValue(); // Returns "BEAM"

// Parse string to enum
var parsedType = ExtensionEnumHelper.FromEnumValue<XmiStructuralCurveMemberTypeEnum>("COLUMN");

// Safe parsing with validation
if (ExtensionEnumHelper.IsValidEnumValue<XmiStructuralCurveMemberTypeEnum>("BRACE"))
{
    var braceType = ExtensionEnumHelper.FromEnumValue<XmiStructuralCurveMemberTypeEnum>("BRACE");
}
```

## Usage Patterns

### Enum Validation
```csharp
public void SetMemberType(string typeString)
{
    if (!ExtensionEnumHelper.IsValidEnumValue<XmiStructuralCurveMemberTypeEnum>(typeString))
    {
        throw new ArgumentException($"Invalid member type: {typeString}");
    }
    
    MemberType = ExtensionEnumHelper.FromEnumValue<XmiStructuralCurveMemberTypeEnum>(typeString);
}
```

### JSON Serialization Integration
```csharp
public class EnumConverter<T> : JsonConverter<T> where T : Enum
{
    public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToEnumValue());
    }
    
    public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var stringValue = reader.Value?.ToString();
        return ExtensionEnumHelper.FromEnumValue<T>(stringValue);
    }
}
```

### Error Handling
```csharp
try
{
    var enumValue = ExtensionEnumHelper.FromEnumValue<XmiShapeEnum>("INVALID_SHAPE");
}
catch (ArgumentException ex)
{
    // Handle invalid enum value
    Console.WriteLine($"Invalid shape: {ex.Message}");
}
```

## Implementation Details

### EnumValueAttribute Reflection
The helper uses reflection to find `EnumValueAttribute` on enum members:

```csharp
private static string GetEnumValue<T>(T enumValue)
{
    var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
    var attribute = fieldInfo?.GetCustomAttribute<EnumValueAttribute>();
    return attribute?.Value ?? enumValue.ToString();
}
```

### Caching for Performance
Enum metadata is cached to improve performance in tight loops:

```csharp
private static readonly ConcurrentDictionary<string, string> _enumValueCache = new();
```

## Best Practices

### Error Handling
1. **Use IsValidEnumValue** for validation before parsing
2. **Handle ArgumentException** when parsing invalid values
3. **Provide default values** for optional enum properties

### Performance Considerations
1. **Cache parsed values** when parsing large datasets
2. **Avoid repeated parsing** of the same string values
3. **Use TryParse pattern** when appropriate (not currently implemented)

### Testing
```csharp
[Test]
public void ToEnumValue_Should_ReturnCorrectString()
{
    var beamType = XmiStructuralCurveMemberTypeEnum.Beam;
    var result = beamType.ToEnumValue();
    
    Assert.That(result, Is.EqualTo("BEAM"));
}

[Test]
public void FromEnumValue_Should_ParseValidString()
{
    var result = ExtensionEnumHelper.FromEnumValue<XmiStructuralCurveMemberTypeEnum>("COLUMN");
    
    Assert.That(result, Is.EqualTo(XmiStructuralCurveMemberTypeEnum.Column));
}

[Test]
public void FromEnumValue_Should_ThrowOnInvalidString()
{
    Assert.Throws<ArgumentException>(() => 
        ExtensionEnumHelper.FromEnumValue<XmiStructuralCurveMemberTypeEnum>("INVALID"));
}
```

## Integration with Other Namespaces

- **Enums** - All enums use this helper for serialization
- **Entities** - Entity classes use helper for enum property handling
- **Serialization** - JSON converters use helper for enum conversion
- **Testing** - Test utilities use helper for enum validation

## Future Enhancements

### Potential Additions
1. **TryParse methods** for non-exception parsing
2. **Case-insensitive parsing** for robustness
3. **Enum description attributes** for user-friendly names
4. **Enum validation rules** for business logic
5. **Batch parsing** for collections of enum strings

### Example Future API
```csharp
// Potential future enhancements
public static bool TryFromEnumValue<T>(string value, out T result) where T : Enum
public static T FromEnumValue<T>(string value, bool ignoreCase = false) where T : Enum
public static string GetEnumDescription<T>(this T enumValue) where T : Enum
```

## Current Limitations

1. **No TryParse** - Only exception-based parsing available
2. **Case-sensitive** - String matching is case-sensitive
3. **No description support** - Only EnumValueAttribute is supported
4. **No validation rules** - Basic string-to-enum conversion only

## Related Documentation

- **Enums/README.md** - Enum definitions and EnumValueAttribute usage
- **Serialization** - JSON serialization patterns
- **Testing** - Unit testing patterns for enum handling

## Adding New Helpers

When adding more utility classes, keep them pure and deterministic so they remain easy to unit test. Tests should live under `tests/XmiSchema.Core.Tests/Utils`.
