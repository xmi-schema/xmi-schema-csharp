using XmiSchema.Core.Entities;
using XmiSchema.Core.Relationships;
using XmiSchema.Core.Geometries;
using XmiSchema.Core.Enums;

namespace XmiSchema.Core.Models
{
    /// <summary>
    /// Represents an in-memory Cross Model Information graph including all entities and relationships.
    /// </summary>
    public class XmiModel
    {
        public List<XmiBaseEntity> Entities { get; set; } = new();
        public List<XmiBaseRelationship> Relationships { get; set; } = new();

        // 添加不同类型的实体
        /// <summary>
        /// Adds a structural material entity to the model.
        /// </summary>
        /// <param name="material">Material to insert.</param>
        public void AddXmiStructuralMaterial(XmiStructuralMaterial material)
        {
            Entities.Add(material);
        }

        /// <summary>
        /// Adds a structural cross-section entity to the model.
        /// </summary>
        /// <param name="crossSection">Cross-section instance.</param>
        public void AddXmiStructuralCrossSection(XmiStructuralCrossSection crossSection)
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
        public void AddXmiStructuralStorey(XmiStructuralStorey storey)
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

        // 添加不同类型的关系
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
        public void AddXmiHasStructuralMaterial(XmiHasStructuralMaterial relation)
        {
            Relationships.Add(relation);
        }

        /// <summary>
        /// Adds a structural node relationship to the model.
        /// </summary>
        /// <param name="relation">Relationship instance.</param>
        public void AddXmiHasStructuralNode(XmiHasStructuralNode relation)
        {
            Relationships.Add(relation);
        }

        /// <summary>
        /// Adds a cross-section relationship to the model.
        /// </summary>
        /// <param name="relation">Relationship instance.</param>
        public void AddXmiHasStructuralCrossSection(XmiHasStructuralCrossSection relation)
        {
            Relationships.Add(relation);
        }

        /// <summary>
        /// Adds a storey relationship to the model.
        /// </summary>
        /// <param name="relation">Relationship instance.</param>
        public void AddXmiHasStorey(XmiHasStructuralStorey relation)
        {
            Relationships.Add(relation);
        }

        // 查询
        /// <summary>
        /// Finds a structural point connection that references the same physical point as the provided connection.
        /// </summary>
        /// <param name="inputConnection">Connection to match.</param>
        /// <returns>The ID for the matching connection if found; otherwise null.</returns>
        public string? FindMatchingPointConnectionByPoint3D(XmiStructuralPointConnection inputConnection)
        {
            // Step 1: 从模型中查找 inputConnection 关联的 Point3D（通过关系）
            var inputPoint = Relationships
                .OfType<XmiHasPoint3D>()
                .FirstOrDefault(rel => rel.Source?.ID == inputConnection.ID)
                ?.Target as XmiPoint3D;

            if (inputPoint == null) return null;

            // Step 2: 在所有其他的 PointConnection 中查找是否有相同坐标的 Point3D（也通过关系查）
            var match = Relationships
                .OfType<XmiHasPoint3D>()
                .Where(rel => rel.Source is XmiStructuralPointConnection && rel.Source.ID != inputConnection.ID)
                .FirstOrDefault(rel => ArePointsEqual(rel.Target as XmiPoint3D, inputPoint));

            return match?.Source?.ID;
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
        /// <param name="storey">Storey containing the connection.</param>
        /// <param name="point">Point geometry representing the node.</param>
        /// <returns>An existing or newly created connection.</returns>
        public XmiStructuralPointConnection CreateStructurePointConnection(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiStructuralStorey storey,
            XmiPoint3D point
        )
        {
            // 验证参数
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));
            // if (storey == null) throw new ArgumentNullException(nameof(storey), "Storey cannot be null");
            // if (point == null) throw new ArgumentNullException(nameof(point), "Point cannot be null");

            // 创建临时点连接对象用于检查
            var tempConnection = new XmiStructuralPointConnection(
                id,
                name,
                ifcGuid,
                nativeId,
                description
            // storey,
            // point
            );

