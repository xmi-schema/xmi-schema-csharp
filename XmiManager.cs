using XmiSchema.Core.Entities;
using XmiSchema.Core.Geometries;
using XmiSchema.Core.Enums;
using XmiSchema.Core.Models;
using XmiSchema.Core.Relationships;
using Newtonsoft.Json;
using System.Reflection;


namespace XmiSchema.Core.Manager;

public class XmiManager : IXmiManager
{
    public List<XmiModel> Models { get; set; }

    public XmiManager()
    {
        Models = new List<XmiModel>();
    }

    // 添加一个实体到指定索引的模型
    public void AddXmiStructuralMaterialToModel(int modelIndex, XmiStructuralMaterial xmiStructuralMaterial)
    {
        if (IsValidModelIndex(modelIndex))
        {
            Models[modelIndex].Entities.Add(xmiStructuralMaterial);
        }
        else
        {
            throw new IndexOutOfRangeException("模型索引无效");
        }
    }
    public void AddXmiStructuralCrossSectionToModel(int modelIndex, XmiStructuralCrossSection xmiStructuralCrossSection)
    {
        if (IsValidModelIndex(modelIndex))
        {
            Models[modelIndex].Entities.Add(xmiStructuralCrossSection);
        }
        else
        {
            throw new IndexOutOfRangeException("模型索引无效");
        }
    }
    public void AddXmiStructuralCurveMemberToModel(int modelIndex, XmiStructuralCurveMember xmiStructuralCurveMember)
    {
        if (IsValidModelIndex(modelIndex))
        {
            Models[modelIndex].Entities.Add(xmiStructuralCurveMember);
        }
        else
        {
            throw new IndexOutOfRangeException("模型索引无效");
        }
    }
    public void AddXmiStructuralPointConnectionToModel(int modelIndex, XmiStructuralPointConnection xmiStructuralPointConnection)
    {
        if (IsValidModelIndex(modelIndex))
        {
            Models[modelIndex].Entities.Add(xmiStructuralPointConnection);
        }
        else
        {
            throw new IndexOutOfRangeException("模型索引无效");
        }
    }
    public void AddXmiStructuralStoreyToModel(int modelIndex, XmiStructuralStorey xmiStructuralStorey)
    {
        if (IsValidModelIndex(modelIndex))
        {
            Models[modelIndex].Entities.Add(xmiStructuralStorey);
        }
        else
        {
            throw new IndexOutOfRangeException("模型索引无效");
        }
    }
    public void AddXmiPoint3DToModel(int modelIndex, XmiPoint3D xmiPoint3D)
    {
        if (IsValidModelIndex(modelIndex))
        {
            Models[modelIndex].Entities.Add(xmiPoint3D);
        }
        else
        {
            throw new IndexOutOfRangeException("模型索引无效");
        }
    }
    public void AddXmiStructuralSurfaceMemberToModel(int modelIndex, XmiStructuralSurfaceMember xmiStructuralSurfaceMember)
    {
        if (IsValidModelIndex(modelIndex))
        {
            Models[modelIndex].Entities.Add(xmiStructuralSurfaceMember);
        }
        else
        {
            throw new IndexOutOfRangeException("模型索引无效");
        }
    }



    // 添加多个实体到指定索引的模型
    public void AddEntitiesToModel(int modelIndex, List<XmiBaseEntity> entities)
    {
        if (IsValidModelIndex(modelIndex))
        {
            Models[modelIndex].Entities.AddRange(entities);
        }
        else
        {
            throw new IndexOutOfRangeException("模型索引无效");
        }
    }


    // 查询实体从指定索引的模型
    public string GetMatchingPoint3DId(int modelIndex, XmiPoint3D importedPoint)
    {
        if (!IsValidModelIndex(modelIndex))
        {
            throw new IndexOutOfRangeException("模型索引无效");
        }

        foreach (var entity in Models[modelIndex].Entities)
        {
            if (entity is XmiPoint3D point)
            {
                if (point.X == importedPoint.X &&
                    point.Y == importedPoint.Y &&
                    point.Z == importedPoint.Z)
                {
                    return point.ID;
                }
            }
        }

        return null; // 或者也可以 return "-1"; 根据业务需要
    }



