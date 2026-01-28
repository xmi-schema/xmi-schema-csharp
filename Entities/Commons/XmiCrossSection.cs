using System;
using XmiSchema.Entities.Bases;
using XmiSchema.Enums;
using XmiSchema.Parameters;


namespace XmiSchema.Entities.Commons;

/// <summary>
/// Represents a structural cross-section profile with geometric and analytical section properties.
/// </summary>
/// <remarks>
/// This class encapsulates the geometry and section properties of structural cross-sections
/// used by structural members such as beams, columns, and braces. Cross-sections define
/// the physical shape and mechanical behavior of structural elements.
///
/// <h4>Key Components</h4>
/// <list type="bullet">
/// <item><description><see cref="Shape"/> - Geometric shape classification (I, rectangular, circular, etc.)</description></item>
/// <item><description><see cref="Parameters"/> - Shape-specific dimensional parameters</description></item>
/// <item><description><see cref="Area"/> - Cross-sectional area for axial calculations</description></item>
/// <item><description><see cref="SecondMomentOfAreaXAxis"/>, <see cref="SecondMomentOfAreaYAxis"/> - Stiffness about principal axes</description></item>
/// <item><description><see cref="ElasticModulusXAxis"/>, <see cref="ElasticModulusYAxis"/> - Elastic section moduli</description></item>
/// <item><description><see cref="PlasticModulusXAxis"/>, <see cref="PlasticModulusYAxis"/> - Plastic section moduli</description></item>
/// <item><description><see cref="TorsionalConstant"/> - Torsional stiffness</description></item>
/// </list>
///
/// <h4>Shape Parameters</h4>
/// The <see cref="Parameters"/> property contains shape-specific dimensional parameters
/// (e.g., width, depth, flange thickness for I-shapes). These are defined by
/// implementations of <see cref="XmiSchema.Parameters.IXmiShapeParameters"/> such as
/// <see cref="XmiSchema.Parameters.RectangularShapeParameters"/> and
/// <see cref="XmiSchema.Parameters.IShapeParameters"/>.
///
/// <h4>Validation</h4>
/// During construction, the class validates that the provided parameters object is compatible
/// with the specified shape type by calling <see cref="XmiSchema.Parameters.IXmiShapeParameters.Validate()"/>.
/// This ensures dimensional parameters are appropriate for the section shape.
///
/// <h4>Inheritance</h4>
/// Extends <see cref="XmiBaseGeometry"/> as cross-sections have geometric properties
/// and can be visualized as 2D shapes. The domain is set to <see cref="XmiBaseEntityDomainEnum.Geometry"/>.
///
/// <h4>Usage</h4>
/// Cross-sections are associated with structural members through <see cref="XmiHasCrossSection"/>
/// relationships. They are typically shared resources, referenced by multiple members with
/// the same profile.
///
/// <h4>Equality</h4>
/// Cross-sections are considered equal if their <see cref="XmiBaseEntity.NativeId"/> values
/// match (case-insensitive comparison).
/// </remarks>
/// <example>
/// Creating a rectangular cross-section:
/// <code>
/// var rectangularParams = new RectangularShapeParameters
/// {
///     Width = 300.0,   // mm
///     Height = 500.0    // mm
/// };
///
/// var rectangularSection = new XmiCrossSection(
///     id: "section_rect_300x500",
///     name: "Rectangular 300x500",
///     ifcGuid: "2h9$K0b0P6j7aB_1m5H7fR",
///     nativeId: "SEC-001",
///     description: "Reinforced concrete column section",
///     shape: XmiShapeEnum.Rectangular,
///     parameters: rectangularParams,
///     area: 150000.0,      // mm² (300 × 500)
///     secondMomentOfAreaXAxis: 3125000000.0,  // mm⁴ (500 × 300³ / 12)
///     secondMomentOfAreaYAxis: 1125000000.0,  // mm⁴ (300 × 500³ / 12)
///     radiusOfGyrationXAxis: 144.3,
///     radiusOfGyrationYAxis: 86.6,
///     elasticModulusXAxis: 15000000.0,  // mm³
///     elasticModulusYAxis: 4500000.0,   // mm³
///     plasticModulusXAxis: 22500000.0,  // mm³
///     plasticModulusYAxis: 7500000.0,   // mm³
///     torsionalConstant: 3.125e9  // mm⁴
/// );
/// </code>
/// </example>
/// <seealso cref="XmiHasCrossSection"/>
/// <seealso cref="XmiSchema.Parameters.IXmiShapeParameters"/>
/// <seealso cref="XmiShapeEnum"/>
/// <seealso cref="XmiBaseGeometry"/>
/// <seealso cref="XmiBaseEntityDomainEnum"/>
public class XmiCrossSection : XmiBaseGeometry, IEquatable<XmiCrossSection>
{
    /// <summary>
    /// Gets or sets the geometric shape classification of the cross-section.
    /// </summary>
    /// <value>The shape enumeration value (e.g., I-shaped, rectangular, circular).</value>
    /// <remarks>
    /// This property defines the geometric form of the cross-section, which determines the
    /// applicable parameter set and section properties calculation methods. Common shapes include:
    /// <list type="bullet">
    /// <item><description>Rectangular - Standard rectangle (e.g., concrete columns)</description></item>
    /// <item><description>I-shaped - I-beam profile (e.g., steel beams)</description></item>
    /// <item><description>Circular - Circular profile (e.g., pipe columns)</description></item>
    /// <item><description>L-shaped - Angle section</description></item>
    /// <item><description>T-shaped - Tee section</description></item>
    /// </list>
    /// </remarks>
    public XmiShapeEnum Shape { get; set; }

