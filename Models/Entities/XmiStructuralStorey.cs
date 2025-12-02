using System;

namespace XmiSchema.Core.Entities;

/// <summary>
/// Defines an analytical storey level within the built environment, capturing elevation, mass, and reaction metadata.
/// </summary>
public class XmiStructuralStorey : XmiBaseEntity, IEquatable<XmiStructuralStorey>
{
    public double StoreyElevation { get; set; }
    public double StoreyMass { get; set; }
    public string StoreyHorizontalReactionX { get; set; }
    public string StoreyHorizontalReactionY { get; set; }
    public string StoreyVerticalReaction { get; set; }

    /// <summary>
    /// Creates a new <see cref="XmiStructuralStorey"/> bound to a specific elevation.
    /// </summary>
    /// <param name="id">Unique identifier for the storey.</param>
    /// <param name="name">Friendly name (e.g., Level 3).</param>
    /// <param name="ifcguid">Related IFC GUID.</param>
    /// <param name="nativeId">Authoring system identifier.</param>
    /// <param name="description">Context describing the storey.</param>
    /// <param name="storeyElevation">Elevation measured in the project units.</param>
    /// <param name="storeyMass">Total mass assigned to the level.</param>
    /// <param name="storeyHorizontalReactionX">Serialized horizontal reaction constraint along X.</param>
    /// <param name="storeyHorizontalReactionY">Serialized horizontal reaction constraint along Y.</param>
    /// <param name="storeyVerticalReaction">Serialized vertical reaction value.</param>
    public XmiStructuralStorey(
        string id,
        string name,
        string ifcguid,
        string nativeId,
        string description,
        double storeyElevation,
        double storeyMass,
        string storeyHorizontalReactionX,
        string storeyHorizontalReactionY,
        string storeyVerticalReaction
    ) : base(id, name, ifcguid, nativeId, description, nameof(XmiStructuralStorey))
    {
        StoreyElevation = storeyElevation;
        StoreyMass = storeyMass;
        StoreyHorizontalReactionX = storeyHorizontalReactionX;
        StoreyHorizontalReactionY = storeyHorizontalReactionY;
        StoreyVerticalReaction = storeyVerticalReaction;
    }

    public bool Equals(XmiStructuralStorey? other)
    {
        if (other == null) return false;
        return string.Equals(NativeId, other.NativeId, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as XmiStructuralStorey);

    public override int GetHashCode()
    {
        return NativeId?.ToLowerInvariant().GetHashCode() ?? 0;
    }
}
