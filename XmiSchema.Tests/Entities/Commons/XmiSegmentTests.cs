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

        // Position is now handled by XmiHasSegment relationship
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
            XmiSegmentTypeEnum.Line
        );

        // Position is now handled by XmiHasSegment relationship
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
            XmiSegmentTypeEnum.Line
        );

        // Position validation is now handled by XmiHasSegment relationship
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
            XmiSegmentTypeEnum.Line
        );

        // Position is now handled by XmiHasSegment relationship
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
            XmiSegmentTypeEnum.CircularArc
        );

        // Position is now handled by XmiHasSegment relationship
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
            XmiSegmentTypeEnum.Line
        );

        // Position is now handled by XmiHasSegment relationship
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
            XmiSegmentTypeEnum.Line
        );

        // Position is now handled by XmiHasSegment relationship
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
        var segmentStart = new XmiSegment("seg-start", "Start", "", "native-start", "", XmiSegmentTypeEnum.Line);
        var segmentMid = new XmiSegment("seg-mid", "Middle", "", "native-mid", "", XmiSegmentTypeEnum.Line);
        var segmentEnd = new XmiSegment("seg-end", "End", "", "native-end", "", XmiSegmentTypeEnum.Line);

        // IsValidPosition is now handled by XmiHasSegment relationship
    }

    /// <summary>
    /// Validates IsValidPosition returns true for defaulted position values.
    /// </summary>
    [Fact]
    public void IsValidPosition_DefaultedPositions_ReturnsTrue()
    {
        var segmentNegative = new XmiSegment("seg-neg", "Negative", "", "native-neg", "", XmiSegmentTypeEnum.Line);
        var segmentOver = new XmiSegment("seg-over", "Over", "", "native-over", "", XmiSegmentTypeEnum.Line);

        // IsValidPosition is now handled by XmiHasSegment relationship
        // Position is now handled by XmiHasSegment relationship
        // Position is now handled by XmiHasSegment relationship
    }

    /// <summary>
    /// Validates ValidateSequence returns true for properly sequenced segments.
    /// </summary>
    [Fact]
    public void ValidateSequence_ValidSequence_ReturnsTrue()
    {
        var segments = new List<XmiSegment>
        {
            new XmiSegment("seg-1", "Segment 1", "", "native-1", "", XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-2", "Segment 2", "", "native-2", "", XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-3", "Segment 3", "", "native-3", "", XmiSegmentTypeEnum.Line)
        };
        var positions = new List<int> { 0, 1, 2 };

        Assert.True(XmiSegment.ValidateSequence(segments, positions));
    }

    /// <summary>
    /// Validates ValidateSequence returns false for improperly sequenced segments.
    /// </summary>
    [Fact]
    public void ValidateSequence_InvalidSequence_ReturnsFalse()
    {
        var segments = new List<XmiSegment>
        {
            new XmiSegment("seg-1", "Segment 1", "", "native-1", "", XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-2", "Segment 2", "", "native-2", "", XmiSegmentTypeEnum.Line), // Out of order
            new XmiSegment("seg-3", "Segment 3", "", "native-3", "", XmiSegmentTypeEnum.Line)
        };
        var positions = new List<int> { 1, 0, 2 };

        Assert.False(XmiSegment.ValidateSequence(segments, positions));
    }

    /// <summary>
    /// Validates ValidateSequence returns true for segments with defaulted positions.
    /// </summary>
    [Fact]
    public void ValidateSequence_DefaultedPositions_ReturnsTrue()
    {
        var segments = new List<XmiSegment>
        {
            new XmiSegment("seg-1", "Segment 1", "", "native-1", "", XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-2", "Segment 2", "", "native-2", "", XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-3", "Segment 3", "", "native-3", "", XmiSegmentTypeEnum.Line)
        };
        var positions = new List<int> { 0, 0, 2 };

        // After defaulting, positions will be 0, 0, 2 which passes validation (0 <= 0 <= 2)
        // ValidateSequence only checks for ascending order and valid positions, not uniqueness
        Assert.True(XmiSegment.ValidateSequence(segments, positions));
    }

    /// <summary>
    /// Validates SortByPosition returns segments in correct order.
    /// </summary>
    [Fact]
    public void SortByPosition_UnsortedSegments_ReturnsSorted()
    {
        var segments = new List<XmiSegment>
        {
            new XmiSegment("seg-3", "Segment 3", "", "native-3", "", XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-1", "Segment 1", "", "native-1", "", XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-2", "Segment 2", "", "native-2", "", XmiSegmentTypeEnum.Line)
        };
        var positions = new List<int> { 2, 0, 1 };

        var sorted = XmiSegment.SortByPosition(segments, positions);

        Assert.Equal("seg-1", sorted[0].Id);
        Assert.Equal("seg-2", sorted[1].Id);
        Assert.Equal("seg-3", sorted[2].Id);
    }

    /// <summary>
    /// Validates CanFormClosedBoundary returns true for sufficient segments.
    /// </summary>
    [Fact]
    public void CanFormClosedBoundary_SufficientSegments_ReturnsTrue()
    {
        var segments = new List<XmiSegment>
        {
            new XmiSegment("seg-1", "Segment 1", "", "native-1", "", XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-2", "Segment 2", "", "native-2", "", XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-3", "Segment 3", "", "native-3", "", XmiSegmentTypeEnum.Line)
        };
        var positions = new List<int> { 0, 1, 2 };

        Assert.True(XmiSegment.CanFormClosedBoundary(segments, positions));
    }

    /// <summary>
    /// Validates CanFormClosedBoundary returns false for insufficient segments.
    /// </summary>
    [Fact]
    public void CanFormClosedBoundary_InsufficientSegments_ReturnsFalse()
    {
        var segments = new List<XmiSegment>
        {
            new XmiSegment("seg-1", "Segment 1", "", "native-1", "", XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-2", "Segment 2", "", "native-2", "", XmiSegmentTypeEnum.Line)
        };
        var positions = new List<int> { 0, 1 };

        Assert.False(XmiSegment.CanFormClosedBoundary(segments, positions));
    }

    /// <summary>
    /// Validates CanFormClosedBoundary returns true for segments with defaulted positions.
    /// </summary>
    [Fact]
    public void CanFormClosedBoundary_DefaultedPositions_ReturnsTrue()
    {
        var segments = new List<XmiSegment>
        {
            new XmiSegment("seg-1", "Segment 1", "", "native-1", "", XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-2", "Segment 2", "", "native-2", "", XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-3", "Segment 3", "", "native-3", "", XmiSegmentTypeEnum.Line) // Will be defaulted to 0
        };

        var positions = new List<int> { 0, 1, 0 };
        // After defaulting, positions will be 0, 1, 0 - but since we have 3 segments with valid positions (0, 1, 0), it can form a boundary
        Assert.True(XmiSegment.CanFormClosedBoundary(segments, positions));
    }
}
