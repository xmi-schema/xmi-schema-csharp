using XmiSchema.Models.Bases;


namespace XmiSchema.Models.Relationships;

/// <summary>
/// Associates entities, such as point connections, to the storey they belong to.
/// </summary>
public class XmiHasStorey : XmiBaseRelationship
{
    /// <summary>
    /// Creates a storey relationship capturing all serialization metadata.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <param name="source">Entity positioned on the storey.</param>
    /// <param name="target">Storey entity.</param>
    /// <param name="name">Label for the relationship.</param>
    /// <param name="description">Notes describing the link.</param>
    /// <param name="entityName">Serialized entity name.</param>
    public XmiHasStorey(
        string id,
        XmiBaseEntity source,
        XmiBaseEntity target,
        string name,
        string description,
        string entityName
    ) : base(id, source, target, name, description, nameof(XmiHasStorey))
    {
    }

    /// <summary>
    /// Generates a minimal storey relationship with auto identifier.
    /// </summary>
    /// <param name="source">Entity positioned on the storey.</param>
    /// <param name="target">Storey entity.</param>
    public XmiHasStorey(
        XmiBaseEntity source,
        XmiBaseEntity target
    ) : base(source, target, nameof(XmiHasStorey))
    {
    }
}
