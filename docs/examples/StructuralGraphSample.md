---
title: "Structural Graph Sample"
layout: default
parent: "Examples"
nav_order: 1
---

# Structural Graph Sample

This example demonstrates how to create a complete structural analysis model using the XMI Schema C# library.

## Overview

The sample creates a simple structural frame with:
- 2 columns (vertical members)
- 1 beam (horizontal member)
- 1 slab (surface member)
- Materials and cross-sections
- Proper relationships between elements

## Complete Code

```csharp
using System;
using System.Collections.Generic;
using XmiSchema.Managers;
using XmiSchema.Entities.Commons;
using XmiSchema.Entities.StructuralAnalytical;
using XmiSchema.Entities.Physical;
using XmiSchema.Entities.Geometries;
using XmiSchema.Entities.Relationships;
using XmiSchema.Enums;

namespace XmiSchema.Examples.StructuralGraphSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create XMI model
            var model = new XmiModel();

            // Create materials
            var concrete = model.CreateXmiMaterial(
                "mat-concrete", "Concrete", "", "mat-conc-1", "",
                XmiStructuralMaterialTypeEnum.Concrete, 2400.0, 30000.0, 14000.0);

            var steel = model.CreateXmiMaterial(
                "mat-steel", "Steel", "", "mat-steel-1", "",
                XmiStructuralMaterialTypeEnum.Steel, 7850.0, 210000.0, 81000.0);

            // Create cross-sections
            var columnSection = model.CreateXmiCrossSection(
                "cs-column", "Column Section", "", "cs-col-1", "",
                XmiShapeEnum.Rectangular, 0.3, 0.3, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);

            var beamSection = model.CreateXmiCrossSection(
                "cs-beam", "Beam Section", "", "cs-beam-1", "",
                XmiShapeEnum.Rectangular, 0.25, 0.4, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);

            // Create storey
            var storey = model.CreateXmiStorey(
                "storey-1", "Ground Floor", "", "storey-1", "",
                0.0, 1000.0, 0.0, 0.0);

            // Create points for columns
            var column1Base = model.CreateXmiPoint3D(0.0, 0.0, 0.0, "pt-col1-base");
            var column1Top = model.CreateXmiPoint3D(0.0, 0.0, 3.0, "pt-col1-top");
            var column2Base = model.CreateXmiPoint3D(6.0, 0.0, 0.0, "pt-col2-base");
            var column2Top = model.CreateXmiPoint3D(6.0, 0.0, 3.0, "pt-col2-top");

            // Create points for beam
            var beamStart = model.CreateXmiPoint3D(0.0, 0.0, 3.0, "pt-beam-start");
            var beamEnd = model.CreateXmiPoint3D(6.0, 0.0, 3.0, "pt-beam-end");

            // Create structural point connections (nodes)
            var column1Node = model.CreateXmiStructuralPointConnection("node-col1", "Column 1 Node", "", "node-col1", "", column1Top);
            var column2Node = model.CreateXmiStructuralPointConnection("node-col2", "Column 2 Node", "", "node-col2", "", column2Top);
            var beamStartNode = model.CreateXmiStructuralPointConnection("node-beam-start", "Beam Start Node", "", "node-beam-start", "", beamStart);
            var beamEndNode = model.CreateXmiStructuralPointConnection("node-beam-end", "Beam End Node", "", "node-beam-end", "", beamEnd);

            // Create segments for beam (with positions)
            var beamSegments = new List<XmiSegment>
            {
                new XmiSegment("seg-beam-1", "Beam Segment 1", "", "seg-beam-1", "", XmiSegmentTypeEnum.Line),
                new XmiSegment("seg-beam-2", "Beam Segment 2", "", "seg-beam-2", "", XmiSegmentTypeEnum.Line)
            };
            var beamPositions = new List<int> { 0, 1 };

            // Create structural curve members
            var column1 = model.CreateXmiStructuralCurveMember(
                "col-1", "Column 1", "", "col-1", "",
                concrete, columnSection, storey, XmiStructuralCurveMemberTypeEnum.Column,
                new List<XmiStructuralPointConnection> { column1Node },
                null, null, XmiSystemLineEnum.MiddleMiddle,
                column1Node, column1Node, 3.0,
                new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1),
                0, 0, 0, 0, 0, 0, "Fixed", "Fixed");

            var column2 = model.CreateXmiStructuralCurveMember(
                "col-2", "Column 2", "", "col-2", "",
                concrete, columnSection, storey, XmiStructuralCurveMemberTypeEnum.Column,
                new List<XmiStructuralPointConnection> { column2Node },
                null, null, XmiSystemLineEnum.MiddleMiddle,
                column2Node, column2Node, 3.0,
                new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1),
                0, 0, 0, 0, 0, 0, "Fixed", "Fixed");

            var beam = model.CreateXmiStructuralCurveMember(
                "beam-1", "Main Beam", "", "beam-1", "",
                steel, beamSection, storey, XmiStructuralCurveMemberTypeEnum.Beam,
                new List<XmiStructuralPointConnection> { beamStartNode, beamEndNode },
                beamSegments, beamPositions, XmiSystemLineEnum.MiddleMiddle,
                beamStartNode, beamEndNode, 6.0,
                new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1),
                0, 0, 0, 0, 0, 0, "Fixed", "Pinned");

            // Create slab corners
            var slabCorner1 = model.CreateXmiPoint3D(0.0, 0.0, 3.0, "pt-slab-1");
            var slabCorner2 = model.CreateXmiPoint3D(6.0, 0.0, 3.0, "pt-slab-2");
            var slabCorner3 = model.CreateXmiPoint3D(6.0, 4.0, 3.0, "pt-slab-3");
            var slabCorner4 = model.CreateXmiPoint3D(0.0, 4.0, 3.0, "pt-slab-4");

            // Create slab nodes
            var slabNodes = new List<XmiStructuralPointConnection>
            {
                model.CreateXmiStructuralPointConnection("node-slab-1", "Slab Node 1", "", "node-slab-1", "", slabCorner1),
                model.CreateXmiStructuralPointConnection("node-slab-2", "Slab Node 2", "", "node-slab-2", "", slabCorner2),
                model.CreateXmiStructuralPointConnection("node-slab-3", "Slab Node 3", "", "node-slab-3", "", slabCorner3),
                model.CreateXmiStructuralPointConnection("node-slab-4", "Slab Node 4", "", "node-slab-4", "", slabCorner4)
            };

            // Create slab segments
            var slabSegments = new List<XmiSegment>
            {
                new XmiSegment("seg-slab-1", "Slab Edge 1", "", "seg-slab-1", "", XmiSegmentTypeEnum.Line),
                new XmiSegment("seg-slab-2", "Slab Edge 2", "", "seg-slab-2", "", XmiSegmentTypeEnum.Line),
                new XmiSegment("seg-slab-3", "Slab Edge 3", "", "seg-slab-3", "", XmiSegmentTypeEnum.Line),
                new XmiSegment("seg-slab-4", "Slab Edge 4", "", "seg-slab-4", "", XmiSegmentTypeEnum.Line)
            };
            var slabPositions = new List<int> { 0, 1, 2, 3 };

            // Create structural surface member (slab)
            var slab = model.CreateXmiStructuralSurfaceMember(
                "slab-1", "Main Slab", "", "slab-1", "",
                concrete, XmiStructuralSurfaceMemberTypeEnum.Slab, 0.2,
                XmiStructuralSurfaceMemberSystemPlaneEnum.Middle,
                slabNodes, storey, slabSegments, slabPositions, 24.0, 3.0,
                new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1), 0.2);

            // Print summary
            Console.WriteLine("XMI Structural Graph Sample Created Successfully!");
            Console.WriteLine($"Entities: {model.Entities.Count}");
            Console.WriteLine($"Relationships: {model.Relationships.Count}");
            Console.WriteLine();
            Console.WriteLine("Created Elements:");
            Console.WriteLine($"- Materials: {model.Entities.OfType<XmiMaterial>().Count()}");
            Console.WriteLine($"- Cross-Sections: {model.Entities.OfType<XmiCrossSection>().Count()}");
            Console.WriteLine($"- Storeys: {model.Entities.OfType<XmiStorey>().Count()}");
            Console.WriteLine($"- Points: {model.Entities.OfType<XmiPoint3d>().Count()}");
            Console.WriteLine($"- Structural Nodes: {model.Entities.OfType<XmiStructuralPointConnection>().Count()}");
            Console.WriteLine($"- Curve Members: {model.Entities.OfType<XmiStructuralCurveMember>().Count()}");
            Console.WriteLine($"- Surface Members: {model.Entities.OfType<XmiStructuralSurfaceMember>().Count()}");
            Console.WriteLine($"- Segments: {model.Entities.OfType<XmiSegment>().Count()}");
        }
    }
}
```