    /// <summary>
    /// Gets or sets the shape-specific dimensional parameters defining the cross-section geometry.
    /// </summary>
    /// <value>An implementation of <see cref="XmiSchema.Parameters.IXmiShapeParameters"/> containing dimension definitions.</value>
    /// <remarks>
    /// This property holds the dimensional parameters that define the exact geometry of the
    /// cross-section, such as width, depth, flange thickness, and web thickness.
    /// The parameter type must match the <see cref="Shape"/> property, otherwise
    /// an <see cref="System.ArgumentException"/> is thrown during construction.
    ///
    /// Common parameter implementations include:
    /// <list type="bullet">
    /// <item><description><see cref="XmiSchema.Parameters.RectangularShapeParameters"/> - width and height</description></item>
    /// <item><description><see cref="XmiSchema.Parameters.IShapeParameters"/> - full I-beam dimensions</description></item>
    /// <item><description><see cref="XmiSchema.Parameters.CircularShapeParameters"/> - diameter</description></item>
    /// </list>
    ///
    /// The <see cref="XmiSchema.Parameters.IXmiShapeParameters.Validate()"/> method is called
    /// during construction to ensure all required parameters are present and valid.
    /// </remarks>
    public IXmiShapeParameters Parameters { get; set; }

    /// <summary>
    /// Gets or sets the cross-sectional area.
    /// </summary>
    /// <value>The area in the project's unit system (e.g., mm², in²).</value>
    /// <remarks>
    /// This property defines the total area of the cross-section, used for axial stress
    /// calculations (σ = P/A), axial stiffness (EA), and self-weight calculations.
    /// </remarks>
    public double Area { get; set; }

    /// <summary>
    /// Gets or sets the second moment of area about the local X axis.
    /// </summary>
    /// <value>The second moment of area (moment of inertia) about the X axis (e.g., mm⁴, in⁴).</value>
    /// <remarks>
    /// This property measures the distribution of area around the X axis and is a key
    /// parameter for flexural stiffness and stress calculations. It's used in bending
    /// moment calculations: σ = My/I, where M is the moment, y is the distance from
    /// the neutral axis, and I is this value.
    ///
    /// For typical I-beams oriented with the strong axis in bending, this represents
    /// the strong axis stiffness.
    /// </remarks>
    public double SecondMomentOfAreaXAxis { get; set; }

    /// <summary>
    /// Gets or sets the second moment of area about the local Y axis.
    /// </summary>
    /// <value>The second moment of area (moment of inertia) about the Y axis (e.g., mm⁴, in⁴).</value>
    /// <remarks>
    /// This property measures the distribution of area around the Y axis and is used for
    /// flexural calculations in the perpendicular direction. For I-beams, this typically
    /// represents the weak axis stiffness.
    /// </remarks>
    public double SecondMomentOfAreaYAxis { get; set; }

    /// <summary>
    /// Gets or sets the radius of gyration about the local X axis.
    /// </summary>
    /// <value>The radius of gyration about the X axis (e.g., mm, in).</value>
    /// <remarks>
    /// The radius of gyration is calculated as √(I/A), where I is the moment of
    /// inertia and A is the area. It represents the distance from the neutral axis at
    /// which the entire area could be concentrated to produce the same moment of inertia.
    /// This property is used in slenderness calculations and buckling analysis.
    /// </remarks>
    public double RadiusOfGyrationXAxis { get; set; }

