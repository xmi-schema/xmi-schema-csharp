using XmiSchema.Entities.Commons;
using XmiSchema.Managers;
using XmiSchema.Entities.Physical;
using XmiSchema.Entities.StructuralAnalytical;

using XmiSchema.Enums;
using XmiSchema.Entities.Relationships;
using XmiSchema.Tests.Managers;
namespace XmiSchema.Tests.Entities.Relationships;

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
            TestModelFactory.CreatePoint(),
            "Owns",
            "desc",
            nameof(XmiHasPoint3d));

        Assert.Equal("rel-1", relation.Id);
        Assert.Equal("Owns", relation.Name);
        Assert.Equal(nameof(XmiHasPoint3d), relation.EntityName);
    }

    /// <summary>
    /// Ensures the compact constructor generates identifiers.
    /// </summary>
    [Fact]
    public void Constructor_WithAutoIdentifier_GeneratesId()
    {
        var relation = new XmiHasPoint3d(TestModelFactory.CreateCurveMember(), TestModelFactory.CreatePoint());

        Assert.False(string.IsNullOrWhiteSpace(relation.Id));
        Assert.Equal(nameof(XmiHasPoint3d), relation.Name);
    }
}
