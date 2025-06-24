using Newtonsoft.Json;

namespace XmiSchema.Core.Geometries;



public class XmiPoint3D : XmiBaseGeometry, IEquatable<XmiPoint3D>
{
    private const double Tolerance = 1e-10;

    [JsonProperty(Order = 6)]
    public double X { get; set; }

    [JsonProperty(Order = 7)]
    public double Y { get; set; }

    [JsonProperty(Order = 8)]
    public double Z { get; set; }

    public XmiPoint3D(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        double x,
        double y,
        double z
    ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiPoint3D))
    {
        X = x;
        Y = y;
        Z = z;
        EntityType = nameof(XmiPoint3D);
    }

    public bool Equals(XmiPoint3D? other)
    {
        if (other == null) return false;

        return Math.Abs(X - other.X) < Tolerance &&
               Math.Abs(Y - other.Y) < Tolerance &&
               Math.Abs(Z - other.Z) < Tolerance;
    }

    // public override bool Equals(object obj) => Equals(obj as XmiPoint3D);

    public override int GetHashCode()
    {
        // You can choose to quantize the values to avoid hash inconsistency due to floating-point noise
        long qx = (long)(X / Tolerance);
        long qy = (long)(Y / Tolerance);
        long qz = (long)(Z / Tolerance);
        return HashCode.Combine(qx, qy, qz);
    }
}
