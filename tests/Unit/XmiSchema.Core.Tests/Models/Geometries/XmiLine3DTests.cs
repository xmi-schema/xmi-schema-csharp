using XmiSchema.Core.Geometries;

namespace XmiSchema.Core.Tests.Models.Geometries;

/// <summary>
/// Verifies the <see cref="XmiLine3d"/> geometric endpoints are stored.
/// </summary>
public class XmiLine3DTests
{
    /// <summary>
    /// Ensures the constructor accepts and exposes both endpoints.
    /// </summary>
    [Fact]
    public void Constructor_AssignsEndpoints()
    {
        var line = TestModelFactory.CreateLine();

        Assert.NotNull(line.StartPoint);
        Assert.NotNull(line.EndPoint);
        Assert.Equal("line-start", line.StartPoint.Id);
    }
}
