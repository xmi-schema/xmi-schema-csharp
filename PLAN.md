# Plan: Streamline XMI Confluence Pages with Entity Documentation

## Overview
Update and streamline all XMI Schema Confluence pages with the comprehensive entity documentation now available in the `docs/` directory. Replace older, generic documentation with detailed, structured documentation including usage examples, best practices, and technical specifications.

## Ticket Reference
- Ticket: XMI-222 (to be created)
- Related: XMI-221 (XML Documentation Comments)

## Current State Analysis

### Confluence Pages Inventory
Based on search results, the following Confluence pages exist for XMI Schema entities:

| Page ID | Title | Status | Notes |
|---------|-------|--------|-------|
| 3670166 | XmiBaseEntity | Basic table format, needs comprehensive update |
| 3670177 | XmiStructuralCurveMember | Generic template, needs detailed documentation |
| 3670186 | XmiStructuralPointConnection | Template with placeholders |
| 3670203 | XmiStructuralCrossSection | Template, needs complete documentation |
| 3670206 | XmiStructuralModelAnalysis | Overview page |
| 3670209 | XmiStructuralStorey | Template |
| 3670212 | XmiStructuralSurfaceMember | Template |
| 3670168 | XmiStructuralMaterial | Template |
| 3670171 | XmiStructuralModel | Template |
| 3670343 | Overall Summary of Entities | High-level overview, needs updating |
| 3670390 | XMI Schema CSharp | Empty page, needs to be populated |

### Available Documentation in docs/
Comprehensive documentation exists for:
- **Base Classes** (6 entities): XmiBaseEntity, XmiBaseGeometry, XmiBaseRelationship, XmiBasePhysicalEntity, XmiBaseStructuralAnalyticalEntity, XmiBaseEnum
- **Common Entities** (5 entities): XmiMaterial, XmiCrossSection, XmiStorey, XmiUnit, XmiSegment
- **Physical Elements** (4 entities): XmiBeam, XmiColumn, XmiSlab, XmiWall
- **Structural Analytical Elements** (3 entities): XmiStructuralCurveMember, XmiStructuralSurfaceMember, XmiStructuralPointConnection
- **Geometries** (3 entities): XmiPoint3D, XmiLine3D, XmiArc3D
- **Relationships** (10 entities): XmiHasGeometry, XmiHasLine3D, XmiHasPoint3D, XmiHasMaterial, XmiHasCrossSection, XmiHasSegment, XmiHasStorey, XmiHasStructuralCurveMember, XmiHasStructuralPointConnection, XmiHasArc3D
- **Examples**: StructuralGraphSample with complete working code example

## Pages to Update

### Phase 1: Base Classes (Priority: High)
1. **XmiBaseEntity** (Page ID: 3670166)
   - Replace with comprehensive documentation from `docs/entities/bases/XmiBaseEntity.md`
   - Include inheritance hierarchy, properties table, usage examples, best practices
   - Add code examples demonstrating entity creation and property access

2. **XmiBaseGeometry** (New Page)
   - Create new page using `docs/entities/bases/XmiBaseGeometry.md`
   - Document geometric primitive base class properties and methods
   - Link to child pages: XmiPoint3D, XmiLine3D, XmiArc3D

3. **XmiBaseRelationship** (New Page)
   - Create new page using `docs/entities/bases/XmiBaseRelationship.md`
   - Document relationship base class with source/target entity pattern
   - Link to all relationship child pages

4. **XmiBasePhysicalEntity** (New Page)
   - Create new page using `docs/entities/bases/XmiBasePhysicalEntity.md`
   - Document intermediate base for physical elements
   - Link to: XmiBeam, XmiColumn, XmiSlab, XmiWall

5. **XmiBaseStructuralAnalyticalEntity** (New Page)
   - Create new page using `docs/entities/bases/XmiBaseStructuralAnalyticalEntity.md`
   - Document analytical element base class
   - Link to: XmiStructuralCurveMember, XmiStructuralSurfaceMember, XmiStructuralPointConnection

6. **XmiBaseEnum** (New Page)
   - Create new page using `docs/entities/bases/XmiBaseEnum.md`
   - Document EnumValueAttribute usage and enum serialization

### Phase 2: Common Entities (Priority: High)
1. **XmiMaterial** (Page ID: 3670168)
   - Replace with `docs/entities/commons/XmiMaterial.md`
   - Include material types table, properties explained, usage examples, analysis applications, common material libraries

2. **XmiCrossSection** (Page ID: 3670203)
   - Replace with `docs/entities/commons/XmiCrossSection.md`
   - Document shape types, geometric properties, analytical properties

