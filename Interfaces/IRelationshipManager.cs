using XmiSchema.Core.Relationships;

namespace XmiSchema.Core.Interfaces;

/// <summary>
/// Generic interface for managing Cross Model Information (XMI) relationships with graph-aware CRUD operations.
/// </summary>
/// <typeparam name="T">
/// The relationship type, which must derive from <see cref="XmiBaseRelationship"/>.
/// This constraint ensures type safety and that only valid XMI relationships can be managed.
/// </typeparam>
/// <remarks>
/// <para>
/// `IRelationshipManager&lt;T&gt;` defines the contract for managing directed edges in the
/// Cross Model Information graph model. Relationships represent connections between entities
/// (nodes) and form the edges of the directed graph.
/// </para>
/// <para>
/// Unlike <see cref="IEntityManager{T}"/> which manages nodes, this interface provides
/// graph-specific query operations:
/// </para>
/// <list type="bullet">
/// <item><description><b>FindBySource</b>: Find all outgoing relationships from an entity</description></item>
/// <item><description><b>FindByTarget</b>: Find all incoming relationships to an entity</description></item>
/// </list>
/// <para>
/// These operations enable graph traversals, dependency analysis, and understanding entity
/// connectivity within the structural engineering model.
/// </para>
/// <para>
/// The generic type constraint `where T : XmiBaseRelationship` ensures that only classes derived
/// from the base relationship class can be managed, maintaining graph integrity.
/// </para>
/// <para>
/// This interface is implemented by <see cref="RelationshipManager{T}"/>, which provides the
/// concrete implementation for relationship collection management.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Create a manager for material relationships
/// IRelationshipManager&lt;XmiHasStructuralMaterial&gt; relManager =
///     new RelationshipManager&lt;XmiHasStructuralMaterial&gt;();
///
/// // Add a relationship
/// var relationship = new XmiHasStructuralMaterial(crossSection, material);
/// relManager.AddRelationship(relationship);
///
/// // Find all relationships from a cross-section
/// var outgoing = relManager.FindBySource("SEC001");
///
/// // Find what references a material
/// var incoming = relManager.FindByTarget("MAT001");
/// </code>
/// </example>
public interface IRelationshipManager<T> where T : XmiBaseRelationship
{
    /// <summary>
    /// Adds a relationship (directed edge) to the managed collection.
    /// </summary>
    /// <param name="relationship">
    /// The relationship to add. Must have a unique ID and valid source/target references.
    /// </param>
    /// <returns>
    /// <c>true</c> if the relationship was successfully added; <c>false</c> if the relationship
    /// already exists (duplicate ID) or if the relationship is null.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method adds a new directed edge to the graph. Each relationship must have:
    /// </para>
    /// <list type="bullet">
    /// <item><description>A unique ID within the collection</description></item>
    /// <item><description>A valid source entity reference (the "from" node)</description></item>
    /// <item><description>A valid target entity reference (the "to" node)</description></item>
    /// </list>
    /// <para>
    /// The method performs validation to ensure data integrity. Null relationships and duplicate
    /// IDs are rejected.
    /// </para>
    /// <para>
    /// Note that this method does NOT validate whether the source and target entities actually
    /// exist in an entity manager. That validation should be performed by higher-level components
    /// like <see cref="XmiSchemaModelBuilder"/>.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var relationship = new XmiHasStructuralMaterial(crossSection, material);
    /// bool success = relationshipManager.AddRelationship(relationship);
    /// if (success)
    /// {
    ///     Console.WriteLine($"Added relationship: {relationship.Source.ID} → {relationship.Target.ID}");
    /// }
    /// </code>
    /// </example>
    bool AddRelationship(T relationship);

