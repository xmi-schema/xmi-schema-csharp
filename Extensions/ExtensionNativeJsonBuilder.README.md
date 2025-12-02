# ExtensionNativeJsonBuilder

Serializes XMI models to JSON graph format with nodes and edges.

## Overview

`ExtensionNativeJsonBuilder` transforms XMI models into JSON representation following a graph structure with nodes (entities) and edges (relationships). It handles special serialization requirements including enum value conversion and entity reference flattening.

## Location

`Extensions/ExtensionNativeJsonBuilder.cs`

## Key Features

- **Graph Format**: Exports as `{ nodes: [...], edges: [...] }`
- **Type Discriminator**: Each node/edge wrapped with its type name
- **Enum Handling**: Converts enums using `EnumValueAttribute` for custom string values
- **Reference Flattening**: Converts entity references to IDs
- **Property Ordering**: Maintains declaration order using MetadataToken

## Constructor

```csharp
public ExtensionNativeJsonBuilder(XmiModel model)
```

**Parameters:**
- `model`: The XMI model to serialize

## Methods

### BuildJson

```csharp
public string BuildJson()
```

Generates JSON string representation of the model.

**Returns:** Formatted JSON string with indentation

**Example:**
```csharp
var builder = new ExtensionNativeJsonBuilder(model);
string json = builder.BuildJson();
Console.WriteLine(json);
```

### Save

```csharp
public void Save(string path)
```

Writes JSON to a file.

**Parameters:**
- `path`: File path where JSON should be saved

**Example:**
```csharp
builder.Save("output/structural_model.json");
```

## JSON Output Format

### Overall Structure

```json
{
  "nodes": [
    { "TypeName": { ...attributes } },
    { "TypeName": { ...attributes } }
  ],
  "edges": [
    { "RelationshipTypeName": { ...attributes } },
    { "RelationshipTypeName": { ...attributes } }
  ]
}
```

### Node Example

```json
{
  "XmiStructuralCurveMember": {
    "ID": "M001",
    "Name": "Beam-1",
    "IFCGUID": "2Dj4jc$nD2hBU9kSEOLXyH",
    "NativeId": "RevitId_12345",
    "Description": "Main support beam",
    "EntityType": "XmiStructuralCurveMember",
    "MemberType": "Beam",
    "CrossSection": "CS001",
    "Segments": ["SEG001", "SEG002"],
    "Nodes": ["NODE001", "NODE002"]
  }
}
```

### Edge Example

```json
{
  "XmiHasStructuralCrossSection": {
    "ID": "a7f3c8e2-9b41-4d5a-8e1c-6f2d9a3b5c7e",
    "Source": "M001",
    "Target": "CS001",
    "Name": "XmiHasStructuralCrossSection",
    "Description": "",
    "EntityType": "XmiHasStructuralCrossSection",
    "UmlType": "Association"
  }
}
```

## Serialization Rules

### Property Filtering

Only properties with non-null values are included in the output.

### Type Handling

| Property Type | Serialization |
|--------------|---------------|
| Primitive (int, bool, etc.) | Direct value |
| string | Direct value |
| decimal, float, double | Direct value |
| DateTime | Direct value |
| Enum | EnumValue attribute or enum name |
| XmiBaseEntity | Entity ID (string) |
| IEnumerable<XmiBaseEntity> | List of entity IDs |

### Enum Serialization

Enums use the `EnumValueAttribute` to provide custom string values:

```csharp
public enum XmiStructuralCurveMemberTypeEnum
{
    [EnumValue("Beam")]
    Beam,

    [EnumValue("Column")]
    Column
}
```

JSON output:
```json
{
  "MemberType": "Beam"  // Not "0" or "Beam" enum string
}
```

### Entity Reference Flattening

Entity references are converted to ID strings:

```csharp
// In code
member.CrossSection = crossSectionObject;

// In JSON
{
  "CrossSection": "CS001"  // ID string, not full object
}
```

