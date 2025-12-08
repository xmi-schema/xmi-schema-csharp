using System;
using XmiSchema.Entities.Commons;
using XmiSchema.Managers;
using XmiSchema.Entities.Physical;
using XmiSchema.Entities.StructuralAnalytical;
using XmiSchema.Entities.Bases;
using XmiSchema.Parameters;
using XmiSchema.Tests.Support;
using XmiSchema.Enums;
namespace XmiSchema.Tests.Models.Entities;

/// <summary>
/// Tests the cross-section entity covering property assignment and equality.
/// </summary>
public class XmiCrossSectionTests
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

    /// <summary>
    /// Parameters must declare the same shape as the cross-section.
    /// </summary>
    [Fact]
    public void Constructor_ThrowsWhenShapeMismatch()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new XmiCrossSection(
                "sec-mismatch",
                "Section mismatch",
                "ifc",
                "native",
                "desc",
                XmiShapeEnum.Rectangular,
                new IShapeParameters(0.3, 0.15, 0.02, 0.012, 0.008),
                0.18,
                0.002,
                0.003,
                0.01,
                0.02,
                0.0005,
                0.0006,
                0.0007,
                0.0008,
                0.0009));

        Assert.Contains("Parameter set is defined for IShape", exception.Message);
    }
}
