using XmiSchema.Entities.Commons;
using XmiSchema.Managers;
using XmiSchema.Entities.Physical;
using XmiSchema.Entities.StructuralAnalytical;
using XmiSchema.Entities.Bases;
using XmiSchema.Enums;
namespace XmiSchema.Tests.Entities.Bases;

/// <summary>
/// Verifies the behavior of <see cref="XmiBaseStructuralAnalyticalEntity"/> base class.
/// </summary>
public class XmiStructuralAnalyticalEntityTests
{
    /// <summary>
    /// Ensures the Domain property is automatically set to StructuralAnalytical.
    /// </summary>
    [Fact]
    public void Constructor_SetsDomainToStructuralAnalytical()
    {
        var entity = new TestStructuralAnalyticalEntity("struct-1", "Structural Entity", "ifc", "native", "desc");

        Assert.Equal(XmiBaseEntityDomainEnum.StructuralAnalytical, entity.Domain);
    }

    /// <summary>
    /// Verifies entity name is set correctly.
    /// </summary>
    [Fact]
    public void Constructor_AssignsEntityName()
    {
        var entity = new TestStructuralAnalyticalEntity("struct-2", "Structural Entity", "ifc", "native", "desc");

        Assert.Equal(nameof(TestStructuralAnalyticalEntity), entity.EntityName);
    }

    /// <summary>
    /// Ensures all base entity properties are properly initialized.
    /// </summary>
    [Fact]
    public void Constructor_InitializesBaseProperties()
    {
        var entity = new TestStructuralAnalyticalEntity("struct-3", "Test Entity", "ifc-guid-456", "native-789", "Test analytical entity");

        Assert.Equal("struct-3", entity.Id);
        Assert.Equal("Test Entity", entity.Name);
        Assert.Equal("ifc-guid-456", entity.IfcGuid);
        Assert.Equal("native-789", entity.NativeId);
        Assert.Equal("Test analytical entity", entity.Description);
    }

    /// <summary>
    /// Test implementation of XmiBaseStructuralAnalyticalEntity for testing purposes.
    /// </summary>
    private class TestStructuralAnalyticalEntity : XmiBaseStructuralAnalyticalEntity
    {
        public TestStructuralAnalyticalEntity(string id, string name, string ifcGuid, string nativeId, string description)
            : base(id, name, ifcGuid, nativeId, description, nameof(TestStructuralAnalyticalEntity))
        {
        }
    }
}
