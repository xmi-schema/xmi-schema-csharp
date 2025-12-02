# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

XmiSchema.Core is a C# library for building and managing structural engineering data models with automatic relationship inference. The library exports models to JSON format representing graph structures (nodes and edges). It targets .NET 8.0 and is published as a NuGet package (XmiSchema.Core).

## Build Commands

```bash
# Restore dependencies
dotnet restore -p:RestoreFallbackFolders=""

# Build project
dotnet build --configuration Release --no-restore -p:RestoreFallbackFolders=""

# Run tests (if test project exists)
dotnet test --configuration Release --no-build --logger trx -p:RestoreFallbackFolders=""

# Pack NuGet package
dotnet pack --configuration Release --output ./packages -p:RestoreFallbackFolders=""
```

## Architecture

### Core Model Pattern

The library follows an entity-relationship model:

- **XmiModel** (Models/Model/XmiModel.cs): The root container holding lists of Entities and Relationships
- **XmiBaseEntity** (Models/Bases/XmiBaseEntity.cs): Base class for all structural entities (materials, members, points, etc.)
- **XmiBaseRelationship** (Models/Bases/XmiBaseRelationship.cs): Base class for all relationships with Source/Target entity references

### Builder Pattern Workflow

The primary entry point is **XmiSchemaModelBuilder** (Builder/XmiSchemaModelBuilder.cs):

1. **Add entities** using `AddEntity()` or `AddEntities()`
2. **Build model** using `BuildModel()` - automatically infers relationships via ExtensionRelationshipExporter
3. **Export** using `ExportJson()` or `BuildJsonString()`

The builder automatically:
- Infers relationships from entity property references (e.g., CurveMember → CrossSection)
- Supports topological sorting via `GetTopologicallySortedEntities()`
- Detects circular dependencies via `GetCycles()`

### Automatic Relationship Inference

**ExtensionRelationshipExporter** (Extensions/ExtensionRelationshipExporter.cs) traverses entities and creates relationships by examining properties:

- StructuralPointConnection → Point3D (XmiHasGeometry)
- StructuralCurveMember → CrossSection (XmiHasStructuralCrossSection)
- StructuralCurveMember → Segments (XmiHasSegment)
- StructuralCurveMember → Nodes (XmiHasStructuralNode)
- CrossSection → Material (XmiHasStructuralMaterial)
- Entities → Storey (XmiHasStructuralStorey)
- Arc3D/Line3D → Point3D endpoints (XmiHasGeometry)

### Manager Pattern

Three manager types provide CRUD operations:

- **EntityManager&lt;T&gt;** (Managers/EntityManager.cs): Manages entities with typed generic support
- **RelationshipManager&lt;T&gt;** (Managers/RelationshipManager.cs): Manages relationships
- **ModelManager** (Managers/ModelManager.cs): High-level model operations

All implement corresponding interfaces in Interfaces/.

### JSON Export

**ExtensionNativeJsonBuilder** (Extensions/ExtensionNativeJsonBuilder.cs):
- Serializes XmiModel to graph JSON format: `{ nodes: [...], edges: [...] }`
- Each node/edge is a dictionary: `{ "TypeName": { ...attributes } }`
- Handles enum serialization via EnumValueAttribute
- Converts entity references to IDs for graph representation

### Dependency Analysis

**ExtensionDependencyAnalyzer** (Extensions/ExtensionDependencyAnalyzer.cs):
- Uses QuikGraph library to build directed graphs from entities/relationships
- Provides topological sorting for correct entity serialization order
- Detects circular dependencies in entity relationships

## Code Organization

```
Builder/              - Builder pattern classes for model construction
Extensions/           - Core processing logic (relationship export, JSON building, dependency analysis)
Interfaces/           - Manager interfaces
Managers/             - Entity, Relationship, and Model manager implementations
Models/
  Bases/             - Base classes (XmiBaseEntity, XmiBaseRelationship, XmiBaseGeometry, XmiBaseEnum)
  Entities/          - Concrete structural entities (CurveMember, SurfaceMember, CrossSection, etc.)
  Enums/             - Type enumerations with EnumValueAttribute for serialization
  Geometries/        - 3D geometry classes (Point3D, Line3D, Arc3D)
  Model/             - XmiModel root class
  Relationships/     - Concrete relationship types (XmiHasGeometry, XmiHasSegment, etc.)
```

## Important Patterns

### Entity Properties
All entities inherit from XmiBaseEntity with properties: ID, Name, IFCGUID, NativeId, Description, EntityType

### Relationship Properties
All relationships inherit from XmiBaseRelationship with properties: ID, Source, Target, Name, Description, EntityType, UmlType

### Enum Serialization
Enums use `[EnumValue("string")]` attribute to control JSON output values

## CI/CD

GitHub Actions workflow (.github/workflows/ci-cd.yml):
- Automatic semantic versioning based on conventional commits
- Updates .csproj version automatically
- Builds, tests, and publishes to NuGet on main/dev branches
- Uses `[skip ci]` in version update commits

## Dependencies

- Newtonsoft.Json 13.0.3 - JSON serialization
- QuikGraph 2.5.0 - Graph algorithms for dependency analysis
