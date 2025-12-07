using System.Linq;
using XmiSchema.Core.Entities;
using XmiSchema.Core.Enums;
using XmiSchema.Core.Geometries;
using XmiSchema.Core.Relationships;
using XmiSchema.Core.Parameters;
using XmiSchema.Core.Models.Entities.StructuralAnalytical;

namespace XmiSchema.Core.Tests.Models.GraphModel;

/// <summary>
/// Covers the factory helpers exposed on <see cref="Core.XmiModel"/>.
/// </summary>
public class XmiModelTests
{
    /// <summary>
    /// Points with identical coordinates are deduplicated.
    /// </summary>
    [Fact]
    public void CreatePoint3D_ReusesExistingPoint()
    {
        var model = new XmiModel();

        var first = model.CreateXmiPoint3d("pt-1", "Point", "ifc", "native", "desc", 1, 2, 3);
        var second = model.CreateXmiPoint3d("pt-2", "Point", "ifc", "native2", "desc", 1, 2, 3);

        Assert.Same(first, second);
        Assert.Single(model.Entities.OfType<XmiPoint3d>());
    }

    /// <summary>
    /// Materials sharing the same native id are deduplicated.
    /// </summary>
    [Fact]
    public void CreateMaterial_ReusesNativeId()
    {
        var model = new XmiModel();

        var first = model.CreateXmiMaterial("mat-1", "Mat", "ifc", "NATIVE", "desc", XmiMaterialTypeEnum.Steel, 50, 78.5, "200000", "80000", "0.3", 1.1);
        var second = model.CreateXmiMaterial("mat-2", "Mat", "ifc", "NATIVE", "desc", XmiMaterialTypeEnum.Steel, 50, 78.5, "200000", "80000", "0.3", 1.1);

        Assert.Same(first, second);
        Assert.Single(model.Entities.OfType<XmiMaterial>());
    }

    /// <summary>
    /// Creating a point connection attaches storey and point relationships.
    /// </summary>
    [Fact]
    public void CreateStructurePointConnection_AddsRelationships()
    {
        var model = new XmiModel();
        var storey = TestModelFactory.CreateStorey();
        var point = TestModelFactory.CreatePoint();
        model.AddXmiStorey(storey);
        model.AddXmiPoint3D(point);

        var connection = model.CreateXmiStructurePointConnection("pc-1", "Node", "ifc", "native", "desc", storey, point);

        Assert.Contains(model.Entities.OfType<XmiStructuralPointConnection>(), e => e.Id == connection.Id);
        Assert.Contains(model.Relationships.OfType<XmiHasStorey>(), r => r.Source == connection && r.Target == storey);
        Assert.Contains(model.Relationships.OfType<XmiHasPoint3d>(), r => r.Source == connection && r.Target == point);
    }

    /// <summary>
    /// Creating a cross-section with a known material establishes the material relationship.
    /// </summary>
    [Fact]
    public void CreateCrossSection_AddsMaterialRelationship()
    {
        var model = new XmiModel();
        var material = TestModelFactory.CreateMaterial();
        model.AddXmiMaterial(material);

        var section = model.CreateXmiCrossSection(
            "sec-1",
            "Section",
            "ifc",
            "native",
            "desc",
            material,
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

        Assert.Contains(model.Entities.OfType<XmiCrossSection>(), e => e.Id == section.Id);
        Assert.Contains(model.Relationships.OfType<XmiHasMaterial>(), r => r.Source == section && r.Target.NativeId == material.NativeId);
    }

    /// <summary>
    /// Creating a curve member attaches cross-section, storey, and node relationships.
    /// </summary>
    [Fact]
    public void CreateStructuralCurveMember_AddsExpectedRelationships()
    {
        var model = new XmiModel();
        var crossSection = TestModelFactory.CreateCrossSection();
        var storey = TestModelFactory.CreateStorey();
        var beginNode = TestModelFactory.CreatePointConnection("pc-begin");
        var endNode = TestModelFactory.CreatePointConnection("pc-end");
        model.AddXmiCrossSection(crossSection);
        model.AddXmiStorey(storey);
        model.AddXmiStructuralPointConnection(beginNode);
        model.AddXmiStructuralPointConnection(endNode);

        var member = model.CreateXmiStructuralCurveMember(
            "cur-1",
            "Member",
            "ifc",
            "native",
            "desc",
            crossSection,
            storey,
            XmiStructuralCurveMemberTypeEnum.Beam,
            new List<XmiStructuralPointConnection> { beginNode, endNode },
            new List<XmiSegment> { TestModelFactory.CreateSegment() },
            XmiSystemLineEnum.MiddleMiddle,
            beginNode,
            endNode,
            5.0,
            "1,0,0",
            "0,1,0",
            "0,0,1",
            0,
            0,
            0,
            0,
            0,
            0,
            "Fixed",
            "Pinned");

        Assert.Contains(model.Entities.OfType<XmiStructuralCurveMember>(), e => e.Id == member.Id);
        Assert.Contains(model.Relationships.OfType<XmiHasCrossSection>(), r => r.Source == member && r.Target == crossSection);
        Assert.Contains(model.Relationships.OfType<XmiHasStorey>(), r => r.Source == member && r.Target == storey);
        Assert.Equal(2, model.Relationships.OfType<XmiHasStructuralPointConnection>().Count());
    }

    /// <summary>
    /// Creating a surface member reuses storey/material references and records relationships.
    /// </summary>
    [Fact]
    public void CreateStructuralSurfaceMember_AttachesRelationships()
    {
        var model = new XmiModel();
        var storey = TestModelFactory.CreateStorey();
        var material = TestModelFactory.CreateMaterial();
        model.AddXmiStorey(storey);
        model.AddXmiMaterial(material);

        var member = model.CreateXmiStructuralSurfaceMember(
            "surf-1",
            "Surface",
            "ifc",
            "native",
            "desc",
            material,
            XmiStructuralSurfaceMemberTypeEnum.Slab,
            0.2,
            XmiStructuralSurfaceMemberSystemPlaneEnum.Middle,
            new List<XmiStructuralPointConnection> { TestModelFactory.CreatePointConnection() },
            storey,
            new List<XmiSegment> { TestModelFactory.CreateSegment() },
            12.5,
            0,
            "1,0,0",
            "0,1,0",
            "0,0,1",
            0.3);

        Assert.Contains(model.Entities.OfType<XmiStructuralSurfaceMember>(), e => e.Id == member.Id);
        Assert.Contains(model.Relationships.OfType<XmiHasStorey>(), r => r.Source == member && r.Target.NativeId == storey.NativeId);
        Assert.Contains(model.Relationships.OfType<XmiHasMaterial>(), r => r.Source == member && r.Target.NativeId == material.NativeId);
    }
}
