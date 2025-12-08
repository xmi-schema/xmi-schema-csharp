using System.Collections.Generic;
using XmiSchema.Entities.Bases;
using XmiSchema.Parameters;
using XmiSchema.Entities.Physical;
using XmiSchema.Entities.StructuralAnalytical;
using XmiSchema.Entities.Geometries;
using XmiSchema.Entities.Commons;
using XmiSchema.Enums;
using XmiSchema.Managers;

namespace XmiSchema.Tests.Managers;

/// <summary>
/// Creates fully-populated domain objects that keep the unit tests terse.
/// </summary>
internal static class TestModelFactory
{
    internal static XmiMaterial CreateMaterial(string id = "mat-1") =>
        new(id,
            $"Material {id}",
            "ifc-guid",
            id.ToUpperInvariant(),
            "Test material",
            XmiMaterialTypeEnum.Steel,
            50,
            78.5,
            "200000",
            "80000",
            "0.3",
            1.2);

    internal static XmiCrossSection CreateCrossSection(string id = "sec-1") =>
        new(id,
            $"Section {id}",
            "ifc-guid",
            id.ToUpperInvariant(),
            "Rectangular section",
            XmiShapeEnum.Rectangular,
            new RectangularShapeParameters(0.3, 0.6),
            0.18,
            0.002,
            0.003,
            0.01,
            0.02,
            0.0005,
            0.0006,
            0.0007,
            0.0008,
            0.0009);

    internal static XmiStorey CreateStorey(string id = "str-1") =>
        new(id,
            $"Storey {id}",
            "ifc-guid",
            id.ToUpperInvariant(),
            "Level description",
            12.0,
            1000);

    internal static XmiPoint3d CreatePoint(string id = "pt-1", double x = 1, double y = 2, double z = 3) =>
        new(id,
            $"Point {id}",
            "ifc-guid",
            id.ToUpperInvariant(),
            "Point description",
            x,
            y,
            z);

    internal static XmiStructuralPointConnection CreatePointConnection(string id = "pc-1") =>
        new(id,
            $"PointConn {id}",
            "ifc-guid",
            id.ToUpperInvariant(),
            "Point connection");

    internal static XmiSegment CreateSegment(string id = "seg-1") =>
        new(id,
            $"Segment {id}",
            "ifc-guid",
            id.ToUpperInvariant(),
            "Segment description",
            0.5f,
            XmiSegmentTypeEnum.Line);

    internal static XmiStructuralCurveMember CreateCurveMember(string id = "cur-1") =>
        new(id,
            $"Curve {id}",
            "ifc-guid",
            id.ToUpperInvariant(),
            "Curve member",
            XmiStructuralCurveMemberTypeEnum.Beam,
            XmiSystemLineEnum.MiddleMiddle,
            5.0,
            "1,0,0",
            "0,1,0",
            "0,0,1",
            0.1,
            0.1,
            0,
            0,
            0,
            0,
            "Fixed",
            "Pinned");

    internal static XmiStructuralSurfaceMember CreateSurfaceMember(string id = "surf-1") =>
        new(id,
            $"Surface {id}",
            "ifc-guid",
            id.ToUpperInvariant(),
            "Surface member",
            XmiStructuralSurfaceMemberTypeEnum.Slab,
            0.2,
            XmiStructuralSurfaceMemberSystemPlaneEnum.Middle,
            12.5,
            0.0,
            "1,0,0",
            "0,1,0",
            "0,0,1",
            0.3);

    internal static XmiUnit CreateUnit(string id = "unit-1") =>
        new(id,
            $"Unit {id}",
            "ifc-guid",
            id.ToUpperInvariant(),
            "Unit mapping",
            nameof(XmiStructuralCurveMember),
            nameof(XmiStructuralCurveMember.Length),
            XmiUnitEnum.Meter);

    internal static XmiLine3d CreateLine(string id = "line-1") =>
        new(id,
            $"Line {id}",
            "ifc-guid",
            id.ToUpperInvariant(),
            "Line geometry",
            CreatePoint("line-start"),
            CreatePoint("line-end", 4, 5, 6));

    internal static XmiArc3d CreateArc(string id = "arc-1") =>
        new(id,
            $"Arc {id}",
            "ifc-guid",
            id.ToUpperInvariant(),
            "Arc geometry",
            CreatePoint("arc-start"),
            CreatePoint("arc-end", 7, 8, 9),
            CreatePoint("arc-center", 3, 3, 3),
            2.5f);

    internal static XmiBeam CreateBeam(string id = "beam-1") =>
        new(id,
            $"Beam {id}",
            "ifc-guid",
            id.ToUpperInvariant(),
            "Steel beam",
            XmiSystemLineEnum.MiddleMiddle,
            5.0,
            "1,0,0",
            "0,1,0",
            "0,0,1",
            0.1,
            0.1,
            0,
            0,
            0,
            0);

    internal static XmiColumn CreateColumn(string id = "col-1") =>
        new(id,
            $"Column {id}",
            "ifc-guid",
            id.ToUpperInvariant(),
            "Concrete column",
            XmiSystemLineEnum.MiddleMiddle,
            3.5,
            "1,0,0",
            "0,1,0",
            "0,0,1",
            0,
            0,
            0,
            0,
            0,
            0);

    internal static XmiSlab CreateSlab(string id = "slab-1") =>
        new(id,
            $"Slab {id}",
            "ifc-guid",
            id.ToUpperInvariant(),
            "Concrete slab");

    internal static XmiWall CreateWall(string id = "wall-1") =>
        new(id,
            $"Wall {id}",
            "ifc-guid",
            id.ToUpperInvariant(),
            "Concrete wall");

    internal static XmiModel CreateModelWithBasics()
    {
        var model = new XmiModel();
        model.AddXmiMaterial(CreateMaterial());
        model.AddXmiStorey(CreateStorey());
        model.AddXmiPoint3d(CreatePoint());
        model.AddXmiStructuralPointConnection(CreatePointConnection());
        model.AddXmiStructuralCurveMember(CreateCurveMember());
        return model;
    }

    internal static XmiManager CreateManagerWithModel()
    {
        var manager = new XmiManager();
        manager.Models.Add(CreateModelWithBasics());
        return manager;
    }
}
