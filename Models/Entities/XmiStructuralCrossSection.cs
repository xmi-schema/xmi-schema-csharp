using XmiSchema.Core.Enums;


namespace XmiSchema.Core.Entities;

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

    public bool Equals(XmiStructuralCrossSection other)
    {
        if (other == null) return false;
        return string.Equals(NativeId, other.NativeId, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object obj) => Equals(obj as XmiStructuralCrossSection);

    public override int GetHashCode()
    {
        return NativeId?.ToLowerInvariant().GetHashCode() ?? 0;
    }
}
