using XmiSchema.Entities.Bases;
using XmiSchema.Enums;

namespace XmiSchema.Tests.Entities.Bases;

/// <summary>
/// Verifies the behavior of <see cref="XmiBaseEntity"/> when constructed with varying metadata.
/// </summary>
public class XmiBaseEntityTests
{
    /// <summary>
    /// Ensures empty names fall back to the identifier to keep payloads valid.
    /// </summary>
    [Fact]
    public void Constructor_DefaultsNameToIdWhenMissing()
    {
        var entity = new XmiBaseEntity("entity-1", string.Empty, "ifc", "native", "desc", string.Empty, XmiBaseEntityDomainEnum.Functional);

        Assert.Equal("entity-1", entity.Name);
    }

    /// <summary>
    /// Ensures entity type defaults to the class name whenever omitted.
    /// </summary>
    [Fact]
    public void Constructor_DefaultsEntityName()
    {
        var entity = new XmiBaseEntity("entity-2", "Entity", "ifc", "native", "desc", string.Empty, XmiBaseEntityDomainEnum.Functional);

        Assert.Equal(nameof(XmiBaseEntity), entity.EntityName);
    }

    /// <summary>
    /// Ensures the Domain property is correctly assigned from constructor.
    /// </summary>
    [Fact]
    public void Constructor_AssignsDomainProperty()
    {
        var entity = new XmiBaseEntity("entity-3", "Entity", "ifc", "native", "desc", "TestType", XmiBaseEntityDomainEnum.Physical);

        Assert.Equal(XmiBaseEntityDomainEnum.Physical, entity.Domain);
    }

    /// <summary>
    /// Verifies all domain types can be assigned.
    /// </summary>
    [Theory]
    [InlineData(XmiBaseEntityDomainEnum.Physical)]
    [InlineData(XmiBaseEntityDomainEnum.StructuralAnalytical)]
    [InlineData(XmiBaseEntityDomainEnum.Geometry)]
    [InlineData(XmiBaseEntityDomainEnum.Functional)]
    public void Constructor_AcceptsAllDomainTypes(XmiBaseEntityDomainEnum domainType)
    {
        var entity = new XmiBaseEntity("entity-4", "Entity", "ifc", "native", "desc", "TestType", domainType);

        Assert.Equal(domainType, entity.Domain);
    }
}
