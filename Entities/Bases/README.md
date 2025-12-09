# Base Types

The base layer defines traits shared by every entity, geometry, and relationship in the XmiSchema package.

- `XmiBaseEntity`: Provides identifiers (`ID`, `ifcGuid`, `NativeId`), descriptive text, an `EntityName` discriminator, and a `Domain` classification. All entities must inherit from this class and invoke its constructor with a sensible fallback name.
- `XmiBaseGeometry`: Extends `XmiBaseEntity` for spatial primitives. When creating new geometries, call the base constructor with the entity name so serializers emit the correct discriminator.
- `XmiBaseRelationship`: Stores the source/target nodes, UML stereotype, and descriptive metadata for graph edges. Relationship classes typically expose two constructors: a fully described one, and a shorthand that auto-generates the ID.
- `EnumValueAttribute` (`XmiBaseEnum.cs`): Decorates enum members with the serialized string consumed by authoring tools. Use it together with `ExtensionEnumHelper.FromEnumValue` to parse raw values safely.

All new domain classes should build on these abstractions to maintain consistency across models and to guarantee compatibility with the JSON serializer and downstream connectors.
