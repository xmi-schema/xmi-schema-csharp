using XmiSchema.Core.Entities;


namespace XmiSchema.Core.Geometries;

/// <summary>
/// Base type for all geometric entities shared by the schema.
/// </summary>
public abstract class XmiBaseGeometry : XmiBaseEntity
{
    /// <summary>
    /// Initializes the geometry metadata.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <param name="name">Display name.</param>
    /// <param name="ifcGuid">IFC GUID reference.</param>
    /// <param name="nativeId">Native identifier from the authoring tool.</param>
    /// <param name="description">Describes the geometry.</param>
    /// <param name="entityType">Optional entity type override.</param>
    public XmiBaseGeometry(
        string id,
        string name,
        string ifcGuid,
        string nativeId, 
        string description,
        string? entityType = null)
        : base(id, name, ifcGuid, nativeId, description,
               string.IsNullOrEmpty(entityType) ? nameof(XmiBaseGeometry) : entityType)
    {
    }
}
