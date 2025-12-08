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
        var slab = new XmiSlab("slab-1", "S1", "ifc-slab", "native-slab-1", "Concrete slab");

        Assert.Equal("slab-1", slab.Id);
        Assert.Equal("S1", slab.Name);
        Assert.Equal("ifc-slab", slab.IfcGuid);
        Assert.Equal("native-slab-1", slab.NativeId);
        Assert.Equal("Concrete slab", slab.Description);
        Assert.Equal(nameof(XmiSlab), slab.EntityType);
    }

    /// <summary>
    /// Verifies that XmiSlab inherits from XmiBasePhysicalEntity and has Physical type.
    /// </summary>
    [Fact]
    public void Constructor_SetsTypeToPhysical()
    {
        var slab = new XmiSlab("slab-2", "S2", "ifc", "native", "desc");

        Assert.Equal(XmiBaseEntityDomainEnum.Physical, slab.Type);
        Assert.IsAssignableFrom<XmiBasePhysicalEntity>(slab);
    }

    /// <summary>
    /// Equality is based on the native identifier.
    /// </summary>
    [Fact]
    public void Equals_UsesNativeId()
    {
        var first = new XmiSlab("slab-3", "S3", "ifc", "SLAB-SHARED", "First slab");
        var second = new XmiSlab("slab-4", "S4", "ifc2", "slab-shared", "Second slab");

        Assert.True(first.Equals(second));
    }

    /// <summary>
    /// Different native IDs result in inequality.
    /// </summary>
    [Fact]
    public void Equals_ReturnsFalseForDifferentNativeIds()
    {
        var first = new XmiSlab("slab-5", "S5", "ifc", "SLAB-A", "Slab A");
        var second = new XmiSlab("slab-6", "S6", "ifc", "SLAB-B", "Slab B");

        Assert.False(first.Equals(second));
    }

    /// <summary>
    /// GetHashCode is consistent with equality.
    /// </summary>
    [Fact]
    public void GetHashCode_ConsistentWithEquals()
    {
        var first = new XmiSlab("slab-7", "S7", "ifc", "SLAB-HASH", "Slab");
        var second = new XmiSlab("slab-8", "S8", "ifc", "slab-hash", "Slab");

        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }
}
