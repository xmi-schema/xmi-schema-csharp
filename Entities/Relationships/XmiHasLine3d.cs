using XmiSchema.Entities.Bases;
using XmiSchema.Entities.Geometries;

namespace XmiSchema.Entities.Relationships;

/// <summary>
/// Associates two entities along a line relationship, typically between members and line geometry references.
/// </summary>
public class XmiHasLine3d : XmiBaseRelationship
{
    /// <summary>
    /// Creates a relationship that is fully configured for serialization.
    /// </summary>
    /// <param name="id">Unique relationship identifier.</param>
    /// <param name="source">Owning entity.</param>
    /// <param name="target">Related entity.</param>
    /// <param name="name">Label for the edge.</param>
    /// <param name="description">Additional notes.</param>
    /// <param name="entityName">Serialized entity name.</param>
    public XmiHasLine3d(
        string id,
        XmiBaseEntity source,
        XmiLine3d target,
        string name,
        string description,
        string entityName
    ) : base(id, source, target, name, description, nameof(XmiHasLine3d))
    {
    }

    /// <summary>
    /// Creates a lightweight relationship with auto-generated identifier.
    /// </summary>
    /// <param name="source">Owning entity.</param>
    /// <param name="target">Related entity.</param>
    public XmiHasLine3d(
        XmiBaseEntity source,
        XmiLine3d target
    ) : base(source, target, nameof(XmiHasLine3d))
    {
    }

    /// <summary>
    /// Gets the target as a strongly-typed <see cref="XmiLine3d"/>.
    /// </summary>
    public XmiLine3d TargetLine3d => (XmiLine3d)Target;

    /// <summary>
    /// Checks if this relationship's target references the exact same
    /// <see cref="XmiPoint3d"/> instances as the other relationship's target.
    /// Use for graph connectivity checks.
    /// </summary>
    /// <param name="other">The relationship to compare against.</param>
    /// <returns>True if both targets share the same point references.</returns>
    public bool HasSameTarget(XmiHasLine3d? other)
    {
        if (other == null) return false;
        return TargetLine3d.Equals(other.TargetLine3d);
    }

    /// <summary>
    /// Checks if this relationship's target has the same coordinates
    /// AND the same vector direction as the other relationship's target.
    /// Use when direction/vector matters (e.g., load direction).
    /// </summary>
    /// <param name="other">The relationship to compare against.</param>
    /// <returns>True if targets have same coordinates and direction.</returns>
    public bool HasDirectionallyEqualTarget(XmiHasLine3d? other)
    {
        if (other == null) return false;
        return TargetLine3d.IsDirectionallyEqual(other.TargetLine3d);
    }

    /// <summary>
    /// Checks if this relationship's target occupies the same space
    /// as the other relationship's target, regardless of direction.
    /// A→B is considered coincident with B→A.
    /// Use for geometry deduplication.
    /// </summary>
    /// <param name="other">The relationship to compare against.</param>
    /// <returns>True if targets occupy the same space.</returns>
    public bool HasCoincidentTarget(XmiHasLine3d? other)
    {
        if (other == null) return false;
        return TargetLine3d.IsCoincident(other.TargetLine3d);
    }
}
