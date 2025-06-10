using XmiSchema.Core.Enums;

namespace XmiSchema.Core.Entities;

public class XmiStructuralMaterial : XmiBaseEntity, IEquatable<XmiStructuralMaterial>
{
    public XmiStructuralMaterialTypeEnum MaterialType { get; set; }
    public double Grade { get; set; }
    public double UnitWeight { get; set; }
    public string EModulus { get; set; }
    public string GModulus { get; set; }
    public string PoissonRatio { get; set; }
    public double ThermalCoefficient { get; set; }

    public XmiStructuralMaterial(
        string id,
        string name,
        string ifcguid,
        string nativeId,
        string description,
        XmiStructuralMaterialTypeEnum materialType,
        double grade,
        double unitWeight,
        string eModulus,
        string gModulus,
        string poissonRatio,
        double thermalCoefficient
    ) : base(id, name, ifcguid, nativeId, description, nameof(XmiStructuralMaterial))
    {
        MaterialType = materialType;
        Grade = grade;
        UnitWeight = unitWeight;
        EModulus = eModulus;
        GModulus = gModulus;
        PoissonRatio = poissonRatio;
        ThermalCoefficient = thermalCoefficient;
    }

    public bool Equals(XmiStructuralMaterial other)
    {
        if (other == null) return false;
        return string.Equals(NativeId, other.NativeId, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object obj) => Equals(obj as XmiStructuralMaterial);

    public override int GetHashCode()
    {
        return NativeId?.ToLowerInvariant().GetHashCode() ?? 0;
    }
}
