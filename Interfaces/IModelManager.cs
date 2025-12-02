using System.Collections.Generic;
using XmiSchema.Core.Entities;
using XmiSchema.Core.Relationships;

namespace XmiSchema.Core.Interfaces;

/// <summary>
/// High-level interface for managing complete Cross Model Information (XMI) graph models.
/// </summary>
/// <remarks>
/// <para>
/// `IModelManager` provides a unified interface for managing both entities (graph nodes) and
/// relationships (graph edges) within a complete XMI model. It serves as a higher-level
/// abstraction over <see cref="IEntityManager{T}"/> and <see cref="IRelationshipManager{T}"/>,
/// coordinating operations across both collections.
/// </para>
/// <para>
/// Unlike the generic typed managers, this interface works with base types
/// (<see cref="XmiBaseEntity"/> and <see cref="XmiBaseRelationship"/>) allowing heterogeneous
/// collections of all entity and relationship types within a single model.
/// </para>
/// <para>
/// This interface is typically used when you need:
/// </para>
/// <list type="bullet">
/// <item><description>Unified access to both entities and relationships</description></item>
/// <item><description>Operations that coordinate between entities and relationships</description></item>
/// <item><description>Complete model management without type-specific concerns</description></item>
/// <item><description>Integration points for higher-level model building workflows</description></item>
/// </list>
/// <para>
/// The concrete implementation <see cref="ModelManager"/> uses internal entity and relationship
/// managers to provide coordinated model management. For most use cases, prefer using
/// <see cref="XmiSchemaModelBuilder"/> which provides a more user-friendly fluent API.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Create a model manager
/// IModelManager modelManager = new ModelManager();
///
/// // Add entities of different types
/// modelManager.AddEntity(material);
/// modelManager.AddEntity(crossSection);
/// modelManager.AddEntity(member);
///
/// // Add relationships
/// modelManager.CreateRelationship(new XmiHasStructuralMaterial(crossSection, material));
///
/// // Retrieve all entities and relationships
/// var entities = modelManager.GetAllEntities();
/// var relationships = modelManager.GetAllRelationships();
/// </code>
/// </example>
public interface IModelManager
{
    /// <summary>
    /// Adds an entity to the model.
    /// </summary>
    /// <param name="entity">
    /// The entity to add. Can be any type derived from <see cref="XmiBaseEntity"/>, including
    /// structural members, materials, geometry, cross-sections, etc.
    /// </param>
    /// <remarks>
    /// <para>
    /// This method adds a new node to the graph model. The entity is added to the internal
    /// entity collection managed by this model manager.
    /// </para>
    /// <para>
    /// Unlike <see cref="IEntityManager{T}.AddEntity"/>, this method does not return a boolean
    /// indicating success. It may throw exceptions if the entity is null or has a duplicate ID.
    /// The exact behavior depends on the implementation.
    /// </para>
    /// <para>
    /// Entities can be of any type derived from <see cref="XmiBaseEntity"/>, allowing a
    /// heterogeneous collection representing all parts of the structural model (materials,
    /// sections, members, geometry, etc.).
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var material = new XmiStructuralMaterial("MAT001", "Concrete C30", ...);
    /// var crossSection = new XmiStructuralCrossSection("SEC001", "300x600", ...);
    /// var member = new XmiStructuralCurveMember("MEM001", "Column", ...);
    ///
    /// modelManager.AddEntity(material);
    /// modelManager.AddEntity(crossSection);
    /// modelManager.AddEntity(member);
    ///
    /// // All entity types added to the same model
    /// var allEntities = modelManager.GetAllEntities();
    /// Console.WriteLine($"Total entities: {allEntities.Count}"); // 3
    /// </code>
    /// </example>
    void AddEntity(XmiBaseEntity entity);

