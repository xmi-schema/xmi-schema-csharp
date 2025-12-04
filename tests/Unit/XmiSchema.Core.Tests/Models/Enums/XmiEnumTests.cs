using XmiSchema.Core.Enums;

namespace XmiSchema.Core.Tests.Models.Enums;

/// <summary>
/// Validates all enum types have expected values and can be properly constructed.
/// </summary>
public class XmiEnumTests
{
    [Fact]
    public void XmiBaseEntityDomainEnum_HasExpectedValues()
    {
        Assert.Equal(4, (int)XmiBaseEntityDomainEnum.Shared);
        Assert.Equal(0, (int)XmiBaseEntityDomainEnum.Physical);
        Assert.Equal(3, (int)XmiBaseEntityDomainEnum.Functional);
    }

    [Fact]
    public void XmiSegmentTypeEnum_HasExpectedValues()
    {
        Assert.Equal(0, (int)XmiSegmentTypeEnum.Line);
        Assert.Equal(1, (int)XmiSegmentTypeEnum.CircularArc);
    }

    [Fact]
    public void XmiSegmentTypeEnum_CanBeAssigned()
    {
        XmiSegmentTypeEnum type = XmiSegmentTypeEnum.Line;
        Assert.Equal(XmiSegmentTypeEnum.Line, type);

        type = XmiSegmentTypeEnum.CircularArc;
        Assert.Equal(XmiSegmentTypeEnum.CircularArc, type);
    }

    [Fact]
    public void XmiStructuralCurveMemberTypeEnum_HasExpectedValues()
    {
        Assert.Equal(0, (int)XmiStructuralCurveMemberTypeEnum.Beam);
        Assert.Equal(1, (int)XmiStructuralCurveMemberTypeEnum.Column);
        Assert.Equal(2, (int)XmiStructuralCurveMemberTypeEnum.Bracing);
    }

    [Fact]
    public void XmiStructuralMaterialTypeEnum_HasExpectedValues()
    {
        Assert.Equal(0, (int)XmiStructuralMaterialTypeEnum.Concrete);
        Assert.Equal(1, (int)XmiStructuralMaterialTypeEnum.Steel);
        Assert.Equal(2, (int)XmiStructuralMaterialTypeEnum.Timber);
        Assert.Equal(3, (int)XmiStructuralMaterialTypeEnum.Aluminium);
        Assert.Equal(5, (int)XmiStructuralMaterialTypeEnum.Masonry);
    }

    [Fact]
    public void XmiStructuralSurfaceMemberTypeEnum_HasExpectedValues()
    {
        Assert.Equal(1, (int)XmiStructuralSurfaceMemberTypeEnum.Wall);
        Assert.Equal(0, (int)XmiStructuralSurfaceMemberTypeEnum.Slab);
    }

    [Fact]
    public void XmiStructuralSurfaceMemberSpanTypeEnum_HasExpectedValues()
    {
        Assert.Equal(0, (int)XmiStructuralSurfaceMemberSpanTypeEnum.OneWay);
        Assert.Equal(1, (int)XmiStructuralSurfaceMemberSpanTypeEnum.TwoWay);
    }

    [Fact]
    public void XmiStructuralSurfaceMemberSystemPlaneEnum_HasExpectedValues()
    {
        Assert.Equal(0, (int)XmiStructuralSurfaceMemberSystemPlaneEnum.Bottom);
        Assert.Equal(1, (int)XmiStructuralSurfaceMemberSystemPlaneEnum.Top);
        Assert.Equal(2, (int)XmiStructuralSurfaceMemberSystemPlaneEnum.Middle);
    }

    [Fact]
    public void XmiUnitEnum_HasExpectedValues()
    {
        Assert.Equal(2, (int)XmiUnitEnum.Meter);
        Assert.Equal(5, (int)XmiUnitEnum.Millimeter);
        Assert.Equal(6, (int)XmiUnitEnum.Centimeter);
    }

    [Fact]
    public void XmiShapeEnum_HasExpectedValues()
    {
        // Test a sample of shape enum values
        Assert.Equal(11, (int)XmiShapeEnum.IShape);
        Assert.Equal(0, (int)XmiShapeEnum.Rectangular);
        Assert.Equal(1, (int)XmiShapeEnum.Circular);
        Assert.Equal(2, (int)XmiShapeEnum.LShape);
        Assert.Equal(3, (int)XmiShapeEnum.TShape);
        Assert.Equal(6, (int)XmiShapeEnum.CShape);
    }

