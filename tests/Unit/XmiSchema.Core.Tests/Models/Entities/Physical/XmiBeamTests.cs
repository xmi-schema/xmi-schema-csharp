using XmiSchema.Core.Entities;
using XmiSchema.Core.Enums;
using XmiSchema.Core.Models.Entities.Physical;

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
        var beam = new XmiBeam("beam-1", "B1", "ifc-beam", "native-beam-1", "Steel beam");

        Assert.Equal("beam-1", beam.Id);
        Assert.Equal("B1", beam.Name);
        Assert.Equal("ifc-beam", beam.IfcGuid);
        Assert.Equal("native-beam-1", beam.NativeId);
        Assert.Equal("Steel beam", beam.Description);
        Assert.Equal(nameof(XmiBeam), beam.EntityType);
    }

    /// <summary>
    /// Verifies that XmiBeam inherits from XmiBasePhysicalEntity and has Physical type.
    /// </summary>
    [Fact]
    public void Constructor_SetsTypeToPhysical()
    {
        var beam = new XmiBeam("beam-2", "B2", "ifc", "native", "desc");

        Assert.Equal(XmiBaseEntityDomainEnum.Physical, beam.Type);
        Assert.IsAssignableFrom<XmiBasePhysicalEntity>(beam);
    }

    /// <summary>
    /// Equality is based on the native identifier.
    /// </summary>
    [Fact]
    public void Equals_UsesNativeId()
    {
        var first = new XmiBeam("beam-3", "B3", "ifc", "BEAM-SHARED", "First beam");
        var second = new XmiBeam("beam-4", "B4", "ifc2", "beam-shared", "Second beam");

        Assert.True(first.Equals(second));
    }

    /// <summary>
    /// Different native IDs result in inequality.
    /// </summary>
    [Fact]
    public void Equals_ReturnsFalseForDifferentNativeIds()
    {
        var first = new XmiBeam("beam-5", "B5", "ifc", "BEAM-A", "Beam A");
        var second = new XmiBeam("beam-6", "B6", "ifc", "BEAM-B", "Beam B");

        Assert.False(first.Equals(second));
    }

    /// <summary>
    /// GetHashCode is consistent with equality.
    /// </summary>
    [Fact]
    public void GetHashCode_ConsistentWithEquals()
    {
        var first = new XmiBeam("beam-7", "B7", "ifc", "BEAM-HASH", "Beam");
        var second = new XmiBeam("beam-8", "B8", "ifc", "beam-hash", "Beam");

        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }
}
