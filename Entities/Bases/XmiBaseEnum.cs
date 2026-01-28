namespace XmiSchema.Entities.Bases;

/// <summary>
/// Custom attribute that maps enum members to serialized string values for XMI payloads.
/// </summary>
/// <remarks>
/// This attribute is applied to enumeration fields to specify the exact string value
/// that should be used when serializing the enum member to JSON. It enables:
///
/// <list type="bullet">
/// <item><description>Custom string values that differ from enum member names</description></item>
/// <item><description>Case control for serialized output</description></item>
/// <item><description>Legacy format compatibility</description></item>
/// <item><description>Domain-specific naming conventions</description></item>
/// </list>
///
/// The attribute is used by Newtonsoft.Json (or other serialization frameworks) to
/// determine the serialized string value during JSON serialization and deserialization.
/// This is particularly important when enum names need to conform to external standards
/// or when they contain characters that are not valid in C# identifiers.
///
/// To parse enum values from their serialized string representation, use the
/// <see cref="XmiSchema.Utils.ExtensionEnumHelper.FromEnumValue{T}(string)"/> method.
/// </remarks>
/// <example>
/// Defining an enum with custom serialized values:
/// <code>
/// using XmiSchema.Entities.Bases;
///
/// public enum XmiShapeEnum
/// {
///     [EnumValue("RECTANGULAR")]
///     Rectangular,
///
///     [EnumValue("I-SHAPED")]
///     IShaped,
///
///     [EnumValue("L-SECTION")]
///     LSection
/// }
///
/// // When serialized to JSON:
/// // { "shape": "RECTANGULAR" }  // Not "Rectangular"
/// // { "shape": "I-SHAPED" }     // Not "IShaped"
/// </code>
/// </example>
/// <example>
/// Parsing enum values from string:
/// <code>
/// string serializedValue = "I-SHAPED";
/// XmiShapeEnum parsedValue = ExtensionEnumHelper.FromEnumValue&lt;XmiShapeEnum&gt;(serializedValue);
/// // parsedValue == XmiShapeEnum.IShaped
/// </code>
/// </example>
/// <seealso cref="XmiSchema.Utils.ExtensionEnumHelper"/>
/// <seealso cref="System.Attribute"/>
/// <seealso cref="System.Enum"/>
[AttributeUsage(AttributeTargets.Field)]
public class EnumValueAttribute : Attribute
{
    /// <summary>
    /// Gets the serialized string value associated with the enum member.
    /// </summary>
    /// <value>The canonical string representation stored in XMI payloads.</value>
    /// <remarks>
    /// This value is used by serialization frameworks to determine the JSON representation
    /// of the enum member. It should be a valid string that can be used in JSON and
    /// should conform to any external standards or conventions required by downstream systems.
    /// </remarks>
    public string Value  { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumValueAttribute"/> class with the
    /// specified serialized string value.
    /// </summary>
    /// <param name="value">The string representation to be stored in XMI payloads when the enum member is serialized.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <exception cref="System.ArgumentException">Thrown when <paramref name="value"/> is null or whitespace.</exception>
    /// <remarks>
    /// Use this attribute on enum fields to customize their serialized representation.
    /// The <paramref name="value"/> parameter determines what string will appear in
    /// JSON output when the enum member is serialized.
    ///
    /// Best practices for choosing enum values:
    /// <list type="bullet">
    /// <item><description>Use uppercase for consistency (e.g., "RECTANGULAR")</description></item>
    /// <item><description>Use hyphens for multi-word values (e.g., "I-SHAPED")</description></item>
    /// <item><description>Follow external standards when applicable</description></item>
    /// <item><description>Avoid special characters that might cause JSON parsing issues</description></item>
    /// <item><description>Keep values stable and backward compatible</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// Applying the attribute to enum members:
    /// <code>
    /// public enum XmiStructuralCurveMemberTypeEnum
    /// {
    ///     [EnumValue("BEAM")]
    ///     Beam,
    ///
    ///     [EnumValue("COLUMN")]
    ///     Column,
    ///
    ///     [EnumValue("BRACING")]
    ///     Bracing
    /// }
    /// </code>
    /// </example>
    public EnumValueAttribute(string value)
    {
        Value  = value;
    }
}
