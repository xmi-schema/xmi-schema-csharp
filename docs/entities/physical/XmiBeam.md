# XmiBeam

Represents a physical beam element in the built environment.

## Purpose

`XmiBeam` defines horizontal structural members that support floors, roofs, or other loads. Beams are physical representations of structural elements that can be linked to analytical members for analysis purposes.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `SystemLine` | `XmiSystemLineEnum` | Yes | Relative position of analytical line inside profile |
| `Length` | `double` | Yes | Physical length of beam (meters) |
| `LocalAxisX` | `XmiAxis` | Yes | Unit direction of local X axis |
| `LocalAxisY` | `XmiAxis` | Yes | Unit direction of local Y axis |
| `LocalAxisZ` | `XmiAxis` | Yes | Unit direction of local Z axis |
| `BeginNodeXOffset` | `double` | Yes | X offset applied to start node (meters) |
| `EndNodeXOffset` | `double` | Yes | X offset applied to end node (meters) |
| `BeginNodeYOffset` | `double` | Yes | Y offset applied to start node (meters) |
| `EndNodeYOffset` | `double` | Yes | Y offset applied to end node (meters) |
| `BeginNodeZOffset` | `double` | Yes | Z offset applied to start node (meters) |
| `EndNodeZOffset` | `double` | Yes | Z offset applied to end node (meters) |

## SystemLine Position

The `SystemLine` property defines where analytical line runs through physical cross-section:

| SystemLine | Description | Typical Use |
|-------------|-------------|--------------|
| **TopTop** | Top of top flange | Composite slab connection |
| **TopMiddle** | Mid-height of top flange | Moment connections |
| **TopBottom** | Bottom of top flange | Precast connections |
| **MiddleMiddle** | Centroid of section | Default analytical position |
| **BottomTop** | Top of bottom flange | Shear connections |
| **BottomMiddle** | Mid-height of bottom flange | General framing |
| **BottomBottom** | Bottom of bottom flange | Seat connections |

## Usage Example

```csharp
// Create a steel beam
var beam = new XmiBeam(
    id: "beam-1",
    name: "Steel Beam B-1",
    ifcGuid: "2g0X$0m55u9hX1w2a7x5f",
    nativeId: "BEAM-B1",
    description: "Primary floor beam spanning 6 meters",
    systemLine: XmiSystemLineEnum.MiddleMiddle,
    length: 6.0,
    localAxisX: new XmiAxis(1, 0, 0),    // Along beam
    localAxisY: new XmiAxis(0, 0, 1),    // Horizontal
    localAxisZ: new XmiAxis(0, 1, 0),    // Vertical
    beginNodeXOffset: 0.0,
    endNodeXOffset: 0.0,
    beginNodeYOffset: 0.0,
    endNodeYOffset: 0.0,
    beginNodeZOffset: 0.0,
    endNodeZOffset: 0.0
);

// Add to model
model.AddXmiBeam(beam);

// Link to material and cross-section
var hasMaterial = new XmiHasMaterial(beam, steelMaterial);
var hasCrossSection = new XmiHasCrossSection(beam, ipe300);
```

## Common Beam Types

- **Primary Beams** - Transfer loads from secondary beams to columns
- **Secondary Beams** - Transfer loads from floor to primary beams
- **Transfer Beams** - Transfer loads from one area to another
- **Edge Beams** - Define perimeter of floor or roof

## Related Classes

- **XmiStructuralCurveMember** - Analytical member linked to physical beam
- **XmiColumn** - Column supporting beams
- **XmiHasMaterial** - Material relationship
- **XmiHasCrossSection** - Cross-section relationship
- **XmiSystemLineEnum** - System line position enum
