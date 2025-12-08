using System.Collections.Generic;
using XmiSchema.Models.Enums;

namespace XmiSchema.Models.Parameters;

/// <summary>
/// Represents a strongly typed parameter set for an <see cref="XmiShapeEnum"/>.
/// </summary>
public interface IXmiShapeParameters
{
    /// <summary>
    /// Gets the associated shape enumeration.
    /// </summary>
    XmiShapeEnum Shape { get; }

    /// <summary>
    /// Returns the serialized parameter dictionary (key/value pairs).
    /// </summary>
    IReadOnlyDictionary<string, double> Values { get; }

    /// <summary>
    /// Executes any shape-specific validation logic.
    /// </summary>
    void Validate();
}
