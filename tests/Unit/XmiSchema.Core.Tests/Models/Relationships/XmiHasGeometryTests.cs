using XmiSchema.Core.Relationships;

namespace XmiSchema.Core.Tests.Models.Relationships;

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
            nameof(XmiHasGeometry),
            "Association");

        Assert.Equal("rel-geom", relation.ID);
        Assert.Equal(nameof(XmiHasGeometry), relation.EntityType);
    }

    [Fact]
    public void Constructor_GeneratesIdentifier()
    {
        var relation = new XmiHasGeometry(TestModelFactory.CreateCurveMember(), TestModelFactory.CreateLine());

        Assert.False(string.IsNullOrWhiteSpace(relation.ID));
    }
}
