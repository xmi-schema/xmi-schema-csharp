using XmiSchema.Entities.Physical;
using XmiSchema.Entities.Geometries;
using XmiSchema.Entities.Commons;
using XmiSchema.Entities.Relationships;
using XmiSchema.Enums;
using XmiSchema.Managers;

namespace XmiSchema.Tests.Managers;

/// <summary>
/// Tests collection validation scenarios in <see cref="XmiModel"/>.
/// </summary>
public class XmiModelCollectionTests
{
    /// <summary>
    /// Validates newly created model has empty collections.
    /// </summary>
    [Fact]
    public void NewModel_HasEmptyCollections()
    {
        var model = new XmiModel();

        Assert.Empty(model.Entities);
        Assert.Empty(model.Relationships);
    }

    /// <summary>
    /// Validates GetXmiEntitiesOfType returns empty list when no entities exist.
    /// </summary>
    [Fact]
    public void GetEntitiesOfType_EmptyModel_ReturnsEmptyList()
    {
        var model = new XmiModel();

        var materials = model.GetXmiEntitiesOfType<XmiMaterial>();
        var points = model.GetXmiEntitiesOfType<XmiPoint3d>();
        var beams = model.GetXmiEntitiesOfType<XmiBeam>();

        Assert.Empty(materials);
        Assert.Empty(points);
        Assert.Empty(beams);
    }

    /// <summary>
    /// Validates GetXmiEntitiesOfType returns empty list when no matching type exists.
    /// </summary>
    [Fact]
    public void GetEntitiesOfType_NoMatchingType_ReturnsEmptyList()
    {
        var model = new XmiModel();
        model.AddXmiMaterial(TestModelFactory.CreateMaterial());
        model.AddXmiPoint3d(TestModelFactory.CreatePoint());

        var beams = model.GetXmiEntitiesOfType<XmiBeam>();

        Assert.Empty(beams);
    }

    /// <summary>
    /// Validates GetXmiEntitiesOfType returns only entities of the specified type.
    /// </summary>
    [Fact]
    public void GetEntitiesOfType_MixedEntities_ReturnsOnlyMatchingType()
    {
        var model = new XmiModel();
        var material = TestModelFactory.CreateMaterial();
        var point1 = TestModelFactory.CreatePoint("pt-1");
        var point2 = TestModelFactory.CreatePoint("pt-2");
        var storey = TestModelFactory.CreateStorey();

        model.AddXmiMaterial(material);
        model.AddXmiPoint3d(point1);
        model.AddXmiPoint3d(point2);
        model.AddXmiStorey(storey);

        var points = model.GetXmiEntitiesOfType<XmiPoint3d>();

        Assert.Equal(2, points.Count);
        Assert.Contains(point1, points);
        Assert.Contains(point2, points);
    }

    /// <summary>
    /// Validates model can contain multiple entities of the same type.
    /// </summary>
    [Fact]
    public void Entities_CanContainMultipleOfSameType()
    {
        var model = new XmiModel();
        var mat1 = TestModelFactory.CreateMaterial("mat-1");
        var mat2 = TestModelFactory.CreateMaterial("mat-2");
        var mat3 = TestModelFactory.CreateMaterial("mat-3");

        model.AddXmiMaterial(mat1);
        model.AddXmiMaterial(mat2);
        model.AddXmiMaterial(mat3);

        Assert.Equal(3, model.Entities.Count);
        Assert.Equal(3, model.GetXmiEntitiesOfType<XmiMaterial>().Count);
    }

    /// <summary>
    /// Validates relationships collection can be populated independently of entities.
    /// </summary>
    [Fact]
    public void Relationships_CanBeAddedWithoutCorrespondingEntities()
    {
        var model = new XmiModel();
        var source = TestModelFactory.CreateCurveMember();
        var target = TestModelFactory.CreateMaterial();

        // Add relationship without adding entities to the model
        var relation = new XmiHasMaterial(source, target);
        model.AddXmiHasMaterial(relation);

        Assert.Empty(model.Entities);
        Assert.Single(model.Relationships);
    }

