# XmiColumn

Represents a physical column element in the built environment.

## Purpose

`XmiColumn` defines vertical structural members that support beams, slabs, and other gravity loads. Columns are physical representations of structural elements that can be linked to analytical members for analysis purposes.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `SystemLine` | `XmiSystemLineEnum` | Yes | Relative position of analytical line inside profile |
| `Length` | `double` | Yes | Physical height of column (meters) |
| `LocalAxisX` | `XmiAxis` | Yes | Unit direction of local X axis |
| `LocalAxisY` | `XmiAxis` | Yes | Unit direction of local Y axis |
| `LocalAxisZ` | `XmiAxis` | Yes | Unit direction of local Z axis |
| `BeginNodeXOffset` | `double` | Yes | X offset applied to start node (meters) |
| `EndNodeXOffset` | `double` | Yes | X offset applied to end node (meters) |
| `BeginNodeYOffset` | `double` | Yes | Y offset applied to start node (meters) |
| `EndNodeYOffset` | `double` | Yes | Y offset applied to end node (meters) |
| `BeginNodeZOffset` | `double` | Yes | Z offset applied to start node (meters) |
| `EndNodeZOffset` | `double` | Yes | Z offset applied to end node (meters) |

## Usage Example

```csharp
// Create a concrete column
var column = new XmiColumn(
    id: "col-1",
    name: "Concrete Column C-1",
    ifcGuid: "3$HnSh4fn5$vQIE9d8M0L9",
    nativeId: "COL-C1",
    description: "Main interior column supporting floor beams",
    systemLine: XmiSystemLineEnum.MiddleMiddle,
    length: 4.2,
    localAxisX: new XmiAxis(1, 0, 0),    // Major axis
    localAxisY: new XmiAxis(0, 1, 0),    // Minor axis
    localAxisZ: new XmiAxis(0, 0, 1),    // Vertical
    beginNodeXOffset: 0.0,
    endNodeXOffset: 0.0,
    beginNodeYOffset: 0.0,
    endNodeYOffset: 0.0,
    beginNodeZOffset: 0.0,
    endNodeZOffset: 0.0
);

// Add to model
model.AddXmiColumn(column);

// Link to material and cross-section
var hasMaterial = new XmiHasMaterial(column, concreteMaterial);
var hasCrossSection = new XmiHasCrossSection(column, rectangular400);
```

## Common Column Types

- **Interior Columns** - Support floor beams in building core
- **Edge Columns** - Located at building perimeter
- **Corner Columns** - At building corners
- **Transfer Columns** - Collect loads from multiple floors

## Related Classes

- **XmiStructuralCurveMember** - Analytical member linked to physical column
- **XmiBeam** - Beam supported by column
- **XmiHasMaterial** - Material relationship
- **XmiHasCrossSection** - Cross-section relationship
- **XmiSystemLineEnum** - System line position enum
