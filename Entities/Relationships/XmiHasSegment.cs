using XmiSchema.Entities.Bases;
using XmiSchema.Entities.Commons;

namespace XmiSchema.Entities.Relationships;

/// <summary>
/// Binds curve members to segment definitions so consumers can rebuild detailed analytical definitions.
/// </summary>
public class XmiHasSegment : XmiBaseRelationship
{
    /// <summary>
    /// Position of the segment within the parent curve member's segment sequence.
    /// This allows the same segment to have different positions in different relationships.
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// Gets whether the position value is valid (non-negative integer).
    /// </summary>
    public bool IsValidPosition => Position >= 0;
    /// <summary>
    /// Creates a segment relationship with explicit identifiers.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <param name="source">Owning entity (usually a curve member).</param>
    /// <param name="target">Segment entity.</param>
    /// <param name="position">Position of the segment in the sequence.</param>
    /// <param name="name">Relationship label.</param>
    /// <param name="description">Additional notes.</param>
    /// <param name="entityName">Serialized entity name.</param>
    public XmiHasSegment(
        string id,
        XmiBaseEntity source,
        XmiSegment target,
        int position,
        string name,
        string description,
        string entityName
    ) : base(id, source, target, name, description, nameof(XmiHasSegment))
    {
        Position = position < 0 ? 0 : position;
    }

    /// <summary>
    /// Auto-generates an identifier for a simple segment relationship.
    /// </summary>
    /// <param name="source">Owning entity.</param>
    /// <param name="target">Segment entity.</param>
    /// <param name="position">Position of the segment in the sequence.</param>
    public XmiHasSegment(
        XmiBaseEntity source,
        XmiSegment target,
        int position
    ) : base(source, target, nameof(XmiHasSegment))
    {
        Position = position < 0 ? 0 : position;
    }
}
