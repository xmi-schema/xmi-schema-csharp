using Newtonsoft.Json;

namespace XmiSchema.Core.Entities
{
    /// <summary>
    /// Provides the foundational metadata shared by every Cross Model Information entity.
    /// </summary>
    public class XmiBaseEntity
    {
        [JsonProperty(Order = 0)]
        public string ID { get; set; }

        [JsonProperty(Order = 1)]
        public string Name { get; set; }

        [JsonProperty(Order = 2)]
        public string IFCGUID { get; set; }

        [JsonProperty(Order = 3)]
        public string NativeId { get; set; }

        [JsonProperty(Order = 4)]
        public string Description { get; set; }

        [JsonProperty(Order = 5)]
        public string EntityType { get; set; }

        // 带参数构造函数
        /// <summary>
        /// Initializes the entity with the identifiers required for serialization.
        /// </summary>
        /// <param name="id">Stable identifier for the entity.</param>
        /// <param name="name">Display name; defaults to <paramref name="id"/>.</param>
        /// <param name="ifcguid">IFC GUID reference when available.</param>
        /// <param name="nativeId">Identifier from the native source system.</param>
        /// <param name="description">Describes the entity purpose.</param>
        /// <param name="entityType">Type hint emitted in payloads.</param>
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
}
