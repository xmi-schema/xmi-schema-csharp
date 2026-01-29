using System;
using XmiSchema.Entities.Bases;
using XmiSchema.Enums;

namespace XmiSchema.Entities.Commons;

/// <summary>
/// Represents a building storey (level) with elevation and mass properties.
/// </summary>
/// <remarks>
/// This class encapsulates information about horizontal building levels that define
/// vertical organization of a building model. Storeys are used to organize
/// structural elements, apply vertical loads, and coordinate between architectural
/// and structural disciplines.
///
/// <h4>Key Properties</h4>
/// <list type="bullet">
/// <item><description><see cref="StoreyElevation"/> - Vertical position relative to project datum</description></item>
/// <item><description><see cref="StoreyMass"/> - Total mass assigned to the storey</description></item>
/// </list>
///
/// <h4>Usage in Building Modeling</h4>
/// Storeys serve multiple purposes:
/// <list type="bullet">
/// <item><description>Organizing structural elements by vertical location</description></item>
/// <item><description>Defining seismic mass distribution for analysis</description></item>
/// <item><description>Coordinating with architectural floor plans</description></item>
/// <item><description>Establishing vertical datum for geometric positioning</description></item>
/// </list>
///
/// <h4>Relationships</h4>
/// Storeys are typically associated with structural elements through
/// <see cref="XmiHasStorey"/> relationships, which link elements (beams, columns,
/// slabs, walls) to their containing storey.
///
/// <h4>Equality</h4>
/// Storeys are considered equal if their <see cref="XmiBaseEntity.NativeId"/> values match
/// (case-insensitive comparison).
///
/// <h4>Domain</h4>
/// All storeys have their <see cref="XmiBaseEntity.Domain"/> property set to
/// <see cref="XmiBaseEntityDomainEnum.Shared"/> as they can be referenced by
/// both physical and analytical elements.
/// </remarks>
/// <example>
/// Creating a building storey:
/// <code>
/// var groundFloor = new XmiStorey(
///     id: "storey_ground",
///     name: "Ground Floor",
///     ifcGuid: "2h9$K0b0P6j7aB_1m5H7fR",
///     nativeId: "STORY-01",
///     description: "Building ground floor level",
///     storeyElevation: 0.0,      // m (relative to project datum)
///     storeyMass: 125000.0       // kg (total floor mass)
/// );
///
/// var firstFloor = new XmiStorey(
///     id: "storey_1st",
///     name: "1st Floor",
///     ifcGuid: "1a2b3c4d5e6f",
///     nativeId: "STORY-02",
///     description: "First floor level",
///     storeyElevation: 3.5,      // m (3.5m above ground floor)
///     storeyMass: 125000.0       // kg
/// );
/// </code>
/// </example>
/// <seealso cref="XmiHasStorey"/>
/// <seealso cref="XmiBaseEntity"/>
/// <seealso cref="XmiBaseEntityDomainEnum"/>
public class XmiStorey : XmiBaseEntity, IEquatable<XmiStorey>
{
    /// <summary>
    /// Gets or sets the elevation of the storey above the project datum.
    /// </summary>
    /// <value>The elevation in the project's unit system (e.g., meters, feet).</value>
    /// <remarks>
    /// This property defines the vertical position of the storey relative to the project's
    /// vertical reference point (datum). Elevation is typically measured as the distance
    /// from the project base level (0.0) to the storey's floor level.
    ///
    /// For example, if the ground floor is at elevation 0.0, a first floor might be
    /// at elevation 3.5m (or 11.5ft), and a second floor at 7.0m (or 23.0ft).
    /// </remarks>
    public double StoreyElevation { get; set; }

