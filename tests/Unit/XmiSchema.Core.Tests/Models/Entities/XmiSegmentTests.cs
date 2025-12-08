using XmiSchema.Models.Commons;
using XmiSchema.Models.Entities.Physical;
using XmiSchema.Models.Entities.StructuralAnalytical;
using XmiSchema.Models.Bases;
using XmiSchema.Tests.Support;

using XmiSchema.Models.Enums;
namespace XmiSchema.Tests.Models.Entities;

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

        Assert.Equal(0.5f, segment.Position);
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
            0.0f,
            XmiSegmentTypeEnum.Line
        );

        Assert.Equal(0.0f, segment.Position);
        Assert.Equal("Start Segment", segment.Name);
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
            1.0f,
            XmiSegmentTypeEnum.Line
        );

        Assert.Equal(1.0f, segment.Position);
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
            0.75f,
            XmiSegmentTypeEnum.CircularArc
        );

        Assert.Equal(0.75f, segment.Position);
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
            0.5f,
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
            0.5f,
            XmiSegmentTypeEnum.Line
        );

        Assert.Equal("seg-1", segment.Id);
        Assert.Equal("Test Segment", segment.Name);
        Assert.Equal("ifc-guid-123", segment.IfcGuid);
        Assert.Equal("native-123", segment.NativeId);
        Assert.Equal("Test description", segment.Description);
        Assert.Equal(nameof(XmiSegment), segment.EntityType);
        Assert.Equal(XmiBaseEntityDomainEnum.Shared, segment.Type);
    }

    /// <summary>
    /// Validates segment can have negative position (edge case - may be invalid in real scenarios).
    /// </summary>
    [Fact]
    public void Constructor_AllowsNegativePosition()
    {
        var segment = new XmiSegment(
            "seg-neg",
            "Negative Position",
            "",
            "native-neg",
            "",
            -0.5f,
            XmiSegmentTypeEnum.Line
        );

        Assert.Equal(-0.5f, segment.Position);
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
            1.5f,
            XmiSegmentTypeEnum.Line
        );

        Assert.Equal(1.5f, segment.Position);
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
            0.5f,
            XmiSegmentTypeEnum.Line
        );

        Assert.Equal("seg-empty", segment.Id);
        Assert.Equal("seg-empty", segment.Name); // XmiBaseEntity defaults name to id when empty
    }
}
