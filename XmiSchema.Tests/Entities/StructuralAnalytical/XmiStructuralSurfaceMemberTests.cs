using XmiSchema.Entities.Commons;
using XmiSchema.Managers;
using XmiSchema.Entities.Physical;
using XmiSchema.Entities.StructuralAnalytical;
using XmiSchema.Entities.Bases;
using XmiSchema.Enums;
using XmiSchema.Tests.Managers;
namespace XmiSchema.Tests.Entities.StructuralAnalytical;

/// <summary>
/// Exercises the surface member entity and its equality semantics.
/// </summary>
public class XmiStructuralSurfaceMemberTests
{
    /// <summary>
    /// Confirms constructor arguments are reflected via the public API.
    /// </summary>
    [Fact]
    public void Constructor_AssignsSurfaceProperties()
    {
        var surface = TestModelFactory.CreateSurfaceMember();

        Assert.Equal(XmiStructuralSurfaceMemberTypeEnum.Slab, surface.SurfaceMemberType);
        Assert.Equal(XmiStructuralSurfaceMemberSystemPlaneEnum.Middle, surface.SystemPlane);
        Assert.Equal(0.2, surface.Thickness);
    }

    /// <summary>
    /// Equality relies on the native identifier to avoid duplicates.
    /// </summary>
    [Fact]
    public void Equals_UsesNativeId()
    {
        var first = TestModelFactory.CreateSurfaceMember("surf-match");
        var second = TestModelFactory.CreateSurfaceMember("surf-match");

        Assert.True(first.Equals(second));
    }
}
