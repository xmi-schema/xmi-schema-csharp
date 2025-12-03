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
        values = new ReadOnlyDictionary<string, double>(new Dictionary<string, double>(parameters));
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
            dict[key] = value;
        }

        return dict;
    }
}
