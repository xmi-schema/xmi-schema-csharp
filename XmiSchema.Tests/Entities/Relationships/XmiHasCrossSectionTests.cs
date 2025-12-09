using XmiSchema.Enums;
using XmiSchema.Entities.Relationships;
using XmiSchema.Tests.Managers;

namespace XmiSchema.Tests.Entities.Relationships;

/// <summary>
/// Validates <see cref="XmiHasCrossSection"/>.
/// </summary>
public class XmiHasCrossSectionTests
{
    [Fact]
    public void Constructor_AssignsMetadata()
    {
        var relation = new XmiHasCrossSection(
            "rel-sec",
            TestModelFactory.CreateCurveMember(),
            TestModelFactory.CreateCrossSection(),
            "Uses",
            "desc",
            nameof(XmiHasCrossSection));

        Assert.Equal("rel-sec", relation.Id);
        Assert.Equal(nameof(XmiHasCrossSection), relation.EntityName);
    }

    [Fact]
    public void Constructor_GeneratesIdentifier()
    {
        var relation = new XmiHasCrossSection(TestModelFactory.CreateCurveMember(), TestModelFactory.CreateCrossSection());

        Assert.False(string.IsNullOrWhiteSpace(relation.Id));
    }
}
