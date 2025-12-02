using XmiSchema.Core.Entities;

namespace XmiSchema.Core.Relationships;

/// <summary>
/// Links entities to the structural material definition applied to them.
/// </summary>
public class XmiHasStructuralMaterial : XmiBaseRelationship
{
    /// <summary>
    /// Creates a fully detailed material relationship.
    /// </summary>
    /// <param name="id">Unique relationship identifier.</param>
    /// <param name="source">Entity consuming the material.</param>
    /// <param name="target">Material entity.</param>
    /// <param name="name">Relationship label.</param>
    /// <param name="description">Notes describing the binding.</param>
    /// <param name="entityName">Serialized entity name.</param>
    /// <param name="umlType">UML stereotype.</param>
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

    /// <summary>
    /// Creates a simple material relationship with an auto identifier.
    /// </summary>
    /// <param name="source">Entity consuming the material.</param>
    /// <param name="target">Material entity.</param>
    public XmiHasStructuralMaterial(
        XmiBaseEntity source,
        XmiBaseEntity target
    ) : base(source, target, nameof(XmiHasStructuralMaterial), "Association")
    {
    }
}
