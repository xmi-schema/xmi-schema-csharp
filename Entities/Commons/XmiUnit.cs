using XmiSchema.Entities.Bases;
using XmiSchema.Enums;

namespace XmiSchema.Entities.Commons;

/// <summary>
/// Defines a mapping between entity attributes and their measurement units.
/// </summary>
/// <remarks>
/// This class establishes the unit system used for specific attributes on entities,
/// enabling consistent unit conversion and interpretation during data exchange between
/// different systems. It resolves ambiguity when multiple unit systems are in use.
///
/// <h4>Purpose</h4>
/// Units are essential for interoperability between systems that may use different
/// measurement standards (e.g., metric vs. imperial). By explicitly declaring
/// which unit is used for each attribute, downstream systems can correctly:
/// <list type="bullet">
/// <item><description>Interpret numerical values</description></item>
/// <item><description>Perform unit conversions</description></item>
/// <item><description>Validate dimensional consistency</description></item>
/// <item><description>Generate correctly labeled outputs</description></item>
/// </list>
///
/// <h4>Key Components</h4>
/// <list type="bullet">
/// <item><description><see cref="Entity"/> - Name of the entity class or type</description></item>
/// <item><description><see cref="Attribute"/> - Name of the property or attribute</description></item>
/// <item><description><see cref="Unit"/> - The measurement unit enumeration</description></item>
/// </list>
///
/// <h4>Common Use Cases</h4>
/// Units are defined for:
/// <list type="bullet">
/// <item><description>Geometric dimensions (length, area, volume)</description></item>
/// <item><description>Mechanical properties (force, stress, modulus)</description></item>
/// <item><description>Mass and weight values</description></item>
/// <item><description>Angular measurements (rotations)</description></item>
/// <item><description>Temperature values</description></item>
/// </list>
///
/// <h4>Domain</h4>
/// All units have their <see cref="XmiBaseEntity.Domain"/> property set to
/// <see cref="XmiBaseEntityDomainEnum.Shared"/> as they provide metadata that
/// applies across the entire XMI graph.
/// </remarks>
/// <example>
/// Defining units for beam properties:
/// <code>
/// var lengthUnit = new XmiUnit(
///     id: "unit_beam_length",
///     name: "Beam Length Unit",
///     ifcGuid: "2h9$K0b0P6j7aB_1m5H7fR",
///     nativeId: "UNIT-001",
///     description: "Length attribute of XmiBeam in meters",
///     entity: "XmiBeam",
///     attribute: "Length",
///     unit: XmiUnitEnum.Meter
/// );
///
/// var forceUnit = new XmiUnit(
///     id: "unit_beam_force",
///     name: "Beam Force Unit",
///     ifcGuid: "1a2b3c4d5e6f",
///     nativeId: "UNIT-002",
///     description: "Force values in kilonewtons",
///     entity: "XmiBeam",
///     attribute: "Force",
///     unit: XmiUnitEnum.KiloNewton
/// );
/// </code>
/// </example>
/// <example>
/// Defining unit for material density:
/// <code>
/// var densityUnit = new XmiUnit(
///     id: "unit_material_density",
///     name: "Material Density Unit",
///     ifcGuid: "3d4e5f6a7b8c",
///     nativeId: "UNIT-003",
///     description: "Unit weight of materials in kN/m³",
///     entity: "XmiMaterial",
///     attribute: "UnitWeight",
///     unit: XmiUnitEnum.KiloNewtonPerCubicMeter
/// );
/// </code>
/// </example>
/// <seealso cref="XmiUnitEnum"/>
/// <seealso cref="XmiBaseEntity"/>
/// <seealso cref="XmiBaseEntityDomainEnum"/>
public class XmiUnit : XmiBaseEntity
{
    /// <summary>
    /// Gets or sets the name of the entity type whose attribute uses this unit.
    /// </summary>
    /// <value>The entity type name as a string (e.g., "XmiBeam", "XmiMaterial", "XmiCrossSection").</value>
    /// <remarks>
    /// This property identifies which entity class or type this unit mapping applies to.
    /// It is typically the C# class name (e.g., "XmiBeam" for beam elements),
    /// enabling the mapping to be applied to all instances of that entity type.
    ///
    /// For example, if <see cref="Entity"/> is "XmiBeam" and <see cref="Attribute"/>
    /// is "Length", then all beam elements should interpret their length values using
    /// the specified <see cref="Unit"/>.
    /// </remarks>
    public string Entity { get; set; }

