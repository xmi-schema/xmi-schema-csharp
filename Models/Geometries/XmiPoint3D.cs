namespace XmiSchema.Core.Geometries;

/// <summary>
/// Represents a point in 3D Cartesian space within the Cross Model Information (XMI) schema.
/// </summary>
/// <remarks>
/// <para>
/// `XmiPoint3D` is the fundamental geometric primitive in the built environment data model,
/// representing a location in three-dimensional space using Cartesian coordinates (X, Y, Z).
/// Points are used throughout the XMI schema to define:
/// </para>
/// <list type="bullet">
/// <item><description>Positions of structural nodes (<see cref="XmiStructuralPointConnection"/>)</description></item>
/// <item><description>Endpoints of linear geometry (<see cref="XmiLine3D"/>)</description></item>
/// <item><description>Control points for curved geometry (<see cref="XmiArc3D"/>)</description></item>
/// <item><description>Reference positions for any spatial entity</description></item>
/// </list>
/// <para>
/// This class inherits from <see cref="XmiBaseGeometry"/>, making it a first-class entity
/// in the graph model. Points exist independently and can be referenced by multiple structural
/// elements through <see cref="XmiHasGeometry"/> relationships.
/// </para>
/// <para>
/// The XMI schema uses a right-handed Cartesian coordinate system where:
/// </para>
/// <list type="bullet">
/// <item><description><b>X-axis</b>: Typically represents the East-West direction (horizontal)</description></item>
/// <item><description><b>Y-axis</b>: Typically represents the North-South direction (horizontal)</description></item>
/// <item><description><b>Z-axis</b>: Typically represents the vertical direction (elevation)</description></item>
/// </list>
/// <para>
/// Coordinates are stored as double-precision floating-point numbers, providing sufficient
/// precision for structural engineering applications. Units should be consistent across the
/// model (typically meters or millimeters).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Create a point at the origin
/// var origin = new XmiPoint3D(
///     id: "PT001",
///     name: "Origin",
///     ifcGuid: "",
///     nativeId: "ORIGIN",
///     description: "Coordinate system origin",
///     x: 0.0,
///     y: 0.0,
///     z: 0.0
/// );
///
/// // Create a point 3.5m above the origin (column top)
/// var columnTop = new XmiPoint3D(
///     id: "PT002",
///     name: "Column Top",
///     ifcGuid: "1a2b3c4d-5678-90ab-cdef-1234567890ab",
///     nativeId: "COL-TOP",
///     description: "Top of ground floor column",
///     x: 0.0,
///     y: 0.0,
///     z: 3.5
/// );
///
/// // Create a point at grid intersection
/// var gridPoint = new XmiPoint3D(
///     id: "PT003",
///     name: "Grid A-1",
///     ifcGuid: "",
///     nativeId: "GRID-A1",
///     description: "Intersection of Grid A and Grid 1",
///     x: 6.0,
///     y: 8.0,
///     z: 0.0
/// );
/// </code>
/// </example>
public class XmiPoint3D : XmiBaseGeometry
{
    /// <summary>
    /// Gets or sets the X-coordinate in the Cartesian coordinate system.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The X-coordinate typically represents the East-West direction in the horizontal plane.
    /// Positive values extend in the positive X direction as defined by the model's coordinate
    /// system.
    /// </para>
    /// <para>
    /// The unit of measurement should be consistent with the model's unit system (typically
    /// meters or millimeters for structural engineering applications).
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var point = new XmiPoint3D("PT001", "Point", "", "", "", 5.5, 3.2, 0.0);
    /// Console.WriteLine($"X-coordinate: {point.X}m"); // Output: X-coordinate: 5.5m
    /// </code>
    /// </example>
    public double X { get; set; }

    /// <summary>
    /// Gets or sets the Y-coordinate in the Cartesian coordinate system.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The Y-coordinate typically represents the North-South direction in the horizontal plane.
    /// Positive values extend in the positive Y direction as defined by the model's coordinate
    /// system.
    /// </para>
    /// <para>
    /// The unit of measurement should be consistent with the model's unit system (typically
    /// meters or millimeters for structural engineering applications).
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var point = new XmiPoint3D("PT001", "Point", "", "", "", 5.5, 3.2, 0.0);
    /// Console.WriteLine($"Y-coordinate: {point.Y}m"); // Output: Y-coordinate: 3.2m
    /// </code>
    /// </example>
    public double Y { get; set; }

