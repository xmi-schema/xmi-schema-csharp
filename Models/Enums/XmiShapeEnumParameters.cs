using System.Collections.Generic;

namespace XmiSchema.Core.Enums;

/// <summary>
/// Provides the expected parameter set for each <see cref="XmiShapeEnum"/> entry.
/// Values are captured from <c>CrossSectionParameter.docx</c>.
/// </summary>
public static class XmiShapeEnumParameters
{
    private static readonly IReadOnlyDictionary<XmiShapeEnum, XmiShapeParameterDefinition> ShapeParameters =
        new Dictionary<XmiShapeEnum, XmiShapeParameterDefinition>
        {
            [XmiShapeEnum.Circular] = new(new[] { "D" }),
            [XmiShapeEnum.Rectangular] = new(new[] { "H", "B" }),
            [XmiShapeEnum.LShape] = new(new[] { "H", "B", "T", "t" }),
            [XmiShapeEnum.TShape] = new(
                new[] { "H", "B", "T", "t" },
                AlternateParameterSets: new IReadOnlyList<string>[]
                {
                    new[] { "d", "B", "T", "t", "r" }
                }),
            [XmiShapeEnum.LInverted] = new(new[] { "H", "B", "T", "t" }),
            [XmiShapeEnum.TInverted] = new(new[] { "H", "B", "T", "t" }),
            [XmiShapeEnum.CShape] = new(new[] { "H", "B", "T1", "T2", "t" }),
            [XmiShapeEnum.Elbow] = new(new[] { "B1", "B2", "T", "a" }),
            [XmiShapeEnum.Trapezium] = new(new[] { "H", "BTop", "BBot" }),
            [XmiShapeEnum.Parallelogram] = new(new[] { "B", "L", "a" }),
            [XmiShapeEnum.Polygon] = new(new[] { "N", "R" }),
            [XmiShapeEnum.IShape] = new(new[] { "D", "B", "T", "t", "r" }),
            [XmiShapeEnum.CircularHollow] = new(new[] { "D", "t" }),
            [XmiShapeEnum.SquareHollow] = new(new[] { "D", "t" }),
            [XmiShapeEnum.RectangularHollow] = new(new[] { "D", "B", "t" }),
            [XmiShapeEnum.TaperedFlangeChannel] = new(new[] { "D", "B", "T", "t" }),
            [XmiShapeEnum.ParallelFlangeChannel] = new(new[] { "D", "B", "T", "t", "r1" }),
            [XmiShapeEnum.PlainChannel] = new(new[] { "D", "B", "t" }),
            [XmiShapeEnum.LippedChannel] = new(new[] { "D", "B", "C", "t", "r1", "r2" }),
            [XmiShapeEnum.ZPurlin] = new(new[] { "D", "E", "F", "L", "t" }, Notes: "E > F"),
            [XmiShapeEnum.EqualAngle] = new(new[] { "A", "t", "r1", "r2" }, Notes: "r1 > r2"),
            [XmiShapeEnum.UnequalAngle] = new(new[] { "A", "B", "t", "r1", "r2" }),
            [XmiShapeEnum.FlatBar] = new(new[] { "B", "t" }),
            [XmiShapeEnum.SquareBar] = new(new[] { "a" }),
            [XmiShapeEnum.DeformedBar] = new(new[] { "D" }),
            [XmiShapeEnum.RoundBar] = new(new[] { "D" }),
            [XmiShapeEnum.Others] = new(Array.Empty<string>()),
            [XmiShapeEnum.Unknown] = new(Array.Empty<string>())
        };

    public static IReadOnlyDictionary<XmiShapeEnum, XmiShapeParameterDefinition> Definitions => ShapeParameters;

    public static bool TryGetParameters(XmiShapeEnum shape, out XmiShapeParameterDefinition? definition) =>
        ShapeParameters.TryGetValue(shape, out definition);
}

public sealed record XmiShapeParameterDefinition(
    IReadOnlyList<string> ParameterKeys,
    IReadOnlyList<IReadOnlyList<string>>? AlternateParameterSets = null,
    string? Notes = null);
