using System;

namespace XmiSchema.Core.Entities;

public class XmiStructuralStorey : XmiBaseEntity, IEquatable<XmiStructuralStorey>
{
    public double StoreyElevation { get; set; }
    public double StoreyMass { get; set; }
    public string StoreyHorizontalReactionX { get; set; }
    public string StoreyHorizontalReactionY { get; set; }
    public string StoreyVerticalReaction { get; set; }

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
