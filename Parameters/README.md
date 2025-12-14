# Parameters Namespace

The `XmiSchema.Parameters` namespace contains interfaces and implementations for shape parameter definitions used in cross-section profiles.

## Core Components

### IXmiShapeParameters Interface
Defines the contract for all shape parameter implementations:

```csharp
public interface IXmiShapeParameters
{
    Dictionary<string, double> ToDictionary();
    XmiShapeEnum ShapeType { get; }
}
```

### XmiShapeParametersBase
Abstract base class providing common functionality for shape parameters:

```csharp
public abstract class XmiShapeParametersBase : IXmiShapeParameters
{
    public abstract XmiShapeEnum ShapeType { get; }
    public abstract Dictionary<string, double> ToDictionary();
}
```

### ShapeParameterSets
Factory class for creating shape parameter instances:

```csharp
public static class ShapeParameterSets
{
    public static IXmiShapeParameters CreateRectangular(double width, double height)
    public static IXmiShapeParameters CreateCircular(double radius)
    public static IXmiShapeParameters CreateISection(double width, double height, double webThickness, double flangeThickness)
    // ... more shape creation methods
}
```

## Available Shape Types

### Rectangular Shape
```csharp
var rectangular = ShapeParameterSets.CreateRectangular(300.0, 500.0);
// Parameters: width, height
```

### Circular Shape
```csharp
var circular = ShapeParameterSets.CreateCircular(150.0);
// Parameters: radius
```

### I-Section Shape
```csharp
var iSection = ShapeParameterSets.CreateISection(200.0, 400.0, 10.0, 15.0);
// Parameters: width, height, web_thickness, flange_thickness
```

### Additional Shapes
- **T-Section** - T-shaped profile
- **L-Section** - L-shaped (angle) profile
- **Box/Tube** - Hollow rectangular profile
- **Pipe** - Hollow circular profile

## Usage Patterns

### Creating Cross-Sections with Shape Parameters
```csharp
// Create shape parameters
var rectangularParams = ShapeParameterSets.CreateRectangular(300.0, 500.0);

// Create cross-section
var crossSection = model.CreateXmiCrossSection(
    id: "cs_001",
    name: "Beam Section",
    ifcGuid: "...",
    nativeId: "cs_001",
    description: "Rectangular beam cross-section",
    shape: XmiShapeEnum.Rectangular,
    parameters: rectangularParams
);
```

### Direct Parameter Dictionary Creation
```csharp
var customParams = new Dictionary<string, double>
{
    [XmiShapeEnumParameters.RectangularWidth] = 250.0,
    [XmiShapeEnumParameters.RectangularHeight] = 450.0
};

var crossSection = model.CreateXmiCrossSection(
    "cs_002", "Custom Section", "...", "cs_002", 
    "Custom rectangular section", XmiShapeEnum.Rectangular, customParams);
```

### Accessing Parameter Values
```csharp
var parameters = crossSection.Parameters;
var width = parameters[XmiShapeEnumParameters.RectangularWidth];
var height = parameters[XmiShapeEnumParameters.RectangularHeight];
```

## Parameter Key Constants

### XmiShapeEnumParameters Class
Provides canonical parameter key names:

```csharp
public static class XmiShapeEnumParameters
{
    // Rectangular parameters
    public const string RectangularWidth = "width";
    public const string RectangularHeight = "height";
    
    // Circular parameters
    public const string CircularRadius = "radius";
    
    // I-Section parameters
    public const string ISectionWidth = "width";
    public const string ISectionHeight = "height";
    public const string ISectionWebThickness = "web_thickness";
    public const string ISectionFlangeThickness = "flange_thickness";
    
    // ... more parameter constants
}
```

## Implementation Guidelines

### Creating Custom Shape Parameters
```csharp
public class CustomShapeParameters : XmiShapeParametersBase
{
    public double Width { get; set; }
    public double Height { get; set; }
    public double CustomProperty { get; set; }
    
    public override XmiShapeEnum ShapeType => XmiShapeEnum.Custom;
    
    public override Dictionary<string, double> ToDictionary()
    {
        return new Dictionary<string, double>
        {
            ["width"] = Width,
            ["height"] = Height,
            ["custom_property"] = CustomProperty
        };
    }
}
```

### Validation in Parameters
```csharp
public class RectangularShapeParameters : XmiShapeParametersBase
{
    private double _width;
    private double _height;
    
    public double Width
    {
        get => _width;
        set => _width = value > 0 ? value : throw new ArgumentException("Width must be positive");
    }
    
    // Similar validation for Height
}
```

## Serialization and Storage

### JSON Serialization
Shape parameters serialize to JSON dictionaries:

```json
{
  "Parameters": {
    "width": 300.0,
    "height": 500.0
  },
  "Shape": "Rectangular"
}
```

### Database Storage
Parameters can be stored as:
- JSON strings in database columns
- Key-value pairs in NoSQL documents
- Structured tables with parameter columns

## Integration with Cross-Sections

### Cross-Section Properties
Shape parameters feed into calculated cross-section properties:

```csharp
public class XmiCrossSection : XmiBaseEntity
{
    public Dictionary<string, double> Parameters { get; set; }
    public XmiShapeEnum Shape { get; set; }
    
    // Calculated properties
    public double Area { get; set; }           // Calculated from parameters
    public double MomentOfInertiaY { get; set; } // Calculated from parameters
    public double MomentOfInertiaZ { get; set; } // Calculated from parameters
    public double SectionModulusY { get; set; } // Calculated from parameters
    public double SectionModulusZ { get; set; } // Calculated from parameters
}
```

## Best Practices

1. **Use factory methods** - Prefer `ShapeParameterSets.Create*()` methods
2. **Validate parameters** - Ensure positive values and logical constraints
3. **Document units** - Specify parameter units (mm, cm, etc.)
4. **Handle missing parameters** - Provide default values where appropriate
5. **Test parameter serialization** - Verify round-trip JSON conversion
6. **Use parameter constants** - Reference `XmiShapeEnumParameters` for key names

## Testing

### Parameter Creation Tests
```csharp
[Test]
public void CreateRectangular_Should_ReturnCorrectParameters()
{
    var params = ShapeParameterSets.CreateRectangular(300.0, 500.0);
    var dict = params.ToDictionary();
    
    Assert.That(params.ShapeType, Is.EqualTo(XmiShapeEnum.Rectangular));
    Assert.That(dict["width"], Is.EqualTo(300.0));
    Assert.That(dict["height"], Is.EqualTo(500.0));
}
```

### Serialization Tests
```csharp
[Test]
public void Parameters_Should_SerializeToJson_Correctly()
{
    var params = ShapeParameterSets.CreateCircular(150.0);
    var json = JsonConvert.SerializeObject(params.ToDictionary());
    var deserialized = JsonConvert.DeserializeObject<Dictionary<string, double>>(json);
    
    Assert.That(deserialized["radius"], Is.EqualTo(150.0));
}
```

## Integration with Other Namespaces

- **Entities.Commons** - Used by `XmiCrossSection` for profile definitions
- **Enums** - `XmiShapeEnum` defines shape types
- **Utils** - May use validation utilities
- **Serialization** - Parameters serialize to JSON dictionaries