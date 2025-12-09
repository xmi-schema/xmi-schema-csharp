using XmiSchema.Entities.Commons;
using XmiSchema.Managers;
using XmiSchema.Entities.Physical;
using XmiSchema.Entities.StructuralAnalytical;
using XmiSchema.Entities.Bases;
using XmiSchema.Enums;
namespace XmiSchema.Tests.Entities.Bases;

/// <summary>
/// Verifies the behavior of <see cref="XmiBasePhysicalEntity"/> base class.
/// </summary>
public class XmiPhysicalEntityTests
{
    /// <summary>
    /// Ensures the Domain property is automatically set to Physical.
    /// </summary>
    [Fact]
    public void Constructor_SetsDomainToPhysical()
    {
        var entity = new TestPhysicalEntity("phys-1", "Physical Entity", "ifc", "native", "desc");

        Assert.Equal(XmiBaseEntityDomainEnum.Physical, entity.Domain);
    }

    /// <summary>
    /// Verifies entity name is set correctly.
    /// </summary>
    [Fact]
    public void Constructor_AssignsEntityName()
    {
        var entity = new TestPhysicalEntity("phys-2", "Physical Entity", "ifc", "native", "desc");

        Assert.Equal(nameof(TestPhysicalEntity), entity.EntityName);
    }

    /// <summary>
    /// Ensures all base entity properties are properly initialized.
    /// </summary>
    [Fact]
    public void Constructor_InitializesBaseProperties()
    {
        var entity = new TestPhysicalEntity("phys-3", "Test Entity", "ifc-guid-123", "native-456", "Test description");

        Assert.Equal("phys-3", entity.Id);
        Assert.Equal("Test Entity", entity.Name);
        Assert.Equal("ifc-guid-123", entity.IfcGuid);
        Assert.Equal("native-456", entity.NativeId);
        Assert.Equal("Test description", entity.Description);
    }

    /// <summary>
    /// Test implementation of XmiBasePhysicalEntity for testing purposes.
    /// </summary>
    private class TestPhysicalEntity : XmiBasePhysicalEntity
    {
        public TestPhysicalEntity(string id, string name, string ifcGuid, string nativeId, string description)
            : base(id, name, ifcGuid, nativeId, description, nameof(TestPhysicalEntity))
        {
        }
    }
}
