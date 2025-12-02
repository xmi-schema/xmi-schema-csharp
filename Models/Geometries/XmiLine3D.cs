namespace XmiSchema.Core.Geometries;

/// <summary>
/// Represents a straight line segment in 3D space defined by two endpoints.
/// </summary>
/// <remarks>
/// <para>
/// `XmiLine3D` represents linear geometry in the Cross Model Information (XMI) schema. It
/// defines a straight line segment between two points in three-dimensional space, commonly
/// used for:
/// </para>
/// <list type="bullet">
/// <item><description>Centerlines of straight structural members (beams, columns, bracing)</description></item>
/// <item><description>Segments within complex member geometry</description></item>
/// <item><description>Reference lines for alignment and positioning</description></item>
/// <item><description>Edges of polygonal surfaces</description></item>
/// </list>
/// <para>
/// The line is defined by two <see cref="XmiPoint3D"/> instances representing the start and
/// end points. The line extends from the start point to the end point in a straight path,
/// with the direction implicitly defined by this ordering.
/// </para>
/// <para>
/// As a subclass of <see cref="XmiBaseGeometry"/>, lines are first-class entities in the
/// graph model. They exist independently and are referenced by structural elements through
/// <see cref="XmiHasGeometry"/> relationships. For example, a <see cref="XmiSegment"/> may
/// reference a Line3D to define its geometric path.
/// </para>
/// <para>
/// For curved geometry, use <see cref="XmiArc3D"/> instead. For more complex paths composed
/// of multiple segments, create separate Line3D and Arc3D instances connected end-to-end.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Create endpoint points
/// var basePoint = new XmiPoint3D("PT001", "Column Base", "", "", "", 0.0, 0.0, 0.0);
/// var topPoint = new XmiPoint3D("PT002", "Column Top", "", "", "", 0.0, 0.0, 3.5);
///
/// // Create vertical line
/// var columnLine = new XmiLine3D(
///     id: "LN001",
///     name: "Column Centerline",
///     ifcGuid: "3fR9wn0gE0RRvO9P_81E$9",
///     nativeId: "COL_CL_001",
///     description: "Vertical centerline of ground floor column",
///     startPoint3D: basePoint,
///     endPoint3D: topPoint
/// );
///
/// // Create horizontal beam line
/// var beamStart = new XmiPoint3D("PT003", "Beam Start", "", "", "", 0.0, 0.0, 3.5);
/// var beamEnd = new XmiPoint3D("PT004", "Beam End", "", "", "", 6.0, 0.0, 3.5);
///
/// var beamLine = new XmiLine3D(
///     id: "LN002",
///     name: "Beam Centerline",
///     ifcGuid: "",
///     nativeId: "BEAM_CL_001",
///     description: "Horizontal centerline of floor beam",
///     startPoint3D: beamStart,
///     endPoint3D: beamEnd
/// );
/// </code>
/// </example>
public class XmiLine3D : XmiBaseGeometry
{
    /// <summary>
    /// Gets or sets the starting point of the line segment.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The start point defines the beginning of the directed line segment. The line extends
    /// from this point to the <see cref="EndPoint3D"/> in a straight path.
    /// </para>
    /// <para>
    /// For structural members, the start point typically represents:
    /// </para>
    /// <list type="bullet">
    /// <item><description><b>Columns</b>: Base or bottom of the member</description></item>
    /// <item><description><b>Beams</b>: Left end or first support</description></item>
    /// <item><description><b>Bracing</b>: Lower connection point</description></item>
    /// </list>
    /// <para>
    /// The start point must be a valid <see cref="XmiPoint3D"/> instance. The point should
    /// typically be added to the model as a separate entity before being referenced by this line.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var startPoint = new XmiPoint3D("PT001", "Start", "", "", "", 0.0, 0.0, 0.0);
    /// var endPoint = new XmiPoint3D("PT002", "End", "", "", "", 0.0, 0.0, 3.5);
    ///
    /// var line = new XmiLine3D("LN001", "Vertical Line", "", "", "",
    ///     startPoint3D: startPoint,
    ///     endPoint3D: endPoint
    /// );
    ///
    /// Console.WriteLine($"Line starts at ({line.StartPoint3D.X}, {line.StartPoint3D.Y}, {line.StartPoint3D.Z})");
    /// // Output: Line starts at (0, 0, 0)
    /// </code>
    /// </example>
    public XmiPoint3D StartPoint3D { get; set; }

