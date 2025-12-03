using System;
using XmiSchema.Core.Entities;

namespace XmiSchema.Core.Models.Entities.Physical;

/// <summary>
/// Represents a physical beam element in the built environment.
/// </summary>
public class XmiBeam : XmiBasePhysicalEntity, IEquatable<XmiBeam>
{
    /// <summary>
    /// Creates a new <see cref="XmiBeam"/> physical element.
    /// </summary>
    /// <param name="id">Unique identifier for the beam.</param>
    /// <param name="name">Friendly name for the beam.</param>
    /// <param name="ifcguid">Related IFC GUID.</param>
    /// <param name="nativeId">Authoring system identifier.</param>
    /// <param name="description">Context describing the beam.</param>
    public XmiBeam(
        string id,
        string name,
        string ifcguid,
        string nativeId,
        string description
    ) : base(id, name, ifcguid, nativeId, description, nameof(XmiBeam))
    {
    }

    public bool Equals(XmiBeam? other)
    {
        if (other == null) return false;
        return string.Equals(NativeId, other.NativeId, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as XmiBeam);

    public override int GetHashCode()
    {
        return NativeId?.ToLowerInvariant().GetHashCode() ?? 0;
    }
}
