using System;

namespace XmiSchema.Entities.Commons;

/// <summary>
/// Represents a unit direction vector for local axis definitions.
/// </summary>
public class XmiAxis : IEquatable<XmiAxis>
{
    private const double Tolerance = 1e-10;

    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public XmiAxis(double x, double y, double z)
    {
        var length = Math.Sqrt(x * x + y * y + z * z);
        if (length <= Tolerance)
        {
            throw new ArgumentException("Axis vector cannot be zero-length.");
        }

        if (Math.Abs(length - 1.0) > Tolerance)
        {
            throw new ArgumentException("Axis vector must be unit length (|v| = 1).");
        }

        X = x;
        Y = y;
        Z = z;
    }

    public bool Equals(XmiAxis? other)
    {
        if (other == null) return false;
        return Math.Abs(X - other.X) <= Tolerance
            && Math.Abs(Y - other.Y) <= Tolerance
            && Math.Abs(Z - other.Z) <= Tolerance;
    }

    public override bool Equals(object? obj) => Equals(obj as XmiAxis);

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);
}
