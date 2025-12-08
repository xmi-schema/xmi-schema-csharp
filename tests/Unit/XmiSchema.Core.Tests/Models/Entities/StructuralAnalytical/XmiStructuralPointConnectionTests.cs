using XmiSchema.Tests.Support;

using XmiSchema.Enums;
using XmiSchema.Entities.StructuralAnalytical;
using XmiSchema.Entities.Geometries;
namespace XmiSchema.Tests.Models.Entities.StructuralAnalytical;

/// <summary>
/// Tests the behavior of <see cref="XmiStructuralPointConnection"/> instances.
/// </summary>
public class XmiStructuralPointConnectionTests
{
    /// <summary>
    /// Connections compare equality based on the referenced <see cref="XmiPoint3d"/>.
    /// </summary>
    [Fact]
    public void Equals_UsesPointCoordinate()
    {
        var a = TestModelFactory.CreatePointConnection("pc-a");
        var b = TestModelFactory.CreatePointConnection("pc-b");
        var point = TestModelFactory.CreatePoint("pt-shared");

        a.Point = point;
        b.Point = new XmiPoint3d(point.Id, point.Name, point.IfcGuid, point.NativeId, point.Description, point.X, point.Y, point.Z);

        Assert.True(a.Equals(b));
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }
}
