using System;
using System.Collections.Generic;

namespace XmiSchema.Models.Bases;

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
    /// <summary>
    /// Optional metadata bag for downstream consumers (e.g., nodeType=Begin/End).
    /// </summary>
    public Dictionary<string, string> Properties { get; set; }
    /// <summary>
    /// Initializes a fully-described relationship.
    /// </summary>
    /// <param name="id">Unique identifier for the relationship.</param>
    /// <param name="source">Entity at the origin of the edge.</param>
    /// <param name="target">Entity at the destination of the edge.</param>
    /// <param name="name">Readable label.</param>
    /// <param name="description">Notes for downstream consumers.</param>
    /// <param name="entityType">Type name recorded in the payload.</param>
    /// <param name="properties">Optional metadata to attach to the relationship.</param>
    public XmiBaseRelationship(
        string id,
        XmiBaseEntity source,
        XmiBaseEntity target,
        string name,
        string description,
        string entityType,
        Dictionary<string, string>? properties = null
    )
    {
        Id = id;
        Source = source;
        Target = target;
        Name = string.IsNullOrEmpty(name) ? "Unnamed"  : name;
        Description = string.IsNullOrEmpty(description) ? "" : description;
        EntityType = string.IsNullOrEmpty(entityType) ? nameof(XmiBaseRelationship) : entityType;
        Properties = properties ?? new Dictionary<string, string>();
    }
    /// <summary>
    /// Generates a relationship with a new identifier using the provided endpoints.
    /// </summary>
    /// <param name="source">Entity at the origin of the edge.</param>
    /// <param name="target">Entity at the destination of the edge.</param>
    /// <param name="entityType">Type name recorded in the payload.</param>
    /// <param name="properties">Optional metadata to attach to the relationship.</param>
    public XmiBaseRelationship(XmiBaseEntity source, XmiBaseEntity target, string entityType, Dictionary<string, string>? properties = null)
            : this(Guid.NewGuid().ToString(), source, target, entityType, "", entityType, properties)
    {
    }
}
