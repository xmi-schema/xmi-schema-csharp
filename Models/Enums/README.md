# Enums

Enumerations in this folder standardize string values flowing through the Cross Model Information schema. Each enum member is annotated with `EnumValueAttribute`, ensuring serialized payloads use the correct labels (e.g., `"Beam"`, `"Rectangular"`).

| Enum | Context |
| --- | --- |
| `XmiSegmentTypeEnum` | Geometry families for `XmiSegment` (line, circular arc, spline, etc.). |
| `XmiShapeEnum` | Profile shapes for `XmiStructuralCrossSection`. |
| `XmiStructuralCurveMemberTypeEnum` | Classification of curve members (beam, column, bracing). |
| `XmiStructuralCurveMemberSystemLineEnum` | Location of the analytical line relative to the profile. |
| `XmiStructuralMaterialTypeEnum` | Material categories (concrete, steel, timber, etc.). |
| `XmiStructuralSurfaceMemberTypeEnum` | Slab/wall/panel classifications. |
| `XmiStructuralSurfaceMemberSpanTypeEnum` | Span behavior of surface members (one-way, two-way). |
| `XmiStructuralSurfaceMemberSystemPlaneEnum` | Analytical plane location for surface elements. |
| `XmiUnitEnum` | Units assigned to entity attributes (meters, millimeters, seconds). |

When adding new enum values, always update `Utils/ExtensionEnumHelper` tests to verify round-trip conversions and confirm that serialized strings align with the schema specification.*** End Patch
