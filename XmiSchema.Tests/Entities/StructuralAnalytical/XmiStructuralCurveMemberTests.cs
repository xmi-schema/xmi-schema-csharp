using XmiSchema.Entities.Commons;
using XmiSchema.Managers;
using XmiSchema.Entities.Physical;
using XmiSchema.Entities.StructuralAnalytical;
using XmiSchema.Entities.Bases;
using XmiSchema.Entities.Relationships;
using XmiSchema.Enums;
using XmiSchema.Tests.Managers;
namespace XmiSchema.Tests.Entities.StructuralAnalytical;

/// <summary>
/// Ensures curve member metadata is carried through constructor arguments.
/// </summary>
public class XmiStructuralCurveMemberTests
{
    /// <summary>
    /// Validates that segments with invalid positions are defaulted to 0 when creating structural curve members.
    /// </summary>
    [Fact]
    public void CreateXmiStructuralCurveMember_DefaultsInvalidSegmentPositionsToZero()
    {
        var model = new XmiModel();
        var material = TestModelFactory.CreateMaterial();
        var crossSection = TestModelFactory.CreateCrossSection();
        var storey = TestModelFactory.CreateStorey();
        var axis = new XmiAxis(1, 0, 0);
        var nodes = new List<XmiStructuralPointConnection>
        {
            TestModelFactory.CreatePointConnection(),
            TestModelFactory.CreatePointConnection("pc-2")
        };
        
        // Create segments with invalid positions
        var segments = new List<XmiSegment>
        {
            new XmiSegment("seg-1", "Segment 1", "", "native-1", "", -3, XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-2", "Segment 2", "", "native-2", "", -7, XmiSegmentTypeEnum.Line)
        };

        var curveMember = model.CreateXmiStructuralCurveMember(
            "curve-member-1",
            "Test Curve Member",
            "",
            "curve-native-1",
            "Test curve member with invalid segment positions",
            material,
            crossSection,
            storey,
            XmiStructuralCurveMemberTypeEnum.Beam,
            nodes,
            segments,
            XmiSystemLineEnum.MiddleMiddle,
            nodes[0],
            nodes[1],
            5.0,
            axis,
            axis,
            axis,
            0.0,
            0.0,
            0.0,
            0.0,
            0.0,
            0.0,
            "Fixed",
            "Pinned"
        );

        // Verify curve member was created successfully
        Assert.NotNull(curveMember);
        
        // Verify segments were added and their positions were defaulted to 0
        var segmentRelationships = model.Relationships.OfType<XmiHasSegment>()
            .Where(r => r.Source.Id == curveMember.Id)
            .ToList();
        
        Assert.Equal(2, segmentRelationships.Count);
        
        foreach (var relationship in segmentRelationships)
        {
            var segment = relationship.Target as XmiSegment;
            Assert.NotNull(segment);
            Assert.Equal(0, segment.Position);
            Assert.True(segment.IsValidPosition);
        }
    }

    /// <summary>
    /// Verifies key properties such as offsets and fixity are stored.
    /// </summary>
    [Fact]
    public void Constructor_AssignsOffsetsAndFixity()
    {
        var member = TestModelFactory.CreateCurveMember();

        Assert.Equal(XmiStructuralCurveMemberTypeEnum.Beam, member.CurveMemberType);
        Assert.Equal(XmiSystemLineEnum.MiddleMiddle, member.SystemLine);
        Assert.Equal("Fixed", member.EndFixityStart);
        Assert.Equal(5.0, member.Length);
    }

    /// <summary>
    /// Equality is based on the native identifier.
    /// </summary>
    [Fact]
    public void Equals_UsesNativeId()
    {
        var first = TestModelFactory.CreateCurveMember("cur-shared");
        var second = TestModelFactory.CreateCurveMember("cur-shared");

        Assert.True(first.Equals(second));
    }
}