    /// <summary>
    /// Gets or sets the radius of gyration about the local Y axis.
    /// </summary>
    /// <value>The radius of gyration about the Y axis (e.g., mm, in).</value>
    /// <remarks>
    /// Similar to <see cref="RadiusOfGyrationXAxis"/> but measured about the Y axis.
    /// Used for slenderness calculations when buckling occurs about the weak axis.
    /// </remarks>
    public double RadiusOfGyrationYAxis { get; set; }

    /// <summary>
    /// Gets or sets the elastic section modulus about the local X axis.
    /// </summary>
    /// <value>The elastic section modulus about the X axis (e.g., mm³, in³).</value>
    /// <remarks>
    /// The elastic section modulus is calculated as I/c, where I is the moment of inertia
    /// and c is the distance from the neutral axis to the extreme fiber. It's used in
    /// elastic stress calculations: σ = M/Z, where M is the bending moment and
    /// Z is the section modulus.
    /// </remarks>
    public double ElasticModulusXAxis { get; set; }

    /// <summary>
    /// Gets or sets the elastic section modulus about the local Y axis.
    /// </summary>
    /// <value>The elastic section modulus about the Y axis (e.g., mm³, in³).</value>
    /// <remarks>
    /// Similar to <see cref="ElasticModulusXAxis"/> but for bending about the Y axis.
    /// </remarks>
    public double ElasticModulusYAxis { get; set; }

    /// <summary>
    /// Gets or sets the plastic section modulus about the local X axis.
    /// </summary>
    /// <value>The plastic section modulus about the X axis (e.g., mm³, in³).</value>
    /// <remarks>
    /// The plastic section modulus represents the fully plastic state of the cross-section
    /// when the entire section has yielded. It's larger than the elastic section
    /// modulus and is used in plastic design methods and capacity calculations.
    /// </remarks>
    public double PlasticModulusXAxis { get; set; }

    /// <summary>
    /// Gets or sets the plastic section modulus about the local Y axis.
    /// </summary>
    /// <value>The plastic section modulus about the Y axis (e.g., mm³, in³).</value>
    /// <remarks>
    /// Similar to <see cref="PlasticModulusXAxis"/> but for bending about the Y axis.
    /// </remarks>
    public double PlasticModulusYAxis { get; set; }

