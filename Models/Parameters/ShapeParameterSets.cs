using System.Collections.Generic;
using XmiSchema.Core.Enums;

namespace XmiSchema.Core.Parameters;

public sealed class RectangularShapeParameters : XmiShapeParametersBase
{
    public RectangularShapeParameters(double height, double width)
        : base(XmiShapeEnum.Rectangular, Build(("H", height), ("B", width)))
    {
    }

    public double H => Values["H"];
    public double B => Values["B"];
}

public sealed class CircularShapeParameters : XmiShapeParametersBase
{
    public CircularShapeParameters(double diameter)
        : base(XmiShapeEnum.Circular, Build(("D", diameter)))
    {
    }

    public double D => Values["D"];
}

public sealed class LShapeParameters : XmiShapeParametersBase
{
    public LShapeParameters(double height, double width, double flangeThickness, double webThickness)
        : base(XmiShapeEnum.LShape, Build(("H", height), ("B", width), ("T", flangeThickness), ("t", webThickness)))
    {
    }

    public double H => Values["H"];
    public double B => Values["B"];
    public double T => Values["T"];
    public double t => Values["t"];
}

public sealed class TShapeParameters : XmiShapeParametersBase
{
    public TShapeParameters(double height, double width, double flangeThickness, double webThickness)
        : base(XmiShapeEnum.TShape, Build(("H", height), ("B", width), ("T", flangeThickness), ("t", webThickness)))
    {
    }

    public TShapeParameters(double depth, double width, double flangeThickness, double webThickness, double radius)
        : base(XmiShapeEnum.TShape, Build(("d", depth), ("B", width), ("T", flangeThickness), ("t", webThickness), ("r", radius)))
    {
    }
}

public sealed class LInvertedShapeParameters : XmiShapeParametersBase
{
    public LInvertedShapeParameters(double height, double width, double flangeThickness, double webThickness)
        : base(XmiShapeEnum.LInverted, Build(("H", height), ("B", width), ("T", flangeThickness), ("t", webThickness)))
    {
    }
}

public sealed class TInvertedShapeParameters : XmiShapeParametersBase
{
    public TInvertedShapeParameters(double height, double width, double flangeThickness, double webThickness)
        : base(XmiShapeEnum.TInverted, Build(("H", height), ("B", width), ("T", flangeThickness), ("t", webThickness)))
    {
    }
}

public sealed class CShapeParameters : XmiShapeParametersBase
{
    public CShapeParameters(double height, double width, double topFlangeThickness, double bottomFlangeThickness, double webThickness)
        : base(XmiShapeEnum.CShape, Build(("H", height), ("B", width), ("T1", topFlangeThickness), ("T2", bottomFlangeThickness), ("t", webThickness)))
    {
    }
}

public sealed class ElbowShapeParameters : XmiShapeParametersBase
{
    public ElbowShapeParameters(double breadth1, double breadth2, double thickness, double angle)
        : base(XmiShapeEnum.Elbow, Build(("B1", breadth1), ("B2", breadth2), ("T", thickness), ("a", angle)))
    {
    }
}

public sealed class TrapeziumShapeParameters : XmiShapeParametersBase
{
    public TrapeziumShapeParameters(double height, double topWidth, double bottomWidth)
        : base(XmiShapeEnum.Trapezium, Build(("H", height), ("BTop", topWidth), ("BBot", bottomWidth)))
    {
    }
}

public sealed class ParallelogramShapeParameters : XmiShapeParametersBase
{
    public ParallelogramShapeParameters(double baseLength, double sideLength, double angle)
        : base(XmiShapeEnum.Parallelogram, Build(("B", baseLength), ("L", sideLength), ("a", angle)))
    {
    }
}

public sealed class PolygonShapeParameters : XmiShapeParametersBase
{
    public PolygonShapeParameters(double sides, double radius)
        : base(XmiShapeEnum.Polygon, Build(("N", sides), ("R", radius)))
    {
    }
}

public sealed class IShapeParameters : XmiShapeParametersBase
{
    public IShapeParameters(double depth, double width, double flangeThickness, double webThickness, double radius)
        : base(XmiShapeEnum.IShape, Build(("D", depth), ("B", width), ("T", flangeThickness), ("t", webThickness), ("r", radius)))
    {
    }
}

public sealed class CircularHollowShapeParameters : XmiShapeParametersBase
{
    public CircularHollowShapeParameters(double diameter, double thickness)
        : base(XmiShapeEnum.CircularHollow, Build(("D", diameter), ("t", thickness)))
    {
    }
}

