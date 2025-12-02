using XmiSchema.Core.Relationships;

namespace XmiSchema.Core.Tests.Models.Relationships;

/// <summary>
/// Validates <see cref="XmiHasStructuralNode"/> constructors.
/// </summary>
public class XmiHasStructuralNodeTests
{
    [Fact]
    public void Constructor_AssignsMetadata()
    {
        var relation = new XmiHasStructuralNode(
            "rel-node",
            TestModelFactory.CreateCurveMember(),
            TestModelFactory.CreatePointConnection(),
            "AttachedTo",
            "desc",
            nameof(XmiHasStructuralNode),
            "Association");

        Assert.Equal("rel-node", relation.Id);
        Assert.Equal("AttachedTo", relation.Name);
    }

    [Fact]
    public void Constructor_GeneratesIdentifier()
    {
        var relation = new XmiHasStructuralNode(TestModelFactory.CreateCurveMember(), TestModelFactory.CreatePointConnection());

        Assert.False(string.IsNullOrWhiteSpace(relation.Id));
    }
}
