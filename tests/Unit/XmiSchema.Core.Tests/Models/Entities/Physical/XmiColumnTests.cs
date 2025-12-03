using XmiSchema.Core.Entities;
using XmiSchema.Core.Enums;
using XmiSchema.Core.Models.Entities.Physical;

namespace XmiSchema.Core.Tests.Models.Entities.Physical;

/// <summary>
/// Validates <see cref="XmiColumn"/> entity creation and properties.
/// </summary>
public class XmiColumnTests
{
    /// <summary>
    /// Ensures constructor assigns all required properties.
    /// </summary>
    [Fact]
    public void Constructor_AssignsAllProperties()
    {
        var column = new XmiColumn("col-1", "C1", "ifc-column", "native-col-1", "Concrete column");

        Assert.Equal("col-1", column.Id);
        Assert.Equal("C1", column.Name);
        Assert.Equal("ifc-column", column.IfcGuid);
        Assert.Equal("native-col-1", column.NativeId);
        Assert.Equal("Concrete column", column.Description);
        Assert.Equal(nameof(XmiColumn), column.EntityType);
    }

    /// <summary>
    /// Verifies that XmiColumn inherits from XmiBasePhysicalEntity and has Physical type.
    /// </summary>
    [Fact]
    public void Constructor_SetsTypeToPhysical()
    {
        var column = new XmiColumn("col-2", "C2", "ifc", "native", "desc");

        Assert.Equal(XmiBaseEntityDomainEnum.Physical, column.Type);
        Assert.IsAssignableFrom<XmiBasePhysicalEntity>(column);
    }

    /// <summary>
    /// Equality is based on the native identifier.
    /// </summary>
    [Fact]
    public void Equals_UsesNativeId()
    {
        var first = new XmiColumn("col-3", "C3", "ifc", "COLUMN-SHARED", "First column");
        var second = new XmiColumn("col-4", "C4", "ifc2", "column-shared", "Second column");

        Assert.True(first.Equals(second));
    }

    /// <summary>
    /// Different native IDs result in inequality.
    /// </summary>
    [Fact]
    public void Equals_ReturnsFalseForDifferentNativeIds()
    {
        var first = new XmiColumn("col-5", "C5", "ifc", "COLUMN-A", "Column A");
        var second = new XmiColumn("col-6", "C6", "ifc", "COLUMN-B", "Column B");

        Assert.False(first.Equals(second));
    }

    /// <summary>
    /// GetHashCode is consistent with equality.
    /// </summary>
    [Fact]
    public void GetHashCode_ConsistentWithEquals()
    {
        var first = new XmiColumn("col-7", "C7", "ifc", "COLUMN-HASH", "Column");
        var second = new XmiColumn("col-8", "C8", "ifc", "column-hash", "Column");

        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }
}