3. **XmiStorey** (Page ID: 3670209)
   - Replace with `docs/entities/commons/XmiStorey.md`
   - Document level representation, elevation, mass, reaction properties

4. **XmiUnit** (New Page)
   - Create using `docs/entities/commons/XmiUnit.md`
   - Document unit mapping and conversion utilities

5. **XmiSegment** (New Page)
   - Create using `docs/entities/commons/XmiSegment.md`
   - Document segment types and usage in curve members

### Phase 3: Physical Elements (Priority: Medium)
1. **XmiBeam** (New Page)
   - Create using `docs/entities/physical/XmiBeam.md`
   - Document physical beam properties and relationships

2. **XmiColumn** (New Page)
   - Create using `docs/entities/physical/XmiColumn.md`
   - Document physical column properties and relationships

3. **XmiSlab** (New Page)
   - Create using `docs/entities/physical/XmiSlab.md`
   - Document physical slab properties and relationships

4. **XmiWall** (New Page)
   - Create using `docs/entities/physical/XmiWall.md`
   - Document physical wall properties and relationships

### Phase 4: Structural Analytical Elements (Priority: High)
1. **XmiStructuralCurveMember** (Page ID: 3670177)
   - Replace with `docs/entities/structural-analytical/XmiStructuralCurveMember.md`
   - Include comprehensive properties, end fixity conditions, usage examples

2. **XmiStructuralSurfaceMember** (Page ID: 3670212)
   - Replace with `docs/entities/structural-analytical/XmiStructuralSurfaceMember.md`
   - Document surface member types, system plane, boundary conditions

3. **XmiStructuralPointConnection** (Page ID: 3670186)
   - Replace with `docs/entities/structural-analytical/XmiStructuralPointConnection.md`
   - Document node properties, coordinate systems, usage patterns

### Phase 5: Geometries (Priority: Medium)
1. **XmiPoint3D** (New Page)
   - Create using `docs/entities/geometries/XmiPoint3d.md`
   - Document 3D point properties, tolerance-aware equality, automatic deduplication

2. **XmiLine3D** (New Page)
   - Create using `docs/entities/geometries/XmiLine3d.md`
   - Document line geometry properties, start/end points

3. **XmiArc3D** (New Page)
   - Create using `docs/entities/geometries/XmiArc3d.md`
   - Document arc geometry properties, start/end/center points, radius

### Phase 6: Relationships (Priority: Low)
1. **XmiHasGeometry** (New Page)
   - Create using `docs/entities/relationships/XmiHasGeometry.md`
   - Document geometry association pattern

2. **XmiHasLine3D** (New Page)
   - Create using `docs/entities/relationships/XmiHasLine3d.md`

3. **XmiHasPoint3D** (New Page)
   - Create using `docs/entities/relationships/XmiHasPoint3d.md`

4. **XmiHasMaterial** (New Page)
   - Create using `docs/entities/relationships/XmiHasMaterial.md`
   - Document material assignment pattern

5. **XmiHasCrossSection** (New Page)
   - Create using `docs/entities/relationships/XmiHasCrossSection.md`
   - Document cross-section assignment pattern

6. **XmiHasSegment** (New Page)
   - Create using `docs/entities/relationships/XmiHasSegment.md`
   - Document segment positioning with positions

7. **XmiHasStorey** (New Page)
   - Create using `docs/entities/relationships/XmiHasStorey.md`
   - Document storey association

8. **XmiHasStructuralCurveMember** (New Page)
   - Create using `docs/entities/relationships/XmiHasStructuralCurveMember.md`

9. **XmiHasStructuralPointConnection** (New Page)
   - Create using `docs/entities/relationships/XmiHasStructuralPointConnection.md`

10. **XmiHasArc3D** (New Page)
    - Create using `docs/entities/relationships/XmiHasArc3d.md`

### Phase 7: Examples (Priority: High)
1. **XMI Schema CSharp** (Page ID: 3670390)
   - Currently empty, populate with overview of C# library
   - Include quick start guide, installation instructions, basic usage
   - Link to StructuralGraphSample example

2. **StructuralGraphSample** (New Page)
   - Create using `docs/examples/StructuralGraphSample.md`
   - Include complete working code example
   - Document key concepts demonstrated
   - Provide expected output

### Phase 8: Overview Pages (Priority: Medium)
1. **Overall Summary of Entities** (Page ID: 3670343)
   - Update with current entity hierarchy
   - Update domain descriptions
   - Add links to all entity pages
   - Include architecture diagram

2. **XMI Schema Implementation** (Page ID: 3670601)
   - Update with library implementation details
   - Include manager classes (XmiModel, XmiManager)
   - Document API patterns

3. **Entity Relationship Diagram** (Page ID: 3670162)
   - Update with current entity relationships
   - Include UML stereotype information

