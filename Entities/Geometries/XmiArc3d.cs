using XmiSchema.Entities.Bases;

namespace XmiSchema.Entities.Geometries;

/// <summary>
/// Represents a three-dimensional circular arc defined by start, end, and center points.
/// </summary>
public class XmiArc3d : XmiBaseGeometry
{
    public XmiPoint3d StartPoint { get; set; }
    public XmiPoint3d EndPoint { get; set; }
    public XmiPoint3d CenterPoint { get; set; }
    public float Radius { get; set; }

    /// <summary>
    /// Initializes a new <see cref="XmiArc3d"/> entity.
    /// </summary>
    /// <param name="id">Unique identifier for the geometry node.</param>
    /// <param name="name">Readable label.</param>
    /// <param name="ifcGuid">Related IFC GUID.</param>
    /// <param name="nativeId">Identifier from the authoring system.</param>
    /// <param name="description">Optional text describing the curve.</param>
    /// <param name="startPoint">Start coordinate.</param>
    /// <param name="endPoint">End coordinate.</param>
    /// <param name="centerPoint">Arc center coordinate.</param>
    /// <param name="radius">Arc radius.</param>
    public XmiArc3d(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        XmiPoint3d startPoint,
        XmiPoint3d endPoint,
        XmiPoint3d centerPoint,
        float radius
    ) : base(id, name, ifcGuid, nativeId, description)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
        CenterPoint = centerPoint;
        Radius = radius;
        EntityName = nameof(XmiArc3d);
    }
}
