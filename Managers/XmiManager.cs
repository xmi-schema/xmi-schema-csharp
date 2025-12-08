using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;
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
    /// Provides high-level helpers for constructing and querying <see cref="XmiModel"/> instances.
    /// </summary>
    public class XmiManager : IXmiManager
    {
        /// <inheritdoc />
        public List<XmiModel> Models { get; set; } = new();

        /// <summary>
        /// Checks whether the provided index can safely access the <see cref="Models"/> list.
        /// </summary>
        /// <param name="index">Index to validate.</param>
        /// <returns><c>true</c> when the index is within range; otherwise <c>false</c>.</returns>
        private bool IsValidModelIndex(int index) => index >= 0 && index < Models.Count;

        // ========== 添加实体 ==========
        /// <inheritdoc />
        public void AddXmiMaterialToModel(int modelIndex, XmiMaterial material)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiMaterial(material);
        }

        /// <inheritdoc />
        public void AddXmiCrossSectionToModel(int modelIndex, XmiCrossSection crossSection)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiCrossSection(crossSection);
        }

        /// <inheritdoc />
        public void AddXmiBeamToModel(int modelIndex, XmiBeam beam)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiBeam(beam);
        }

        /// <inheritdoc />
        public void AddXmiColumnToModel(int modelIndex, XmiColumn column)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiColumn(column);
        }

        /// <inheritdoc />
        public void AddXmiSlabToModel(int modelIndex, XmiSlab slab)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiSlab(slab);
        }

        /// <inheritdoc />
        public void AddXmiWallToModel(int modelIndex, XmiWall wall)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiWall(wall);
        }

        /// <inheritdoc />
        public void AddXmiStructuralCurveMemberToModel(int modelIndex, XmiStructuralCurveMember member)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiStructuralCurveMember(member);
        }

        /// <inheritdoc />
        public void AddXmiStructuralSurfaceMemberToModel(int modelIndex, XmiStructuralSurfaceMember member)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiStructuralSurfaceMember(member);
        }

        /// <inheritdoc />
        public void AddXmiStructuralPointConnectionToModel(int modelIndex, XmiStructuralPointConnection connection)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiStructuralPointConnection(connection);
        }

        /// <inheritdoc />
        public void AddXmiStoreyToModel(int modelIndex, XmiStorey storey)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiStorey(storey);
        }

        /// <inheritdoc />
        public void AddXmiPoint3DToModel(int modelIndex, XmiPoint3d point)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiPoint3d(point);
        }

        // 批量添加
        /// <inheritdoc />
        public void AddEntitiesToModel(int modelIndex, List<XmiBaseEntity> entities)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            foreach (var e in entities)
            {
                Models[modelIndex].Entities.Add(e);
            }
        }

        // ========== 添加关系 ==========
        /// <inheritdoc />
        public void AddXmiHasPoint3DToModel(int modelIndex, XmiHasPoint3d relation)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiHasPoint3D(relation);
        }

        /// <inheritdoc />
        public void AddXmiHasMaterialToModel(int modelIndex, XmiHasMaterial relation)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiHasMaterial(relation);
        }

        /// <inheritdoc />
        public void AddXmiHasStructuralPointConnectionToModel(int modelIndex, XmiHasStructuralPointConnection relation)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiHasStructuralPointConnection(relation);
        }

        /// <inheritdoc />
        public void AddXmiHasCrossSectionToModel(int modelIndex, XmiHasCrossSection relation)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiHasCrossSection(relation);
        }

        /// <inheritdoc />
        public void AddXmiHasStoreyToModel(int modelIndex, XmiHasStorey relation)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiHasStorey(relation);
        }

        /// <inheritdoc />
        public void AddXmiHasStructuralCurveMemberToModel(int modelIndex, XmiHasStructuralCurveMember relation)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            Models[modelIndex].AddXmiHasStructuralCurveMember(relation);
        }

        // ========== 查询 ==========
        // public string GetMatchingPoint3DId(int modelIndex, XmiPoint3d importedPoint)
        // {
        //     if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
        //     return Models[modelIndex].GetMatchingPoint3DId(importedPoint);
        // }

        /// <inheritdoc />
        public List<T> GetXmiEntitiesOfType<T>(int modelIndex) where T : XmiBaseEntity
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].GetXmiEntitiesOfType<T>();
        }

        /// <inheritdoc />
        public T? GetXmiEntityById<T>(int modelIndex, string id) where T : XmiBaseEntity
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].GetXmiEntitiesOfType<T>().FirstOrDefault(e => e.Id == id);
        }

        /// <inheritdoc />
        public XmiBeam CreateXmiBeam(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterial? material,
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
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].CreateXmiBeam(
                id, name, ifcGuid, nativeId, description, material,
                systemLine, length, localAxisX, localAxisY, localAxisZ,
                beginNodeXOffset, endNodeXOffset,
                beginNodeYOffset, endNodeYOffset,
                beginNodeZOffset, endNodeZOffset
            );
        }

        /// <inheritdoc />
        public XmiColumn CreateXmiColumn(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterial? material,
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
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].CreateXmiColumn(
                id, name, ifcGuid, nativeId, description, material,
                systemLine, length, localAxisX, localAxisY, localAxisZ,
                beginNodeXOffset, endNodeXOffset,
                beginNodeYOffset, endNodeYOffset,
                beginNodeZOffset, endNodeZOffset
            );
        }

        /// <inheritdoc />
        public XmiSlab CreateXmiSlab(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterial? material
        )
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].CreateXmiSlab(
                id, name, ifcGuid, nativeId, description, material
            );
        }

        /// <inheritdoc />
        public XmiWall CreateXmiWall(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterial? material
        )
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].CreateXmiWall(
                id, name, ifcGuid, nativeId, description, material
            );
        }

        /// <summary>
        /// Attempts to locate a different point connection that references an identical <see cref="XmiPoint3d"/>.
        /// </summary>
        /// <param name="modelIndex">Model index enclosing both the input and potential matches.</param>
        /// <param name="inputConnection">Point connection whose coordinates are used as the search key.</param>
        /// <returns>The identifier of the first matching connection or <c>null</c>.</returns>
        public string? FindMatchingPointConnectionByPoint3D(int modelIndex, XmiStructuralPointConnection inputConnection)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();

            var model = Models[modelIndex];

            // Step 1: 从模型中查找 inputConnection 关联的 Point3D（通过关系）
            var inputPoint = model.Relationships
                .OfType<XmiHasPoint3d>()
                .FirstOrDefault(rel => rel.Source?.Id == inputConnection.Id)
                ?.Target as XmiPoint3d;

            if (inputPoint == null) return null;

            // Step 2: 在所有其他的 PointConnection 中查找是否有相同坐标的 Point3D（也通过关系查）
            var match = model.Relationships
                .OfType<XmiHasPoint3d>()
                .Where(rel => rel.Source is XmiStructuralPointConnection && rel.Source.Id != inputConnection.Id)
                .FirstOrDefault(rel => ArePointsEqual(rel.Target as XmiPoint3d, inputPoint));

            return match?.Source?.Id;
        }

        /// <inheritdoc />
        public string? FindMatchingXmiStructuralPointConnectionByPoint3D(int modelIndex, XmiStructuralPointConnection inputConnection) =>
            FindMatchingPointConnectionByPoint3D(modelIndex, inputConnection);

        /// <summary>
        /// Compares two point instances using the provided tolerance.
        /// </summary>
        /// <param name="p1">First point to compare.</param>
        /// <param name="p2">Second point to compare.</param>
        /// <param name="tolerance">Allowed delta between coordinates.</param>
        /// <returns><c>true</c> when both points are present and within tolerance.</returns>
        private bool ArePointsEqual(XmiPoint3d? p1, XmiPoint3d? p2, double tolerance = 1e-10)
        {
            if (p1 == null || p2 == null) return false;
            return Math.Abs(p1.X - p2.X) < tolerance &&
                   Math.Abs(p1.Y - p2.Y) < tolerance &&
                   Math.Abs(p1.Z - p2.Z) < tolerance;
        }

        // ========== 创建实体 ==========
        /// <inheritdoc />
        public XmiStructuralPointConnection CreateXmiStructuralPointConnection(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiStorey? storey,
            XmiPoint3d point
        )
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].CreateXmiStructurePointConnection(
                id, name, ifcGuid, nativeId, description,
                storey, point
            );
        }

        /// <inheritdoc />
        public XmiPoint3d CreateXmiPoint3D(
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
            return Models[modelIndex].CreateXmiPoint3d(id, name, ifcGuid, nativeId, description, x, y, z);
        }

        /// <inheritdoc />
        public XmiMaterial CreateXmiMaterial(
            int modelIndex,
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
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].CreateXmiMaterial(
                id, name, ifcGuid, nativeId, description,
                materialType, grade, unitWeight,
                eModulus, gModulus, poissonRatio, thermalCoefficient
            );
        }

        /// <inheritdoc />
        public XmiCrossSection CreateXmiCrossSection(
            int modelIndex,
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
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].CreateXmiCrossSection(
                id, name, ifcGuid, nativeId, description,
                material, shape, parameters,
                area, secondMomentOfAreaXAxis, secondMomentOfAreaYAxis,
                radiusOfGyrationXAxis, radiusOfGyrationYAxis,
                elasticModulusXAxis, elasticModulusYAxis,
                plasticModulusXAxis, plasticModulusYAxis,
                torsionalConstant
            );
        }

        /// <inheritdoc />
        public XmiStorey CreateXmiStorey(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            double storeyElevation,
            double storeyMass
        )
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].CreateXmiStorey(
                id, name, ifcGuid, nativeId, description,
                storeyElevation, storeyMass
            );
        }

        /// <inheritdoc />
        public XmiStructuralCurveMember CreateXmiStructuralCurveMember(
            int modelIndex,
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
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].CreateXmiStructuralCurveMember(
                id, name, ifcGuid, nativeId, description,
                material, crossSection, storey, curveMemberType,
                nodes, segments, systemLine,
                beginNode, endNode, length,
                localAxisX, localAxisY, localAxisZ,
                beginNodeXOffset, endNodeXOffset,
                beginNodeYOffset, endNodeYOffset,
                beginNodeZOffset, endNodeZOffset,
                endFixityStart, endFixityEnd
            );
        }

        /// <inheritdoc />
        public XmiStructuralSurfaceMember CreateXmiStructuralSurfaceMember(
            int modelIndex,
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
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();
            return Models[modelIndex].CreateXmiStructuralSurfaceMember(
                id, name, ifcGuid, nativeId, description,
                material, surfaceMemberType, thickness, systemPlane,
                nodes, storey, segments,
                area, zOffset,
                localAxisX, localAxisY, localAxisZ,
                height
            );
        }

        // ========== 构建与保存 ==========
        /// <inheritdoc />
        public string BuildJson(int modelIndex)
        {
            if (!IsValidModelIndex(modelIndex)) throw new IndexOutOfRangeException();

            var model = Models[modelIndex];
            var nodes = model.Entities.Select(GetAttributes).ToList();
            var edges = model.Relationships.Select(GetAttributes).ToList();

            var graphJson = new { nodes, edges };

            return JsonConvert.SerializeObject(graphJson, Formatting.Indented);
        }

        /// <summary>
        /// Projects the public properties of the provided object into a JSON-friendly dictionary.
        /// </summary>
        /// <param name="obj">Entity or relationship to inspect.</param>
        /// <returns>Dictionary keyed by property names with primitive or ID values.</returns>
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
                    finalValue = entityRef.Id;
                }
                else if (value is IEnumerable<XmiBaseEntity> entityList)
                {
                    finalValue = entityList.Select(e => e.Id).ToList();
                }
                else if (value is IDictionary dictionary)
                {
                    finalValue = dictionary
                        .Cast<DictionaryEntry>()
                        .Where(entry => entry.Key is not null)
                        .ToDictionary(
                            entry => entry.Key!.ToString()!,
                            entry => entry.Value ?? string.Empty);
                }
                else if (value is IXmiShapeParameters shapeParameters)
                {
                    finalValue = shapeParameters.Values.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);
                }

                if (finalValue != null)
                {
                    dict[prop.Name] = finalValue;
                }
            }

            return dict;
        }

        /// <inheritdoc />
        public void Save(string path)
        {
            File.WriteAllText(path, BuildJson(0));
            Console.WriteLine($"JSON 图文件保存成功：{path}");
        }
    }
}
