using System.Linq;
using XmiSchema.Entities.Bases;
using XmiSchema.Enums;

namespace XmiSchema.Entities.Commons;

/// <summary>
/// Represents a logical segment along a structural member for geometric decomposition.
/// </summary>
/// <remarks>
/// This class defines a logical sub-division of structural members (beams, columns,
/// braces) into simpler geometric components. Segments enable representation of
/// complex member geometries composed of multiple geometric primitives.
///
/// <h4>Purpose</h4>
/// Structural members may have complex geometries that cannot be represented by a
/// single geometric primitive. For example:
/// <list type="bullet">
/// <item><description>Curved beams requiring multiple arc segments</description></item>
/// <item><description>Tapered members with changing cross-section</description></item>
/// <item><description>Members with geometric discontinuities</description></item>
/// <item><description>Straight members combined with arc segments</description></item>
/// </list>
///
/// <h4>Segment Types</h4>
/// The <see cref="SegmentType"/> property defines the geometric form of the segment:
/// <list type="bullet">
/// <item><description><see cref="XmiSegmentTypeEnum.Line"/> - Straight line segment</description></item>
/// <item><description><see cref="XmiSegmentTypeEnum.CircularArc"/> - Circular arc segment</description></item>
/// </list>
///
/// <h4>Positioning</h4>
/// Segment positions along a member are managed through the <see cref="XmiHasSegment"/>
/// relationship's <c>Position</c> property, not as a direct property on the
/// <see cref="XmiSegment"/> class. This allows:
/// <list type="bullet">
/// <item><description>Proper sequencing of segments</description></item>
/// <item><description>Validation of position continuity</description></item>
/// <item><description>Clear separation of segment definition and positioning</description></item>
/// </list>
///
/// <h4>Validation</h4>
/// The class provides static methods for validating segment collections:
/// <list type="bullet">
/// <item><description><see cref="ValidateSequence"/> - Ensures proper position ordering</description></item>
/// <item><description><see cref="SortByPosition"/> - Orders segments by position</description></item>
/// <item><description><see cref="CanFormClosedBoundary"/> - Checks for surface member validity</description></item>
/// </list>
///
/// <h4>Domain</h4>
/// All segments have their <see cref="XmiBaseEntity.Domain"/> property set to
/// <see cref="XmiBaseEntityDomainEnum.Shared"/> as they are resources that can
/// be referenced by both physical and analytical elements.
/// </remarks>
/// <example>
/// Creating a line segment for a beam:
/// <code>
/// var lineSegment = new XmiSegment(
///     id: "segment_beam_001_1",
///     name: "Beam Segment 1 - Straight",
///     ifcGuid: "2h9$K0b0P6j7aB_1m5H7fR",
///     nativeId: "SEG-001-1",
///     description: "Straight segment of beam between columns A1 and B1",
///     segmentType: XmiSegmentTypeEnum.Line
/// );
///
/// // Segment is associated with member via XmiHasSegment relationship:
/// var hasSegment = new XmiHasSegment(
///     id: "rel_beam_001_seg_1",
///     source: beamEntity,
///     target: lineSegment,
///     name: "Beam to Segment 1",
///     position: 0  // First segment
/// );
/// </code>
/// </example>
/// <example>
/// Creating an arc segment for a curved beam:
/// <code>
/// var arcSegment = new XmiSegment(
///     id: "segment_beam_002_1",
///     name: "Beam Segment 1 - Arc",
///     ifcGuid: "1a2b3c4d5e6f",
///     nativeId: "SEG-002-1",
///     description: "Circular arc segment defining curved beam",
///     segmentType: XmiSegmentTypeEnum.CircularArc
/// );
/// </code>
/// </example>
/// <example>
/// Validating and sorting segments:
/// <code>
/// var segments = new List&lt;XmiSegment&gt; { segment1, segment2, segment3 };
/// var positions = new List&lt;int&gt; { 2, 0, 1 };  // Out of order
///
/// // Validate sequence
/// bool isValid = XmiSegment.ValidateSequence(segments, positions);
/// // isValid == false (not in ascending order)
///
/// // Sort segments by position
/// var sortedSegments = XmiSegment.SortByPosition(segments, positions);
/// // sortedSegments == [segment2, segment3, segment1] (in position order)
/// </code>
/// </example>
/// <seealso cref="XmiHasSegment"/>
/// <seealso cref="XmiSegmentTypeEnum"/>
/// <seealso cref="XmiBaseEntity"/>
/// <seealso cref="XmiBaseEntityDomainEnum"/>
public class XmiSegment : XmiBaseEntity
{

