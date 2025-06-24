using XmiSchema.Core.Entities;
using XmiSchema.Core.Geometries;
using XmiSchema.Core.Enums;
using XmiSchema.Core.Models;
using XmiSchema.Core.Relationships;
using Newtonsoft.Json;
using System.Reflection;

namespace XmiSchema.Core.Manager
{
    public class XmiManager : IXmiManager
    {
        public List<XmiModel> Models { get; set; } = new();

        private bool IsValidModelIndex(int index) => index >= 0 && index < Models.Count;

        // ========== 添加实体 ==========
        public void AddXmiStructuralMaterialToModel(int modelIndex, XmiStructuralMaterial material)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiStructuralMaterial(material);
        }

        public void AddXmiStructuralCrossSectionToModel(int modelIndex, XmiStructuralCrossSection crossSection)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiStructuralCrossSection(crossSection);
        }

        public void AddXmiStructuralCurveMemberToModel(int modelIndex, XmiStructuralCurveMember member)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiStructuralCurveMember(member);
        }

        public void AddXmiStructuralSurfaceMemberToModel(int modelIndex, XmiStructuralSurfaceMember member)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiStructuralSurfaceMember(member);
        }

        public void AddXmiStructuralPointConnectionToModel(int modelIndex, XmiStructuralPointConnection connection)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiStructuralPointConnection(connection);
        }

        public void AddXmiStructuralStoreyToModel(int modelIndex, XmiStructuralStorey storey)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiStructuralStorey(storey);
        }

        public void AddXmiPoint3DToModel(int modelIndex, XmiPoint3D point)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiPoint3D(point);
        }

        // 批量添加
        public void AddEntitiesToModel(int modelIndex, List<XmiBaseEntity> entities)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            foreach (var e in entities)
            {
                Models[modelIndex].Entities.Add(e);
            }
        }

        // ========== 添加关系 ==========
        public void AddXmiHasPoint3DToModel(int modelIndex, XmiHasPoint3D relation)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiHasPoint3D(relation);
        }

        public void AddXmiHasStructuralMaterialToModel(int modelIndex, XmiHasStructuralMaterial relation)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiHasStructuralMaterial(relation);
        }

        public void AddXmiHasStructuralNodeToModel(int modelIndex, XmiHasStructuralNode relation)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiHasStructuralNode(relation);
        }

        public void AddXmiHasStructuralCrossSectionToModel(int modelIndex, XmiHasStructuralCrossSection relation)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiHasStructuralCrossSection(relation);
        }

        public void AddXmiHasStoreyToModel(int modelIndex, XmiHasStructuralStorey relation)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiHasStorey(relation);
        }

        // ========== 查询 ==========
        // public string GetMatchingPoint3DId(int modelIndex, XmiPoint3D importedPoint)
        // {
        //     if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
        //     return Models[modelIndex].GetMatchingPoint3DId(importedPoint);
        // }

        public List<T> GetEntitiesOfType<T>(int modelIndex) where T : XmiBaseEntity
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].GetEntitiesOfType<T>();
        }

        public T? GetEntityById<T>(int modelIndex, string id) where T : XmiBaseEntity
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].GetEntitiesOfType<T>().FirstOrDefault(e => e.ID == id);
        }

        public string? FindMatchingPointConnectionByPoint3D(int modelIndex, XmiStructuralPointConnection inputConnection)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();

            var model = Models[modelIndex];

            // Step 1: 从模型中查找 inputConnection 关联的 Point3D（通过关系）
            var inputPoint = model.Relationships
                .OfType<XmiHasPoint3D>()
                .FirstOrDefault(rel => rel.Source?.ID == inputConnection.ID)
                ?.Target as XmiPoint3D;

            if (inputPoint == null) return null;

            // Step 2: 在所有其他的 PointConnection 中查找是否有相同坐标的 Point3D（也通过关系查）
            var match = model.Relationships
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

        // ========== 创建实体 ==========
        public XmiStructuralPointConnection CreateStructuralPointConnection(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiStructuralStorey storey,
            XmiPoint3D point
        )
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].CreateStructurePointConnection(
                id, name, ifcGuid, nativeId, description,
                storey, point
            );
        }

        public XmiPoint3D CreatePoint3D(
            int modelIndex,
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
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].CreatePoint3D(id, name, ifcGuid, nativeId, description, x, y, z);
        }

        public XmiStructuralMaterial CreateStructuralMaterial(
            int modelIndex,
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
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].CreateStructuralMaterial(
                id, name, ifcGuid, nativeId, description,
                materialType, grade, unitWeight,
                eModulus, gModulus, poissonRatio, thermalCoefficient
            );
        }

        public XmiStructuralCrossSection CreateStructuralCrossSection(
            int modelIndex,
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
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].CreateStructuralCrossSection(
                id, name, ifcGuid, nativeId, description,
                material, shape, parameters,
                area, secondMomentOfAreaXAxis, secondMomentOfAreaYAxis,
                radiusOfGyrationXAxis, radiusOfGyrationYAxis,
                elasticModulusXAxis, elasticModulusYAxis,
                plasticModulusXAxis, plasticModulusYAxis,
                torsionalConstant
            );
        }

        public XmiStructuralStorey CreateStructuralStorey(
            int modelIndex,
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
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].CreateStructuralStorey(
                id, name, ifcGuid, nativeId, description,
                storeyElevation, storeyMass,
                storeyHorizontalReactionX, storeyHorizontalReactionY, storeyVerticalReaction
            );
        }

        public XmiStructuralCurveMember CreateStructuralCurveMember(
            int modelIndex,
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
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].CreateStructuralCurveMember(
                id, name, ifcGuid, nativeId, description,
                crossSection, storey, curvememberType,
                nodes, segments, systemLine,
                beginNode, endNode, length,
                localAxisX, localAxisY, localAxisZ,
                beginNodeXOffset, endNodeXOffset,
                beginNodeYOffset, endNodeYOffset,
                beginNodeZOffset, endNodeZOffset,
                endFixityStart, endFixityEnd
            );
        }

        public XmiStructuralSurfaceMember CreateStructuralSurfaceMember(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiStructuralCrossSection crossSection,
            XmiStructuralStorey storey,
            XmiStructuralSurfaceMemberTypeEnum surfaceMemberType,
            List<XmiStructuralPointConnection> nodes,
            List<XmiSegment> segments,
            XmiStructuralSurfaceMemberSystemPlaneEnum systemPlane,
            double area,
            double zOffset,
            double thickness,
            string localAxisX,
            string localAxisY,
            string localAxisZ,
            double height
        )
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].CreateStructuralSurfaceMember(
                id, name, ifcGuid, nativeId, description,
                crossSection, storey, surfaceMemberType,
                nodes, segments, systemPlane,
                area, zOffset, thickness,
                localAxisX, localAxisY, localAxisZ,
                height
            );
        }

        // ========== 构建与保存 ==========
        public string BuildJson(int modelIndex)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();

            var model = Models[modelIndex];
            var nodes = model.Entities.Select(GetAttributes).ToList();
            var edges = model.Relationships.Select(GetAttributes).ToList();

            var graphJson = new { nodes, edges };

            return JsonConvert.SerializeObject(graphJson, Formatting.Indented);
        }

        private Dictionary<string, object> GetAttributes(object obj)
        {
            var dict = new Dictionary<string, object>();
            var props = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                           .OrderBy(p => p.MetadataToken);

            foreach (var prop in props)
            {
                var value = prop.GetValue(obj);
                if (value == null) continue;

                var type = prop.PropertyType;
                object? finalValue = null;
                if (type.IsEnum)
                {
                    var enumName = value?.ToString();
                    if (!string.IsNullOrEmpty(enumName))
                    {
                        var field = type.GetField(enumName);
                        var enumValueAttr = field?.GetCustomAttribute<EnumValueAttribute>();
                        finalValue = enumValueAttr?.Value ?? enumName;
                    }
                }
                else if (type.IsPrimitive || type == typeof(string) ||
                         type == typeof(decimal) || type == typeof(DateTime) ||
                         type == typeof(float) || type == typeof(double))
                {
                    finalValue = value;
                }
                else if (value is XmiBaseEntity entityRef)
                {
                    finalValue = entityRef.ID;
                }
                else if (value is IEnumerable<XmiBaseEntity> entityList)
                {
                    finalValue = entityList.Select(e => e.ID).ToList();
                }

                if (finalValue != null)
                {
                    dict[prop.Name] = finalValue;
                }
            }

            return dict;
        }

        public void Save(string path)
        {
            File.WriteAllText(path, BuildJson(0));
            Console.WriteLine($"JSON 图文件保存成功：{path}");
        }
    }
}

