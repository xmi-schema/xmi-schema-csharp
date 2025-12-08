using XmiSchema.Models.Bases;

namespace XmiSchema.Models.Relationships;

/// <summary>
/// Links members to the cross-section definition they reference.
/// </summary>
public class XmiHasCrossSection : XmiBaseRelationship
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
    public XmiHasCrossSection(
        string id,
        XmiBaseEntity source,
        XmiBaseEntity target,
        string name,
        string description,
        string entityName
    ) : base(id, source, target, name, description, nameof(XmiHasCrossSection))
    {
    }

    /// <summary>
    /// Generates a basic cross-section relationship with automatic identifier.
    /// </summary>
    /// <param name="source">Entity requiring the cross-section.</param>
    /// <param name="target">Cross-section entity.</param>
    public XmiHasCrossSection(
        XmiBaseEntity source,
        XmiBaseEntity target
    ) : base(source, target, nameof(XmiHasCrossSection))
    {
    }
}
