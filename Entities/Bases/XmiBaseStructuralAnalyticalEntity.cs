using XmiSchema.Enums;

namespace XmiSchema.Entities.Bases
{
    /// <summary>
    /// Abstract base class for all structural analytical elements in the XMI schema.
    /// </summary>
    /// <remarks>
    /// This class extends <see cref="XmiBaseEntity"/> to provide a common foundation for
    /// idealized, analysis-ready representations of structural elements. Derived classes represent
    /// specific analytical element types used in structural analysis:
    ///
    /// <list type="bullet">
    /// <item><description><see cref="XmiStructuralCurveMember"/> - Linear analytical members (beams, columns, bracing)</description></item>
    /// <item><description><see cref="XmiStructuralSurfaceMember"/> - Surface-based analytical elements (slabs, walls, plates)</description></item>
    /// <item><description><see cref="XmiStructuralPointConnection"/> - Analytical nodes connecting members</description></item>
    /// </list>
    ///
    /// Analytical entities represent simplified, idealized versions of physical elements
    /// suitable for structural analysis calculations. They typically have:
    /// <list type="bullet">
    /// <item><description>Simplified geometry (lines for curve members, surfaces for surface members)</description></item>
    /// <item><description>Material and section properties for analysis</description></item>
    /// <item><description>Connection fixity conditions</description></item>
    /// <item><description>Load application points</description></item>
    /// </list>
    ///
    /// They are distinguished from physical entities (<see cref="XmiBasePhysicalEntity"/>)
    /// which represent actual constructible elements with full geometry and detailing.
    ///
    /// All analytical entities have their <see cref="XmiBaseEntity.Domain"/> property set to
    /// <see cref="XmiBaseEntityDomainEnum.StructuralAnalytical"/> for proper classification and querying.
    ///
    /// Analytical elements are typically linked to physical elements through relationships
    /// such as <see cref="XmiHasStructuralCurveMember"/>, enabling bidirectional mapping
    /// between physical and analytical models for traceability and synchronization.
    /// </remarks>
    /// <example>
    /// Creating an analytical curve member (beam):
    /// <code>
    /// var startPoint = model.CreatePoint3D(0.0, 0.0, 0.0);
    /// var endPoint = model.CreatePoint3D(6.0, 0.0, 0.0);
    /// var line = new XmiLine3D(/*...*/);
    ///
    /// var curveMember = new XmiStructuralCurveMember(
    ///     id: "curve_member_001",
    ///     name: "Analytical Beam B-1",
    ///     ifcGuid: "2h9$K0b0P6j7aB_1m5H7fR",
    ///     nativeId: "CM-12345",
    ///     description: "Idealized beam for structural analysis",
    ///     domain: XmiBaseEntityDomainEnum.StructuralAnalytical,
    ///     // Additional analytical parameters...
    ///     endFixityStart: new XmiFixity(...),
    ///     endFixityEnd: new XmiFixity(...),
    ///     // Material, cross-section, geometry relationships added separately
    /// );
    /// // curveMember.Domain == XmiBaseEntityDomainEnum.StructuralAnalytical
    /// </code>
    /// </example>
    /// <seealso cref="XmiStructuralCurveMember"/>
    /// <seealso cref="XmiStructuralSurfaceMember"/>
    /// <seealso cref="XmiStructuralPointConnection"/>
    /// <seealso cref="XmiBaseEntity"/>
    /// <seealso cref="XmiBasePhysicalEntity"/>
    /// <seealso cref="XmiBaseEntityDomainEnum"/>
    public abstract class XmiBaseStructuralAnalyticalEntity : XmiBaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmiBaseStructuralAnalyticalEntity"/> class
        /// with the specified identifiers and metadata.
        /// </summary>
        /// <param name="id">The stable, unique identifier for this analytical element. Must not be null or whitespace.</param>
        /// <param name="name">The human-readable display name. If null or whitespace, defaults to <paramref name="id"/>.</param>
        /// <param name="ifcGuid">The IFC GUID reference for BIM interoperability. Can be null.</param>
        /// <param name="nativeId">The identifier from the native source system for traceability. Can be null.</param>
        /// <param name="description">A textual description of this analytical element. Can be null.</param>
        /// <param name="entityName">The entity type name for polymorphic deserialization
        /// (e.g., "XmiStructuralCurveMember", "XmiStructuralSurfaceMember").</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="id"/> is null or whitespace.</exception>
        /// <remarks>
        /// This constructor passes the provided parameters to the base <see cref="XmiBaseEntity"/> constructor,
        /// automatically setting the <see cref="XmiBaseEntity.Domain"/> property to
        /// <see cref="XmiBaseEntityDomainEnum.StructuralAnalytical"/>. The <paramref name="entityName"/> parameter
        /// must be provided by derived classes to specify their concrete type name for proper JSON polymorphic
        /// deserialization.
        ///
        /// Derived classes (e.g., <see cref="XmiStructuralCurveMember"/>, <see cref="XmiStructuralSurfaceMember"/>)
        /// should accept additional domain-specific parameters in their constructors and pass their type name
        /// to <paramref name="entityName"/>.
        ///
        /// Analytical elements are typically created through <see cref="XmiModel"/> factory methods that
        /// handle relationship creation to materials, cross-sections, geometries, and connections.
        /// </remarks>
        /// <example>
        /// Constructor pattern in a derived class:
        /// <code>
        /// public class XmiStructuralCurveMember : XmiBaseStructuralAnalyticalEntity
        /// {
        ///     public XmiStructuralCurveMember(
        ///         string id,
        ///         string name,
        ///         // ... base parameters ...
        ///         XmiFixity endFixityStart,
        ///         XmiFixity endFixityEnd
        ///     ) : base(id, name, ifcGuid, nativeId, description, "XmiStructuralCurveMember")
        ///     {
        ///         // Initialize analytical member-specific properties
        ///     }
        /// }
        /// </code>
        /// </example>
        protected XmiBaseStructuralAnalyticalEntity(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            string entityName
        ) : base(id, name, ifcGuid, nativeId, description, entityName, XmiBaseEntityDomainEnum.StructuralAnalytical)
        {
        }
    }
}
