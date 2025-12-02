namespace XmiSchema.Core.Entities;

/// <summary>
/// Base class for all entities in the XMI schema model.
/// Provides common properties and functionality for structural engineering entities.
/// </summary>
/// <remarks>
/// This class serves as the foundation for all entity types in the built environment data model.
/// It includes essential identifiers and metadata that are common across all entity types.
/// All concrete entities should inherit from this class to ensure consistency in the data model.
/// </remarks>
public class XmiBaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for this entity.
    /// </summary>
    /// <remarks>
    /// This is the primary identifier used throughout the system to reference this entity.
    /// It must be unique within the model and is used for establishing relationships.
    /// </remarks>
    public string ID { get; set; }

    /// <summary>
    /// Gets or sets the display name of this entity.
    /// </summary>
    /// <remarks>
    /// If not provided, defaults to the ID value.
    /// This is a human-readable name for the entity.
    /// </remarks>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the IFC (Industry Foundation Classes) GUID for this entity.
    /// </summary>
    /// <remarks>
    /// Used for interoperability with IFC-based systems and BIM applications.
    /// Can be empty if the entity is not mapped to an IFC element.
    /// </remarks>
    public string IFCGUID { get; set; }

    /// <summary>
    /// Gets or sets the native identifier from the source system.
    /// </summary>
    /// <remarks>
    /// This preserves the original identifier from the system where the entity was created,
    /// enabling traceability back to the source data.
    /// </remarks>
    public string NativeId { get; set; }

    /// <summary>
    /// Gets or sets a textual description of this entity.
    /// </summary>
    /// <remarks>
    /// Provides additional context and information about the entity beyond its name.
    /// </remarks>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the type discriminator for this entity.
    /// </summary>
    /// <remarks>
    /// Used for type identification during serialization and deserialization.
    /// Defaults to the class name if not explicitly provided.
    /// </remarks>
    public string EntityType { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmiBaseEntity"/> class.
    /// </summary>
    /// <param name="id">The unique identifier for the entity. Must not be null or empty.</param>
    /// <param name="name">The display name for the entity. If null or whitespace, defaults to <paramref name="id"/>.</param>
    /// <param name="ifcguid">The IFC GUID for interoperability with IFC systems.</param>
    /// <param name="nativeId">The original identifier from the source system.</param>
    /// <param name="description">A textual description of the entity.</param>
    /// <param name="entityType">The type discriminator. If null or empty, defaults to "XmiBaseEntity".</param>
    public XmiBaseEntity(
        string id,
        string name,
        string ifcguid,
        string nativeId,
        string description,
        string entityType
    )
    {
        ID = id;
        Name = string.IsNullOrWhiteSpace(name) ? id : name;
        IFCGUID = ifcguid;
        NativeId = nativeId;
        Description = description;
        EntityType = string.IsNullOrEmpty(entityType) ? nameof(XmiBaseEntity) : entityType;
    }
}
