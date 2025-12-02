using XmiSchema.Core.Entities;

namespace XmiSchema.Core.Tests.Models.Bases;

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
        var entity = new XmiBaseEntity("entity-1", string.Empty, "ifc", "native", "desc", string.Empty);

        Assert.Equal("entity-1", entity.Name);
    }

    /// <summary>
    /// Ensures entity type defaults to the class name whenever omitted.
    /// </summary>
    [Fact]
    public void Constructor_DefaultsEntityType()
    {
        var entity = new XmiBaseEntity("entity-2", "Entity", "ifc", "native", "desc", string.Empty);

        Assert.Equal(nameof(XmiBaseEntity), entity.EntityType);
    }
}
