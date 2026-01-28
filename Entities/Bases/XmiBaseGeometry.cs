using XmiSchema.Enums;

namespace XmiSchema.Entities.Bases;

/// <summary>
/// Abstract base class for all geometric entities in the XMI schema.
/// </summary>
/// <remarks>
/// This class extends <see cref="XmiBaseEntity"/> to provide a common foundation for
/// geometric primitives that define the spatial characteristics of building elements.
/// Derived classes represent specific geometric types:
///
/// <list type="bullet">
/// <item><description><see cref="XmiPoint3D"/> - A point in 3D space</description></item>
/// <item><description><see cref="XmiLine3D"/> - A straight line between two points</description></item>
/// <item><description><see cref="XmiArc3D"/> - A circular arc with center, radius, and angle</description></item>
/// </list>
///
/// Geometric entities are used by physical and analytical elements to define their
/// shape, position, and orientation in 3D space. They can be shared across multiple
/// entities to reduce duplication and ensure consistency.
///
/// All geometric entities have their <see cref="XmiBaseEntity.Domain"/> property set to
/// <see cref="XmiBaseEntityDomainEnum.Geometry"/> for proper classification and querying.
/// </remarks>
/// <example>
/// Creating a point geometry:
/// <code>
/// var point = new XmiPoint3D(
///     id: "point_001",
///     name: "Column Base Point",
///     ifcGuid: "1a2b3c4d5e6f",
///     nativeId: "P-001",
///     description: "Base point for column C-1",
///     x: 0.0,
///     y: 0.0,
///     z: 0.0
/// );
/// // Inherits from XmiBaseGeometry
/// // point.Domain == XmiBaseEntityDomainEnum.Geometry
/// </code>
/// </example>
/// <seealso cref="XmiPoint3D"/>
/// <seealso cref="XmiLine3D"/>
/// <seealso cref="XmiArc3D"/>
/// <seealso cref="XmiBaseEntity"/>
/// <seealso cref="XmiBaseEntityDomainEnum"/>
public abstract class XmiBaseGeometry : XmiBaseEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="XmiBaseGeometry"/> class with the
    /// specified identifiers and metadata.
    /// </summary>
    /// <param name="id">The stable, unique identifier for this geometry. Must not be null or whitespace.</param>
    /// <param name="name">The human-readable display name. If null or whitespace, defaults to <paramref name="id"/>.</param>
    /// <param name="ifcGuid">The IFC GUID reference for BIM interoperability. Can be null.</param>
    /// <param name="nativeId">The identifier from the native source system for traceability. Can be null.</param>
    /// <param name="description">A textual description of this geometry. Can be null.</param>
    /// <param name="entityName">The entity type name for polymorphic deserialization.
    /// If null or empty, defaults to "XmiBaseGeometry". Derived classes should override this.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="id"/> is null or whitespace.</exception>
    /// <remarks>
    /// This constructor passes the provided parameters to the base <see cref="XmiBaseEntity"/> constructor,
    /// automatically setting the <see cref="XmiBaseEntity.Domain"/> property to
    /// <see cref="XmiBaseEntityDomainEnum.Geometry"/>. The <paramref name="entityName"/> parameter
    /// allows derived classes to specify their concrete type name for proper JSON polymorphic
    /// deserialization.
    ///
    /// Geometric entities are typically created and managed by <see cref="XmiModel"/> factory methods
    /// such as <c>CreatePoint3D</c>, which enforce deduplication and consistency.
    /// </remarks>
    /// <seealso cref="XmiModel"/>
    /// <seealso cref="XmiBaseEntityDomainEnum.Geometry"/>
    public XmiBaseGeometry(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        string? entityName = null)
        : base(id, name, ifcGuid, nativeId, description,
               string.IsNullOrEmpty(entityName) ? nameof(XmiBaseGeometry) : entityName,
               XmiBaseEntityDomainEnum.Geometry)
    {
    }
}
