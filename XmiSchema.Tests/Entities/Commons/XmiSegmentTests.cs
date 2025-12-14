using XmiSchema.Entities.Physical;
using XmiSchema.Entities.StructuralAnalytical;
using XmiSchema.Entities.Bases;
using XmiSchema.Entities.Commons;
using XmiSchema.Enums;
using XmiSchema.Tests.Managers;
namespace XmiSchema.Tests.Entities.Commons;

/// <summary>
/// Covers the lightweight <see cref="XmiSegment"/> entity.
/// </summary>
public class XmiSegmentTests
{
    /// <summary>
    /// Validates constructor inputs are surfaced via properties.
    /// </summary>
    [Fact]
    public void Constructor_AssignsProperties()
    {
        var segment = TestModelFactory.CreateSegment();

        Assert.Equal(0, segment.Position);
        Assert.Equal(XmiSegmentTypeEnum.Line, segment.SegmentType);
    }

    /// <summary>
    /// Validates segment with position at start (0.0).
    /// </summary>
    [Fact]
    public void Constructor_AcceptsPositionAtStart()
    {
        var segment = new XmiSegment(
            "seg-start",
            "Start Segment",
            "",
            "native-start",
            "",
            0,
            XmiSegmentTypeEnum.Line
        );

        Assert.Equal(0, segment.Position);
        Assert.Equal("Start Segment", segment.Name);
    }

    /// <summary>
    /// Validates that negative positions are defaulted to 0.
    /// </summary>
    [Fact]
    public void Constructor_DefaultsNegativePositionToZero()
    {
        var segment = new XmiSegment(
            "seg-negative",
            "Negative Position Segment",
            "",
            "native-negative",
            "",
            -1,
            XmiSegmentTypeEnum.Line
        );

        Assert.Equal(0, segment.Position);
        Assert.True(segment.IsValidPosition);
    }

    /// <summary>
    /// Validates segment with position at end (1.0).
    /// </summary>
    [Fact]
    public void Constructor_AcceptsPositionAtEnd()
    {
        var segment = new XmiSegment(
            "seg-end",
            "End Segment",
            "",
            "native-end",
            "",
            1,
            XmiSegmentTypeEnum.Line
        );

        Assert.Equal(1, segment.Position);
        Assert.Equal("End Segment", segment.Name);
    }

    /// <summary>
    /// Validates segment with intermediate position values.
    /// </summary>
    [Fact]
    public void Constructor_AcceptsIntermediatePosition()
    {
        var segment = new XmiSegment(
            "seg-mid",
            "Middle Segment",
            "",
            "native-mid",
            "",
            2,
            XmiSegmentTypeEnum.CircularArc
        );

        Assert.Equal(2, segment.Position);
        Assert.Equal(XmiSegmentTypeEnum.CircularArc, segment.SegmentType);
    }

    /// <summary>
    /// Validates segment can store different segment types.
    /// </summary>
    [Fact]
    public void Constructor_AcceptsCircularArcSegmentType()
    {
        var segment = new XmiSegment(
            "seg-arc",
            "Arc Segment",
            "",
            "native-arc",
            "",
            1,
            XmiSegmentTypeEnum.CircularArc
        );

        Assert.Equal(XmiSegmentTypeEnum.CircularArc, segment.SegmentType);
    }

    /// <summary>
    /// Validates segment inherits from XmiBaseEntity correctly.
    /// </summary>
    [Fact]
    public void Constructor_InheritsBaseEntityProperties()
    {
        var segment = new XmiSegment(
            "seg-1",
            "Test Segment",
            "ifc-guid-123",
            "native-123",
            "Test description",
            1,
            XmiSegmentTypeEnum.Line
        );

        Assert.Equal("seg-1", segment.Id);
        Assert.Equal("Test Segment", segment.Name);
        Assert.Equal("ifc-guid-123", segment.IfcGuid);
        Assert.Equal("native-123", segment.NativeId);
        Assert.Equal("Test description", segment.Description);
        Assert.Equal(nameof(XmiSegment), segment.EntityName);
        Assert.Equal(XmiBaseEntityDomainEnum.Shared, segment.Domain);
    }

