using XmiSchema.Tests.Support;

using XmiSchema.Models.Enums;
namespace XmiSchema.Tests.Models.Geometries;

/// <summary>
/// Validates arc geometry metadata.
/// </summary>
public class XmiArc3dTests
{
    /// <summary>
    /// Ensures radius and references are stored.
    /// </summary>
    [Fact]
    public void Constructor_AssignsArcData()
    {
        var arc = TestModelFactory.CreateArc();

        Assert.Equal(2.5f, arc.Radius);
        Assert.Equal("arc-start", arc.StartPoint.Id);
    }
}
