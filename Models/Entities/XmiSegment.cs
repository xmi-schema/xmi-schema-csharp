using XmiSchema.Core.Enums;
using XmiSchema.Core.Geometries;


namespace XmiSchema.Core.Entities;

public class XmiSegment : XmiBaseEntity
{

    // public XmiBaseGeometry Geometry { get; set; }   // Surface member the support is assigned to
    public float Position { get; set; }
    // public XmiStructuralPointConnection BeginNode { get; set; }
    // public XmiStructuralPointConnection EndNode { get; set; }
    public XmiSegmentTypeEnum SegmentType { get; set; }


    // 带参数构造函数（包含父类属性 + 子类属性）
    public XmiSegment(
        string id,
        string name,
        string ifcguid,
        string nativeId,
        string description,
        // XmiBaseGeometry geometry,
        float position,
        // XmiStructuralPointConnection beginNode,
        // XmiStructuralPointConnection endNode,
        XmiSegmentTypeEnum segmentType
    ) : base(id, name, ifcguid, nativeId, description, nameof(XmiSegment))
    {
        // Geometry = geometry;
        Position = position;
        // BeginNode = beginNode;

        // EndNode = endNode;
        SegmentType = segmentType;
    }
}
