using XmiSchema.Enums;

namespace XmiSchema.Entities.Bases
{
    /// <summary>
    /// Base class for all structural analytical domain entities in the Cross Model Information schema.
    /// </summary>
    public abstract class XmiBaseStructuralAnalyticalEntity : XmiBaseEntity
    {
        /// <summary>
        /// Initializes a new structural analytical entity with the required metadata.
        /// </summary>
        /// <param name="id">Stable identifier for the entity.</param>
        /// <param name="name">Display name; defaults to <paramref name="id"/>.</param>
        /// <param name="ifcGuid">IFC GUID reference when available.</param>
        /// <param name="nativeId">Identifier from the native source system.</param>
        /// <param name="description">Describes the entity purpose.</param>
        /// <param name="entityName">Entity name emitted in payloads.</param>
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
