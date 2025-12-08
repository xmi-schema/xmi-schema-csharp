using XmiSchema.Entities.Bases;

namespace XmiSchema.Enums;

public enum XmiUnitEnum
{
    [EnumValue("m^3")] Meter3,
    [EnumValue("m^2")] Meter2,
    [EnumValue("m")] Meter,
    [EnumValue("m^4")] Meter4,
    [EnumValue("mm^4")] Millimeter4,
    [EnumValue("mm")] Millimeter,
    [EnumValue("cm")] Centimeter,
    [EnumValue("mm^3")] Millimeter3,
    [EnumValue("mm^2")] Millimeter2,
    [EnumValue("sec")] Second,
    [EnumValue("Unknown")] Unknown
}