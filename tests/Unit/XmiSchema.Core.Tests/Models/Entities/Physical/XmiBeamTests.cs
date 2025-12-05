using XmiSchema.Core.Entities;
using XmiSchema.Core.Enums;
using XmiSchema.Core.Models.Entities.Physical;
using XmiSchema.Core.Tests.Support;

namespace XmiSchema.Core.Tests.Models.Entities.Physical;

/// <summary>
/// Validates <see cref="XmiBeam"/> entity creation and properties.
/// </summary>
public class XmiBeamTests
{
    /// <summary>
    /// Ensures constructor assigns all required properties.
    /// </summary>
    [Fact]
    public void Constructor_AssignsAllProperties()
    {
        var beam = TestModelFactory.CreateBeam();

        Assert.Equal("beam-1", beam.Id);
        Assert.Equal("Beam beam-1", beam.Name);
        Assert.Equal("BEAM-1", beam.NativeId);
        Assert.Equal(nameof(XmiBeam), beam.EntityType);
        Assert.Equal(XmiSystemLineEnum.MiddleMiddle, beam.SystemLine);
        Assert.Equal(5.0, beam.Length);
    }

    /// <summary>
    /// Verifies that XmiBeam inherits from XmiBasePhysicalEntity and has Physical type.
    /// </summary>
    [Fact]
    public void Constructor_SetsTypeToPhysical()
    {
        var beam = TestModelFactory.CreateBeam();

        Assert.Equal(XmiBaseEntityDomainEnum.Physical, beam.Type);
        Assert.IsAssignableFrom<XmiBasePhysicalEntity>(beam);
    }

    /// <summary>
    /// Equality is based on the native identifier.
    /// </summary>
    [Fact]
    public void Equals_UsesNativeId()
    {
        var first = TestModelFactory.CreateBeam("beam-shared");
        var second = TestModelFactory.CreateBeam("beam-shared");

        Assert.True(first.Equals(second));
    }

    /// <summary>
    /// Different native IDs result in inequality.
    /// </summary>
    [Fact]
    public void Equals_ReturnsFalseForDifferentNativeIds()
    {
        var first = TestModelFactory.CreateBeam("beam-a");
        var second = TestModelFactory.CreateBeam("beam-b");

        Assert.False(first.Equals(second));
    }

    /// <summary>
    /// GetHashCode is consistent with equality.
    /// </summary>
    [Fact]
    public void GetHashCode_ConsistentWithEquals()
    {
        var first = TestModelFactory.CreateBeam("beam-hash");
        var second = TestModelFactory.CreateBeam("beam-hash");

        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }
}
