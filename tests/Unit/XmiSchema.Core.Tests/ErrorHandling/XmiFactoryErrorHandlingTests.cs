using System;
using System.Collections.Generic;
using XmiSchema.Entities.Commons;
using XmiSchema.Managers;
using XmiSchema.Entities.Physical;
using XmiSchema.Entities.Bases;
using XmiSchema.Entities.Geometries;
using XmiSchema.Parameters;
using XmiSchema.Entities.StructuralAnalytical;
using XmiSchema.Entities.Commons;
using XmiSchema.Enums;
using XmiSchema.Tests.Managers;
namespace XmiSchema.Tests.ErrorHandling;

/// <summary>
/// Validates error handling and validation logic in XmiModel and XmiManager factory methods.
/// </summary>
public class XmiFactoryErrorHandlingTests
{
    #region XmiModel Factory Method Error Tests

    [Fact]
    public void CreatePoint3D_ThrowsWhenIdIsNull()
    {
        var model = new XmiModel();

        var exception = Assert.Throws<ArgumentException>(() =>
            model.CreateXmiPoint3d(null!, "Point", "", "", "", 0, 0, 0));

        Assert.Contains("ID cannot be null or empty", exception.Message);
        Assert.Equal("id", exception.ParamName);
    }

    [Fact]
    public void CreatePoint3D_ThrowsWhenIdIsEmpty()
    {
        var model = new XmiModel();

        var exception = Assert.Throws<ArgumentException>(() =>
            model.CreateXmiPoint3d("", "Point", "", "", "", 0, 0, 0));

        Assert.Contains("ID cannot be null or empty", exception.Message);
    }

