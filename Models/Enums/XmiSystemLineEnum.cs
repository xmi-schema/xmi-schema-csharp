using XmiSchema.Models.Bases;

namespace XmiSchema.Models.Enums;
public enum XmiSystemLineEnum
{
    [EnumValue("TopMiddle")] TopMiddle,
    [EnumValue("TopLeft")] TopLeft,
    [EnumValue("TopRight")] TopRight,
    [EnumValue("MiddleMiddle")] MiddleMiddle,
    [EnumValue("MiddleLeft")] MiddleLeft,
    [EnumValue("MiddleRight")] MiddleRight,
    [EnumValue("BottomLeft")] BottomLeft,
    [EnumValue("BottomMiddle")] BottomMiddle,
    [EnumValue("BottomRight")] BottomRight,
    [EnumValue("Unknown")] Unknown
}