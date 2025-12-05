# XmiModel

`XmiModel` is the central in-memory graph that stores every entity (`Entities`) and relationship (`Relationships`). Helper methods inside `XmiModel.cs` wrap the most common workflows:

- `AddXmi*` methods append domain entities or relationships that have already been assembled elsewhere.
- `CreateXmiStructuralPointConnection` and `CreatePoint3D` provide de-duplication logic so two connections that reference the same coordinates reuse the same IDs.
- `FindMatchingPointConnectionByPoint3D` searches existing relationships to determine whether an imported node already exists; use this before creating new nodes from external software.
- `GetEntitiesOfType<T>` returns strongly-typed subsets, enabling callers to run LINQ queries without casting.

Future enhancements—like validation, graph traversal helpers, or persistence routines—should continue to live here so the rest of the library can stay lightweight. When adding APIs, update the interface (`IXmiManager`) and create matching unit tests inside `tests/XmiSchema.Core.Tests/Models`.*** End Patch
