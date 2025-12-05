using System;
using System.Collections.Generic;
using System.Linq;
using XmiSchema.Core.Entities;
using XmiSchema.Core.Relationships;
using XmiSchema.Core.Geometries;
using XmiSchema.Core.Enums;
using XmiSchema.Core.Parameters;
using XmiSchema.Core.Models.Entities.Physical;
using XmiSchema.Core.Models.Entities.StructuralAnalytical;

namespace XmiSchema.Core.Models
{
    /// <summary>
    /// Represents an in-memory Cross Model Information graph including all entities and relationships.
    /// </summary>
    public class XmiModel
    {
        public List<XmiBaseEntity> Entities { get; set; } = new();
        public List<XmiBaseRelationship> Relationships { get; set; } = new();

        /// <summary>
        /// Adds a structural material entity to the model.
        /// </summary>
        /// <param name="material">Material to insert.</param>
        public void AddXmiMaterial(XmiMaterial material)
        {
            Entities.Add(material);
        }

        /// <summary>
        /// Adds a structural cross-section entity to the model.
        /// </summary>
        /// <param name="crossSection">Cross-section instance.</param>
        public void AddXmiCrossSection(XmiCrossSection crossSection)
        {
            Entities.Add(crossSection);
        }

        /// <summary>
        /// Adds a curve member entity to the model.
        /// </summary>
        /// <param name="member">Curve member.</param>
        public void AddXmiStructuralCurveMember(XmiStructuralCurveMember member)
        {
            Entities.Add(member);
        }

        /// <summary>
        /// Adds a surface member entity to the model.
        /// </summary>
        /// <param name="member">Surface member.</param>
        public void AddXmiStructuralSurfaceMember(XmiStructuralSurfaceMember member)
        {
            Entities.Add(member);
        }

        /// <summary>
        /// Adds a point connection entity to the model.
        /// </summary>
        /// <param name="connection">Point connection.</param>
        public void AddXmiStructuralPointConnection(XmiStructuralPointConnection connection)
        {
            Entities.Add(connection);
        }

        /// <summary>
        /// Adds a storey entity to the model.
        /// </summary>
        /// <param name="storey">Storey entity.</param>
        public void AddXmiStorey(XmiStorey storey)
        {
            Entities.Add(storey);
        }

        /// <summary>
        /// Adds a 3D point entity to the model.
        /// </summary>
        /// <param name="point">Point entity.</param>
        public void AddXmiPoint3D(XmiPoint3D point)
        {
            Entities.Add(point);
        }

        /// <summary>
        /// Adds a beam physical element to the model.
        /// </summary>
        /// <param name="beam">Beam entity.</param>
        public void AddXmiBeam(XmiBeam beam)
        {
            Entities.Add(beam);
        }

        /// <summary>
        /// Adds a column physical element to the model.
        /// </summary>
        /// <param name="column">Column entity.</param>
        public void AddXmiColumn(XmiColumn column)
        {
            Entities.Add(column);
        }

        /// <summary>
        /// Adds a slab physical element to the model.
        /// </summary>
        /// <param name="slab">Slab entity.</param>
        public void AddXmiSlab(XmiSlab slab)
        {
            Entities.Add(slab);
        }

        /// <summary>
        /// Adds a wall physical element to the model.
        /// </summary>
        /// <param name="wall">Wall entity.</param>
        public void AddXmiWall(XmiWall wall)
        {
            Entities.Add(wall);
        }

        /// <summary>
        /// Adds a point relationship to the model.
        /// </summary>
        /// <param name="relation">Relationship instance.</param>
        public void AddXmiHasPoint3D(XmiHasPoint3D relation)
        {
            Relationships.Add(relation);
        }

        /// <summary>
        /// Adds a material relationship to the model.
        /// </summary>
        /// <param name="relation">Relationship instance.</param>
        public void AddXmiHasMaterial(XmiHasMaterial relation)
        {
            Relationships.Add(relation);
        }

        /// <summary>
        /// Adds a structural node relationship to the model.
        /// </summary>
        /// <param name="relation">Relationship instance.</param>
        public void AddXmiHasStructuralPointConnection(XmiHasStructuralPointConnection relation)
        {
            Relationships.Add(relation);
        }

        /// <summary>
        /// Adds a cross-section relationship to the model.
        /// </summary>
        /// <param name="relation">Relationship instance.</param>
        public void AddXmiHasCrossSection(XmiHasCrossSection relation)
        {
            Relationships.Add(relation);
        }

        /// <summary>
        /// Adds a storey relationship to the model.
        /// </summary>
        /// <param name="relation">Relationship instance.</param>
        public void AddXmiHasStorey(XmiHasStorey relation)
        {
            Relationships.Add(relation);
        }

