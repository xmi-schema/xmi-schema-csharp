using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using XmiSchema.Entities.Bases;
using XmiSchema.Entities.Commons;
using XmiSchema.Enums;

namespace XmiSchema.Entities.Physical;

/// <summary>
/// Represents a physical beam element in the built environment.
/// </summary>
public class XmiBeam : XmiBasePhysicalEntity, IEquatable<XmiBeam>
{
    [JsonConverter(typeof(StringEnumConverter))]
    public XmiSystemLineEnum SystemLine { get; set; }

    public double Length { get; set; }

    public XmiAxis LocalAxisX { get; set; }
    public XmiAxis LocalAxisY { get; set; }
    public XmiAxis LocalAxisZ { get; set; }

    public double BeginNodeXOffset { get; set; }
    public double EndNodeXOffset { get; set; }
    public double BeginNodeYOffset { get; set; }
    public double EndNodeYOffset { get; set; }
    public double BeginNodeZOffset { get; set; }
    public double EndNodeZOffset { get; set; }

    /// <summary>
    /// Creates a new <see cref="XmiBeam"/> physical element.
    /// </summary>
    /// <param name="id">Unique identifier for the beam.</param>
    /// <param name="name">Friendly name for the beam.</param>
    /// <param name="ifcGuid">Related IFC GUID.</param>
    /// <param name="nativeId">Authoring system identifier.</param>
    /// <param name="description">Context describing the beam.</param>
    /// <param name="systemLine">Relative position of the analytical line inside the physical profile.</param>
    /// <param name="length">Physical length of the beam.</param>
    /// <param name="localAxisX">Unit direction of local X.</param>
    /// <param name="localAxisY">Unit direction of local Y.</param>
    /// <param name="localAxisZ">Unit direction of local Z.</param>
    /// <param name="beginNodeXOffset">X offset applied to the start node.</param>
    /// <param name="endNodeXOffset">X offset applied to the end node.</param>
    /// <param name="beginNodeYOffset">Y offset applied to the start node.</param>
    /// <param name="endNodeYOffset">Y offset applied to the end node.</param>
    /// <param name="beginNodeZOffset">Z offset applied to the start node.</param>
    /// <param name="endNodeZOffset">Z offset applied to the end node.</param>
    public XmiBeam(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        XmiSystemLineEnum systemLine,
        double length,
        XmiAxis localAxisX,
        XmiAxis localAxisY,
        XmiAxis localAxisZ,
        double beginNodeXOffset,
        double endNodeXOffset,
        double beginNodeYOffset,
        double endNodeYOffset,
        double beginNodeZOffset,
        double endNodeZOffset
    ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiBeam))
    {
        SystemLine = systemLine;
        Length = length;
        LocalAxisX = localAxisX;
        LocalAxisY = localAxisY;
        LocalAxisZ = localAxisZ;
        BeginNodeXOffset = beginNodeXOffset;
        EndNodeXOffset = endNodeXOffset;
        BeginNodeYOffset = beginNodeYOffset;
        EndNodeYOffset = endNodeYOffset;
        BeginNodeZOffset = beginNodeZOffset;
        EndNodeZOffset = endNodeZOffset;
    }

    /// <summary>
    /// Determines equality based on the native identifier.
    /// </summary>
    /// <param name="other">Another <see cref="XmiBeam"/> instance to compare.</param>
    /// <returns>
    /// <c>true</c> if the native identifiers match (case-insensitive); otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Beam equality is based on <see cref="NativeId"/> for consistency with other XmiSchema entities.
    /// This allows identifying duplicate beams from different data sources while ignoring case differences.
    /// </remarks>
    public bool Equals(XmiBeam? other)
    {
        if (other == null) return false;
        return string.Equals(NativeId, other.NativeId, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Determines equality based on the native identifier.
    /// </summary>
    /// <param name="obj">Another object to compare.</param>
    /// <returns>
    /// <c>true</c> if this beam equals the compared object; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method overrides <see cref="Equals(XmiBeam?)"/> to handle null comparison.
    /// </remarks>
    public override bool Equals(object? obj) => Equals(obj as XmiBeam);

    /// <summary>
    /// Returns a hash code based on the native identifier.
    /// </summary>
    /// <returns>
    /// A hash code derived from the lowercase native identifier.
    /// </returns>
    /// <remarks>
    /// The hash code is consistent with <see cref="Equals(XmiBeam?)"/> behavior and ensures
    /// that beams with the same native ID are considered equal for collection operations.
    /// The <c>0</c> fallback ensures non-null references return a valid hash code.
    /// </remarks>
    public override int GetHashCode()
    {
        return NativeId?.ToLowerInvariant().GetHashCode() ?? 0;
    }
    {
        SystemLine = systemLine;
        Length = length;
        LocalAxisX = localAxisX;
        LocalAxisY = localAxisY;
        LocalAxisZ = localAxisZ;
        BeginNodeXOffset = beginNodeXOffset;
        EndNodeXOffset = endNodeXOffset;
        BeginNodeYOffset = beginNodeYOffset;
        EndNodeYOffset = endNodeYOffset;
        BeginNodeZOffset = beginNodeZOffset;
        EndNodeZOffset = endNodeZOffset;
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
