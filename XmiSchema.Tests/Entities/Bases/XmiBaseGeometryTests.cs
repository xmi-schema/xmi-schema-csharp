using XmiSchema.Entities.Geometries;

using XmiSchema.Enums;
using XmiSchema.Entities.Bases;
namespace XmiSchema.Tests.Entities.Bases;

/// <summary>
/// Validates the behavior of <see cref="XmiBaseGeometry"/> through a lightweight stub.
/// </summary>
public class XmiBaseGeometryTests
{
    private sealed class StubGeometry : XmiBaseGeometry
    {
        internal StubGeometry(string? entityName = null)
            : base("geom-1", "Geom", "ifc", "native", "desc", entityName)
        {
        }
    }

    /// <summary>
    /// Verifies that omitted entity name values fall back to the geometry type name.
    /// </summary>
    [Fact]
    public void Constructor_DefaultsEntityName()
    {
        var geometry = new StubGeometry();

        Assert.Equal(nameof(XmiBaseGeometry), geometry.EntityName);
    }

    /// <summary>
    /// Verifies that explicit entity name strings are honored.
    /// </summary>
    [Fact]
    public void Constructor_UsesProvidedEntityName()
    {
        var geometry = new StubGeometry("CustomType");

        Assert.Equal("CustomType", geometry.EntityName);
    }
}
