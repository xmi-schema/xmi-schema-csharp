using XmiSchema.Core.Entities;
using XmiSchema.Core.Enums;

namespace XmiSchema.Core.Tests.Models.Entities.StructuralAnalytical;

/// <summary>
/// Ensures curve member metadata is carried through constructor arguments.
/// </summary>
public class XmiStructuralCurveMemberTests
{
    /// <summary>
    /// Verifies key properties such as offsets and fixity are stored.
    /// </summary>
    [Fact]
    public void Constructor_AssignsOffsetsAndFixity()
    {
        var member = TestModelFactory.CreateCurveMember();

        Assert.Equal(XmiStructuralCurveMemberTypeEnum.Beam, member.CurveMemberType);
        Assert.Equal(XmiStructuralCurveMemberSystemLineEnum.MiddleMiddle, member.SystemLine);
        Assert.Equal("Fixed", member.EndFixityStart);
        Assert.Equal(5.0, member.Length);
    }

    /// <summary>
    /// Equality is based on the native identifier.
    /// </summary>
    [Fact]
    public void Equals_UsesNativeId()
    {
        var first = TestModelFactory.CreateCurveMember("cur-shared");
        var second = TestModelFactory.CreateCurveMember("cur-shared");

        Assert.True(first.Equals(second));
    }
}
