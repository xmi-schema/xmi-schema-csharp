using XmiSchema.Models.Bases;
using XmiSchema.Models.Enums;


namespace XmiSchema.Models.Commons;

/// <summary>
/// Represents a logical segment within a structural curve member, including its position and shape classification.
/// </summary>
public class XmiSegment : XmiBaseEntity
{

    // public XmiBaseGeometry Geometry { get; set; }   // Surface member the support is assigned to
    public float Position { get; set; }
    // public XmiStructuralPointConnection BeginNode { get; set; }
    // public XmiStructuralPointConnection EndNode { get; set; }
    public XmiSegmentTypeEnum SegmentType { get; set; }


    // 带参数构造函数（包含父类属性 + 子类属性）
    /// <summary>
    /// Creates a new <see cref="XmiSegment"/> tied to a specific parent curve member.
    /// </summary>
    /// <param name="id">Unique identifier inside the Cross Model Information graph.</param>
    /// <param name="name">Readable label; uses <paramref name="id"/> when omitted.</param>
    /// <param name="ifcGuid">IFC GUID that links to the originating BIM element.</param>
    /// <param name="nativeId">Source identifier from the authoring system.</param>
    /// <param name="description">Free-form notes about the segment.</param>
    /// <param name="position">Normalized position value along the parent member (0-1).</param>
    /// <param name="segmentType">Geometric definition for downstream consumers.</param>
    public XmiSegment(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        // XmiBaseGeometry geometry,
        float position,
        // XmiStructuralPointConnection beginNode,
        // XmiStructuralPointConnection endNode,
        XmiSegmentTypeEnum segmentType
    ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiSegment), XmiBaseEntityDomainEnum.Shared)
    {
        // Geometry = geometry;
        Position = position;
        // BeginNode = beginNode;

        // EndNode = endNode;
        SegmentType = segmentType;
    }
}
