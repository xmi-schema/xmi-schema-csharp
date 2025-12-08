using XmiSchema.Entities.Bases;

namespace XmiSchema.Entities.Relationships;

/// <summary>
/// Associates an entity with an arc geometry reference.
/// </summary>
public class XmiHasArc3d : XmiBaseRelationship
{
    /// <summary>
    /// Creates a relationship with explicit identifiers and labels.
    /// </summary>
    public XmiHasArc3d(
        string id,
        XmiBaseEntity source,
        XmiBaseEntity target,
        string name,
        string description,
        string entityName
    ) : base(id, source, target, name, description, nameof(XmiHasArc3d))
    {
    }

    /// <summary>
    /// Creates a lightweight relationship with auto-generated identifier.
    /// </summary>
    public XmiHasArc3d(
        XmiBaseEntity source,
        XmiBaseEntity target
    ) : base(source, target, nameof(XmiHasArc3d))
    {
    }
}