    /// <summary>
    /// Gets or sets the Z-coordinate in the Cartesian coordinate system.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The Z-coordinate typically represents the vertical direction (elevation) in the coordinate
    /// system. Positive values extend upward from the base elevation.
    /// </para>
    /// <para>
    /// For structural engineering models, Z often represents:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Floor-to-floor heights (e.g., Z = 0 at ground level, Z = 3.5 at first floor)</description></item>
    /// <item><description>Member elevations above a reference datum</description></item>
    /// <item><description>Depth below ground for foundations (negative values)</description></item>
    /// </list>
    /// <para>
    /// The unit of measurement should be consistent with the model's unit system (typically
    /// meters or millimeters for structural engineering applications).
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var groundLevel = new XmiPoint3D("PT001", "Ground", "", "", "", 0.0, 0.0, 0.0);
    /// var firstFloor = new XmiPoint3D("PT002", "1st Floor", "", "", "", 0.0, 0.0, 3.5);
    /// var basement = new XmiPoint3D("PT003", "Basement", "", "", "", 0.0, 0.0, -2.5);
    ///
    /// Console.WriteLine($"Ground level Z: {groundLevel.Z}m");    // 0.0m
    /// Console.WriteLine($"First floor Z: {firstFloor.Z}m");      // 3.5m
    /// Console.WriteLine($"Basement Z: {basement.Z}m");           // -2.5m
    /// </code>
    /// </example>
    public double Z { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmiPoint3D"/> class with specified coordinates
    /// and metadata.
    /// </summary>
    /// <param name="id">
    /// Unique identifier for the point. Must be unique within the model.
    /// </param>
    /// <param name="name">
    /// Display name for the point. Common names include grid references (e.g., "Grid A-1"),
    /// node labels (e.g., "N101"), or descriptive names (e.g., "Column Base").
    /// </param>
    /// <param name="ifcGuid">
    /// IFC GUID for BIM interoperability. Maps this point to corresponding IFC entities
    /// (e.g., IfcCartesianPoint). Use empty string if not mapping to IFC.
    /// </param>
    /// <param name="nativeId">
    /// Original identifier from the source system (e.g., analysis software, CAD system).
    /// Useful for traceability and round-trip data exchange.
    /// </param>
    /// <param name="description">
    /// Textual description of the point's purpose or location. Examples: "Intersection of Grid A
    /// and Grid 1", "Top of ground floor column at gridline A-1", "Foundation anchor point".
    /// </param>
    /// <param name="x">
    /// X-coordinate in the model's coordinate system. Typically represents the East-West direction.
    /// Units should be consistent with the model (meters or millimeters).
    /// </param>
    /// <param name="y">
    /// Y-coordinate in the model's coordinate system. Typically represents the North-South direction.
    /// Units should be consistent with the model (meters or millimeters).
    /// </param>
    /// <param name="z">
    /// Z-coordinate in the model's coordinate system. Typically represents elevation.
    /// Units should be consistent with the model (meters or millimeters).
    /// </param>
    /// <remarks>
    /// <para>
    /// The constructor initializes the point with the specified coordinates and metadata. The
    /// <see cref="XmiBaseEntity.EntityType"/> property is automatically set to "XmiPoint3D".
    /// </para>
    /// <para>
    /// All coordinates are stored as double-precision floating-point numbers, providing
    /// approximately 15-17 decimal digits of precision. This is sufficient for structural
    /// engineering applications where typical precision requirements are 0.1mm to 1mm.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Create a point at a grid intersection
    /// var gridPoint = new XmiPoint3D(
    ///     id: "PT_A1_L0",
    ///     name: "Grid A-1 @ Ground",
    ///     ifcGuid: "2eQ8vn9fD9QQvN8O_70D$8",
    ///     nativeId: "ETABS_N101",
    ///     description: "Column base at grid intersection A-1 on ground level",
    ///     x: 6000.0,   // 6m east from origin
    ///     y: 8000.0,   // 8m north from origin
    ///     z: 0.0       // at ground level
    /// );
    ///
    /// // Verify creation
    /// Console.WriteLine($"Point ID: {gridPoint.ID}");
    /// Console.WriteLine($"Location: ({gridPoint.X}, {gridPoint.Y}, {gridPoint.Z})");
    /// Console.WriteLine($"Type: {gridPoint.EntityType}"); // Output: "XmiPoint3D"
    /// </code>
    /// </example>
    public XmiPoint3D(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        double x,
        double y,
        double z
    ) : base(id, name, ifcGuid, nativeId, description)
    {
        X = x;
        Y = y;
        Z = z;
        EntityType = nameof(XmiPoint3D);
    }
}
