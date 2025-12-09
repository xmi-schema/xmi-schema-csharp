using XmiSchema.Entities.Bases;
using XmiSchema.Entities.StructuralAnalytical;

namespace XmiSchema.Entities.Relationships;

/// <summary>
/// Connects an entity to its structural node representation in the analytical model.
/// </summary>
public class XmiHasStructuralPointConnection : XmiBaseRelationship
{
    /// <summary>
    /// Creates a structural node relationship with explicit identifiers.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <param name="source">Entity that references the node.</param>
    /// <param name="target">Node entity.</param>
    /// <param name="name">Label for the relationship.</param>
    /// <param name="description">Notes describing the link.</param>
    /// <param name="entityName">Serialized entity name.</param>
    public XmiHasStructuralPointConnection(
        string id,
        XmiBaseStructuralAnalyticalEntity source,
        XmiStructuralPointConnection target,
        string name,
        string description,
        string entityName
    ) : base(id, source, target, name, description, nameof(XmiHasStructuralPointConnection))
    {
    }

    /// <summary>
    /// Generates a structural node relationship with default labeling.
    /// </summary>
    /// <param name="source">Entity that references the node.</param>
    /// <param name="target">Node entity.</param>
    public XmiHasStructuralPointConnection(
        XmiBaseStructuralAnalyticalEntity source,
        XmiStructuralPointConnection target
    ) : base(source, target, nameof(XmiHasStructuralPointConnection))
    {
    }
}
