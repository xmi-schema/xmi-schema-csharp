using XmiSchema.Core.Entities;

namespace XmiSchema.Core.Tests.Models.Entities;

/// <summary>
/// Covers the metadata handling of <see cref="XmiStructuralStorey"/>.
/// </summary>
public class XmiStructuralStoreyTests
{
    /// <summary>
    /// Ensures all constructor arguments are mapped to properties.
    /// </summary>
    [Fact]
    public void Constructor_AssignsReactions()
    {
        var storey = TestModelFactory.CreateStorey();

        Assert.Equal("Fx", storey.StoreyHorizontalReactionX);
        Assert.Equal("Fz", storey.StoreyVerticalReaction);
    }

    /// <summary>
    /// Storeys compare equality using the native identifier.
    /// </summary>
    [Fact]
    public void Equals_UsesNativeId()
    {
        var first = TestModelFactory.CreateStorey("storey-shared");
        var second = TestModelFactory.CreateStorey("storey-shared");

        Assert.True(first.Equals(second));
    }
}
