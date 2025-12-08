using Newtonsoft.Json;
using XmiSchema.Models.Bases;
using XmiSchema.Models.Enums;

namespace XmiSchema.Models.Commons;

/// <summary>
/// Describes a structural material that can be referenced by other XMI entities, including the mechanical constants used during analysis.
/// </summary>
public class XmiMaterial : XmiBaseEntity, IEquatable<XmiMaterial>
{
    public XmiMaterialTypeEnum MaterialType { get; set; }
    public double Grade { get; set; }
    public double UnitWeight { get; set; }

    [JsonProperty(PropertyName = "EModulus")]
    public string ElasticModulus { get; set; }

    [JsonProperty(PropertyName = "GModulus")]
    public string ShearModulus { get; set; }
    public string PoissonRatio { get; set; }
    public double ThermalCoefficient { get; set; }

    /// <summary>
    /// Initializes a new <see cref="XmiMaterial"/> with the physical properties required for Cross Model Information analysis.
    /// </summary>
    /// <param name="id">Unique identifier inside the XMI document.</param>
    /// <param name="name">Human readable label for the material; falls back to <paramref name="id"/>.</param>
    /// <param name="ifcGuid">Optional IFC GUID that links the material back to its native BIM object.</param>
    /// <param name="nativeId">Identifier coming from the source authoring tool.</param>
    /// <param name="description">Free-form description explaining when the material is used.</param>
    /// <param name="materialType">Generalized material category (concrete, steel, etc.).</param>
    /// <param name="grade">Producer grade/strength number.</param>
    /// <param name="unitWeight">Mass density expressed in the preferred unit system.</param>
    /// <param name="elasticModulus">Elastic modulus (E) captured as a string to preserve precision and units.</param>
    /// <param name="shearModulus">Shear modulus (G) string representation.</param>
    /// <param name="poissonRatio">Poisson ratio value.</param>
    /// <param name="thermalCoefficient">Thermal expansion coefficient.</param>
    public XmiMaterial(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        XmiMaterialTypeEnum materialType,
        double grade,
        double unitWeight,
        string elasticModulus,
        string shearModulus,
        string poissonRatio,
        double thermalCoefficient
    ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiMaterial), XmiBaseEntityDomainEnum.Shared)
    {
        MaterialType = materialType;
        Grade = grade;
        UnitWeight = unitWeight;
        ElasticModulus = elasticModulus;
        ShearModulus = shearModulus;
        PoissonRatio = poissonRatio;
        ThermalCoefficient = thermalCoefficient;
    }

    public bool Equals(XmiMaterial? other)
    {
        if (other is null) return false;
        return string.Equals(NativeId, other.NativeId, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as XmiMaterial);

    public override int GetHashCode() =>
        NativeId?.ToLowerInvariant().GetHashCode() ?? 0;
}
