using XmiSchema.Entities.Geometries;

using XmiSchema.Enums;
using XmiSchema.Entities.Bases;
namespace XmiSchema.Tests.Models.Bases;

/// <summary>
/// Validates the behavior of <see cref="XmiBaseGeometry"/> through a lightweight stub.
/// </summary>
public class XmiBaseGeometryTests
{
    private sealed class StubGeometry : XmiBaseGeometry
    {
        internal StubGeometry(string? entityType = null)
            : base("geom-1", "Geom", "ifc", "native", "desc", entityType)
        {
        }
    }

    /// <summary>
    /// Verifies that omitted entity type values fall back to the geometry type name.
    /// </summary>
    [Fact]
    public void Constructor_DefaultsEntityType()
    {
        var geometry = new StubGeometry();

        Assert.Equal(nameof(XmiBaseGeometry), geometry.EntityType);
    }

    /// <summary>
    /// Verifies that explicit entity type strings are honored.
    /// </summary>
    [Fact]
    public void Constructor_UsesProvidedEntityType()
    {
        var geometry = new StubGeometry("CustomType");

        Assert.Equal("CustomType", geometry.EntityType);
    }
}
