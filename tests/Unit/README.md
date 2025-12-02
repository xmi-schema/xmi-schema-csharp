# Unit Tests

The `tests/Unit/XmiSchema.Core.Tests` project mirrors the production namespace layout so that every library class has a dedicated test fixture. Use the standard .NET test workflow to execute all suites:

```bash
dotnet test xmi-schema-Csharp.Core.sln
```

## Folder Organization
- `Support/` – shared builders (e.g., `TestModelFactory`) keep fixtures concise and consistent.
- `Models/` – grouped by `Bases`, `Entities`, `Geometries`, `Relationships`, and `XmiModel` helpers.
- `Manager/` – coverage for `XmiManager` orchestration methods.
- `Utils/` – tests for attributes and helper extensions.

Each test class includes XML doc comments describing its intent, aligning with the documentation requirements for the production code.
