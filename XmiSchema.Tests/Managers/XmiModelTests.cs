using XmiSchema.Parameters;
using XmiSchema.Entities.Physical;
using XmiSchema.Entities.StructuralAnalytical;
using XmiSchema.Entities.Geometries;
using XmiSchema.Entities.Commons;
using XmiSchema.Entities.Relationships;
using XmiSchema.Enums;
using XmiSchema.Managers;

namespace XmiSchema.Tests.Managers;

public class XmiModelTests
{
    private static readonly XmiAxis AxisX = new(1, 0, 0);
    private static readonly XmiAxis AxisY = new(0, 1, 0);
    private static readonly XmiAxis AxisZ = new(0, 0, 1);

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
        model.AddXmiPoint3d(point);

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
        var material = TestModelFactory.CreateMaterial();
        var storey = TestModelFactory.CreateStorey();
        var beginNode = TestModelFactory.CreatePointConnection("pc-begin");
        var endNode = TestModelFactory.CreatePointConnection("pc-end");
        model.AddXmiCrossSection(crossSection);
        model.AddXmiMaterial(material);
        model.AddXmiStorey(storey);
        model.AddXmiStructuralPointConnection(beginNode);
        model.AddXmiStructuralPointConnection(endNode);

        var member = model.CreateXmiStructuralCurveMember(
            "cur-1",
            "Member",
            "ifc",
            "native",
            "desc",
            material,
            crossSection,
            storey,
            XmiStructuralCurveMemberTypeEnum.Beam,
            new List<XmiStructuralPointConnection> { beginNode, endNode },
            new List<XmiSegment> { TestModelFactory.CreateSegment() },
            new List<int> { 0 },
            XmiSystemLineEnum.MiddleMiddle,
            beginNode,
            endNode,
            5.0,
            AxisX,
            AxisY,
            AxisZ,
            0,
            0,
            0,
            0,
            0,
            0,
            "Fixed",
            "Pinned");

