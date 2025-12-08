using XmiSchema.Entities.Physical;
using XmiSchema.Entities.StructuralAnalytical;
using XmiSchema.Tests.Support;

using XmiSchema.Enums;
using XmiSchema.Entities.Commons;
namespace XmiSchema.Tests.Models.Entities;

/// <summary>
/// Covers the metadata handling of <see cref="XmiStorey"/>.
/// </summary>
public class XmiStoreyTests
{
    /// <summary>
    /// Ensures all constructor arguments are mapped to properties.
    /// </summary>
    [Fact]
    public void Constructor_AssignsProperties()
    {
        var storey = TestModelFactory.CreateStorey();

        Assert.Equal(12.0, storey.StoreyElevation);
        Assert.Equal(1000, storey.StoreyMass);
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
