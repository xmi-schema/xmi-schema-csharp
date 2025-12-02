using XmiSchema.Core.Entities;

namespace XmiSchema.Core.Interfaces;

/// <summary>
/// Generic interface for managing Cross Model Information (XMI) entities with CRUD operations.
/// </summary>
/// <typeparam name="T">
/// The entity type, which must derive from <see cref="XmiBaseEntity"/>.
/// This constraint ensures type safety and that only valid XMI entities can be managed.
/// </typeparam>
/// <remarks>
/// <para>
/// `IEntityManager&lt;T&gt;` defines the contract for managing collections of entities in the
/// Cross Model Information schema. It provides standard Create, Read, Update, and Delete (CRUD)
/// operations for entities representing structural elements, materials, geometry, and other
/// built environment data.
/// </para>
/// <para>
/// The generic type constraint `where T : XmiBaseEntity` ensures that only classes derived from
/// the base entity class can be managed, maintaining the integrity of the XMI data model.
/// </para>
/// <para>
/// This interface is implemented by <see cref="EntityManager{T}"/>, which provides the concrete
/// implementation for entity collection management. The manager pattern separates data structure
/// concerns from business logic, making the codebase more maintainable and testable.
/// </para>
/// <para>
/// Typical usage involves managing specific entity types such as materials, cross-sections,
/// structural members, and geometric primitives within a model building workflow.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Create a manager for structural materials
/// IEntityManager&lt;XmiStructuralMaterial&gt; materialManager = new EntityManager&lt;XmiStructuralMaterial&gt;();
///
/// // Add a material
/// var concrete = new XmiStructuralMaterial("MAT001", "Concrete C30", ...);
/// materialManager.AddEntity(concrete);
///
/// // Retrieve a material
/// var material = materialManager.GetEntity("MAT001");
///
/// // Find materials by name
/// var concretes = materialManager.FindByName("Concrete");
/// </code>
/// </example>
public interface IEntityManager<T> where T : XmiBaseEntity
{
    /// <summary>
    /// Adds an entity to the managed collection.
    /// </summary>
    /// <param name="Entity">
    /// The entity to add. Must have a unique ID that doesn't already exist in the collection.
    /// </param>
    /// <returns>
    /// <c>true</c> if the entity was successfully added; <c>false</c> if the entity already
    /// exists (duplicate ID) or if the entity is null.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method adds a new entity to the internal collection. Each entity must have a unique
    /// ID within the collection. Attempting to add an entity with a duplicate ID will fail and
    /// return <c>false</c>.
    /// </para>
    /// <para>
    /// The method performs validation to ensure data integrity. Null entities are rejected.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var material = new XmiStructuralMaterial("MAT001", "Steel S355", ...);
    /// bool success = entityManager.AddEntity(material);
    /// if (success)
    /// {
    ///     Console.WriteLine("Material added successfully");
    /// }
    /// </code>
    /// </example>
    bool AddEntity(T Entity);

    /// <summary>
    /// Removes an entity from the managed collection by its unique identifier.
    /// </summary>
    /// <param name="id">
    /// The unique ID of the entity to remove.
    /// </param>
    /// <returns>
    /// <c>true</c> if the entity was found and removed; <c>false</c> if no entity with the
    /// specified ID exists in the collection.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method removes an entity from the collection based on its ID. If the entity doesn't
    /// exist, the operation fails silently and returns <c>false</c>.
    /// </para>
    /// <para>
    /// Note that removing an entity does NOT automatically remove relationships that reference it.
    /// If the entity is part of the model's relationship graph, you should manage those
    /// relationships separately using <see cref="IRelationshipManager{T}"/>.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// bool removed = entityManager.RemoveEntity("MAT001");
    /// if (removed)
    /// {
    ///     Console.WriteLine("Material removed");
    /// }
    /// else
    /// {
    ///     Console.WriteLine("Material not found");
    /// }
    /// </code>
    /// </example>
    bool RemoveEntity(string id);

