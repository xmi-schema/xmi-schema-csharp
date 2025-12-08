using System;
using XmiSchema.Entities.Bases;
using XmiSchema.Entities.Commons;

namespace XmiSchema.Entities.Physical;

/// <summary>
/// Represents a physical wall element in the built environment.
/// </summary>
public class XmiWall : XmiBasePhysicalEntity, IEquatable<XmiWall>
{
    public double ZOffset { get; set; }
    public XmiAxis LocalAxisX { get; set; }
    public XmiAxis LocalAxisY { get; set; }
    public XmiAxis LocalAxisZ { get; set; }
    public double Height { get; set; }

    /// <summary>
    /// Creates a new <see cref="XmiWall"/> physical element.
    /// </summary>
    /// <param name="id">Unique identifier for the wall.</param>
    /// <param name="name">Friendly name for the wall.</param>
    /// <param name="ifcGuid">Related IFC GUID.</param>
    /// <param name="nativeId">Authoring system identifier.</param>
    /// <param name="description">Context describing the wall.</param>
    /// <param name="zOffset">Offset along the Z axis relative to its host.</param>
    /// <param name="localAxisX">Unit direction of local X.</param>
    /// <param name="localAxisY">Unit direction of local Y.</param>
    /// <param name="localAxisZ">Unit direction of local Z.</param>
    /// <param name="height">Physical height of the wall.</param>
    public XmiWall(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        double zOffset,
        XmiAxis localAxisX,
        XmiAxis localAxisY,
        XmiAxis localAxisZ,
        double height
    ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiWall))
    {
        ZOffset = zOffset;
        LocalAxisX = localAxisX;
        LocalAxisY = localAxisY;
        LocalAxisZ = localAxisZ;
        Height = height;
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
