using XmiSchema.Entities.Geometries;
using XmiSchema.Enums;
using XmiSchema.Tests.Managers;

namespace XmiSchema.Tests.Entities.Geometries;

/// <summary>
/// Verifies the <see cref="XmiLine3d"/> geometric endpoints are stored
/// and equality comparisons work correctly.
/// </summary>
public class XmiLine3dTests
{
    /// <summary>
    /// Ensures the constructor accepts and exposes both endpoints.
    /// </summary>
    [Fact]
    public void Constructor_AssignsEndpoints()
    {
        var line = TestModelFactory.CreateLine();

        Assert.NotNull(line.StartPoint);
        Assert.NotNull(line.EndPoint);
        Assert.Equal("line-start", line.StartPoint.Id);
    }

    #region Equals (Referential Equality)

    /// <summary>
    /// Equals returns true when both lines share the same XmiPoint3d references.
    /// </summary>
    [Fact]
    public void Equals_SameReferences_ReturnsTrue()
    {
        var startPoint = TestModelFactory.CreatePoint("start", 0, 0, 0);
        var endPoint = TestModelFactory.CreatePoint("end", 10, 10, 10);

        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc", startPoint, endPoint);
        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc", startPoint, endPoint);

        Assert.True(line1.Equals(line2));
    }

