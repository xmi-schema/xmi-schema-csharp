using XmiSchema.Core.Entities;

namespace XmiSchema.Core.Relationships;

/// <summary>
/// Base class that carries the shared attributes for relationships between XMI entities.
/// </summary>
public class XmiBaseRelationship
{
    public string Id { get; set; }
    public XmiBaseEntity Source { get; set; }
    public XmiBaseEntity Target { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string EntityType { get; set; }
    public string UmlType { get; set; }
    /// <summary>
    /// Initializes a fully-described relationship.
    /// </summary>
    /// <param name="id">Unique identifier for the relationship.</param>
    /// <param name="source">Entity at the origin of the edge.</param>
    /// <param name="target">Entity at the destination of the edge.</param>
    /// <param name="name">Readable label.</param>
    /// <param name="description">Notes for downstream consumers.</param>
    /// <param name="entityType">Type name recorded in the payload.</param>
    /// <param name="umlType">UML stereotype for the edge.</param>
    public XmiBaseRelationship(
        string id,
        XmiBaseEntity source,
        XmiBaseEntity target,
        string name,
        string description,
        string entityType,
        string umlType
    )
    {
        Id = id;
        Source = source;
        Target = target;
        Name = string.IsNullOrEmpty(name) ? "Unnamed"  : name;
        Description = string.IsNullOrEmpty(description) ? "" : description;
        EntityType = string.IsNullOrEmpty(entityType) ? nameof(XmiBaseRelationship) : entityType;
        UmlType = string.IsNullOrEmpty(umlType)? "": umlType;
    }
    /// <summary>
    /// Generates a relationship with a new identifier using the provided endpoints.
    /// </summary>
    /// <param name="source">Entity at the origin of the edge.</param>
    /// <param name="target">Entity at the destination of the edge.</param>
    /// <param name="entityType">Type name recorded in the payload.</param>
    /// <param name="umlType">UML stereotype for the edge.</param>
    public XmiBaseRelationship(XmiBaseEntity source, XmiBaseEntity target, string entityType, string umlType)
            : this(Guid.NewGuid().ToString(), source, target, entityType, "", entityType, umlType)
    {        
    }
}
