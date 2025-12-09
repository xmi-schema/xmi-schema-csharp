using XmiSchema.Entities.Bases;

namespace XmiSchema.Entities.Geometries;

/// <summary>
/// Defines a straight 3D line segment between two <see cref="XmiPoint3d"/> nodes.
/// </summary>
public class XmiLine3d : XmiBaseGeometry, IEquatable<XmiLine3d>
{
    public XmiPoint3d StartPoint { get; set; }
    public XmiPoint3d EndPoint { get; set; }

    /// <summary>
    /// Creates a new <see cref="XmiLine3d"/> suitable for associating with curve members.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <param name="name">Human readable name.</param>
    /// <param name="ifcGuid">Optional IFC GUID.</param>
    /// <param name="nativeId">Native identifier from the authoring system.</param>
    /// <param name="description">Optional description.</param>
    /// <param name="startPoint">Start coordinate.</param>
    /// <param name="endPoint">End coordinate.</param>
    public XmiLine3d(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        XmiPoint3d startPoint,
        XmiPoint3d endPoint
    ) : base(id, name, ifcGuid, nativeId, description)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
        EntityName = nameof(XmiLine3d);
    }

    /// <summary>
    /// Referential equality: checks if both lines reference
    /// the exact same <see cref="XmiPoint3d"/> instances.
    /// Use for graph connectivity checks.
    /// </summary>
    /// <param name="other">The line to compare against.</param>
    /// <returns>True if both StartPoint and EndPoint are the same references.</returns>
    public bool Equals(XmiLine3d? other)
    {
        if (other == null) return false;
        return ReferenceEquals(StartPoint, other.StartPoint) &&
               ReferenceEquals(EndPoint, other.EndPoint);
    }

    /// <summary>
    /// Directional equality: checks if two lines have the same
    /// coordinates AND the same vector direction.
    /// Start→End must match Start→End (not reversed).
    /// Use when direction/vector matters (e.g., load direction).
    /// </summary>
    /// <param name="other">The line to compare against.</param>
    /// <returns>True if coordinates match within tolerance and direction is the same.</returns>
    public bool IsDirectionallyEqual(XmiLine3d? other)
    {
        if (other == null) return false;
        return StartPoint.Equals(other.StartPoint) &&
               EndPoint.Equals(other.EndPoint);
    }

    /// <summary>
    /// Coincident equality: checks if two lines occupy the same
    /// space, regardless of direction.
    /// A→B is considered coincident with B→A.
    /// Use for geometry deduplication where direction doesn't matter.
    /// </summary>
    /// <param name="other">The line to compare against.</param>
    /// <returns>True if the lines occupy the same space, regardless of direction.</returns>
    public bool IsCoincident(XmiLine3d? other)
    {
        if (other == null) return false;

        bool sameDirection = StartPoint.Equals(other.StartPoint) &&
                             EndPoint.Equals(other.EndPoint);

        bool oppositeDirection = StartPoint.Equals(other.EndPoint) &&
                                 EndPoint.Equals(other.StartPoint);

        return sameDirection || oppositeDirection;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(StartPoint?.GetHashCode() ?? 0,
                                EndPoint?.GetHashCode() ?? 0);
    }
}
