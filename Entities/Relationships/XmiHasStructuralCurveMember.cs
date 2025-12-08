using XmiSchema.Entities.Bases;

namespace XmiSchema.Entities.Relationships;

/// <summary>
/// Links physical elements to their structural curve member analytical representation.
/// </summary>
public class XmiHasStructuralCurveMember : XmiBaseRelationship
{
    /// <summary>
    /// Creates a structural curve member relationship with explicit metadata.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <param name="source">Physical entity (e.g., XmiBeam, XmiColumn).</param>
    /// <param name="target">Structural analytical curve member entity.</param>
    /// <param name="name">Relationship label.</param>
    /// <param name="description">Notes describing the assignment.</param>
    /// <param name="entityName">Serialized entity name.</param>
    public XmiHasStructuralCurveMember(
        string id,
        XmiBasePhysicalEntity source,
        XmiBaseStructuralAnalyticalEntity target,
        string name,
        string description,
        string entityName
    ) : base(id, source, target, name, description, nameof(XmiHasStructuralCurveMember))
    {
    }

    /// <summary>
    /// Generates a basic structural curve member relationship with automatic identifier.
    /// </summary>
    /// <param name="source">Physical entity (e.g., XmiBeam, XmiColumn).</param>
    /// <param name="target">Structural analytical curve member entity.</param>
    public XmiHasStructuralCurveMember(
        XmiBasePhysicalEntity source,
        XmiBaseStructuralAnalyticalEntity target
    ) : base(source, target, nameof(XmiHasStructuralCurveMember))
    {
    }
}
