using XmiSchema.Core.Entities;
using XmiSchema.Core.Enums;
using XmiSchema.Core.Models.Entities.Physical;
using XmiSchema.Core.Tests.Support;

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
        var column = TestModelFactory.CreateColumn();

        Assert.Equal("col-1", column.Id);
        Assert.Equal("Column col-1", column.Name);
        Assert.Equal("COL-1", column.NativeId);
        Assert.Equal(nameof(XmiColumn), column.EntityType);
        Assert.Equal(XmiStructuralCurveMemberSystemLineEnum.MiddleMiddle, column.SystemLine);
        Assert.Equal(3.5, column.Length);
        Assert.Equal("Fixed", column.EndFixityStart);
        Assert.Equal("Fixed", column.EndFixityEnd);
    }

    /// <summary>
    /// Verifies that XmiColumn inherits from XmiBasePhysicalEntity and has Physical type.
    /// </summary>
    [Fact]
    public void Constructor_SetsTypeToPhysical()
    {
        var column = TestModelFactory.CreateColumn();

        Assert.Equal(XmiBaseEntityDomainEnum.Physical, column.Type);
        Assert.IsAssignableFrom<XmiBasePhysicalEntity>(column);
    }

    /// <summary>
    /// Equality is based on the native identifier.
    /// </summary>
    [Fact]
    public void Equals_UsesNativeId()
    {
        var first = TestModelFactory.CreateColumn("col-shared");
        var second = TestModelFactory.CreateColumn("col-shared");

        Assert.True(first.Equals(second));
    }

    /// <summary>
    /// Different native IDs result in inequality.
    /// </summary>
    [Fact]
    public void Equals_ReturnsFalseForDifferentNativeIds()
    {
        var first = TestModelFactory.CreateColumn("col-a");
        var second = TestModelFactory.CreateColumn("col-b");

        Assert.False(first.Equals(second));
    }

    /// <summary>
    /// GetHashCode is consistent with equality.
    /// </summary>
    [Fact]
    public void GetHashCode_ConsistentWithEquals()
    {
        var first = TestModelFactory.CreateColumn("col-hash");
        var second = TestModelFactory.CreateColumn("col-hash");

        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }
}
