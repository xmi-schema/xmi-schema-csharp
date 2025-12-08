using XmiSchema.Models.Bases;

namespace XmiSchema.Models.Enums;

public enum XmiStructuralCurveMemberTypeEnum
{
    [EnumValue("Beam")] Beam,
    [EnumValue("Column")] Column,
    [EnumValue("Bracing")] Bracing,
    [EnumValue("Other")] Other,
    [EnumValue("Unknown")] Unknown
}