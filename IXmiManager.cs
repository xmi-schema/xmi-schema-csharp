using XmiSchema.Core.Entities;
using XmiSchema.Core.Relationships;
using XmiSchema.Core.Models;
using XmiSchema.Core.Geometries;

namespace XmiSchema.Core.Manager
{
    public interface IXmiManager
    {
        List<XmiModel> Models { get; set; }

        void AddEntitiesToModel(int modelIndex, List<XmiBaseEntity> entities);
        void AddXmiPoint3DToModel(int modelIndex, XmiPoint3D xmiPoint3D);
        void AddXmiStructuralMaterialToModel(int modelIndex, XmiStructuralMaterial xmiStructuralMaterial);
        void AddXmiStructuralCrossSectionToModel(int modelIndex, XmiStructuralCrossSection xmiStructuralCrossSection);
        void AddXmiStructuralCurveMemberToModel(int modelIndex, XmiStructuralCurveMember xmiStructuralCurveMember);
        void AddXmiStructuralPointConnectionToModel(int modelIndex, XmiStructuralPointConnection xmiStructuralPointConnection);
        void AddXmiStructuralStoreyToModel(int modelIndex, XmiStructuralStorey xmiStructuralStorey);
        void AddXmiStructuralSurfaceMemberToModel(int modelIndex, XmiStructuralSurfaceMember xmiStructuralSurfaceMember);

        void AddXmiHasPoint3DToModel(int modelIndex, XmiHasPoint3D xmiHasPoint3D);
        void AddXmiHasStructuralMaterialToModel(int modelIndex, XmiHasStructuralMaterial xmiHasStructuralMaterial);
        void AddXmiHasStructuralNodeToModel(int modelIndex, XmiHasStructuralNode xmiHasStructuralNode);
        void AddXmiHasStructuralCrossSectionToModel(int modelIndex, XmiHasStructuralCrossSection xmiHasStructuralCrossSection);
        void AddXmiHasStoreyToModel(int modelIndex, XmiHasStructuralStorey xmiHasStructuralStorey);

        List<T> GetEntitiesOfType<T>(int modelIndex) where T : XmiBaseEntity;

        string BuildJson(int modelIndex);
        void Save(string path);
    }
}