    /// <summary>
    /// Gets or sets the name of the attribute or property on the entity.
    /// </summary>
    /// <value>The attribute name as a string (e.g., "Length", "Force", "UnitWeight", "Area").</value>
    /// <remarks>
    /// This property identifies which specific property or attribute on the entity type
    /// uses this unit mapping. It works in conjunction with <see cref="Entity"/>
    /// to uniquely identify the attribute requiring unit specification.
    ///
    /// Common attributes include:
    /// <list type="bullet">
    /// <item><description>"Length" - Linear dimensions</description></item>
    /// <item><description>"Area" - Section areas</description></item>
    /// <item><description>"Force" - Applied loads</description></item>
    /// <item><description>"UnitWeight" - Material density</description></item>
    /// <item><description>"Stress" - Material strengths</description></item>
    /// </list>
    /// </remarks>
    public string Attribute { get; set; }

    /// <summary>
    /// Gets or sets the measurement unit enumeration value.
    /// </summary>
    /// <value>The unit enumeration value indicating the measurement system.</value>
    /// <remarks>
    /// This property defines which unit of measurement is used for the attribute.
    /// The enumeration covers various unit categories:
    /// <list type="bullet">
    /// <item><description>Length units: Meter, Millimeter, Foot, Inch</description></item>
    /// <item><description>Force units: KiloNewton, Pound, Kip</description></item>
    /// <item><description>Mass units: Kilogram, PoundMass</description></item>
    /// <item><description>Pressure/Stress units: KiloNewtonPerSquareMeter, PSI</description></item>
    /// <item><description>Density units: KiloNewtonPerCubicMeter, PoundPerCubicFoot</description></item>
    /// <item><description>Temperature units: Celsius, Fahrenheit</description></item>
    /// </list>
    ///
    /// Downstream systems use this information to correctly interpret numerical values
    /// and perform conversions to their preferred unit system.
    /// </remarks>
    /// <seealso cref="XmiUnitEnum"/>
    public XmiUnitEnum Unit { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmiUnit"/> class that maps
    /// a specific entity attribute to a measurement unit.
    /// </summary>
    /// <param name="id">The stable, unique identifier for this unit mapping. Must not be null or whitespace.</param>
    /// <param name="name">The human-readable display name. If null or whitespace, defaults to <paramref name="id"/>.</param>
    /// <param name="ifcGuid">The IFC GUID reference for BIM interoperability. Can be null.</param>
    /// <param name="nativeId">The identifier from the native source system for traceability. Can be null.</param>
    /// <param name="description">A textual description of this unit mapping (e.g., which system it's intended for). Can be null.</param>
    /// <param name="entity">The name of the entity type whose attribute uses this unit (e.g., "XmiBeam", "XmiMaterial").</param>
    /// <param name="attribute">The name of the specific attribute or property on the entity (e.g., "Length", "Force", "UnitWeight").</param>
    /// <param name="unit">The measurement unit enumeration indicating the unit system for this attribute.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="id"/> is null or whitespace.</exception>
    /// <remarks>
    /// This constructor creates a unit mapping that associates a specific attribute on an
    /// entity type with a measurement unit. The mapping enables downstream systems
    /// to correctly interpret numerical values and perform unit conversions.
    ///
    /// The <see cref="XmiBaseEntity.Domain"/> property is set to
    /// <see cref="XmiBaseEntityDomainEnum.Shared"/> as unit mappings provide
    /// metadata that applies across the entire XMI graph and is not specific to
    /// a single entity instance.
    ///
    /// Unit mappings are typically managed by <see cref="XmiModel"/> and included in
    /// the XMI graph as reference information for all entities.
    /// </remarks>
    /// <seealso cref="XmiUnitEnum"/>
    /// <seealso cref="XmiModel"/>
    /// <seealso cref="XmiBaseEntityDomainEnum.Shared"/>
    public XmiUnit(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        string entity,
        string attribute,
        XmiUnitEnum unit
    ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiUnit), XmiBaseEntityDomainEnum.Shared)
    {
        Entity = entity;
        Attribute = attribute;
        Unit = unit;
    }
}
}