    /// <summary>
    /// Creates and adds a relationship to the model.
    /// </summary>
    /// <param name="relationship">
    /// The relationship to create. Can be any type derived from <see cref="XmiBaseRelationship"/>,
    /// including material links, geometry references, node connections, etc.
    /// </param>
    /// <remarks>
    /// <para>
    /// This method adds a new directed edge to the graph model. The relationship is added to the
    /// internal relationship collection managed by this model manager.
    /// </para>
    /// <para>
    /// The method name "CreateRelationship" emphasizes that this operation establishes a new
    /// connection in the graph, creating an edge between two existing nodes (entities).
    /// </para>
    /// <para>
    /// Unlike <see cref="IRelationshipManager{T}.AddRelationship"/>, this method does not return
    /// a boolean indicating success. It may throw exceptions if the relationship is null, has a
    /// duplicate ID, or references non-existent entities. The exact behavior depends on the
    /// implementation.
    /// </para>
    /// <para>
    /// Relationships can be of any type derived from <see cref="XmiBaseRelationship"/>, allowing
    /// a heterogeneous collection of all relationship types in the model.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Entities must be added first
    /// modelManager.AddEntity(material);
    /// modelManager.AddEntity(crossSection);
    /// modelManager.AddEntity(member);
    ///
    /// // Create relationships between entities
    /// modelManager.CreateRelationship(new XmiHasStructuralMaterial(crossSection, material));
    /// modelManager.CreateRelationship(new XmiHasStructuralCrossSection(member, crossSection));
    ///
    /// // Relationships now connect the entities
    /// var allRelationships = modelManager.GetAllRelationships();
    /// Console.WriteLine($"Total relationships: {allRelationships.Count}"); // 2
    /// </code>
    /// </example>
    void CreateRelationship(XmiBaseRelationship relationship);

    /// <summary>
    /// Retrieves all entities in the model.
    /// </summary>
    /// <returns>
    /// A list containing all entities (nodes) in the graph model. The list contains instances of
    /// all entity types derived from <see cref="XmiBaseEntity"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method returns all nodes in the graph model as a concrete <see cref="List{T}"/>.
    /// The returned list contains entities of all types: materials, cross-sections, members,
    /// geometry, storeys, etc.
    /// </para>
    /// <para>
    /// The returned list is typically a copy or snapshot of the internal collection, so
    /// modifications to the list do not affect the model (implementation-dependent).
    /// </para>
    /// <para>
    /// This method is commonly used for:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Model validation and integrity checking</description></item>
    /// <item><description>Exporting the complete model to JSON</description></item>
    /// <item><description>Enumerating and filtering entities by type</description></item>
    /// <item><description>Calculating model statistics</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// <code>
    /// var allEntities = modelManager.GetAllEntities();
    ///
    /// Console.WriteLine($"Total entities: {allEntities.Count}");
    ///
    /// // Filter by type using LINQ
    /// var materials = allEntities.OfType&lt;XmiStructuralMaterial&gt;().ToList();
    /// var members = allEntities.OfType&lt;XmiStructuralCurveMember&gt;().ToList();
    /// var geometry = allEntities.OfType&lt;XmiPoint3D&gt;().ToList();
    ///
    /// Console.WriteLine($"Materials: {materials.Count}");
    /// Console.WriteLine($"Members: {members.Count}");
    /// Console.WriteLine($"Geometry: {geometry.Count}");
    /// </code>
    /// </example>
    List<XmiBaseEntity> GetAllEntities();

    /// <summary>
    /// Retrieves all relationships in the model.
    /// </summary>
    /// <returns>
    /// A list containing all relationships (edges) in the graph model. The list contains instances
    /// of all relationship types derived from <see cref="XmiBaseRelationship"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method returns all directed edges in the graph model as a concrete <see cref="List{T}"/>.
    /// The returned list contains relationships of all types: material links, geometry references,
    /// node connections, segment associations, etc.
    /// </para>
    /// <para>
    /// The returned list is typically a copy or snapshot of the internal collection, so
    /// modifications to the list do not affect the model (implementation-dependent).
    /// </para>
    /// <para>
    /// This method is commonly used for:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Graph analysis and visualization</description></item>
    /// <item><description>Exporting the complete model to JSON</description></item>
    /// <item><description>Dependency analysis and topological sorting</description></item>
    /// <item><description>Enumerating and filtering relationships by type</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// <code>
    /// var allRelationships = modelManager.GetAllRelationships();
    ///
    /// Console.WriteLine($"Total relationships: {allRelationships.Count}");
    ///
    /// // Filter by type using LINQ
    /// var materialLinks = allRelationships.OfType&lt;XmiHasStructuralMaterial&gt;().ToList();
    /// var geometryLinks = allRelationships.OfType&lt;XmiHasGeometry&gt;().ToList();
    /// var segmentLinks = allRelationships.OfType&lt;XmiHasSegment&gt;().ToList();
    ///
    /// Console.WriteLine($"Material links: {materialLinks.Count}");
    /// Console.WriteLine($"Geometry links: {geometryLinks.Count}");
    /// Console.WriteLine($"Segment links: {segmentLinks.Count}");
    /// </code>
    /// </example>
    List<XmiBaseRelationship> GetAllRelationships();
}