    [Fact]
    public void XmiShapeEnum_HasAllShapeTypes()
    {
        var allShapes = Enum.GetValues(typeof(XmiShapeEnum));

        // Verify we have multiple shape types
        Assert.True(allShapes.Length > 20, "Expected more than 20 shape types");
    }

    [Fact]
    public void XmiBaseEntityDomainEnum_CanBeCompared()
    {
        var domain1 = XmiBaseEntityDomainEnum.Physical;
        var domain2 = XmiBaseEntityDomainEnum.Physical;
        var domain3 = XmiBaseEntityDomainEnum.Functional;

        Assert.Equal(domain1, domain2);
        Assert.NotEqual(domain1, domain3);
    }

    [Fact]
    public void XmiStructuralMaterialTypeEnum_CanBeUsedInSwitch()
    {
        var materialType = XmiStructuralMaterialTypeEnum.Steel;

        string result = materialType switch
        {
            XmiStructuralMaterialTypeEnum.Concrete => "Concrete",
            XmiStructuralMaterialTypeEnum.Steel => "Steel",
            XmiStructuralMaterialTypeEnum.Timber => "Timber",
            XmiStructuralMaterialTypeEnum.Aluminium => "Aluminium",
            XmiStructuralMaterialTypeEnum.Masonry => "Masonry",
            XmiStructuralMaterialTypeEnum.Others => "Others",
            _ => "Unknown"
        };

        Assert.Equal("Steel", result);
    }

    [Fact]
    public void AllEnums_CanBeConvertedToString()
    {
        Assert.Equal("Shared", XmiBaseEntityDomainEnum.Shared.ToString());
        Assert.Equal("Line", XmiSegmentTypeEnum.Line.ToString());
        Assert.Equal("Beam", XmiStructuralCurveMemberTypeEnum.Beam.ToString());
        Assert.Equal("Concrete", XmiStructuralMaterialTypeEnum.Concrete.ToString());
        Assert.Equal("Wall", XmiStructuralSurfaceMemberTypeEnum.Wall.ToString());
        Assert.Equal("OneWay", XmiStructuralSurfaceMemberSpanTypeEnum.OneWay.ToString());
        Assert.Equal("Meter", XmiUnitEnum.Meter.ToString());
        Assert.Equal("IShape", XmiShapeEnum.IShape.ToString());
    }

    [Fact]
    public void AllEnums_CanBeParsedFromString()
    {
        Assert.Equal(XmiBaseEntityDomainEnum.Physical,
            Enum.Parse<XmiBaseEntityDomainEnum>("Physical"));
        Assert.Equal(XmiSegmentTypeEnum.CircularArc,
            Enum.Parse<XmiSegmentTypeEnum>("CircularArc"));
        Assert.Equal(XmiStructuralCurveMemberTypeEnum.Column,
            Enum.Parse<XmiStructuralCurveMemberTypeEnum>("Column"));
        Assert.Equal(XmiStructuralMaterialTypeEnum.Steel,
            Enum.Parse<XmiStructuralMaterialTypeEnum>("Steel"));
    }

    [Fact]
    public void InvalidEnum_ThrowsWhenParsing()
    {
        Assert.Throws<ArgumentException>(() =>
            Enum.Parse<XmiSegmentTypeEnum>("InvalidValue"));
    }

    [Fact]
    public void EnumValues_CanBeIterated()
    {
        var materialTypes = Enum.GetValues<XmiStructuralMaterialTypeEnum>();

        Assert.Contains(XmiStructuralMaterialTypeEnum.Concrete, materialTypes);
        Assert.Contains(XmiStructuralMaterialTypeEnum.Steel, materialTypes);
        Assert.Contains(XmiStructuralMaterialTypeEnum.Timber, materialTypes);
        Assert.Equal(10, materialTypes.Length);
    }

    [Fact]
    public void XmiStructuralCurveMemberTypeEnum_AllValuesAreValid()
    {
        var values = Enum.GetValues<XmiStructuralCurveMemberTypeEnum>();

        foreach (var value in values)
        {
            Assert.True(Enum.IsDefined(typeof(XmiStructuralCurveMemberTypeEnum), value));
        }
    }
}