        Assert.Contains(model.Entities.OfType<XmiStructuralCurveMember>(), e => e.Id == member.Id);
        Assert.Contains(model.Relationships.OfType<XmiHasMaterial>(), r => r.Source == member && r.Target == material);
        Assert.Contains(model.Relationships.OfType<XmiHasCrossSection>(), r => r.Source == member && r.Target == crossSection);
        Assert.Contains(model.Relationships.OfType<XmiHasStorey>(), r => r.Source == member && r.Target == storey);
        Assert.Equal(2, model.Relationships.OfType<XmiHasStructuralPointConnection>().Count());
    }

    /// <summary>
    /// Curve members can be created without a cross-section when upstream data omits it.
    /// </summary>
    [Fact]
    public void CreateStructuralCurveMember_AllowsNullCrossSection()
    {
        var model = new XmiModel();
        var beginNode = TestModelFactory.CreatePointConnection("pc-begin");
        var endNode = TestModelFactory.CreatePointConnection("pc-end");
        model.AddXmiStructuralPointConnection(beginNode);
        model.AddXmiStructuralPointConnection(endNode);

        var member = model.CreateXmiStructuralCurveMember(
            "cur-2",
            "Member no cs",
            "ifc",
            "native-no-cs",
            "desc",
            null,
            null,
            null,
            XmiStructuralCurveMemberTypeEnum.Column,
            new List<XmiStructuralPointConnection> { beginNode, endNode },
            null,
            null,
            XmiSystemLineEnum.MiddleMiddle,
            beginNode,
            endNode,
            4.0,
            AxisX,
            AxisY,
            AxisZ,
            0,
            0,
            0,
            0,
            0,
            0,
            "Fixed",
            "Pinned");

        Assert.Contains(model.Entities.OfType<XmiStructuralCurveMember>(), e => e.Id == member.Id);
        Assert.DoesNotContain(model.Relationships.OfType<XmiHasCrossSection>(), r => r.Source == member);
        Assert.Equal(2, model.Relationships.OfType<XmiHasStructuralPointConnection>().Count());
    }

    /// <summary>
    /// Curve member creation skips cross-section relationships when no section was persisted.
    /// </summary>
    [Fact]
    public void CreateStructuralCurveMember_DoesNotLinkPlaceholderCrossSection()
    {
        var model = new XmiModel();
        var beginNode = TestModelFactory.CreatePointConnection("pc-begin");
        var endNode = TestModelFactory.CreatePointConnection("pc-end");
        model.AddXmiStructuralPointConnection(beginNode);
        model.AddXmiStructuralPointConnection(endNode);

        var placeholderCrossSection = new XmiCrossSection(
            "sec-missing",
            "Missing Section",
            "ifc",
            "",
            "desc",
            XmiShapeEnum.Rectangular,
            new RectangularShapeParameters(0.2, 0.4),
            0.08,
            0.001,
            0.0015,
            0.01,
            0.015,
            0.0003,
            0.00035,
            0.0004,
            0.00045,
            0.0005);

        var member = model.CreateXmiStructuralCurveMember(
            "cur-3",
            "Member no section instance",
            "ifc",
            "native-no-section",
            "desc",
            null,
            placeholderCrossSection,
            null,
            XmiStructuralCurveMemberTypeEnum.Column,
            new List<XmiStructuralPointConnection> { beginNode, endNode },
            null,
            null,
            XmiSystemLineEnum.MiddleMiddle,
            beginNode,
            endNode,
            4.0,
            AxisX,
            AxisY,
            AxisZ,
            0,
            0,
            0,
            0,
            0,
            0,
            "Fixed",
            "Pinned");

        Assert.Contains(model.Entities.OfType<XmiStructuralCurveMember>(), e => e.Id == member.Id);
        Assert.DoesNotContain(model.Relationships.OfType<XmiHasCrossSection>(), r => r.Source == member);
    }

    /// <summary>
    /// Curve member creation skips cross-section relationships when NativeId is null.
    /// </summary>
    [Fact]
    public void CreateStructuralCurveMember_DoesNotLinkCrossSectionWithNullNativeId()
    {
        var model = new XmiModel();
        var beginNode = TestModelFactory.CreatePointConnection("pc-begin");
        var endNode = TestModelFactory.CreatePointConnection("pc-end");
        model.AddXmiStructuralPointConnection(beginNode);
        model.AddXmiStructuralPointConnection(endNode);

        var crossSectionWithNullNativeId = new XmiCrossSection(
            "sec-null-native",
            "Section Null Native",
            "ifc",
            null!,
            "desc",
            XmiShapeEnum.Rectangular,
            new RectangularShapeParameters(0.2, 0.4),
            0.08,
            0.001,
            0.0015,
            0.01,
            0.015,
            0.0003,
            0.00035,
            0.0004,
            0.00045,
            0.0005);

        var member = model.CreateXmiStructuralCurveMember(
            "cur-null-native",
            "Member null native section",
            "ifc",
            "native-null-section",
            "desc",
            null,
            crossSectionWithNullNativeId,
            null,
            XmiStructuralCurveMemberTypeEnum.Beam,
            new List<XmiStructuralPointConnection> { beginNode, endNode },
            null,
            null,
            XmiSystemLineEnum.MiddleMiddle,
            beginNode,
            endNode,
            4.0,
            AxisX,
            AxisY,
            AxisZ,
            0,
            0,
            0,
            0,
            0,
            0,
            "Fixed",
            "Pinned");

        Assert.Contains(model.Entities.OfType<XmiStructuralCurveMember>(), e => e.Id == member.Id);
        Assert.DoesNotContain(model.Relationships.OfType<XmiHasCrossSection>(), r => r.Source == member);
    }

    /// <summary>
    /// Curve member creation reuses existing cross-section by NativeId.
    /// </summary>
    [Fact]
    public void CreateStructuralCurveMember_ReusesCrossSectionByNativeId()
    {
        var model = new XmiModel();
        var existingCrossSection = TestModelFactory.CreateCrossSection();
        model.AddXmiCrossSection(existingCrossSection);
        var beginNode = TestModelFactory.CreatePointConnection("pc-begin");
        var endNode = TestModelFactory.CreatePointConnection("pc-end");
        model.AddXmiStructuralPointConnection(beginNode);
        model.AddXmiStructuralPointConnection(endNode);

        // Create a new cross-section instance with same NativeId
        var duplicateCrossSection = new XmiCrossSection(
            "sec-different-id",
            "Different Section Name",
            "ifc-different",
            existingCrossSection.NativeId,
            "different desc",
            XmiShapeEnum.Rectangular,
            new RectangularShapeParameters(0.5, 0.8),
            0.40,
            0.010,
            0.015,
            0.05,
            0.06,
            0.003,
            0.004,
            0.005,
            0.006,
            0.007);

        var member = model.CreateXmiStructuralCurveMember(
            "cur-reuse-cs",
            "Member reuse cs",
            "ifc",
            "native-reuse-cs",
            "desc",
            null,
            duplicateCrossSection,
            null,
            XmiStructuralCurveMemberTypeEnum.Beam,
            new List<XmiStructuralPointConnection> { beginNode, endNode },
            null,
            null,
            XmiSystemLineEnum.MiddleMiddle,
            beginNode,
            endNode,
            5.0,
            AxisX,
            AxisY,
            AxisZ,
            0,
            0,
            0,
            0,
            0,
            0,
            "Fixed",
            "Pinned");

        Assert.Contains(model.Entities.OfType<XmiStructuralCurveMember>(), e => e.Id == member.Id);
        var csRelation = model.Relationships.OfType<XmiHasCrossSection>().FirstOrDefault(r => r.Source == member);
        Assert.NotNull(csRelation);
        Assert.Equal(existingCrossSection.Id, csRelation.Target.Id);
    }

    /// <summary>
    /// Curve member creation reuses existing material by NativeId.
    /// </summary>
    [Fact]
    public void CreateStructuralCurveMember_ReusesMaterialByNativeId()
    {
        var model = new XmiModel();
        var existingMaterial = TestModelFactory.CreateMaterial();
        model.AddXmiMaterial(existingMaterial);
        var beginNode = TestModelFactory.CreatePointConnection("pc-begin");
        var endNode = TestModelFactory.CreatePointConnection("pc-end");
        model.AddXmiStructuralPointConnection(beginNode);
        model.AddXmiStructuralPointConnection(endNode);

        // Create a new material instance with same NativeId
        var duplicateMaterial = new XmiMaterial(
            "mat-different-id",
            "Different Material Name",
            "ifc-different",
            existingMaterial.NativeId,
            "different desc",
            XmiMaterialTypeEnum.Concrete,
            60,
            25.0,
            "30000",
            "12500",
            "0.2",
            1.0);

        var member = model.CreateXmiStructuralCurveMember(
            "cur-reuse-mat",
            "Member reuse mat",
            "ifc",
            "native-reuse-mat",
            "desc",
            duplicateMaterial,
            null,
            null,
            XmiStructuralCurveMemberTypeEnum.Column,
            new List<XmiStructuralPointConnection> { beginNode, endNode },
            null,
            null,
            XmiSystemLineEnum.MiddleMiddle,
            beginNode,
            endNode,
            3.0,
            AxisX,
            AxisY,
            AxisZ,
            0,
            0,
            0,
            0,
            0,
            0,
            "Fixed",
            "Fixed");

        Assert.Contains(model.Entities.OfType<XmiStructuralCurveMember>(), e => e.Id == member.Id);
        var matRelation = model.Relationships.OfType<XmiHasMaterial>().FirstOrDefault(r => r.Source == member);
        Assert.NotNull(matRelation);
        Assert.Equal(existingMaterial.Id, matRelation.Target.Id);
    }

    /// <summary>
    /// Curve member creation reuses existing storey by NativeId.
    /// </summary>
    [Fact]
    public void CreateStructuralCurveMember_ReusesStoreyByNativeId()
    {
        var model = new XmiModel();
        var existingStorey = TestModelFactory.CreateStorey();
        model.AddXmiStorey(existingStorey);
        var beginNode = TestModelFactory.CreatePointConnection("pc-begin");
        var endNode = TestModelFactory.CreatePointConnection("pc-end");
        model.AddXmiStructuralPointConnection(beginNode);
        model.AddXmiStructuralPointConnection(endNode);

        // Create a new storey instance with same NativeId
        var duplicateStorey = new XmiStorey(
            "storey-different-id",
            "Different Storey Name",
            "ifc-different",
            existingStorey.NativeId,
            "different desc",
            10.0,
            2000.0);

        var member = model.CreateXmiStructuralCurveMember(
            "cur-reuse-storey",
            "Member reuse storey",
            "ifc",
            "native-reuse-storey",
            "desc",
            null,
            null,
            duplicateStorey,
            XmiStructuralCurveMemberTypeEnum.Beam,
            new List<XmiStructuralPointConnection> { beginNode, endNode },
            null,
            null,
            XmiSystemLineEnum.MiddleMiddle,
            beginNode,
            endNode,
            6.0,
            AxisX,
            AxisY,
            AxisZ,
            0,
            0,
            0,
            0,
            0,
            0,
            "Pinned",
            "Pinned");

        Assert.Contains(model.Entities.OfType<XmiStructuralCurveMember>(), e => e.Id == member.Id);
        var storeyRelation = model.Relationships.OfType<XmiHasStorey>().FirstOrDefault(r => r.Source == member);
        Assert.NotNull(storeyRelation);
        Assert.Equal(existingStorey.Id, storeyRelation.Target.Id);
    }

    /// <summary>
    /// Curve member creation throws when ID is null or empty.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void CreateStructuralCurveMember_ThrowsWhenIdIsNullOrEmpty(string? invalidId)
    {
        var model = new XmiModel();
        var beginNode = TestModelFactory.CreatePointConnection("pc-begin");
        var endNode = TestModelFactory.CreatePointConnection("pc-end");

        Assert.Throws<ArgumentException>(() => model.CreateXmiStructuralCurveMember(
            invalidId!,
            "Member",
            "ifc",
            "native",
            "desc",
            null,
            null,
            null,
            XmiStructuralCurveMemberTypeEnum.Beam,
            new List<XmiStructuralPointConnection> { beginNode, endNode },
            null,
            null,
            XmiSystemLineEnum.MiddleMiddle,
            beginNode,
            endNode,
            5.0,
            AxisX,
            AxisY,
            AxisZ,
            0,
            0,
            0,
            0,
            0,
            0,
            "Fixed",
            "Pinned"));
    }

    /// <summary>
    /// Curve member creation throws when name is null or empty.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void CreateStructuralCurveMember_ThrowsWhenNameIsNullOrEmpty(string? invalidName)
    {
        var model = new XmiModel();
        var beginNode = TestModelFactory.CreatePointConnection("pc-begin");
        var endNode = TestModelFactory.CreatePointConnection("pc-end");

        Assert.Throws<ArgumentException>(() => model.CreateXmiStructuralCurveMember(
            "cur-valid-id",
            invalidName!,
            "ifc",
            "native",
            "desc",
            null,
            null,
            null,
            XmiStructuralCurveMemberTypeEnum.Beam,
            new List<XmiStructuralPointConnection> { beginNode, endNode },
            null,
            null,
            XmiSystemLineEnum.MiddleMiddle,
            beginNode,
            endNode,
            5.0,
            AxisX,
            AxisY,
            AxisZ,
            0,
            0,
            0,
            0,
            0,
            0,
            "Fixed",
            "Pinned"));
    }

    /// <summary>
    /// Creating a surface member supports optional materials.
    /// </summary>
    [Fact]
    public void CreateStructuralSurfaceMember_AllowsNullMaterial()
    {
        var model = new XmiModel();
        var storey = TestModelFactory.CreateStorey();
        var begin = TestModelFactory.CreatePointConnection("pc-a");
        var end = TestModelFactory.CreatePointConnection("pc-b");
        model.AddXmiStorey(storey);
        model.AddXmiStructuralPointConnection(begin);
        model.AddXmiStructuralPointConnection(end);

        var member = model.CreateXmiStructuralSurfaceMember(
            "surf-no-mat",
            "Surface without material",
            "ifc",
            "native",
            "desc",
            null,
            XmiStructuralSurfaceMemberTypeEnum.Slab,
            0.2,
            XmiStructuralSurfaceMemberSystemPlaneEnum.Middle,
            new List<XmiStructuralPointConnection> { begin, end },
            storey,
            new List<XmiSegment>(),
            new List<int>(),
            12.5,
            0.0,
            AxisX,
            AxisY,
            AxisZ,
            0.3);

        Assert.Contains(model.Entities.OfType<XmiStructuralSurfaceMember>(), e => e.Id == member.Id);
        Assert.DoesNotContain(model.Relationships.OfType<XmiHasMaterial>(), r => r.Source == member);
    }

    /// <summary>
    /// Beam factory links material when supplied.
    /// </summary>
    [Fact]
    public void CreateBeam_AddsMaterialRelationshipWhenProvided()
    {
        var model = new XmiModel();
        var material = TestModelFactory.CreateMaterial();
        model.AddXmiMaterial(material);

        var beam = model.CreateXmiBeam(
            "beam-create",
            "Beam create",
            "ifc",
            "native",
            "desc",
            material,
            null,
            null,
            XmiSystemLineEnum.MiddleMiddle,
            5.0,
            AxisX,
            AxisY,
            AxisZ,
            0,
            0,
            0,
            0,
            0,
            0);

        Assert.Contains(model.Entities.OfType<XmiBeam>(), e => e.Id == beam.Id);
        Assert.Contains(model.Relationships.OfType<XmiHasMaterial>(), r => r.Source == beam && r.Target == material);
    }

    /// <summary>
    /// Column/slab/wall factories tolerate missing material.
    /// </summary>
    [Fact]
    public void CreatePhysicalElements_AllowNullMaterial()
    {
        var model = new XmiModel();

        var column = model.CreateXmiColumn(
            "col-create",
            "Column create",
            "ifc",
            "native",
            "desc",
            null,
            null,
            null,
            XmiSystemLineEnum.MiddleMiddle,
            3.0,
            AxisX,
            AxisY,
            AxisZ,
            0,
            0,
            0,
            0,
            0,
            0);

        var slab = model.CreateXmiSlab(
            "slab-create",
            "Slab create",
            "ifc",
            "native",
            "desc",
            null,
            null,
            null,
            0,
            AxisX,
            AxisY,
            AxisZ,
            0.25);

        var wall = model.CreateXmiWall(
            "wall-create",
            "Wall create",
            "ifc",
            "native",
            "desc",
            null,
            null,
            null,
            0,
            AxisX,
            AxisY,
            AxisZ,
            3.0);

        Assert.Contains(model.Entities.OfType<XmiColumn>(), e => e.Id == column.Id);
        Assert.Contains(model.Entities.OfType<XmiSlab>(), e => e.Id == slab.Id);
        Assert.Contains(model.Entities.OfType<XmiWall>(), e => e.Id == wall.Id);
        Assert.DoesNotContain(model.Relationships.OfType<XmiHasMaterial>(), r => r.Source == column || r.Source == slab || r.Source == wall);
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
            new List<int> { 0 },
            12.5,
            0,
            AxisX,
            AxisY,
            AxisZ,
            0.3);

        Assert.Contains(model.Entities.OfType<XmiStructuralSurfaceMember>(), e => e.Id == member.Id);
        Assert.Contains(model.Relationships.OfType<XmiHasStorey>(), r => r.Source == member && r.Target.NativeId == storey.NativeId);
        Assert.Contains(model.Relationships.OfType<XmiHasMaterial>(), r => r.Source == member && r.Target.NativeId == material.NativeId);
    }

    /// <summary>
    /// Beam factory creates segment relationships when segments are provided.
    /// </summary>
    [Fact]
    public void CreateBeam_WithSegments_AddsSegmentRelationships()
    {
        var model = new XmiModel();
        var segment1 = TestModelFactory.CreateSegment("seg-beam-1");
        var segment2 = TestModelFactory.CreateSegment("seg-beam-2");
        var segments = new List<XmiSegment> { segment1, segment2 };

        var beam = model.CreateXmiBeam(
            "beam-seg",
            "Beam with segments",
            "ifc",
            "native-beam-seg",
            "desc",
            null,
            segments,
            new List<int> { 0, 1 },
            XmiSystemLineEnum.MiddleMiddle,
            5.0,
            AxisX,
            AxisY,
            AxisZ,
            0,
            0,
            0,
            0,
            0,
            0);

        Assert.Contains(model.Entities.OfType<XmiBeam>(), e => e.Id == beam.Id);
        var segmentRelationships = model.Relationships.OfType<XmiHasSegment>().Where(r => r.Source == beam).ToList();
        Assert.Equal(2, segmentRelationships.Count);
    }

    /// <summary>
    /// Column factory creates segment relationships when segments are provided.
    /// </summary>
    [Fact]
    public void CreateColumn_WithSegments_AddsSegmentRelationships()
    {
        var model = new XmiModel();
        var segment = TestModelFactory.CreateSegment("seg-col");
        var segments = new List<XmiSegment> { segment };

        var column = model.CreateXmiColumn(
            "col-seg",
            "Column with segments",
            "ifc",
            "native-col-seg",
            "desc",
            null,
            segments,
            new List<int> { 0 },
            XmiSystemLineEnum.MiddleMiddle,
            3.0,
            AxisX,
            AxisY,
            AxisZ,
            0,
            0,
            0,
            0,
            0,
            0);

        Assert.Contains(model.Entities.OfType<XmiColumn>(), e => e.Id == column.Id);
        var segmentRelationships = model.Relationships.OfType<XmiHasSegment>().Where(r => r.Source == column).ToList();
        Assert.Single(segmentRelationships);
    }

    /// <summary>
    /// Slab factory wires segment relationships and records thickness metadata.
    /// </summary>
    [Fact]
    public void CreateSlab_WithSegments_AddsSegmentRelationships()
    {
        var model = new XmiModel();
        var segment1 = TestModelFactory.CreateSegment("seg-slab-1");
        var segment2 = TestModelFactory.CreateSegment("seg-slab-2");
        var segments = new List<XmiSegment> { segment1, segment2 };

        var slab = model.CreateXmiSlab(
            "slab-seg",
            "Slab with segments",
            "ifc",
            "native-slab-seg",
            "desc",
            null,
            segments,
            new List<int> { 0, 1 },
            0.1,
            AxisX,
            AxisY,
            AxisZ,
            0.35);

        Assert.Contains(model.Entities.OfType<XmiSlab>(), e => e.Id == slab.Id);
        var segmentRelationships = model.Relationships.OfType<XmiHasSegment>().Where(r => r.Source == slab).ToList();
        Assert.Equal(2, segmentRelationships.Count);
        Assert.Equal(0.35, slab.Thickness);
    }

    /// <summary>
    /// Wall factory wires segment relationships and records height metadata.
    /// </summary>
    [Fact]
    public void CreateWall_WithSegments_AddsSegmentRelationships()
    {
        var model = new XmiModel();
        var segment1 = TestModelFactory.CreateSegment("seg-wall-1");
        var segment2 = TestModelFactory.CreateSegment("seg-wall-2");
        var segments = new List<XmiSegment> { segment1, segment2 };

        var wall = model.CreateXmiWall(
            "wall-seg",
            "Wall with segments",
            "ifc",
            "native-wall-seg",
            "desc",
            null,
            segments,
            new List<int> { 0, 1 },
            0.2,
            AxisX,
            AxisY,
            AxisZ,
            4.2);

        Assert.Contains(model.Entities.OfType<XmiWall>(), e => e.Id == wall.Id);
        var segmentRelationships = model.Relationships.OfType<XmiHasSegment>().Where(r => r.Source == wall).ToList();
        Assert.Equal(2, segmentRelationships.Count);
        Assert.Equal(4.2, wall.Height);
    }

    /// <summary>
    /// Curve member factory creates segment relationships when segments are provided.
    /// </summary>
    [Fact]
    public void CreateStructuralCurveMember_WithSegments_AddsSegmentRelationships()
    {
        var model = new XmiModel();
        var beginNode = TestModelFactory.CreatePointConnection("pc-begin");
        var endNode = TestModelFactory.CreatePointConnection("pc-end");
        var segment1 = TestModelFactory.CreateSegment("seg-curve-1");
        var segment2 = TestModelFactory.CreateSegment("seg-curve-2");
        model.AddXmiStructuralPointConnection(beginNode);
        model.AddXmiStructuralPointConnection(endNode);

        var member = model.CreateXmiStructuralCurveMember(
            "cur-seg",
            "Curve member with segments",
            "ifc",
            "native-cur-seg",
            "desc",
            null,
            null,
            null,
            XmiStructuralCurveMemberTypeEnum.Beam,
            new List<XmiStructuralPointConnection> { beginNode, endNode },
            new List<XmiSegment> { segment1, segment2 },
            new List<int> { 0, 1 },
            XmiSystemLineEnum.MiddleMiddle,
            beginNode,
            endNode,
            5.0,
            AxisX,
            AxisY,
            AxisZ,
            0,
            0,
            0,
            0,
            0,
            0,
            "Fixed",
            "Pinned");

        Assert.Contains(model.Entities.OfType<XmiStructuralCurveMember>(), e => e.Id == member.Id);
        var segmentRelationships = model.Relationships.OfType<XmiHasSegment>().Where(r => r.Source == member).ToList();
        Assert.Equal(2, segmentRelationships.Count);
    }

    /// <summary>
    /// Surface member factory creates segment relationships when segments are provided.
    /// </summary>
    [Fact]
    public void CreateStructuralSurfaceMember_WithSegments_AddsSegmentRelationships()
    {
        var model = new XmiModel();
        var segment1 = TestModelFactory.CreateSegment("seg-surf-1");
        var segment2 = TestModelFactory.CreateSegment("seg-surf-2");
        var segment3 = TestModelFactory.CreateSegment("seg-surf-3");

        var member = model.CreateXmiStructuralSurfaceMember(
            "surf-seg",
            "Surface with segments",
            "ifc",
            "native-surf-seg",
            "desc",
            null,
            XmiStructuralSurfaceMemberTypeEnum.Slab,
            0.2,
            XmiStructuralSurfaceMemberSystemPlaneEnum.Middle,
            new List<XmiStructuralPointConnection> { TestModelFactory.CreatePointConnection() },
            null,
            new List<XmiSegment> { segment1, segment2, segment3 },
            new List<int> { 0, 1, 2 },
            12.5,
            0,
            AxisX,
            AxisY,
            AxisZ,
            0.3);

        Assert.Contains(model.Entities.OfType<XmiStructuralSurfaceMember>(), e => e.Id == member.Id);
        var segmentRelationships = model.Relationships.OfType<XmiHasSegment>().Where(r => r.Source == member).ToList();
        Assert.Equal(3, segmentRelationships.Count);
    }

    /// <summary>
    /// Segment relationships reuse existing segments by NativeId.
    /// </summary>
    [Fact]
    public void CreateBeam_ReusesExistingSegmentByNativeId()
    {
        var model = new XmiModel();
        var existingSegment = TestModelFactory.CreateSegment("seg-existing");
        model.Entities.Add(existingSegment);

        // Create a new segment with same NativeId but different Id
        var duplicateSegment = new XmiSegment(
            "seg-different-id",
            "Different Segment Name",
            "ifc-different",
            existingSegment.NativeId,
            "different desc",
            XmiSegmentTypeEnum.Line);

        var beam = model.CreateXmiBeam(
            "beam-reuse-seg",
            "Beam reuse segment",
            "ifc",
            "native-beam-reuse",
            "desc",
            null,
            new List<XmiSegment> { duplicateSegment },
            new List<int> { 0 },
            XmiSystemLineEnum.MiddleMiddle,
            5.0,
            AxisX,
            AxisY,
            AxisZ,
            0,
            0,
            0,
            0,
            0,
            0);

        var segmentRelationship = model.Relationships.OfType<XmiHasSegment>().FirstOrDefault(r => r.Source == beam);
        Assert.NotNull(segmentRelationship);
        Assert.Equal(existingSegment.Id, segmentRelationship.Target.Id);
    }

    /// <summary>
    /// CreateXmiLineSegment creates segment with geometry and point relationships.
    /// </summary>
    [Fact]
    public void CreateLineSegment_CreatesGeometryAndPointRelationships()
    {
        var model = new XmiModel();
        var startPoint = new XmiPoint3d("pt-start", "Start", "ifc", "native-start", "desc", 0, 0, 0);
        var endPoint = new XmiPoint3d("pt-end", "End", "ifc", "native-end", "desc", 5, 0, 0);
        var line = model.CreateXmiLine3d("line-seg-1", "Line", "ifc-line", "native-line", "desc-line", startPoint, endPoint);

        var segment = model.CreateXmiLineSegment(
            "seg-line-1",
            "Line Segment",
            "ifc",
            "native-seg-line",
            "desc",
            line);

        // Verify segment was created
        Assert.Contains(model.Entities.OfType<XmiSegment>(), e => e.Id == segment.Id);
        Assert.Equal(XmiSegmentTypeEnum.Line, segment.SegmentType);

        // Verify line geometry was created
        var storedLine = model.Entities.OfType<XmiLine3d>().FirstOrDefault();
        Assert.NotNull(storedLine);

        // Verify segment -> line relationship
        var lineRelation = model.Relationships.OfType<XmiHasLine3d>().FirstOrDefault(r => r.Source == segment);
        Assert.NotNull(lineRelation);
        Assert.Equal(storedLine, lineRelation.Target);
    }

    /// <summary>
    /// CreateXmiLineSegment sets correct point types on relationships.
    /// </summary>
    [Fact]
    public void CreateLineSegment_SetsCorrectPointTypes()
    {
        var model = new XmiModel();
        var startPoint = new XmiPoint3d("pt-start", "Start", "ifc", "native-start", "desc", 0, 0, 0);
        var endPoint = new XmiPoint3d("pt-end", "End", "ifc", "native-end", "desc", 5, 0, 0);
        var line = model.CreateXmiLine3d("line-pt", "Line PT", "ifc-line", "native-line-pt", "desc-line", startPoint, endPoint);

        var segment = model.CreateXmiLineSegment(
            "seg-line-pt",
            "Line Segment PointType",
            "ifc",
            "native-seg-line-pt",
            "desc",
            line);

        line = model.Entities.OfType<XmiLine3d>().FirstOrDefault();
        Assert.NotNull(line);

        // Verify segment -> line relationship exists
        var lineRelation = model.Relationships.OfType<XmiHasLine3d>().FirstOrDefault(r => r.Source == segment);
        Assert.NotNull(lineRelation);
        Assert.Equal(line, lineRelation.Target);
    }

    /// <summary>
    /// CreateXmiArcSegment creates segment with geometry and point relationships.
    /// </summary>
    [Fact]
    public void CreateArcSegment_CreatesGeometryAndPointRelationships()
    {
        var model = new XmiModel();
        var startPoint = new XmiPoint3d("pt-arc-start", "Start", "ifc", "native-arc-start", "desc", 0, 0, 0);
        var endPoint = new XmiPoint3d("pt-arc-end", "End", "ifc", "native-arc-end", "desc", 10, 0, 0);
        var centerPoint = new XmiPoint3d("pt-arc-center", "Center", "ifc", "native-arc-center", "desc", 5, 5, 0);
        var arc = model.CreateXmiArc3d("arc-seg-1", "Arc", "ifc-arc", "native-arc", "desc-arc", startPoint, endPoint, centerPoint, 5.0f);

        var segment = model.CreateXmiArcSegment(
            "seg-arc-1",
            "Arc Segment",
            "ifc",
            "native-seg-arc",
            "desc",
            arc);

        // Verify segment was created
        Assert.Contains(model.Entities.OfType<XmiSegment>(), e => e.Id == segment.Id);
        Assert.Equal(XmiSegmentTypeEnum.CircularArc, segment.SegmentType);

        // Verify arc geometry was created with radius property
        var storedArc = model.Entities.OfType<XmiArc3d>().FirstOrDefault();
        Assert.NotNull(storedArc);
        Assert.Equal(5.0f, storedArc.Radius);

        // Verify segment -> arc relationship
        var arcRelation = model.Relationships.OfType<XmiHasArc3d>().FirstOrDefault(r => r.Source == segment);
        Assert.NotNull(arcRelation);
        Assert.Equal(storedArc, arcRelation.Target);
    }

    /// <summary>
    /// CreateXmiLineSegment reuses existing points by coordinates.
    /// </summary>
    [Fact]
    public void CreateLineSegment_ReusesExistingPointsByCoordinates()
    {
        var model = new XmiModel();
        var existingPoint = new XmiPoint3d("pt-existing", "Existing", "ifc", "native-existing", "desc", 0, 0, 0);
        model.AddXmiPoint3d(existingPoint);

        // Create a new line with a start point at identical coordinates
        var duplicateStartPoint = new XmiPoint3d("pt-duplicate", "Duplicate", "ifc", "native-dup", "desc", 0, 0, 0);
        var endPoint = new XmiPoint3d("pt-end-new", "End New", "ifc", "native-end-new", "desc", 10, 0, 0);
        var line = model.CreateXmiLine3d("line-reuse", "Line reuse", "ifc-line", "native-line-reuse", "desc-line", duplicateStartPoint, endPoint);

        var segment = model.CreateXmiLineSegment(
            "seg-line-reuse",
            "Line Segment Reuse",
            "ifc",
            "native-seg-line-reuse",
            "desc",
            line);

        // Verify line uses the existing point (by coordinates)
        line = model.Entities.OfType<XmiLine3d>().FirstOrDefault();
        Assert.NotNull(line);
        Assert.Equal(existingPoint, line.StartPoint);
    }

    /// <summary>
    /// CreateXmiLineSegment throws when ID is null or empty.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void CreateLineSegment_ThrowsWhenIdIsNullOrEmpty(string? invalidId)
    {
        var model = new XmiModel();
        var startPoint = new XmiPoint3d("pt-start", "Start", "ifc", "native-start", "desc", 0, 0, 0);
        var endPoint = new XmiPoint3d("pt-end", "End", "ifc", "native-end", "desc", 5, 0, 0);
        var line = model.CreateXmiLine3d("line-throw", "Line throw", "ifc-line", "native-line-throw", "desc-line", startPoint, endPoint);

        Assert.Throws<ArgumentException>(() => model.CreateXmiLineSegment(
            invalidId!,
            "Line Segment",
            "ifc",
            "native",
            "desc",
            line));
    }

    /// <summary>
    /// CreateXmiArcSegment throws when name is null or empty.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void CreateArcSegment_ThrowsWhenNameIsNullOrEmpty(string? invalidName)
    {
        var model = new XmiModel();
        var startPoint = new XmiPoint3d("pt-start", "Start", "ifc", "native-start", "desc", 0, 0, 0);
        var endPoint = new XmiPoint3d("pt-end", "End", "ifc", "native-end", "desc", 10, 0, 0);
        var centerPoint = new XmiPoint3d("pt-center", "Center", "ifc", "native-center", "desc", 5, 5, 0);
        var arc = model.CreateXmiArc3d("arc-throw", "Arc Throw", "ifc-arc", "native-arc-throw", "desc-arc", startPoint, endPoint, centerPoint, 5.0f);

        Assert.Throws<ArgumentException>(() => model.CreateXmiArcSegment(
            "seg-arc",
            invalidName!,
            "ifc",
            "native",
            "desc",
            arc));
    }

    #region FindCoincidentLines

    /// <summary>
    /// FindCoincidentLines returns lines that occupy the same space.
    /// </summary>
    [Fact]
    public void FindCoincidentLines_ReturnsCoincidentLines()
    {
        var model = new XmiModel();
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("s1", 0, 0, 0),
            TestModelFactory.CreatePoint("e1", 10, 10, 10));

        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("s2", 0, 0, 0),
            TestModelFactory.CreatePoint("e2", 10, 10, 10));

        var line3 = new XmiLine3d("line-3", "Line 3", "ifc-3", "N3", "desc",
            TestModelFactory.CreatePoint("s3", 5, 5, 5),
            TestModelFactory.CreatePoint("e3", 15, 15, 15));

        model.AddXmiLine3d(line1);
        model.AddXmiLine3d(line2);
        model.AddXmiLine3d(line3);

        var coincident = model.FindCoincidentLines(line1).ToList();

        Assert.Single(coincident);
        Assert.Contains(line2, coincident);
        Assert.DoesNotContain(line3, coincident);
    }

    /// <summary>
    /// FindCoincidentLines includes opposite direction lines.
    /// </summary>
    [Fact]
    public void FindCoincidentLines_IncludesOppositeDirectionLines()
    {
        var model = new XmiModel();
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("s1", 0, 0, 0),
            TestModelFactory.CreatePoint("e1", 10, 10, 10));

        var lineReversed = new XmiLine3d("line-rev", "Line Reversed", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("s2", 10, 10, 10),
            TestModelFactory.CreatePoint("e2", 0, 0, 0));

        model.AddXmiLine3d(line1);
        model.AddXmiLine3d(lineReversed);

        var coincident = model.FindCoincidentLines(line1).ToList();

        Assert.Single(coincident);
        Assert.Contains(lineReversed, coincident);
    }

    /// <summary>
    /// FindCoincidentLines returns empty when no matches.
    /// </summary>
    [Fact]
    public void FindCoincidentLines_ReturnsEmptyWhenNoMatches()
    {
        var model = new XmiModel();
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("s1", 0, 0, 0),
            TestModelFactory.CreatePoint("e1", 10, 10, 10));

        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("s2", 100, 100, 100),
            TestModelFactory.CreatePoint("e2", 200, 200, 200));

        model.AddXmiLine3d(line1);
        model.AddXmiLine3d(line2);

        var coincident = model.FindCoincidentLines(line1).ToList();

        Assert.Empty(coincident);
    }

    /// <summary>
    /// FindCoincidentLines returns empty for null input.
    /// </summary>
    [Fact]
    public void FindCoincidentLines_ReturnsEmptyForNull()
    {
        var model = new XmiModel();

        var coincident = model.FindCoincidentLines(null!).ToList();

        Assert.Empty(coincident);
    }

    #endregion

    #region FindDirectionallyEqualLines

    /// <summary>
    /// FindDirectionallyEqualLines returns lines with same direction only.
    /// </summary>
    [Fact]
    public void FindDirectionallyEqualLines_ReturnsSameDirectionOnly()
    {
        var model = new XmiModel();
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("s1", 0, 0, 0),
            TestModelFactory.CreatePoint("e1", 10, 10, 10));

        var lineSameDir = new XmiLine3d("line-same", "Line Same", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("s2", 0, 0, 0),
            TestModelFactory.CreatePoint("e2", 10, 10, 10));

        var lineReversed = new XmiLine3d("line-rev", "Line Reversed", "ifc-3", "N3", "desc",
            TestModelFactory.CreatePoint("s3", 10, 10, 10),
            TestModelFactory.CreatePoint("e3", 0, 0, 0));

        model.AddXmiLine3d(line1);
        model.AddXmiLine3d(lineSameDir);
        model.AddXmiLine3d(lineReversed);

        var directionallyEqual = model.FindDirectionallyEqualLines(line1).ToList();

        Assert.Single(directionallyEqual);
        Assert.Contains(lineSameDir, directionallyEqual);
        Assert.DoesNotContain(lineReversed, directionallyEqual);
    }

    #endregion

    #region FindCoincidentLineRelationships

    /// <summary>
    /// FindCoincidentLineRelationships returns relationships with coincident targets.
    /// </summary>
    [Fact]
    public void FindCoincidentLineRelationships_ReturnsRelationshipsWithCoincidentTargets()
    {
        var model = new XmiModel();
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("s1", 0, 0, 0),
            TestModelFactory.CreatePoint("e1", 10, 10, 10));

        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("s2", 0, 0, 0),
            TestModelFactory.CreatePoint("e2", 10, 10, 10));

        var line3 = new XmiLine3d("line-3", "Line 3", "ifc-3", "N3", "desc",
            TestModelFactory.CreatePoint("s3", 100, 100, 100),
            TestModelFactory.CreatePoint("e3", 200, 200, 200));

        var segment1 = TestModelFactory.CreateSegment("seg-1");
        var segment2 = TestModelFactory.CreateSegment("seg-2");
        var segment3 = TestModelFactory.CreateSegment("seg-3");

        model.AddXmiLine3d(line1);
        model.AddXmiLine3d(line2);
        model.AddXmiLine3d(line3);
        model.AddXmiSegment(segment1);
        model.AddXmiSegment(segment2);
        model.AddXmiSegment(segment3);

        var rel1 = new XmiHasLine3d(segment1, line1);
        var rel2 = new XmiHasLine3d(segment2, line2);
        var rel3 = new XmiHasLine3d(segment3, line3);
        model.AddXmiHasLine3d(rel1);
        model.AddXmiHasLine3d(rel2);
        model.AddXmiHasLine3d(rel3);

        var coincidentRels = model.FindCoincidentLineRelationships(line1).ToList();

        Assert.Equal(2, coincidentRels.Count);
        Assert.Contains(rel1, coincidentRels);
        Assert.Contains(rel2, coincidentRels);
        Assert.DoesNotContain(rel3, coincidentRels);
    }

    #endregion

    #region FindDirectionallyEqualLineRelationships

    /// <summary>
    /// FindDirectionallyEqualLineRelationships returns relationships with same direction targets.
    /// </summary>
    [Fact]
    public void FindDirectionallyEqualLineRelationships_ReturnsSameDirectionTargets()
    {
        var model = new XmiModel();
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("s1", 0, 0, 0),
            TestModelFactory.CreatePoint("e1", 10, 10, 10));

        var lineSameDir = new XmiLine3d("line-same", "Line Same", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("s2", 0, 0, 0),
            TestModelFactory.CreatePoint("e2", 10, 10, 10));

        var lineReversed = new XmiLine3d("line-rev", "Line Reversed", "ifc-3", "N3", "desc",
            TestModelFactory.CreatePoint("s3", 10, 10, 10),
            TestModelFactory.CreatePoint("e3", 0, 0, 0));

        var segment1 = TestModelFactory.CreateSegment("seg-1");
        var segment2 = TestModelFactory.CreateSegment("seg-2");
        var segment3 = TestModelFactory.CreateSegment("seg-3");

        model.AddXmiLine3d(line1);
        model.AddXmiLine3d(lineSameDir);
        model.AddXmiLine3d(lineReversed);
        model.AddXmiSegment(segment1);
        model.AddXmiSegment(segment2);
        model.AddXmiSegment(segment3);

        var rel1 = new XmiHasLine3d(segment1, line1);
        var rel2 = new XmiHasLine3d(segment2, lineSameDir);
        var rel3 = new XmiHasLine3d(segment3, lineReversed);
        model.AddXmiHasLine3d(rel1);
        model.AddXmiHasLine3d(rel2);
        model.AddXmiHasLine3d(rel3);

        var directionalRels = model.FindDirectionallyEqualLineRelationships(line1).ToList();

        Assert.Equal(2, directionalRels.Count);
        Assert.Contains(rel1, directionalRels);
        Assert.Contains(rel2, directionalRels);
        Assert.DoesNotContain(rel3, directionalRels);
    }

    #endregion
}
