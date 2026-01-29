# AGENTS.md

This guide provides coding standards and workflows for AI agents working in the XmiSchema repository.

## Build, Lint, and Test Commands

```bash
dotnet restore XmiSchema.sln
dotnet build XmiSchema.sln --configuration Debug
dotnet test XmiSchema.sln                    # Run all tests
dotnet test XmiSchema.sln --no-build         # No rebuild
dotnet test --filter "FullyQualifiedName~XmiBeamTests"  # Test class
dotnet test --filter "FullyQualifiedName~Constructor_AssignsAllProperties"  # Test method
dotnet test XmiSchema.sln --verbosity detailed
dotnet pack XmiSchema.csproj --configuration Release --output ./packages
```

## Code Style Guidelines

### Imports and Using Statements
- Implicit usings enabled (`<ImplicitUsings>enable</ImplicitUsings>`)
- Explicit usings grouped by namespace, sorted alphabetically
- Standard library imports first, then project namespaces
- Test project includes `global using Xunit;` in `GlobalUsings.cs`

### Naming Conventions
- **Classes**: PascalCase (e.g., `XmiBeam`, `XmiModel`)
- **Methods**: PascalCase (e.g., `CreateXmiBeam`, `AddXmiMaterial`)
- **Properties**: PascalCase (e.g., `SystemLine`, `LocalAxisX`)
- **Parameters**: camelCase (e.g., `systemLine`, `localAxisX`)
- **Interfaces**: PascalCase with 'I' prefix (e.g., `IXmiShapeParameters`)
- **Enums**: PascalCase (e.g., `XmiShapeEnum`, `XmiSystemLineEnum`)
- **Enum Values**: PascalCase (e.g., `Rectangular`, `CircularHollow`)

### Formatting
- 4 spaces indentation (no tabs)
- Namespace opening braces on new line
- Class/method/constructor opening braces on same line
- Each property on separate line
- One blank line between methods and logical sections

### Types and Type Safety
- Nullable reference types enabled: `string? id`
- Generic type parameter: `T` for type constraints (e.g., `GetXmiEntitiesOfType<T>()`)
- All entities inherit from `XmiBaseEntity`
- Polymorphic deserialization via `EntityName` property discriminator
- Use `nameof(Type)` instead of string literals for type names

### JSON Serialization (Newtonsoft.Json)
```csharp
[JsonProperty(PropertyName = "id", Order = 0)]
public string Id { get; set; }

[JsonProperty(Order = 1)]
public string Name { get; set; }

[JsonConverter(typeof(StringEnumConverter))]
public XmiSystemLineEnum SystemLine { get; set; }
```
- Use `PropertyName` to override default JSON property names
- Use `Order` to control property serialization sequence
- Use `[JsonConverter(typeof(StringEnumConverter))]` for enums

### Enum Definitions
```csharp
public enum XmiShapeEnum
{
    [EnumValue("Rectangular")] Rectangular,
    [EnumValue("Circular")] Circular,
}
```
- Apply `[EnumValue("SerializedString")]` attribute to all members
- Use `ExtensionEnumHelper.FromEnumValue<T>(string)` for parsing

### Constructor Signatures
Standard parameter order for all entities:
```
[id, name, ifcGuid, nativeId, description, ...domain-specific-args]
```

**Entity constructors must pass EntityName discriminator:**
```csharp
public XmiBeam(...) : base(id, name, ifcGuid, nativeId, description, nameof(XmiBeam))
```

**Relationship constructors (provide both):**
1. Fully described: `(string id, XmiBaseEntity source, XmiBaseEntity target, ...)`
2. Shorthand with auto-generated ID: `(XmiBaseEntity source, XmiBaseEntity target, ...)`

### Error Handling
- Use `ArgumentNullException.ThrowIfNull` for null parameter validation
- Use `ArgumentException` for invalid parameter values
- Avoid swallowing exceptions - let them propagate
- Use try-catch only when specific recovery logic is needed

### Testing Conventions
- Test class name: `[EntityName]Tests` (e.g., `XmiBeamTests`)
- Test method names: `MethodName_Scenario_ExpectedResult`
- Use `[Fact]` for single tests, `[Theory]` with `[InlineData]` for data-driven
- Use `TestModelFactory` for consistent test fixtures
- Add XML doc comments to test classes describing intent
- Arrange-Act-Assert pattern preferred

### Project Structure
```
Entities/Bases/              - Base classes (XmiBaseEntity, XmiBaseRelationship)
Entities/Commons/            - Shared entities (Material, CrossSection, Storey)
Entities/Physical/           - Physical elements (Beam, Column, Slab, Wall)
Entities/StructuralAnalytical/ - Analytical elements (CurveMember, SurfaceMember)
Entities/Geometries/         - Geometric primitives (Point3D, Line3D, Arc3D)
Entities/Relationships/      - Graph edges (XmiHasMaterial, XmiHasGeometry)
Enums/                       - Domain enums with EnumValue attribute
Parameters/                  - Shape parameter interfaces and implementations
Managers/                    - XmiModel, XmiManager orchestration
Utils/                       - Helper utilities (ExtensionEnumHelper)
```

## Important Practices

1. **Point Deduplication**: Always create `XmiPoint3d` via `XmiModel.CreatePoint3D()` to avoid duplicates
2. **EntityName Discriminator**: Always pass `nameof(ClassName)` to base constructor
3. **Test Coverage**: All tests must pass before committing (target: 266+ tests)
4. **Shape Parameters**: Reference `Enums/XmiShapeEnumParameters.cs` for canonical parameter keys
5. **Target Framework**: Both library and tests target `net8.0`
6. **Package Version**: Update `<Version>` in `XmiSchema.csproj` before releases

## Critical Workflows

### Adding a New Entity
Create entity class inheriting from appropriate base, implement constructor with standard parameter order, pass `nameof(YourEntity)` to base constructor, add `[JsonProperty(Order = N)]` attributes, create test class, add factory method to `TestModelFactory`, add `AddXmiYourEntity()` method to `XmiModel`, run tests.

### Adding a New Enum
Define enum with `[EnumValue("String")]` on each member, add tests in `XmiEnumTests` for round-trip conversion, use `[JsonConverter(typeof(StringEnumConverter))]` for JSON properties, test serialization/deserialization.

### Modifying Constructor Signatures
Maintain backward compatibility when possible, update all test fixtures in `TestModelFactory`, update XML documentation, run full test suite, document API changes in CLAUDE.md.

## Target Framework
- .NET 8.0 for both library and test projects
- xUnit 2.9.2 for testing
- Newtonsoft.Json 13.0.3 for JSON serialization
