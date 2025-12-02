using XmiSchema.Core.Entities;

namespace XmiSchema.Core.Relationships;
public class XmiHasStructuralNode : XmiBaseRelationship
{
    public XmiHasStructuralNode(
        string id,
        XmiBaseEntity source,
        XmiBaseEntity target,
        string name,
        string description,
        string entityName,
        string umlType
    ) : base(id, source, target, name, description, nameof(XmiHasStructuralNode), "Association")
    {
    }

    public XmiHasStructuralNode(
        XmiBaseEntity source,
        XmiBaseEntity target
    ) : base(source, target, nameof(XmiHasStructuralNode), "Association")
    {
    }
}
