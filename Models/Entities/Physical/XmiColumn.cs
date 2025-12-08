using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using XmiSchema.Models.Bases;
using XmiSchema.Models.Enums;

namespace XmiSchema.Models.Entities.Physical;

/// <summary>
/// Represents a physical column element in the built environment.
/// </summary>
public class XmiColumn : XmiBasePhysicalEntity, IEquatable<XmiColumn>
{
    [JsonConverter(typeof(StringEnumConverter))]
    public XmiSystemLineEnum SystemLine { get; set; }

    public double Length { get; set; }

    public string LocalAxisX { get; set; }
    public string LocalAxisY { get; set; }
    public string LocalAxisZ { get; set; }

    public double BeginNodeXOffset { get; set; }
    public double EndNodeXOffset { get; set; }
    public double BeginNodeYOffset { get; set; }
    public double EndNodeYOffset { get; set; }
    public double BeginNodeZOffset { get; set; }
    public double EndNodeZOffset { get; set; }

    /// <summary>
    /// Creates a new <see cref="XmiColumn"/> physical element.
    /// </summary>
    /// <param name="id">Unique identifier for the column.</param>
    /// <param name="name">Friendly name for the column.</param>
    /// <param name="ifcGuid">Related IFC GUID.</param>
    /// <param name="nativeId">Authoring system identifier.</param>
    /// <param name="description">Context describing the column.</param>
    /// <param name="systemLine">Relative position of the analytical line inside the physical profile.</param>
    /// <param name="length">Physical length of the column.</param>
    /// <param name="localAxisX">Serialized orientation of local X.</param>
    /// <param name="localAxisY">Serialized orientation of local Y.</param>
    /// <param name="localAxisZ">Serialized orientation of local Z.</param>
    /// <param name="beginNodeXOffset">X offset applied to the start node.</param>
    /// <param name="endNodeXOffset">X offset applied to the end node.</param>
    /// <param name="beginNodeYOffset">Y offset applied to the start node.</param>
    /// <param name="endNodeYOffset">Y offset applied to the end node.</param>
    /// <param name="beginNodeZOffset">Z offset applied to the start node.</param>
    /// <param name="endNodeZOffset">Z offset applied to the end node.</param>
    public XmiColumn(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        XmiSystemLineEnum systemLine,
        double length,
        string localAxisX,
        string localAxisY,
        string localAxisZ,
        double beginNodeXOffset,
        double endNodeXOffset,
        double beginNodeYOffset,
        double endNodeYOffset,
        double beginNodeZOffset,
        double endNodeZOffset
    ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiColumn))
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
