using XmiSchema.Core.Relationships;
using XmiSchema.Tests.Support;

namespace XmiSchema.Tests.Models.Relationships;

/// <summary>
/// Validates <see cref="XmiHasStructuralPointConnectiontConnection"/> constructors.
/// </summary>
public class XmiHasStructuralPointConnectionTests
{
    [Fact]
    public void Constructor_AssignsMetadata()
    {
        var relation = new XmiHasStructuralPointConnection(
            "rel-node",
            TestModelFactory.CreateCurveMember(),
            TestModelFactory.CreatePointConnection(),
            "AttachedTo",
            "desc",
            nameof(XmiHasStructuralPointConnection));

        Assert.Equal("rel-node", relation.Id);
        Assert.Equal("AttachedTo", relation.Name);
    }

    [Fact]
    public void Constructor_GeneratesIdentifier()
    {
        var relation = new XmiHasStructuralPointConnection(TestModelFactory.CreateCurveMember(), TestModelFactory.CreatePointConnection());

        Assert.False(string.IsNullOrWhiteSpace(relation.Id));
    }
}