    /// <summary>
    /// Validates orphaned relationships don't prevent model operation.
    /// </summary>
    [Fact]
    public void Relationships_OrphanedSource_DoesNotThrow()
    {
        var model = new XmiModel();
        var material = TestModelFactory.CreateMaterial();
        var curveMember = TestModelFactory.CreateCurveMember();

        // Only add the target entity
        model.AddXmiMaterial(material);

        // Add relationship with source not in Entities collection
        var relation = new XmiHasMaterial(curveMember, material);
        model.AddXmiHasMaterial(relation);

        Assert.Single(model.Entities);
        Assert.Single(model.Relationships);
        Assert.DoesNotContain(curveMember, model.Entities);
    }

    /// <summary>
    /// Validates orphaned relationships with missing target don't prevent model operation.
    /// </summary>
    [Fact]
    public void Relationships_OrphanedTarget_DoesNotThrow()
    {
        var model = new XmiModel();
        var material = TestModelFactory.CreateMaterial();
        var curveMember = TestModelFactory.CreateCurveMember();

        // Only add the source entity
        model.AddXmiStructuralCurveMember(curveMember);

        // Add relationship with target not in Entities collection
        var relation = new XmiHasMaterial(curveMember, material);
        model.AddXmiHasMaterial(relation);

        Assert.Single(model.Entities);
        Assert.Single(model.Relationships);
        Assert.DoesNotContain(material, model.Entities);
    }

    /// <summary>
    /// Validates relationships with both endpoints missing don't prevent model operation.
    /// </summary>
    [Fact]
    public void Relationships_BothOrphaned_DoesNotThrow()
    {
        var model = new XmiModel();
        var material = TestModelFactory.CreateMaterial();
        var curveMember = TestModelFactory.CreateCurveMember();

        // Add relationship without adding either entity
        var relation = new XmiHasMaterial(curveMember, material);
        model.AddXmiHasMaterial(relation);

        Assert.Empty(model.Entities);
        Assert.Single(model.Relationships);
    }

    /// <summary>
    /// Validates model can have many relationships for the same entity.
    /// </summary>
    [Fact]
    public void Relationships_MultipleForSameEntity_Allowed()
    {
        var model = new XmiModel();
        var curveMember = TestModelFactory.CreateCurveMember();
        var material = TestModelFactory.CreateMaterial();
        var crossSection = TestModelFactory.CreateCrossSection();
        var storey = TestModelFactory.CreateStorey();

        model.AddXmiStructuralCurveMember(curveMember);
        model.AddXmiMaterial(material);
        model.AddXmiCrossSection(crossSection);
        model.AddXmiStorey(storey);

        model.AddXmiHasMaterial(new XmiHasMaterial(curveMember, material));
        model.AddXmiHasCrossSection(new XmiHasCrossSection(curveMember, crossSection));
        model.AddXmiHasStorey(new XmiHasStorey(curveMember, storey));

        Assert.Equal(3, model.Relationships.Count);
        Assert.Equal(3, model.Relationships.Count(r => r.Source.Id == curveMember.Id));
    }

    /// <summary>
    /// Validates duplicate relationships can be added (no automatic deduplication).
    /// </summary>
    [Fact]
    public void Relationships_Duplicates_AreNotDeduplicatedAutomatically()
    {
        var model = new XmiModel();
        var source = TestModelFactory.CreateCurveMember();
        var target = TestModelFactory.CreateMaterial();

        model.AddXmiStructuralCurveMember(source);
        model.AddXmiMaterial(target);

        var relation1 = new XmiHasMaterial(source, target);
        var relation2 = new XmiHasMaterial(source, target);

        model.AddXmiHasMaterial(relation1);
        model.AddXmiHasMaterial(relation2);

        Assert.Equal(2, model.Relationships.Count);
    }

    /// <summary>
    /// Validates entities collection preserves insertion order.
    /// </summary>
    [Fact]
    public void Entities_PreservesInsertionOrder()
    {
        var model = new XmiModel();
        var mat1 = TestModelFactory.CreateMaterial("mat-1");
        var point = TestModelFactory.CreatePoint("pt-1");
        var mat2 = TestModelFactory.CreateMaterial("mat-2");
        var storey = TestModelFactory.CreateStorey();

        model.AddXmiMaterial(mat1);
        model.AddXmiPoint3d(point);
        model.AddXmiMaterial(mat2);
        model.AddXmiStorey(storey);

        Assert.Equal(4, model.Entities.Count);
        Assert.Same(mat1, model.Entities[0]);
        Assert.Same(point, model.Entities[1]);
        Assert.Same(mat2, model.Entities[2]);
        Assert.Same(storey, model.Entities[3]);
    }

