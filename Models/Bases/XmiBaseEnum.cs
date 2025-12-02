namespace XmiSchema.Core.Enums;

/// <summary>
/// Maps enum members to serialized string values consumed by downstream systems.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class EnumValueAttribute : Attribute
{
    public string Value  { get; }

    /// <summary>
    /// Creates a new attribute containing the canonical serialized value.
    /// </summary>
    /// <param name="value">String representation stored in the XMI payload.</param>
    public EnumValueAttribute(string value)
    {
        Value  = value;
    }
}
