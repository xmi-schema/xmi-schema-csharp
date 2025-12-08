using System;
using XmiSchema.Entities.Bases;
using XmiSchema.Entities.Commons;

namespace XmiSchema.Entities.Physical;

/// <summary>
/// Represents a physical slab element in the built environment.
/// </summary>
public class XmiSlab : XmiBasePhysicalEntity, IEquatable<XmiSlab>
{
    public double ZOffset { get; set; }
    public XmiAxis LocalAxisX { get; set; }
    public XmiAxis LocalAxisY { get; set; }
    public XmiAxis LocalAxisZ { get; set; }
    public double Thickness { get; set; }

    /// <summary>
    /// Creates a new <see cref="XmiSlab"/> physical element.
    /// </summary>
    /// <param name="id">Unique identifier for the slab.</param>
    /// <param name="name">Friendly name for the slab.</param>
    /// <param name="ifcGuid">Related IFC GUID.</param>
    /// <param name="nativeId">Authoring system identifier.</param>
    /// <param name="description">Context describing the slab.</param>
    /// <param name="zOffset">Offset along the Z axis relative to its host.</param>
    /// <param name="localAxisX">Unit direction of local X.</param>
    /// <param name="localAxisY">Unit direction of local Y.</param>
    /// <param name="localAxisZ">Unit direction of local Z.</param>
    /// <param name="thickness">Physical thickness of the slab.</param>
    public XmiSlab(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        double zOffset,
        XmiAxis localAxisX,
        XmiAxis localAxisY,
        XmiAxis localAxisZ,
        double thickness
    ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiSlab))
    {
        ZOffset = zOffset;
        LocalAxisX = localAxisX;
        LocalAxisY = localAxisY;
        LocalAxisZ = localAxisZ;
        Thickness = thickness;
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