    /// <summary>
    /// Validates relationships collection preserves insertion order.
    /// </summary>
    [Fact]
    public void Relationships_PreservesInsertionOrder()
    {
        var model = new XmiModel();
        var source1 = TestModelFactory.CreateCurveMember("cm-1");
        var source2 = TestModelFactory.CreateCurveMember("cm-2");
        var material = TestModelFactory.CreateMaterial();
        var storey = TestModelFactory.CreateStorey();

        var rel1 = new XmiHasMaterial(source1, material);
        var rel2 = new XmiHasStorey(source1, storey);
        var rel3 = new XmiHasMaterial(source2, material);

        model.AddXmiHasMaterial(rel1);
        model.AddXmiHasStorey(rel2);
        model.AddXmiHasMaterial(rel3);

        Assert.Equal(3, model.Relationships.Count);
        Assert.Same(rel1, model.Relationships[0]);
        Assert.Same(rel2, model.Relationships[1]);
        Assert.Same(rel3, model.Relationships[2]);
    }

    /// <summary>
    /// Validates LINQ queries work on empty entity collections.
    /// </summary>
    [Fact]
    public void Entities_LinqQueries_WorkOnEmptyCollection()
    {
        var model = new XmiModel();

        var firstMaterial = model.Entities.OfType<XmiMaterial>().FirstOrDefault();
        var anyPoints = model.Entities.OfType<XmiPoint3d>().Any();
        var materialCount = model.Entities.OfType<XmiMaterial>().Count();

        Assert.Null(firstMaterial);
        Assert.False(anyPoints);
        Assert.Equal(0, materialCount);
    }

    /// <summary>
    /// Validates LINQ queries work on empty relationship collections.
    /// </summary>
    [Fact]
    public void Relationships_LinqQueries_WorkOnEmptyCollection()
    {
        var model = new XmiModel();

        var firstRelation = model.Relationships.OfType<XmiHasMaterial>().FirstOrDefault();
        var anyStoreyRelations = model.Relationships.OfType<XmiHasStorey>().Any();
        var relationCount = model.Relationships.OfType<XmiHasMaterial>().Count();

        Assert.Null(firstRelation);
        Assert.False(anyStoreyRelations);
        Assert.Equal(0, relationCount);
    }

    /// <summary>
    /// Validates filtering relationships by source works correctly.
    /// </summary>
    [Fact]
    public void Relationships_FilterBySource_ReturnsMatchingRelationships()
    {
        var model = new XmiModel();
        var source1 = TestModelFactory.CreateCurveMember("cm-1");
        var source2 = TestModelFactory.CreateCurveMember("cm-2");
        var material = TestModelFactory.CreateMaterial();
        var storey = TestModelFactory.CreateStorey();

        model.AddXmiHasMaterial(new XmiHasMaterial(source1, material));
        model.AddXmiHasStorey(new XmiHasStorey(source1, storey));
        model.AddXmiHasMaterial(new XmiHasMaterial(source2, material));

        var source1Relations = model.Relationships.Where(r => r.Source.Id == source1.Id).ToList();

        Assert.Equal(2, source1Relations.Count);
        Assert.All(source1Relations, r => Assert.Equal(source1.Id, r.Source.Id));
    }

    /// <summary>
    /// Validates filtering relationships by target works correctly.
    /// </summary>
    [Fact]
    public void Relationships_FilterByTarget_ReturnsMatchingRelationships()
    {
        var model = new XmiModel();
        var source1 = TestModelFactory.CreateCurveMember("cm-1");
        var source2 = TestModelFactory.CreateCurveMember("cm-2");
        var material1 = TestModelFactory.CreateMaterial("mat-1");
        var material2 = TestModelFactory.CreateMaterial("mat-2");

        model.AddXmiHasMaterial(new XmiHasMaterial(source1, material1));
        model.AddXmiHasMaterial(new XmiHasMaterial(source2, material1));
        model.AddXmiHasMaterial(new XmiHasMaterial(source2, material2));

        var material1Relations = model.Relationships
            .OfType<XmiHasMaterial>()
            .Where(r => r.Target.Id == material1.Id)
            .ToList();

        Assert.Equal(2, material1Relations.Count);
        Assert.All(material1Relations, r => Assert.Equal(material1.Id, r.Target.Id));
    }

