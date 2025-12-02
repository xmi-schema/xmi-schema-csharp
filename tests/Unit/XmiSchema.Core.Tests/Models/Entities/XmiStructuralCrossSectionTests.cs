using XmiSchema.Core.Entities;
using XmiSchema.Core.Enums;

namespace XmiSchema.Core.Tests.Models.Entities;

/// <summary>
/// Tests the cross-section entity covering property assignment and equality.
/// </summary>
public class XmiStructuralCrossSectionTests
{
    /// <summary>
    /// Ensures the constructor records all section properties.
    /// </summary>
    [Fact]
    public void Constructor_PersistsSectionProperties()
    {
        var section = TestModelFactory.CreateCrossSection();

        Assert.Equal(XmiShapeEnum.Rectangular, section.Shape);
        Assert.Equal(0.18, section.Area);
        Assert.Equal(0.0009, section.TorsionalConstant);
    }

    /// <summary>
    /// Equality checks rely on the native identifier to avoid duplicates.
    /// </summary>
    [Fact]
    public void Equals_ReturnsTrueForMatchingNativeId()
    {
        var first = TestModelFactory.CreateCrossSection("sec-match");
        var second = TestModelFactory.CreateCrossSection("sec-match");

        Assert.True(first.Equals(second));
    }
}
