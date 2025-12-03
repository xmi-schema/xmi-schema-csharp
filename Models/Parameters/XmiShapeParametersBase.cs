using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using XmiSchema.Core.Enums;

namespace XmiSchema.Core.Parameters;

/// <summary>
/// Base implementation storing the canonical dictionary representation for a shape.
/// </summary>
public abstract class XmiShapeParametersBase : IXmiShapeParameters
{
    private readonly IReadOnlyDictionary<string, double> values;

    protected XmiShapeParametersBase(XmiShapeEnum shape, IDictionary<string, double> parameters)
    {
        Shape = shape;
        values = new ReadOnlyDictionary<string, double>(Sanitize(parameters));
    }

    public XmiShapeEnum Shape { get; }

    public IReadOnlyDictionary<string, double> Values => values;

    public virtual void Validate()
    {
        // Placeholder for shape-specific validation overrides.
    }

    protected static IDictionary<string, double> Build(params (string Key, double Value)[] entries)
    {
        var dict = new Dictionary<string, double>(entries.Length, StringComparer.OrdinalIgnoreCase);
        foreach (var (key, value) in entries)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(entries), key, "Shape parameters must be non-negative.");
            }

            dict[key] = value;
        }

        return dict;
    }

    private static IDictionary<string, double> Sanitize(IDictionary<string, double> source)
    {
        var dict = new Dictionary<string, double>(source.Count, StringComparer.OrdinalIgnoreCase);
        foreach (var kvp in source)
        {
            if (kvp.Value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(source), kvp.Key, "Shape parameters must be non-negative.");
            }

            dict[kvp.Key] = kvp.Value;
        }

        return dict;
    }
}