    // 构建关系


    // 添加一个关系到指定索引的模型
    public void AddXmiHasPoint3DToModel(int modelIndex, XmiHasPoint3D xmiHasPoint3D)
    {
        if (IsValidModelIndex(modelIndex))
        {
            Models[modelIndex].Relationships.Add(xmiHasPoint3D);
        }
        else
        {
            throw new IndexOutOfRangeException("模型索引无效");
        }
    }
    public void AddXmiHasStructuralMaterialToModel(int modelIndex, XmiHasStructuralMaterial xmiHasStructuralMaterial)
    {
        if (IsValidModelIndex(modelIndex))
        {
            Models[modelIndex].Relationships.Add(xmiHasStructuralMaterial);
        }
        else
        {
            throw new IndexOutOfRangeException("模型索引无效");
        }
    }

    public void AddXmiHasStructuralNodeToModel(int modelIndex, XmiHasStructuralNode xmiHasStructuralNode)
    {
        if (IsValidModelIndex(modelIndex))
        {
            Models[modelIndex].Relationships.Add(xmiHasStructuralNode);
        }
        else
        {
            throw new IndexOutOfRangeException("模型索引无效");
        }
    }

    public void AddXmiHasStructuralCrossSectionToModel(int modelIndex, XmiHasStructuralCrossSection xmiHasStructuralCrossSection)
    {
        if (IsValidModelIndex(modelIndex))
        {
            Models[modelIndex].Relationships.Add(xmiHasStructuralCrossSection);
        }
        else
        {
            throw new IndexOutOfRangeException("模型索引无效");
        }
    }

    public void AddXmiHasStoreyToModel(int modelIndex, XmiHasStructuralStorey xmiHasStructuralStorey)
    {
        if (IsValidModelIndex(modelIndex))
        {
            Models[modelIndex].Relationships.Add(xmiHasStructuralStorey);
        }
        else
        {
            throw new IndexOutOfRangeException("模型索引无效");
        }
    }

    
    // 泛型查询

    public List<T> GetEntitiesOfType<T>(int modelIndex) where T : XmiBaseEntity
    {
        if (IsValidModelIndex(modelIndex))
        {
            return Models[modelIndex].Entities
                .OfType<T>()
                .ToList();
        }
        else
        {
            throw new IndexOutOfRangeException("模型索引无效");
        }
    }

    // 私有辅助方法：验证索引是否在范围内
    private bool IsValidModelIndex(int index)
    {
        return index >= 0 && index < Models.Count;
    }



    public string BuildJson(int modelIndex)
    {
        var nodes = Models[modelIndex].Entities
            .Select(e => GetAttributes(e))
            .ToList();

        var edges = Models[modelIndex].Relationships
            .Select(r => GetAttributes(r))
            .ToList();

        var graphJson = new
        {
            nodes,
            edges
        };

        return JsonConvert.SerializeObject(graphJson, Formatting.Indented);
    }

    private Dictionary<string, object> GetAttributes(object obj)
    {
        var dict = new Dictionary<string, object>();

        var props = obj.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .OrderBy(p => p.MetadataToken); // 按声明顺序输出

        foreach (var prop in props)
        {
            var value = prop.GetValue(obj);
            if (value == null) continue;

            var type = prop.PropertyType;
            object finalValue = null;

            if (type.IsEnum)
            {
                var enumName = value.ToString();
                if (!string.IsNullOrEmpty(enumName))
                {
                    var field = type.GetField(enumName);
                    var enumValueAttr = field?.GetCustomAttribute<EnumValueAttribute>();
                    finalValue = enumValueAttr?.Value ?? enumName;
                }
                else
                {
                    finalValue = value;
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
        // return
        return dict;
    }
    public void Save(string path)
    {
        // 目前默认为0
        File.WriteAllText(path, BuildJson(0));
        Console.WriteLine($"JSON 图文件保存成功：{path}");
    }
}
