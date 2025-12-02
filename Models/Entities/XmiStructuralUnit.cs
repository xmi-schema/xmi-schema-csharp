using XmiSchema.Core.Enums;

namespace XmiSchema.Core.Entities;

/// <summary>
/// Defines the measurement unit assigned to a given entity attribute, enabling consistent conversion during data exchange.
/// </summary>
public class XmiStructuralUnit : XmiBaseEntity
{
    public string Entity { get; set; }
    public string Attribute { get; set; }
    public XmiUnitEnum Unit { get; set; }

    /// <summary>
    /// Creates a new <see cref="XmiStructuralUnit"/> mapping an attribute to a unit.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <param name="name">Readable name for the mapping.</param>
    /// <param name="ifcguid">Optional IFC GUID reference.</param>
    /// <param name="nativeId">Original identifier from the authoring tool.</param>
    /// <param name="description">Notes about the mapping.</param>
    /// <param name="entity">Name of the entity whose attribute uses this unit.</param>
    /// <param name="attribute">Attribute name.</param>
    /// <param name="unit">Enumeration describing the unit.</param>
    public XmiStructuralUnit(
        string id,
        string name,
        string ifcguid,
        string nativeId,
        string description,
        string entity,
        string attribute,
        XmiUnitEnum unit
    ) : base(id, name, ifcguid, nativeId, description, nameof(XmiStructuralUnit))
    {
        Entity = entity;
        Attribute = attribute;
        Unit = unit;
    }
}
