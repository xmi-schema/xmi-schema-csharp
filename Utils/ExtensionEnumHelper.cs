using System.Reflection;
using XmiSchema.Entities.Bases;

namespace XmiSchema.Utils;

/// <summary>
/// Provides utilities for converting serialized string values back into XMI enums.
/// </summary>
public static class ExtensionEnumHelper
{
    /// <summary>
    /// Returns the enum value annotated with <see cref="EnumValueAttribute"/> that matches the provided string.
    /// </summary>
    /// <typeparam name="TEnum">Enumeration type declared in <c>XmiSchema.Core.Enums</c>.</typeparam>
    /// <param name="value">Serialized value to match.</param>
    public static TEnum? FromEnumValue<TEnum>(string value) where TEnum : struct, Enum
    {
        foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attribute = field.GetCustomAttribute<EnumValueAttribute>();
            if (attribute != null && attribute.Value.Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                var enumValue = field.GetValue(null);
                if (enumValue is TEnum typedValue)
                {
                    return typedValue;
                }
            }
        }

        return null;
    }
}
