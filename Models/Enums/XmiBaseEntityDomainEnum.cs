namespace XmiSchema.Core.Enums;

/// <summary>
/// Defines the domain classification for XMI entities.
/// </summary>
public enum XmiBaseEntityDomainEnum
{
    [EnumValue("Physical")] Physical,
    [EnumValue("StructuralAnalytical")] StructuralAnalytical,
    [EnumValue("Geometry")] Geometry,
    [EnumValue("Functional")] Functional,
        [EnumValue("Shared")] Shared
}
