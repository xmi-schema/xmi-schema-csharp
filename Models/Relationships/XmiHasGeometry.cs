using XmiSchema.Models.Bases;

namespace XmiSchema.Models.Relationships;

/// <summary>
/// Links an entity to its geometric representation, typically associating members to lines or arcs.
/// </summary>
public class XmiHasGeometry : XmiBaseRelationship
{
    /// <summary>
    /// Creates a fully-described geometry relationship.
    /// </summary>
    /// <param name="id">Unique identifier for the relationship edge.</param>
    /// <param name="source">Entity that owns the geometry.</param>
    /// <param name="target">Geometry entity.</param>
    /// <param name="name">Descriptive label.</param>
    /// <param name="description">Optional notes.</param>
    /// <param name="entityName">Type name emitted in serialized payloads.</param>
    public XmiHasGeometry(
        string id,
        XmiBaseEntity source,
        XmiBaseGeometry target,
        string name,
        string description,
        string entityName
    ) : base(id, source, target, name, description, nameof(XmiHasGeometry))
    {
    }

    /// <summary>
    /// Creates a minimal geometry relationship; the constructor generates a unique identifier.
    /// </summary>
    /// <param name="source">Entity that owns the geometry.</param>
    /// <param name="target">Geometry entity.</param>
    public XmiHasGeometry(
        XmiBaseEntity source,
        XmiBaseGeometry target
    ) : base(source, target, nameof(XmiHasGeometry))
    {
    }
}
