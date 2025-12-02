using XmiSchema.Core.Entities;
using XmiSchema.Core.Geometries;
using XmiSchema.Core.Relationships;
using XmiSchema.Core.Models;
using XmiSchema.Core.Enums;

namespace XmiSchema.Core.Manager
{
    /// <summary>
    /// Contract for components that manage Cross Model Information graphs and helper creation routines.
    /// </summary>
    public interface IXmiManager
    {
        List<XmiModel> Models { get; set; }

        // 添加实体方法
        void AddXmiStructuralMaterialToModel(int modelIndex, XmiStructuralMaterial material);
        void AddXmiStructuralCrossSectionToModel(int modelIndex, XmiStructuralCrossSection crossSection);
        void AddXmiStructuralCurveMemberToModel(int modelIndex, XmiStructuralCurveMember member);
        void AddXmiStructuralSurfaceMemberToModel(int modelIndex, XmiStructuralSurfaceMember member);
        void AddXmiStructuralPointConnectionToModel(int modelIndex, XmiStructuralPointConnection connection);
        void AddXmiStructuralStoreyToModel(int modelIndex, XmiStructuralStorey storey);
        void AddXmiPoint3DToModel(int modelIndex, XmiPoint3D point);
        void AddEntitiesToModel(int modelIndex, List<XmiBaseEntity> entities);

        // 添加关系方法
        void AddXmiHasPoint3DToModel(int modelIndex, XmiHasPoint3D relation);
        void AddXmiHasStructuralMaterialToModel(int modelIndex, XmiHasStructuralMaterial relation);
        void AddXmiHasStructuralNodeToModel(int modelIndex, XmiHasStructuralNode relation);
        void AddXmiHasStructuralCrossSectionToModel(int modelIndex, XmiHasStructuralCrossSection relation);
        void AddXmiHasStoreyToModel(int modelIndex, XmiHasStructuralStorey relation);

        // 查询方法
        // string GetMatchingPoint3DId(int modelIndex, XmiPoint3D importedPoint);
        List<T> GetEntitiesOfType<T>(int modelIndex) where T : XmiBaseEntity;
        T? GetEntityById<T>(int modelIndex, string id) where T : XmiBaseEntity;
        // string? FindMatchingPointConnectionByPoint3D(int modelIndex, XmiStructuralPointConnection inputConnection);

        // 创建实体方法
        XmiPoint3D CreatePoint3D(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            double x,
            double y,
            double z
        );

        XmiStructuralMaterial CreateStructuralMaterial(
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
        );

        XmiStructuralCrossSection CreateStructuralCrossSection(
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
        );

        XmiStructuralStorey CreateStructuralStorey(
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
        );

        XmiStructuralCurveMember CreateStructuralCurveMember(
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
        );

        XmiStructuralSurfaceMember CreateStructuralSurfaceMember(
            int modelIndex,
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
        );

        XmiStructuralPointConnection CreateStructuralPointConnection(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiStructuralStorey storey,
            XmiPoint3D point
        );

        // 构建与保存
        string BuildJson(int modelIndex);
        void Save(string path);
    }
}
