using XmiSchema.Entities.Bases;

namespace XmiSchema.Enums;
public enum XmiSegmentTypeEnum
{
    [EnumValue("Line")] Line,
    [EnumValue("Circular Arc")] CircularArc,
    [EnumValue("Parabolic Arc")] ParabolicArc,
    [EnumValue("Bezier")] Bezier,
    [EnumValue("Spline")] Spline,
    [EnumValue("Others")] Others,
    [EnumValue("Unknown")] Unknown
}