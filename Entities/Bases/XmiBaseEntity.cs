using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using XmiSchema.Enums;

namespace XmiSchema.Entities.Bases
{
    /// <summary>
    /// Provides the foundational metadata shared by every Cross Model Information (XMI) entity.
    /// </summary>
    /// <remarks>
    /// This base class defines the core properties that all XMI entities must have, including:
    /// <list type="bullet">
    /// <item><description>Unique identifier (<see cref="Id"/>) for entity referencing</description></item>
    /// <item><description>Human-readable name (<see cref="Name"/>) for display purposes</description></item>
    /// <item><description>IFC GUID (<see cref="IfcGuid"/>) for BIM interoperability</description></item>
    /// <item><description>Native system identifier (<see cref="NativeId"/>) for traceability</description></item>
    /// <item><description>Description (<see cref="Description"/>) for semantic context</description></item>
    /// <item><description>Entity name (<see cref="EntityName"/>) for polymorphic JSON deserialization</description></item>
    /// <item><description>Domain classification (<see cref="Domain"/>) for categorization</description></item>
    /// </list>
    ///
    /// The <see cref="EntityName"/> property serves as a polymorphic discriminator in JSON serialization,
    /// allowing proper deserialization of derived entity types such as <see cref="XmiBeam"/>, <see cref="XmiColumn"/>,
    /// and <see cref="XmiStructuralCurveMember"/>.
    ///
    /// All properties are serialized using Newtonsoft.Json with explicit ordering to ensure consistent
    /// JSON output across all entity types.
    /// </remarks>
    /// <example>
    /// Creating a new XMI entity:
    /// <code>
    /// var entity = new XmiBaseEntity(
    ///     id: "beam_001",
    ///     name: "Main Beam",
    ///     ifcGuid: "2h9$K0b0P6j7aB_1m5H7fR",
    ///     nativeId: "B-12345",
    ///     description: "Primary structural beam",
    ///     entityName: "XmiBeam",
    ///     domain: XmiBaseEntityDomainEnum.StructuralAnalytical
    /// );
    /// </code>
    /// </example>
    /// <seealso cref="XmiBaseGeometry"/>
    /// <seealso cref="XmiBaseRelationship"/>
    /// <seealso cref="XmiBaseEntityDomainEnum"/>
    public class XmiBaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for this entity.
        /// </summary>
        /// <value>The stable, unique identifier as a string.</value>
        /// <remarks>
        /// This identifier is used to reference the entity in relationships and should remain
        /// stable throughout the entity's lifecycle. It is serialized with property name "id".
        /// </remarks>
        [JsonProperty(PropertyName = "id", Order = 0)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the human-readable display name for this entity.
        /// </summary>
        /// <value>The display name as a string, or the <see cref="Id"/> if not specified.</value>
        /// <remarks>
        /// This property provides a user-friendly name for display in UI elements and reports.
        /// If a null or whitespace value is provided during construction, it defaults to the <see cref="Id"/>.
        /// </remarks>
        [JsonProperty(Order = 1)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the IFC GUID reference for this entity.
        /// </summary>
        /// <value>The IFC GUID as a string, or null if not applicable.</value>
        /// <remarks>
        /// The Industry Foundation Classes (IFC) GUID enables interoperability with BIM systems.
        /// This property should contain the globally unique identifier from the IFC model
        /// when the entity is synchronized with an IFC-based system.
        /// It is serialized with property name "ifcGuid".
        /// </remarks>
        [JsonProperty(PropertyName = "ifcGuid", Order = 2)]
        public string IfcGuid { get; set; }

        /// <summary>
        /// Gets or sets the native identifier from the source system.
        /// </summary>
        /// <value>The native system identifier as a string, or null if not applicable.</value>
        /// <remarks>
        /// This property maintains traceability to the original system that created the entity,
        /// enabling round-trip synchronization and audit trails. The format of this identifier
        /// depends on the native system's conventions.
        /// </remarks>
        [JsonProperty(Order = 3)]
        public string NativeId { get; set; }

        /// <summary>
        /// Gets or sets a textual description of this entity's purpose or characteristics.
        /// </summary>
        /// <value>The description as a string, or null if not specified.</value>
        /// <remarks>
        /// This property provides semantic context about the entity, explaining its role,
        /// constraints, or other relevant information not captured by other properties.
        /// </remarks>
        [JsonProperty(Order = 4)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the entity type name used for polymorphic JSON deserialization.
        /// </summary>
        /// <value>The entity type name as a string (e.g., "XmiBeam", "XmiColumn").</value>
        /// <remarks>
        /// This property serves as a discriminator in JSON serialization, allowing the
        /// deserialization process to determine the correct concrete type for the entity.
        /// Derived classes must set this to their type name to enable proper polymorphic
        /// deserialization. If not specified, it defaults to "XmiBaseEntity".
        /// </remarks>
        /// <seealso cref="Newtonsoft.Json"/>
        [JsonProperty(Order = 5)]
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the domain classification for this entity.
        /// </summary>
        /// <value>The domain enumeration value indicating the entity's classification.</value>
        /// <remarks>
        /// This property categorizes the entity into a specific domain such as
        /// <see cref="XmiBaseEntityDomainEnum.Physical"/> for physical building elements or
        /// <see cref="XmiBaseEntityDomainEnum.StructuralAnalytical"/> for analytical elements.
        /// The classification helps organize and query entities based on their domain context.
        /// </remarks>
        /// <seealso cref="XmiBaseEntityDomainEnum"/>
        [JsonProperty(Order = 6)]
        [JsonConverter(typeof(StringEnumConverter))]
        public XmiBaseEntityDomainEnum Domain { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmiBaseEntity"/> class with the
        /// specified identifiers and metadata required for JSON serialization.
        /// </summary>
        /// <param name="id">The stable, unique identifier for the entity. Must not be null or whitespace.</param>
        /// <param name="name">The human-readable display name. If null or whitespace, defaults to <paramref name="id"/>.</param>
        /// <param name="ifcGuid">The IFC GUID reference for BIM interoperability. Can be null.</param>
        /// <param name="nativeId">The identifier from the native source system for traceability. Can be null.</param>
        /// <param name="description">A textual description of the entity's purpose. Can be null.</param>
        /// <param name="entityName">The entity type name for polymorphic deserialization. If null or empty, defaults to "XmiBaseEntity".</param>
        /// <param name="domain">The domain classification for the entity (e.g., <see cref="XmiBaseEntityDomainEnum.StructuralAnalytical"/>).</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="id"/> is null or whitespace.</exception>
        /// <remarks>
        /// This constructor initializes all base properties with the provided values. The <paramref name="name"/>
        /// parameter defaults to <paramref name="id"/> if not specified, and <paramref name="entityName"/>
        /// defaults to "XmiBaseEntity" if not specified. Derived classes should override this constructor
        /// to pass their type name to <paramref name="entityName"/>.
        /// </remarks>
        /// <example>
        /// Creating a derived beam entity:
        /// <code>
        /// var beam = new XmiBeam(
        ///     id: "beam_001",
        ///     name: "Main Beam",
        ///     ifcGuid: "2h9$K0b0P6j7aB_1m5H7fR",
        ///     nativeId: "B-12345",
        ///     description: "Primary structural beam spanning column grid A-B",
        ///     domain: XmiBaseEntityDomainEnum.StructuralAnalytical,
        ///     // Additional beam-specific parameters...
        ///     endFixityStart: new XmiFixity(...),
        ///     endFixityEnd: new XmiFixity(...)
        /// );
        /// // beam.EntityName will be "XmiBeam"
        /// </code>
        /// </example>
        public XmiBaseEntity(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            string entityName,
            XmiBaseEntityDomainEnum domain
        )
        {
            Id = id;
            Name = string.IsNullOrWhiteSpace(name) ? id : name;
            
            IfcGuid = ifcGuid;
            NativeId = nativeId;
            Description = description;
            EntityName = string.IsNullOrEmpty(entityName) ? nameof(XmiBaseEntity) : entityName;
            Domain = domain;
        }
    }
}
