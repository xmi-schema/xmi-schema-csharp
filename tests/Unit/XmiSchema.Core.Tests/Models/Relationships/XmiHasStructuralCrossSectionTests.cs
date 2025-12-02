using XmiSchema.Core.Relationships;

namespace XmiSchema.Core.Tests.Models.Relationships;

/// <summary>
/// Validates <see cref="XmiHasStructuralCrossSection"/>.
/// </summary>
public class XmiHasStructuralCrossSectionTests
{
    [Fact]
    public void Constructor_AssignsMetadata()
    {
        var relation = new XmiHasStructuralCrossSection(
            "rel-sec",
            TestModelFactory.CreateCurveMember(),
            TestModelFactory.CreateCrossSection(),
            "Uses",
            "desc",
            nameof(XmiHasStructuralCrossSection),
            "Association");

        Assert.Equal("rel-sec", relation.ID);
        Assert.Equal(nameof(XmiHasStructuralCrossSection), relation.EntityType);
    }

    [Fact]
    public void Constructor_GeneratesIdentifier()
    {
        var relation = new XmiHasStructuralCrossSection(TestModelFactory.CreateCurveMember(), TestModelFactory.CreateCrossSection());

        Assert.False(string.IsNullOrWhiteSpace(relation.ID));
    }
}
