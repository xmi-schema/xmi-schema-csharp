using System.Linq;
using XmiSchema.Entities.Bases;
using XmiSchema.Enums;

namespace XmiSchema.Entities.Commons;

/// <summary>
/// Represents a logical segment within a structural curve member, including its shape classification.
/// Position is now handled by the XmiHasSegment relationship.
/// </summary>
public class XmiSegment : XmiBaseEntity
{

    // public XmiBaseGeometry Geometry { get; set; }   // Surface member the support is assigned to
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
    /// <param name="segmentType">Geometric definition for downstream consumers.</param>
    public XmiSegment(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        // XmiBaseGeometry geometry,
        // XmiStructuralPointConnection beginNode,
        // XmiStructuralPointConnection endNode,
        XmiSegmentTypeEnum segmentType
    ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiSegment), XmiBaseEntityDomainEnum.Shared)
    {
        SegmentType = segmentType;
    }

    /// <summary>
    /// Validates that a collection of segments has proper position sequencing.
    /// Segments should be ordered by position from 0, 1, 2, etc. without gaps.
    /// </summary>
    /// <param name="segments">Collection of segments to validate.</param>
    /// <param name="positions">Corresponding positions for each segment.</param>
    /// <returns>True if segments are properly sequenced, false otherwise.</returns>
    public static bool ValidateSequence(System.Collections.Generic.List<XmiSegment> segments, System.Collections.Generic.List<int> positions)
    {
        if (segments == null || segments.Count < 2) return true;
        if (positions == null || positions.Count != segments.Count) return false;
        
        // Check for invalid positions first
        foreach (var position in positions)
        {
            if (position < 0)
                return false;
        }
        
        // Check that segments are in ascending order by position
        for (int i = 0; i < positions.Count - 1; i++)
        {
            if (positions[i] > positions[i + 1])
                return false;
        }
        return true;
    }

    /// <summary>
    /// Sorts a collection of segments by their position in ascending order.
    /// </summary>
    /// <param name="segments">Collection of segments to sort.</param>
    /// <param name="positions">Corresponding positions for each segment.</param>
    /// <returns>New list with segments sorted by position.</returns>
    public static System.Collections.Generic.List<XmiSegment> SortByPosition(System.Collections.Generic.List<XmiSegment> segments, System.Collections.Generic.List<int> positions)
    {
        if (segments == null || positions == null || segments.Count != positions.Count) 
            return new System.Collections.Generic.List<XmiSegment>();
        
        var indexedSegments = segments.Zip(positions, (segment, position) => new { segment, position })
                                     .OrderBy(x => x.position)
                                     .Select(x => x.segment)
                                     .ToList();
        return indexedSegments;
    }

    /// <summary>
    /// Validates that segments can form a closed boundary for a surface member.
    /// For a closed boundary, we typically need at least 3 segments.
    /// </summary>
    /// <param name="segments">Collection of segments to validate.</param>
    /// <param name="positions">Corresponding positions for each segment.</param>
    /// <returns>True if segments can form a closed boundary, false otherwise.</returns>
    public static bool CanFormClosedBoundary(System.Collections.Generic.List<XmiSegment> segments, System.Collections.Generic.List<int> positions)
    {
        if (segments == null || segments.Count < 3) return false;
        if (positions == null || positions.Count != segments.Count) return false;
        
        // Check that all positions are valid
        foreach (var position in positions)
        {
            if (position < 0)
                return false;
        }
        
        return true;
    }
}