public sealed class SquareHollowShapeParameters : XmiShapeParametersBase
{
    public SquareHollowShapeParameters(double diameter, double thickness)
        : base(XmiShapeEnum.SquareHollow, Build(("D", diameter), ("t", thickness)))
    {
    }
}

public sealed class RectangularHollowShapeParameters : XmiShapeParametersBase
{
    public RectangularHollowShapeParameters(double depth, double width, double thickness)
        : base(XmiShapeEnum.RectangularHollow, Build(("D", depth), ("B", width), ("t", thickness)))
    {
    }
}

public sealed class TaperedFlangeChannelShapeParameters : XmiShapeParametersBase
{
    public TaperedFlangeChannelShapeParameters(double depth, double width, double flangeThickness, double webThickness)
        : base(XmiShapeEnum.TaperedFlangeChannel, Build(("D", depth), ("B", width), ("T", flangeThickness), ("t", webThickness)))
    {
    }
}

public sealed class ParallelFlangeChannelShapeParameters : XmiShapeParametersBase
{
    public ParallelFlangeChannelShapeParameters(double depth, double width, double flangeThickness, double webThickness, double radius1)
        : base(XmiShapeEnum.ParallelFlangeChannel, Build(("D", depth), ("B", width), ("T", flangeThickness), ("t", webThickness), ("r1", radius1)))
    {
    }
}

public sealed class PlainChannelShapeParameters : XmiShapeParametersBase
{
    public PlainChannelShapeParameters(double depth, double width, double thickness)
        : base(XmiShapeEnum.PlainChannel, Build(("D", depth), ("B", width), ("t", thickness)))
    {
    }
}

public sealed class LippedChannelShapeParameters : XmiShapeParametersBase
{
    public LippedChannelShapeParameters(double depth, double width, double lip, double thickness, double radius1, double radius2)
        : base(XmiShapeEnum.LippedChannel, Build(("D", depth), ("B", width), ("C", lip), ("t", thickness), ("r1", radius1), ("r2", radius2)))
    {
    }
}

public sealed class ZPurlinShapeParameters : XmiShapeParametersBase
{
    public ZPurlinShapeParameters(double depth, double e, double f, double length, double thickness)
        : base(XmiShapeEnum.ZPurlin, Build(("D", depth), ("E", e), ("F", f), ("L", length), ("t", thickness)))
    {
    }
}

public sealed class EqualAngleShapeParameters : XmiShapeParametersBase
{
    public EqualAngleShapeParameters(double leg, double thickness, double radius1, double radius2)
        : base(XmiShapeEnum.EqualAngle, Build(("A", leg), ("t", thickness), ("r1", radius1), ("r2", radius2)))
    {
    }
}

public sealed class UnequalAngleShapeParameters : XmiShapeParametersBase
{
    public UnequalAngleShapeParameters(double legA, double legB, double thickness, double radius1, double radius2)
        : base(XmiShapeEnum.UnequalAngle, Build(("A", legA), ("B", legB), ("t", thickness), ("r1", radius1), ("r2", radius2)))
    {
    }
}

public sealed class FlatBarShapeParameters : XmiShapeParametersBase
{
    public FlatBarShapeParameters(double width, double thickness)
        : base(XmiShapeEnum.FlatBar, Build(("B", width), ("t", thickness)))
    {
    }
}

public sealed class SquareBarShapeParameters : XmiShapeParametersBase
{
    public SquareBarShapeParameters(double side)
        : base(XmiShapeEnum.SquareBar, Build(("a", side)))
    {
    }
}

public sealed class DeformedBarShapeParameters : XmiShapeParametersBase
{
    public DeformedBarShapeParameters(double diameter)
        : base(XmiShapeEnum.DeformedBar, Build(("D", diameter)))
    {
    }
}

public sealed class RoundBarShapeParameters : XmiShapeParametersBase
{
    public RoundBarShapeParameters(double diameter)
        : base(XmiShapeEnum.RoundBar, Build(("D", diameter)))
    {
    }
}

public sealed class OtherShapeParameters : XmiShapeParametersBase
{
    public OtherShapeParameters(IDictionary<string, double> values)
        : base(XmiShapeEnum.Others, values)
    {
    }
}

public sealed class UnknownShapeParameters : XmiShapeParametersBase
{
    public UnknownShapeParameters(IDictionary<string, double> values)
        : base(XmiShapeEnum.Unknown, values)
    {
    }
}
