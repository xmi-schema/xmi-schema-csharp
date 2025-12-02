using FluentAssertions;
using XmiSchema.Core.Entities;
using XmiSchema.Core.Enums;
using XmiSchema.Core.Geometries;
using XmiSchema.Core.Results;
using Xunit;

namespace XmiSchema.Core.Tests.Builder;

/// <summary>
/// Unit tests for XmiSchemaModelBuilder class
/// </summary>
public class XmiSchemaModelBuilderTests
{
    [Fact]
    public void AddEntity_WithSingleEntity_ShouldAddSuccessfully()
    {
        // Arrange
        var builder = new XmiSchemaModelBuilder();
        var entity = CreateTestEntity("1", "Test Entity");

        // Act
        builder.AddEntity(entity);
        var model = builder.BuildModel();

        // Assert
        model.Should().NotBeNull();
        model.Entities.Should().HaveCount(1);
        model.Entities.First().ID.Should().Be("1");
    }

    [Fact]
    public void AddEntities_WithMultipleEntities_ShouldAddAllSuccessfully()
    {
        // Arrange
        var builder = new XmiSchemaModelBuilder();
        var entities = new[]
        {
            CreateTestEntity("1", "Entity 1"),
            CreateTestEntity("2", "Entity 2"),
            CreateTestEntity("3", "Entity 3")
        };

        // Act
        builder.AddEntities(entities);
        var model = builder.BuildModel();

        // Assert
        model.Entities.Should().HaveCount(3);
        model.Entities.Should().Contain(e => e.ID == "1");
        model.Entities.Should().Contain(e => e.ID == "2");
        model.Entities.Should().Contain(e => e.ID == "3");
    }

    [Fact]
    public void BuildModel_WithNoEntities_ShouldReturnEmptyModel()
    {
        // Arrange
        var builder = new XmiSchemaModelBuilder();

        // Act
        var model = builder.BuildModel();

        // Assert
        model.Should().NotBeNull();
        model.Entities.Should().BeEmpty();
        model.Relationships.Should().BeEmpty();
    }

    [Fact]
    public void BuildModel_WithEntities_ShouldInferRelationships()
    {
        // Arrange
        var builder = new XmiSchemaModelBuilder();

        var material = CreateTestMaterial("mat1", "Concrete");
        var crossSection = CreateTestCrossSection("cs1", "CS1", material);

        builder.AddEntity(material);
        builder.AddEntity(crossSection);

        // Act
        var model = builder.BuildModel();

        // Assert
        model.Entities.Should().HaveCount(2);
        model.Relationships.Should().NotBeEmpty();
        model.Relationships.Should().Contain(r =>
            r.Source.ID == "cs1" && r.Target.ID == "mat1");
    }

    [Fact]
    public void BuildJsonString_ShouldReturnValidJsonString()
    {
        // Arrange
        var builder = new XmiSchemaModelBuilder();
        var entity = CreateTestEntity("1", "Test Entity");
        builder.AddEntity(entity);

        // Act
        var json = builder.BuildJsonString();

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("nodes");
        json.Should().Contain("edges");
    }

