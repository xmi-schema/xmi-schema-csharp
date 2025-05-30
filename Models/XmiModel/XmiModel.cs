using XmiSchema.Core.Entities;
using XmiSchema.Core.Relationships;
using XmiSchema.Core.Geometries;

namespace XmiSchema.Core.Models
{
    public class XmiModel
    {
        public List<XmiBaseEntity> Entities { get; set; } = new();
        public List<XmiBaseRelationship> Relationships { get; set; } = new();

        // 添加不同类型的实体
        public void AddXmiStructuralMaterial(XmiStructuralMaterial material)
        {
            Entities.Add(material);
        }

        public void AddXmiStructuralCrossSection(XmiStructuralCrossSection crossSection)
        {
            Entities.Add(crossSection);
        }

        public void AddXmiStructuralCurveMember(XmiStructuralCurveMember member)
        {
            Entities.Add(member);
        }

        public void AddXmiStructuralSurfaceMember(XmiStructuralSurfaceMember member)
        {
            Entities.Add(member);
        }

        public void AddXmiStructuralPointConnection(XmiStructuralPointConnection connection)
        {
            Entities.Add(connection);
        }

        public void AddXmiStructuralStorey(XmiStructuralStorey storey)
        {
            Entities.Add(storey);
        }

        public void AddXmiPoint3D(XmiPoint3D point)
        {
            Entities.Add(point);
        }

        // 添加不同类型的关系
        public void AddXmiHasPoint3D(XmiHasPoint3D relation)
        {
            Relationships.Add(relation);
        }

        public void AddXmiHasStructuralMaterial(XmiHasStructuralMaterial relation)
        {
            Relationships.Add(relation);
        }

        public void AddXmiHasStructuralNode(XmiHasStructuralNode relation)
        {
            Relationships.Add(relation);
        }

        public void AddXmiHasStructuralCrossSection(XmiHasStructuralCrossSection relation)
        {
            Relationships.Add(relation);
        }

        public void AddXmiHasStorey(XmiHasStructuralStorey relation)
        {
            Relationships.Add(relation);
        }

        // 查询
        public string GetMatchingPoint3DId(XmiPoint3D importedPoint)
        {
            return Entities
                .OfType<XmiPoint3D>()
                .FirstOrDefault(p => p.X == importedPoint.X && p.Y == importedPoint.Y && p.Z == importedPoint.Z)?.ID;
        }

        public List<T> GetEntitiesOfType<T>() where T : XmiBaseEntity
        {
            return Entities.OfType<T>().ToList();
        }
    }
}
