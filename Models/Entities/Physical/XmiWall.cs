using System;
using XmiSchema.Core.Entities;

namespace XmiSchema.Core.Models.Entities.Physical;

/// <summary>
/// Represents a physical wall element in the built environment.
/// </summary>
public class XmiWall : XmiBasePhysicalEntity, IEquatable<XmiWall>
{
    /// <summary>
    /// Creates a new <see cref="XmiWall"/> physical element.
    /// </summary>
    /// <param name="id">Unique identifier for the wall.</param>
    /// <param name="name">Friendly name for the wall.</param>
    /// <param name="ifcGuid">Related IFC GUID.</param>
    /// <param name="nativeId">Authoring system identifier.</param>
    /// <param name="description">Context describing the wall.</param>
    public XmiWall(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description
    ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiWall))
    {
    }

    public bool Equals(XmiWall? other)
    {
        if (other == null) return false;
        return string.Equals(NativeId, other.NativeId, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as XmiWall);

    public override int GetHashCode()
    {
        return NativeId?.ToLowerInvariant().GetHashCode() ?? 0;
    }
}