    [Fact]
    public void ExportJson_ShouldCreateFile()
    {
        // Arrange
        var builder = new XmiSchemaModelBuilder();
        var entity = CreateTestEntity("1", "Test Entity");
        builder.AddEntity(entity);

        var tempFile = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.json");

        try
        {
            // Act
            builder.ExportJson(tempFile);

            // Assert
            File.Exists(tempFile).Should().BeTrue();
            var content = File.ReadAllText(tempFile);
            content.Should().Contain("nodes");
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }

    [Fact]
    public void GetTopologicallySortedEntities_WithDependencies_ShouldReturnAllEntities()
    {
        // Arrange
        var builder = new XmiSchemaModelBuilder();

        // Create entities with dependencies: Material <- CrossSection
        var material = CreateTestMaterial("mat1", "Steel");
        var crossSection = CreateTestCrossSection("cs1", "I-Beam", material);

        // Add in reverse order to test sorting
        builder.AddEntity(crossSection);
        builder.AddEntity(material);

        // Act
        var sorted = builder.GetTopologicallySortedEntities();

        // Assert
        sorted.Should().NotBeNull();
        sorted.Should().HaveCount(2);
        sorted.Should().Contain(e => e.ID == "mat1");
        sorted.Should().Contain(e => e.ID == "cs1");
    }

    [Fact]
    public void GetCycles_WithNoCycles_ShouldReturnEmptyList()
    {
        // Arrange
        var builder = new XmiSchemaModelBuilder();
        var material = CreateTestMaterial("mat1", "Concrete");
        var crossSection = CreateTestCrossSection("cs1", "CS1", material);

        builder.AddEntity(material);
        builder.AddEntity(crossSection);

        // Act
        var cycles = builder.GetCycles();

        // Assert
        cycles.Should().BeEmpty();
    }

    [Fact]
    public void AddEntity_MultipleTimes_ShouldUpdateExistingEntity()
    {
        // Arrange
        var builder = new XmiSchemaModelBuilder();
        var entity1 = CreateTestEntity("1", "Original Name");
        var entity2 = CreateTestEntity("1", "Updated Name");

        // Act
        builder.AddEntity(entity1);
        builder.AddEntity(entity2);
        var model = builder.BuildModel();

        // Assert
        model.Entities.Should().HaveCount(1);
        model.Entities.First().Name.Should().Be("Updated Name");
    }

    [Fact]
    public void BuildModel_MultipleCallsShouldReturnConsistentResults()
    {
        // Arrange
        var builder = new XmiSchemaModelBuilder();
        var material = CreateTestMaterial("mat1", "Steel");
        var crossSection = CreateTestCrossSection("cs1", "I-Beam", material);

        builder.AddEntity(material);
        builder.AddEntity(crossSection);

        // Act
        var model1 = builder.BuildModel();
        var model2 = builder.BuildModel();

        // Assert
        model1.Entities.Should().HaveCount(2);
        model2.Entities.Should().HaveCount(2);
        model1.Relationships.Should().NotBeEmpty();
        model2.Relationships.Should().NotBeEmpty();
    }

    [Fact]
    public void BuildModel_WithPointConnection_ShouldCreateGeometryRelationship()
    {
        // Arrange
        var builder = new XmiSchemaModelBuilder();

        var point = CreateTestPoint3D("p1", "Point1", 0, 0, 0);
        var connection = CreateTestPointConnection("pc1", "Connection1", point);

        builder.AddEntity(point);
        builder.AddEntity(connection);

        // Act
        var model = builder.BuildModel();

        // Assert
        model.Relationships.Should().NotBeEmpty();
        model.Relationships.Should().Contain(r =>
            r.Source.ID == "pc1" && r.Target.ID == "p1");
    }

    [Fact]
    public void AddEntities_WithEmptyCollection_ShouldNotThrow()
    {
        // Arrange
        var builder = new XmiSchemaModelBuilder();
        var emptyEntities = Enumerable.Empty<XmiBaseEntity>();

        // Act
        var act = () => builder.AddEntities(emptyEntities);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void BuildJsonString_WithComplexModel_ShouldSerializeCorrectly()
    {
        // Arrange
        var builder = new XmiSchemaModelBuilder();

        // Create a complex structure
        var material = CreateTestMaterial("mat1", "Concrete C30");
        var crossSection = CreateTestCrossSection("cs1", "Column Section", material);
        var point = CreateTestPoint3D("p1", "Point1", 0, 0, 0);

        builder.AddEntity(material);
        builder.AddEntity(crossSection);
        builder.AddEntity(point);

        // Act
        var json = builder.BuildJsonString();

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("XmiStructuralMaterial");
        json.Should().Contain("XmiStructuralCrossSection");
        json.Should().Contain("XmiPoint3D");
        json.Should().Contain("mat1");
        json.Should().Contain("cs1");
        json.Should().Contain("p1");
    }

    [Fact]
    public void GetTopologicallySortedEntities_WithIndependentEntities_ShouldReturnAll()
    {
        // Arrange
        var builder = new XmiSchemaModelBuilder();

        var point1 = CreateTestPoint3D("p1", "Point1", 0, 0, 0);
        var point2 = CreateTestPoint3D("p2", "Point2", 1, 1, 1);
        var point3 = CreateTestPoint3D("p3", "Point3", 2, 2, 2);

        builder.AddEntity(point1);
        builder.AddEntity(point2);
        builder.AddEntity(point3);

        // Act
        var sorted = builder.GetTopologicallySortedEntities();

        // Assert
        sorted.Should().HaveCount(3);
    }

    /// <summary>
    /// Helper method to create test entities
    /// </summary>
    private static XmiBaseEntity CreateTestEntity(string id, string name)
    {
        return new XmiBaseEntity(
            id: id,
            name: name,
            ifcguid: $"GUID-{id}",
            nativeId: $"Native-{id}",
            description: $"Description for {name}",
            entityType: nameof(XmiBaseEntity)
        );
    }

    /// <summary>
    /// Helper method to create test material
    /// </summary>
    private static XmiStructuralMaterial CreateTestMaterial(string id, string name)
    {
        return new XmiStructuralMaterial(
            id: id,
            name: name,
            ifcguid: $"GUID-{id}",
            nativeId: $"Native-{id}",
            description: $"Material {name}",
            materialType: XmiStructuralMaterialTypeEnum.Concrete,
            grade: 30.0,
            unitWeight: 2400.0,
            eModulus: 30000.0,
            gModulus: 12500.0,
            poissonRatio: 0.2,
            thermalCoefficient: 0.00001
        );
    }

    /// <summary>
    /// Helper method to create test cross section
    /// </summary>
    private static XmiStructuralCrossSection CreateTestCrossSection(
        string id,
        string name,
        XmiStructuralMaterial material)
    {
        return new XmiStructuralCrossSection(
            id: id,
            name: name,
            ifcguid: $"GUID-{id}",
            nativeId: $"Native-{id}",
            description: $"CrossSection {name}",
            material: material,
            shape: XmiShapeEnum.IShape,
            parameters: new[] { "300", "200", "10", "15" },
            area: 0.01,
            secondMomentOfAreaXAxis: 0.0001,
            secondMomentOfAreaYAxis: 0.00005,
            radiusOfGyrationXAxis: 0.1,
            radiusOfGyrationYAxis: 0.07,
            elasticModulusXAxis: 0.0002,
            elasticModulusYAxis: 0.0001,
            plasticModulusXAxis: 0.00025,
            plasticModulusYAxis: 0.00012,
            torsionalConstant: 0.00001
        );
    }

    /// <summary>
    /// Helper method to create test Point3D
    /// </summary>
    private static XmiPoint3D CreateTestPoint3D(
        string id,
        string name,
        double x,
        double y,
        double z)
    {
        return new XmiPoint3D(
            id: id,
            name: name,
            ifcGuid: $"GUID-{id}",
            nativeId: $"Native-{id}",
            description: $"Point {name}",
            x: x,
            y: y,
            z: z
        );
    }

    /// <summary>
    /// Helper method to create test PointConnection
    /// </summary>
    private static XmiStructuralPointConnection CreateTestPointConnection(
        string id,
        string name,
        XmiPoint3D point)
    {
        return new XmiStructuralPointConnection(
            id: id,
            name: name,
            ifcGuid: $"GUID-{id}",
            nativeId: $"Native-{id}",
            description: $"Connection {name}",
            storey: null!,
            point: point
        );
    }
}
