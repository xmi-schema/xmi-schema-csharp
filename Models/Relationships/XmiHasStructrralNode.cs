using XmiSchema.Core.Entities;

namespace XmiSchema.Core.Relationships;

/// <summary>
/// Connects an entity to its structural node representation in the analytical model.
/// </summary>
public class XmiHasStructuralNode : XmiBaseRelationship
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
    /// <param name="umlType">UML stereotype.</param>
    public XmiHasStructuralNode(
        string id,
        XmiBaseEntity source,
        XmiBaseEntity target,
        string name,
        string description,
        string entityName,
        string umlType
    ) : base(id, source, target, name, description, nameof(XmiHasStructuralNode), "Association")
    {
    }

    /// <summary>
    /// Generates a structural node relationship with default labeling.
    /// </summary>
    /// <param name="source">Entity that references the node.</param>
    /// <param name="target">Node entity.</param>
    public XmiHasStructuralNode(
        XmiBaseEntity source,
        XmiBaseEntity target
    ) : base(source, target, nameof(XmiHasStructuralNode), "Association")
    {
    }
}
