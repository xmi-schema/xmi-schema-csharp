# XmiShapeEnum Parameter Reference

`XmiCrossSection.Parameters` now stores the shape inputs through strongly typed classes (e.g., `RectangularShapeParameters`, `IShapeParameters`). Each class serializes into a dictionary so every symbol (H, B, T, etc.) is explicitly paired with its numeric value. This ensures consumers can reason about the payload without tracking array indexes.

Example for a rectangular column:

```json
{
  "Shape": "Rectangular",
  "Parameters": {
    "H": 0.4,
    "B": 0.4
  }
}
```

The table below mirrors the content of `CrossSectionParameter.docx` and the implementation in `Models/Enums/XmiShapeEnumParameters.cs`. Use it as the authoritative list of parameter keys for each `XmiShapeEnum`. Values captured in the dictionary are numeric magnitudes (double precision in code, JSON numbers on export). In code, construct the corresponding `*ShapeParameters` class (for example `new RectangularShapeParameters(h, b)`), which encapsulates these keys before serialization.

| Shape | Parameter Keys | Notes / Alternate Representations |
| --- | --- | --- |
| Circular | D | – |
| Rectangular | H, B | – |
| L Shape | H, B, T, t | – |
| T Shape | H, B, T, t | Alternate: d, B, T, t, r |
| L Inverted | H, B, T, t | – |
| T Inverted | H, B, T, t | – |
| C Shape | H, B, T1, T2, t | – |
| Elbow | B1, B2, T, a | – |
| Trapezium | H, BTop, BBot | – |
| Parallelogram | B, L, a | – |
| Polygon | N, R | – |
| I Shape | D, B, T, t, r | – |
| Circular Hollow | D, t | – |
| Square Hollow | D, t | – |
| Rectangular Hollow | D, B, t | – |
| Tapered Flange Channel | D, B, T, t | – |
| Parallel Flange Channel | D, B, T, t, r1 | – |
| Plain Channel | D, B, t | – |
| Lipped Channel | D, B, C, t, r1, r2 | – |
| Z Purlin | D, E, F, L, t | Constraint: E > F |
| Equal Angle | A, t, r1, r2 | Constraint: r1 > r2 |
| Unequal Angle | A, B, t, r1, r2 | – |
| Flat Bar | B, t | – |
| Square Bar | a | – |
| Deformed Bar | D | – |
| Round Bar | D | – |
| Others | – | Provide any key/value pairs required by the authoring tool. |
| Unknown | – | Reserved for placeholder sections. |

When serializing to JSON via `XmiManager`, these parameter dictionaries are emitted under the `Parameters` property, matching the format shown above.