        /// <summary>
        /// Adds a structural curve member relationship to the model.
        /// </summary>
        /// <param name="relation">Relationship instance.</param>
        public void AddXmiHasStructuralCurveMember(XmiHasStructuralCurveMember relation)
        {
            Relationships.Add(relation);
        }

        /// <summary>
        /// Finds a structural point connection that references the same physical point as the provided connection.
        /// </summary>
        /// <param name="inputConnection">Connection to match.</param>
        /// <returns>The ID for the matching connection if found; otherwise null.</returns>
        public string? FindMatchingPointConnectionByPoint3D(XmiStructuralPointConnection inputConnection)
        {
            // Step 1: retrieve the Point3D referenced by the incoming connection via existing relationships.
            var inputPoint = Relationships
                .OfType<XmiHasPoint3D>()
                .FirstOrDefault(rel => rel.Source?.Id == inputConnection.Id)
                ?.Target as XmiPoint3D;

            if (inputPoint == null) return null;

            // Step 2: scan other point connections to see if any reference a point with matching coordinates.
            var match = Relationships
                .OfType<XmiHasPoint3D>()
                .Where(rel => rel.Source is XmiStructuralPointConnection && rel.Source.Id != inputConnection.Id)
                .FirstOrDefault(rel => ArePointsEqual(rel.Target as XmiPoint3D, inputPoint));

            return match?.Source?.Id;
        }

        private bool ArePointsEqual(XmiPoint3D? p1, XmiPoint3D? p2, double tolerance = 1e-10)
        {
            if (p1 == null || p2 == null) return false;
            return Math.Abs(p1.X - p2.X) < tolerance &&
                   Math.Abs(p1.Y - p2.Y) < tolerance &&
                   Math.Abs(p1.Z - p2.Z) < tolerance;
        }

