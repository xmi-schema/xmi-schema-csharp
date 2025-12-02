namespace XmiSchema.Core.Enums;

/// <summary>
/// Attribute for specifying custom string values for enum fields during JSON serialization.
/// </summary>
/// <remarks>
/// This attribute enables human-readable string values in JSON output instead of integer enum values.
/// It is essential for the Cross Model Information schema to maintain readable and interoperable JSON exports.
///
/// The XMI schema uses this pattern extensively to ensure that exported JSON contains descriptive
/// string values (e.g., "Concrete" instead of 0) making the data more accessible to external systems
/// and human readers.
///
/// The <see cref="ExtensionEnumHelper"/> class provides utility methods to convert between
/// enum values and their string representations.
/// </remarks>
/// <example>
/// <code>
/// public enum XmiStructuralMaterialTypeEnum
/// {
///     [EnumValue("Concrete")] Concrete,
///     [EnumValue("Steel")] Steel,
///     [EnumValue("Timber")] Timber
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Field)]
public class EnumValueAttribute : Attribute
{
    /// <summary>
    /// Gets the string value that will be used in JSON serialization.
    /// </summary>
    /// <remarks>
    /// This value appears in the exported JSON instead of the enum's numeric value or C# name.
    /// It should be human-readable and follow consistent naming conventions across the XMI schema.
    /// </remarks>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumValueAttribute"/> class.
    /// </summary>
    /// <param name="value">
    /// The string value to use in JSON serialization. Should be descriptive and consistent with
    /// built environment industry terminology.
    /// </param>
    /// <remarks>
    /// The string value is stored and later retrieved by serialization logic to produce
    /// human-readable JSON output.
    /// </remarks>
    public EnumValueAttribute(string value)
    {
        Value = value;
    }
}