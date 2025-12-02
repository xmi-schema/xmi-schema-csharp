using FluentAssertions;
using XmiSchema.Core.Entities;
using XmiSchema.Core.Managers;
using Xunit;

namespace XmiSchema.Core.Tests.Managers;

/// <summary>
/// Unit tests for EntityManager<T> class
/// </summary>
public class EntityManagerTests
{
    private readonly EntityManager<XmiBaseEntity> _entityManager;

    public EntityManagerTests()
    {
        _entityManager = new EntityManager<XmiBaseEntity>();
    }

    [Fact]
    public void AddEntity_WithValidEntity_ShouldReturnTrue()
    {
        // Arrange
        var entity = CreateTestEntity("1", "Test Entity");

        // Act
        var result = _entityManager.AddEntity(entity);

        // Assert
        result.Should().BeTrue();
        _entityManager.GetEntity("1").Should().NotBeNull();
    }

    [Fact]
    public void AddEntity_WithNullId_ShouldReturnFalse()
    {
        // Arrange
        var entity = CreateTestEntity(null!, "Test Entity");

        // Act
        var result = _entityManager.AddEntity(entity);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void AddEntity_WithEmptyId_ShouldReturnFalse()
    {
        // Arrange
        var entity = CreateTestEntity("", "Test Entity");

        // Act
        var result = _entityManager.AddEntity(entity);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void AddEntity_WithWhitespaceId_ShouldReturnFalse()
    {
        // Arrange
        var entity = CreateTestEntity("   ", "Test Entity");

        // Act
        var result = _entityManager.AddEntity(entity);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void AddEntity_WithDuplicateId_ShouldUpdateExistingEntity()
    {
        // Arrange
        var entity1 = CreateTestEntity("1", "Entity 1");
        var entity2 = CreateTestEntity("1", "Entity 2");

        // Act
        _entityManager.AddEntity(entity1);
        _entityManager.AddEntity(entity2);

        // Assert
        var result = _entityManager.GetEntity("1");
        result.Should().NotBeNull();
        result!.Name.Should().Be("Entity 2");
        _entityManager.GetAllEntities().Should().HaveCount(1);
    }

    [Fact]
    public void RemoveEntity_WithExistingId_ShouldReturnTrue()
    {
        // Arrange
        var entity = CreateTestEntity("1", "Test Entity");
        _entityManager.AddEntity(entity);

        // Act
        var result = _entityManager.RemoveEntity("1");

        // Assert
        result.Should().BeTrue();
        _entityManager.GetEntity("1").Should().BeNull();
    }

    [Fact]
    public void RemoveEntity_WithNonExistingId_ShouldReturnFalse()
    {
        // Act
        var result = _entityManager.RemoveEntity("999");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetEntity_WithExistingId_ShouldReturnEntity()
    {
        // Arrange
        var entity = CreateTestEntity("1", "Test Entity");
        _entityManager.AddEntity(entity);

        // Act
        var result = _entityManager.GetEntity("1");

        // Assert
        result.Should().NotBeNull();
        result!.ID.Should().Be("1");
        result.Name.Should().Be("Test Entity");
    }

    [Fact]
    public void GetEntity_WithNonExistingId_ShouldReturnNull()
    {
        // Act
        var result = _entityManager.GetEntity("999");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetAllEntities_WithNoEntities_ShouldReturnEmptyCollection()
    {
        // Act
        var result = _entityManager.GetAllEntities();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetAllEntities_WithMultipleEntities_ShouldReturnAllEntities()
    {
        // Arrange
        var entity1 = CreateTestEntity("1", "Entity 1");
        var entity2 = CreateTestEntity("2", "Entity 2");
        var entity3 = CreateTestEntity("3", "Entity 3");

        _entityManager.AddEntity(entity1);
        _entityManager.AddEntity(entity2);
        _entityManager.AddEntity(entity3);

        // Act
        var result = _entityManager.GetAllEntities().ToList();

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(e => e.ID == "1");
        result.Should().Contain(e => e.ID == "2");
        result.Should().Contain(e => e.ID == "3");
    }

    [Fact]
    public void FindByName_WithMatchingName_ShouldReturnMatchingEntities()
    {
        // Arrange
        var entity1 = CreateTestEntity("1", "TestName");
        var entity2 = CreateTestEntity("2", "TestName");
        var entity3 = CreateTestEntity("3", "OtherName");

        _entityManager.AddEntity(entity1);
        _entityManager.AddEntity(entity2);
        _entityManager.AddEntity(entity3);

        // Act
        var result = _entityManager.FindByName("TestName").ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(e => e.ID == "1");
        result.Should().Contain(e => e.ID == "2");
        result.Should().NotContain(e => e.ID == "3");
    }

    [Fact]
    public void FindByName_WithNoMatches_ShouldReturnEmptyCollection()
    {
        // Arrange
        var entity = CreateTestEntity("1", "TestName");
        _entityManager.AddEntity(entity);

        // Act
        var result = _entityManager.FindByName("NonExistentName");

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void FindByName_IsCaseSensitive()
    {
        // Arrange
        var entity = CreateTestEntity("1", "TestName");
        _entityManager.AddEntity(entity);

        // Act
        var result = _entityManager.FindByName("testname");

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Clear_WithMultipleEntities_ShouldRemoveAllEntities()
    {
        // Arrange
        _entityManager.AddEntity(CreateTestEntity("1", "Entity 1"));
        _entityManager.AddEntity(CreateTestEntity("2", "Entity 2"));
        _entityManager.AddEntity(CreateTestEntity("3", "Entity 3"));

        // Act
        _entityManager.Clear();

        // Assert
        _entityManager.GetAllEntities().Should().BeEmpty();
    }

    [Fact]
    public void Clear_WithNoEntities_ShouldNotThrowException()
    {
        // Act
        var act = () => _entityManager.Clear();

        // Assert
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData("ID1", "Name1")]
    [InlineData("ID2", "Name2")]
    [InlineData("123", "Test Entity")]
    public void AddEntity_WithVariousValidInputs_ShouldSucceed(string id, string name)
    {
        // Arrange
        var entity = CreateTestEntity(id, name);

        // Act
        var result = _entityManager.AddEntity(entity);

        // Assert
        result.Should().BeTrue();
        var retrieved = _entityManager.GetEntity(id);
        retrieved.Should().NotBeNull();
        retrieved!.Name.Should().Be(name);
    }

    [Fact]
    public void EntityManager_ShouldMaintainEntityReferences()
    {
        // Arrange
        var entity = CreateTestEntity("1", "Test Entity");
        _entityManager.AddEntity(entity);

        // Act
        entity.Name = "Modified Name";
        var retrieved = _entityManager.GetEntity("1");

        // Assert
        retrieved!.Name.Should().Be("Modified Name");
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
}
