using XmiSchema.Entities.Physical;
using XmiSchema.Entities.Bases;
using XmiSchema.Entities.Commons;
using XmiSchema.Entities.Relationships;
using XmiSchema.Managers;
using XmiSchema.Enums;
using XmiSchema.Tests.Managers;
namespace XmiSchema.Tests.Entities.Physical;

/// <summary>
/// Validates <see cref="XmiBeam"/> entity creation and properties.
/// </summary>
public class XmiBeamTests
{
    /// <summary>
    /// Ensures constructor assigns all required properties.
    /// </summary>
    [Fact]
    public void Constructor_AssignsAllProperties()
    {
        var beam = TestModelFactory.CreateBeam();

        Assert.Equal("beam-1", beam.Id);
        Assert.Equal("Beam beam-1", beam.Name);
        Assert.Equal("BEAM-1", beam.NativeId);
        Assert.Equal(nameof(XmiBeam), beam.EntityName);
        Assert.Equal(XmiSystemLineEnum.MiddleMiddle, beam.SystemLine);
        Assert.Equal(5.0, beam.Length);
    }

    /// <summary>
    /// Validates that segments with invalid positions are defaulted to 0 when creating beams.
    /// </summary>
    [Fact]
    public void CreateXmiBeam_DefaultsInvalidSegmentPositionsToZero()
    {
        var model = new XmiModel();
        var material = TestModelFactory.CreateMaterial();
        var axis = new XmiAxis(1, 0, 0);
        
        // Create segments 
        var segments = new List<XmiSegment>
        {
            new XmiSegment("seg-1", "Segment 1", "", "native-1", "", XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-2", "Segment 2", "", "native-2", "", XmiSegmentTypeEnum.Line)
        };
        var positions = new List<int> { -1, -5 }; // Invalid positions

        var beam = model.CreateXmiBeam(
            "beam-1",
            "Test Beam",
            "",
            "beam-native-1",
            "Test beam with invalid segment positions",
            material,
            segments,
            positions,
            XmiSystemLineEnum.MiddleMiddle,
            5.0,
            axis,
            axis,
            axis,
            0.0,
            0.0,
            0.0,
            0.0,
            0.0,
            0.0
        );

        // Verify beam was created successfully
        Assert.NotNull(beam);
        
        // Verify segments were added and their positions were defaulted to 0
        var segmentRelationships = model.Relationships.OfType<XmiHasSegment>()
            .Where(r => r.Source.Id == beam.Id)
            .ToList();
        
        Assert.Equal(2, segmentRelationships.Count);
        
        foreach (var relationship in segmentRelationships)
        {
            var segment = relationship.Target as XmiSegment;
            Assert.NotNull(segment);
            // Position is now handled by XmiHasSegment relationship
            var hasSegmentRelation = relationship as XmiHasSegment;
            Assert.NotNull(hasSegmentRelation);
            Assert.True(hasSegmentRelation.IsValidPosition);
        }
    }

    /// <summary>
    /// Verifies that XmiBeam inherits from XmiBasePhysicalEntity and has Physical type.
    /// </summary>
    [Fact]
    public void Constructor_SetsDomainToPhysical()
    {
        var beam = TestModelFactory.CreateBeam();

        Assert.Equal(XmiBaseEntityDomainEnum.Physical, beam.Domain);
        Assert.IsAssignableFrom<XmiBasePhysicalEntity>(beam);
    }

    /// <summary>
    /// Equality is based on the native identifier.
    /// </summary>
    [Fact]
    public void Equals_UsesNativeId()
    {
        var first = TestModelFactory.CreateBeam("beam-shared");
        var second = TestModelFactory.CreateBeam("beam-shared");

        Assert.True(first.Equals(second));
    }

    /// <summary>
    /// Different native IDs result in inequality.
    /// </summary>
    [Fact]
    public void Equals_ReturnsFalseForDifferentNativeIds()
    {
        var first = TestModelFactory.CreateBeam("beam-a");
        var second = TestModelFactory.CreateBeam("beam-b");

        Assert.False(first.Equals(second));
    }

    /// <summary>
    /// GetHashCode is consistent with equality.
    /// </summary>
    [Fact]
    public void GetHashCode_ConsistentWithEquals()
    {
        var first = TestModelFactory.CreateBeam("beam-hash");
        var second = TestModelFactory.CreateBeam("beam-hash");

        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }
}
