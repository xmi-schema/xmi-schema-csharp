using XmiSchema.Models.Bases;
using XmiSchema.Core.Utils;

namespace XmiSchema.Tests.Utils;

/// <summary>
/// Tests the enum helper used to resolve serialized values.
/// </summary>
public class ExtensionEnumHelperTests
{
    /// <summary>
    /// Finds the enum value that matches the annotated string (case-insensitive).
    /// </summary>
    [Fact]
    public void FromEnumValue_ReturnsMatchingValue()
    {
        var result = ExtensionEnumHelper.FromEnumValue<XmiMaterialTypeEnum>("steel");

        Assert.Equal(XmiMaterialTypeEnum.Steel, result);
    }

    /// <summary>
    /// Returns null when no matching attribute exists.
    /// </summary>
    [Fact]
    public void FromEnumValue_ReturnsNullWhenMissing()
    {
        var result = ExtensionEnumHelper.FromEnumValue<XmiMaterialTypeEnum>("not-a-value");

        Assert.Null(result);
    }
}