    /// <summary>
    /// Gets or sets the ending point of the line segment.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The end point defines the terminus of the directed line segment. The line extends
    /// from the <see cref="StartPoint3D"/> to this point in a straight path.
    /// </para>
    /// <para>
    /// For structural members, the end point typically represents:
    /// </para>
    /// <list type="bullet">
    /// <item><description><b>Columns</b>: Top or cap of the member</description></item>
    /// <item><description><b>Beams</b>: Right end or second support</description></item>
    /// <item><description><b>Bracing</b>: Upper connection point</description></item>
    /// </list>
    /// <para>
    /// The end point must be a valid <see cref="XmiPoint3D"/> instance. The point should
    /// typically be added to the model as a separate entity before being referenced by this line.
    /// </para>
    /// <para>
    /// The direction of the line is implicitly defined as pointing from start to end. This
    /// direction may be significant for defining local axes of structural members.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var startPoint = new XmiPoint3D("PT001", "Start", "", "", "", 0.0, 0.0, 0.0);
    /// var endPoint = new XmiPoint3D("PT002", "End", "", "", "", 6.0, 0.0, 0.0);
    ///
    /// var line = new XmiLine3D("LN001", "Horizontal Line", "", "", "",
    ///     startPoint3D: startPoint,
    ///     endPoint3D: endPoint
    /// );
    ///
    /// // Calculate line length
    /// double dx = line.EndPoint3D.X - line.StartPoint3D.X;
    /// double dy = line.EndPoint3D.Y - line.StartPoint3D.Y;
    /// double dz = line.EndPoint3D.Z - line.StartPoint3D.Z;
    /// double length = Math.Sqrt(dx * dx + dy * dy + dz * dz);
    ///
    /// Console.WriteLine($"Line length: {length}m"); // Output: Line length: 6m
    /// </code>
    /// </example>
    public XmiPoint3D EndPoint3D { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmiLine3D"/> class with specified
    /// start and end points.
    /// </summary>
    /// <param name="id">
    /// Unique identifier for the line. Must be unique within the model.
    /// </param>
    /// <param name="name">
    /// Display name for the line. Common names include "Centerline", member labels
    /// (e.g., "B1-CL"), or descriptive names (e.g., "Column Axis").
    /// </param>
    /// <param name="ifcGuid">
    /// IFC GUID for BIM interoperability. Maps this line to corresponding IFC entities
    /// (e.g., IfcPolyline, IfcLine, IfcTrimmedCurve). Use empty string if not mapping to IFC.
    /// </param>
    /// <param name="nativeId">
    /// Original identifier from the source system (e.g., analysis software, CAD system).
    /// Useful for traceability and round-trip data exchange.
    /// </param>
    /// <param name="description">
    /// Textual description of the line's purpose or location. Examples: "Centerline of ground
    /// floor column at gridline A-1", "Reference line for beam alignment".
    /// </param>
    /// <param name="startPoint3D">
    /// The starting point of the line segment. Must be a valid <see cref="XmiPoint3D"/>
    /// instance. The line extends from this point to the end point.
    /// </param>
    /// <param name="endPoint3D">
    /// The ending point of the line segment. Must be a valid <see cref="XmiPoint3D"/>
    /// instance. The line extends from the start point to this point, defining the line's
    /// direction implicitly.
    /// </param>
    /// <remarks>
    /// <para>
    /// The constructor initializes the line with the specified endpoints and metadata. The
    /// <see cref="XmiBaseEntity.EntityType"/> property is automatically set to "XmiLine3D".
    /// </para>
    /// <para>
    /// Both start and end points should typically be added to the model as separate entities
    /// before creating the line. The <see cref="XmiSchemaModelBuilder"/> will automatically
    /// create <see cref="XmiHasGeometry"/> relationships linking this line to its endpoint
    /// points.
    /// </para>
    /// <para>
    /// The line represents a straight path - there is no curvature. For curved paths, use
    /// <see cref="XmiArc3D"/> instead. For complex paths with multiple segments, create
    /// separate geometry instances.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Create a vertical column centerline
    /// var basePoint = new XmiPoint3D(
    ///     id: "PT_COL_BASE",
    ///     name: "Column Base",
    ///     ifcGuid: "",
    ///     nativeId: "N101",
    ///     description: "Base node at elevation 0.0",
    ///     x: 6.0,
    ///     y: 8.0,
    ///     z: 0.0
    /// );
    ///
    /// var topPoint = new XmiPoint3D(
    ///     id: "PT_COL_TOP",
    ///     name: "Column Top",
    ///     ifcGuid: "",
    ///     nativeId: "N102",
    ///     description: "Top node at elevation 3.5",
    ///     x: 6.0,
    ///     y: 8.0,
    ///     z: 3.5
    /// );
    ///
    /// var centerline = new XmiLine3D(
    ///     id: "LN_COL001",
    ///     name: "Column C1 Centerline",
    ///     ifcGuid: "4gS0xo1hF1SSwP0Q_92F$0",
    ///     nativeId: "ETABS_L001",
    ///     description: "Centerline of column C1 from ground to first floor",
    ///     startPoint3D: basePoint,
    ///     endPoint3D: topPoint
    /// );
    ///
    /// // Verify creation
    /// Console.WriteLine($"Line ID: {centerline.ID}");
    /// Console.WriteLine($"Type: {centerline.EntityType}"); // Output: "XmiLine3D"
    /// Console.WriteLine($"Start: ({centerline.StartPoint3D.X}, {centerline.StartPoint3D.Y}, {centerline.StartPoint3D.Z})");
    /// Console.WriteLine($"End: ({centerline.EndPoint3D.X}, {centerline.EndPoint3D.Y}, {centerline.EndPoint3D.Z})");
    /// </code>
    /// </example>
    public XmiLine3D(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        XmiPoint3D startPoint3D,
        XmiPoint3D endPoint3D
    ) : base(id, name, ifcGuid, nativeId, description)
    {
        StartPoint3D = startPoint3D;
        EndPoint3D = endPoint3D;
        EntityType = nameof(XmiLine3D);
    }
}
