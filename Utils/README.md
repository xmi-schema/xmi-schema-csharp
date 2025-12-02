# Utils

Utility helpers that support the domain model live here. Currently the folder contains:

- `ExtensionEnumHelper`: Reflects over enums decorated with `EnumValueAttribute` and converts serialized strings back into their strongly typed counterparts. It is used when parsing payloads exported by other platforms.

When adding more helpers, keep them pure and deterministic so they remain easy to unit test. Tests should live under `tests/XmiSchema.Core.Tests/Utils`.*** End Patch
