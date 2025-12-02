# Repository Guidelines

## Project Structure & Module Organization
The solution file `xmi-schema-Csharp.Core.sln` currently loads a single library project (`xmi-schema-Csharp.Core.csproj`). Domain objects live under `Models/`, with `Models/Entities` describing structural elements, `Models/Geometries` handling points and curves, and `Models/Relationships` defining how the objects connect. Shared primitives such as `XmiBaseEntity` and `XmiBaseGeometry` sit in `Models/Bases/`. `IXmiManager.cs` and `XmiManager.cs` expose the public API for reading/writing XMI models, while helpers such as `Utils/ExtensionEnumHelper.cs` centralize enum conversions. Generated build outputs remain in `bin/` and `obj/`; keep them clean in commits.

## Build, Test, and Development Commands
Use the .NET CLI from the repo root:
```
dotnet restore xmi-schema-Csharp.Core.sln   # restore packages
dotnet build xmi-schema-Csharp.Core.sln -c Release   # compile library
dotnet test                                   # runs once a test project is added
dotnet format                                 # optional, enforces style before committing
```
When iterating on a single file (for example `Models/Entities/XmiStructuralCurveMember.cs`), `dotnet build --no-restore` keeps loops fast.

## Coding Style & Naming Conventions
Stick to standard C# conventions: 4-space indentation, braces on new lines, PascalCase for public types/methods, camelCase for locals, and prefix interfaces with `I` (mirroring `IXmiManager`). Keep enums singular (`XmiShapeEnum`) and ensure new enums are handled in `ExtensionEnumHelper`. Document complex behaviors with XML comments so downstream SDKs can surface tooltips. Run `dotnet format` or the IDE equivalent before pushing.

## Testing Guidelines
No test project ships today; create one when contributing features: `dotnet new xunit -n XmiSchema.Core.Tests` and add it to the solution. Match namespaces to the production counterparts (e.g., tests for `Models.Entities` live in `XmiSchema.Core.Tests.Models.Entities`). Use descriptive method names like `StructuralCurveMember_ShouldBuildSegments()` and favor data-driven `[Theory]` cases for enum conversions. Treat high-impact parsers/managers as coverage priorities and ensure every bug fix includes a regression test.

## Commit & Pull Request Guidelines
Follow the established `type: short summary` pattern seen in history (`chore: 更新版本号为 0.2.19`, `Fix: structuralmembercreatrefunction`). Keep the summary under 70 characters, optionally adding `[skip ci]` for documentation-only changes. Each PR should describe the motivation, list key files touched (e.g., `Models/Relationships/XmiHasSegment.cs`), link related issues, and include verification steps (`dotnet build`, tests). Where behavior changes affect consumers, attach code snippets or screenshots demonstrating the new result.
