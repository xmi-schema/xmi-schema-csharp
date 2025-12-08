using XmiSchema.Enums;
using XmiSchema.Tests.Managers;
namespace XmiSchema.Tests.Entities.Geometries;

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
