using XmiSchema.Models.Bases;


namespace XmiSchema.Models.Enums;

public enum XmiMaterialTypeEnum
{
    [EnumValue("Concrete")] Concrete,
    [EnumValue("Steel")] Steel,
    [EnumValue("Timber")] Timber,
    [EnumValue("Aluminium")] Aluminium,
    [EnumValue("Composite")] Composite,
    [EnumValue("Masonry")] Masonry,
    [EnumValue("Others")] Others,
    [EnumValue("Rebar")] Rebar,
    [EnumValue("Tendon")] Tendon,
    [EnumValue("Unknown")] Unknown
}