using XmiSchema.Core.Entities;


namespace XmiSchema.Core.Relationships;

/// <summary>
/// Connects a structural entity to a point reference, typically used for structural nodes.
/// </summary>
public class XmiHasPoint3D : XmiBaseRelationship
{
    /// <summary>
    /// Creates a descriptive point relationship record.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <param name="source">Entity that owns the point.</param>
    /// <param name="target">Target entity containing the coordinate (usually <see cref="XmiPoint3D"/>).</param>
    /// <param name="name">Label for the edge.</param>
    /// <param name="description">Notes explaining the binding.</param>
    /// <param name="entityName">Serialized entity type name.</param>
    public XmiHasPoint3D(
        string id,
        XmiBaseEntity source,
        XmiBaseEntity target,
        string name,
        string description,
        string entityName
    ) : base(id, source, target, name, description, nameof(XmiHasPoint3D))
    {
    }

    /// <summary>
    /// Creates a minimal point relationship, generating the identifier automatically.
    /// </summary>
    /// <param name="source">Entity that owns the point.</param>
    /// <param name="target">Target entity containing the coordinate.</param>
    public XmiHasPoint3D(
        XmiBaseEntity source,
        XmiBaseEntity target
    ) : base(source, target, nameof(XmiHasPoint3D))
    {
    }
}
