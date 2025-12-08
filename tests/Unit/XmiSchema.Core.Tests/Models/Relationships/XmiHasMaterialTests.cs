using XmiSchema.Models.Relationships;
using XmiSchema.Tests.Support;

using XmiSchema.Models.Enums;
namespace XmiSchema.Tests.Models.Relationships;

/// <summary>
/// Tests <see cref="XmiHasMaterial"/> constructors.
/// </summary>
public class XmiHasMaterialTests
{
    [Fact]
    public void Constructor_WithExplicitValues_AssignsMetadata()
    {
        var relation = new XmiHasMaterial(
            "rel-mat",
            TestModelFactory.CreateCurveMember(),
            TestModelFactory.CreateMaterial(),
            "Uses",
            "desc",
            nameof(XmiHasMaterial));

        Assert.Equal("rel-mat", relation.Id);
        Assert.Equal("Uses", relation.Name);
        Assert.Equal(nameof(XmiHasMaterial), relation.EntityType);
    }

    [Fact]
    public void Constructor_WithAutoIdentifier_GeneratesId()
    {
        var relation = new XmiHasMaterial(TestModelFactory.CreateCurveMember(), TestModelFactory.CreateMaterial());

        Assert.False(string.IsNullOrWhiteSpace(relation.Id));
        Assert.Equal(nameof(XmiHasMaterial), relation.Name);
    }
}
