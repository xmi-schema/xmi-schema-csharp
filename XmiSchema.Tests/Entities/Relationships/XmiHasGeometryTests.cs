using XmiSchema.Enums;
using XmiSchema.Entities.Relationships;
using XmiSchema.Tests.Managers;
namespace XmiSchema.Tests.Entities.Relationships;

/// <summary>
/// Tests geometry relationship constructors.
/// </summary>
public class XmiHasGeometryTests
{
    [Fact]
    public void Constructor_AssignsMetadata()
    {
        var relation = new XmiHasGeometry(
            "rel-geom",
            TestModelFactory.CreateCurveMember(),
            TestModelFactory.CreateLine(),
            "Geometric",
            "desc",
            nameof(XmiHasGeometry));

        Assert.Equal("rel-geom", relation.Id);
        Assert.Equal(nameof(XmiHasGeometry), relation.EntityType);
    }

    [Fact]
    public void Constructor_GeneratesIdentifier()
    {
        var relation = new XmiHasGeometry(TestModelFactory.CreateCurveMember(), TestModelFactory.CreateLine());

        Assert.False(string.IsNullOrWhiteSpace(relation.Id));
    }
}