    /// <summary>
    /// Gets or sets the torsional constant (St. Venant's constant).
    /// </summary>
    /// <value>The torsional constant (e.g., mm⁴, in⁴).</value>
    /// <remarks>
    /// This property defines the section's resistance to torsional deformation. It's used in
    /// torsional stress calculations: τ = T/J, where T is the applied torque and J is
    /// the torsional constant. Critical for members subject to torsional loading.
    /// </remarks>
    public double TorsionalConstant { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmiCrossSection"/> class with the
    /// specified geometric and analytical properties.
    /// </summary>
    /// <param name="id">The stable, unique identifier for this cross-section. Must not be null or whitespace.</param>
    /// <param name="name">The human-readable display name. If null or whitespace, defaults to <paramref name="id"/>.</param>
    /// <param name="ifcGuid">The IFC GUID reference for BIM interoperability. Can be null.</param>
    /// <param name="nativeId">The identifier from the native source system for traceability. Can be null.</param>
    /// <param name="description">A textual description of this cross-section. Can be null.</param>
    /// <param name="shape">The geometric shape classification (e.g., <see cref="XmiShapeEnum.Rectangular"/>, <see cref="XmiShapeEnum.IShaped"/>).</param>
    /// <param name="parameters">The shape-specific dimensional parameters. Must not be null and must match <paramref name="shape"/>.</param>
    /// <param name="area">The cross-sectional area in project units.</param>
    /// <param name="secondMomentOfAreaXAxis">The second moment of area about the local X axis.</param>
    /// <param name="secondMomentOfAreaYAxis">The second moment of area about the local Y axis.</param>
    /// <param name="radiusOfGyrationXAxis">The radius of gyration about the X axis.</param>
    /// <param name="radiusOfGyrationYAxis">The radius of gyration about the Y axis.</param>
    /// <param name="elasticModulusXAxis">The elastic section modulus about the X axis.</param>
    /// <param name="elasticModulusYAxis">The elastic section modulus about the Y axis.</param>
    /// <param name="plasticModulusXAxis">The plastic section modulus about the X axis.</param>
    /// <param name="plasticModulusYAxis">The plastic section modulus about the Y axis.</param>
    /// <param name="torsionalConstant">The torsional constant (J).</param>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="parameters"/> is null.</exception>
    /// <exception cref="System.ArgumentException">Thrown when <paramref name="parameters.Shape"/> does not match <paramref name="shape"/>.</exception>
    /// <remarks>
    /// This constructor validates that the provided parameters object is compatible with the
    /// specified shape type. The validation ensures that the parameters object's
    /// <see cref="XmiSchema.Parameters.IXmiShapeParameters.Shape"/> property matches the
    /// <paramref name="shape"/> parameter, and calls <see cref="XmiSchema.Parameters.IXmiShapeParameters.Validate()"/>
    /// to check that all required parameters are present.
    ///
    /// Note: A material property is commented out in the current implementation. Material
    /// associations are typically handled through <see cref="XmiHasMaterial"/> relationships
    /// rather than direct references.
    /// </remarks>
    /// <seealso cref="XmiSchema.Parameters.IXmiShapeParameters"/>
    /// <seealso cref="XmiSchema.Parameters.RectangularShapeParameters"/>
    /// <seealso cref="XmiSchema.Parameters.IShapeParameters"/>
    public XmiCrossSection(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        // XmiMaterial material,
        XmiShapeEnum shape,
        IXmiShapeParameters parameters,
        double area,
        double secondMomentOfAreaXAxis,
        double secondMomentOfAreaYAxis,
        double radiusOfGyrationXAxis,
        double radiusOfGyrationYAxis,
        double elasticModulusXAxis,
        double elasticModulusYAxis,
        double plasticModulusXAxis,
        double plasticModulusYAxis,
        double torsionalConstant
    ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiCrossSection))
    {

        // Material = material;
        if (parameters is null) throw new ArgumentNullException(nameof(parameters));

        if (parameters.Shape != shape)
        {
            throw new ArgumentException($"Parameter set is defined for {parameters.Shape} but {shape} was requested.", nameof(parameters));
        }

        Shape = shape;
        Parameters = parameters;
        Parameters.Validate();
        Area = area;
        SecondMomentOfAreaXAxis = secondMomentOfAreaXAxis;
        SecondMomentOfAreaYAxis = secondMomentOfAreaYAxis;
        RadiusOfGyrationXAxis = radiusOfGyrationXAxis;
        RadiusOfGyrationYAxis = radiusOfGyrationYAxis;
        ElasticModulusXAxis = elasticModulusXAxis;
        ElasticModulusYAxis = elasticModulusYAxis;
        PlasticModulusXAxis = plasticModulusXAxis;
        PlasticModulusYAxis = plasticModulusYAxis;
        TorsionalConstant = torsionalConstant;
    }

    /// <summary>
    /// Determines whether this <see cref="XmiCrossSection"/> is equal to another cross-section.
    /// </summary>
    /// <param name="other">The cross-section to compare with this cross-section, or null.</param>
    /// <returns>True if cross-sections have matching <see cref="XmiBaseEntity.NativeId"/> values; otherwise, false.</returns>
    /// <remarks>
    /// Cross-sections are considered equal if their <see cref="XmiBaseEntity.NativeId"/> values
    /// match using case-insensitive comparison. This enables cross-section deduplication
    /// based on source system identifiers.
    /// </remarks>
    public bool Equals(XmiCrossSection? other)
    {
        if (other is null) return false;
        return string.Equals(NativeId, other.NativeId, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Determines whether this <see cref="XmiCrossSection"/> is equal to another object.
    /// </summary>
    /// <param name="obj">The object to compare with this cross-section, or null.</param>
    /// <returns>True if <paramref name="obj"/> is an <see cref="XmiCrossSection"/> with matching <see cref="XmiBaseEntity.NativeId"/>; otherwise, false.</returns>
    public override bool Equals(object? obj) => Equals(obj as XmiCrossSection);

    /// <summary>
    /// Returns a hash code for this <see cref="XmiCrossSection"/> based on its native identifier.
    /// </summary>
    /// <returns>A hash code derived from the lower-case <see cref="XmiBaseEntity.NativeId"/>.</returns>
    /// <remarks>
    /// The hash code is generated from the case-insensitive <see cref="XmiBaseEntity.NativeId"/>
    /// to ensure consistency with the <see cref="Equals(XmiCrossSection)"/> implementation.
    /// </remarks>
    public override int GetHashCode() => NativeId?.ToLowerInvariant().GetHashCode() ?? 0;
}
