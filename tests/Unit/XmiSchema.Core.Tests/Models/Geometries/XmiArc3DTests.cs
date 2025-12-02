using XmiSchema.Core.Geometries;

namespace XmiSchema.Core.Tests.Models.Geometries;

/// <summary>
/// Validates arc geometry metadata.
/// </summary>
public class XmiArc3DTests
{
    /// <summary>
    /// Ensures radius and references are stored.
    /// </summary>
    [Fact]
    public void Constructor_AssignsArcData()
    {
        var arc = TestModelFactory.CreateArc();

        Assert.Equal(2.5f, arc.Radius);
        Assert.Equal("arc-start", arc.StartPoint.ID);
    }
}