    /// <summary>
    /// Validates model can contain large numbers of entities without issue.
    /// </summary>
    [Fact]
    public void Entities_LargeCollection_HandledCorrectly()
    {
        var model = new XmiModel();

        for (int i = 0; i < 1000; i++)
        {
            model.AddXmiPoint3d(TestModelFactory.CreatePoint($"pt-{i}"));
        }

        Assert.Equal(1000, model.Entities.Count);
        Assert.Equal(1000, model.GetXmiEntitiesOfType<XmiPoint3d>().Count);
    }

    /// <summary>
    /// Validates model can contain large numbers of relationships without issue.
    /// </summary>
    [Fact]
    public void Relationships_LargeCollection_HandledCorrectly()
    {
        var model = new XmiModel();
        var material = TestModelFactory.CreateMaterial();

        for (int i = 0; i < 1000; i++)
        {
            var source = TestModelFactory.CreateCurveMember($"cm-{i}");
            model.AddXmiHasMaterial(new XmiHasMaterial(source, material));
        }

        Assert.Equal(1000, model.Relationships.Count);
    }

    /// <summary>
    /// Validates entity collection allows mixed domain types.
    /// </summary>
    [Fact]
    public void Entities_MixedDomains_AllAllowed()
    {
        var model = new XmiModel();

        // Shared domain
        var point = TestModelFactory.CreatePoint("pt-1");

        // Physical domain
        var beam = new XmiBeam(
            "beam-1",
            "Beam",
            "ifc-guid",
            "native-1",
            "Physical beam",
            XmiSystemLineEnum.MiddleMiddle,
            5.0,
            "1,0,0",
            "0,1,0",
            "0,0,1",
            0, 0, 0, 0, 0, 0
        );

        // Functional/Structural domain
        var curveMember = TestModelFactory.CreateCurveMember("cm-1");

        model.AddXmiPoint3d(point);
        model.AddXmiBeam(beam);
        model.AddXmiStructuralCurveMember(curveMember);

        Assert.Equal(3, model.Entities.Count);
        Assert.Contains(point, model.Entities);
        Assert.Contains(beam, model.Entities);
        Assert.Contains(curveMember, model.Entities);
    }

    /// <summary>
    /// Validates GetXmiEntitiesOfType returns a new list (not a reference to internal collection).
    /// </summary>
    [Fact]
    public void GetEntitiesOfType_ReturnsNewList()
    {
        var model = new XmiModel();
        var material = TestModelFactory.CreateMaterial();
        model.AddXmiMaterial(material);

        var list1 = model.GetXmiEntitiesOfType<XmiMaterial>();
        var list2 = model.GetXmiEntitiesOfType<XmiMaterial>();

        Assert.NotSame(list1, list2);
        Assert.Equal(list1.Count, list2.Count);
    }

    /// <summary>
    /// Validates FindMatchingXmiStructuralPointConnectionByPoint3d returns null when relationships collection is empty.
    /// </summary>
    [Fact]
    public void FindMatchingPointConnectionByPoint3D_EmptyRelationships_ReturnsNull()
    {
        var model = new XmiModel();
        var connection = TestModelFactory.CreatePointConnection("pc-1");

        var result = model.FindMatchingXmiStructuralPointConnectionByPoint3d(connection);

        Assert.Null(result);
    }

    /// <summary>
    /// Validates FindMatchingXmiStructuralPointConnectionByPoint3d returns null when no matching point exists.
    /// </summary>
    [Fact]
    public void FindMatchingPointConnectionByPoint3D_NoMatchingPoint_ReturnsNull()
    {
        var model = new XmiModel();
        var connection1 = TestModelFactory.CreatePointConnection("pc-1");
        var point1 = new XmiPoint3d("pt-1", "Point1", "", "", "", 1, 2, 3);

        var connection2 = TestModelFactory.CreatePointConnection("pc-2");
        var point2 = new XmiPoint3d("pt-2", "Point2", "", "", "", 4, 5, 6);

        model.AddXmiHasPoint3d(new XmiHasPoint3d(connection1, point1));

        var result = model.FindMatchingXmiStructuralPointConnectionByPoint3d(connection2);

        Assert.Null(result);
    }
}
