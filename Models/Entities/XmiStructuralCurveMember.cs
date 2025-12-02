using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using XmiSchema.Core.Enums;

namespace XmiSchema.Core.Entities;

/// <summary>
/// Represents a linear structural element (beam, column, brace) in the XMI graph, storing alignment data and offsets.
/// </summary>
public class XmiStructuralCurveMember : XmiBaseEntity, IEquatable<XmiStructuralCurveMember>
{
    // public XmiStructuralCrossSection CrossSection { get; set; }
    // public XmiStructuralStorey Storey { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public XmiStructuralCurveMemberTypeEnum CurvememberType { get; set; }
    // public List<XmiStructuralPointConnection> Nodes { get; set; }
    // public List<XmiSegment> Segments { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public XmiStructuralCurveMemberSystemLineEnum SystemLine { get; set; }
    // public XmiStructuralPointConnection BeginNode { get; set; }
    // public XmiStructuralPointConnection EndNode { get; set; }
    public double Length { get; set; }


    public string LocalAxisX { get; set; }
    public string LocalAxisY { get; set; }
    public string LocalAxisZ { get; set; }

    public double BeginNodeYOffset { get; set; }
    public double EndNodeYOffset { get; set; }
    public double BeginNodeZOffset { get; set; }
    public double EndNodeZOffset { get; set; }
    public double BeginNodeXOffset { get; set; }
    public double EndNodeXOffset { get; set; }


    public string EndFixityStart { get; set; }
    public string EndFixityEnd { get; set; }




    /// <summary>
    /// Configures a new <see cref="XmiStructuralCurveMember"/> with system line metadata and local axis offsets.
    /// </summary>
    /// <param name="id">Unique identifier for the member entity.</param>
    /// <param name="name">Friendly name exposed to client applications.</param>
    /// <param name="ifcguid">IFC GUID reference for traceability.</param>
    /// <param name="nativeId">Identifier from the authoring tool.</param>
    /// <param name="description">Optional descriptive text.</param>
    /// <param name="curvememberType">Member type classification (beam, column, etc.).</param>
    /// <param name="systemLine">Relative position of the analytical line inside the physical profile.</param>
    /// <param name="length">Analytical length of the element.</param>
    /// <param name="localAxisX">Serialized orientation of local X.</param>
    /// <param name="localAxisY">Serialized orientation of local Y.</param>
    /// <param name="localAxisZ">Serialized orientation of local Z.</param>
    /// <param name="beginNodeXOffset">X offset applied to the start node.</param>
    /// <param name="endNodeXOffset">X offset applied to the end node.</param>
    /// <param name="beginNodeYOffset">Y offset applied to the start node.</param>
    /// <param name="endNodeYOffset">Y offset applied to the end node.</param>
    /// <param name="beginNodeZOffset">Z offset applied to the start node.</param>
    /// <param name="endNodeZOffset">Z offset applied to the end node.</param>
    /// <param name="endFixityStart">Boundary condition definition at the start.</param>
    /// <param name="endFixityEnd">Boundary condition definition at the end.</param>
    public XmiStructuralCurveMember(
        string id,
        string name,
        string ifcguid,
        string nativeId,
        string description,
        // XmiStructuralCrossSection crossSection,
        // XmiStructuralStorey storey,
        XmiStructuralCurveMemberTypeEnum curvememberType,
        // List<XmiStructuralPointConnection> nodes,
        // List<XmiSegment> segments,
        XmiStructuralCurveMemberSystemLineEnum systemLine,
        // XmiStructuralPointConnection beginNode,
        // XmiStructuralPointConnection endNode,
        double length,

        string localAxisX,
        string localAxisY,
        string localAxisZ,
        double beginNodeXOffset,
        double endNodeXOffset,
        double beginNodeYOffset,
        double endNodeYOffset,
        double beginNodeZOffset,
        double endNodeZOffset,


        string endFixityStart,
        string endFixityEnd

    ) : base(id, name, ifcguid, nativeId, description, nameof(XmiStructuralCurveMember))
    {
        // CrossSection = crossSection;
        // Storey = storey;
        CurvememberType = curvememberType;
        // Nodes = nodes;
        // Segments = segments;
        SystemLine = systemLine;
        // BeginNode = beginNode;
        // EndNode = endNode;
        Length = length;
        LocalAxisX = localAxisX;
        LocalAxisY = localAxisY;
        LocalAxisZ = localAxisZ;
        BeginNodeYOffset = beginNodeYOffset;
        EndNodeYOffset = endNodeYOffset;
        BeginNodeZOffset = beginNodeZOffset;
        EndNodeZOffset = endNodeZOffset;
        BeginNodeXOffset = beginNodeXOffset;
        EndNodeXOffset = endNodeXOffset;
        EndFixityStart = endFixityStart;
        EndFixityEnd = endFixityEnd;

    }

    public bool Equals(XmiStructuralCurveMember? other)
    {
        if (other == null) return false;
        return string.Equals(NativeId, other.NativeId, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as XmiStructuralCurveMember);

    public override int GetHashCode()
    {
        return NativeId?.ToLowerInvariant().GetHashCode() ?? 0;
    }
}