    /// <summary>
    /// Equals returns false when lines have different XmiPoint3d instances,
    /// even if coordinates are identical.
    /// </summary>
    [Fact]
    public void Equals_DifferentReferences_SameCoordinates_ReturnsFalse()
    {
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("start1", 0, 0, 0),
            TestModelFactory.CreatePoint("end1", 10, 10, 10));

        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("start2", 0, 0, 0),
            TestModelFactory.CreatePoint("end2", 10, 10, 10));

        Assert.False(line1.Equals(line2));
    }

    /// <summary>
    /// Equals returns false when comparing with null.
    /// </summary>
    [Fact]
    public void Equals_Null_ReturnsFalse()
    {
        var line = TestModelFactory.CreateLine();

        Assert.False(line.Equals(null));
    }

    #endregion

    #region IsDirectionallyEqual (Same Direction)

    /// <summary>
    /// IsDirectionallyEqual returns true when coordinates match and direction is the same.
    /// </summary>
    [Fact]
    public void IsDirectionallyEqual_SameCoordinates_SameDirection_ReturnsTrue()
    {
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("start1", 0, 0, 0),
            TestModelFactory.CreatePoint("end1", 10, 10, 10));

        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("start2", 0, 0, 0),
            TestModelFactory.CreatePoint("end2", 10, 10, 10));

        Assert.True(line1.IsDirectionallyEqual(line2));
    }

    /// <summary>
    /// IsDirectionallyEqual returns true when coordinates are within tolerance.
    /// </summary>
    [Fact]
    public void IsDirectionallyEqual_CoordinatesWithinTolerance_ReturnsTrue()
    {
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("start1", 0, 0, 0),
            TestModelFactory.CreatePoint("end1", 10, 10, 10));

        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("start2", 0 + 1e-11, 0, 0),
            TestModelFactory.CreatePoint("end2", 10 + 1e-11, 10, 10));

        Assert.True(line1.IsDirectionallyEqual(line2));
    }

    /// <summary>
    /// IsDirectionallyEqual returns false when lines have opposite direction.
    /// </summary>
    [Fact]
    public void IsDirectionallyEqual_OppositeDirection_ReturnsFalse()
    {
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("start1", 0, 0, 0),
            TestModelFactory.CreatePoint("end1", 10, 10, 10));

        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("start2", 10, 10, 10),
            TestModelFactory.CreatePoint("end2", 0, 0, 0));

        Assert.False(line1.IsDirectionallyEqual(line2));
    }

    /// <summary>
    /// IsDirectionallyEqual returns false when coordinates differ.
    /// </summary>
    [Fact]
    public void IsDirectionallyEqual_DifferentCoordinates_ReturnsFalse()
    {
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("start1", 0, 0, 0),
            TestModelFactory.CreatePoint("end1", 10, 10, 10));

        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("start2", 0, 0, 0),
            TestModelFactory.CreatePoint("end2", 20, 20, 20));

        Assert.False(line1.IsDirectionallyEqual(line2));
    }

    /// <summary>
    /// IsDirectionallyEqual returns false when comparing with null.
    /// </summary>
    [Fact]
    public void IsDirectionallyEqual_Null_ReturnsFalse()
    {
        var line = TestModelFactory.CreateLine();

        Assert.False(line.IsDirectionallyEqual(null));
    }

    #endregion

    #region IsCoincident (Same Segment Regardless of Direction)

    /// <summary>
    /// IsCoincident returns true when lines have the same direction.
    /// </summary>
    [Fact]
    public void IsCoincident_SameDirection_ReturnsTrue()
    {
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("start1", 0, 0, 0),
            TestModelFactory.CreatePoint("end1", 10, 10, 10));

        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("start2", 0, 0, 0),
            TestModelFactory.CreatePoint("end2", 10, 10, 10));

        Assert.True(line1.IsCoincident(line2));
    }

    /// <summary>
    /// IsCoincident returns true when lines have opposite direction (A->B vs B->A).
    /// </summary>
    [Fact]
    public void IsCoincident_OppositeDirection_ReturnsTrue()
    {
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("start1", 0, 0, 0),
            TestModelFactory.CreatePoint("end1", 10, 10, 10));

        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("start2", 10, 10, 10),
            TestModelFactory.CreatePoint("end2", 0, 0, 0));

        Assert.True(line1.IsCoincident(line2));
    }

    /// <summary>
    /// IsCoincident returns true when coordinates are within tolerance, opposite direction.
    /// </summary>
    [Fact]
    public void IsCoincident_OppositeDirection_WithinTolerance_ReturnsTrue()
    {
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("start1", 0, 0, 0),
            TestModelFactory.CreatePoint("end1", 10, 10, 10));

        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("start2", 10 + 1e-11, 10, 10),
            TestModelFactory.CreatePoint("end2", 0 + 1e-11, 0, 0));

        Assert.True(line1.IsCoincident(line2));
    }

    /// <summary>
    /// IsCoincident returns false when lines occupy different space.
    /// </summary>
    [Fact]
    public void IsCoincident_DifferentCoordinates_ReturnsFalse()
    {
        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc",
            TestModelFactory.CreatePoint("start1", 0, 0, 0),
            TestModelFactory.CreatePoint("end1", 10, 10, 10));

        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc",
            TestModelFactory.CreatePoint("start2", 5, 5, 5),
            TestModelFactory.CreatePoint("end2", 15, 15, 15));

        Assert.False(line1.IsCoincident(line2));
    }

    /// <summary>
    /// IsCoincident returns false when comparing with null.
    /// </summary>
    [Fact]
    public void IsCoincident_Null_ReturnsFalse()
    {
        var line = TestModelFactory.CreateLine();

        Assert.False(line.IsCoincident(null));
    }

    #endregion

    #region GetHashCode

    /// <summary>
    /// GetHashCode returns consistent values for lines with same point references.
    /// </summary>
    [Fact]
    public void GetHashCode_SameReferences_SameHashCode()
    {
        var startPoint = TestModelFactory.CreatePoint("start", 0, 0, 0);
        var endPoint = TestModelFactory.CreatePoint("end", 10, 10, 10);

        var line1 = new XmiLine3d("line-1", "Line 1", "ifc-1", "N1", "desc", startPoint, endPoint);
        var line2 = new XmiLine3d("line-2", "Line 2", "ifc-2", "N2", "desc", startPoint, endPoint);

        Assert.Equal(line1.GetHashCode(), line2.GetHashCode());
    }

    #endregion
}
