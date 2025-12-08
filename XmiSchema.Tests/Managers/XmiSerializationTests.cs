using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using XmiSchema.Entities.Physical;
using XmiSchema.Entities.StructuralAnalytical;
using XmiSchema.Entities.Bases;
using XmiSchema.Parameters;
using XmiSchema.Entities.Relationships;
using XmiSchema.Entities.Geometries;
using XmiSchema.Entities.Commons;
using XmiSchema.Enums;
using XmiSchema.Managers;
namespace XmiSchema.Tests.Managers;

/// <summary>
/// Validates JSON serialization and deserialization of all XMI entities, relationships, and models.
/// </summary>
public class XmiSerializationTests
{
    private readonly JsonSerializerSettings _settings;

    public XmiSerializationTests()
    {
        _settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter> { new StringEnumConverter() }
        };
    }

    #region Basic Entity Serialization Tests

    [Fact]
    public void XmiMaterial_SerializesToJson()
    {
        var material = TestModelFactory.CreateMaterial();
        var json = JsonConvert.SerializeObject(material, _settings);

        Assert.NotNull(json);
        Assert.Contains("\"MaterialType\"", json);
        Assert.Contains("\"Steel\"", json); // Enum should be string
        Assert.Contains("\"EModulus\"", json); // Custom property name
        Assert.Contains("\"200000\"", json);
    }

    [Fact]
    public void XmiMaterial_DeserializesFromJson()
    {
        var material = TestModelFactory.CreateMaterial();
        var json = JsonConvert.SerializeObject(material, _settings);
        var deserialized = JsonConvert.DeserializeObject<XmiMaterial>(json, _settings);

        Assert.NotNull(deserialized);
        Assert.Equal(material.Id, deserialized!.Id);
        Assert.Equal(material.MaterialType, deserialized.MaterialType);
        Assert.Equal(material.ElasticModulus, deserialized.ElasticModulus);
        Assert.Equal(material.ShearModulus, deserialized.ShearModulus);
    }

    [Fact]
    public void XmiMaterial_RoundTrip_PreservesAllProperties()
    {
        var original = TestModelFactory.CreateMaterial();
        var json = JsonConvert.SerializeObject(original, _settings);
        var deserialized = JsonConvert.DeserializeObject<XmiMaterial>(json, _settings);

        Assert.Equal(original.Id, deserialized!.Id);
        Assert.Equal(original.Name, deserialized.Name);
        Assert.Equal(original.IfcGuid, deserialized.IfcGuid);
        Assert.Equal(original.NativeId, deserialized.NativeId);
        Assert.Equal(original.Description, deserialized.Description);
        Assert.Equal(original.MaterialType, deserialized.MaterialType);
        Assert.Equal(original.Grade, deserialized.Grade);
        Assert.Equal(original.UnitWeight, deserialized.UnitWeight);
        Assert.Equal(original.ElasticModulus, deserialized.ElasticModulus);
        Assert.Equal(original.ShearModulus, deserialized.ShearModulus);
        Assert.Equal(original.PoissonRatio, deserialized.PoissonRatio);
        Assert.Equal(original.ThermalCoefficient, deserialized.ThermalCoefficient);
    }

    [Fact]
    public void XmiPoint3D_SerializesToJson()
    {
        var point = TestModelFactory.CreatePoint();
        var json = JsonConvert.SerializeObject(point, _settings);

        Assert.Contains("\"X\"", json);
        Assert.Contains("\"Y\"", json);
        Assert.Contains("\"Z\"", json);
        Assert.Contains("1", json); // X coordinate (may be 1.0)
        Assert.Contains("2", json); // Y coordinate (may be 2.0)
        Assert.Contains("3", json); // Z coordinate (may be 3.0)
    }

    [Fact]
    public void XmiPoint3D_RoundTrip_PreservesCoordinates()
    {
        var original = TestModelFactory.CreatePoint("pt-1", 1.5, 2.5, 3.5);
        var json = JsonConvert.SerializeObject(original, _settings);
        var deserialized = JsonConvert.DeserializeObject<XmiPoint3d>(json, _settings);

        Assert.NotNull(deserialized);
        Assert.Equal(original.X, deserialized!.X);
        Assert.Equal(original.Y, deserialized.Y);
        Assert.Equal(original.Z, deserialized.Z);
    }

    [Fact]
    public void XmiCrossSection_SerializesToJson()
    {
        var crossSection = TestModelFactory.CreateCrossSection();
        var json = JsonConvert.SerializeObject(crossSection, _settings);

        Assert.Contains("\"Shape\"", json);
        Assert.Contains("\"Rectangular\"", json);
        Assert.Contains("\"Area\"", json);
    }

    [Fact]
    public void XmiStorey_SerializesToJson()
    {
        var storey = TestModelFactory.CreateStorey();
        var json = JsonConvert.SerializeObject(storey, _settings);

        Assert.Contains("\"StoreyElevation\"", json);
        Assert.Contains("12", json); // May be 12.0
    }

    [Fact]
    public void XmiStructuralCurveMember_SerializesToJson()
    {
        var curveMember = TestModelFactory.CreateCurveMember();
        var json = JsonConvert.SerializeObject(curveMember, _settings);

        Assert.Contains("\"CurveMemberType\"", json);
        Assert.Contains("\"Beam\"", json); // Enum as string
        Assert.Contains("\"Length\"", json);
        Assert.Contains("5", json); // May be 5.0
    }

    [Fact]
    public void XmiBeam_SerializesToJson()
    {
        var beam = TestModelFactory.CreateBeam();
        var json = JsonConvert.SerializeObject(beam, _settings);

        Assert.Contains("\"SystemLine\"", json);
        Assert.Contains("\"MiddleMiddle\"", json);
        Assert.Contains("\"Length\"", json);
    }

    [Fact]
    public void XmiColumn_SerializesToJson()
    {
        var column = TestModelFactory.CreateColumn();
        var json = JsonConvert.SerializeObject(column, _settings);

        Assert.Contains("\"SystemLine\"", json);
        Assert.Contains("\"Length\"", json);
        Assert.Contains("3.5", json); // May be quoted or not depending on serializer
    }

    #endregion

    #region JsonProperty Attribute Tests

    [Fact]
    public void XmiBaseEntity_SerializesWithCustomPropertyNames()
    {
        var material = TestModelFactory.CreateMaterial();
        var json = JsonConvert.SerializeObject(material, _settings);
        var jObject = JObject.Parse(json);

        // Verify custom JSON property names from [JsonProperty] attributes
        Assert.NotNull(jObject["id"]); // Not "Id"
        Assert.NotNull(jObject["ifcGuid"]);  // Not "IfcGuid"
    }

    [Fact]
    public void XmiMaterial_SerializesEModulusWithCustomName()
    {
        var material = TestModelFactory.CreateMaterial();
        var json = JsonConvert.SerializeObject(material, _settings);
        var jObject = JObject.Parse(json);

        Assert.NotNull(jObject["EModulus"]); // Custom property name
        Assert.Null(jObject["ElasticModulus"]); // Original C# property name should not appear
    }

    [Fact]
    public void XmiMaterial_SerializesGModulusWithCustomName()
    {
        var material = TestModelFactory.CreateMaterial();
        var json = JsonConvert.SerializeObject(material, _settings);
        var jObject = JObject.Parse(json);

        Assert.NotNull(jObject["GModulus"]); // Custom property name
        Assert.Null(jObject["ShearModulus"]); // Original C# property name should not appear
    }

    [Fact]
    public void XmiMaterial_JsonHasCustomPropertyNames()
    {
        // Test verifies that custom property names appear in JSON
        // Note: Full deserialization requires constructor with all parameters
        var material = TestModelFactory.CreateMaterial();
        var json = JsonConvert.SerializeObject(material, _settings);
        var jObject = JObject.Parse(json);

        Assert.NotNull(jObject["id"]);
        Assert.NotNull(jObject["ifcGuid"]);
        Assert.NotNull(jObject["EModulus"]);
        Assert.NotNull(jObject["GModulus"]);
        Assert.Equal("200000", jObject["EModulus"]!.ToString());
        Assert.Equal("80000", jObject["GModulus"]!.ToString());
    }

    #endregion

    #region Enum Serialization Tests

    [Fact]
    public void Enums_SerializeAsStrings()
    {
        var material = new XmiMaterial(
            "mat-1", "Material", "", "native", "",
            XmiMaterialTypeEnum.Concrete, 30, 25,
            "30000", "12000", "0.2", 1.0
        );

        var json = JsonConvert.SerializeObject(material, _settings);

        Assert.Contains("\"Concrete\"", json); // String, not integer
        Assert.DoesNotContain("\"MaterialType\": 0", json);
    }

    [Fact]
    public void XmiBaseEntityDomainEnum_SerializesAsString()
    {
        var point = TestModelFactory.CreatePoint();
        var json = JsonConvert.SerializeObject(point, _settings);

        Assert.Contains("\"Type\"", json);
        Assert.Contains("\"Geometry\"", json); // XmiPoint3d has Geometry type
    }

    [Fact]
    public void XmiShapeEnum_SerializesAsString()
    {
        var crossSection = TestModelFactory.CreateCrossSection();
        var json = JsonConvert.SerializeObject(crossSection, _settings);

        Assert.Contains("\"Rectangular\"", json);
    }

    [Fact]
    public void XmiSegmentTypeEnum_SerializesAsString()
    {
        var segment = TestModelFactory.CreateSegment();
        var json = JsonConvert.SerializeObject(segment, _settings);

        Assert.Contains("\"Line\"", json);
    }

    [Fact]
    public void Enums_DeserializeFromStrings()
    {
        var json = @"{
            ""ID"": ""mat-1"",
            ""Name"": ""Material"",
            ""ifcGuid"": """",
            ""NativeId"": ""native"",
            ""Description"": """",
            ""EntityType"": ""XmiMaterial"",
            ""Type"": ""Shared"",
            ""MaterialType"": ""Steel"",
            ""Grade"": 50,
            ""UnitWeight"": 78.5,
            ""EModulus"": ""200000"",
            ""GModulus"": ""80000"",
            ""PoissonRatio"": ""0.3"",
            ""ThermalCoefficient"": 1.2
        }";

        var material = JsonConvert.DeserializeObject<XmiMaterial>(json, _settings);

        Assert.Equal(XmiMaterialTypeEnum.Steel, material!.MaterialType);
        Assert.Equal(XmiBaseEntityDomainEnum.Shared, material.Type);
    }

    #endregion

    #region Null Handling Tests

    [Fact]
    public void NullValues_AreNotSerializedByDefault()
    {
        var material = new XmiMaterial(
            "mat-1", "Material", "", "", "", // Empty strings
            XmiMaterialTypeEnum.Steel, 50, 78.5,
            "200000", "80000", "0.3", 1.2
        );

        var json = JsonConvert.SerializeObject(material, _settings);
        var jObject = JObject.Parse(json);

        // Empty strings may still appear, but nulls should be ignored
        Assert.DoesNotContain("null", json.ToLower());
    }

    [Fact]
    public void EmptyStrings_AreSerializedAsEmptyStrings()
    {
        var point = new XmiPoint3d("pt-1", "Point", "", "", "", 1, 2, 3);
        var json = JsonConvert.SerializeObject(point, _settings);

        Assert.Contains("\"\"", json); // Empty strings are preserved
    }

    #endregion

    #region XmiModel Serialization Tests

    [Fact]
    public void XmiModel_SerializesWithEntities()
    {
        var model = new XmiModel();
        model.AddXmiMaterial(TestModelFactory.CreateMaterial());
        model.AddXmiPoint3d(TestModelFactory.CreatePoint());

        var json = JsonConvert.SerializeObject(model, _settings);

        Assert.Contains("\"Entities\"", json);
        Assert.Contains("XmiMaterial", json);
        Assert.Contains("XmiPoint3d", json);
    }

    [Fact]
    public void XmiModel_SerializesWithRelationships()
    {
        var model = new XmiModel();
        var material = TestModelFactory.CreateMaterial();
        var crossSection = TestModelFactory.CreateCrossSection();

        model.AddXmiMaterial(material);
        model.AddXmiCrossSection(crossSection);

        var relation = new XmiHasMaterial(crossSection, material);
        model.AddXmiHasMaterial(relation);

        var json = JsonConvert.SerializeObject(model, _settings);

        Assert.Contains("\"Relationships\"", json);
        Assert.Contains("XmiHasMaterial", json);
    }

    [Fact]
    public void XmiModel_RoundTrip_PreservesEntityCount()
    {
        var original = TestModelFactory.CreateModelWithBasics();
        var json = JsonConvert.SerializeObject(original, _settings);
        var deserialized = JsonConvert.DeserializeObject<XmiModel>(json, _settings);

        Assert.NotNull(deserialized);
        Assert.Equal(original.Entities.Count, deserialized!.Entities.Count);
    }

    [Fact]
    public void EmptyXmiModel_SerializesToJson()
    {
        var model = new XmiModel();
        var json = JsonConvert.SerializeObject(model, _settings);

        Assert.NotNull(json);
        Assert.Contains("\"Entities\"", json);
        Assert.Contains("\"Relationships\"", json);
        Assert.Contains("[]", json); // Empty arrays
    }

    [Fact]
    public void XmiModel_WithMultipleEntities_SerializesAll()
    {
        var model = new XmiModel();

        for (int i = 0; i < 5; i++)
        {
            model.AddXmiPoint3d(TestModelFactory.CreatePoint($"pt-{i}", i, i * 2, i * 3));
        }

        var json = JsonConvert.SerializeObject(model, _settings);
        var deserialized = JsonConvert.DeserializeObject<XmiModel>(json, _settings);

        Assert.NotNull(deserialized);
        Assert.Equal(5, deserialized!.Entities.Count);
    }

    #endregion

    #region Relationship Serialization Tests

    [Fact]
    public void XmiHasPoint3D_SerializesToJson()
    {
        var connection = TestModelFactory.CreatePointConnection();
        var point = TestModelFactory.CreatePoint();
        var relation = new XmiHasPoint3d(connection, point);

        var json = JsonConvert.SerializeObject(relation, _settings);

        Assert.Contains("\"Source\"", json);
        Assert.Contains("\"Target\"", json);
        Assert.Contains("XmiHasPoint3d", json);
    }

    [Fact]
    public void XmiHasMaterial_SerializesToJson()
    {
        var crossSection = TestModelFactory.CreateCrossSection();
        var material = TestModelFactory.CreateMaterial();
        var relation = new XmiHasMaterial(crossSection, material);

        var json = JsonConvert.SerializeObject(relation, _settings);

        Assert.Contains("\"Source\"", json);
        Assert.Contains("\"Target\"", json);
    }

    [Fact]
    public void XmiHasStorey_SerializesToJson()
    {
        var connection = TestModelFactory.CreatePointConnection();
        var storey = TestModelFactory.CreateStorey();
        var relation = new XmiHasStorey(connection, storey);

        var json = JsonConvert.SerializeObject(relation, _settings);

        Assert.NotNull(json);
        Assert.Contains("XmiHasStorey", json);
    }

    [Fact]
    public void Relationship_SerializesSourceAndTargetIds()
    {
        var material = TestModelFactory.CreateMaterial();
        var crossSection = TestModelFactory.CreateCrossSection();
        var relation = new XmiHasMaterial(crossSection, material);

        var json = JsonConvert.SerializeObject(relation, _settings);
        var jObject = JObject.Parse(json);

        Assert.NotNull(jObject["Source"]);
        Assert.NotNull(jObject["Target"]);

        var source = jObject["Source"] as JObject;
        var target = jObject["Target"] as JObject;

        Assert.Equal(crossSection.Id, source!["id"]!.ToString());
        Assert.Equal(material.Id, target!["id"]!.ToString());
    }

    #endregion

    #region XmiManager BuildJson Tests

    [Fact]
    public void XmiManager_BuildJson_ReturnsValidJson()
    {
        var manager = TestModelFactory.CreateManagerWithModel();
        var json = manager.BuildJson(0);

        Assert.NotNull(json);
        Assert.Contains("\"nodes\"", json);
        Assert.Contains("\"edges\"", json);
    }

    [Fact]
    public void XmiManager_BuildJson_IncludesAllEntities()
    {
        var manager = new XmiManager();
        var model = new XmiModel();

        model.AddXmiMaterial(TestModelFactory.CreateMaterial());
        model.AddXmiPoint3d(TestModelFactory.CreatePoint());
        model.AddXmiStorey(TestModelFactory.CreateStorey());

        manager.Models.Add(model);

        var json = manager.BuildJson(0);
        var jObject = JObject.Parse(json);
        var nodes = jObject["nodes"] as JArray;

        Assert.NotNull(nodes);
        Assert.Equal(3, nodes!.Count);
    }

    [Fact]
    public void XmiManager_BuildJson_IncludesAllRelationships()
    {
        var manager = new XmiManager();
        var model = new XmiModel();

        var material = TestModelFactory.CreateMaterial();
        var crossSection = TestModelFactory.CreateCrossSection();

        model.AddXmiMaterial(material);
        model.AddXmiCrossSection(crossSection);
        model.AddXmiHasMaterial(new XmiHasMaterial(crossSection, material));

        manager.Models.Add(model);

        var json = manager.BuildJson(0);
        var jObject = JObject.Parse(json);
        var edges = jObject["edges"] as JArray;

        Assert.NotNull(edges);
        Assert.Single(edges!);
    }

    [Fact]
    public void XmiManager_BuildJson_ThrowsForInvalidModelIndex()
    {
        var manager = new XmiManager();
        Assert.Throws<IndexOutOfRangeException>(() => manager.BuildJson(0));
    }

    #endregion

    #region Shape Parameters Serialization Tests

    [Fact]
    public void RectangularShapeParameters_SerializesToJson()
    {
        var parameters = new RectangularShapeParameters(0.5, 0.3);
        var json = JsonConvert.SerializeObject(parameters, _settings);

        Assert.Contains("\"Shape\"", json);
        Assert.Contains("\"Rectangular\"", json);
        Assert.Contains("\"Values\"", json);
    }

    [Fact]
    public void RectangularShapeParameters_SerializesValuesCorrectly()
    {
        var parameters = new RectangularShapeParameters(0.5, 0.3);
        var json = JsonConvert.SerializeObject(parameters, _settings);
        var jObject = JObject.Parse(json);

        Assert.NotNull(jObject["Values"]);
        var values = jObject["Values"] as JObject;
        Assert.NotNull(values);
        Assert.Equal(0.5, values!["H"]!.Value<double>());
        Assert.Equal(0.3, values["B"]!.Value<double>());
    }

    [Fact]
    public void CircularShapeParameters_SerializesToJson()
    {
        var parameters = new CircularShapeParameters(250);
        var json = JsonConvert.SerializeObject(parameters, _settings);

        Assert.Contains("\"Circular\"", json);
        Assert.Contains("\"D\"", json);
        Assert.Contains("250", json);
    }

    [Fact]
    public void IShapeParameters_SerializesToJson()
    {
        var parameters = new IShapeParameters(400, 200, 10, 8, 12);
        var json = JsonConvert.SerializeObject(parameters, _settings);

        Assert.Contains("\"IShape\"", json);
        Assert.Contains("\"Values\"", json);
    }

    #endregion

    #region Complex Serialization Scenarios

    [Fact]
    public void ComplexModel_WithEntitiesAndRelationships_SerializesCorrectly()
    {
        var model = new XmiModel();

        // Add entities
        var material = TestModelFactory.CreateMaterial();
        var crossSection = TestModelFactory.CreateCrossSection();
        var storey = TestModelFactory.CreateStorey();
        var point = TestModelFactory.CreatePoint();
        var connection = TestModelFactory.CreatePointConnection();

        model.AddXmiMaterial(material);
        model.AddXmiCrossSection(crossSection);
        model.AddXmiStorey(storey);
        model.AddXmiPoint3d(point);
        model.AddXmiStructuralPointConnection(connection);

        // Add relationships
        model.AddXmiHasMaterial(new XmiHasMaterial(crossSection, material));
        model.AddXmiHasStorey(new XmiHasStorey(connection, storey));
        model.AddXmiHasPoint3d(new XmiHasPoint3d(connection, point));

        var json = JsonConvert.SerializeObject(model, _settings);
        var jObject = JObject.Parse(json);

        var entities = jObject["Entities"] as JArray;
        var relationships = jObject["Relationships"] as JArray;

        Assert.NotNull(entities);
        Assert.NotNull(relationships);
        Assert.Equal(5, entities!.Count);
        Assert.Equal(3, relationships!.Count);
    }

    [Fact]
    public void PhysicalElements_SerializeWithCorrectType()
    {
        var beam = TestModelFactory.CreateBeam();
        var column = TestModelFactory.CreateColumn();
        var slab = TestModelFactory.CreateSlab();
        var wall = TestModelFactory.CreateWall();

        var beamJson = JsonConvert.SerializeObject(beam, _settings);
        var columnJson = JsonConvert.SerializeObject(column, _settings);
        var slabJson = JsonConvert.SerializeObject(slab, _settings);
        var wallJson = JsonConvert.SerializeObject(wall, _settings);

        Assert.Contains("\"XmiBeam\"", beamJson);
        Assert.Contains("\"XmiColumn\"", columnJson);
        Assert.Contains("\"XmiSlab\"", slabJson);
        Assert.Contains("\"XmiWall\"", wallJson);
    }

    [Fact]
    public void MixedEntityTypes_InModel_SerializeCorrectly()
    {
        var model = new XmiModel();

        model.AddXmiBeam(TestModelFactory.CreateBeam());
        model.AddXmiColumn(TestModelFactory.CreateColumn());
        model.AddXmiSlab(TestModelFactory.CreateSlab());
        model.AddXmiWall(TestModelFactory.CreateWall());
        model.AddXmiPoint3d(TestModelFactory.CreatePoint());

        var json = JsonConvert.SerializeObject(model, _settings);
        var deserialized = JsonConvert.DeserializeObject<XmiModel>(json, _settings);

        Assert.NotNull(deserialized);
        Assert.Equal(5, deserialized!.Entities.Count);
    }

    [Fact]
    public void LargeModel_SerializesAndDeserializes()
    {
        var model = new XmiModel();

        // Add 100 entities
        for (int i = 0; i < 100; i++)
        {
            model.AddXmiPoint3d(TestModelFactory.CreatePoint($"pt-{i}", i, i * 2, i * 3));
        }

        var json = JsonConvert.SerializeObject(model, _settings);
        var deserialized = JsonConvert.DeserializeObject<XmiModel>(json, _settings);

        Assert.NotNull(deserialized);
        Assert.Equal(100, deserialized!.Entities.Count);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void SpecialCharacters_InStrings_AreEscapedCorrectly()
    {
        var material = new XmiMaterial(
            "mat-1", "Material \"with quotes\"", "ifc\nguid", "native\\id", "Description: special & chars",
            XmiMaterialTypeEnum.Steel, 50, 78.5,
            "200000", "80000", "0.3", 1.2
        );

        var json = JsonConvert.SerializeObject(material, _settings);
        var deserialized = JsonConvert.DeserializeObject<XmiMaterial>(json, _settings);

        Assert.NotNull(deserialized);
        Assert.Equal(material.Name, deserialized!.Name);
        Assert.Equal(material.NativeId, deserialized.NativeId);
        Assert.Equal(material.Description, deserialized.Description);
    }

    [Fact]
    public void UnicodeCharacters_InStrings_PreserveCorrectly()
    {
        var material = new XmiMaterial(
            "mat-1", "材料", "ifc-guid", "native-中文", "描述",
            XmiMaterialTypeEnum.Steel, 50, 78.5,
            "200000", "80000", "0.3", 1.2
        );

        var json = JsonConvert.SerializeObject(material, _settings);
        var deserialized = JsonConvert.DeserializeObject<XmiMaterial>(json, _settings);

        Assert.NotNull(deserialized);
        Assert.Equal("材料", deserialized!.Name);
        Assert.Equal("native-中文", deserialized.NativeId);
        Assert.Equal("描述", deserialized.Description);
    }

    [Fact]
    public void VeryLargeNumbers_SerializeCorrectly()
    {
        var point = new XmiPoint3d(
            "pt-1", "Point", "", "", "",
            double.MaxValue / 2, // Very large number
            double.MinValue / 2, // Very small (negative) number
            0
        );

        var json = JsonConvert.SerializeObject(point, _settings);
        var deserialized = JsonConvert.DeserializeObject<XmiPoint3d>(json, _settings);

        Assert.NotNull(deserialized);
        Assert.Equal(point.X, deserialized!.X);
        Assert.Equal(point.Y, deserialized.Y);
    }

    [Fact]
    public void ZeroValues_SerializeCorrectly()
    {
        var point = new XmiPoint3d("pt-1", "Origin", "", "", "", 0, 0, 0);
        var json = JsonConvert.SerializeObject(point, _settings);

        Assert.Contains("\"X\": 0", json);
        Assert.Contains("\"Y\": 0", json);
        Assert.Contains("\"Z\": 0", json);
    }

    #endregion
}
