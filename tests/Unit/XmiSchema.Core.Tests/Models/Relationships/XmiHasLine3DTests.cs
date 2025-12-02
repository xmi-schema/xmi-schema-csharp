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
            nameof(XmiHasLine3D),
            "Association");

        Assert.Equal("rel-line", relation.ID);
        Assert.Equal("AlignedWith", relation.Name);
    }

    [Fact]
    public void Constructor_GeneratesIdentifier()
    {
        var relation = new XmiHasLine3D(TestModelFactory.CreateCurveMember(), TestModelFactory.CreateCurveMember("curve-b"));

        Assert.False(string.IsNullOrWhiteSpace(relation.ID));
    }
}
