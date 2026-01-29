# XmiStructuralCurveMember

Represents a linear structural element (beam, column, brace) in XMI analytical model.

## Purpose

`XmiStructuralCurveMember` defines 1D analytical members used for structural analysis.

## Properties

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `CurveMemberType` | `XmiStructuralCurveMemberTypeEnum` | Yes | Member type classification |
| `SystemLine` | `XmiSystemLineEnum` | Yes | Relative position of analytical line |
| `Length` | `double` | Yes | Analytical length of element (meters) |
| `LocalAxisX` | `XmiAxis` | Yes | Unit direction of local X axis |
| `LocalAxisY` | `XmiAxis` | Yes | Unit direction of local Y axis |
| `LocalAxisZ` | `XmiAxis` | Yes | Unit direction of local Z axis |
| `BeginNodeXOffset` | `double` | Yes | X offset at start node (meters) |
| `EndNodeXOffset` | `double` | Yes | X offset at end node (meters) |
| `BeginNodeYOffset` | `double` | Yes | Y offset at start node (meters) |
| `EndNodeYOffset` | `double` | Yes | Y offset at end node (meters) |
| `BeginNodeZOffset` | `double` | Yes | Z offset at start node (meters) |
| `EndNodeZOffset` | `double` | Yes | Z offset at end node (meters) |
| `EndFixityStart` | `string` | Yes | Boundary condition at start (e.g., "fixed", "pinned") |
| `EndFixityEnd` | `string` | Yes | Boundary condition at end (e.g., "fixed", "pinned") |

## Usage Example

```csharp
var member = new XmiStructuralCurveMember(
    id: "mem-1",
    name: "Analytical Beam B-1",
    ifcGuid: "2g0X$0m55u9hX1w2a7x5f",
    nativeId: "MEM-B1",
    description: "Primary floor beam for structural analysis",
    curveMemberType: XmiStructuralCurveMemberTypeEnum.Beam,
    systemLine: XmiSystemLineEnum.MiddleMiddle,
    length: 6.0,
    localAxisX: new XmiAxis(1, 0, 0),
    localAxisY: new XmiAxis(0, 0, 1),
    localAxisZ: new XmiAxis(0, 1, 0),
    beginNodeXOffset: 0.0,
    endNodeXOffset: 0.0,
    beginNodeYOffset: 0.0,
    endNodeYOffset: 0.0,
    beginNodeZOffset: 0.0,
    endNodeZOffset: 0.0,
    endFixityStart: "pinned",
    endFixityEnd: "continuous"
);

model.AddXmiStructuralCurveMember(member);
```

## Related Classes

- **XmiBeam** / **XmiColumn** - Physical element linked to analytical member
- **XmiStructuralPointConnection** - Node connections at member ends
- **XmiHasLine3D** - Geometry relationship
- **XmiStructuralCurveMemberTypeEnum** - Member type classification
