using XmiSchema.Core.Relationships;

namespace XmiSchema.Core.Tests.Models.Relationships;

/// <summary>
/// Ensures <see cref="XmiHasStructuralStorey"/> behaves as expected.
/// </summary>
public class XmiHasStructuralStoreyTests
{
    [Fact]
    public void Constructor_AssignsMetadata()
    {
        var relation = new XmiHasStructuralStorey(
            "rel-storey",
            TestModelFactory.CreatePointConnection(),
            TestModelFactory.CreateStorey(),
            "ContainedIn",
            "desc",
            nameof(XmiHasStructuralStorey),
            "Association");

        Assert.Equal("rel-storey", relation.ID);
        Assert.Equal("ContainedIn", relation.Name);
    }

    [Fact]
    public void Constructor_GeneratesIdentifier()
    {
        var relation = new XmiHasStructuralStorey(TestModelFactory.CreatePointConnection(), TestModelFactory.CreateStorey());

        Assert.False(string.IsNullOrWhiteSpace(relation.ID));
    }
}
