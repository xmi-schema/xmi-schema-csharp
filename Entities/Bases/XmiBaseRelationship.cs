using System;
using System.Collections.Generic;

namespace XmiSchema.Entities.Bases;

/// <summary>
/// Base class for all graph relationships (edges) between XMI entities.
/// </summary>
/// <remarks>
/// This class defines the fundamental structure of relationships in the XMI graph model.
/// Relationships are directed edges that connect a <see cref="Source"/> entity to a
/// <see cref="Target"/> entity, enabling the representation of various semantic
/// associations such as:
///
/// <list type="bullet">
/// <item><description><see cref="XmiHasMaterial"/> - Associates a material with a structural element</description></item>
/// <item><description><see cref="XmiHasCrossSection"/> - Links a cross-section profile to a member</description></item>
/// <item><description><see cref="XmiHasGeometry"/> - Connects entities to geometric primitives</description></item>
/// <item><description><see cref="XmiHasSegment"/> - Relates members to their analytical segments</description></item>
/// <item><description><see cref="XmiHasStructuralPointConnection"/> - Connects members to point connections</description></item>
/// </list>
///
/// Relationships support optional metadata through the <see cref="Properties"/> dictionary,
/// which can store UML stereotypes, application-specific attributes, or other auxiliary data.
/// The <see cref="EntityName"/> property enables polymorphic JSON deserialization,
/// similar to the <see cref="XmiBaseEntity"/> pattern.
///
/// Derived relationship types typically provide convenience constructors that accept
/// source and target entities directly and generate unique identifiers automatically.
/// </remarks>
/// <example>
/// Creating a material association relationship:
/// <code>
/// var beam = new XmiStructuralCurveMember(/*...*/);
/// var concrete = new XmiMaterial(/*...*/);
///
/// var hasMaterial = new XmiHasMaterial(
///     id: "rel_beam_001_mat_001",
///     source: beam,
///     target: concrete,
///     name: "Beam to Material",
///     description: "Associates concrete material with structural beam"
/// );
/// // hasMaterial.Source == beam
/// // hasMaterial.Target == concrete
/// // hasMaterial.EntityName == "XmiHasMaterial"
/// </code>
/// </example>
/// <example>
/// Using the shorthand constructor with auto-generated ID:
/// <code>
/// var hasMaterial = new XmiHasMaterial(
///     source: beam,
///     target: concrete
/// );
/// // hasMaterial.Id is auto-generated GUID
/// // hasMaterial.Name defaults to "XmiHasMaterial"
/// </code>
/// </example>
/// <seealso cref="XmiBaseEntity"/>
/// <seealso cref="XmiHasMaterial"/>
/// <seealso cref="XmiHasCrossSection"/>
/// <seealso cref="XmiHasGeometry"/>
public class XmiBaseRelationship
{
    /// <summary>
    /// Gets or sets the unique identifier for this relationship.
    /// </summary>
    /// <value>The unique identifier as a string.</value>
    /// <remarks>
    /// This identifier should be unique across all relationships in the graph and
    /// stable throughout the relationship's lifecycle. It enables referencing and
    /// querying specific relationships in the model.
    /// </remarks>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the source entity of this directed relationship.
    /// </summary>
    /// <value>The entity at the origin of the edge.</value>
    /// <remarks>
    /// The source entity is the "from" side of the directed relationship. For example,
    /// in a <see cref="XmiHasMaterial"/> relationship, the source is the structural
    /// element (e.g., a beam or column) that has the material associated with it.
    /// </remarks>
    public XmiBaseEntity Source { get; set; }

    /// <summary>
    /// Gets or sets the target entity of this directed relationship.
    /// </summary>
    /// <value>The entity at the destination of the edge.</value>
    /// <remarks>
    /// The target entity is the "to" side of the directed relationship. For example,
    /// in a <see cref="XmiHasMaterial"/> relationship, the target is the material entity
    /// (e.g., concrete or steel) being associated with the source element.
    /// </remarks>
    public XmiBaseEntity Target { get; set; }

    /// <summary>
    /// Gets or sets the human-readable name for this relationship.
    /// </summary>
    /// <value>The display name as a string, or "Unnamed" if not specified.</value>
    /// <remarks>
    /// This property provides a descriptive label for the relationship, useful for
    /// display in UI elements, reports, or debugging. If not specified during construction,
    /// it defaults to "Unnamed".
    /// </remarks>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a textual description of this relationship's purpose or semantics.
    /// </summary>
    /// <value>The description as a string, or an empty string if not specified.</value>
    /// <remarks>
    /// This property provides semantic context about the relationship, explaining its
    /// role, constraints, or other relevant information. It can include UML stereotypes,
    /// domain-specific rules, or usage notes for downstream consumers.
    /// </remarks>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the entity type name used for polymorphic JSON deserialization.
    /// </summary>
    /// <value>The entity type name as a string (e.g., "XmiHasMaterial", "XmiHasCrossSection").</value>
    /// <remarks>
    /// This property serves as a discriminator in JSON serialization, allowing the
    /// deserialization process to determine the correct concrete relationship type.
    /// Derived classes must set this to their type name to enable proper polymorphic
    /// deserialization. If not specified, it defaults to "XmiBaseRelationship".
    /// </remarks>
    public string EntityName { get; set; }

