using XmiSchema.Entities.Bases;

namespace XmiSchema.Enums;

public enum XmiStructuralCurveMemberTypeEnum
{
    [EnumValue("Beam")] Beam,
    [EnumValue("Column")] Column,
    [EnumValue("Bracing")] Bracing,
    [EnumValue("Other")] Other,
    [EnumValue("Unknown")] Unknown
}