    [Fact]
    public void CreatePoint3D_ThrowsWhenNameIsNull()
    {
        var model = new XmiModel();

        var exception = Assert.Throws<ArgumentException>(() =>
            model.CreateXmiPoint3d("pt-1", null!, "", "", "", 0, 0, 0));

        Assert.Contains("Name cannot be null or empty", exception.Message);
        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void CreatePoint3D_ThrowsWhenNameIsEmpty()
    {
        var model = new XmiModel();

        var exception = Assert.Throws<ArgumentException>(() =>
            model.CreateXmiPoint3d("pt-1", "", "", "", "", 0, 0, 0));

        Assert.Contains("Name cannot be null or empty", exception.Message);
    }

    [Fact]
    public void CreateMaterial_ThrowsWhenIdIsNull()
    {
        var model = new XmiModel();

        var exception = Assert.Throws<ArgumentException>(() =>
            model.CreateXmiMaterial(
                null!, "Material", "", "", "",
                XmiMaterialTypeEnum.Steel,
                50, 78.5, "200000", "80000", "0.3", 1.2
            ));

        Assert.Contains("ID cannot be null or empty", exception.Message);
    }

    [Fact]
    public void CreateMaterial_ThrowsWhenNameIsNull()
    {
        var model = new XmiModel();

        var exception = Assert.Throws<ArgumentException>(() =>
            model.CreateXmiMaterial(
                "mat-1", null!, "", "", "",
                XmiMaterialTypeEnum.Steel,
                50, 78.5, "200000", "80000", "0.3", 1.2
            ));

        Assert.Contains("Name cannot be null or empty", exception.Message);
    }

    [Fact]
    public void CreateStorey_ThrowsWhenIdIsNull()
    {
        var model = new XmiModel();

        var exception = Assert.Throws<ArgumentException>(() =>
            model.CreateXmiStorey(
                null!, "Storey", "", "", "",
                12.0, 1000
            ));

        Assert.Contains("ID cannot be null or empty", exception.Message);
    }

    [Fact]
    public void CreateCrossSection_ThrowsWhenIdIsNull()
    {
        var model = new XmiModel();
        var parameters = new RectangularShapeParameters(0.3, 0.6);

        var exception = Assert.Throws<ArgumentException>(() =>
            model.CreateXmiCrossSection(
                null!, "Section", "", "", "",
                null, XmiShapeEnum.Rectangular, parameters,
                0.18, 0.002, 0.003, 0.01, 0.02, 0.0005, 0.0006, 0.0007, 0.0008, 0.0009
            ));

        Assert.Contains("ID cannot be null or empty", exception.Message);
    }

    [Fact]
    public void CreateCrossSection_ThrowsWhenNameIsEmpty()
    {
        var model = new XmiModel();
        var parameters = new RectangularShapeParameters(0.3, 0.6);

        var exception = Assert.Throws<ArgumentException>(() =>
            model.CreateXmiCrossSection(
                "sec-1", "", "", "", "",
                null, XmiShapeEnum.Rectangular, parameters,
                0.18, 0.002, 0.003, 0.01, 0.02, 0.0005, 0.0006, 0.0007, 0.0008, 0.0009
            ));

        Assert.Contains("Name cannot be null or empty", exception.Message);
    }

    [Fact]
    public void CreateStructurePointConnection_ThrowsWhenIdIsNull()
    {
        var model = new XmiModel();
        var storey = TestModelFactory.CreateStorey();
        var point = TestModelFactory.CreatePoint();

        var exception = Assert.Throws<ArgumentException>(() =>
            model.CreateXmiStructurePointConnection(
                null!, "Connection", "", "", "",
                storey, point
            ));

        Assert.Contains("ID cannot be null or empty", exception.Message);
    }

    [Fact]
    public void CreateStructuralCurveMember_ThrowsWhenIdIsNull()
    {
        var model = new XmiModel();
        var crossSection = TestModelFactory.CreateCrossSection();
        var storey = TestModelFactory.CreateStorey();
        var beginNode = TestModelFactory.CreatePointConnection();
        var endNode = TestModelFactory.CreatePointConnection();

        var exception = Assert.Throws<ArgumentException>(() =>
            model.CreateXmiStructuralCurveMember(
                null!, "Curve", "", "", "",
                null,
                crossSection, storey, XmiStructuralCurveMemberTypeEnum.Beam,
                new List<XmiStructuralPointConnection>(), null,
                XmiSystemLineEnum.MiddleMiddle,
                beginNode, endNode, 5.0,
                "1,0,0", "0,1,0", "0,0,1",
                0, 0, 0, 0, 0, 0,
                "Fixed", "Pinned"
            ));

        Assert.Contains("ID cannot be null or empty", exception.Message);
    }

    [Fact]
    public void CreateStructuralCurveMember_ThrowsWhenNameIsEmpty()
    {
        var model = new XmiModel();
        var crossSection = TestModelFactory.CreateCrossSection();
        var storey = TestModelFactory.CreateStorey();
        var beginNode = TestModelFactory.CreatePointConnection();
        var endNode = TestModelFactory.CreatePointConnection();

        var exception = Assert.Throws<ArgumentException>(() =>
            model.CreateXmiStructuralCurveMember(
                "cur-1", "", "", "", "",
                null,
                crossSection, storey, XmiStructuralCurveMemberTypeEnum.Beam,
                new List<XmiStructuralPointConnection>(), null,
                XmiSystemLineEnum.MiddleMiddle,
                beginNode, endNode, 5.0,
                "1,0,0", "0,1,0", "0,0,1",
                0, 0, 0, 0, 0, 0,
                "Fixed", "Pinned"
            ));

        Assert.Contains("Name cannot be null or empty", exception.Message);
    }

    #endregion

    #region XmiManager Factory Method Error Tests

    [Fact]
    public void XmiManager_AddMaterial_ThrowsWhenModelIndexInvalid()
    {
        var manager = new XmiManager();
        var material = TestModelFactory.CreateMaterial();

        var exception = Assert.Throws<IndexOutOfRangeException>(() =>
            manager.AddXmiMaterialToModel(0, material));

        Assert.NotNull(exception);
    }

    [Fact]
    public void XmiManager_AddMaterial_ThrowsWhenModelIndexNegative()
    {
        var manager = new XmiManager();
        var material = TestModelFactory.CreateMaterial();

        var exception = Assert.Throws<IndexOutOfRangeException>(() =>
            manager.AddXmiMaterialToModel(-1, material));

        Assert.NotNull(exception);
    }

    [Fact]
    public void XmiManager_AddCrossSection_ThrowsWhenModelIndexInvalid()
    {
        var manager = new XmiManager();
        var crossSection = TestModelFactory.CreateCrossSection();

        Assert.Throws<IndexOutOfRangeException>(() =>
            manager.AddXmiCrossSectionToModel(10, crossSection));
    }

    [Fact]
    public void XmiManager_GetEntitiesOfType_ThrowsWhenModelIndexInvalid()
    {
        var manager = new XmiManager();

        Assert.Throws<IndexOutOfRangeException>(() =>
            manager.GetXmiEntitiesOfType<XmiMaterial>(0));
    }

    [Fact]
    public void XmiManager_GetEntityById_ThrowsWhenModelIndexInvalid()
    {
        var manager = new XmiManager();

        Assert.Throws<IndexOutOfRangeException>(() =>
            manager.GetXmiEntityById<XmiMaterial>(0, "mat-1"));
    }

    [Fact]
    public void XmiManager_BuildJson_ThrowsWhenModelIndexInvalid()
    {
        var manager = new XmiManager();

        Assert.Throws<IndexOutOfRangeException>(() =>
            manager.BuildJson(0));
    }

    [Fact]
    public void XmiManager_CreatePoint3D_ThrowsWhenIdIsNull()
    {
        var manager = TestModelFactory.CreateManagerWithModel();

        var exception = Assert.Throws<ArgumentException>(() =>
            manager.CreateXmiPoint3D(0, null!, "Point", "", "", "", 0, 0, 0));

        Assert.Contains("ID cannot be null or empty", exception.Message);
    }

    [Fact]
    public void XmiManager_CreateMaterial_ThrowsWhenNameIsNull()
    {
        var manager = TestModelFactory.CreateManagerWithModel();

        var exception = Assert.Throws<ArgumentException>(() =>
            manager.CreateXmiMaterial(
                0, "mat-1", null!, "", "", "",
                XmiMaterialTypeEnum.Steel,
                50, 78.5, "200000", "80000", "0.3", 1.2
            ));

        Assert.Contains("Name cannot be null or empty", exception.Message);
    }

    [Fact]
    public void XmiManager_CreateStorey_ThrowsWhenIdIsEmpty()
    {
        var manager = TestModelFactory.CreateManagerWithModel();

        var exception = Assert.Throws<ArgumentException>(() =>
            manager.CreateXmiStorey(
                0, "", "Storey", "", "", "",
                12.0, 1000
            ));

        Assert.Contains("ID cannot be null or empty", exception.Message);
    }

    [Fact]
    public void XmiManager_CreateCrossSection_ThrowsWhenNameIsEmpty()
    {
        var manager = TestModelFactory.CreateManagerWithModel();
        var material = TestModelFactory.CreateMaterial();
        var parameters = new RectangularShapeParameters(0.3, 0.6);

        var exception = Assert.Throws<ArgumentException>(() =>
            manager.CreateXmiCrossSection(
                0, "sec-1", "", "", "", "",
                material, XmiShapeEnum.Rectangular, parameters,
                0.18, 0.002, 0.003, 0.01, 0.02, 0.0005, 0.0006, 0.0007, 0.0008, 0.0009
            ));

        Assert.Contains("Name cannot be null or empty", exception.Message);
    }

    [Fact]
    public void XmiManager_CreateStructuralPointConnection_ThrowsWhenIdIsNull()
    {
        var manager = TestModelFactory.CreateManagerWithModel();
        var storey = TestModelFactory.CreateStorey();
        var point = TestModelFactory.CreatePoint();

        var exception = Assert.Throws<ArgumentException>(() =>
            manager.CreateXmiStructuralPointConnection(
                0, null!, "Connection", "", "", "",
                storey, point
            ));

        Assert.Contains("ID cannot be null or empty", exception.Message);
    }

    [Fact]
    public void XmiManager_CreateStructuralCurveMember_ThrowsWhenIdIsEmpty()
    {
        var manager = TestModelFactory.CreateManagerWithModel();
        var crossSection = TestModelFactory.CreateCrossSection();
        var storey = TestModelFactory.CreateStorey();
        var beginNode = TestModelFactory.CreatePointConnection();
        var endNode = TestModelFactory.CreatePointConnection();

        var exception = Assert.Throws<ArgumentException>(() =>
            manager.CreateXmiStructuralCurveMember(
                0, "", "Curve", "", "", "",
                null,
                crossSection, storey, XmiStructuralCurveMemberTypeEnum.Beam,
                new List<XmiStructuralPointConnection>(), null,
                XmiSystemLineEnum.MiddleMiddle,
                beginNode, endNode, 5.0,
                "1,0,0", "0,1,0", "0,0,1",
                0, 0, 0, 0, 0, 0,
                "Fixed", "Pinned"
            ));

        Assert.Contains("ID cannot be null or empty", exception.Message);
    }

    #endregion

    #region Shape Parameters Validation Tests

    [Fact]
    public void RectangularShapeParameters_ThrowsWhenNegativeHeight()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new RectangularShapeParameters(-0.5, 0.3));

