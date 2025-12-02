using FluentAssertions;
using XmiSchema.Core.Entities;
using XmiSchema.Core.Managers;
using XmiSchema.Core.Relationships;
using Xunit;

namespace XmiSchema.Core.Tests.Managers;

/// <summary>
/// Unit tests for RelationshipManager<T> class
/// </summary>
public class RelationshipManagerTests
{
    private readonly RelationshipManager<XmiBaseRelationship> _relationshipManager;
    private readonly XmiBaseEntity _sourceEntity;
    private readonly XmiBaseEntity _targetEntity;

    public RelationshipManagerTests()
    {
        _relationshipManager = new RelationshipManager<XmiBaseRelationship>();
        _sourceEntity = CreateTestEntity("source1", "Source Entity");
        _targetEntity = CreateTestEntity("target1", "Target Entity");
    }

    [Fact]
    public void AddRelationship_WithValidRelationship_ShouldReturnTrue()
    {
        // Arrange
        var relationship = CreateTestRelationship("rel1", _sourceEntity, _targetEntity);

        // Act
        var result = _relationshipManager.AddRelationship(relationship);

        // Assert
        result.Should().BeTrue();
        _relationshipManager.GetRelationship("rel1").Should().NotBeNull();
    }

    [Fact]
    public void AddRelationship_WithNullId_ShouldReturnFalse()
    {
        // Arrange
        var relationship = CreateTestRelationship(null!, _sourceEntity, _targetEntity);

        // Act
        var result = _relationshipManager.AddRelationship(relationship);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void AddRelationship_WithEmptyId_ShouldReturnFalse()
    {
        // Arrange
        var relationship = CreateTestRelationship("", _sourceEntity, _targetEntity);

        // Act
        var result = _relationshipManager.AddRelationship(relationship);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void AddRelationship_WithWhitespaceId_ShouldReturnFalse()
    {
        // Arrange
        var relationship = CreateTestRelationship("   ", _sourceEntity, _targetEntity);

        // Act
        var result = _relationshipManager.AddRelationship(relationship);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void AddRelationship_WithDuplicateId_ShouldUpdateExistingRelationship()
    {
        // Arrange
        var relationship1 = CreateTestRelationship("rel1", _sourceEntity, _targetEntity);
        var relationship2 = CreateTestRelationship("rel1", _targetEntity, _sourceEntity);

        // Act
        _relationshipManager.AddRelationship(relationship1);
        _relationshipManager.AddRelationship(relationship2);

        // Assert
        var result = _relationshipManager.GetRelationship("rel1");
        result.Should().NotBeNull();
        result!.Source.ID.Should().Be(_targetEntity.ID);
        _relationshipManager.GetAll().Should().HaveCount(1);
    }

    [Fact]
    public void RemoveRelationship_WithExistingId_ShouldReturnTrue()
    {
        // Arrange
        var relationship = CreateTestRelationship("rel1", _sourceEntity, _targetEntity);
        _relationshipManager.AddRelationship(relationship);

        // Act
        var result = _relationshipManager.RemoveRelationship("rel1");

        // Assert
        result.Should().BeTrue();
        _relationshipManager.GetRelationship("rel1").Should().BeNull();
    }

    [Fact]
    public void RemoveRelationship_WithNonExistingId_ShouldReturnFalse()
    {
        // Act
        var result = _relationshipManager.RemoveRelationship("nonexistent");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetRelationship_WithExistingId_ShouldReturnRelationship()
    {
        // Arrange
        var relationship = CreateTestRelationship("rel1", _sourceEntity, _targetEntity);
        _relationshipManager.AddRelationship(relationship);

        // Act
        var result = _relationshipManager.GetRelationship("rel1");

        // Assert
        result.Should().NotBeNull();
        result!.ID.Should().Be("rel1");
        result.Source.Should().Be(_sourceEntity);
        result.Target.Should().Be(_targetEntity);
    }

    [Fact]
    public void GetRelationship_WithNonExistingId_ShouldReturnNull()
    {
        // Act
        var result = _relationshipManager.GetRelationship("nonexistent");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetAll_WithNoRelationships_ShouldReturnEmptyCollection()
    {
        // Act
        var result = _relationshipManager.GetAll();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetAll_WithMultipleRelationships_ShouldReturnAllRelationships()
    {
        // Arrange
        var entity1 = CreateTestEntity("e1", "Entity 1");
        var entity2 = CreateTestEntity("e2", "Entity 2");
        var entity3 = CreateTestEntity("e3", "Entity 3");

        var rel1 = CreateTestRelationship("rel1", entity1, entity2);
        var rel2 = CreateTestRelationship("rel2", entity2, entity3);
        var rel3 = CreateTestRelationship("rel3", entity1, entity3);

        _relationshipManager.AddRelationship(rel1);
        _relationshipManager.AddRelationship(rel2);
        _relationshipManager.AddRelationship(rel3);

        // Act
        var result = _relationshipManager.GetAll().ToList();

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(r => r.ID == "rel1");
        result.Should().Contain(r => r.ID == "rel2");
        result.Should().Contain(r => r.ID == "rel3");
    }

    [Fact]
    public void FindBySource_WithMatchingSourceId_ShouldReturnMatchingRelationships()
    {
        // Arrange
        var entity1 = CreateTestEntity("e1", "Entity 1");
        var entity2 = CreateTestEntity("e2", "Entity 2");
        var entity3 = CreateTestEntity("e3", "Entity 3");

        var rel1 = CreateTestRelationship("rel1", entity1, entity2);
        var rel2 = CreateTestRelationship("rel2", entity1, entity3);
        var rel3 = CreateTestRelationship("rel3", entity2, entity3);

        _relationshipManager.AddRelationship(rel1);
        _relationshipManager.AddRelationship(rel2);
        _relationshipManager.AddRelationship(rel3);

        // Act
        var result = _relationshipManager.FindBySource("e1").ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(r => r.ID == "rel1");
        result.Should().Contain(r => r.ID == "rel2");
        result.Should().NotContain(r => r.ID == "rel3");
    }

    [Fact]
    public void FindBySource_WithNoMatches_ShouldReturnEmptyCollection()
    {
        // Arrange
        var relationship = CreateTestRelationship("rel1", _sourceEntity, _targetEntity);
        _relationshipManager.AddRelationship(relationship);

        // Act
        var result = _relationshipManager.FindBySource("nonexistent");

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void FindByTarget_WithMatchingTargetId_ShouldReturnMatchingRelationships()
    {
        // Arrange
        var entity1 = CreateTestEntity("e1", "Entity 1");
        var entity2 = CreateTestEntity("e2", "Entity 2");
        var entity3 = CreateTestEntity("e3", "Entity 3");

        var rel1 = CreateTestRelationship("rel1", entity1, entity3);
        var rel2 = CreateTestRelationship("rel2", entity2, entity3);
        var rel3 = CreateTestRelationship("rel3", entity1, entity2);

        _relationshipManager.AddRelationship(rel1);
        _relationshipManager.AddRelationship(rel2);
        _relationshipManager.AddRelationship(rel3);

        // Act
        var result = _relationshipManager.FindByTarget("e3").ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(r => r.ID == "rel1");
        result.Should().Contain(r => r.ID == "rel2");
        result.Should().NotContain(r => r.ID == "rel3");
    }

    [Fact]
    public void FindByTarget_WithNoMatches_ShouldReturnEmptyCollection()
    {
        // Arrange
        var relationship = CreateTestRelationship("rel1", _sourceEntity, _targetEntity);
        _relationshipManager.AddRelationship(relationship);

        // Act
        var result = _relationshipManager.FindByTarget("nonexistent");

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Clear_WithMultipleRelationships_ShouldRemoveAllRelationships()
    {
        // Arrange
        var entity1 = CreateTestEntity("e1", "Entity 1");
        var entity2 = CreateTestEntity("e2", "Entity 2");

        _relationshipManager.AddRelationship(CreateTestRelationship("rel1", entity1, entity2));
        _relationshipManager.AddRelationship(CreateTestRelationship("rel2", entity2, entity1));

        // Act
        _relationshipManager.Clear();

        // Assert
        _relationshipManager.GetAll().Should().BeEmpty();
    }

    [Fact]
    public void Clear_WithNoRelationships_ShouldNotThrowException()
    {
        // Act
        var act = () => _relationshipManager.Clear();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void RelationshipManager_ShouldHandleComplexGraphStructure()
    {
        // Arrange - Create a more complex graph
        var entities = Enumerable.Range(1, 5)
            .Select(i => CreateTestEntity($"e{i}", $"Entity {i}"))
            .ToList();

        // Create relationships forming a graph
        _relationshipManager.AddRelationship(CreateTestRelationship("rel1", entities[0], entities[1]));
        _relationshipManager.AddRelationship(CreateTestRelationship("rel2", entities[0], entities[2]));
        _relationshipManager.AddRelationship(CreateTestRelationship("rel3", entities[1], entities[3]));
        _relationshipManager.AddRelationship(CreateTestRelationship("rel4", entities[2], entities[3]));
        _relationshipManager.AddRelationship(CreateTestRelationship("rel5", entities[3], entities[4]));

        // Act & Assert
        _relationshipManager.GetAll().Should().HaveCount(5);
        _relationshipManager.FindBySource("e1").Should().HaveCount(2); // e1 -> e2, e1 -> e3
        _relationshipManager.FindByTarget("e4").Should().HaveCount(2); // e2 -> e4, e3 -> e4
        _relationshipManager.FindBySource("e4").Should().HaveCount(1); // e4 -> e5
        _relationshipManager.FindByTarget("e5").Should().HaveCount(1); // e4 -> e5
    }

    [Fact]
    public void FindBySource_AndTarget_ShouldBeConsistent()
    {
        // Arrange
        var entity1 = CreateTestEntity("e1", "Entity 1");
        var entity2 = CreateTestEntity("e2", "Entity 2");
        var relationship = CreateTestRelationship("rel1", entity1, entity2);
        _relationshipManager.AddRelationship(relationship);

        // Act
        var bySource = _relationshipManager.FindBySource("e1").ToList();
        var byTarget = _relationshipManager.FindByTarget("e2").ToList();

        // Assert
        bySource.Should().HaveCount(1);
        byTarget.Should().HaveCount(1);
        bySource.First().Should().Be(relationship);
        byTarget.First().Should().Be(relationship);
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
    /// Helper method to create test relationships
    /// </summary>
    private static XmiBaseRelationship CreateTestRelationship(
        string id,
        XmiBaseEntity source,
        XmiBaseEntity target)
    {
        return new XmiBaseRelationship(
            id: id,
            source: source,
            target: target,
            name: $"Relationship-{id}",
            description: $"Test relationship {id}",
            entityType: nameof(XmiBaseRelationship),
            umlTtype: "Association"
        );
    }
}
