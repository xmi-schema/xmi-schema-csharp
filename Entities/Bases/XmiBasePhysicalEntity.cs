using XmiSchema.Enums;

namespace XmiSchema.Entities.Bases
{
    /// <summary>
    /// Abstract base class for all physical building elements in the XMI schema.
    /// </summary>
    /// <remarks>
    /// This class extends <see cref="XmiBaseEntity"/> to provide a common foundation for
    /// physical building components that represent actual, constructible elements in a
    /// building model. Derived classes represent specific physical element types:
    ///
    /// <list type="bullet">
    /// <item><description><see cref="XmiBeam"/> - Horizontal structural member spanning between supports</description></item>
    /// <item><description><see cref="XmiColumn"/> - Vertical structural member supporting loads</description></item>
    /// <item><description><see cref="XmiSlab"/> - Horizontal structural surface (floor or roof)</description></item>
    /// <item><description><see cref="XmiWall"/> - Vertical structural or non-structural partition</description></item>
    /// </list>
    ///
    /// Physical entities represent the "as-designed" or "as-built" state of building
    /// elements, including their geometry, material properties, and physical attributes.
    /// They are distinguished from analytical entities (<see cref="XmiBaseStructuralAnalyticalEntity"/>)
    /// which represent idealized, analysis-ready representations.
    ///
    /// All physical entities have their <see cref="XmiBaseEntity.Domain"/> property set to
    /// <see cref="XmiBaseEntityDomainEnum.Physical"/> for proper classification and querying.
    ///
    /// Physical elements are typically linked to analytical elements through relationships
    /// such as <see cref="XmiHasStructuralCurveMember"/>, enabling bidirectional mapping
    /// between physical and analytical models.
    /// </remarks>
    /// <example>
    /// Creating a physical beam entity:
    /// <code>
    /// var beam = new XmiBeam(
    ///     id: "beam_001",
    ///     name: "Main Beam B-1",
    ///     ifcGuid: "2h9$K0b0P6j7aB_1m5H7fR",
    ///     nativeId: "B-12345",
    ///     description: "Primary structural beam spanning column grid A-B",
    ///     domain: XmiBaseEntityDomainEnum.Physical,
    ///     // Additional beam-specific parameters...
    ///     endFixityStart: new XmiFixity(...),
    ///     endFixityEnd: new XmiFixity(...)
    /// );
    /// // beam.Domain == XmiBaseEntityDomainEnum.Physical
    /// </code>
    /// </example>
    /// <seealso cref="XmiBeam"/>
    /// <seealso cref="XmiColumn"/>
    /// <seealso cref="XmiSlab"/>
    /// <seealso cref="XmiWall"/>
    /// <seealso cref="XmiBaseEntity"/>
    /// <seealso cref="XmiBaseStructuralAnalyticalEntity"/>
    /// <seealso cref="XmiBaseEntityDomainEnum"/>
    public abstract class XmiBasePhysicalEntity : XmiBaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmiBasePhysicalEntity"/> class with
        /// the specified identifiers and metadata.
        /// </summary>
        /// <param name="id">The stable, unique identifier for this physical element. Must not be null or whitespace.</param>
        /// <param name="name">The human-readable display name. If null or whitespace, defaults to <paramref name="id"/>.</param>
        /// <param name="ifcGuid">The IFC GUID reference for BIM interoperability. Can be null.</param>
        /// <param name="nativeId">The identifier from the native source system for traceability. Can be null.</param>
        /// <param name="description">A textual description of this physical element. Can be null.</param>
        /// <param name="entityName">The entity type name for polymorphic deserialization (e.g., "XmiBeam", "XmiColumn").</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="id"/> is null or whitespace.</exception>
        /// <remarks>
        /// This constructor passes the provided parameters to the base <see cref="XmiBaseEntity"/> constructor,
        /// automatically setting the <see cref="XmiBaseEntity.Domain"/> property to
        /// <see cref="XmiBaseEntityDomainEnum.Physical"/>. The <paramref name="entityName"/> parameter
        /// must be provided by derived classes to specify their concrete type name for proper JSON polymorphic
        /// deserialization.
        ///
        /// Derived classes (e.g., <see cref="XmiBeam"/>, <see cref="XmiColumn"/>) should accept additional
        /// domain-specific parameters in their constructors and pass their type name to <paramref name="entityName"/>.
        /// </remarks>
        /// <example>
        /// Constructor pattern in a derived class:
        /// <code>
        /// public class XmiBeam : XmiBasePhysicalEntity
        /// {
        ///     public XmiBeam(
        ///         string id,
        ///         string name,
        ///         // ... base parameters ...
        ///         XmiFixity endFixityStart,
        ///         XmiFixity endFixityEnd
        ///     ) : base(id, name, ifcGuid, nativeId, description, "XmiBeam")
        ///     {
        ///         // Initialize beam-specific properties
        ///     }
        /// }
        /// </code>
        /// </example>
        protected XmiBasePhysicalEntity(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            string entityName
        ) : base(id, name, ifcGuid, nativeId, description, entityName, XmiBaseEntityDomainEnum.Physical)
        {
        }
    }
}