    /// <summary>
    /// Gets or sets the total mass associated with this storey.
    /// </summary>
    /// <value>The mass in the project's unit system (e.g., kilograms, pounds).</value>
    /// <remarks>
    /// This property represents the cumulative mass of all structural and non-structural
    /// elements assigned to this storey. It is critical for:
    /// <list type="bullet">
    /// <item><description>Seismic analysis - determining lateral forces</description></item>
    /// <item><description>Dynamic analysis - natural frequency calculations</description></item>
    /// <item><description>Foundation design - vertical load distribution</description></item>
    /// <item><description>Performance-based design - mass distribution</description></item>
    /// </list>
    ///
    /// Mass typically includes dead load (self-weight, permanent finishes) and live load
    /// portions used for dynamic analysis.
    /// </remarks>
    public double StoreyMass { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmiStorey"/> class with the
    /// specified elevation and mass properties.
    /// </summary>
    /// <param name="id">The stable, unique identifier for this storey. Must not be null or whitespace.</param>
    /// <param name="name">The human-readable display name (e.g., "Ground Floor", "Level 1"). If null or whitespace, defaults to <paramref name="id"/>.</param>
    /// <param name="ifcGuid">The IFC GUID reference for BIM interoperability. Can be null.</param>
    /// <param name="nativeId">The identifier from the native source system for traceability. Can be null.</param>
    /// <param name="description">A textual description of this storey (e.g., purpose, typical usage). Can be null.</param>
    /// <param name="storeyElevation">The elevation relative to the project datum (e.g., 0.0 for ground floor, 3.5 for first floor).</param>
    /// <param name="storeyMass">The total mass assigned to this storey for analysis (e.g., 125000.0 for a typical floor).</param>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="id"/> is null or whitespace.</exception>
    /// <remarks>
    /// This constructor initializes all storey properties and sets the
    /// <see cref="XmiBaseEntity.Domain"/> property to
    /// <see cref="XmiBaseEntityDomainEnum.Shared"/> as storeys are shared resources
    /// across the XMI graph, referenced by multiple structural elements.
    ///
    /// Storeys are typically created through <see cref="XmiModel"/> factory methods that
    /// handle the creation of associated <see cref="XmiHasStorey"/> relationships.
    /// </remarks>
    /// <seealso cref="XmiHasStorey"/>
    /// <seealso cref="XmiModel"/>
    /// <seealso cref="XmiBaseEntityDomainEnum.Shared"/>
    public XmiStorey(
        string id,
        string name,
        string ifcGuid,
        string nativeId,
        string description,
        double storeyElevation,
        double storeyMass
    ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiStorey), XmiBaseEntityDomainEnum.Shared)
    {
        StoreyElevation = storeyElevation;
        StoreyMass = storeyMass;
    }

    /// <summary>
    /// Determines whether this <see cref="XmiStorey"/> is equal to another storey.
    /// </summary>
    /// <param name="other">The storey to compare with this storey, or null.</param>
    /// <returns>True if storeys have matching <see cref="XmiBaseEntity.NativeId"/> values; otherwise, false.</returns>
    /// <remarks>
    /// Storeys are considered equal if their <see cref="XmiBaseEntity.NativeId"/> values match
    /// using case-insensitive comparison. This enables storey deduplication based on
    /// source system identifiers rather than requiring exact object references.
    /// </remarks>
    public bool Equals(XmiStorey? other)
    {
        if (other == null) return false;
        return string.Equals(NativeId, other.NativeId, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Determines whether this <see cref="XmiStorey"/> is equal to another object.
    /// </summary>
    /// <param name="obj">The object to compare with this storey, or null.</param>
    /// <returns>True if <paramref name="obj"/> is an <see cref="XmiStorey"/> with matching <see cref="XmiBaseEntity.NativeId"/>; otherwise, false.</returns>
    public override bool Equals(object? obj) => Equals(obj as XmiStorey);

    /// <summary>
    /// Returns a hash code for this <see cref="XmiStorey"/> based on its native identifier.
    /// </summary>
    /// <returns>A hash code derived from the lower-case <see cref="XmiBaseEntity.NativeId"/>.</returns>
    /// <remarks>
    /// The hash code is generated from the case-insensitive <see cref="XmiBaseEntity.NativeId"/>
    /// to ensure consistency with the <see cref="Equals(XmiStorey)"/> implementation.
    /// </remarks>
    public override int GetHashCode()
    {
        return NativeId?.ToLowerInvariant().GetHashCode() ?? 0;
    }
}

    public bool Equals(XmiStorey? other)
    {
        if (other == null) return false;
        return string.Equals(NativeId, other.NativeId, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as XmiStorey);

    public override int GetHashCode()
    {
        return NativeId?.ToLowerInvariant().GetHashCode() ?? 0;
    }
}
