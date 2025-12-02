using XmiSchema.Core.Relationships;

namespace XmiSchema.Core.Tests.Models.Relationships;

/// <summary>
/// Confirms segment relationships honor the provided source/target metadata.
/// </summary>
public class XmiHasSegmentTests
{
    [Fact]
    public void Constructor_AssignsMetadata()
    {
        var relation = new XmiHasSegment(
            "rel-seg",
            TestModelFactory.CreateCurveMember(),
            TestModelFactory.CreateSegment(),
            "Contains",
            "desc",
            nameof(XmiHasSegment),
            "Association");

        Assert.Equal("rel-seg", relation.Id);
        Assert.Equal("Contains", relation.Name);
    }

    [Fact]
    public void Constructor_GeneratesIdentifier()
    {
        var relation = new XmiHasSegment(TestModelFactory.CreateCurveMember(), TestModelFactory.CreateSegment());

        Assert.False(string.IsNullOrWhiteSpace(relation.Id));
    }
}
