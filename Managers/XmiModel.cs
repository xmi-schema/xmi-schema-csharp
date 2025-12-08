using XmiSchema.Entities.Physical;
using XmiSchema.Entities.StructuralAnalytical;
using XmiSchema.Entities.Bases;
using XmiSchema.Entities.Geometries;
using XmiSchema.Entities.Commons;
using XmiSchema.Entities.Relationships;
using XmiSchema.Enums;
using XmiSchema.Parameters;

namespace XmiSchema.Managers
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
        public void AddXmiPoint3d(XmiPoint3d point)
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
        public void AddXmiHasPoint3d(XmiHasPoint3d relation)
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
        /// Adds a segment relationship to the model.
        /// </summary>
        /// <param name="relation">Relationship instance.</param>
        public void AddXmiHasSegment(XmiHasSegment relation)
        {
            Relationships.Add(relation);
        }

        /// <summary>
        /// Adds a geometry relationship to the model.
        /// </summary>
        /// <param name="relation">Relationship instance.</param>
        public void AddXmiHasGeometry(XmiHasGeometry relation)
        {
            Relationships.Add(relation);
        }

        /// <summary>
        /// Adds a line geometry entity to the model.
        /// </summary>
        /// <param name="line">Line entity to add.</param>
        public void AddXmiLine3d(XmiLine3d line)
        {
            Entities.Add(line);
        }

        /// <summary>
        /// Adds an arc geometry entity to the model.
        /// </summary>
        /// <param name="arc">Arc entity to add.</param>
        public void AddXmiArc3d(XmiArc3d arc)
        {
            Entities.Add(arc);
        }

        /// <summary>
        /// Adds a segment entity to the model.
        /// </summary>
        /// <param name="segment">Segment entity to add.</param>
        public void AddXmiSegment(XmiSegment segment)
        {
            Entities.Add(segment);
        }

        /// <summary>
        /// Finds a structural point connection that references the same physical point as the provided connection.
        /// </summary>
        /// <param name="inputConnection">Connection to match.</param>
        /// <returns>The ID for the matching connection if found; otherwise null.</returns>
        public string? FindMatchingXmiStructuralPointConnectionByPoint3d(XmiStructuralPointConnection inputConnection)
        {
            // Step 1: retrieve the Point3d referenced by the incoming connection via existing relationships.
            var inputPoint = Relationships
                .OfType<XmiHasPoint3d>()
                .FirstOrDefault(rel => rel.Source?.Id == inputConnection.Id)
                ?.Target as XmiPoint3d;

            if (inputPoint == null) return null;

            // Step 2: scan other point connections to see if any reference a point with matching coordinates.
            var match = Relationships
                .OfType<XmiHasPoint3d>()
                .Where(rel => rel.Source is XmiStructuralPointConnection && rel.Source.Id != inputConnection.Id)
                .FirstOrDefault(rel => AreXmiPoint3dsEqual(rel.Target as XmiPoint3d, inputPoint));

            return match?.Source?.Id;
        }

        private bool AreXmiPoint3dsEqual(XmiPoint3d? p1, XmiPoint3d? p2, double tolerance = 1e-10)
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
        public XmiStructuralPointConnection CreateXmiStructurePointConnection(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiStorey? storey,
            XmiPoint3d point
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

                var existingConnectionId = FindMatchingXmiStructuralPointConnectionByPoint3d(tempConnection);
                if (existingConnectionId != null)
                {
                    return GetXmiEntitiesOfType<XmiStructuralPointConnection>()
                        .FirstOrDefault(c => c.Id == existingConnectionId) ?? tempConnection;
                }

                // Look for an existing storey with the same native identifier; fall back to the provided one if missing.
                XmiStorey? existingStorey = null;
                if (storey != null)
                {
                    existingStorey = GetXmiEntitiesOfType<XmiStorey>()
                        .FirstOrDefault(s => s.Id == storey.Id) ?? storey;
                }

                // Reuse an existing point with matching coordinates whenever possible.
                var existingPoint = GetXmiEntitiesOfType<XmiPoint3d>()
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
                    var pointRelation = new XmiHasPoint3d(connection, existingPoint);
                    AddXmiHasPoint3d(pointRelation);
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
        public List<T> GetXmiEntitiesOfType<T>() where T : XmiBaseEntity
        {
            return Entities.OfType<T>().ToList();
        }

        /// <summary>
        /// Creates a new <see cref="XmiPoint3d"/> entity, adding it to the model.
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
        public XmiPoint3d CreateXmiPoint3d(
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
                var tempPoint = new XmiPoint3d(id, name, ifcGuid, nativeId, description, x, y, z);
                var existingPoint = Entities
                    .OfType<XmiPoint3d>()
                    .FirstOrDefault(p => p.Equals(tempPoint));

                if (existingPoint != null)
                {
                    return existingPoint;
                }

                var point = new XmiPoint3d(
                    id,
                    name,
                    ifcGuid,
                    nativeId,
                    description,
                    x,
                    y,
                    z
                );

                AddXmiPoint3d(point);

                return point;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create Point3d.", ex);
            }
        }

        /// <summary>
        /// Creates a 3D line geometry and wires start/end point relationships, reusing points when possible.
        /// </summary>
        /// <returns>The created line.</returns>
        public XmiLine3d CreateXmiLine3d(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiPoint3d startPoint,
            XmiPoint3d endPoint
        )
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));

            try
            {
                var existingStartPoint = Entities
                    .OfType<XmiPoint3d>()
                    .FirstOrDefault(p => AreXmiPoint3dsEqual(p, startPoint)) ?? startPoint;
                if (!Entities.OfType<XmiPoint3d>().Any(p => ReferenceEquals(p, existingStartPoint)))
                {
                    AddXmiPoint3d(existingStartPoint);
                }

                var existingEndPoint = Entities
                    .OfType<XmiPoint3d>()
                    .FirstOrDefault(p => AreXmiPoint3dsEqual(p, endPoint)) ?? endPoint;
                if (!Entities.OfType<XmiPoint3d>().Any(p => ReferenceEquals(p, existingEndPoint)))
                {
                    AddXmiPoint3d(existingEndPoint);
                }

                var line = new XmiLine3d(
                    id,
                    name,
                    ifcGuid,
                    nativeId,
                    description,
                    existingStartPoint,
                    existingEndPoint
                );

                AddXmiLine3d(line);

                AddXmiHasPoint3d(new XmiHasPoint3d(line, existingStartPoint, XmiPoint3dTypeEnum.Start));
                AddXmiHasPoint3d(new XmiHasPoint3d(line, existingEndPoint, XmiPoint3dTypeEnum.End));

                return line;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create line geometry.", ex);
            }
        }

        /// <summary>
        /// Creates a 3D arc geometry and wires start/end/center point relationships, reusing points when possible.
        /// </summary>
        /// <returns>The created arc.</returns>
        public XmiArc3d CreateXmiArc3d(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiPoint3d startPoint,
            XmiPoint3d endPoint,
            XmiPoint3d centerPoint,
            float radius
        )
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));

            try
            {
                var existingStartPoint = Entities
                    .OfType<XmiPoint3d>()
                    .FirstOrDefault(p => AreXmiPoint3dsEqual(p, startPoint)) ?? startPoint;
                if (!Entities.OfType<XmiPoint3d>().Any(p => ReferenceEquals(p, existingStartPoint)))
                {
                    AddXmiPoint3d(existingStartPoint);
                }

                var existingEndPoint = Entities
                    .OfType<XmiPoint3d>()
                    .FirstOrDefault(p => AreXmiPoint3dsEqual(p, endPoint)) ?? endPoint;
                if (!Entities.OfType<XmiPoint3d>().Any(p => ReferenceEquals(p, existingEndPoint)))
                {
                    AddXmiPoint3d(existingEndPoint);
                }

                var existingCenterPoint = Entities
                    .OfType<XmiPoint3d>()
                    .FirstOrDefault(p => AreXmiPoint3dsEqual(p, centerPoint)) ?? centerPoint;
                if (!Entities.OfType<XmiPoint3d>().Any(p => ReferenceEquals(p, existingCenterPoint)))
                {
                    AddXmiPoint3d(existingCenterPoint);
                }

                var arc = new XmiArc3d(
                    id,
                    name,
                    ifcGuid,
                    nativeId,
                    description,
                    existingStartPoint,
                    existingEndPoint,
                    existingCenterPoint,
                    radius
                );

                AddXmiArc3d(arc);

                AddXmiHasPoint3d(new XmiHasPoint3d(arc, existingStartPoint, XmiPoint3dTypeEnum.Start));
                AddXmiHasPoint3d(new XmiHasPoint3d(arc, existingEndPoint, XmiPoint3dTypeEnum.End));
                AddXmiHasPoint3d(new XmiHasPoint3d(arc, existingCenterPoint, XmiPoint3dTypeEnum.Center));

                return arc;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create arc geometry.", ex);
            }
        }

        /// <summary>
        /// Creates a beam physical element and optionally links a material and segments.
        /// </summary>
        /// <returns>The created beam.</returns>
        public XmiBeam CreateXmiBeam(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterial? material,
            List<XmiSegment>? segments,
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
        )
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));

            try
            {
                XmiMaterial? existingMaterial = null;
                if (material != null && !string.IsNullOrEmpty(material.NativeId))
                {
                    existingMaterial = GetXmiEntitiesOfType<XmiMaterial>().FirstOrDefault(m => m.NativeId == material.NativeId) ?? material;
                }

                var beam = new XmiBeam(
                    id,
                    name,
                    ifcGuid,
                    nativeId,
                    description,
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
                    endNodeZOffset
                );

                AddXmiBeam(beam);

                if (existingMaterial != null)
                {
                    AddXmiHasMaterial(new XmiHasMaterial(beam, existingMaterial));
                }

                if (segments != null)
                {
                    var existingSegments = GetXmiEntitiesOfType<XmiSegment>();
                    foreach (var segment in segments)
                    {
                        var existingSegment = existingSegments.FirstOrDefault(s => s.NativeId == segment.NativeId) ?? segment;
                        AddXmiHasSegment(new XmiHasSegment(beam, existingSegment));
                    }
                }

                return beam;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create beam.", ex);
            }
        }

        /// <summary>
        /// Creates a column physical element and optionally links a material and segments.
        /// </summary>
        /// <returns>The created column.</returns>
        public XmiColumn CreateXmiColumn(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterial? material,
            List<XmiSegment>? segments,
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
        )
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));

            try
            {
                XmiMaterial? existingMaterial = null;
                if (material != null && !string.IsNullOrEmpty(material.NativeId))
                {
                    existingMaterial = GetXmiEntitiesOfType<XmiMaterial>().FirstOrDefault(m => m.NativeId == material.NativeId) ?? material;
                }

                var column = new XmiColumn(
                    id,
                    name,
                    ifcGuid,
                    nativeId,
                    description,
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
                    endNodeZOffset
                );

                AddXmiColumn(column);

                if (existingMaterial != null)
                {
                    AddXmiHasMaterial(new XmiHasMaterial(column, existingMaterial));
                }

                if (segments != null)
                {
                    var existingSegments = GetXmiEntitiesOfType<XmiSegment>();
                    foreach (var segment in segments)
                    {
                        var existingSegment = existingSegments.FirstOrDefault(s => s.NativeId == segment.NativeId) ?? segment;
                        AddXmiHasSegment(new XmiHasSegment(column, existingSegment));
                    }
                }

                return column;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create column.", ex);
            }
        }

        /// <summary>
        /// Creates a slab physical element and optionally links a material.
        /// </summary>
        /// <returns>The created slab.</returns>
        public XmiSlab CreateXmiSlab(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterial? material
        )
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));

            try
            {
                XmiMaterial? existingMaterial = null;
                if (material != null && !string.IsNullOrEmpty(material.NativeId))
                {
                    existingMaterial = GetXmiEntitiesOfType<XmiMaterial>().FirstOrDefault(m => m.NativeId == material.NativeId) ?? material;
                }

                var slab = new XmiSlab(
                    id,
                    name,
                    ifcGuid,
                    nativeId,
                    description
                );

                AddXmiSlab(slab);

                if (existingMaterial != null)
                {
                    AddXmiHasMaterial(new XmiHasMaterial(slab, existingMaterial));
                }

                return slab;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create slab.", ex);
            }
        }

        /// <summary>
        /// Creates a wall physical element and optionally links a material.
        /// </summary>
        /// <returns>The created wall.</returns>
        public XmiWall CreateXmiWall(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterial? material
        )
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));

            try
            {
                XmiMaterial? existingMaterial = null;
                if (material != null && !string.IsNullOrEmpty(material.NativeId))
                {
                    existingMaterial = GetXmiEntitiesOfType<XmiMaterial>().FirstOrDefault(m => m.NativeId == material.NativeId) ?? material;
                }

                var wall = new XmiWall(
                    id,
                    name,
                    ifcGuid,
                    nativeId,
                    description
                );

                AddXmiWall(wall);

                if (existingMaterial != null)
                {
                    AddXmiHasMaterial(new XmiHasMaterial(wall, existingMaterial));
                }

                return wall;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create wall.", ex);
            }
        }

        /// <summary>
        /// Creates or reuses a structural curve member, optionally wiring up material, cross-section, storey, and node relationships.
        /// </summary>
        /// <returns>The created curve member.</returns>
        public XmiStructuralCurveMember CreateXmiStructuralCurveMember(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterial? material,
            XmiCrossSection? crossSection,
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
                XmiMaterial? existingMaterial = null;
                if (material != null && !string.IsNullOrEmpty(material.NativeId))
                {
                    existingMaterial = GetXmiEntitiesOfType<XmiMaterial>().FirstOrDefault(m => m.NativeId == material.NativeId) ?? material;
                }

                XmiCrossSection? existingCrossSection = null;
                if (crossSection != null && !string.IsNullOrEmpty(crossSection.NativeId))
                {
                    var crossSections = GetXmiEntitiesOfType<XmiCrossSection>();
                    existingCrossSection = crossSections.FirstOrDefault(c => c.NativeId == crossSection.NativeId) ?? crossSection;
                }

                XmiStorey? existingStorey = null;
                if (storey != null)
                {
                    existingStorey = GetXmiEntitiesOfType<XmiStorey>()
                        .FirstOrDefault(s => s.NativeId == storey.NativeId) ?? storey;
                }

                var existingBeginNodeId = FindMatchingXmiStructuralPointConnectionByPoint3d(beginNode);
                var existingBeginNode = existingBeginNodeId != null
                    ? GetXmiEntitiesOfType<XmiStructuralPointConnection>().FirstOrDefault(n => n.Id == existingBeginNodeId) ?? beginNode
                    : beginNode;

                var existingEndNodeId = FindMatchingXmiStructuralPointConnectionByPoint3d(endNode);
                var existingEndNode = existingEndNodeId != null
                    ? GetXmiEntitiesOfType<XmiStructuralPointConnection>().FirstOrDefault(n => n.Id == existingEndNodeId) ?? endNode
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

                if (existingMaterial != null)
                {
                    var materialRelation = new XmiHasMaterial(curveMember, existingMaterial);
                    AddXmiHasMaterial(materialRelation);
                }

                if (existingCrossSection != null)
                {
                    var crossSectionRelation = new XmiHasCrossSection(curveMember, existingCrossSection);
                    AddXmiHasCrossSection(crossSectionRelation);
                }

                if (existingStorey != null)
                {
                    var storeyRelation = new XmiHasStorey(curveMember, existingStorey);
                    AddXmiHasStorey(storeyRelation);
                }

                var beginNodeRelation = new XmiHasStructuralPointConnection(curveMember, existingBeginNode);
                var endNodeRelation = new XmiHasStructuralPointConnection(curveMember, existingEndNode);
                AddXmiHasStructuralPointConnection(beginNodeRelation);
                AddXmiHasStructuralPointConnection(endNodeRelation);

                if (segments != null)
                {
                    var existingSegments = GetXmiEntitiesOfType<XmiSegment>();
                    foreach (var segment in segments)
                    {
                        var existingSegment = existingSegments.FirstOrDefault(s => s.NativeId == segment.NativeId) ?? segment;
                        AddXmiHasSegment(new XmiHasSegment(curveMember, existingSegment));
                    }
                }

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
        public XmiCrossSection CreateXmiCrossSection(
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
                    var materials = GetXmiEntitiesOfType<XmiMaterial>() ?? Enumerable.Empty<XmiMaterial>();
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
        public XmiStorey CreateXmiStorey(
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
                var existingStorey = GetXmiEntitiesOfType<XmiStorey>()
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
        public XmiMaterial CreateXmiMaterial(
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
                var existingMaterial = GetXmiEntitiesOfType<XmiMaterial>()
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
        public XmiStructuralSurfaceMember CreateXmiStructuralSurfaceMember(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterial? material,
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
                    existingStorey = GetXmiEntitiesOfType<XmiStorey>()
                        .FirstOrDefault(s => s.NativeId == storey.NativeId) ?? storey;
                }

                XmiMaterial? existingMaterial = null;

                if (material != null && !string.IsNullOrEmpty(material.NativeId))
                {
                    var materials = GetXmiEntitiesOfType<XmiMaterial>() ?? Enumerable.Empty<XmiMaterial>();
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

                if (segments != null)
                {
                    var existingSegments = GetXmiEntitiesOfType<XmiSegment>();
                    foreach (var segment in segments)
                    {
                        var existingSegment = existingSegments.FirstOrDefault(s => s.NativeId == segment.NativeId) ?? segment;
                        AddXmiHasSegment(new XmiHasSegment(surfaceMember, existingSegment));
                    }
                }

                return surfaceMember;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create structural surface member.", ex);
            }
        }

        /// <summary>
        /// Creates a line segment with associated geometry and point relationships.
        /// Creates: XmiSegment -> XmiHasGeometry -> existing XmiLine3d (with its own point relationships)
        /// </summary>
        /// <param name="id">Unique identifier for the segment.</param>
        /// <param name="name">Human readable name.</param>
        /// <param name="ifcGuid">Optional IFC GUID reference.</param>
        /// <param name="nativeId">Native identifier from the authoring system.</param>
        /// <param name="description">Optional description.</param>
        /// <param name="position">Normalized position value along the parent member (0-1).</param>
        /// <param name="line">Existing line geometry to associate.</param>
        /// <returns>The created segment entity.</returns>
        public XmiSegment CreateXmiLineSegment(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            float position,
            XmiLine3d line
        )
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));
            if (line == null) throw new ArgumentNullException(nameof(line));

            try
            {
                var existingLine = Entities
                    .OfType<XmiLine3d>()
                    .FirstOrDefault(l => (!string.IsNullOrEmpty(line.NativeId) && l.NativeId == line.NativeId) || l.Id == line.Id)
                    ?? line;

                if (!Entities.OfType<XmiLine3d>().Any(l => ReferenceEquals(l, existingLine)))
                {
                    AddXmiLine3d(existingLine);
                }

                // Ensure points exist and align with the stored line instance.
                var existingStartPoint = Entities
                    .OfType<XmiPoint3d>()
                    .FirstOrDefault(p => AreXmiPoint3dsEqual(p, existingLine.StartPoint)) ?? existingLine.StartPoint;
                if (!Entities.OfType<XmiPoint3d>().Any(p => ReferenceEquals(p, existingStartPoint)))
                {
                    AddXmiPoint3d(existingStartPoint);
                }

                var existingEndPoint = Entities
                    .OfType<XmiPoint3d>()
                    .FirstOrDefault(p => AreXmiPoint3dsEqual(p, existingLine.EndPoint)) ?? existingLine.EndPoint;
                if (!Entities.OfType<XmiPoint3d>().Any(p => ReferenceEquals(p, existingEndPoint)))
                {
                    AddXmiPoint3d(existingEndPoint);
                }

                existingLine.StartPoint = existingStartPoint;
                existingLine.EndPoint = existingEndPoint;

                // Make sure the line has point relationships (add only if missing to avoid duplicates).
                if (!Relationships.OfType<XmiHasPoint3d>().Any(r => r.Source == existingLine && r.PointType == XmiPoint3dTypeEnum.Start))
                {
                    AddXmiHasPoint3d(new XmiHasPoint3d(existingLine, existingStartPoint, XmiPoint3dTypeEnum.Start));
                }

                if (!Relationships.OfType<XmiHasPoint3d>().Any(r => r.Source == existingLine && r.PointType == XmiPoint3dTypeEnum.End))
                {
                    AddXmiHasPoint3d(new XmiHasPoint3d(existingLine, existingEndPoint, XmiPoint3dTypeEnum.End));
                }

                // Create the segment
                var segment = new XmiSegment(
                    id,
                    name,
                    ifcGuid,
                    nativeId,
                    description,
                    position,
                    XmiSegmentTypeEnum.Line
                );
                AddXmiSegment(segment);

                // Create segment -> geometry relationship
                AddXmiHasGeometry(new XmiHasGeometry(segment, existingLine));

                return segment;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create line segment.", ex);
            }
        }

        /// <summary>
        /// Creates a circular arc segment with associated geometry and point relationships.
        /// Creates: XmiSegment -> XmiHasGeometry -> existing XmiArc3d (with its own point relationships)
        /// </summary>
        /// <param name="id">Unique identifier for the segment.</param>
        /// <param name="name">Human readable name.</param>
        /// <param name="ifcGuid">Optional IFC GUID reference.</param>
        /// <param name="nativeId">Native identifier from the authoring system.</param>
        /// <param name="description">Optional description.</param>
        /// <param name="position">Normalized position value along the parent member (0-1).</param>
        /// <param name="arc">Existing arc geometry to associate.</param>
        /// <returns>The created segment entity.</returns>
        public XmiSegment CreateXmiArcSegment(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            float position,
            XmiArc3d arc
        )
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));
            if (arc == null) throw new ArgumentNullException(nameof(arc));

            try
            {
                var existingArc = Entities
                    .OfType<XmiArc3d>()
                    .FirstOrDefault(a => (!string.IsNullOrEmpty(arc.NativeId) && a.NativeId == arc.NativeId) || a.Id == arc.Id)
                    ?? arc;

                if (!Entities.OfType<XmiArc3d>().Any(a => ReferenceEquals(a, existingArc)))
                {
                    AddXmiArc3d(existingArc);
                }

                // Ensure points exist and align with the stored arc instance.
                var existingStartPoint = Entities
                    .OfType<XmiPoint3d>()
                    .FirstOrDefault(p => AreXmiPoint3dsEqual(p, existingArc.StartPoint)) ?? existingArc.StartPoint;
                if (!Entities.OfType<XmiPoint3d>().Any(p => ReferenceEquals(p, existingStartPoint)))
                {
                    AddXmiPoint3d(existingStartPoint);
                }

                var existingEndPoint = Entities
                    .OfType<XmiPoint3d>()
                    .FirstOrDefault(p => AreXmiPoint3dsEqual(p, existingArc.EndPoint)) ?? existingArc.EndPoint;
                if (!Entities.OfType<XmiPoint3d>().Any(p => ReferenceEquals(p, existingEndPoint)))
                {
                    AddXmiPoint3d(existingEndPoint);
                }

                var existingCenterPoint = Entities
                    .OfType<XmiPoint3d>()
                    .FirstOrDefault(p => AreXmiPoint3dsEqual(p, existingArc.CenterPoint)) ?? existingArc.CenterPoint;
                if (!Entities.OfType<XmiPoint3d>().Any(p => ReferenceEquals(p, existingCenterPoint)))
                {
                    AddXmiPoint3d(existingCenterPoint);
                }

                existingArc.StartPoint = existingStartPoint;
                existingArc.EndPoint = existingEndPoint;
                existingArc.CenterPoint = existingCenterPoint;

                // Create the segment
                var segment = new XmiSegment(
                    id,
                    name,
                    ifcGuid,
                    nativeId,
                    description,
                    position,
                    XmiSegmentTypeEnum.CircularArc
                );
                AddXmiSegment(segment);

                // Create segment -> geometry relationship
                AddXmiHasGeometry(new XmiHasGeometry(segment, existingArc));

                // Ensure arc has point relationships (add only if missing to avoid duplicates).
                if (!Relationships.OfType<XmiHasPoint3d>().Any(r => r.Source == existingArc && r.PointType == XmiPoint3dTypeEnum.Start))
                {
                    AddXmiHasPoint3d(new XmiHasPoint3d(existingArc, existingStartPoint, XmiPoint3dTypeEnum.Start));
                }

                if (!Relationships.OfType<XmiHasPoint3d>().Any(r => r.Source == existingArc && r.PointType == XmiPoint3dTypeEnum.End))
                {
                    AddXmiHasPoint3d(new XmiHasPoint3d(existingArc, existingEndPoint, XmiPoint3dTypeEnum.End));
                }

                if (!Relationships.OfType<XmiHasPoint3d>().Any(r => r.Source == existingArc && r.PointType == XmiPoint3dTypeEnum.Center))
                {
                    AddXmiHasPoint3d(new XmiHasPoint3d(existingArc, existingCenterPoint, XmiPoint3dTypeEnum.Center));
                }

                return segment;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create arc segment.", ex);
            }
        }
    }
}
