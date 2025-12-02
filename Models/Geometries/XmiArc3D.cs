namespace XmiSchema.Core.Geometries;

/// <summary>
/// Represents a circular arc segment in 3D space defined by start/end points, center, and radius.
/// </summary>
/// <remarks>
/// <para>
/// `XmiArc3D` represents curved geometry in the Cross Model Information (XMI) schema. It defines
/// a circular arc segment in three-dimensional space, commonly used for:
/// </para>
/// <list type="bullet">
/// <item><description>Curved structural members (arches, curved beams)</description></item>
/// <item><description>Curved segments within complex member geometry</description></item>
/// <item><description>Rounded edges and fillets in structural shapes</description></item>
/// <item><description>Architectural curved elements in the built environment</description></item>
/// </list>
/// <para>
/// The arc is fully defined by three points (start, end, center) and a radius. The arc follows
/// a circular path from the start point to the end point, centered at the center point with
/// the specified radius. The direction of the arc (clockwise or counter-clockwise) is determined
/// by the spatial arrangement of these three points.
/// </para>
/// <para>
/// As a subclass of <see cref="XmiBaseGeometry"/>, arcs are first-class entities in the graph
/// model, referenced by structural elements through <see cref="XmiHasGeometry"/> relationships.
/// </para>
/// </remarks>
public class XmiArc3D : XmiBaseGeometry
{
    /// <summary>
    /// Gets or sets the starting point of the arc segment.
    /// </summary>
    /// <remarks>
    /// The start point defines the beginning of the curved arc segment. The arc follows a
    /// circular path from this point to <see cref="EndPoint"/>, centered at <see cref="CentrePoint"/>.
    /// </remarks>
    public XmiPoint3D StartPoint { get; set; }

    /// <summary>
    /// Gets or sets the ending point of the arc segment.
    /// </summary>
    /// <remarks>
    /// The end point defines the terminus of the curved arc segment. The arc follows a circular
    /// path from <see cref="StartPoint"/> to this point, centered at <see cref="CentrePoint"/>.
    /// </remarks>
    public XmiPoint3D EndPoint { get; set; }

    /// <summary>
    /// Gets or sets the center point of the circular arc.
    /// </summary>
    /// <remarks>
    /// The center point defines the center of the circular path that the arc follows. The arc
    /// lies on a circle centered at this point with radius <see cref="Radius"/>. The center
    /// point should be equidistant from both start and end points (within tolerance).
    /// </remarks>
    public XmiPoint3D CentrePoint { get; set; }

    /// <summary>
    /// Gets or sets the radius of the circular arc.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The radius defines the distance from the <see cref="CentrePoint"/> to any point on the
    /// arc. All points along the arc lie at this distance from the center.
    /// </para>
    /// <para>
    /// The radius should be consistent with the distance from the center point to the start
    /// and end points. Units should match the model's unit system (typically meters or millimeters).
    /// </para>
    /// </remarks>
    public float Radius { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmiArc3D"/> class with specified
    /// arc parameters.
    /// </summary>
    /// <param name="id">Unique identifier for the arc.</param>
    /// <param name="name">Display name for the arc (e.g., "Arch Centerline", "Curved Edge").</param>
    /// <param name="ifcGuid">IFC GUID for BIM interoperability (maps to IfcCircle, IfcTrimmedCurve).</param>
    /// <param name="nativeId">Original identifier from the source system.</param>
    /// <param name="description">Textual description of the arc's purpose or location.</param>
    /// <param name="startPoint">The starting point of the arc segment.</param>
    /// <param name="endPoint">The ending point of the arc segment.</param>
    /// <param name="centrePoint">The center point of the circular arc.</param>
    /// <param name="radius">The radius of the circular arc.</param>
    /// <remarks>
    /// <para>
    /// The constructor initializes the arc with the specified geometry and metadata. The
    /// <see cref="XmiBaseEntity.EntityType"/> property is automatically set to "XmiArc3D".
    /// </para>
    /// <para>
    /// All three points should be added to the model as separate entities. The
    /// <see cref="XmiSchemaModelBuilder"/> will automatically create <see cref="XmiHasGeometry"/>
    /// relationships linking this arc to its defining points.
    /// </para>
    /// </remarks>
    public XmiArc3D(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        XmiPoint3D startPoint,
        XmiPoint3D endPoint,
        XmiPoint3D centrePoint,
        float radius
    ) : base(id, name, ifcGuid, nativeId, description)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
        CentrePoint = centrePoint;
        Radius = radius;
        EntityType = nameof(XmiArc3D);
    }
}
