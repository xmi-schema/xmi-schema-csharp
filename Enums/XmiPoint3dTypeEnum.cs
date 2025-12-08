using XmiSchema.Entities.Bases;

namespace XmiSchema.Enums;

/// <summary>
/// Identifies the role of a point within a geometry relationship.
/// </summary>
public enum XmiPoint3dTypeEnum
{
    [EnumValue("Start")] Start,
    [EnumValue("End")] End,
    [EnumValue("Center")] Center
}
