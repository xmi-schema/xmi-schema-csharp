using XmiSchema.Entities.Bases;

namespace XmiSchema.Entities.Relationships;

/// <summary>
/// Associates two entities along a line relationship, typically between members and line geometry references.
/// </summary>
public class XmiHasLine3d : XmiBaseRelationship
{
    /// <summary>
    /// Creates a relationship that is fully configured for serialization.
    /// </summary>
    /// <param name="id">Unique relationship identifier.</param>
    /// <param name="source">Owning entity.</param>
    /// <param name="target">Related entity.</param>
    /// <param name="name">Label for the edge.</param>
    /// <param name="description">Additional notes.</param>
    /// <param name="entityName">Serialized entity name.</param>
    public XmiHasLine3d(
        string id,
        XmiBaseEntity source,
        XmiBaseEntity target,
        string name,
        string description,
        string entityName
    ) : base(id, source, target, name, description, nameof(XmiHasLine3d))
    {
    }

    /// <summary>
    /// Creates a lightweight relationship with auto-generated identifier.
    /// </summary>
    /// <param name="source">Owning entity.</param>
    /// <param name="target">Related entity.</param>
    public XmiHasLine3d(
        XmiBaseEntity source,
        XmiBaseEntity target
    ) : base(source, target, nameof(XmiHasLine3d))
    {
    }
}
