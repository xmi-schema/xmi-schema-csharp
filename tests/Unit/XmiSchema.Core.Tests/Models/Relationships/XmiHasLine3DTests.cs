using XmiSchema.Core.Relationships;

namespace XmiSchema.Core.Tests.Models.Relationships;

/// <summary>
/// Ensures line relationships retain their metadata.
/// </summary>
public class XmiHasLine3DTests
{
    [Fact]
    public void Constructor_AssignsMetadata()
    {
        var relation = new XmiHasLine3D(
            "rel-line",
            TestModelFactory.CreateCurveMember(),
            TestModelFactory.CreateCurveMember("curve-b"),
            "AlignedWith",
            "desc",
            nameof(XmiHasLine3D));

        Assert.Equal("rel-line", relation.Id);
        Assert.Equal("AlignedWith", relation.Name);
    }

    [Fact]
    public void Constructor_GeneratesIdentifier()
    {
        var relation = new XmiHasLine3D(TestModelFactory.CreateCurveMember(), TestModelFactory.CreateCurveMember("curve-b"));

        Assert.False(string.IsNullOrWhiteSpace(relation.Id));
    }
}
