using Newtonsoft.Json;
using XmiSchema.Entities.Bases;
using XmiSchema.Enums;

namespace XmiSchema.Entities.Commons;

/// <summary>
/// Represents a structural material with mechanical properties for structural analysis.
/// </summary>
/// <remarks>
/// This class encapsulates the physical and mechanical properties of construction materials
/// used in building design and analysis. Materials can be referenced by structural elements
/// such as beams, columns, slabs, and walls through <see cref="XmiHasMaterial"/> relationships.
///
/// <h4>Material Properties</h4>
/// The class stores both basic material classification and detailed mechanical properties:
/// <list type="bullet">
/// <item><description><see cref="MaterialType"/> - General category (concrete, steel, timber, etc.)</description></item>
/// <item><description><see cref="Grade"/> - Strength grade or material designation</description></item>
/// <item><description><see cref="UnitWeight"/> - Mass density for weight calculations</description></item>
/// <item><description><see cref="ElasticModulus"/> - Young's modulus (E) for elastic deformation</description></item>
/// <item><description><see cref="ShearModulus"/> - Shear modulus (G) for shear deformation</description></item>
/// <item><description><see cref="PoissonRatio"/> - Poisson's ratio for lateral strain</description></item>
/// <item><description><see cref="ThermalCoefficient"/> - Coefficient of thermal expansion</description></item>
/// </list>
///
/// <h4>Serialization</h4>
/// Elastic modulus and shear modulus are stored as strings to preserve numerical precision
/// and allow units to be included in the serialized value. The <see cref="Newtonsoft.Json.JsonPropertyAttribute"/>
/// specifies custom property names "EModulus" and "GModulus" in JSON output.
///
/// <h4>Equality</h4>
/// Materials are considered equal if their <see cref="XmiBaseEntity.NativeId"/> values match
/// (case-insensitive comparison), enabling material deduplication based on source system identifiers.
///
/// <h4>Domain</h4>
/// All materials have their <see cref="XmiBaseEntity.Domain"/> property set to
/// <see cref="XmiBaseEntityDomainEnum.Shared"/> as they can be referenced by both physical
/// and analytical elements across the XMI graph.
/// </remarks>
/// <example>
/// Creating a concrete material:
/// <code>
/// var concrete = new XmiMaterial(
///     id: "mat_concrete_c30",
///     name: "C30 Concrete",
///     ifcGuid: "2h9$K0b0P6j7aB_1m5H7fR",
///     nativeId: "MAT-001",
///     description: "Structural concrete grade C30/37",
///     materialType: XmiMaterialTypeEnum.Concrete,
///     grade: 30.0,
///     unitWeight: 24.0,       // kN/m³
///     elasticModulus: "32000",  // MPa (stored as string)
///     shearModulus: "13333",    // MPa (stored as string)
///     poissonRatio: "0.2",
///     thermalCoefficient: 0.00001 // per °C
/// );
/// </code>
/// </example>
/// <example>
/// Creating a steel material:
/// <code>
/// var steel = new XmiMaterial(
///     id: "mat_steel_s355",
///     name: "S355 Steel",
///     ifcGuid: "1a2b3c4d5e6f",
///     nativeId: "MAT-002",
///     description: "Structural steel grade S355",
///     materialType: XmiMaterialTypeEnum.Steel,
///     grade: 355.0,
///     unitWeight: 78.5,
///     elasticModulus: "210000",
///     shearModulus: "81000",
///     poissonRatio: "0.3",
///     thermalCoefficient: 0.000012
/// );
/// </code>
/// </example>
/// <seealso cref="XmiHasMaterial"/>
/// <seealso cref="XmiMaterialTypeEnum"/>
/// <seealso cref="XmiBaseEntity"/>
/// <seealso cref="XmiBaseEntityDomainEnum"/>
public class XmiMaterial : XmiBaseEntity, IEquatable<XmiMaterial>
{
    /// <summary>
    /// Gets or sets the general material category or type.
    /// </summary>
    /// <value>The material type enumeration value.</value>
    /// <remarks>
    /// This property categorizes the material into broad types such as concrete, steel, timber,
    /// aluminum, or other construction materials. The type determines the expected range of
    /// mechanical properties and influences analysis assumptions.
    /// </remarks>
    public XmiMaterialTypeEnum MaterialType { get; set; }

