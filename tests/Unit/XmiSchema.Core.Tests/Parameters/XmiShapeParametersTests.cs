using System;
using System.Collections.Generic;
using XmiSchema.Core.Enums;
using XmiSchema.Core.Parameters;

namespace XmiSchema.Core.Tests.Parameters;

/// <summary>
/// Covers the strongly typed shape parameter classes and their lookup metadata.
/// </summary>
public class XmiShapeParametersTests
{
    [Fact]
    public void RectangularShapeParameters_StoresMagnitude()
    {
        var parameters = new RectangularShapeParameters(0.5, 0.3);

        Assert.Equal(XmiShapeEnum.Rectangular, parameters.Shape);
        Assert.Equal(0.5, parameters.Values["H"]);
        Assert.Equal(0.3, parameters.Values["B"]);
    }

    [Fact]
    public void ShapeParameters_ThrowsWhenNegative()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new RectangularShapeParameters(-0.1, 0.3));
        Assert.Contains("Shape parameters must be non-negative", exception.Message);
    }

    [Fact]
    public void OtherShapeParameters_PreservesCustomKeys()
    {
        var parameters = new OtherShapeParameters(new Dictionary<string, double> { ["Custom"] = 1.2 });
        Assert.Equal(1.2, parameters.Values["Custom"]);
    }

    [Fact]
    public void XmiShapeEnumParameters_ReturnsExpectedKeys()
    {
        var success = XmiShapeEnumParameters.TryGetParameters(XmiShapeEnum.Rectangular, out var definition);

        Assert.True(success);
        Assert.NotNull(definition);
        Assert.Contains("H", definition!.ParameterKeys);
        Assert.Contains("B", definition.ParameterKeys);
    }
}