    /// <summary>
    /// Validates segment defaults negative position to 0 (changed behavior).
    /// </summary>
    [Fact]
    public void Constructor_DefaultsNegativePositionToZeroUpdated()
    {
        var segment = new XmiSegment(
            "seg-neg-updated",
            "Negative Position Updated",
            "",
            "native-neg-updated",
            "",
            -5,
            XmiSegmentTypeEnum.Line
        );

        Assert.Equal(0, segment.Position);
    }

    /// <summary>
    /// Validates segment can have position greater than 1 (edge case - may be invalid in real scenarios).
    /// </summary>
    [Fact]
    public void Constructor_AllowsPositionGreaterThanOne()
    {
        var segment = new XmiSegment(
            "seg-over",
            "Over Position",
            "",
            "native-over",
            "",
            2,
            XmiSegmentTypeEnum.Line
        );

        Assert.Equal(2, segment.Position);
    }

    /// <summary>
    /// Validates segment with empty string properties.
    /// </summary>
    [Fact]
    public void Constructor_AcceptsEmptyStrings()
    {
        var segment = new XmiSegment(
            "seg-empty",
            "",
            "",
            "",
            "",
            1,
            XmiSegmentTypeEnum.Line
        );

        Assert.Equal("seg-empty", segment.Id);
        Assert.Equal("seg-empty", segment.Name); // XmiBaseEntity defaults name to id when empty
    }

    /// <summary>
    /// Validates IsValidPosition returns true for valid position values.
    /// </summary>
    [Fact]
    public void IsValidPosition_ValidPositions_ReturnsTrue()
    {
        var segmentStart = new XmiSegment("seg-start", "Start", "", "native-start", "", 0, XmiSegmentTypeEnum.Line);
        var segmentMid = new XmiSegment("seg-mid", "Middle", "", "native-mid", "", 1, XmiSegmentTypeEnum.Line);
        var segmentEnd = new XmiSegment("seg-end", "End", "", "native-end", "", 2, XmiSegmentTypeEnum.Line);

        Assert.True(segmentStart.IsValidPosition);
        Assert.True(segmentMid.IsValidPosition);
        Assert.True(segmentEnd.IsValidPosition);
    }

    /// <summary>
    /// Validates IsValidPosition returns true for defaulted position values.
    /// </summary>
    [Fact]
    public void IsValidPosition_DefaultedPositions_ReturnsTrue()
    {
        var segmentNegative = new XmiSegment("seg-neg", "Negative", "", "native-neg", "", -1, XmiSegmentTypeEnum.Line);
        var segmentOver = new XmiSegment("seg-over", "Over", "", "native-over", "", -2, XmiSegmentTypeEnum.Line);

        Assert.True(segmentNegative.IsValidPosition);
        Assert.True(segmentOver.IsValidPosition);
        Assert.Equal(0, segmentNegative.Position);
        Assert.Equal(0, segmentOver.Position);
    }

    /// <summary>
    /// Validates ValidateSequence returns true for properly sequenced segments.
    /// </summary>
    [Fact]
    public void ValidateSequence_ValidSequence_ReturnsTrue()
    {
        var segments = new List<XmiSegment>
        {
            new XmiSegment("seg-1", "Segment 1", "", "native-1", "", 0, XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-2", "Segment 2", "", "native-2", "", 1, XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-3", "Segment 3", "", "native-3", "", 2, XmiSegmentTypeEnum.Line)
        };

        Assert.True(XmiSegment.ValidateSequence(segments));
    }

