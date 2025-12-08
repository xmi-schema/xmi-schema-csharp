using System;
using XmiSchema.Models.Bases;
using XmiSchema.Models.Enums;

namespace XmiSchema.Models.Entities.StructuralAnalytical;

/// <summary>
/// Models plates, slabs, and other surface members in the XMI graph with system plane and local axes metadata.
/// </summary>
public class XmiStructuralSurfaceMember : XmiBaseStructuralAnalyticalEntity, IEquatable<XmiStructuralSurfaceMember>
{
    // public XmiMaterial Material { get; set; }
    public XmiStructuralSurfaceMemberTypeEnum SurfaceMemberType { get; set; }
    public double Thickness { get; set; }
    public XmiStructuralSurfaceMemberSystemPlaneEnum SystemPlane { get; set; }
    // public List<XmiStructuralPointConnection> Nodes { get; set; }
    // public XmiStorey Storey { get; set; }

    // public List<XmiSegment> Segments { get; set; }
    public double Area { get; set; }
    public double ZOffset { get; set; }
    public string LocalAxisX { get; set; }
    public string LocalAxisY { get; set; }
    public string LocalAxisZ { get; set; }
    public double Height { get; set; }

    /// <summary>
    /// Initializes a new <see cref="XmiStructuralSurfaceMember"/> including analytical orientation.
    /// </summary>
    /// <param name="id">Unique XMI identifier.</param>
    /// <param name="name">Human readable title.</param>
    /// <param name="ifcGuid">Optional IFC GUID.</param>
    /// <param name="nativeId">Native authoring-system identifier.</param>
    /// <param name="description">Description of the surface.</param>
    /// <param name="surfaceMemberType">Surface type classification.</param>
    /// <param name="thickness">Member thickness.</param>
    /// <param name="systemPlane">Plane orientation relative to the model.</param>
    /// <param name="area">Planar area.</param>
    /// <param name="zOffset">Offset along the Z axis.</param>
    /// <param name="localAxisX">Serialized local X axis orientation.</param>
    /// <param name="localAxisY">Serialized local Y axis orientation.</param>
    /// <param name="localAxisZ">Serialized local Z axis orientation.</param>
    /// <param name="height">Total extrusion height.</param>
    public XmiStructuralSurfaceMember(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        // XmiMaterial material,
        XmiStructuralSurfaceMemberTypeEnum surfaceMemberType,
        double thickness,
        XmiStructuralSurfaceMemberSystemPlaneEnum systemPlane,
        // List<XmiStructuralPointConnection> nodes,
        // XmiStorey storey,
        // List<XmiSegment> segments,
        double area,
        double zOffset,
        string localAxisX,
        string localAxisY,
        string localAxisZ,
        double height
    ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiStructuralSurfaceMember))
    {
        // Material = material;
        SurfaceMemberType = surfaceMemberType;
        Thickness = thickness;
        SystemPlane = systemPlane;
        // Nodes = nodes;
        // Storey = storey;
        // Segments = segments;
        Area = area;
        ZOffset = zOffset;
        LocalAxisX = localAxisX;
        LocalAxisY = localAxisY;
        LocalAxisZ = localAxisZ;
        Height = height;
    }

    public bool Equals(XmiStructuralSurfaceMember? other)
    {
        if (other == null) return false;
        return string.Equals(NativeId, other.NativeId, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as XmiStructuralSurfaceMember);

    public override int GetHashCode()
    {
        return NativeId?.ToLowerInvariant().GetHashCode() ?? 0;
    }
}