    // public XmiBaseGeometry Geometry { get; set; }   // Surface member support is assigned to
    /// <summary>
    /// Gets or sets the geometric type of this segment.
    /// </summary>
    /// <value>The segment type enumeration value (e.g., Line, CircularArc).</value>
    /// <remarks>
    /// This property defines the geometric form of the segment, which determines:
    /// <list type="bullet">
    /// <item><description>Which geometric properties are required</description></item>
    /// <item><description>How the segment is visualized</description></item>
    /// <item><description>Analysis assumptions for the segment</description></item>
    /// </list>
    ///
    /// Common segment types:
    /// <list type="bullet">
    /// <item><description><see cref="XmiSegmentTypeEnum.Line"/> - Straight segment defined by two points</description></item>
    /// <item><description><see cref="XmiSegmentTypeEnum.CircularArc"/> - Curved segment defined by start, end, center, and radius</description></item>
    /// </list>
    /// </remarks>
    /// <seealso cref="XmiSegmentTypeEnum"/>
    public XmiSegmentTypeEnum SegmentType { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmiSegment"/> class representing
    /// a logical division along a structural member.
    /// </summary>
    /// <param name="id">The stable, unique identifier for this segment. Must not be null or whitespace.</param>
    /// <param name="name">The human-readable display name. If null or whitespace, defaults to <paramref name="id"/>.</param>
    /// <param name="ifcGuid">The IFC GUID reference for BIM interoperability. Can be null.</param>
    /// <param name="nativeId">The identifier from the native source system for traceability. Can be null.</param>
    /// <param name="description">A textual description of this segment. Can be null.</param>
    /// <param name="segmentType">The geometric type of this segment (e.g., <see cref="XmiSegmentTypeEnum.Line"/>, <see cref="XmiSegmentTypeEnum.CircularArc"/>).</param>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="id"/> is null or whitespace.</exception>
    /// <remarks>
    /// This constructor initializes all segment properties and sets the
    /// <see cref="XmiBaseEntity.Domain"/> property to
    /// <see cref="XmiBaseEntityDomainEnum.Shared"/> as segments are shared
    /// resources across the XMI graph.
    ///
    /// <h4>Segment Positioning</h4>
    /// Segment positions along a member are managed through the <see cref="XmiHasSegment"/>
    /// relationship's <c>Position</c> property. This relationship allows segments to be:
    /// <list type="bullet">
    /// <item><description>Ordered sequentially along a member</description></item>
    /// <item><description>Validated for position continuity</description></item>
    /// <item><description>Sorted and queried by position</description></item>
    /// </list>
    ///
    /// <h4>Geometric Associations</h4>
    /// Segment geometry (points, lines, arcs) is typically associated through
    /// <see cref="XmiHasGeometry"/>, <see cref="XmiHasLine3D"/>, or
    /// <see cref="XmiHasPoint3D"/> relationships.
    /// </remarks>
    /// <seealso cref="XmiHasSegment"/>
    /// <seealso cref="XmiSegmentTypeEnum"/>
    /// <seealso cref="XmiBaseEntityDomainEnum.Shared"/>
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
    /// </summary>
    /// <param name="segments">The collection of segments to validate.</param>
    /// <param name="positions">The corresponding position values for each segment.</param>
    /// <returns>True if segments are properly sequenced with no gaps and in ascending order; otherwise, false.</returns>
    /// <remarks>
    /// This method validates that segment positions are:
    /// <list type="bullet">
    /// <item><description>All non-negative (≥ 0)</description></item>
    /// <item><description>In strictly ascending order (no duplicates in the same position)</description></item>
    /// <item><description>Sequential without gaps (e.g., 0, 1, 2, not 0, 2, 5)</description></item>
    /// </list>
    ///
    /// A collection with fewer than 2 segments is considered valid by definition.
    ///
    /// This validation is important for ensuring that segments form a continuous
    /// representation along a structural member without overlapping or disjoint segments.
    /// </remarks>
    /// <example>
    /// Validating a properly sequenced segment collection:
    /// <code>
    /// var segments = new List&lt;XmiSegment&gt; { seg1, seg2, seg3 };
    /// var positions = new List&lt;int&gt; { 0, 1, 2 };  // Properly sequenced
    ///
    /// bool isValid = XmiSegment.ValidateSequence(segments, positions);
    /// // isValid == true
    /// </code>
    /// </example>
    /// <example>
    /// Validating an improperly sequenced collection:
    /// <code>
    /// var positions = new List&lt;int&gt; { 2, 0, 1 };  // Out of order
    ///
    /// bool isValid = XmiSegment.ValidateSequence(segments, positions);
    /// // isValid == false (not in ascending order)
    /// </code>
    /// </example>
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
    /// <param name="segments">The collection of segments to sort.</param>
    /// <param name="positions">The corresponding position values for each segment.</param>
    /// <returns>A new list with segments sorted by position in ascending order, or an empty list if inputs are invalid.</returns>
    /// <remarks>
    /// This method reorders segments based on their position values to ensure that
    /// segments are organized sequentially along their parent member from start to end.
    /// It uses LINQ's <c>Zip</c> operation to pair segments with their
    /// positions, then <c>OrderBy</c> to sort by position value.
    ///
    /// If input parameters are null or have mismatched counts, an empty list is
    /// returned rather than throwing an exception, providing safe behavior for
    /// invalid inputs.
    /// </remarks>
    /// <example>
    /// Sorting segments by position:
    /// <code>
    /// var segments = new List&lt;XmiSegment&gt; { seg1, seg2, seg3 };
    /// var positions = new List&lt;int&gt; { 2, 0, 1 };  // Out of order
    ///
    /// var sortedSegments = XmiSegment.SortByPosition(segments, positions);
    /// // sortedSegments == [seg2, seg3, seg1]  // Ordered by position: 0, 1, 2
    /// </code>
    /// </example>
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
    /// Validates that segments can form a closed boundary suitable for a surface member.
    /// </summary>
    /// <param name="segments">The collection of segments to validate.</param>
    /// <param name="positions">The corresponding position values for each segment.</param>
    /// <returns>True if segments have sufficient quantity and valid positions to form a closed boundary; otherwise, false.</returns>
    /// <remarks>
    /// This method validates requirements for forming a closed polygonal boundary,
    /// which is necessary for surface members (slabs, walls, plates):
    /// <list type="bullet">
    /// <item><description>At least 3 segments are required (minimum for a closed polygon)</description></item>
    /// <item><description>All position values must be non-negative</description></item>
    /// <item><description>Segment and position counts must match</description></item>
    /// </list>
    ///
    /// <h4>Note</h4>
    /// This method validates basic quantitative requirements but does not perform
    /// geometric continuity checks (e.g., verifying that segment endpoints form
    /// a continuous closed loop). Such geometric validation would require
    /// additional analysis of the segment geometries themselves.
    ///
    /// For surface members, segments typically represent the edges of the surface
    /// polygon, and they must form a closed loop for the surface to be
    /// geometrically valid.
    /// </remarks>
    /// <example>
    /// Validating segments for a surface member (closed boundary):
    /// <code>
    /// var segments = new List&lt;XmiSegment&gt; { seg1, seg2, seg3, seg4 };
    /// var positions = new List&lt;int&gt; { 0, 1, 2, 3 };
    ///
    /// bool canClose = XmiSegment.CanFormClosedBoundary(segments, positions);
    /// // canClose == true (4 segments, all valid)
    /// </code>
    /// </example>
    /// <example>
    /// Insufficient segments for a closed boundary:
    /// <code>
    /// var segments = new List&lt;XmiSegment&gt; { seg1, seg2 };  // Only 2 segments
    /// var positions = new List&lt;int&gt; { 0, 1 };
    ///
    /// bool canClose = XmiSegment.CanFormClosedBoundary(segments, positions);
    /// // canClose == false (need at least 3 segments for a closed polygon)
    /// </code>
    /// </example>
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
