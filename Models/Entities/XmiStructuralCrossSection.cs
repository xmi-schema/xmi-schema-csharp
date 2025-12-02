using XmiSchema.Core.Enums;


namespace XmiSchema.Core.Entities;

/// <summary>
/// Captures geometric and analytical properties of a structural cross-section shared by curve or surface members.
/// </summary>
public class XmiStructuralCrossSection : XmiBaseEntity, IEquatable<XmiStructuralCrossSection>
{
    // public required XmiStructuralMaterial Material { get; set; }
    public XmiShapeEnum Shape { get; set; }
    public string[] Parameters { get; set; }
    public double Area { get; set; }
    public double SecondMomentOfAreaXAxis { get; set; }
    public double SecondMomentOfAreaYAxis { get; set; }
    public double RadiusOfGyrationXAxis { get; set; }
    public double RadiusOfGyrationYAxis { get; set; }
    public double ElasticModulusXAxis { get; set; }
    public double ElasticModulusYAxis { get; set; }
    public double PlasticModulusXAxis { get; set; }
    public double PlasticModulusYAxis { get; set; }
    public double TorsionalConstant { get; set; }

    /// <summary>
    /// Initializes a new <see cref="XmiStructuralCrossSection"/> with the inputs consumed by the XMI graph.
    /// </summary>
    /// <param name="id">Unique local identifier.</param>
    /// <param name="name">Human readable name for reports.</param>
    /// <param name="ifcguid">Optional IFC GUID reference.</param>
    /// <param name="nativeId">Source-system identifier.</param>
    /// <param name="description">Explanation of origin or use.</param>
    /// <param name="shape">Generic shape classification (I, T, L, etc.).</param>
    /// <param name="parameters">Shape-specific parameters such as flange widths.</param>
    /// <param name="area">Cross-sectional area.</param>
    /// <param name="secondMomentOfAreaXAxis">Second moment of area about the local X axis.</param>
    /// <param name="secondMomentOfAreaYAxis">Second moment of area about the local Y axis.</param>
    /// <param name="radiusOfGyrationXAxis">Radius of gyration about X.</param>
    /// <param name="radiusOfGyrationYAxis">Radius of gyration about Y.</param>
    /// <param name="elasticModulusXAxis">Elastic section modulus about X.</param>
    /// <param name="elasticModulusYAxis">Elastic section modulus about Y.</param>
    /// <param name="plasticModulusXAxis">Plastic section modulus about X.</param>
    /// <param name="plasticModulusYAxis">Plastic section modulus about Y.</param>
    /// <param name="torsionalConstant">Torsional constant (J).</param>
    public XmiStructuralCrossSection(
        string id,
        string name,
        string ifcguid,
        string nativeId,
        string description,
        // XmiStructuralMaterial material,
        XmiShapeEnum shape,
        string[] parameters,
        double area,
        double secondMomentOfAreaXAxis,
        double secondMomentOfAreaYAxis,
        double radiusOfGyrationXAxis,
        double radiusOfGyrationYAxis,
        double elasticModulusXAxis,
        double elasticModulusYAxis,
        double plasticModulusXAxis,
        double plasticModulusYAxis,
        double torsionalConstant
    ) : base(id, name, ifcguid, nativeId, description, nameof(XmiStructuralCrossSection))
    {

        // Material = material;
        Shape = shape;
        Parameters = parameters;
        Area = area;
        SecondMomentOfAreaXAxis = secondMomentOfAreaXAxis;
        SecondMomentOfAreaYAxis = secondMomentOfAreaYAxis;
        RadiusOfGyrationXAxis = radiusOfGyrationXAxis;
        RadiusOfGyrationYAxis = radiusOfGyrationYAxis;
        ElasticModulusXAxis = elasticModulusXAxis;
        ElasticModulusYAxis = elasticModulusYAxis;
        PlasticModulusXAxis = plasticModulusXAxis;
        PlasticModulusYAxis = plasticModulusYAxis;
        TorsionalConstant = torsionalConstant;
    }

    public bool Equals(XmiStructuralCrossSection? other)
    {
        if (other is null) return false;
        return string.Equals(NativeId, other.NativeId, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as XmiStructuralCrossSection);

    public override int GetHashCode() => NativeId?.ToLowerInvariant().GetHashCode() ?? 0;
}
