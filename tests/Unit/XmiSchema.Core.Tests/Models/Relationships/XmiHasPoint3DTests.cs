using XmiSchema.Entities.Commons;
using XmiSchema.Managers;
using XmiSchema.Entities.Physical;
using XmiSchema.Entities.StructuralAnalytical;
using XmiSchema.Tests.Support;

using XmiSchema.Enums;
using XmiSchema.Entities.Relationships;
namespace XmiSchema.Tests.Models.Relationships;

/// <summary>
/// Covers the constructors on <see cref="XmiHasPoint3d"/>.
/// </summary>
public class XmiHasPoint3DTests
{
    /// <summary>
    /// Ensures the explicit constructor stores the provided metadata.
    /// </summary>
    [Fact]
    public void Constructor_WithExplicitValues_AssignsMetadata()
    {
        var relation = new XmiHasPoint3d(
            "rel-1",
            TestModelFactory.CreateCurveMember(),
            TestModelFactory.CreatePointConnection(),
            "Owns",
            "desc",
            nameof(XmiHasPoint3d));

        Assert.Equal("rel-1", relation.Id);
        Assert.Equal("Owns", relation.Name);
        Assert.Equal(nameof(XmiHasPoint3d), relation.EntityType);
    }

    /// <summary>
    /// Ensures the compact constructor generates identifiers.
    /// </summary>
    [Fact]
    public void Constructor_WithAutoIdentifier_GeneratesId()
    {
        var relation = new XmiHasPoint3d(TestModelFactory.CreateCurveMember(), TestModelFactory.CreatePointConnection());

        Assert.False(string.IsNullOrWhiteSpace(relation.Id));
        Assert.Equal(nameof(XmiHasPoint3d), relation.Name);
    }
}