    /// <summary>
    /// Removes a relationship from the managed collection by its unique identifier.
    /// </summary>
    /// <param name="id">
    /// The unique ID of the relationship to remove.
    /// </param>
    /// <returns>
    /// <c>true</c> if the relationship was found and removed; <c>false</c> if no relationship
    /// with the specified ID exists in the collection.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method removes a directed edge from the graph. If the relationship doesn't exist,
    /// the operation fails silently and returns <c>false</c>.
    /// </para>
    /// <para>
    /// Removing a relationship does NOT remove the source or target entities. Graph nodes
    /// (entities) and edges (relationships) are managed independently.
    /// </para>
    /// <para>
    /// After removal, queries using <see cref="FindBySource"/> and <see cref="FindByTarget"/>
    /// will no longer return the removed relationship.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// bool removed = relationshipManager.RemoveRelationship("REL001");
    /// if (removed)
    /// {
    ///     Console.WriteLine("Relationship removed from graph");
    /// }
    /// else
    /// {
    ///     Console.WriteLine("Relationship not found");
    /// }
    /// </code>
    /// </example>
    bool RemoveRelationship(string id);

    /// <summary>
    /// Retrieves a relationship from the collection by its unique identifier.
    /// </summary>
    /// <param name="id">
    /// The unique ID of the relationship to retrieve.
    /// </param>
    /// <returns>
    /// The relationship with the specified ID, or <c>null</c> if no matching relationship is found.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method provides direct lookup by relationship ID. It's useful when you have a
    /// specific relationship identifier and need to retrieve the complete relationship object
    /// with its source and target references.
    /// </para>
    /// <para>
    /// The nullable return type (<c>T?</c>) indicates that the method may return null if the
    /// relationship doesn't exist, so callers should check for null before using the result.
    /// </para>
    /// <para>
    /// For graph traversal operations (finding relationships by source or target), use
    /// <see cref="FindBySource"/> or <see cref="FindByTarget"/> instead.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var relationship = relationshipManager.GetRelationship("REL001");
    /// if (relationship != null)
    /// {
    ///     Console.WriteLine($"Relationship: {relationship.Source.ID} → {relationship.Target.ID}");
    ///     Console.WriteLine($"Type: {relationship.EntityType}");
    /// }
    /// else
    /// {
    ///     Console.WriteLine("Relationship not found");
    /// }
    /// </code>
    /// </example>
    T? GetRelationship(string id);

    /// <summary>
    /// Retrieves all relationships in the managed collection.
    /// </summary>
    /// <returns>
    /// An enumerable sequence of all relationships (edges) in the graph. Returns an empty
    /// sequence if the collection is empty.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method returns all directed edges currently in the graph. The returned collection
    /// is typically used for:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Graph analysis (counting edges, statistics)</description></item>
    /// <item><description>Exporting the complete graph to JSON</description></item>
    /// <item><description>LINQ queries on relationships</description></item>
    /// <item><description>Validation and integrity checking</description></item>
    /// </list>
    /// <para>
    /// The return type is <c>IEnumerable&lt;T&gt;</c>, which supports LINQ queries and deferred
    /// execution. Modifications to the underlying collection during iteration may cause exceptions.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Get all relationships
    /// var allRelationships = relationshipManager.GetAll();
    ///
    /// // Count by type
    /// var relationshipCounts = allRelationships
    ///     .GroupBy(r => r.EntityType)
    ///     .Select(g => new { Type = g.Key, Count = g.Count() });
    ///
    /// foreach (var group in relationshipCounts)
    /// {
    ///     Console.WriteLine($"{group.Type}: {group.Count} relationships");
    /// }
    /// </code>
    /// </example>
    IEnumerable<T> GetAll();

