---
layout: default
title: Base Types
---

# Base Types

The base layer defines traits shared by every entity, geometry, and relationship in the XmiSchema package.

## XmiBaseEntity

Provides identifiers (`ID`, `ifcGuid`, `NativeId`), descriptive text, and an `EntityType` string that controls serialization. All entities must inherit from this class and invoke its constructor with a sensible fallback name.

**Key Properties:**
- `Id`: Unique identifier
- `Name`: Friendly display name
- `ifcGuid`: IFC GUID for BIM interoperability
- `NativeId`: Authoring system identifier
- `Description`: Contextual description
- `EntityType`: Discriminator for serialization
- `Type`: Domain classification (Physical, StructuralAnalytical, Geometry, Functional)

## XmiBasePhysicalEntity

Abstract base class for all physical domain entities (beams, columns, slabs, walls). Automatically sets `Type = Physical`.

**Inherits from:** `XmiBaseEntity`

**Used by:**
- `XmiBeam`
- `XmiColumn`
- `XmiSlab`
- `XmiWall`

## XmiBaseStructuralAnalyticalEntity

Abstract base class for structural analytical entities. Automatically sets `Type = StructuralAnalytical`.

**Inherits from:** `XmiBaseEntity`

**Used by:**
- `XmiStructuralCurveMember`
- `XmiStructuralSurfaceMember`
- `XmiStructuralPointConnection`

## XmiBaseGeometry

Extends `XmiBaseEntity` for spatial primitives. When creating new geometries, call the base constructor with the entity type name so serializers emit the correct discriminator. Automatically sets `Type = Geometry`.

**Inherits from:** `XmiBaseEntity`

**Used by:**
- `XmiPoint3D`
- `XmiLine3D`
- `XmiArc3D`

## XmiBaseRelationship

Stores the source/target nodes, UML stereotype, and descriptive metadata for graph edges. Relationship classes typically expose two constructors: a fully described one, and a shorthand that auto-generates the ID.

**Key Properties:**
- `Source`: Source entity reference
- `Target`: Target entity reference
- `RelationshipType`: Type discriminator

## EnumValueAttribute

Decorates enum members with the serialized string consumed by authoring tools. Use it together with `ExtensionEnumHelper.FromEnumValue` to parse raw values safely.

**Usage:**
```csharp
public enum XmiShapeEnum
{
    [EnumValue("Rectangular")]
    Rectangular,

    [EnumValue("Circular")]
    Circular
}
```

---

All new domain classes should build on these abstractions to maintain consistency across models and to guarantee compatibility with the JSON serializer and downstream connectors.

[Back to API Reference](.)
