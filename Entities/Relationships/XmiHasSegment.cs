using XmiSchema.Entities.Bases;

namespace XmiSchema.Entities.Relationships;

/// <summary>
/// Binds curve members to segment definitions so consumers can rebuild detailed analytical definitions.
/// </summary>
public class XmiHasSegment : XmiBaseRelationship
{
    /// <summary>
    /// Creates a segment relationship with explicit identifiers.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <param name="source">Owning entity (usually a curve member).</param>
    /// <param name="target">Segment entity.</param>
    /// <param name="name">Relationship label.</param>
    /// <param name="description">Additional notes.</param>
    /// <param name="entityName">Serialized entity name.</param>
    public XmiHasSegment(
        string id,
        XmiBaseEntity source,
        XmiBaseEntity target,
        string name,
        string description,
        string entityName
    ) : base(id, source, target, name, description, nameof(XmiHasSegment))
    {
    }

    /// <summary>
    /// Auto-generates an identifier for a simple segment relationship.
    /// </summary>
    /// <param name="source">Owning entity.</param>
    /// <param name="target">Segment entity.</param>
    public XmiHasSegment(
        XmiBaseEntity source,
        XmiBaseEntity target
    ) : base(source, target, nameof(XmiHasSegment))
    {
    }
}
