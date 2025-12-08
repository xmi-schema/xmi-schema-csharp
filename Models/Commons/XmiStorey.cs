using System;
using XmiSchema.Models.Bases;
using XmiSchema.Models.Enums;

namespace XmiSchema.Models.Commons;

/// <summary>
/// Defines a generic storey level within the built environment, capturing elevation and mass metadata.
/// Can represent structural, architectural, or any defined building level.
/// </summary>
public class XmiStorey : XmiBaseEntity, IEquatable<XmiStorey>
{
    public double StoreyElevation { get; set; }
    public double StoreyMass { get; set; }

    /// <summary>
    /// Creates a new <see cref="XmiStorey"/> bound to a specific elevation.
    /// </summary>
    /// <param name="id">Unique identifier for the storey.</param>
    /// <param name="name">Friendly name (e.g., Level 3).</param>
    /// <param name="ifcGuid">Related IFC GUID.</param>
    /// <param name="nativeId">Authoring system identifier.</param>
    /// <param name="description">Context describing the storey.</param>
    /// <param name="storeyElevation">Elevation measured in the project units.</param>
    /// <param name="storeyMass">Total mass assigned to the level.</param>
    public XmiStorey(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        double storeyElevation,
        double storeyMass
    ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiStorey), XmiBaseEntityDomainEnum.Shared)
    {
        StoreyElevation = storeyElevation;
        StoreyMass = storeyMass;
    }

    public bool Equals(XmiStorey? other)
    {
        if (other == null) return false;
        return string.Equals(NativeId, other.NativeId, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as XmiStorey);

    public override int GetHashCode()
    {
        return NativeId?.ToLowerInvariant().GetHashCode() ?? 0;
    }
}
