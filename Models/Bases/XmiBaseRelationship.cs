using XmiSchema.Core.Entities;

namespace XmiSchema.Core.Relationships;

/// <summary>
/// Base class for all relationships in the XMI schema model.
/// Represents directed edges in the graph structure connecting entities.
/// </summary>
/// <remarks>
/// Relationships define connections between entities in the built environment data model.
/// Each relationship has a source entity and a target entity, forming a directed edge in the graph.
/// This enables modeling complex dependencies and associations in structural engineering data.
/// </remarks>
public class XmiBaseRelationship
{
    /// <summary>
    /// Gets or sets the unique identifier for this relationship.
    /// </summary>
    /// <remarks>
    /// This is the primary identifier used throughout the system to reference this relationship.
    /// It must be unique within the model.
    /// </remarks>
    public string ID { get; set; }

    /// <summary>
    /// Gets or sets the source entity of this relationship.
    /// </summary>
    /// <remarks>
    /// The source entity is the starting point of the directed relationship edge.
    /// For example, in a "Member has CrossSection" relationship, the member is the source.
    /// </remarks>
    public XmiBaseEntity Source { get; set; }

    /// <summary>
    /// Gets or sets the target entity of this relationship.
    /// </summary>
    /// <remarks>
    /// The target entity is the ending point of the directed relationship edge.
    /// For example, in a "Member has CrossSection" relationship, the cross-section is the target.
    /// </remarks>
    public XmiBaseEntity Target { get; set; }

    /// <summary>
    /// Gets or sets the display name of this relationship.
    /// </summary>
    /// <remarks>
    /// If not provided, defaults to "Unnamed".
    /// This provides a human-readable description of the relationship type.
    /// </remarks>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a textual description of this relationship.
    /// </summary>
    /// <remarks>
    /// Provides additional context about the nature and purpose of this relationship.
    /// </remarks>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the type discriminator for this relationship.
    /// </summary>
    /// <remarks>
    /// Used for type identification during serialization and deserialization.
    /// Defaults to the class name if not explicitly provided.
    /// </remarks>
    public string EntityType { get; set; }

    /// <summary>
    /// Gets or sets the UML relationship type.
    /// </summary>
    /// <remarks>
    /// Specifies the nature of the relationship using UML terminology (e.g., "Composition", "Association", "Aggregation").
    /// This helps in understanding the semantic meaning and lifecycle implications of the relationship.
    /// </remarks>
    public string UmlType { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmiBaseRelationship"/> class with full parameters.
    /// </summary>
    /// <param name="id">The unique identifier for the relationship.</param>
    /// <param name="source">The source entity of the relationship.</param>
    /// <param name="target">The target entity of the relationship.</param>
    /// <param name="name">The display name. If null or empty, defaults to "Unnamed".</param>
    /// <param name="description">A textual description of the relationship.</param>
    /// <param name="entityType">The type discriminator. If null or empty, defaults to "XmiBaseRelationship".</param>
    /// <param name="umlTtype">The UML relationship type (e.g., "Composition", "Association").</param>
    public XmiBaseRelationship(
        string id,
        XmiBaseEntity source,
        XmiBaseEntity target,
        string name,
        string description,
        string entityType,
        string umlTtype
    )
    {
        ID = id;
        Source = source;
        Target = target;
        Name = string.IsNullOrEmpty(name) ? "Unnamed" : name;
        Description = string.IsNullOrEmpty(description) ? "" : description;
        EntityType = string.IsNullOrEmpty(entityType) ? nameof(XmiBaseRelationship) : entityType;
        UmlType = string.IsNullOrEmpty(umlTtype) ? "" : umlTtype;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmiBaseRelationship"/> class with auto-generated ID.
    /// </summary>
    /// <param name="source">The source entity of the relationship.</param>
    /// <param name="target">The target entity of the relationship.</param>
    /// <param name="entityType">The type discriminator for this relationship.</param>
    /// <param name="umlType">The UML relationship type (e.g., "Composition", "Association").</param>
    /// <remarks>
    /// This constructor automatically generates a GUID for the relationship ID and uses the entity type as the name.
    /// </remarks>
    public XmiBaseRelationship(XmiBaseEntity source, XmiBaseEntity target, string entityType, string umlType)
            : this(Guid.NewGuid().ToString(), source, target, entityType, "", entityType, umlType)
    {
    }
}
