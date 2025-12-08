using XmiSchema.Core.Geometries;
using XmiSchema.Tests.Support;

namespace XmiSchema.Tests.Models.Geometries;

/// <summary>
/// Validates the positional logic of <see cref="XmiPoint3d"/>.
/// </summary>
public class XmiPoint3DTests
{
    /// <summary>
    /// Ensures equality honors the built-in tolerance.
    /// </summary>
    [Fact]
    public void Equals_UsesTolerance()
    {
        var first = TestModelFactory.CreatePoint(x: 1.0, y: 2.0, z: 3.0);
        var second = TestModelFactory.CreatePoint(x: 1.0 + 1e-11, y: 2.0, z: 3.0);

        Assert.True(first.Equals(second));
    }

    /// <summary>
    /// Confirms the hash code uses quantized coordinates for stability.
    /// </summary>
    [Fact]
    public void GetHashCode_ConsidersQuantizedCoordinates()
    {
        var first = TestModelFactory.CreatePoint(x: 1.0, y: 2.0, z: 3.0);
        var second = TestModelFactory.CreatePoint(x: 1.0 + 1e-11, y: 2.0, z: 3.0);

        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }
}
