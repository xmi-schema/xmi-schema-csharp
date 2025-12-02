using XmiSchema.Core.Entities;
using XmiSchema.Core.Relationships;

namespace XmiSchema.Core.Tests.Models.Relationships;

/// <summary>
/// Covers the constructors on <see cref="XmiHasPoint3D"/>.
/// </summary>
public class XmiHasPoint3DTests
{
    /// <summary>
    /// Ensures the explicit constructor stores the provided metadata.
    /// </summary>
    [Fact]
    public void Constructor_WithExplicitValues_AssignsMetadata()
    {
        var relation = new XmiHasPoint3D(
            "rel-1",
            TestModelFactory.CreateCurveMember(),
            TestModelFactory.CreatePointConnection(),
            "Owns",
            "desc",
            nameof(XmiHasPoint3D),
            "Association");

        Assert.Equal("rel-1", relation.ID);
        Assert.Equal("Owns", relation.Name);
        Assert.Equal(nameof(XmiHasPoint3D), relation.EntityType);
    }

    /// <summary>
    /// Ensures the compact constructor generates identifiers.
    /// </summary>
    [Fact]
    public void Constructor_WithAutoIdentifier_GeneratesId()
    {
        var relation = new XmiHasPoint3D(TestModelFactory.CreateCurveMember(), TestModelFactory.CreatePointConnection());

        Assert.False(string.IsNullOrWhiteSpace(relation.ID));
        Assert.Equal(nameof(XmiHasPoint3D), relation.Name);
    }
}
