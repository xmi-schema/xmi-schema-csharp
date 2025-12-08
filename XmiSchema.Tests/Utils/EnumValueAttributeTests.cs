using XmiSchema.Entities.Bases;

using XmiSchema.Enums;
namespace XmiSchema.Tests.Utils;

/// <summary>
/// Verifies the simple attribute used to annotate enums.
/// </summary>
public class EnumValueAttributeTests
{
    /// <summary>
    /// Confirms the constructor stores the serialized value.
    /// </summary>
    [Fact]
    public void Constructor_AssignsValue()
    {
        var attribute = new EnumValueAttribute("Serialized");

        Assert.Equal("Serialized", attribute.Value);
    }
}
