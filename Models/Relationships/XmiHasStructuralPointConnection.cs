using XmiSchema.Core.Entities;

namespace XmiSchema.Core.Relationships;

/// <summary>
/// Connects an entity to its structural node representation in the analytical model.
/// </summary>
public class XmiHasStructuralPointConnection : XmiBaseRelationship
{
    /// <summary>
    /// Specifies the role of this connection (e.g., "BeginNode", "EndNode", "IntermediateNode").
    /// </summary>
    public string? ConnectionType { get; set; }

    /// <summary>
    /// Creates a structural node relationship with explicit identifiers.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <param name="source">Entity that references the node.</param>
    /// <param name="target">Node entity.</param>
    /// <param name="name">Label for the relationship.</param>
    /// <param name="description">Notes describing the link.</param>
    /// <param name="entityName">Serialized entity name.</param>
    /// <param name="connectionType">Role of this connection (e.g., "BeginNode", "EndNode").</param>
    public XmiHasStructuralPointConnection(
        string id,
        XmiBaseStructuralAnalyticalEntity source,
        XmiBaseStructuralAnalyticalEntity target,
        string name,
        string description,
        string entityName,
        string? connectionType = null
    ) : base(id, source, target, name, description, nameof(XmiHasStructuralPointConnection))
    {
        ConnectionType = connectionType;
    }

    /// <summary>
    /// Generates a structural node relationship with default labeling.
    /// </summary>
    /// <param name="source">Entity that references the node.</param>
    /// <param name="target">Node entity.</param>
    /// <param name="connectionType">Role of this connection (e.g., "BeginNode", "EndNode").</param>
    public XmiHasStructuralPointConnection(
        XmiBaseStructuralAnalyticalEntity source,
        XmiBaseStructuralAnalyticalEntity target,
        string? connectionType = null
    ) : base(source, target, nameof(XmiHasStructuralPointConnection))
    {
        ConnectionType = connectionType;
    }
}
