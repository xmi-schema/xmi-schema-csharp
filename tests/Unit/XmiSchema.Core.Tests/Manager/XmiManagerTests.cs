using System;
using System.IO;
using System.Linq;
using XmiSchema.Core.Entities;
using XmiSchema.Core.Manager;
using XmiSchema.Core.Models;
using XmiSchema.Core.Relationships;
using XmiSchema.Core.Models.Entities.Physical;

namespace XmiSchema.Core.Tests.Manager;

/// <summary>
/// Exercises the orchestration helpers on <see cref="XmiManager"/>.
/// </summary>
public class XmiManagerTests
{
    /// <summary>
    /// Ensures add helpers delegate to the underlying model list.
    /// </summary>
    [Fact]
    public void AddEntityHelpers_InsertIntoTargetModel()
    {
        var manager = TestModelFactory.CreateManagerWithModel();
        var material = TestModelFactory.CreateMaterial("mat-new");

        manager.AddXmiMaterialToModel(0, material);

        Assert.Contains(manager.Models[0].Entities.OfType<XmiMaterial>(), m => m.Id == "mat-new");
    }

    /// <summary>
    /// Physical element add helpers mirror XmiModel methods and populate the destination model.
    /// </summary>
    [Fact]
    public void AddPhysicalEntities_InsertIntoTargetModel()
    {
        var manager = TestModelFactory.CreateManagerWithModel();

        var beam = TestModelFactory.CreateBeam("beam-new");
        var column = TestModelFactory.CreateColumn("col-new");
        var slab = TestModelFactory.CreateSlab("slab-new");
        var wall = TestModelFactory.CreateWall("wall-new");

        manager.AddXmiBeamToModel(0, beam);
        manager.AddXmiColumnToModel(0, column);
        manager.AddXmiSlabToModel(0, slab);
        manager.AddXmiWallToModel(0, wall);

        Assert.Contains(manager.Models[0].Entities.OfType<XmiBeam>(), e => e.Id == "beam-new");
        Assert.Contains(manager.Models[0].Entities.OfType<XmiColumn>(), e => e.Id == "col-new");
        Assert.Contains(manager.Models[0].Entities.OfType<XmiSlab>(), e => e.Id == "slab-new");
        Assert.Contains(manager.Models[0].Entities.OfType<XmiWall>(), e => e.Id == "wall-new");
    }

    /// <summary>
    /// Confirms entity lookup works for identifiers present inside the model.
    /// </summary>
    [Fact]
    public void GetEntityById_ReturnsMatchingEntity()
    {
        var manager = TestModelFactory.CreateManagerWithModel();
        var storey = TestModelFactory.CreateStorey("storey-lookup");
        manager.AddXmiStoreyToModel(0, storey);

        var result = manager.GetXmiEntityById<XmiStorey>(0, storey.Id);

        Assert.NotNull(result);
        Assert.Equal(storey.Id, result!.Id);
    }

    /// <summary>
    /// The point matching helper returns the alternate connection referencing the same coordinates.
    /// </summary>
    [Fact]
    public void FindMatchingPointConnectionByPoint3D_ReturnsOtherConnectionId()
    {
        var manager = new XmiManager();
        var model = new XmiModel();
        manager.Models.Add(model);

        var point = TestModelFactory.CreatePoint();
        var first = TestModelFactory.CreatePointConnection("pc-first");
        var second = TestModelFactory.CreatePointConnection("pc-second");
        model.AddXmiPoint3D(point);
        model.AddXmiStructuralPointConnection(first);
        model.AddXmiStructuralPointConnection(second);
        model.AddXmiHasPoint3D(new XmiHasPoint3d(first, point));
        model.AddXmiHasPoint3D(new XmiHasPoint3d(second, point));

        var match = manager.FindMatchingPointConnectionByPoint3D(0, first);

        Assert.Equal(second.Id, match);
    }

    /// <summary>
    /// Alias method mirrors XmiModel signature and delegates to the existing lookup.
    /// </summary>
    [Fact]
    public void FindMatchingXmiStructuralPointConnectionByPoint3D_DelegatesToPrimaryLookup()
    {
        var manager = new XmiManager();
        var model = new XmiModel();
        manager.Models.Add(model);

        var point = TestModelFactory.CreatePoint();
        var first = TestModelFactory.CreatePointConnection("pc-first");
        var second = TestModelFactory.CreatePointConnection("pc-second");
        model.AddXmiPoint3D(point);
        model.AddXmiStructuralPointConnection(first);
        model.AddXmiStructuralPointConnection(second);
        model.AddXmiHasPoint3D(new XmiHasPoint3d(first, point));
        model.AddXmiHasPoint3D(new XmiHasPoint3d(second, point));

        var match = manager.FindMatchingXmiStructuralPointConnectionByPoint3D(0, first);

        Assert.Equal(second.Id, match);
    }

    /// <summary>
    /// Serializing via <see cref="XmiManager.BuildJson"/> yields the nodes/edges payload.
    /// </summary>
    [Fact]
    public void BuildJson_ReturnsGraphShape()
    {
        var manager = TestModelFactory.CreateManagerWithModel();
        var storey = TestModelFactory.CreateStorey();
        var connection = TestModelFactory.CreatePointConnection();
        manager.AddXmiStoreyToModel(0, storey);
        manager.AddXmiStructuralPointConnectionToModel(0, connection);
        manager.AddXmiHasStoreyToModel(0, new XmiHasStorey(connection, storey));

        var json = manager.BuildJson(0);

        Assert.Contains("\"nodes\"", json);
        Assert.Contains(connection.Id, json);
        Assert.Contains(storey.Id, json);
    }

    /// <summary>
    /// Relationship helper mirrors XmiModel for structural curve member bindings.
    /// </summary>
    [Fact]
    public void AddStructuralCurveMemberRelationship_InsertIntoTargetModel()
    {
        var manager = TestModelFactory.CreateManagerWithModel();
        var beam = TestModelFactory.CreateBeam("beam-rel");
        var curve = TestModelFactory.CreateCurveMember("cur-rel");
        manager.AddXmiBeamToModel(0, beam);
        manager.AddXmiStructuralCurveMemberToModel(0, curve);

        var relation = new XmiHasStructuralCurveMember(beam, curve);
        manager.AddXmiHasStructuralCurveMemberToModel(0, relation);

        Assert.Contains(manager.Models[0].Relationships.OfType<XmiHasStructuralCurveMember>(), r => r.Source == beam && r.Target == curve);
    }

    /// <summary>
    /// Save writes the JSON graph to the requested path.
    /// </summary>
    [Fact]
    public void Save_WritesFile()
    {
        var manager = TestModelFactory.CreateManagerWithModel();
        var tempFile = Path.Combine(Path.GetTempPath(), $"xmi-schema-{Guid.NewGuid():N}.json");

        manager.Save(tempFile);

        Assert.True(File.Exists(tempFile));
        File.Delete(tempFile);
    }
}
