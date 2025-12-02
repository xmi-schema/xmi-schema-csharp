using XmiSchema.Core.Entities;


namespace XmiSchema.Core.Relationships;

/// <summary>
/// Associates two entities along a line relationship, typically between members and line geometry references.
/// </summary>
public class XmiHasLine3D : XmiBaseRelationship
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
    /// <param name="umlType">UML stereotype classification.</param>
    public XmiHasLine3D(
        string id,
        XmiBaseEntity source,
        XmiBaseEntity target,
        string name,
        string description,
        string entityName,
        string umlType
    ) : base(id, source, target, name, description, nameof(XmiHasLine3D), "Association")
    {
    }

    /// <summary>
    /// Creates a lightweight relationship with auto-generated identifier.
    /// </summary>
    /// <param name="source">Owning entity.</param>
    /// <param name="target">Related entity.</param>
    public XmiHasLine3D(
        XmiBaseEntity source,
        XmiBaseEntity target
    ) : base(source, target, nameof(XmiHasLine3D), "Association")
    {
    }
}