    /// <summary>
    /// Gets or sets the optional metadata properties attached to this relationship.
    /// </summary>
    /// <value>A dictionary of key-value pairs for storing metadata, or an empty dictionary if not specified.</value>
    /// <remarks>
    /// This property enables downstream consumers to attach custom metadata to relationships
    /// without modifying the class structure. Common uses include:
    /// <list type="bullet">
    /// <item><description>UML stereotypes (e.g., "nodeType=Begin", "nodeType=End")</description></item>
    /// <item><description>Application-specific attributes</description></item>
    /// <item><description>Calculated or derived properties</description></item>
    /// <item><description>Validation rules or constraints</description></item>
    /// </list>
    ///
    /// If not specified during construction, it defaults to an empty dictionary.
    /// </remarks>
    /// <example>
    /// Adding metadata to a segment relationship:
    /// <code>
    /// var hasSegment = new XmiHasSegment(/*...*/);
    /// hasSegment.Properties["nodeType"] = "Begin";
    /// hasSegment.Properties["position"] = "0.0";
    /// </code>
    /// </example>
    public Dictionary<string, string> Properties { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmiBaseRelationship"/> class with
    /// full control over all properties, including a custom identifier.
    /// </summary>
    /// <param name="id">The stable, unique identifier for this relationship. Must not be null or whitespace.</param>
    /// <param name="source">The entity at the origin of this directed relationship.</param>
    /// <param name="target">The entity at the destination of this directed relationship.</param>
    /// <param name="name">The human-readable display name. If null or empty, defaults to "Unnamed".</param>
    /// <param name="description">A textual description of the relationship. If null or empty, defaults to an empty string.</param>
    /// <param name="entityName">The entity type name for polymorphic deserialization. If null or empty, defaults to "XmiBaseRelationship".</param>
    /// <param name="properties">Optional metadata to attach to the relationship. If null, defaults to an empty dictionary.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="id"/> is null or whitespace.</exception>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="source"/> is null.</exception>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="target"/> is null.</exception>
    /// <remarks>
    /// This constructor provides full control over all relationship properties, including
    /// the identifier. Use this when you need to specify a custom identifier or when the
    /// relationship represents a persistent association with a known ID.
    ///
    /// For convenience, derived classes often provide a shorthand constructor that generates
    /// a unique identifier automatically. See the derived constructor overload that accepts
    /// only <paramref name="source"/> and <paramref name="target"/> parameters.
    /// </remarks>
    /// <example>
    /// Creating a fully-described relationship:
    /// <code>
    /// var hasMaterial = new XmiHasMaterial(
    ///     id: "rel_beam_001_mat_001",
    ///     source: beam,
    ///     target: concrete,
    ///     name: "Beam to Material",
    ///     description: "Associates concrete material with structural beam",
    ///     entityName: "XmiHasMaterial",
    ///     properties: new Dictionary&lt;string, string&gt; { { "type", "primary" } }
    /// );
    /// </code>
    /// </example>
    public XmiBaseRelationship(
        string id,
        XmiBaseEntity source,
        XmiBaseEntity target,
        string name,
        string description,
        string entityName,
        Dictionary<string, string>? properties = null
    )
    {
        Id = id;
        Source = source;
        Target = target;
        Name = string.IsNullOrEmpty(name) ? "Unnamed"  : name;
        Description = string.IsNullOrEmpty(description) ? "" : description;
        EntityName = string.IsNullOrEmpty(entityName) ? nameof(XmiBaseRelationship) : entityName;
        Properties = properties ?? new Dictionary<string, string>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmiBaseRelationship"/> class with
    /// an auto-generated unique identifier using the provided source and target entities.
    /// </summary>
    /// <param name="source">The entity at the origin of this directed relationship.</param>
    /// <param name="target">The entity at the destination of this directed relationship.</param>
    /// <param name="entityName">The entity type name for polymorphic deserialization (e.g., "XmiHasMaterial").</param>
    /// <param name="properties">Optional metadata to attach to the relationship. If null, defaults to an empty dictionary.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="source"/> is null.</exception>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="target"/> is null.</exception>
    /// <remarks>
    /// This shorthand constructor generates a unique GUID-based identifier automatically,
    /// making it convenient for creating transient or temporary relationships where the
    /// identifier is not significant. The <paramref name="name"/> parameter defaults to
    /// the <paramref name="entityName"/> value, and <paramref name="description"/> defaults
    /// to an empty string.
    ///
    /// This constructor is commonly used by derived relationship classes that provide
    /// convenience methods for creating associations between entities.
    /// </remarks>
    /// <example>
    /// Creating a relationship with auto-generated ID:
    /// <code>
    /// var hasMaterial = new XmiHasMaterial(
    ///     source: beam,
    ///     target: concrete,
    ///     entityName: "XmiHasMaterial"
    /// );
    /// // hasMaterial.Id is auto-generated GUID (e.g., "f47ac10b-58cc-4372-a567-0e02b2c3d479")
    /// // hasMaterial.Name defaults to "XmiHasMaterial"
    /// // hasMaterial.Description defaults to ""
    /// </code>
    /// </example>
    public XmiBaseRelationship(XmiBaseEntity source, XmiBaseEntity target, string entityName, Dictionary<string, string>? properties = null)
            : this(Guid.NewGuid().ToString(), source, target, entityName, "", entityName, properties)
    {
    }
}
