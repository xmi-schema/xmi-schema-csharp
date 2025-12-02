using XmiSchema.Core.Entities;

namespace XmiSchema.Core.Geometries;

/// <summary>
/// Abstract base class for all 3D geometry entities in the Cross Model Information schema.
/// </summary>
/// <remarks>
/// This class provides the foundation for all geometric primitives used in the built environment model.
/// Geometry entities represent the spatial form and position of structural elements.
///
/// All concrete geometry types (Point3D, Line3D, Arc3D) inherit from this class and can be
/// referenced by structural entities through the XmiHasGeometry relationship.
///
/// Geometries in the XMI schema are independent entities that can be shared across multiple
/// structural elements, enabling efficient representation of spatial data.
/// </remarks>
public abstract class XmiBaseGeometry : XmiBaseEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="XmiBaseGeometry"/> class.
    /// </summary>
    /// <param name="id">The unique identifier for the geometry entity. Must not be null or empty.</param>
    /// <param name="name">The display name for the geometry. If null or whitespace, defaults to <paramref name="id"/>.</param>
    /// <param name="ifcGuid">The IFC GUID for interoperability with IFC-based BIM systems.</param>
    /// <param name="nativeId">The original identifier from the source system that created this geometry.</param>
    /// <param name="description">A textual description of this geometry entity.</param>
    /// <remarks>
    /// This constructor automatically sets the EntityType to "XmiBaseGeometry".
    /// Derived classes will override this with their specific type name (e.g., "XmiPoint3D").
    /// </remarks>
    protected XmiBaseGeometry(string id, string name, string ifcGuid, string nativeId, string description)
        : base(id, name, ifcGuid, nativeId, description, nameof(XmiBaseGeometry))
    {
    }
}
