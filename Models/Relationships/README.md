# Relationships

Relationship entities inherit from `XmiBaseRelationship` and form the edges of the Cross Model Information graph. Each relationship pairs a source entity with a target entity, exposing the association through a UML type string.

| Relationship | Purpose | Common Source → Target |
| --- | --- | --- |
| `XmiHasGeometry` | Binds an entity to its geometry (line, arc, etc.). | Member/Surface → `XmiBaseGeometry` |
| `XmiHasLine3D` | Specialised helper for line references. | `XmiStructuralCurveMember` → `XmiLine3D` |
| `XmiHasPoint3D` | Connects point connections to coordinates. | `XmiStructuralPointConnection` → `XmiPoint3D` |
| `XmiHasSegment` | Links curve members to their `XmiSegment` definitions. | `XmiStructuralCurveMember` → `XmiSegment` |
| `XmiHasStructuralMaterial` | Assigns materials to consuming entities. | Cross section / member → `XmiStructuralMaterial` |
| `XmiHasStructuralNode` | Declares a member’s analytical node dependency. | `XmiStructuralCurveMember` → `XmiStructuralPointConnection` |
| `XmiHasStructuralCrossSection` | Specifies which cross-section a member uses. | `XmiStructuralCurveMember` → `XmiStructuralCrossSection` |
| `XmiHasStructuralStorey` | Places an entity on a storey level. | Point connection → `XmiStructuralStorey` |

Whenever you add new entity types, introduce matching relationships so consuming systems can navigate the graph. Keep constructors consistent with the `<id, source, target, ...>` signature and cover them with unit tests in `tests/XmiSchema.Core.Tests/Relationships`.*** End Patch
