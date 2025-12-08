using XmiSchema.Entities.Commons;
using XmiSchema.Managers;
using XmiSchema.Entities.StructuralAnalytical;
using XmiSchema.Entities.Physical;
using XmiSchema.Entities.Bases;
using XmiSchema.Enums;
namespace XmiSchema.Tests.Entities.Physical;

/// <summary>
/// Validates <see cref="XmiSlab"/> entity creation and properties.
/// </summary>
public class XmiSlabTests
{
    /// <summary>
    /// Ensures constructor assigns all required properties.
    /// </summary>
    [Fact]
    public void Constructor_AssignsAllProperties()
    {
        var slab = new XmiSlab("slab-1", "S1", "ifc-slab", "native-slab-1", "Concrete slab", 1.2, new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1), 0.25);

        Assert.Equal("slab-1", slab.Id);
        Assert.Equal("S1", slab.Name);
        Assert.Equal("ifc-slab", slab.IfcGuid);
        Assert.Equal("native-slab-1", slab.NativeId);
        Assert.Equal("Concrete slab", slab.Description);
        Assert.Equal(nameof(XmiSlab), slab.EntityType);
        Assert.Equal(1.2, slab.ZOffset);
        Assert.Equal(new XmiAxis(1, 0, 0), slab.LocalAxisX);
        Assert.Equal(new XmiAxis(0, 1, 0), slab.LocalAxisY);
        Assert.Equal(new XmiAxis(0, 0, 1), slab.LocalAxisZ);
        Assert.Equal(0.25, slab.Thickness);
    }

    /// <summary>
    /// Verifies that XmiSlab inherits from XmiBasePhysicalEntity and has Physical type.
    /// </summary>
    [Fact]
    public void Constructor_SetsTypeToPhysical()
    {
        var slab = new XmiSlab("slab-2", "S2", "ifc", "native", "desc", 0, new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1), 0.3);

        Assert.Equal(XmiBaseEntityDomainEnum.Physical, slab.Type);
        Assert.IsAssignableFrom<XmiBasePhysicalEntity>(slab);
    }

    /// <summary>
    /// Equality is based on the native identifier.
    /// </summary>
    [Fact]
    public void Equals_UsesNativeId()
    {
        var first = new XmiSlab("slab-3", "S3", "ifc", "SLAB-SHARED", "First slab", 0, new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1), 0.3);
        var second = new XmiSlab("slab-4", "S4", "ifc2", "slab-shared", "Second slab", 0, new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1), 0.25);

        Assert.True(first.Equals(second));
    }

    /// <summary>
    /// Different native IDs result in inequality.
    /// </summary>
    [Fact]
    public void Equals_ReturnsFalseForDifferentNativeIds()
    {
        var first = new XmiSlab("slab-5", "S5", "ifc", "SLAB-A", "Slab A", 0, new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1), 0.2);
        var second = new XmiSlab("slab-6", "S6", "ifc", "SLAB-B", "Slab B", 0, new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1), 0.2);

        Assert.False(first.Equals(second));
    }

    /// <summary>
    /// GetHashCode is consistent with equality.
    /// </summary>
    [Fact]
    public void GetHashCode_ConsistentWithEquals()
    {
        var first = new XmiSlab("slab-7", "S7", "ifc", "SLAB-HASH", "Slab", 0, new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1), 0.2);
        var second = new XmiSlab("slab-8", "S8", "ifc", "slab-hash", "Slab", 0, new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1), 0.2);

        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }
}