            // 检查是否存在具有相同点的连接
            var existingConnectionId = FindMatchingPointConnectionByPoint3D(tempConnection);
            if (existingConnectionId != null)
            {
                // 如果找到匹配的连接，返回已存在的连接
                return GetEntitiesOfType<XmiStructuralPointConnection>()
                    .FirstOrDefault(c => c.ID == existingConnectionId) ?? tempConnection;
            }

            // 检查是否存在具有相同nativeId的楼层
            var existingStorey = GetEntitiesOfType<XmiStructuralStorey>()
                .FirstOrDefault(s => s.NativeId == storey.NativeId);

            // 如果找不到相同nativeId的楼层，使用传入的楼层
            existingStorey ??= storey;

            // 检查是否存在具有相同坐标的点
            var existingPoints = GetEntitiesOfType<XmiPoint3D>();
            var existingPoint = existingPoints.FirstOrDefault(p => p.Equals(point));

            // 如果找不到匹配的点，使用传入的点
            existingPoint ??= point;

            // 创建点连接对象
            var connection = new XmiStructuralPointConnection(
                id,
                name,
                ifcGuid,
                nativeId,
                description//,
                           // existingStorey,
                           // existingPoint
            );

            // 添加到模型
            AddXmiStructuralPointConnection(connection);

            // 创建并添加关系
            var storeyRelation = new XmiHasStructuralStorey(connection, existingStorey);
            AddXmiHasStorey(storeyRelation);

            // 只有当找到已存在的点时才创建点关系
            if (existingPoint != null)
            {
                var pointRelation = new XmiHasPoint3D(connection, existingPoint);
                AddXmiHasPoint3D(pointRelation);
            }

            return connection;
        }

        /// <summary>
        /// Returns all entities of the requested type.
        /// </summary>
        /// <typeparam name="T">Subtype of <see cref="XmiBaseEntity"/> to retrieve.</typeparam>
        public List<T> GetEntitiesOfType<T>() where T : XmiBaseEntity
        {
            return Entities.OfType<T>().ToList();
        }

