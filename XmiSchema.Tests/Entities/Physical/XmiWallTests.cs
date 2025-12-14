using XmiSchema.Entities.Commons;
using XmiSchema.Managers;
using XmiSchema.Entities.StructuralAnalytical;
using XmiSchema.Entities.Physical;
using XmiSchema.Entities.Bases;
using XmiSchema.Entities.Relationships;
using XmiSchema.Enums;
using XmiSchema.Tests.Managers;
namespace XmiSchema.Tests.Entities.Physical;

/// <summary>
/// Validates <see cref="XmiWall"/> entity creation and properties.
/// </summary>
public class XmiWallTests
{
    /// <summary>
    /// Validates that segments with invalid positions are defaulted to 0 when creating walls.
    /// </summary>
    [Fact]
    public void CreateXmiWall_DefaultsInvalidSegmentPositionsToZero()
    {
        var model = new XmiModel();
        var material = TestModelFactory.CreateMaterial();
        var axis = new XmiAxis(1, 0, 0);
        
        // Create segments with invalid positions
        var segments = new List<XmiSegment>
        {
            new XmiSegment("seg-1", "Segment 1", "", "native-1", "", XmiSegmentTypeEnum.Line),
            new XmiSegment("seg-2", "Segment 2", "", "native-2", "", XmiSegmentTypeEnum.Line)
        };

        var wall = model.CreateXmiWall(
            "wall-1",
            "Test Wall",
            "",
            "wall-native-1",
            "Test wall with invalid segment positions",
            material,
            segments,
            new List<int> { -2, -10 },
            0.0,
            axis,
            axis,
            axis,
            3.0
        );

        // Verify wall was created successfully
        Assert.NotNull(wall);
        
        // Verify segments were added and their positions were defaulted to 0
        var segmentRelationships = model.Relationships.OfType<XmiHasSegment>()
            .Where(r => r.Source.Id == wall.Id)
            .ToList();
        
        Assert.Equal(2, segmentRelationships.Count);
        
        // Verify that the positions were defaulted to 0 via the relationships
        var hasSegmentRelationships = model.Relationships.OfType<XmiHasSegment>().ToList();
        Assert.Equal(2, hasSegmentRelationships.Count);
        
        // Check that positions were properly defaulted to 0
        foreach (var hasSegmentRel in hasSegmentRelationships)
        {
            Assert.Equal(0, hasSegmentRel.Position);
        }
    }

    /// <summary>
    /// Ensures constructor assigns all required properties.
    /// </summary>
    [Fact]
    public void Constructor_AssignsAllProperties()
    {
        var wall = new XmiWall("wall-1", "W1", "ifc-wall", "native-wall-1", "Concrete wall", 0.4, new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1), 3.2);

        Assert.Equal("wall-1", wall.Id);
        Assert.Equal("W1", wall.Name);
        Assert.Equal("ifc-wall", wall.IfcGuid);
        Assert.Equal("native-wall-1", wall.NativeId);
        Assert.Equal("Concrete wall", wall.Description);
        Assert.Equal(nameof(XmiWall), wall.EntityName);
        Assert.Equal(0.4, wall.ZOffset);
        Assert.Equal(new XmiAxis(1, 0, 0), wall.LocalAxisX);
        Assert.Equal(new XmiAxis(0, 1, 0), wall.LocalAxisY);
        Assert.Equal(new XmiAxis(0, 0, 1), wall.LocalAxisZ);
        Assert.Equal(3.2, wall.Height);
    }

    /// <summary>
    /// Verifies that XmiWall inherits from XmiBasePhysicalEntity and has Physical type.
    /// </summary>
    [Fact]
    public void Constructor_SetsDomainToPhysical()
    {
        var wall = new XmiWall("wall-2", "W2", "ifc", "native", "desc", 0, new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1), 3.0);

        Assert.Equal(XmiBaseEntityDomainEnum.Physical, wall.Domain);
        Assert.IsAssignableFrom<XmiBasePhysicalEntity>(wall);
    }

    /// <summary>
    /// Equality is based on the native identifier.
    /// </summary>
    [Fact]
    public void Equals_UsesNativeId()
    {
        var first = new XmiWall("wall-3", "W3", "ifc", "WALL-SHARED", "First wall", 0, new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1), 3.0);
        var second = new XmiWall("wall-4", "W4", "ifc2", "wall-shared", "Second wall", 0, new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1), 3.0);

        Assert.True(first.Equals(second));
    }

    /// <summary>
    /// Different native IDs result in inequality.
    /// </summary>
    [Fact]
    public void Equals_ReturnsFalseForDifferentNativeIds()
    {
        var first = new XmiWall("wall-5", "W5", "ifc", "WALL-A", "Wall A", 0, new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1), 3.0);
        var second = new XmiWall("wall-6", "W6", "ifc", "WALL-B", "Wall B", 0, new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1), 3.0);

        Assert.False(first.Equals(second));
    }

    /// <summary>
    /// GetHashCode is consistent with equality.
    /// </summary>
    [Fact]
    public void GetHashCode_ConsistentWithEquals()
    {
        var first = new XmiWall("wall-7", "W7", "ifc", "WALL-HASH", "Wall", 0, new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1), 3.0);
        var second = new XmiWall("wall-8", "W8", "ifc", "wall-hash", "Wall", 0, new XmiAxis(1, 0, 0), new XmiAxis(0, 1, 0), new XmiAxis(0, 0, 1), 3.0);

        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }
}
