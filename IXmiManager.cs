using XmiSchema.Core.Entities;
using XmiSchema.Core.Geometries;
using XmiSchema.Core.Relationships;
using XmiSchema.Core.Models;

namespace XmiSchema.Core.Manager
{
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

        // 查询
        string GetMatchingPoint3DId(int modelIndex, XmiPoint3D importedPoint);
        List<T> GetEntitiesOfType<T>(int modelIndex) where T : XmiBaseEntity;
        T? GetEntityById<T>(int modelIndex, string id) where T : XmiBaseEntity;
        string? FindMatchingPointConnectionByPoint3D(int modelIndex, XmiStructuralPointConnection inputConnection);
       
        // 构建 JSON 图和保存
        string BuildJson(int modelIndex);
        void Save(string path);
    }
}
