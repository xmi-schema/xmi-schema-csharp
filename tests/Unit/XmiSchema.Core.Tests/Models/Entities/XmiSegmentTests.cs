using XmiSchema.Core.Entities;
using XmiSchema.Core.Enums;

namespace XmiSchema.Core.Tests.Models.Entities;

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
}
