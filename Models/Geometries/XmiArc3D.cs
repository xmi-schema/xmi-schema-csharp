namespace XmiSchema.Core.Geometries;

/// <summary>
/// Represents a three-dimensional circular arc defined by start, end, and center points.
/// </summary>
public class XmiArc3D : XmiBaseGeometry
{
    public XmiPoint3D StartPoint { get; set; }
    public XmiPoint3D EndPoint { get; set; }
    public XmiPoint3D CenterPoint { get; set; }
    public float Radius { get; set; }

    /// <summary>
    /// Initializes a new <see cref="XmiArc3D"/> entity.
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
    public XmiArc3D(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        XmiPoint3D startPoint ,
        XmiPoint3D endPoint ,
        XmiPoint3D centerPoint ,
        float radius
    ) : base(id, name, ifcGuid, nativeId, description)
    {
        StartPoint  = startPoint ;
        EndPoint  = endPoint ;
        CenterPoint  = centerPoint ;
        Radius = radius;
        EntityType = nameof(XmiArc3D);
    }
}
