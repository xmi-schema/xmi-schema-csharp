using System;
using System.Collections.Generic;
using XmiSchema.Core.Entities;
using XmiSchema.Core.Enums;
using XmiSchema.Core.Manager;
using XmiSchema.Core.Models;
using XmiSchema.Core.Models.Entities.StructuralAnalytical;
using XmiSchema.Core.Parameters;

var manager = new XmiManager();
manager.Models.Add(new XmiModel());

// Create basic metadata
var storey = manager.CreateStorey(
    modelIndex: 0,
    id: "storey-1",
    name: "Level 1",
    ifcGuid: "storey-guid",
    nativeId: "LEVEL_1",
    description: "Entry level",
    storeyElevation: 0,
    storeyMass: 800);

var basePoint = manager.CreatePoint3D(0, "pt-start", "Start", "pt-guid", "PT_START", "Start point", 0, 0, 0);
var topPoint = manager.CreatePoint3D(0, "pt-end", "End", "pt-guid-2", "PT_END", "End point", 0, 0, 3);

var startConnection = manager.CreateStructuralPointConnection(0, "pc-start", "Start Node", "pc-guid", "PC_START", "Column base", storey, basePoint);
var endConnection = manager.CreateStructuralPointConnection(0, "pc-end", "End Node", "pc-guid-2", "PC_END", "Column top", storey, topPoint);

var material = manager.CreateMaterial(
    0,
    "mat-1",
    "Concrete C40",
    "mat-guid",
    "MAT_C40",
    "Typical concrete",
    XmiMaterialTypeEnum.Concrete,
    grade: 40,
    unitWeight: 24,
    eModulus: "33000",
    gModulus: "13000",
    poissonRatio: "0.2",
    thermalCoefficient: 1.0);

var crossSection = manager.CreateCrossSection(
    0,
    "sec-rect",
    "400x400",
    "sec-guid",
    "SEC_400",
    "Column section",
    material,
    XmiShapeEnum.Rectangular,
    new RectangularShapeParameters(0.4, 0.4),
    area: 0.16,
    secondMomentOfAreaXAxis: 0.0021,
    secondMomentOfAreaYAxis: 0.0021,
    radiusOfGyrationXAxis: 0.14,
    radiusOfGyrationYAxis: 0.14,
    elasticModulusXAxis: 0.014,
    elasticModulusYAxis: 0.014,
    plasticModulusXAxis: 0.02,
    plasticModulusYAxis: 0.02,
    torsionalConstant: 0.0005);

var curveMember = manager.CreateStructuralCurveMember(
    0,
    "col-1",
    "Grid A/1 Column",
    "cur-guid",
    "COLUMN_A1",
    "Sample column",
    crossSection,
    storey,
    XmiStructuralCurveMemberTypeEnum.Column,
    new List<XmiStructuralPointConnection> { startConnection, endConnection },
    new List<XmiSegment> { new("seg-1", "Segment", "seg-guid", "SEG_1", "Straight segment", 0f, XmiSegmentTypeEnum.Line) },
    XmiSystemLineEnum.MiddleMiddle,
    startConnection,
    endConnection,
    length: 3.0,
    localAxisX: "1,0,0",
    localAxisY: "0,1,0",
    localAxisZ: "0,0,1",
    beginNodeXOffset: 0,
    endNodeXOffset: 0,
    beginNodeYOffset: 0,
    endNodeYOffset: 0,
    beginNodeZOffset: 0,
    endNodeZOffset: 0,
    endFixityStart: "Fixed",
    endFixityEnd: "Pinned");

var json = manager.BuildJson(0);
Console.WriteLine("Generated XMI graph:");
Console.WriteLine(json);
