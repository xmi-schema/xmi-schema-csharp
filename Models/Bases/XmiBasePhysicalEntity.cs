using XmiSchema.Models.Enums;

namespace XmiSchema.Models.Bases
{
    /// <summary>
    /// Base class for all physical domain entities in the Cross Model Information schema.
    /// </summary>
    public abstract class XmiBasePhysicalEntity : XmiBaseEntity
    {
        /// <summary>
        /// Initializes a new physical entity with the required metadata.
        /// </summary>
        /// <param name="id">Stable identifier for the entity.</param>
        /// <param name="name">Display name; defaults to <paramref name="id"/>.</param>
        /// <param name="ifcGuid">IFC GUID reference when available.</param>
        /// <param name="nativeId">Identifier from the native source system.</param>
        /// <param name="description">Describes the entity purpose.</param>
        /// <param name="entityType">Type hint emitted in payloads.</param>
        protected XmiBasePhysicalEntity(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            string entityType
        ) : base(id, name, ifcGuid, nativeId, description, entityType, XmiBaseEntityDomainEnum.Physical)
        {
        }
    }
}