    /// <summary>
    /// Retrieves an entity from the collection by its unique identifier.
    /// </summary>
    /// <param name="id">
    /// The unique ID of the entity to retrieve.
    /// </param>
    /// <returns>
    /// The entity with the specified ID, or <c>null</c> if no matching entity is found.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method provides O(1) lookup performance when implemented with a dictionary-based
    /// internal structure. It's the primary method for retrieving entities by their unique
    /// identifier.
    /// </para>
    /// <para>
    /// The nullable return type (<c>T?</c>) indicates that the method may return null if the
    /// entity doesn't exist, so callers should check for null before using the result.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var material = entityManager.GetEntity("MAT001");
    /// if (material != null)
    /// {
    ///     Console.WriteLine($"Found: {material.Name}");
    /// }
    /// else
    /// {
    ///     Console.WriteLine("Material not found");
    /// }
    /// </code>
    /// </example>
    T? GetEntity(string id);

    /// <summary>
    /// Retrieves all entities in the managed collection.
    /// </summary>
    /// <returns>
    /// An enumerable sequence of all entities in the collection. Returns an empty sequence
    /// if the collection is empty.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method returns all entities currently managed by this instance. The returned
    /// collection is typically used for iteration, filtering, or transformation operations.
    /// </para>
    /// <para>
    /// The return type is <c>IEnumerable&lt;T&gt;</c>, which supports LINQ queries and deferred
    /// execution. The actual collection is not copied, so modifications to the underlying
    /// collection during iteration may cause exceptions.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Get all materials
    /// var allMaterials = entityManager.GetAllEntities();
    ///
    /// // Filter with LINQ
    /// var concreteMaterials = allMaterials
    ///     .OfType&lt;XmiStructuralMaterial&gt;()
    ///     .Where(m => m.MaterialType == XmiStructuralMaterialTypeEnum.Concrete);
    ///
    /// foreach (var material in concreteMaterials)
    /// {
    ///     Console.WriteLine(material.Name);
    /// }
    /// </code>
    /// </example>
    IEnumerable<T> GetAllEntities();

    /// <summary>
    /// Finds entities in the collection by name using case-insensitive partial matching.
    /// </summary>
    /// <param name="name">
    /// The name or partial name to search for. The search is case-insensitive and matches
    /// entities whose Name property contains this string.
    /// </param>
    /// <returns>
    /// An enumerable sequence of entities whose names contain the specified search string.
    /// Returns an empty sequence if no matches are found.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method performs a case-insensitive substring search on entity names. It's useful
    /// for finding entities when you only know part of the name or want to find all entities
    /// with similar names.
    /// </para>
    /// <para>
    /// The search operation is typically O(n) as it must check all entities in the collection.
    /// For large collections with frequent name-based queries, consider maintaining a separate
    /// index.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Find all entities with "Concrete" in the name
    /// var concreteEntities = entityManager.FindByName("Concrete");
    ///
    /// foreach (var entity in concreteEntities)
    /// {
    ///     Console.WriteLine($"{entity.ID}: {entity.Name}");
    /// }
    /// // Output might be:
    /// // MAT001: Concrete C30
    /// // MAT002: Concrete C40
    /// // MAT003: High-strength Concrete
    /// </code>
    /// </example>
    IEnumerable<T> FindByName(string name);

    /// <summary>
    /// Removes all entities from the managed collection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method clears the entire entity collection, removing all managed entities. Use with
    /// caution as this operation cannot be undone.
    /// </para>
    /// <para>
    /// After calling this method, the collection will be empty and <c>GetAllEntities()</c> will
    /// return an empty sequence.
    /// </para>
    /// <para>
    /// Note that clearing entities does NOT automatically clear relationships. If you're managing
    /// a complete model with both entities and relationships, you should also clear the
    /// relationship manager separately.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Clear all materials
    /// entityManager.Clear();
    ///
    /// // Verify empty
    /// var count = entityManager.GetAllEntities().Count();
    /// Console.WriteLine($"Entity count: {count}"); // Output: Entity count: 0
    /// </code>
    /// </example>
    void Clear();
}