## Key Concepts Demonstrated

### 1. Entity Creation
- **Materials**: Concrete and steel with material properties
- **Cross-Sections**: Rectangular profiles for columns and beams
- **Storeys**: Floor level representation
- **Points**: 3D coordinates with automatic deduplication
- **Nodes**: Structural point connections
- **Curve Members**: Columns and beams with fixity conditions
- **Surface Members**: Slabs with boundary definition
- **Segments**: Logical divisions along members

### 2. Relationships
- **Material Assignment**: `XmiHasMaterial` relationships
- **Cross-Section Assignment**: `XmiHasCrossSection` relationships
- **Storey Assignment**: `XmiHasStorey` relationships
- **Geometry Association**: Point connections to geometric points
- **Segment Positioning**: `XmiHasSegment` relationships with positions

### 3. API Features
- **Automatic Deduplication**: Points with same coordinates are reused
- **Position Handling**: Segment positions managed through relationships
- **Validation**: Proper parameter validation in factory methods
- **Type Safety**: Strongly typed enums for all properties

## Running the Sample

1. Build the solution:
   ```bash
   dotnet build XmiSchema.sln
   ```

2. Run the sample:
   ```bash
   dotnet run --project examples/StructuralGraphSample/StructuralGraphSample.csproj
   ```

## Expected Output

```
XMI Structural Graph Sample Created Successfully!
Entities: 25
Relationships: 28

Created Elements:
- Materials: 2
- Cross-Sections: 2
- Storeys: 1
- Points: 8
- Structural Nodes: 7
- Curve Members: 3
- Surface Members: 1
- Segments: 6
```

## JSON Serialization

The model can be serialized to JSON for data exchange:

```csharp
using Newtonsoft.Json;

var json = JsonConvert.SerializeObject(model, Formatting.Indented);
File.WriteAllText("structural-model.json", json);
```

This creates a complete JSON representation of the structural model that can be shared between different engineering applications.