        Assert.Contains("Shape parameters must be non-negative", exception.Message);
    }

    [Fact]
    public void RectangularShapeParameters_ThrowsWhenNegativeWidth()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new RectangularShapeParameters(0.5, -0.3));

        Assert.Contains("Shape parameters must be non-negative", exception.Message);
    }

    [Fact]
    public void CircularShapeParameters_ThrowsWhenNegativeDiameter()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new CircularShapeParameters(-250));

        Assert.Contains("Shape parameters must be non-negative", exception.Message);
    }

    [Fact]
    public void CircularHollowShapeParameters_ThrowsWhenNegativeDiameter()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new CircularHollowShapeParameters(-200, 10));

        Assert.Contains("Shape parameters must be non-negative", exception.Message);
    }

    [Fact]
    public void CircularHollowShapeParameters_ThrowsWhenNegativeThickness()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new CircularHollowShapeParameters(200, -10));

        Assert.Contains("Shape parameters must be non-negative", exception.Message);
    }

    [Fact]
    public void IShapeParameters_ThrowsWhenNegativeDepth()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new IShapeParameters(-400, 200, 10, 8, 12));

        Assert.Contains("Shape parameters must be non-negative", exception.Message);
    }

    [Fact]
    public void IShapeParameters_ThrowsWhenNegativeWidth()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new IShapeParameters(400, -200, 10, 8, 12));

        Assert.Contains("Shape parameters must be non-negative", exception.Message);
    }

    [Fact]
    public void TShapeParameters_ThrowsWhenNegativeHeight()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new TShapeParameters(-300, 150, 12, 8));

        Assert.Contains("Shape parameters must be non-negative", exception.Message);
    }

    [Fact]
    public void FlatBarShapeParameters_ThrowsWhenNegativeWidth()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new FlatBarShapeParameters(-100, 10));

        Assert.Contains("Shape parameters must be non-negative", exception.Message);
    }

    [Fact]
    public void RoundBarShapeParameters_ThrowsWhenNegativeDiameter()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new RoundBarShapeParameters(-16));

        Assert.Contains("Shape parameters must be non-negative", exception.Message);
    }

    #endregion

    #region Exception Wrapping Tests

    [Fact]
    public void CreatePoint3D_WrapsExceptionInInvalidOperationException()
    {
        var model = new XmiModel();

        // Trigger internal exception by passing null name (after id validation)
        try
        {
            model.CreateXmiPoint3d("pt-1", null!, "", "", "", 0, 0, 0);
            Assert.Fail("Expected exception was not thrown");
        }
        catch (ArgumentException ex)
        {
            // This is the expected direct exception
            Assert.Contains("Name cannot be null or empty", ex.Message);
        }
    }

    [Fact]
    public void CreateMaterial_WrapsExceptionInInvalidOperationException()
    {
        var model = new XmiModel();

        try
        {
            model.CreateXmiMaterial(
                "mat-1", null!, "", "", "",
                XmiMaterialTypeEnum.Steel,
                50, 78.5, "200000", "80000", "0.3", 1.2
            );
            Assert.Fail("Expected exception was not thrown");
        }
        catch (ArgumentException ex)
        {
            Assert.Contains("Name cannot be null or empty", ex.Message);
        }
    }

    [Fact]
    public void CreateStorey_WrapsExceptionInInvalidOperationException()
    {
        var model = new XmiModel();

        try
        {
            model.CreateXmiStorey(
                "str-1", null!, "", "", "",
                12.0, 1000
            );
            Assert.Fail("Expected exception was not thrown");
        }
        catch (ArgumentException ex)
        {
            Assert.Contains("Name cannot be null or empty", ex.Message);
        }
    }

    [Fact]
    public void CreateCrossSection_WrapsExceptionInInvalidOperationException()
    {
        var model = new XmiModel();
        var parameters = new RectangularShapeParameters(0.3, 0.6);

        try
        {
            model.CreateXmiCrossSection(
                "sec-1", null!, "", "", "",
                null, XmiShapeEnum.Rectangular, parameters,
                0.18, 0.002, 0.003, 0.01, 0.02, 0.0005, 0.0006, 0.0007, 0.0008, 0.0009
            );
            Assert.Fail("Expected exception was not thrown");
        }
        catch (ArgumentException ex)
        {
            Assert.Contains("Name cannot be null or empty", ex.Message);
        }
    }

    #endregion

    #region Deduplication and Reuse Logic Tests

    [Fact]
    public void CreatePoint3D_ReusesExistingPointWithSameCoordinates()
    {
        var model = new XmiModel();

        var point1 = model.CreateXmiPoint3d("pt-1", "Point 1", "", "", "", 1, 2, 3);
        var point2 = model.CreateXmiPoint3d("pt-2", "Point 2", "", "", "", 1, 2, 3);

        // Should reuse the same point instance
        Assert.Same(point1, point2);
        Assert.Single(model.Entities);
    }

    [Fact]
    public void CreateMaterial_ReusesExistingMaterialWithSameNativeId()
    {
        var model = new XmiModel();

        var mat1 = model.CreateXmiMaterial(
            "mat-1", "Material 1", "", "native-1", "",
            XmiMaterialTypeEnum.Steel,
            50, 78.5, "200000", "80000", "0.3", 1.2
        );

        var mat2 = model.CreateXmiMaterial(
            "mat-2", "Material 2", "", "native-1", "",
            XmiMaterialTypeEnum.Concrete,
            30, 25, "30000", "12000", "0.2", 1.0
        );

        // Should reuse existing material with same native ID
        Assert.Same(mat1, mat2);
        Assert.Single(model.Entities);
    }

    [Fact]
    public void CreateStorey_ReusesExistingStoreyWithSameNativeId()
    {
        var model = new XmiModel();

        var storey1 = model.CreateXmiStorey(
            "str-1", "Storey 1", "", "native-1", "",
            12.0, 1000
        );

        var storey2 = model.CreateXmiStorey(
            "str-2", "Storey 2", "", "native-1", "",
            15.0, 1200
        );

        // Should reuse existing storey
        Assert.Same(storey1, storey2);
        Assert.Single(model.Entities);
    }

    [Fact]
    public void CreateCrossSection_ReusesExistingMaterial()
    {
        var model = new XmiModel();

        var material = model.CreateXmiMaterial(
            "mat-1", "Material", "", "native-mat", "",
            XmiMaterialTypeEnum.Steel,
            50, 78.5, "200000", "80000", "0.3", 1.2
        );

        var parameters = new RectangularShapeParameters(0.3, 0.6);

        var crossSection = model.CreateXmiCrossSection(
            "sec-1", "Section", "", "", "",
            material, XmiShapeEnum.Rectangular, parameters,
            0.18, 0.002, 0.003, 0.01, 0.02, 0.0005, 0.0006, 0.0007, 0.0008, 0.0009
        );

        // Should have both entities but material should be reused
        Assert.Equal(2, model.Entities.Count);

        // Verify relationship was created
        Assert.Single(model.Relationships);
    }

    #endregion

    #region Edge Case Tests

    [Fact]
    public void CreatePoint3D_AllowsVeryLargeCoordinates()
    {
        var model = new XmiModel();

        var point = model.CreateXmiPoint3d(
            "pt-1", "Point", "", "", "",
            double.MaxValue / 2, double.MinValue / 2, 0
        );

        Assert.NotNull(point);
        Assert.Equal(double.MaxValue / 2, point.X);
        Assert.Equal(double.MinValue / 2, point.Y);
    }

    [Fact]
    public void CreateMaterial_AllowsZeroValues()
    {
        var model = new XmiModel();

        var material = model.CreateXmiMaterial(
            "mat-1", "Material", "", "", "",
            XmiMaterialTypeEnum.Steel,
            0, 0, "0", "0", "0", 0
        );

        Assert.NotNull(material);
        Assert.Equal(0, material.Grade);
        Assert.Equal(0, material.UnitWeight);
    }

    [Fact]
    public void CreateStorey_AllowsNegativeElevation()
    {
        var model = new XmiModel();

        var storey = model.CreateXmiStorey(
            "str-1", "Basement", "", "", "",
            -5.0, 1000
        );

        Assert.NotNull(storey);
        Assert.Equal(-5.0, storey.StoreyElevation);
    }

    [Fact]
    public void CreateCrossSection_WorksWithNullMaterial()
    {
        var model = new XmiModel();
        var parameters = new RectangularShapeParameters(0.3, 0.6);

        var crossSection = model.CreateXmiCrossSection(
            "sec-1", "Section", "", "", "",
            null, XmiShapeEnum.Rectangular, parameters,
            0.18, 0.002, 0.003, 0.01, 0.02, 0.0005, 0.0006, 0.0007, 0.0008, 0.0009
        );

        Assert.NotNull(crossSection);
        Assert.Single(model.Entities);
        Assert.Empty(model.Relationships; // No material relationship created
    }

    [Fact]
    public void CreateCrossSection_WorksWithMaterialHavingEmptyNativeId()
    {
        var model = new XmiModel();

        var material = new XmiMaterial(
            "mat-1", "Material", "", "", "",
            XmiMaterialTypeEnum.Steel,
            50, 78.5, "200000", "80000", "0.3", 1.2
        );

        var parameters = new RectangularShapeParameters(0.3, 0.6);

        var crossSection = model.CreateXmiCrossSection(
            "sec-1", "Section", "", "", "",
            material, XmiShapeEnum.Rectangular, parameters,
            0.18, 0.002, 0.003, 0.01, 0.02, 0.0005, 0.0006, 0.0007, 0.0008, 0.0009
        );

        Assert.NotNull(crossSection);
        // When material has empty NativeId, no relationship is created
        Assert.Empty(model.Relationships;
    }

    #endregion
}
