namespace XmiSchema.Core.Geometries;

/// <summary>
/// Defines a straight 3D line segment between two <see cref="XmiPoint3D"/> nodes.
/// </summary>
public class XmiLine3D : XmiBaseGeometry
{
    public XmiPoint3D StartPoint { get; set; }
    public XmiPoint3D EndPoint { get; set; }

    /// <summary>
    /// Creates a new <see cref="XmiLine3D"/> suitable for associating with curve members.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <param name="name">Human readable name.</param>
    /// <param name="ifcGuid">Optional IFC GUID.</param>
    /// <param name="nativeId">Native identifier from the authoring system.</param>
    /// <param name="description">Optional description.</param>
    /// <param name="startPoint">Start coordinate.</param>
    /// <param name="endPoint">End coordinate.</param>
    public XmiLine3D(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        XmiPoint3D startPoint,
        XmiPoint3D endPoint
    ) : base(id, name, ifcGuid, nativeId, description)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
        EntityType = nameof(XmiLine3D);
    }
}
