using XmiSchema.Core.Entities;
using XmiSchema.Core.Relationships;

namespace XmiSchema.Core.Tests.Models.Bases;

/// <summary>
/// Covers the shared logic implemented by <see cref="XmiBaseRelationship"/>.
/// </summary>
public class XmiBaseRelationshipTests
{
    /// <summary>
    /// Ensures fallback values populate when names or descriptions are omitted.
    /// </summary>
    [Fact]
    public void Constructor_DefaultsUnsetMetadata()
    {
        var source = new XmiBaseEntity("src", string.Empty, "ifc", "native", string.Empty, string.Empty);
        var target = new XmiBaseEntity("tgt", string.Empty, "ifc", "native", string.Empty, string.Empty);

        var relationship = new XmiBaseRelationship("rel-1", source, target, string.Empty, string.Empty, string.Empty, string.Empty);

        Assert.Equal("Unnamed", relationship.Name);
        Assert.Equal(string.Empty, relationship.Description);
        Assert.Equal(nameof(XmiBaseRelationship), relationship.EntityType);
    }

    /// <summary>
    /// Confirms that the shorthand constructor generates identifiers but preserves UML types.
    /// </summary>
    [Fact]
    public void Constructor_GeneratesIdentifier()
    {
        var source = new XmiBaseEntity("src", "Source", "ifc", "native", string.Empty, string.Empty);
        var target = new XmiBaseEntity("tgt", "Target", "ifc", "native", string.Empty, string.Empty);

        var relationship = new XmiBaseRelationship(source, target, "EdgeType", "Association");

        Assert.False(string.IsNullOrWhiteSpace(relationship.ID));
        Assert.Equal("EdgeType", relationship.Name);
        Assert.Equal("Association", relationship.UmlType);
    }
}
