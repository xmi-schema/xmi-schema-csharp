using System;
using XmiSchema.Core.Entities;

namespace XmiSchema.Core.Models.Entities.Physical;

/// <summary>
/// Represents a physical column element in the built environment.
/// </summary>
public class XmiColumn : XmiBasePhysicalEntity, IEquatable<XmiColumn>
{
    /// <summary>
    /// Creates a new <see cref="XmiColumn"/> physical element.
    /// </summary>
    /// <param name="id">Unique identifier for the column.</param>
    /// <param name="name">Friendly name for the column.</param>
    /// <param name="ifcguid">Related IFC GUID.</param>
    /// <param name="nativeId">Authoring system identifier.</param>
    /// <param name="description">Context describing the column.</param>
    public XmiColumn(
        string id,
        string name,
        string ifcguid,
        string nativeId,
        string description
    ) : base(id, name, ifcguid, nativeId, description, nameof(XmiColumn))
    {
    }

    public bool Equals(XmiColumn? other)
    {
        if (other == null) return false;
        return string.Equals(NativeId, other.NativeId, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as XmiColumn);

    public override int GetHashCode()
    {
        return NativeId?.ToLowerInvariant().GetHashCode() ?? 0;
    }
}
