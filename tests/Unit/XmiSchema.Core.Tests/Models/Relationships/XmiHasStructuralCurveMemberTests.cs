using XmiSchema.Core.Entities;
using XmiSchema.Core.Relationships;
using XmiSchema.Core.Tests.Support;

namespace XmiSchema.Core.Tests.Models.Relationships;

/// <summary>
/// Validates <see cref="XmiHasStructuralCurveMember"/> relationship constructors.
/// </summary>
public class XmiHasStructuralCurveMemberTests
{
    /// <summary>
    /// Verifies constructor with explicit metadata assigns all properties.
    /// </summary>
    [Fact]
    public void Constructor_AssignsMetadata()
    {
        var beam = TestModelFactory.CreateBeam();
        var curveMember = TestModelFactory.CreateCurveMember();

        var relation = new XmiHasStructuralCurveMember(
            "rel-curve-1",
            beam,
            curveMember,
            "PhysicalToAnalytical",
            "Links physical beam to analytical curve",
            nameof(XmiHasStructuralCurveMember));

        Assert.Equal("rel-curve-1", relation.Id);
        Assert.Equal("PhysicalToAnalytical", relation.Name);
        Assert.Equal(beam, relation.Source);
        Assert.Equal(curveMember, relation.Target);
        Assert.Equal(nameof(XmiHasStructuralCurveMember), relation.EntityType);
    }

    /// <summary>
    /// Verifies simple constructor generates identifier automatically.
    /// </summary>
    [Fact]
    public void Constructor_GeneratesIdentifier()
    {
        var column = TestModelFactory.CreateColumn();
        var curveMember = TestModelFactory.CreateCurveMember();

        var relation = new XmiHasStructuralCurveMember(column, curveMember);

        Assert.False(string.IsNullOrWhiteSpace(relation.Id));
        Assert.Equal(column, relation.Source);
        Assert.Equal(curveMember, relation.Target);
    }

    /// <summary>
    /// Ensures the relationship accepts XmiBasePhysicalEntity as source.
    /// </summary>
    [Fact]
    public void Constructor_AcceptsPhysicalEntityAsSource()
    {
        var beam = TestModelFactory.CreateBeam();
        var curveMember = TestModelFactory.CreateCurveMember();

        var relation = new XmiHasStructuralCurveMember(beam, curveMember);

        Assert.IsAssignableFrom<XmiBasePhysicalEntity>(relation.Source);
    }

    /// <summary>
    /// Ensures the relationship accepts XmiBaseStructuralAnalyticalEntity as target.
    /// </summary>
    [Fact]
    public void Constructor_AcceptsStructuralAnalyticalEntityAsTarget()
    {
        var beam = TestModelFactory.CreateBeam();
        var curveMember = TestModelFactory.CreateCurveMember();

        var relation = new XmiHasStructuralCurveMember(beam, curveMember);

        Assert.IsAssignableFrom<XmiBaseStructuralAnalyticalEntity>(relation.Target);
    }

    /// <summary>
    /// Verifies relationship can link column to curve member.
    /// </summary>
    [Fact]
    public void Constructor_CanLinkColumnToCurveMember()
    {
        var column = TestModelFactory.CreateColumn();
        var curveMember = TestModelFactory.CreateCurveMember();

        var relation = new XmiHasStructuralCurveMember(column, curveMember);

        Assert.NotNull(relation);
        Assert.Equal(column.Id, relation.Source.Id);
        Assert.Equal(curveMember.Id, relation.Target.Id);
    }
}
