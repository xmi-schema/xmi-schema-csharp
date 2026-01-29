# XmiWall

Represents a physical wall element in the built environment.

## Purpose

`XmiWall` defines vertical planar elements that provide enclosure, support floors, or resist lateral loads. Walls are physical representations of structural elements that can be linked to analytical surface members for analysis purposes.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `ZOffset` | `double` | Yes | Offset along Z axis relative to host (meters) |
| `LocalAxisX` | `XmiAxis` | Yes | Unit direction of local X axis |
| `LocalAxisY` | `XmiAxis` | Yes | Unit direction of local Y axis |
| `LocalAxisZ` | `XmiAxis` | Yes | Unit direction of local Z axis |
| `Height` | `double` | Yes | Physical height of wall (meters) |

## Usage Example

```csharp
// Create a concrete shear wall
var wall = new XmiWall(
    id: "wall-1",
    name: "Shear Wall W1-Grid A",
    ifcGuid: "2g0X$0m55u9hX1w2a7x5f",
    nativeId: "WALL-W1",
    description: "Reinforced concrete shear wall resisting lateral loads",
    zOffset: 0.0,
    localAxisX: new XmiAxis(1, 0, 0),    // Along wall length
    localAxisY: new XmiAxis(0, 1, 0),    // Perpendicular to wall
    localAxisZ: new XmiAxis(0, 0, 1),    // Vertical
    height: 3.5  // 3500mm high
);

// Add to model
model.AddXmiWall(wall);

// Link to material and geometry
var hasMaterial = new XmiHasMaterial(wall, concreteMaterial);
var hasGeometry = new XmiHasGeometry(wall, polygonGeometry);
```

## Common Wall Types

- **Load-Bearing Walls** - Transfer vertical loads from floors/roofs to foundations
- **Shear Walls** - Provide lateral stability and resist wind/seismic loads
- **Non-Load-Bearing Walls** - Provide enclosure only (partition walls)
- **Retaining Walls** - Resist lateral earth pressure
- **Curtain Walls** - Exterior cladding not supporting loads

## Related Classes

- **XmiStructuralSurfaceMember** - Analytical member linked to physical wall
- **XmiStorey** - Storey hosting wall
- **XmiHasMaterial** - Material relationship
- **XmiHasGeometry** - Geometry relationship