    /// <summary>
    /// Gets or sets the material grade or strength designation.
    /// </summary>
    /// <value>The grade or strength number (e.g., 30 for C30 concrete, 355 for S355 steel).</value>
    /// <remarks>
    /// This value represents the standardized grade or strength specification of the material.
    /// For concrete, it typically refers to the characteristic compressive strength in MPa.
    /// For steel, it refers to the yield strength in MPa. The value is used in design
    /// code calculations and capacity checks.
    /// </remarks>
    public double Grade { get; set; }

    /// <summary>
    /// Gets or sets the unit weight (density) of the material.
    /// </summary>
    /// <value>The mass density in the project's unit system (e.g., kN/m³, lb/ft³).</value>
    /// <remarks>
    /// This property defines the weight per unit volume of the material, used for self-weight
    /// calculations in structural analysis. Common values include approximately 24 kN/m³ for
    /// normal-weight concrete and 78.5 kN/m³ for steel.
    /// </remarks>
    public double UnitWeight { get; set; }

    /// <summary>
    /// Gets or sets the Young's modulus (elastic modulus) of the material.
    /// </summary>
    /// <value>The elastic modulus as a string to preserve precision and units (e.g., "32000" MPa).</value>
    /// <remarks>
    /// This property defines the material's stiffness and resistance to elastic deformation under
    /// axial or flexural loading. It is stored as a string to preserve numerical precision
    /// and allow units to be explicitly specified. Typical values range from ~20,000-35,000 MPa
    /// for concrete to ~200,000-210,000 MPa for steel.
    ///
    /// Serialized to JSON with property name "EModulus" via <see cref="Newtonsoft.Json.JsonPropertyAttribute"/>.
    /// </remarks>
    [JsonProperty(PropertyName = "EModulus")]
    public string ElasticModulus { get; set; }

    /// <summary>
    /// Gets or sets the shear modulus (rigidity modulus) of the material.
    /// </summary>
    /// <value>The shear modulus as a string to preserve precision and units (e.g., "13333" MPa).</value>
    /// <remarks>
    /// This property defines the material's resistance to shear deformation. It is related to
    /// Young's modulus by the formula: G = E / (2 * (1 + ν)), where ν is Poisson's ratio.
    /// It is stored as a string for the same reasons as <see cref="ElasticModulus"/>.
    ///
    /// Serialized to JSON with property name "GModulus" via <see cref="Newtonsoft.Json.JsonPropertyAttribute"/>.
    /// </remarks>
    [JsonProperty(PropertyName = "GModulus")]
    public string ShearModulus { get; set; }

    /// <summary>
    /// Gets or sets the Poisson's ratio of the material.
    /// </summary>
    /// <value>The Poisson's ratio as a string (dimensionless, typically 0.1-0.35).</value>
    /// <remarks>
    /// This property defines the ratio of lateral strain to axial strain when a material is
    /// stretched. It is a fundamental material property affecting shear modulus and
    /// volumetric behavior. Typical values are ~0.2 for concrete and ~0.3 for steel.
    /// The value is stored as a string for consistency with modulus properties.
    /// </remarks>
    public string PoissonRatio { get; set; }