        /// <summary>
        /// Creates or reuses a structural point connection for the provided storey and coordinate.
        /// </summary>
        /// <param name="id">Unique identifier for the new connection.</param>
        /// <param name="name">Display name.</param>
        /// <param name="ifcGuid">IFC GUID reference.</param>
        /// <param name="nativeId">Native identifier.</param>
        /// <param name="description">Description for the connection.</param>
        /// <param name="storey">Optional storey containing the connection.</param>
        /// <param name="point">Point geometry representing the node.</param>
        /// <returns>An existing or newly created connection.</returns>
        public XmiStructuralPointConnection CreateStructurePointConnection(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiStorey? storey,
            XmiPoint3D point
        )
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));
            try
            {
                var tempConnection = new XmiStructuralPointConnection(
                    id,
                    name,
                    ifcGuid,
                    nativeId,
                    description
                );

                var existingConnectionId = FindMatchingPointConnectionByPoint3D(tempConnection);
                if (existingConnectionId != null)
                {
                    return GetEntitiesOfType<XmiStructuralPointConnection>()
                        .FirstOrDefault(c => c.Id == existingConnectionId) ?? tempConnection;
                }

                // Look for an existing storey with the same native identifier; fall back to the provided one if missing.
                XmiStorey? existingStorey = null;
                if (storey != null)
                {
                    existingStorey = GetEntitiesOfType<XmiStorey>()
                        .FirstOrDefault(s => s.NativeId == storey.NativeId) ?? storey;
                }

                // Reuse an existing point with matching coordinates whenever possible.
                var existingPoint = GetEntitiesOfType<XmiPoint3D>()
                    .FirstOrDefault(p => p.Equals(point)) ?? point;

                var connection = new XmiStructuralPointConnection(
                    id,
                    name,
                    ifcGuid,
                    nativeId,
                    description
                );

                AddXmiStructuralPointConnection(connection);

                if (existingStorey != null)
                {
                    var storeyRelation = new XmiHasStorey(connection, existingStorey);
                    AddXmiHasStorey(storeyRelation);
                }

                if (existingPoint != null)
                {
                    var pointRelation = new XmiHasPoint3D(connection, existingPoint);
                    AddXmiHasPoint3D(pointRelation);
                }

                return connection;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create or reuse a structural point connection.", ex);
            }
        }

        /// <summary>
        /// Returns all entities of the requested type.
        /// </summary>
        /// <typeparam name="T">Subtype of <see cref="XmiBaseEntity"/> to retrieve.</typeparam>
        public List<T> GetEntitiesOfType<T>() where T : XmiBaseEntity
        {
            return Entities.OfType<T>().ToList();
        }

        /// <summary>
        /// Creates a new <see cref="XmiPoint3D"/> entity, adding it to the model.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="name">Display name.</param>
        /// <param name="ifcGuid">IFC GUID reference.</param>
        /// <param name="nativeId">Native identifier.</param>
        /// <param name="description">Point description.</param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        /// <returns>The newly created point.</returns>
        public XmiPoint3D CreatePoint3D(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            double x,
            double y,
            double z
        )
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));

            try
            {
                var tempPoint = new XmiPoint3D(id, name, ifcGuid, nativeId, description, x, y, z);
                var existingPoints = GetEntitiesOfType<XmiPoint3D>();
                var existingPoint = existingPoints.FirstOrDefault(p => p.Equals(tempPoint));

                if (existingPoint != null)
                {
                    return existingPoint;
                }

                var point = new XmiPoint3D(
                    id,
                    name,
                    ifcGuid,
                    nativeId,
                    description,
                    x,
                    y,
                    z
                );

                AddXmiPoint3D(point);

                return point;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create Point3D.", ex);
            }
        }

        /// <summary>
        /// Creates or reuses a structural curve member, wiring up cross-section, storey, and node relationships.
        /// </summary>
        /// <returns>The created curve member.</returns>
        public XmiStructuralCurveMember CreateStructuralCurveMember(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiCrossSection crossSection,
            XmiStorey? storey,
            XmiStructuralCurveMemberTypeEnum curveMemberType,
            List<XmiStructuralPointConnection> nodes,
            List<XmiSegment>? segments,
            XmiSystemLineEnum systemLine,
            XmiStructuralPointConnection beginNode,
            XmiStructuralPointConnection endNode,
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
        )
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));
            try
            {
                var existingCrossSection = GetEntitiesOfType<XmiCrossSection>()
                    .FirstOrDefault(c => c.NativeId == crossSection.NativeId) ?? crossSection;

                XmiStorey? existingStorey = null;
                if (storey != null)
                {
                    existingStorey = GetEntitiesOfType<XmiStorey>()
                        .FirstOrDefault(s => s.NativeId == storey.NativeId) ?? storey;
                }

                var existingBeginNodeId = FindMatchingPointConnectionByPoint3D(beginNode);
                var existingBeginNode = existingBeginNodeId != null
                    ? GetEntitiesOfType<XmiStructuralPointConnection>().FirstOrDefault(n => n.Id == existingBeginNodeId) ?? beginNode
                    : beginNode;

                var existingEndNodeId = FindMatchingPointConnectionByPoint3D(endNode);
                var existingEndNode = existingEndNodeId != null
                    ? GetEntitiesOfType<XmiStructuralPointConnection>().FirstOrDefault(n => n.Id == existingEndNodeId) ?? endNode
                    : endNode;

                var curveMember = new XmiStructuralCurveMember(
                    id,
                    name,
                    ifcGuid,
                    nativeId,
                    description,
                    curveMemberType,
                    systemLine,
                    length,
                    localAxisX,
                    localAxisY,
                    localAxisZ,
                    beginNodeXOffset,
                    endNodeXOffset,
                    beginNodeYOffset,
                    endNodeYOffset,
                    beginNodeZOffset,
                    endNodeZOffset,
                    endFixityStart,
                    endFixityEnd
                );

                AddXmiStructuralCurveMember(curveMember);

                var crossSectionRelation = new XmiHasCrossSection(curveMember, existingCrossSection);
                AddXmiHasCrossSection(crossSectionRelation);

                if (existingStorey != null)
                {
                    var storeyRelation = new XmiHasStorey(curveMember, existingStorey);
                    AddXmiHasStorey(storeyRelation);
                }

                var beginNodeRelation = new XmiHasStructuralPointConnection(curveMember, existingBeginNode);
                var endNodeRelation = new XmiHasStructuralPointConnection(curveMember, existingEndNode);
                AddXmiHasStructuralPointConnection(beginNodeRelation);
                AddXmiHasStructuralPointConnection(endNodeRelation);

                return curveMember;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create structural curve member.", ex);
            }
        }

        /// <summary>
        /// Creates a structural cross-section, reusing an existing material relationship when a matching native ID is found.
        /// </summary>
        /// <returns>The created cross-section entity.</returns>
        public XmiCrossSection CreateCrossSection(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterial? material,
            XmiShapeEnum shape,
            IXmiShapeParameters parameters,
            double area,
            double secondMomentOfAreaXAxis,
            double secondMomentOfAreaYAxis,
            double radiusOfGyrationXAxis,
            double radiusOfGyrationYAxis,
            double elasticModulusXAxis,
            double elasticModulusYAxis,
            double plasticModulusXAxis,
            double plasticModulusYAxis,
            double torsionalConstant
        )
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("ID cannot be null or empty", nameof(id));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

            try
            {
                XmiMaterial? existingMaterial = null;

                // Safe handling: only reuse material references when a native ID is provided.
                if (material != null && !string.IsNullOrEmpty(material.NativeId))
                {
                    var materials = GetEntitiesOfType<XmiMaterial>() ?? Enumerable.Empty<XmiMaterial>();
                    existingMaterial = materials.FirstOrDefault(m => m?.NativeId == material.NativeId) ?? material;
                }

                var crossSection = new XmiCrossSection(
                    id,
                    name,
                    ifcGuid,
                    nativeId,
                    description,
                    shape,
                    parameters,
                    area,
                    secondMomentOfAreaXAxis,
                    secondMomentOfAreaYAxis,
                    radiusOfGyrationXAxis,
                    radiusOfGyrationYAxis,
                    elasticModulusXAxis,
                    elasticModulusYAxis,
                    plasticModulusXAxis,
                    plasticModulusYAxis,
                    torsionalConstant
                );

                AddXmiCrossSection(crossSection);

                if (existingMaterial != null)
                {
                    var materialRelation = new XmiHasMaterial(crossSection, existingMaterial);
                    AddXmiHasMaterial(materialRelation);
                }

                return crossSection;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create structural cross-section.", ex);
            }
        }



        /// <summary>
        /// Creates or reuses a storey by native identifier.
        /// </summary>
        /// <returns>The created or reused storey.</returns>
        public XmiStorey CreateStorey(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            double storeyElevation,
            double storeyMass
        )
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));

            try
            {
                var existingStorey = GetEntitiesOfType<XmiStorey>()
                    .FirstOrDefault(s => s.NativeId == nativeId);

                if (existingStorey != null)
                {
                    return existingStorey;
                }

                var storey = new XmiStorey(
                    id,
                    name,
                    ifcGuid,
                    nativeId,
                    description,
                    storeyElevation,
                    storeyMass
                );

                AddXmiStorey(storey);

                return storey;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create storey.", ex);
            }
        }

        /// <summary>
        /// Creates or reuses a structural material identified by <paramref name="nativeId"/>.
        /// </summary>
        /// <returns>The created or reused material.</returns>
        public XmiMaterial CreateMaterial(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterialTypeEnum materialType,
            double grade,
            double unitWeight,
            string eModulus,
            string gModulus,
            string poissonRatio,
            double thermalCoefficient
        )
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));

            try
            {
                var existingMaterial = GetEntitiesOfType<XmiMaterial>()
                    .FirstOrDefault(m => m.NativeId == nativeId);

                if (existingMaterial != null)
                {
                    return existingMaterial;
                }

                var material = new XmiMaterial(
                    id,
                    name,
                    ifcGuid,
                    nativeId,
                    description,
                    materialType,
                    grade,
                    unitWeight,
                    eModulus,
                    gModulus,
                    poissonRatio,
                    thermalCoefficient
                );

                AddXmiMaterial(material);

                return material;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create structural material.", ex);
            }
        }

        /// <summary>
        /// Creates a structural surface member and wires the relevant storey and material relationships.
        /// </summary>
        /// <returns>The created surface member.</returns>
        public XmiStructuralSurfaceMember CreateStructuralSurfaceMember(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterial material,
            XmiStructuralSurfaceMemberTypeEnum surfaceMemberType,
            double thickness,
            XmiStructuralSurfaceMemberSystemPlaneEnum systemPlane,
            List<XmiStructuralPointConnection> nodes,
            XmiStorey? storey,
            List<XmiSegment> segments,
            double area,
            double zOffset,
            string localAxisX,
            string localAxisY,
            string localAxisZ,
            double height
        )
        {
            try
            {
                XmiStorey? existingStorey = null;
                if (storey != null)
                {
                    existingStorey = GetEntitiesOfType<XmiStorey>()
                        .FirstOrDefault(s => s.NativeId == storey.NativeId) ?? storey;
                }

                XmiMaterial? existingMaterial = null;

                if (material != null && !string.IsNullOrEmpty(material.NativeId))
                {
                    var materials = GetEntitiesOfType<XmiMaterial>() ?? Enumerable.Empty<XmiMaterial>();
                    existingMaterial = materials.FirstOrDefault(m => m?.NativeId == material.NativeId) ?? material;
                }

                var surfaceMember = new XmiStructuralSurfaceMember(
                    id,
                    name,
                    ifcGuid,
                    nativeId,
                    description,
                    surfaceMemberType,
                    thickness,
                    systemPlane,
                    area,
                    zOffset,
                    localAxisX,
                    localAxisY,
                    localAxisZ,
                    height
                );

                AddXmiStructuralSurfaceMember(surfaceMember);

                if (existingMaterial != null)
                {
                    var materialRelation = new XmiHasMaterial(surfaceMember, existingMaterial);
                    AddXmiHasMaterial(materialRelation);
                }

                if (existingStorey != null)
                {
                    var storeyRelation = new XmiHasStorey(surfaceMember, existingStorey);
                    AddXmiHasStorey(storeyRelation);
                }

                return surfaceMember;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create structural surface member.", ex);
            }
        }
    }
}
