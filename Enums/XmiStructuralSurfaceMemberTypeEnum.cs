using XmiSchema.Entities.Bases;

namespace XmiSchema.Enums;

public enum XmiStructuralSurfaceMemberTypeEnum
{
    [EnumValue("Slab")] Slab,
    [EnumValue("Wall")] Wall,
    [EnumValue("PadFooting")] PadFooting,
    [EnumValue("StripFooting")] StripFooting,
    [EnumValue("Pilecap")] Pilecap,
    [EnumValue("RoofPanel")] RoofPanel,
    [EnumValue("WallPanel")] WallPanel,
    [EnumValue("Raft")] Raft,
    [EnumValue("Unknown")] Unknown
}