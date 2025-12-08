using System;
using XmiSchema.Entities.Bases;

namespace XmiSchema.Entities.Physical;

/// <summary>
/// Represents a physical slab element in the built environment.
/// </summary>
public class XmiSlab : XmiBasePhysicalEntity, IEquatable<XmiSlab>
{
    /// <summary>
    /// Creates a new <see cref="XmiSlab"/> physical element.
    /// </summary>
    /// <param name="id">Unique identifier for the slab.</param>
    /// <param name="name">Friendly name for the slab.</param>
    /// <param name="ifcGuid">Related IFC GUID.</param>
    /// <param name="nativeId">Authoring system identifier.</param>
    /// <param name="description">Context describing the slab.</param>
    public XmiSlab(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description
    ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiSlab))
    {
    }

    public bool Equals(XmiSlab? other)
    {
        if (other == null) return false;
        return string.Equals(NativeId, other.NativeId, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as XmiSlab);

    public override int GetHashCode()
    {
        return NativeId?.ToLowerInvariant().GetHashCode() ?? 0;
    }
}