    /// <summary>
    /// Finds all relationships where the specified entity is the source (outgoing edges).
    /// </summary>
    /// <param name="sourceId">
    /// The ID of the source entity. Returns all relationships where this entity is the "from" node.
    /// </param>
    /// <returns>
    /// An enumerable sequence of relationships originating from the specified entity. Returns
    /// an empty sequence if no relationships are found.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method enables graph traversal by finding all outgoing edges from a specific node.
    /// In structural engineering terms, this answers questions like:
    /// </para>
    /// <list type="bullet">
    /// <item><description>"What material does this cross-section use?" (CrossSection → Material)</description></item>
    /// <item><description>"What segments make up this member?" (Member → Segments)</description></item>
    /// <item><description>"What nodes does this member connect to?" (Member → Nodes)</description></item>
    /// </list>
    /// <para>
    /// The operation is typically O(n) as it must scan all relationships. For large graphs with
    /// frequent source queries, consider maintaining an adjacency list index.
    /// </para>
    /// <para>
    /// This is a fundamental operation for dependency analysis and topological sorting, as it
    /// identifies what an entity depends on or references.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Find what a cross-section references
    /// var outgoing = relationshipManager.FindBySource("SEC001");
    ///
    /// foreach (var relationship in outgoing)
    /// {
    ///     Console.WriteLine($"{relationship.EntityType}: {relationship.Source.ID} → {relationship.Target.ID}");
    /// }
    /// // Output might be:
    /// // XmiHasStructuralMaterial: SEC001 → MAT001
    /// </code>
    /// </example>
    IEnumerable<T> FindBySource(string sourceId);

    /// <summary>
    /// Finds all relationships where the specified entity is the target (incoming edges).
    /// </summary>
    /// <param name="targetId">
    /// The ID of the target entity. Returns all relationships where this entity is the "to" node.
    /// </param>
    /// <returns>
    /// An enumerable sequence of relationships pointing to the specified entity. Returns
    /// an empty sequence if no relationships are found.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method enables reverse graph traversal by finding all incoming edges to a specific node.
    /// In structural engineering terms, this answers questions like:
    /// </para>
    /// <list type="bullet">
    /// <item><description>"What cross-sections use this material?" (Material ← CrossSections)</description></item>
    /// <item><description>"What members are on this storey?" (Storey ← Members)</description></item>
    /// <item><description>"What elements reference this geometry?" (Geometry ← Elements)</description></item>
    /// </list>
    /// <para>
    /// The operation is typically O(n) as it must scan all relationships. For large graphs with
    /// frequent target queries, consider maintaining a reverse adjacency list index.
    /// </para>
    /// <para>
    /// This operation is essential for impact analysis: "If I change this entity, what else is
    /// affected?" It also helps identify orphaned entities (targets with no incoming relationships).
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Find what references a material
    /// var incoming = relationshipManager.FindByTarget("MAT001");
    ///
    /// Console.WriteLine($"Material MAT001 is used by {incoming.Count()} entities:");
    /// foreach (var relationship in incoming)
    /// {
    ///     Console.WriteLine($"  {relationship.Source.ID} ({relationship.Source.Name})");
    /// }
    /// // Output might be:
    /// // Material MAT001 is used by 3 entities:
    /// //   SEC001 (300x600 Column Section)
    /// //   SEC002 (200x400 Beam Section)
    /// //   SURF001 (200mm Slab)
    /// </code>
    /// </example>
    IEnumerable<T> FindByTarget(string targetId);

    /// <summary>
    /// Removes all relationships from the managed collection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method clears all edges from the graph, removing all relationships while leaving
    /// entities (nodes) intact. Use with caution as this operation cannot be undone.
    /// </para>
    /// <para>
    /// After calling this method, the graph will have no edges, and <c>GetAll()</c> will return
    /// an empty sequence. Entities will remain in the model but will be disconnected.
    /// </para>
    /// <para>
    /// This operation is useful when rebuilding the relationship graph from scratch, such as
    /// during model re-import or when using <see cref="ExtensionRelationshipExporter"/> to
    /// re-infer relationships.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Clear all relationships
    /// relationshipManager.Clear();
    ///
    /// // Verify empty
    /// var count = relationshipManager.GetAll().Count();
    /// Console.WriteLine($"Relationship count: {count}"); // Output: Relationship count: 0
    ///
    /// // Entities still exist, but graph is now disconnected
    /// </code>
    /// </example>
    void Clear();
}

