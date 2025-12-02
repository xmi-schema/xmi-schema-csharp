using XmiSchema.Core.Entities;

namespace XmiSchema.Core.Relationships;

/// <summary>
/// Links members to the cross-section definition they reference.
/// </summary>
public class XmiHasStructuralCrossSection : XmiBaseRelationship
{
    /// <summary>
    /// Creates a cross-section relationship with explicit metadata.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <param name="source">Entity requiring the cross-section.</param>
    /// <param name="target">Cross-section entity.</param>
    /// <param name="name">Relationship label.</param>
    /// <param name="description">Notes describing the assignment.</param>
    /// <param name="entityName">Serialized entity name.</param>
    /// <param name="umlType">UML stereotype.</param>
    public XmiHasStructuralCrossSection(
        string id,
        XmiBaseEntity source,
        XmiBaseEntity target,
        string name,
        string description,
        string entityName,
        string umlType
    ) : base(id, source, target, name, description, nameof(XmiHasStructuralCrossSection), "Association")
    {
    }

    /// <summary>
    /// Generates a basic cross-section relationship with automatic identifier.
    /// </summary>
    /// <param name="source">Entity requiring the cross-section.</param>
    /// <param name="target">Cross-section entity.</param>
    public XmiHasStructuralCrossSection(
        XmiBaseEntity source,
        XmiBaseEntity target
    ) : base(source, target, nameof(XmiHasStructuralCrossSection), "Association")
    {
    }
}
