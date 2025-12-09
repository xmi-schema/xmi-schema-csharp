using Newtonsoft.Json;
using XmiSchema.Entities.Bases;

namespace XmiSchema.Entities.Geometries;

/// <summary>
/// Represents a spatial point in the Cross Model Information schema with tolerance aware comparisons.
/// </summary>
public class XmiPoint3d : XmiBaseGeometry, IEquatable<XmiPoint3d>
{
    private const double Tolerance = 1e-10;

    [JsonProperty(Order = 6)]
    public double X { get; set; }

    [JsonProperty(Order = 7)]
    public double Y { get; set; }

    [JsonProperty(Order = 8)]
    public double Z { get; set; }

    /// <summary>
    /// Initializes a new <see cref="XmiPoint3d"/> with project coordinates.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <param name="name">Display name.</param>
    /// <param name="ifcGuid">Optional IFC GUID reference.</param>
    /// <param name="nativeId">Native identifier from the source model.</param>
    /// <param name="description">Describes the point usage.</param>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="z">Z coordinate.</param>
    public XmiPoint3d(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        double x,
        double y,
        double z
    ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiPoint3d))
    {
        X = x;
        Y = y;
        Z = z;
        EntityName = nameof(XmiPoint3d);
    }

    public bool Equals(XmiPoint3d? other)
    {
        if (other == null) return false;

        return Math.Abs(X - other.X) < Tolerance &&
               Math.Abs(Y - other.Y) < Tolerance &&
               Math.Abs(Z - other.Z) < Tolerance;
    }

    // public override bool Equals(object obj) => Equals(obj as XmiPoint3d);

    public override int GetHashCode()
    {
        // You can choose to quantize the values to avoid hash inconsistency due to floating-point noise
        long qx = (long)(X / Tolerance);
        long qy = (long)(Y / Tolerance);
        long qz = (long)(Z / Tolerance);
        return HashCode.Combine(qx, qy, qz);
    }
}
