using XmiSchema.Core.Relationships;

namespace XmiSchema.Core.Tests.Models.Relationships;

/// <summary>
/// Tests <see cref="XmiHasStructuralMaterial"/> constructors.
/// </summary>
public class XmiHasStructuralMaterialTests
{
    [Fact]
    public void Constructor_WithExplicitValues_AssignsMetadata()
    {
        var relation = new XmiHasStructuralMaterial(
            "rel-mat",
            TestModelFactory.CreateCurveMember(),
            TestModelFactory.CreateMaterial(),
            "Uses",
            "desc",
            nameof(XmiHasStructuralMaterial),
            "Association");

        Assert.Equal("rel-mat", relation.ID);
        Assert.Equal("Uses", relation.Name);
        Assert.Equal(nameof(XmiHasStructuralMaterial), relation.EntityType);
    }

    [Fact]
    public void Constructor_WithAutoIdentifier_GeneratesId()
    {
        var relation = new XmiHasStructuralMaterial(TestModelFactory.CreateCurveMember(), TestModelFactory.CreateMaterial());

        Assert.False(string.IsNullOrWhiteSpace(relation.ID));
        Assert.Equal(nameof(XmiHasStructuralMaterial), relation.Name);
    }
}
