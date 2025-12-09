using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using XmiSchema.Enums;

namespace XmiSchema.Entities.Bases
{
    /// <summary>
    /// Provides the foundational metadata shared by every Cross Model Information entity.
    /// </summary>
    public class XmiBaseEntity
    {
        [JsonProperty(PropertyName = "id", Order = 0)]
        public string Id { get; set; }

        [JsonProperty(Order = 1)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "ifcGuid", Order = 2)]
        public string IfcGuid { get; set; }

        [JsonProperty(Order = 3)]
        public string NativeId { get; set; }

        [JsonProperty(Order = 4)]
        public string Description { get; set; }

        [JsonProperty(Order = 5)]
        public string EntityName { get; set; }

        [JsonProperty(Order = 6)]
        [JsonConverter(typeof(StringEnumConverter))]
        public XmiBaseEntityDomainEnum Domain { get; set; }

        // 带参数构造函数
        /// <summary>
        /// Initializes the entity with the identifiers required for serialization.
        /// </summary>
        /// <param name="id">Stable identifier for the entity.</param>
        /// <param name="name">Display name; defaults to <paramref name="id"/>.</param>
        /// <param name="ifcGuid">IFC GUID reference when available.</param>
        /// <param name="nativeId">Identifier from the native source system.</param>
        /// <param name="description">Describes the entity purpose.</param>
        /// <param name="entityName">Domain hint emitted in payloads.</param>
        /// <param name="domain">Domain classification for the entity.</param>
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
