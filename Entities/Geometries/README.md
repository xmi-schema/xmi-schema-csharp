# Geometries

Geometric primitives live here and inherit from `XmiBaseGeometry`. They can be linked to analytical entities through the `Relationships` folder.

| Class | Description | Notes |
| --- | --- | --- |
| `XmiPoint3D` | A single coordinate with tolerance-aware equality. | Prefer creating points via `XmiModel.CreatePoint3D` so duplicates are reused. |
| `XmiLine3D` | Straight line between two points. | Associate with curve members using `XmiHasLine3D` or `XmiHasGeometry`. |
| `XmiArc3D` | Circular arc defined by start, end, and center points plus radius. | Useful when exporting curved beams/walls and preserving curvature. |

Any new geometry class should expose immutable positional data and call the `XmiBaseGeometry` constructor with the entity type name so serializers can disambiguate shapes. Tests covering equality and serialization edge cases should live in `tests/XmiSchema.Core.Tests/Geometries`.*** End Patch
