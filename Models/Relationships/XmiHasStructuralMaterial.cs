using XmiSchema.Core.Entities;

namespace XmiSchema.Core.Relationships;

public class XmiHasStructuralMaterial : XmiBaseRelationship
{
    public XmiHasStructuralMaterial(
        string id,
        XmiBaseEntity source,
        XmiBaseEntity target,
        string name,
        string description,
        string entityName,
        string umlType
    ) : base(id, source, target, name, description, nameof(XmiHasStructuralMaterial), "Association")
    {
    }

    public XmiHasStructuralMaterial(
        XmiBaseEntity source,
        XmiBaseEntity target
    ) : base(source, target, nameof(XmiHasStructuralMaterial), "Association")
    {
    }
}
