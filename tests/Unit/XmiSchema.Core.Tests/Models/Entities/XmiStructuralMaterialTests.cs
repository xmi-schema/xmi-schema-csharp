using XmiSchema.Core.Entities;
using XmiSchema.Core.Enums;

namespace XmiSchema.Core.Tests.Models.Entities;

/// <summary>
/// Validates the material entity used throughout the schema.
/// </summary>
public class XmiStructuralMaterialTests
{
    /// <summary>
    /// Ensures constructor parameters are assigned to their respective properties.
    /// </summary>
    [Fact]
    public void Constructor_PopulatesMechanicalProperties()
    {
        var material = TestModelFactory.CreateMaterial();

        Assert.Equal(XmiStructuralMaterialTypeEnum.Steel, material.MaterialType);
        Assert.Equal(78.5, material.UnitWeight);
        Assert.Equal("200000", material.ElasticModulus);
    }

    /// <summary>
    /// Verifies equality semantics rely on the native identifier.
    /// </summary>
    [Fact]
    public void Equals_ReturnsTrueForMatchingNativeId()
    {
        var first = TestModelFactory.CreateMaterial("mat-dup");
        var second = TestModelFactory.CreateMaterial("mat-dup");

        Assert.True(first.Equals(second));
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }
}
