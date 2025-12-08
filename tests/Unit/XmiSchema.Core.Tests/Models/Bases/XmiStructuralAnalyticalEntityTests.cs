using XmiSchema.Core.Entities;
using XmiSchema.Models.Bases;

namespace XmiSchema.Tests.Models.Bases;

/// <summary>
/// Verifies the behavior of <see cref="XmiBaseStructuralAnalyticalEntity"/> base class.
/// </summary>
public class XmiStructuralAnalyticalEntityTests
{
    /// <summary>
    /// Ensures the Type property is automatically set to StructuralAnalytical.
    /// </summary>
    [Fact]
    public void Constructor_SetsTypeToStructuralAnalytical()
    {
        var entity = new TestStructuralAnalyticalEntity("struct-1", "Structural Entity", "ifc", "native", "desc");

        Assert.Equal(XmiBaseEntityDomainEnum.StructuralAnalytical, entity.Type);
    }

    /// <summary>
    /// Verifies entity type is set correctly.
    /// </summary>
    [Fact]
    public void Constructor_AssignsEntityType()
    {
        var entity = new TestStructuralAnalyticalEntity("struct-2", "Structural Entity", "ifc", "native", "desc");

        Assert.Equal(nameof(TestStructuralAnalyticalEntity), entity.EntityType);
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
