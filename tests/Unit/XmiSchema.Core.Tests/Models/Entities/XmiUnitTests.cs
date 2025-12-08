using XmiSchema.Models.Bases;
using XmiSchema.Models.Entities.StructuralAnalytical;
using XmiSchema.Tests.Support;

using XmiSchema.Models.Enums;
namespace XmiSchema.Tests.Models.Entities;

/// <summary>
/// Exercises the lightweight unit mapping entity.
/// </summary>
public class XmiUnitTests
{
    /// <summary>
    /// Confirms constructor arguments are reflected through the public properties.
    /// </summary>
    [Fact]
    public void Constructor_AssignsUnitMetadata()
    {
        var unit = TestModelFactory.CreateUnit();

        Assert.Equal(nameof(XmiStructuralCurveMember), unit.Entity);
        Assert.Equal(XmiUnitEnum.Meter, unit.Unit);
    }
}
