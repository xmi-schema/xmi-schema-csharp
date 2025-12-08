using XmiSchema.Entities.Bases;
using XmiSchema.Enums;

namespace XmiSchema.Entities.Relationships;

/// <summary>
/// Connects a structural entity to a point reference, typically used for structural nodes.
/// </summary>
public class XmiHasPoint3d : XmiBaseRelationship
{
    /// <summary>
    /// Identifies the role of the target point (Start, End, or Center).
    /// </summary>
    public XmiPoint3dTypeEnum? PointType { get; set; }

    /// <summary>
    /// Creates a descriptive point relationship record.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <param name="source">Entity that owns the point.</param>
    /// <param name="target">Target entity containing the coordinate (usually <see cref="XmiPoint3D"/>).</param>
    /// <param name="name">Label for the edge.</param>
    /// <param name="description">Notes explaining the binding.</param>
    /// <param name="entityName">Serialized entity type name.</param>
    /// <param name="pointType">Optional role of the point (Start, End, Center).</param>
    public XmiHasPoint3d(
        string id,
        XmiBaseEntity source,
        XmiBaseEntity target,
        string name,
        string description,
        string entityName,
        XmiPoint3dTypeEnum? pointType = null
    ) : base(id, source, target, name, description, nameof(XmiHasPoint3d))
    {
        PointType = pointType;
    }

    /// <summary>
    /// Creates a minimal point relationship, generating the identifier automatically.
    /// </summary>
    /// <param name="source">Entity that owns the point.</param>
    /// <param name="target">Target entity containing the coordinate.</param>
    /// <param name="pointType">Optional role of the point (Start, End, Center).</param>
    public XmiHasPoint3d(
        XmiBaseEntity source,
        XmiBaseEntity target,
        XmiPoint3dTypeEnum? pointType = null
    ) : base(source, target, nameof(XmiHasPoint3d))
    {
        PointType = pointType;
    }
}
