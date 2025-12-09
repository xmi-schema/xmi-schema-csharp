using XmiSchema.Entities.Geometries;
using XmiSchema.Entities.Relationships;
using XmiSchema.Enums;
using XmiSchema.Tests.Managers;

namespace XmiSchema.Tests.Entities.Relationships;

/// <summary>
/// Ensures line relationships retain their metadata and target equality methods work correctly.
/// </summary>
public class XmiHasLine3DTests
{
    [Fact]
    public void Constructor_AssignsMetadata()
    {
        var line = TestModelFactory.CreateLine();
        var relation = new XmiHasLine3d(
            "rel-line",
            TestModelFactory.CreateCurveMember(),
            line,
            "AlignedWith",
            "desc",
            nameof(XmiHasLine3d));

        Assert.Equal("rel-line", relation.Id);
        Assert.Equal("AlignedWith", relation.Name);
    }

    [Fact]
    public void Constructor_GeneratesIdentifier()
    {
        var line = TestModelFactory.CreateLine();
        var relation = new XmiHasLine3d(TestModelFactory.CreateCurveMember(), line);

        Assert.False(string.IsNullOrWhiteSpace(relation.Id));
    }

    #region TargetLine3d Property

    [Fact]
    public void TargetLine3d_ReturnsStronglyTypedTarget()
    {
        var line = TestModelFactory.CreateLine();
        var relation = new XmiHasLine3d(TestModelFactory.CreateCurveMember(), line);

        Assert.Same(line, relation.TargetLine3d);
    }

    #endregion

    #region HasSameTarget (Referential Equality)

    [Fact]
    public void HasSameTarget_SameLineReference_ReturnsTrue()
    {
        var startPoint = TestModelFactory.CreatePoint("start", 0, 0, 0);
        var endPoint = TestModelFactory.CreatePoint("end", 10, 10, 10);
        var line = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc", startPoint, endPoint);

        var rel1 = new XmiHasLine3d(TestModelFactory.CreateCurveMember("c1"), line);
        var rel2 = new XmiHasLine3d(TestModelFactory.CreateCurveMember("c2"), line);

        Assert.True(rel1.HasSameTarget(rel2));
    }

    [Fact]
    public void HasSameTarget_DifferentLineReferences_SameCoords_ReturnsFalse()
    {
        var startPoint = TestModelFactory.CreatePoint("start", 0, 0, 0);
        var endPoint = TestModelFactory.CreatePoint("end", 10, 10, 10);
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc", startPoint, endPoint);

        var startPoint2 = TestModelFactory.CreatePoint("start2", 0, 0, 0);
        var endPoint2 = TestModelFactory.CreatePoint("end2", 10, 10, 10);
        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc", startPoint2, endPoint2);

        var rel1 = new XmiHasLine3d(TestModelFactory.CreateCurveMember("c1"), line1);
        var rel2 = new XmiHasLine3d(TestModelFactory.CreateCurveMember("c2"), line2);

        Assert.False(rel1.HasSameTarget(rel2));
    }

    [Fact]
    public void HasSameTarget_Null_ReturnsFalse()
    {
        var line = TestModelFactory.CreateLine();
        var rel = new XmiHasLine3d(TestModelFactory.CreateCurveMember(), line);

        Assert.False(rel.HasSameTarget(null));
    }

    #endregion

    #region HasDirectionallyEqualTarget (Same Direction)

    [Fact]
    public void HasDirectionallyEqualTarget_SameCoordsSameDirection_ReturnsTrue()
    {
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("s1", 0, 0, 0),
            TestModelFactory.CreatePoint("e1", 10, 10, 10));

        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("s2", 0, 0, 0),
            TestModelFactory.CreatePoint("e2", 10, 10, 10));

        var rel1 = new XmiHasLine3d(TestModelFactory.CreateCurveMember("c1"), line1);
        var rel2 = new XmiHasLine3d(TestModelFactory.CreateCurveMember("c2"), line2);

        Assert.True(rel1.HasDirectionallyEqualTarget(rel2));
    }

    [Fact]
    public void HasDirectionallyEqualTarget_OppositeDirection_ReturnsFalse()
    {
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("s1", 0, 0, 0),
            TestModelFactory.CreatePoint("e1", 10, 10, 10));

        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("s2", 10, 10, 10),
            TestModelFactory.CreatePoint("e2", 0, 0, 0));

        var rel1 = new XmiHasLine3d(TestModelFactory.CreateCurveMember("c1"), line1);
        var rel2 = new XmiHasLine3d(TestModelFactory.CreateCurveMember("c2"), line2);

        Assert.False(rel1.HasDirectionallyEqualTarget(rel2));
    }

    [Fact]
    public void HasDirectionallyEqualTarget_Null_ReturnsFalse()
    {
        var line = TestModelFactory.CreateLine();
        var rel = new XmiHasLine3d(TestModelFactory.CreateCurveMember(), line);

        Assert.False(rel.HasDirectionallyEqualTarget(null));
    }

    #endregion

    #region HasCoincidentTarget (Same Space)

    [Fact]
    public void HasCoincidentTarget_SameDirection_ReturnsTrue()
    {
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("s1", 0, 0, 0),
            TestModelFactory.CreatePoint("e1", 10, 10, 10));

        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("s2", 0, 0, 0),
            TestModelFactory.CreatePoint("e2", 10, 10, 10));

        var rel1 = new XmiHasLine3d(TestModelFactory.CreateCurveMember("c1"), line1);
        var rel2 = new XmiHasLine3d(TestModelFactory.CreateCurveMember("c2"), line2);

        Assert.True(rel1.HasCoincidentTarget(rel2));
    }

    [Fact]
    public void HasCoincidentTarget_OppositeDirection_ReturnsTrue()
    {
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("s1", 0, 0, 0),
            TestModelFactory.CreatePoint("e1", 10, 10, 10));

        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("s2", 10, 10, 10),
            TestModelFactory.CreatePoint("e2", 0, 0, 0));

        var rel1 = new XmiHasLine3d(TestModelFactory.CreateCurveMember("c1"), line1);
        var rel2 = new XmiHasLine3d(TestModelFactory.CreateCurveMember("c2"), line2);

        Assert.True(rel1.HasCoincidentTarget(rel2));
    }

    [Fact]
    public void HasCoincidentTarget_DifferentCoords_ReturnsFalse()
    {
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("s1", 0, 0, 0),
            TestModelFactory.CreatePoint("e1", 10, 10, 10));

        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("s2", 5, 5, 5),
            TestModelFactory.CreatePoint("e2", 15, 15, 15));

        var rel1 = new XmiHasLine3d(TestModelFactory.CreateCurveMember("c1"), line1);
        var rel2 = new XmiHasLine3d(TestModelFactory.CreateCurveMember("c2"), line2);

        Assert.False(rel1.HasCoincidentTarget(rel2));
    }

    [Fact]
    public void HasCoincidentTarget_Null_ReturnsFalse()
    {
        var line = TestModelFactory.CreateLine();
        var rel = new XmiHasLine3d(TestModelFactory.CreateCurveMember(), line);

        Assert.False(rel.HasCoincidentTarget(null));
    }

    #endregion
}