## Documentation Standards

### Page Structure Template
Each entity page should include:
1. **Title**: Clear entity name
2. **Purpose**: Brief description of entity's role
3. **Properties**: Complete table with Type, Required, Description columns
4. **Usage Example**: C# code demonstrating entity creation
5. **Key Characteristics**: Important behaviors and patterns
6. **Best Practices**: Guidelines for using the entity correctly
7. **Related Entities**: Links to related entities
8. **Related Enums**: Links to relevant enumerations
9. **Notes**: Important considerations and constraints

### Confluence Formatting
- Use Info Macros for important notes
- Use Code Macros for code examples with C# syntax highlighting
- Use Expand Macros for lengthy details
- Use Table Macros for property lists
- Include Table of Contents for easy navigation
- Add breadcrumbs and child page links
- Use proper heading hierarchy (H1, H2, H3)
- Include page labels for discoverability

### Cross-Referencing
- Link to base classes from derived classes
- Link to derived classes from base classes
- Link to related entities in usage examples
- Link to relationship classes where entities are used together
- Link to enums used by entities

## Migration Strategy

### Content Mapping
- **Properties**: Map entity properties to Confluence tables
- **Code Examples**: Include in Code Macros with C# highlighting
- **Best Practices**: Format as Info Macros or tip boxes
- **Related Sections**: Use child pages or expandable sections
- **Diagrams**: Include architecture and hierarchy diagrams where applicable

### Backward Compatibility
- Keep old page structure as reference (archive in comments)
- Add migration notes for users familiar with old format
- Maintain page IDs to prevent broken links
- Update internal links to point to new content

## Quality Assurance

### Review Checklist
- [ ] All pages include complete property tables
- [ ] All pages have at least one code example
- [ ] All pages have usage context and best practices
- [ ] All related entities are cross-linked
- [ ] All code examples compile and run
- [ ] Formatting is consistent across all pages
- [ ] Page labels are applied for discoverability
- [ ] Navigation hierarchy is correct
- [ ] Examples are tested and verified
- [ ] Cross-references resolve correctly

### Validation Steps
1. **Content Review**: Verify completeness of each page
2. **Link Verification**: Ensure all links work
3. **Code Testing**: Verify all code examples compile
4. **Consistency Check**: Ensure formatting is uniform
5. **Navigation Test**: Verify page hierarchy and breadcrumbs
6. **Accessibility Check**: Ensure content is readable and navigable

## Success Criteria

- [ ] All entity pages updated with comprehensive documentation
- [ ] All new pages created for missing entities
- [ ] All code examples tested and verified
- [ ] All cross-references implemented
- [ ] All pages properly formatted in Confluence
- [ ] Navigation structure is logical and complete
- [ ] Documentation is consistent with codebase
- [ ] All pages include best practices and usage examples
- [ ] XMI Schema CSharp overview page populated with quick start guide
- [ ] Examples section added with working code samples

## Related Resources

### Internal Resources
- `docs/entities/` - Comprehensive entity documentation
- `docs/examples/` - Working code examples
- `docs/_config.yml` - Documentation configuration
- `PLAN.md` (XML documentation) - Related ticket XMI-221

### Confluence Resources
- Space: XKB (Knowledge Base)
- Parent pages: XMI Schema Implementation, Overall Summary of Entities
- Page hierarchy needs to be maintained

### External References
- XMI Standard Documentation
- IFC Standard Documentation
- BIM Standards Documentation
- C# Documentation Guidelines

## Timeline Estimate

| Phase | Description | Estimated Time |
|--------|-------------|---------------|
| Phase 1 | Base Classes | 4 hours |
| Phase 2 | Common Entities | 3 hours |
| Phase 3 | Physical Elements | 2 hours |
| Phase 4 | Structural Analytical | 3 hours |
| Phase 5 | Geometries | 2 hours |
| Phase 6 | Relationships | 3 hours |
| Phase 7 | Examples | 2 hours |
| Phase 8 | Overview Pages | 2 hours |
| **Total** | | **21 hours** |

## Notes

### Prioritization
- **High Priority**: Core entities (base classes, common entities, analytical elements) - needed for development
- **Medium Priority**: Physical elements, geometries, overview pages - useful for users
- **Low Priority**: Relationships - can be documented as needed, follow same pattern

### Maintenance
- Documentation should be updated when entities change
- Code examples should be tested with each library release
- Best practices should be reviewed periodically
- Links should be checked for broken references

### Future Enhancements
- Add interactive code snippets that can be copied and run
- Include API reference generation from XML documentation comments
- Add video tutorials for complex entities
- Integrate with automated documentation generation tools
- Add community contribution guidelines