### Collection Flattening

Collections of entities become arrays of ID strings:

```csharp
// In code
member.Segments = new[] { segment1, segment2 };

// In JSON
{
  "Segments": ["SEG001", "SEG002"]
}
```

## Internal Implementation

### GetAttributes Method

```csharp
private Dictionary<string, object> GetAttributes(object obj)
```

Core serialization logic:

1. **Get Properties**: Uses reflection to get all public instance properties
2. **Order Properties**: Sorts by MetadataToken to maintain declaration order
3. **Process Each Property**:
   - Skip if value is null
   - Handle enums with EnumValueAttribute
   - Handle primitives and strings directly
   - Convert entity references to IDs
   - Convert entity collections to ID lists

### Property Ordering

```csharp
var props = obj.GetType()
    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
    .OrderBy(p => p.MetadataToken); // Declaration order
```

This ensures consistent JSON output matching the property declaration order in classes.

## Complete Example

```csharp
// Create model
var builder = new XmiSchemaModelBuilder();

var material = new XmiStructuralMaterial("MAT001", "Concrete C30", ...);
var crossSection = new XmiStructuralCrossSection("CS001", "300x600", material, ...);
var member = new XmiStructuralCurveMember("M001", "Beam-1", crossSection, ...);

builder.AddEntities(new[] { material, crossSection, member });
var model = builder.BuildModel();

// Serialize to JSON
var jsonBuilder = new ExtensionNativeJsonBuilder(model);

// Option 1: Get JSON string
string json = jsonBuilder.BuildJson();
Console.WriteLine(json);

// Option 2: Save to file
jsonBuilder.Save("output/model.json");
```

## Output Example

```json
{
  "nodes": [
    {
      "XmiStructuralMaterial": {
        "ID": "MAT001",
        "Name": "Concrete C30",
        "EntityType": "XmiStructuralMaterial",
        "MaterialType": "Concrete"
      }
    },
    {
      "XmiStructuralCrossSection": {
        "ID": "CS001",
        "Name": "300x600",
        "EntityType": "XmiStructuralCrossSection",
        "Material": "MAT001",
        "Shape": "Rectangular"
      }
    },
    {
      "XmiStructuralCurveMember": {
        "ID": "M001",
        "Name": "Beam-1",
        "EntityType": "XmiStructuralCurveMember",
        "MemberType": "Beam",
        "CrossSection": "CS001"
      }
    }
  ],
  "edges": [
    {
      "XmiHasStructuralMaterial": {
        "ID": "rel-001",
        "Source": "CS001",
        "Target": "MAT001",
        "EntityType": "XmiHasStructuralMaterial"
      }
    },
    {
      "XmiHasStructuralCrossSection": {
        "ID": "rel-002",
        "Source": "M001",
        "Target": "CS001",
        "EntityType": "XmiHasStructuralCrossSection"
      }
    }
  ]
}
```

## Design Patterns

### Builder Pattern
Constructs complex JSON representation step-by-step.

### Reflection Pattern
Uses .NET reflection to dynamically inspect object properties.

### Type Discriminator Pattern
Wraps each object in a dictionary with its type name as the key.

## Dependencies

- **Newtonsoft.Json**: JSON serialization library
- **System.Reflection**: Property inspection

## Related Classes

- **XmiModel**: Input model structure
- **XmiSchemaModelBuilder**: Builds models for serialization
- **EnumValueAttribute**: Custom enum string values
- **XmiBaseEntity**: Base class for all serialized entities
- **XmiBaseRelationship**: Base class for all serialized relationships

## See Also

- [XmiSchemaModelBuilder](../Builder/XmiSchemaModelBuilder.README.md) - Model builder
- [XmiModel](../Models/Model/XmiModel.README.md) - Model structure
- [XmiBaseEnum](../Models/Bases/XmiBaseEnum.README.md) - Enum attribute system