    /// <summary>
    /// Validates ValidateSequence returns false for improperly sequenced segments.
    /// </summary>
    [Fact]
    public void ValidateSequence_InvalidSequence_ReturnsFalse()
    {
        var segments = new List<XmiSegment>
        {
            new XmiSegment("seg-1", "Segment 1", "", "native-1", "", 1, XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-2", "Segment 2", "", "native-2", "", 0, XmiSegmentTypeEnum.Line), // Out of order
            new XmiSegment("seg-3", "Segment 3", "", "native-3", "", 2, XmiSegmentTypeEnum.Line)
        };

        Assert.False(XmiSegment.ValidateSequence(segments));
    }

    /// <summary>
    /// Validates ValidateSequence returns true for segments with defaulted positions.
    /// </summary>
    [Fact]
    public void ValidateSequence_DefaultedPositions_ReturnsTrue()
    {
        var segments = new List<XmiSegment>
        {
            new XmiSegment("seg-1", "Segment 1", "", "native-1", "", 0, XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-2", "Segment 2", "", "native-2", "", -1, XmiSegmentTypeEnum.Line), // Will be defaulted to 0
            new XmiSegment("seg-3", "Segment 3", "", "native-3", "", 2, XmiSegmentTypeEnum.Line)
        };

        // After defaulting, positions will be 0, 0, 2 which passes validation (0 <= 0 <= 2)
        // ValidateSequence only checks for ascending order and valid positions, not uniqueness
        Assert.True(XmiSegment.ValidateSequence(segments));
    }

    /// <summary>
    /// Validates SortByPosition returns segments in correct order.
    /// </summary>
    [Fact]
    public void SortByPosition_UnsortedSegments_ReturnsSorted()
    {
        var segments = new List<XmiSegment>
        {
            new XmiSegment("seg-3", "Segment 3", "", "native-3", "", 2, XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-1", "Segment 1", "", "native-1", "", 0, XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-2", "Segment 2", "", "native-2", "", 1, XmiSegmentTypeEnum.Line)
        };

        var sorted = XmiSegment.SortByPosition(segments);

        Assert.Equal(0, sorted[0].Position);
        Assert.Equal(1, sorted[1].Position);
        Assert.Equal(2, sorted[2].Position);
    }

    /// <summary>
    /// Validates CanFormClosedBoundary returns true for sufficient segments.
    /// </summary>
    [Fact]
    public void CanFormClosedBoundary_SufficientSegments_ReturnsTrue()
    {
        var segments = new List<XmiSegment>
        {
            new XmiSegment("seg-1", "Segment 1", "", "native-1", "", 0, XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-2", "Segment 2", "", "native-2", "", 1, XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-3", "Segment 3", "", "native-3", "", 2, XmiSegmentTypeEnum.Line)
        };

        Assert.True(XmiSegment.CanFormClosedBoundary(segments));
    }

    /// <summary>
    /// Validates CanFormClosedBoundary returns false for insufficient segments.
    /// </summary>
    [Fact]
    public void CanFormClosedBoundary_InsufficientSegments_ReturnsFalse()
    {
        var segments = new List<XmiSegment>
        {
            new XmiSegment("seg-1", "Segment 1", "", "native-1", "", 0, XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-2", "Segment 2", "", "native-2", "", 1, XmiSegmentTypeEnum.Line)
        };

        Assert.False(XmiSegment.CanFormClosedBoundary(segments));
    }

    /// <summary>
    /// Validates CanFormClosedBoundary returns true for segments with defaulted positions.
    /// </summary>
    [Fact]
    public void CanFormClosedBoundary_DefaultedPositions_ReturnsTrue()
    {
        var segments = new List<XmiSegment>
        {
            new XmiSegment("seg-1", "Segment 1", "", "native-1", "", 0, XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-2", "Segment 2", "", "native-2", "", 1, XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-3", "Segment 3", "", "native-3", "", -1, XmiSegmentTypeEnum.Line) // Will be defaulted to 0
        };

        // After defaulting, positions will be 0, 1, 0 - but since we have 3 segments with valid positions (0, 1, 0), it can form a boundary
        Assert.True(XmiSegment.CanFormClosedBoundary(segments));
    }
}
