using System;
using System.Collections.Generic;
using XmiSchema.Models.Bases;
using XmiSchema.Core.Parameters;

namespace XmiSchema.Tests.Parameters;

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

    [Fact]
    public void IShapeParameters_StoresMagnitude()
    {
        var parameters = new IShapeParameters(400, 200, 10, 8, 12);

        Assert.Equal(XmiShapeEnum.IShape, parameters.Shape);
        Assert.Equal(400, parameters.Values["D"]);
        Assert.Equal(200, parameters.Values["B"]);
        Assert.Equal(8, parameters.Values["t"]);
        Assert.Equal(12, parameters.Values["r"]);
    }

    [Fact]
    public void TShapeParameters_StoresMagnitude()
    {
        var parameters = new TShapeParameters(300, 150, 12, 8);

        Assert.Equal(XmiShapeEnum.TShape, parameters.Shape);
        Assert.Equal(300, parameters.Values["H"]);
        Assert.Equal(150, parameters.Values["B"]);
        Assert.Equal(8, parameters.Values["t"]);
    }

    [Fact]
    public void TShapeParameters_WithRadius_StoresMagnitude()
    {
        var parameters = new TShapeParameters(300, 150, 12, 8, 10);

        Assert.Equal(XmiShapeEnum.TShape, parameters.Shape);
        Assert.Equal(300, parameters.Values["d"]);
        Assert.Equal(150, parameters.Values["B"]);
        Assert.Equal(8, parameters.Values["t"]);
        Assert.Equal(10, parameters.Values["r"]);
    }

    [Fact]
    public void LShapeParameters_StoresMagnitude()
    {
        var parameters = new LShapeParameters(100, 100, 10, 8);

        Assert.Equal(XmiShapeEnum.LShape, parameters.Shape);
        Assert.Equal(100, parameters.H);
        Assert.Equal(100, parameters.B);
        Assert.Equal(8, parameters.t);
    }

    [Fact]
    public void CShapeParameters_StoresMagnitude()
    {
        var parameters = new CShapeParameters(200, 80, 10, 12, 6);

        Assert.Equal(XmiShapeEnum.CShape, parameters.Shape);
        Assert.Equal(200, parameters.Values["H"]);
        Assert.Equal(80, parameters.Values["B"]);
        Assert.Equal(10, parameters.Values["T1"]);
        Assert.Equal(12, parameters.Values["T2"]);
        Assert.Equal(6, parameters.Values["t"]);
    }

    [Fact]
    public void EqualAngleShapeParameters_StoresMagnitude()
    {
        var parameters = new EqualAngleShapeParameters(75, 8, 10, 5);

        Assert.Equal(XmiShapeEnum.EqualAngle, parameters.Shape);
        Assert.Equal(75, parameters.Values["A"]);
        Assert.Equal(8, parameters.Values["t"]);
        Assert.Equal(10, parameters.Values["r1"]);
        Assert.Equal(5, parameters.Values["r2"]);
    }

    [Fact]
    public void UnequalAngleShapeParameters_StoresMagnitude()
    {
        var parameters = new UnequalAngleShapeParameters(100, 75, 10, 12, 6);

        Assert.Equal(XmiShapeEnum.UnequalAngle, parameters.Shape);
        Assert.Equal(100, parameters.Values["A"]);
        Assert.Equal(75, parameters.Values["B"]);
        Assert.Equal(10, parameters.Values["t"]);
        Assert.Equal(12, parameters.Values["r1"]);
        Assert.Equal(6, parameters.Values["r2"]);
    }

    [Fact]
    public void CircularShapeParameters_StoresMagnitude()
    {
        var parameters = new CircularShapeParameters(250);

        Assert.Equal(XmiShapeEnum.Circular, parameters.Shape);
        Assert.Equal(250, parameters.D);
    }

    [Fact]
    public void CircularHollowShapeParameters_StoresMagnitude()
    {
        var parameters = new CircularHollowShapeParameters(200, 10);

        Assert.Equal(XmiShapeEnum.CircularHollow, parameters.Shape);
        Assert.Equal(200, parameters.Values["D"]);
        Assert.Equal(10, parameters.Values["t"]);
    }

    [Fact]
    public void SquareHollowShapeParameters_StoresMagnitude()
    {
        var parameters = new SquareHollowShapeParameters(150, 8);

        Assert.Equal(XmiShapeEnum.SquareHollow, parameters.Shape);
        Assert.Equal(150, parameters.Values["D"]);
        Assert.Equal(8, parameters.Values["t"]);
    }

    [Fact]
    public void RectangularHollowShapeParameters_StoresMagnitude()
    {
        var parameters = new RectangularHollowShapeParameters(200, 100, 10);

        Assert.Equal(XmiShapeEnum.RectangularHollow, parameters.Shape);
        Assert.Equal(200, parameters.Values["D"]);
        Assert.Equal(100, parameters.Values["B"]);
        Assert.Equal(10, parameters.Values["t"]);
    }

    [Fact]
    public void TrapeziumShapeParameters_StoresMagnitude()
    {
        var parameters = new TrapeziumShapeParameters(100, 80, 120);

        Assert.Equal(XmiShapeEnum.Trapezium, parameters.Shape);
        Assert.Equal(100, parameters.Values["H"]);
        Assert.Equal(80, parameters.Values["BTop"]);
        Assert.Equal(120, parameters.Values["BBot"]);
    }

    [Fact]
    public void ParallelogramShapeParameters_StoresMagnitude()
    {
        var parameters = new ParallelogramShapeParameters(100, 80, 45);

        Assert.Equal(XmiShapeEnum.Parallelogram, parameters.Shape);
        Assert.Equal(100, parameters.Values["B"]);
        Assert.Equal(80, parameters.Values["L"]);
        Assert.Equal(45, parameters.Values["a"]);
    }

    [Fact]
    public void PolygonShapeParameters_StoresMagnitude()
    {
        var parameters = new PolygonShapeParameters(6, 50);

        Assert.Equal(XmiShapeEnum.Polygon, parameters.Shape);
        Assert.Equal(6, parameters.Values["N"]);
        Assert.Equal(50, parameters.Values["R"]);
    }

    [Fact]
    public void FlatBarShapeParameters_StoresMagnitude()
    {
        var parameters = new FlatBarShapeParameters(100, 10);

        Assert.Equal(XmiShapeEnum.FlatBar, parameters.Shape);
        Assert.Equal(100, parameters.Values["B"]);
        Assert.Equal(10, parameters.Values["t"]);
    }

    [Fact]
    public void RoundBarShapeParameters_StoresMagnitude()
    {
        var parameters = new RoundBarShapeParameters(16);

        Assert.Equal(XmiShapeEnum.RoundBar, parameters.Shape);
        Assert.Equal(16, parameters.Values["D"]);
    }

    [Fact]
    public void IShapeParameters_ThrowsWhenNegativeDepth()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new IShapeParameters(-400, 200, 10, 8, 12));
        Assert.Contains("Shape parameters must be non-negative", exception.Message);
    }

    [Fact]
    public void TShapeParameters_ThrowsWhenNegativeWidth()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new TShapeParameters(300, -150, 12, 8));
        Assert.Contains("Shape parameters must be non-negative", exception.Message);
    }

    [Fact]
    public void CircularShapeParameters_ThrowsWhenNegativeDiameter()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new CircularShapeParameters(-250));
        Assert.Contains("Shape parameters must be non-negative", exception.Message);
    }

    [Fact]
    public void LShapeParameters_AllowsZeroValues()
    {
        var parameters = new LShapeParameters(0, 0, 0, 0);

        Assert.Equal(0, parameters.H);
        Assert.Equal(0, parameters.B);
        Assert.Equal(0, parameters.T);
        Assert.Equal(0, parameters.t);
    }

    [Fact]
    public void UnknownShapeParameters_PreservesCustomKeys()
    {
        var customValues = new Dictionary<string, double>
        {
            ["Param1"] = 100.5,
            ["Param2"] = 200.75
        };
        var parameters = new UnknownShapeParameters(customValues);

        Assert.Equal(XmiShapeEnum.Unknown, parameters.Shape);
        Assert.Equal(100.5, parameters.Values["Param1"]);
        Assert.Equal(200.75, parameters.Values["Param2"]);
    }
}
