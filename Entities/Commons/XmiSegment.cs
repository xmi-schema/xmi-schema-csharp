using System.Linq;
using XmiSchema.Entities.Bases;
using XmiSchema.Enums;

namespace XmiSchema.Entities.Commons;

/// <summary>
/// Represents a logical segment within a structural curve member, including its position and shape classification.
/// </summary>
public class XmiSegment : XmiBaseEntity
{

    // public XmiBaseGeometry Geometry { get; set; }   // Surface member the support is assigned to
    public int Position { get; set; }
    public XmiSegmentTypeEnum SegmentType { get; set; }

    /// <summary>
    /// Gets whether the position value is valid (non-negative integer).
    /// </summary>
    public bool IsValidPosition => Position >= 0;

    // 带参数构造函数（包含父类属性 + 子类属性）
    /// <summary>
    /// Creates a new <see cref="XmiSegment"/> tied to a specific parent curve member.
    /// </summary>
    /// <param name="id">Unique identifier inside the Cross Model Information graph.</param>
    /// <param name="name">Readable label; uses <paramref name="id"/> when omitted.</param>
    /// <param name="ifcGuid">IFC GUID that links to the originating BIM element.</param>
    /// <param name="nativeId">Source identifier from the authoring system.</param>
    /// <param name="description">Free-form notes about the segment.</param>
    /// <param name="position">Integer position in the segment sequence (0, 1, 2, etc.).</param>
    /// <param name="segmentType">Geometric definition for downstream consumers.</param>
    public XmiSegment(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        // XmiBaseGeometry geometry,
        int position,
        // XmiStructuralPointConnection beginNode,
        // XmiStructuralPointConnection endNode,
        XmiSegmentTypeEnum segmentType
    ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiSegment), XmiBaseEntityDomainEnum.Shared)
    {
        Position = position < 0 ? 0 : position;
        SegmentType = segmentType;
    }

    /// <summary>
    /// Validates that a collection of segments has proper position sequencing.
    /// Segments should be ordered by position from 0, 1, 2, etc. without gaps.
    /// </summary>
    /// <param name="segments">Collection of segments to validate.</param>
    /// <returns>True if segments are properly sequenced, false otherwise.</returns>
    public static bool ValidateSequence(System.Collections.Generic.List<XmiSegment> segments)
    {
        if (segments == null || segments.Count < 2) return true;
        
        // Check for invalid positions first
        foreach (var segment in segments)
        {
            if (!segment.IsValidPosition)
                return false;
        }
        
        // Check that segments are in ascending order by position
        for (int i = 0; i < segments.Count - 1; i++)
        {
            if (segments[i].Position > segments[i + 1].Position)
                return false;
        }
        return true;
    }

    /// <summary>
    /// Sorts a collection of segments by their position in ascending order.
    /// </summary>
    /// <param name="segments">Collection of segments to sort.</param>
    /// <returns>New list with segments sorted by position.</returns>
    public static System.Collections.Generic.List<XmiSegment> SortByPosition(System.Collections.Generic.List<XmiSegment> segments)
    {
        if (segments == null) return new System.Collections.Generic.List<XmiSegment>();
        return segments.OrderBy(s => s.Position).ToList();
    }

    /// <summary>
    /// Validates that segments can form a closed boundary for a surface member.
    /// For a closed boundary, we typically need at least 3 segments.
    /// </summary>
    /// <param name="segments">Collection of segments to validate.</param>
    /// <returns>True if segments can form a closed boundary, false otherwise.</returns>
    public static bool CanFormClosedBoundary(System.Collections.Generic.List<XmiSegment> segments)
    {
        if (segments == null || segments.Count < 3) return false;
        
        // Check that all segments have valid positions
        foreach (var segment in segments)
        {
            if (!segment.IsValidPosition)
                return false;
        }
        
        return true;
    }
}
