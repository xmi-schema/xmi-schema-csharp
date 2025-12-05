using XmiSchema.Core.Enums;

namespace XmiSchema.Core.Entities;

/// <summary>
/// Defines the measurement unit assigned to a given entity attribute, enabling consistent conversion during data exchange.
/// </summary>
public class XmiUnit : XmiBaseEntity
{
    public string Entity { get; set; }
    public string Attribute { get; set; }
    public XmiUnitEnum Unit { get; set; }

    /// <summary>
    /// Creates a new <see cref="XmiUnit"/> mapping an attribute to a unit.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <param name="name">Readable name for the mapping.</param>
    /// <param name="ifcGuid">Optional IFC GUID reference.</param>
    /// <param name="nativeId">Original identifier from the authoring tool.</param>
    /// <param name="description">Notes about the mapping.</param>
    /// <param name="entity">Name of the entity whose attribute uses this unit.</param>
    /// <param name="attribute">Attribute name.</param>
    /// <param name="unit">Enumeration describing the unit.</param>
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
