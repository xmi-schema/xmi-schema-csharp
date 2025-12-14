---
title: "XmiManager Class"
layout: default
parent: "API Reference"
nav_order: 3
---

# XmiManager Class

Manages a collection of `XmiModel` instances and provides higher-level orchestration methods.

## Constructor

```csharp
public XmiManager()
```

## Properties

| Property | Type | Description |
|----------|------|-------------|
| `Models` | `List<XmiModel>` | Collection of managed XMI models |

## Model Management Methods

### AddModel

Adds a new XMI model to the manager.

```csharp
public void AddModel(XmiModel model)
```

### GetModel

Retrieves a model by index.

```csharp
public XmiModel GetModel(int modelIndex)
```

## Factory Methods

The `XmiManager` provides factory methods that delegate to specific models, with index validation.

### Physical Element Factories

#### CreateXmiBeam

```csharp
public XmiBeam CreateXmiBeam(
    int modelIndex,
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiMaterial? material,
    List<XmiSegment>? segments,
    List<int>? positions,
    XmiSystemLineEnum systemLine,
    double length,
    XmiAxis localAxisX,
    XmiAxis localAxisY,
    XmiAxis localAxisZ,
    double beginNodeXOffset,
    double endNodeXOffset,
    double beginNodeYOffset,
    double endNodeYOffset,
    double beginNodeZOffset,
    double endNodeZOffset
)
```

#### CreateXmiColumn

```csharp
public XmiColumn CreateXmiColumn(
    int modelIndex,
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiMaterial? material,
    List<XmiSegment>? segments,
    List<int>? positions,
    XmiSystemLineEnum systemLine,
    double length,
    XmiAxis localAxisX,
    XmiAxis localAxisY,
    XmiAxis localAxisZ,
    double beginNodeXOffset,
    double endNodeXOffset,
    double beginNodeYOffset,
    double endNodeYOffset,
    double beginNodeZOffset,
    double endNodeZOffset
)
```

#### CreateXmiSlab

```csharp
public XmiSlab CreateXmiSlab(
    int modelIndex,
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiMaterial? material,
    List<XmiSegment>? segments,
    List<int>? positions,
    double zOffset,
    XmiAxis localAxisX,
    XmiAxis localAxisY,
    XmiAxis localAxisZ,
    double thickness
)
```

#### CreateXmiWall

```csharp
public XmiWall CreateXmiWall(
    int modelIndex,
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiMaterial? material,
    List<XmiSegment>? segments,
    List<int>? positions,
    double zOffset,
    XmiAxis localAxisX,
    XmiAxis localAxisY,
    XmiAxis localAxisZ,
    double height
)
```

### Structural Analytical Element Factories

#### CreateXmiStructuralCurveMember

```csharp
public XmiStructuralCurveMember CreateXmiStructuralCurveMember(
    int modelIndex,
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiMaterial? material,
    XmiCrossSection? crossSection,
    XmiStorey? storey,
    XmiStructuralCurveMemberTypeEnum curveMemberType,
    List<XmiStructuralPointConnection> nodes,
    List<XmiSegment>? segments,
    List<int>? positions,
    XmiSystemLineEnum systemLine,
    XmiStructuralPointConnection beginNode,
    XmiStructuralPointConnection endNode,
    double length,
    XmiAxis localAxisX,
    XmiAxis localAxisY,
    XmiAxis localAxisZ,
    double beginNodeXOffset,
    double endNodeXOffset,
    double beginNodeYOffset,
    double endNodeYOffset,
    double beginNodeZOffset,
    double endNodeZOffset,
    string endFixityStart,
    string endFixityEnd
)
```

#### CreateXmiStructuralSurfaceMember

```csharp
public XmiStructuralSurfaceMember CreateXmiStructuralSurfaceMember(
    int modelIndex,
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiMaterial? material,
    XmiStructuralSurfaceMemberTypeEnum surfaceMemberType,
    double thickness,
    XmiStructuralSurfaceMemberSystemPlaneEnum systemPlane,
    List<XmiStructuralPointConnection> nodes,
    XmiStorey? storey,
    List<XmiSegment> segments,
    List<int> positions,
    double area,
    double zOffset,
    XmiAxis localAxisX,
    XmiAxis localAxisY,
    XmiAxis localAxisZ,
    double height
)
```

#### CreateXmiStructuralPointConnection

```csharp
public XmiStructuralPointConnection CreateXmiStructuralPointConnection(
    int modelIndex,
    string id,
    string name,
    string ifcGuid,
    string nativeId,
    string description,
    XmiPoint3d point
)
```

## Entity Management Methods

### AddXmiMaterialToModel

```csharp
public void AddXmiMaterialToModel(int modelIndex, XmiMaterial material)
```

### AddXmiCrossSectionToModel

```csharp
public void AddXmiCrossSectionToModel(int modelIndex, XmiCrossSection crossSection)
```

### AddXmiStoreyToModel

```csharp
public void AddXmiStoreyToModel(int modelIndex, XmiStorey storey)
```

### AddXmiStructuralCurveMemberToModel

```csharp
public void AddXmiStructuralCurveMemberToModel(int modelIndex, XmiStructuralCurveMember member)
```

### AddXmiStructuralSurfaceMemberToModel

```csharp
public void AddXmiStructuralSurfaceMemberToModel(int modelIndex, XmiStructuralSurfaceMember member)
```

### AddXmiStructuralPointConnectionToModel

```csharp
public void AddXmiStructuralPointConnectionToModel(int modelIndex, XmiStructuralPointConnection connection)
```

## Usage Example

```csharp
// Create manager
var manager = new XmiManager();

// Add a model
manager.AddModel(new XmiModel());

// Create entities in specific model
var material = manager.CreateXmiMaterial(0, "mat-1", "Steel", "", "steel-1", "", 
    XmiStructuralMaterialTypeEnum.Steel, 7850.0, 210000.0, 81000.0);

var beam = manager.CreateXmiBeam(0, "beam-1", "Main Beam", "", "beam-1", "", 
    material, null, null, XmiSystemLineEnum.MiddleMiddle, 6.0,
    axisX, axisY, axisZ, 0, 0, 0, 0, 0, 0);

// Get the model
var model = manager.GetModel(0);
```

## Error Handling

All factory methods validate the `modelIndex` parameter and throw `IndexOutOfRangeException` for invalid indices.