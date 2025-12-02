using System;
using System.IO;
using System.Linq;
using XmiSchema.Core.Entities;
using XmiSchema.Core.Manager;
using XmiSchema.Core.Models;
using XmiSchema.Core.Relationships;

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

        manager.AddXmiStructuralMaterialToModel(0, material);

        Assert.Contains(manager.Models[0].Entities.OfType<XmiStructuralMaterial>(), m => m.Id == "mat-new");
    }

    /// <summary>
    /// Confirms entity lookup works for identifiers present inside the model.
    /// </summary>
    [Fact]
    public void GetEntityById_ReturnsMatchingEntity()
    {
        var manager = TestModelFactory.CreateManagerWithModel();
        var storey = TestModelFactory.CreateStorey("storey-lookup");
        manager.AddXmiStructuralStoreyToModel(0, storey);

        var result = manager.GetEntityById<XmiStructuralStorey>(0, storey.Id);

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
        model.AddXmiHasPoint3D(new XmiHasPoint3D(first, point));
        model.AddXmiHasPoint3D(new XmiHasPoint3D(second, point));

        var match = manager.FindMatchingPointConnectionByPoint3D(0, first);

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
        manager.AddXmiStructuralStoreyToModel(0, storey);
        manager.AddXmiStructuralPointConnectionToModel(0, connection);
        manager.AddXmiHasStoreyToModel(0, new XmiHasStructuralStorey(connection, storey));

        var json = manager.BuildJson(0);

        Assert.Contains("\"nodes\"", json);
        Assert.Contains(connection.Id, json);
        Assert.Contains(storey.Id, json);
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
