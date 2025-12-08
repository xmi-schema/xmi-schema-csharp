using XmiSchema.Entities.Bases;

namespace XmiSchema.Entities.Geometries;

/// <summary>
/// Defines a straight 3D line segment between two <see cref="XmiPoint3d"/> nodes.
/// </summary>
public class XmiLine3d : XmiBaseGeometry
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
}