    /// <summary>
    /// Gets or sets the coefficient of thermal expansion of the material.
    /// </summary>
    /// <value>The thermal expansion coefficient (per degree unit, typically 1e-5 to 1e-6).</value>
    /// <remarks>
    /// This property defines how much the material expands or contracts per unit change in
    /// temperature. It is critical for thermal stress analysis, differential expansion
    /// calculations, and movement joint design. Typical values range from 1e-5 for concrete
    /// to 1.2e-5 for steel.
    /// </remarks>
    public double ThermalCoefficient { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmiMaterial"/> class with the
    /// specified physical and mechanical properties.
    /// </summary>
    /// <param name="id">The stable, unique identifier for this material. Must not be null or whitespace.</param>
    /// <param name="name">The human-readable display name. If null or whitespace, defaults to <paramref name="id"/>.</param>
    /// <param name="ifcGuid">The IFC GUID reference for BIM interoperability. Can be null.</param>
    /// <param name="nativeId">The identifier from the native source system for traceability. Can be null.</param>
    /// <param name="description">A textual description of the material's use and characteristics. Can be null.</param>
    /// <param name="materialType">The generalized material category (e.g., <see cref="XmiMaterialTypeEnum.Concrete"/>, <see cref="XmiMaterialTypeEnum.Steel"/>).</param>
    /// <param name="grade">The material strength grade or designation (e.g., 30 for C30, 355 for S355).</param>
    /// <param name="unitWeight">The mass density in project units (e.g., 24.0 for concrete, 78.5 for steel).</param>
    /// <param name="elasticModulus">The elastic modulus (E) as a string with units preserved (e.g., "32000" MPa).</param>
    /// <param name="shearModulus">The shear modulus (G) as a string with units preserved (e.g., "13333" MPa).</param>
    /// <param name="poissonRatio">The Poisson's ratio as a string (dimensionless, e.g., "0.2").</param>
    /// <param name="thermalCoefficient">The thermal expansion coefficient (e.g., 0.00001).</param>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="id"/> is null or whitespace.</exception>
    /// <remarks>
    /// This constructor initializes all material properties and sets the <see cref="XmiBaseEntity.Domain"/>
    /// property to <see cref="XmiBaseEntityDomainEnum.Shared"/> as materials are shared resources
    /// across the XMI graph.
    ///
    /// Modulus values are stored as strings to preserve numerical precision and allow unit
    /// specifications to be included. This is particularly important when interfacing with
    /// external systems that require specific unit formats.
    /// </remarks>
    /// <seealso cref="XmiHasMaterial"/>
    /// <seealso cref="XmiMaterialTypeEnum"/>
    /// <seealso cref="XmiBaseEntityDomainEnum.Shared"/>
    public XmiMaterial(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        XmiMaterialTypeEnum materialType,
        double grade,
        double unitWeight,
        string elasticModulus,
        string shearModulus,
        string poissonRatio,
        double thermalCoefficient
    ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiMaterial), XmiBaseEntityDomainEnum.Shared)
    {
        MaterialType = materialType;
        Grade = grade;
        UnitWeight = unitWeight;
        ElasticModulus = elasticModulus;
        ShearModulus = shearModulus;
        PoissonRatio = poissonRatio;
        ThermalCoefficient = thermalCoefficient;
    }

    /// <summary>
    /// Determines whether this <see cref="XmiMaterial"/> is equal to another material.
    /// </summary>
    /// <param name="other">The material to compare with this material, or null.</param>
    /// <returns>True if the materials have matching <see cref="XmiBaseEntity.NativeId"/> values; otherwise, false.</returns>
    /// <remarks>
    /// Materials are considered equal if their <see cref="XmiBaseEntity.NativeId"/> values match
    /// using case-insensitive comparison. This enables material deduplication based on source
    /// system identifiers rather than requiring exact object references.
    /// </remarks>
    public bool Equals(XmiMaterial? other)
    {
        if (other is null) return false;
        return string.Equals(NativeId, other.NativeId, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Determines whether this <see cref="XmiMaterial"/> is equal to another object.
    /// </summary>
    /// <param name="obj">The object to compare with this material, or null.</param>
    /// <returns>True if <paramref name="obj"/> is an <see cref="XmiMaterial"/> with matching <see cref="XmiBaseEntity.NativeId"/>; otherwise, false.</returns>
    public override bool Equals(object? obj) => Equals(obj as XmiMaterial);

    /// <summary>
    /// Returns a hash code for this <see cref="XmiMaterial"/> based on its native identifier.
    /// </summary>
    /// <returns>A hash code derived from the lower-case <see cref="XmiBaseEntity.NativeId"/>.</returns>
    /// <remarks>
    /// The hash code is generated from the case-insensitive <see cref="XmiBaseEntity.NativeId"/>
    /// to ensure consistency with the <see cref="Equals(XmiMaterial)"/> implementation.
    /// </remarks>
    public override int GetHashCode() =>
        NativeId?.ToLowerInvariant().GetHashCode() ?? 0;
}

    public bool Equals(XmiMaterial? other)
    {
        if (other is null) return false;
        return string.Equals(NativeId, other.NativeId, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as XmiMaterial);

    public override int GetHashCode() =>
        NativeId?.ToLowerInvariant().GetHashCode() ?? 0;
}
