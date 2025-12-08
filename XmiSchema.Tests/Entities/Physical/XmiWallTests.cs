using XmiSchema.Entities.Commons;
using XmiSchema.Managers;
using XmiSchema.Entities.StructuralAnalytical;
using XmiSchema.Entities.Physical;
using XmiSchema.Entities.Bases;
using XmiSchema.Enums;
namespace XmiSchema.Tests.Entities.Physical;

/// <summary>
/// Validates <see cref="XmiWall"/> entity creation and properties.
/// </summary>
public class XmiWallTests
{
    /// <summary>
    /// Ensures constructor assigns all required properties.
    /// </summary>
    [Fact]
    public void Constructor_AssignsAllProperties()
    {
        var wall = new XmiWall("wall-1", "W1", "ifc-wall", "native-wall-1", "Concrete wall");

        Assert.Equal("wall-1", wall.Id);
        Assert.Equal("W1", wall.Name);
        Assert.Equal("ifc-wall", wall.IfcGuid);
        Assert.Equal("native-wall-1", wall.NativeId);
        Assert.Equal("Concrete wall", wall.Description);
        Assert.Equal(nameof(XmiWall), wall.EntityType);
    }

    /// <summary>
    /// Verifies that XmiWall inherits from XmiBasePhysicalEntity and has Physical type.
    /// </summary>
    [Fact]
    public void Constructor_SetsTypeToPhysical()
    {
        var wall = new XmiWall("wall-2", "W2", "ifc", "native", "desc");

        Assert.Equal(XmiBaseEntityDomainEnum.Physical, wall.Type);
        Assert.IsAssignableFrom<XmiBasePhysicalEntity>(wall);
    }

    /// <summary>
    /// Equality is based on the native identifier.
    /// </summary>
    [Fact]
    public void Equals_UsesNativeId()
    {
        var first = new XmiWall("wall-3", "W3", "ifc", "WALL-SHARED", "First wall");
        var second = new XmiWall("wall-4", "W4", "ifc2", "wall-shared", "Second wall");

        Assert.True(first.Equals(second));
    }

    /// <summary>
    /// Different native IDs result in inequality.
    /// </summary>
    [Fact]
    public void Equals_ReturnsFalseForDifferentNativeIds()
    {
        var first = new XmiWall("wall-5", "W5", "ifc", "WALL-A", "Wall A");
        var second = new XmiWall("wall-6", "W6", "ifc", "WALL-B", "Wall B");

        Assert.False(first.Equals(second));
    }

    /// <summary>
    /// GetHashCode is consistent with equality.
    /// </summary>
    [Fact]
    public void GetHashCode_ConsistentWithEquals()
    {
        var first = new XmiWall("wall-7", "W7", "ifc", "WALL-HASH", "Wall");
        var second = new XmiWall("wall-8", "W8", "ifc", "wall-hash", "Wall");

        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }
}
