using XmiSchema.Enums;
using XmiSchema.Entities.Relationships;
using XmiSchema.Tests.Managers;
namespace XmiSchema.Tests.Entities.Relationships;

/// <summary>
/// Ensures <see cref="XmiHasStorey"/> behaves as expected.
/// </summary>
public class XmiHasStoreyTests
{
    [Fact]
    public void Constructor_AssignsMetadata()
    {
        var relation = new XmiHasStorey(
            "rel-storey",
            TestModelFactory.CreatePointConnection(),
            TestModelFactory.CreateStorey(),
            "ContainedIn",
            "desc",
            nameof(XmiHasStorey));

        Assert.Equal("rel-storey", relation.Id);
        Assert.Equal("ContainedIn", relation.Name);
    }

    [Fact]
    public void Constructor_GeneratesIdentifier()
    {
        var relation = new XmiHasStorey(TestModelFactory.CreatePointConnection(), TestModelFactory.CreateStorey());

        Assert.False(string.IsNullOrWhiteSpace(relation.Id));
    }
}