        // 创建点的方法
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
            // 验证参数
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));

            // 创建临时点对象用于检查是否已存在
            var tempPoint = new XmiPoint3D(id, name, ifcGuid, nativeId, description, x, y, z);

            // 检查是否已存在相同坐标的点
            var existingPoints = GetEntitiesOfType<XmiPoint3D>();
            var existingPoint = existingPoints.FirstOrDefault(p => p.Equals(tempPoint));

            if (existingPoint != null)
            {
                // 如果找到匹配的点，返回已存在的点
                return existingPoint;
            }

            // 如果不存在相同坐标的点，创建新点并添加到模型
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

            // 添加到模型
            AddXmiPoint3D(point);

            return point;
        }

        public XmiStructuralCurveMember CreateStructuralCurveMember(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiStructuralCrossSection crossSection,
            XmiStructuralStorey storey,
            XmiStructuralCurveMemberTypeEnum curvememberType,
            List<XmiStructuralPointConnection> nodes,
            List<XmiSegment>? segments,
            XmiStructuralCurveMemberSystemLineEnum systemLine,
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
            // 验证参数
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));
            // if (crossSection == null) throw new ArgumentNullException(nameof(crossSection), "CrossSection cannot be null");
            // if (storey == null) throw new ArgumentNullException(nameof(storey), "Storey cannot be null");
            // if (nodes == null || !nodes.Any()) throw new ArgumentException("Nodes cannot be null or empty", nameof(nodes));
            // if (segments == null || !segments.Any()) throw new ArgumentException("Segments cannot be null or empty", nameof(segments));
            // if (beginNode == null) throw new ArgumentNullException(nameof(beginNode), "BeginNode cannot be null");
            // if (endNode == null) throw new ArgumentNullException(nameof(endNode), "EndNode cannot be null");
            // if (length <= 0) throw new ArgumentException("Length must be greater than 0", nameof(length));

            // 检查是否存在具有相同nativeId的截面
            var existingCrossSection = GetEntitiesOfType<XmiStructuralCrossSection>()
                .FirstOrDefault(c => c.NativeId == crossSection.NativeId);

            // 如果找不到相同nativeId的截面，使用传入的截面
            existingCrossSection ??= crossSection;

            // 检查是否存在具有相同nativeId的楼层
            var existingStorey = GetEntitiesOfType<XmiStructuralStorey>()
                .FirstOrDefault(s => s.NativeId == storey.NativeId);

            // 如果找不到相同nativeId的楼层，使用传入的楼层
            existingStorey ??= storey;

            // 检查起始节点是否存在相同点的连接
            var existingBeginNodeId = FindMatchingPointConnectionByPoint3D(beginNode);
            var existingBeginNode = existingBeginNodeId != null
                ? GetEntitiesOfType<XmiStructuralPointConnection>().FirstOrDefault(n => n.ID == existingBeginNodeId)??beginNode
                : beginNode;

            // 检查结束节点是否存在相同点的连接
            var existingEndNodeId = FindMatchingPointConnectionByPoint3D(endNode);
            var existingEndNode = existingEndNodeId != null
                ? GetEntitiesOfType<XmiStructuralPointConnection>().FirstOrDefault(n => n.ID == existingEndNodeId)??endNode
                : endNode;

            // 创建构件对象
            var curveMember = new XmiStructuralCurveMember(
                id,
                name,
                ifcGuid,
                nativeId,
                description,
                // existingCrossSection,
                // existingStorey,
                curvememberType,
                // nodes,
                // segments,
                systemLine,
                // existingBeginNode,
                // existingEndNode,
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

            // 添加到模型
            AddXmiStructuralCurveMember(curveMember);

            // 创建并添加关系
            var crossSectionRelation = new XmiHasStructuralCrossSection(curveMember, existingCrossSection);
            var storeyRelation = new XmiHasStructuralStorey(curveMember, existingStorey);
            var beginNodeRelation = new XmiHasStructuralNode(curveMember, existingBeginNode);
            var endNodeRelation = new XmiHasStructuralNode(curveMember, existingEndNode);

            AddXmiHasStructuralCrossSection(crossSectionRelation);
            AddXmiHasStorey(storeyRelation);
            AddXmiHasStructuralNode(beginNodeRelation);
            AddXmiHasStructuralNode(endNodeRelation);

            return curveMember;
        }

        public XmiStructuralCrossSection CreateStructuralCrossSection(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiStructuralMaterial? material,
            XmiShapeEnum shape,
            string[] parameters,
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

            XmiStructuralMaterial? existingMaterial = null;

            // ✅ 安全处理：仅在 NativeId 有效时查找或创建材料引用关系
            if (material != null && !string.IsNullOrEmpty(material.NativeId))
            {
                var materials = GetEntitiesOfType<XmiStructuralMaterial>() ?? Enumerable.Empty<XmiStructuralMaterial>();
                existingMaterial = materials.FirstOrDefault(m => m?.NativeId == material.NativeId) ?? material;
            }

            // ✅ 创建截面
            var crossSection = new XmiStructuralCrossSection(
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

            AddXmiStructuralCrossSection(crossSection);

            // ✅ 创建关系（仅在 existingMaterial 有效时）
            if (existingMaterial != null)
            {
                var materialRelation = new XmiHasStructuralMaterial(crossSection, existingMaterial);
                AddXmiHasStructuralMaterial(materialRelation);
            }

            return crossSection;
        }



        public XmiStructuralStorey CreateStructuralStorey(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            double storeyElevation,
            double storeyMass,
            string storeyHorizontalReactionX,
            string storeyHorizontalReactionY,
            string storeyVerticalReaction
        )
        {
            // 验证参数
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));

            // 检查是否存在具有相同nativeId的楼层
            var existingStorey = GetEntitiesOfType<XmiStructuralStorey>()
                .FirstOrDefault(s => s.NativeId == nativeId);

            if (existingStorey != null)
            {
                // 如果找到匹配的楼层，返回已存在的楼层
                return existingStorey;
            }

            // 创建楼层对象
            var storey = new XmiStructuralStorey(
                id,
                name,
                ifcGuid,
                nativeId,
                description,
                storeyElevation,
                storeyMass,
                storeyHorizontalReactionX,
                storeyHorizontalReactionY,
                storeyVerticalReaction
            );

            // 添加到模型
            AddXmiStructuralStorey(storey);

            return storey;
        }

        public XmiStructuralMaterial CreateStructuralMaterial(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiStructuralMaterialTypeEnum materialType,
            double grade,
            double unitWeight,
            string eModulus,
            string gModulus,
            string poissonRatio,
            double thermalCoefficient
        )
        {
            // 验证参数
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));

            // 检查是否存在具有相同nativeId的材料
            var existingMaterial = GetEntitiesOfType<XmiStructuralMaterial>()
                .FirstOrDefault(m => m.NativeId == nativeId);

            if (existingMaterial != null)
            {
                // 如果找到匹配的材料，返回已存在的材料
                return existingMaterial;
            }

            // 创建材料对象
            var material = new XmiStructuralMaterial(
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

            // 添加到模型
            AddXmiStructuralMaterial(material);

            return material;
        }

        public XmiStructuralSurfaceMember CreateStructuralSurfaceMember(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiStructuralMaterial material,
            XmiStructuralSurfaceMemberTypeEnum surfaceMemberType,
            double thickness,
            XmiStructuralSurfaceMemberSystemPlaneEnum systemPlane,
            List<XmiStructuralPointConnection> nodes,
            XmiStructuralStorey storey,
            List<XmiSegment> segments,
            double area,
            double zOffset,
            string localAxisX,
            string localAxisY,
            string localAxisZ,
            double height
        )
        {
            // 验证参数
            // if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));
            // if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));
            // if (crossSection == null) throw new ArgumentNullException(nameof(crossSection), "CrossSection cannot be null");
            // // if (storey == null) throw new ArgumentNullException(nameof(storey), "Storey cannot be null");
            // if (nodes == null || !nodes.Any()) throw new ArgumentException("Nodes cannot be null or empty", nameof(nodes));
            // if (segments == null || !segments.Any()) throw new ArgumentException("Segments cannot be null or empty", nameof(segments));
            // if (thickness <= 0) throw new ArgumentException("Thickness must be greater than 0", nameof(thickness));

            // 检查是否存在具有相同nativeId的楼层
            var existingStorey = GetEntitiesOfType<XmiStructuralStorey>()
                .FirstOrDefault(s => s.NativeId == storey.NativeId);

            // 如果找不到相同nativeId的楼层，使用传入的楼层
            existingStorey ??= storey;

            XmiStructuralMaterial? existingMaterial = null;

            // ✅ 安全处理：仅在 NativeId 有效时查找或创建材料引用关系
            if (material != null && !string.IsNullOrEmpty(material.NativeId))
            {
                var materials = GetEntitiesOfType<XmiStructuralMaterial>() ?? Enumerable.Empty<XmiStructuralMaterial>();
                existingMaterial = materials.FirstOrDefault(m => m?.NativeId == material.NativeId) ?? material;
            }

            // 检查所有节点是否存在相同点的连接
            var existingNodes = nodes.Select(node =>
            {
                var existingNodeId = FindMatchingPointConnectionByPoint3D(node);
                return existingNodeId != null
                    ? GetEntitiesOfType<XmiStructuralPointConnection>().FirstOrDefault(n => n.ID == existingNodeId)
                    : node;
            }).ToList();

            // 创建构件对象
            var surfaceMember = new XmiStructuralSurfaceMember(
                id,
                name,
                ifcGuid,
                nativeId,
                description,
                // existingCrossSection.Material,
                surfaceMemberType,
                thickness,
                systemPlane,
                // existingNodes,
                // existingStorey,
                // segments,
                area,
                zOffset,
                localAxisX,
                localAxisY,
                localAxisZ,
                height
            );

            // 添加到模型
            AddXmiStructuralSurfaceMember(surfaceMember);

            // 创建并添加关系

                        // ✅ 创建关系（仅在 existingMaterial 有效时）
            if (existingMaterial != null)
            {
                var materialRelation = new XmiHasStructuralMaterial(surfaceMember, existingMaterial);
                AddXmiHasStructuralMaterial(materialRelation);
            }

            var storeyRelation = new XmiHasStructuralStorey(surfaceMember, existingStorey);

            AddXmiHasStorey(storeyRelation);

            return surfaceMember;
        }
    }
}
