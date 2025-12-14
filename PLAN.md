# PLAN.md: Surface Segment Sequencing Implementation

## Overview

This plan outlines the implementation of surface segment sequencing for XMI surface members (walls, slabs, and structural surface members). The goal is to ensure that when segments are provided, they are properly sequenced based on their `Position` property to define how lines are drawn for the surface member boundary.

## Current State Analysis

Based on codebase analysis:

1. **XmiSegment** already has a `Position` property (float) representing normalized position (0-1) along parent member
2. **XmiModel** has `CreateXmiStructuralSurfaceMember`, `CreateXmiWall`, and `CreateXmiSlab` methods that accept `List<XmiSegment> segments`
3. Current implementation adds segments to surface members but doesn't enforce sequencing
4. Tests show segments are added but position-based ordering is not validated

## Implementation Plan

### Phase 1: Enhance XmiSegment Position Validation

**Files to modify:**
- `Entities/Commons/XmiSegment.cs`

**Changes:**
1. Add validation for Position property (0.0 to 1.0 range)
2. Add method to validate segment sequence consistency
3. Consider adding sequencing metadata

### Phase 2: Update XmiModel Factory Methods

**Files to modify:**
- `Managers/XmiModel.cs`

**Methods to enhance:**
1. `CreateXmiStructuralSurfaceMember()` - lines 1240-1328
2. `CreateXmiWall()` - lines 850-915  
3. `CreateXmiSlab()` - lines 779-844

**Enhancements:**
1. Add segment position validation before adding relationships
2. Sort segments by Position when creating relationships
3. Add validation for segment sequence continuity
4. Ensure segments form a closed boundary for surface members

### Phase 3: Update XmiManager Interface

**Files to modify:**
- `Managers/IXmiManager.cs` - update interface methods
- `Managers/XmiManager.cs` - update implementation methods

**Methods to update:**
1. `CreateXmiStructuralSurfaceMember()` - lines 509-532
2. `CreateXmiWall()` - lines 276-293
3. `CreateXmiSlab()` - lines 252-269

### Phase 4: Enhance Test Coverage

**Files to modify:**
- `XmiSchema.Tests/Managers/XmiModelTests.cs`
- `XmiSchema.Tests/Entities/Commons/XmiSegmentTests.cs`

**New tests needed:**
1. Segment position validation (0.0-1.0 range)
2. Segment sequencing validation
3. Surface member boundary closure validation
4. Invalid segment sequence handling
5. Segment ordering in relationships

### Phase 5: Add Helper Methods

**New methods to add to XmiModel:**
1. `ValidateSegmentSequence(List<XmiSegment> segments)` - validates position ordering
2. `SortSegmentsByPosition(List<XmiSegment> segments)` - returns ordered segments
3. `ValidateSurfaceBoundary(List<XmiSegment> segments)` - ensures closed loop

## Detailed Implementation Steps

### Step 1: XmiSegment Enhancement

```csharp
// Add to XmiSegment.cs
public bool IsValidPosition => Position >= 0.0f && Position <= 1.0f;

public static bool ValidateSequence(List<XmiSegment> segments)
{
    if (segments == null || segments.Count < 2) return true;
    
    var sorted = segments.OrderBy(s => s.Position).ToList();
    for (int i = 0; i < sorted.Count - 1; i++)
    {
        if (sorted[i].Position > sorted[i + 1].Position) return false;
    }
    return true;
}
```

### Step 2: XmiModel Factory Method Updates

```csharp
// Add to each Create* method before segment processing
if (segments != null && segments.Count > 0)
{
    // Validate segment positions
    foreach (var segment in segments)
    {
        if (!segment.IsValidPosition)
            throw new ArgumentException($"Segment {segment.Id} has invalid position: {segment.Position}");
    }
    
    // Validate and sort segments
    if (!XmiSegment.ValidateSequence(segments))
        throw new ArgumentException("Segments are not properly sequenced");
    
    var sortedSegments = segments.OrderBy(s => s.Position).ToList();
    
    // Process sorted segments...
}
```

### Step 3: Surface Boundary Validation

```csharp
// Add method to validate closed boundary
private bool ValidateClosedBoundary(List<XmiSegment> segments)
{
    if (segments.Count < 3) return false; // Need at least 3 segments for closure
    
    // Check if first and last segments connect properly
    // This would need geometry validation based on segment types
    return true; // Placeholder for actual geometric validation
}
```

## Testing Strategy

### Unit Tests to Add

1. **XmiSegmentTests.cs:**
   - `Constructor_InvalidPosition_ThrowsException`
   - `ValidateSequence_ValidSequence_ReturnsTrue`
   - `ValidateSequence_InvalidSequence_ReturnsFalse`

2. **XmiModelTests.cs:**
   - `CreateStructuralSurfaceMember_InvalidSegmentPosition_ThrowsException`
   - `CreateStructuralSurfaceMember_UnsortedSegments_SortsCorrectly`
   - `CreateWall_WithSegments_CreatesOrderedRelationships`
   - `CreateSlab_WithSegments_ValidatesBoundary`

### Integration Tests

1. Test complete surface member creation with properly sequenced segments
2. Test error handling for invalid segment sequences
3. Test serialization/deserialization maintains segment order

## Risk Assessment

**Low Risk:**
- Position validation (backward compatible)
- Segment sorting (improves existing functionality)

**Medium Risk:**
- Strict sequence validation (may break existing code)
- Boundary closure validation (complex geometry logic)

**Mitigation:**
- Add validation as optional initially
- Provide migration path for existing code
- Comprehensive test coverage

## Success Criteria

1. ✅ Segments are validated for position range (0.0-1.0)
2. ✅ Segments are automatically sorted by position in factory methods
3. ✅ Surface members ensure segments form closed boundaries
4. ✅ All existing tests continue to pass
5. ✅ New tests cover edge cases and error conditions
6. ✅ Documentation updated with new behavior

## Timeline Estimate

- **Phase 1:** 1-2 days (XmiSegment enhancements)
- **Phase 2:** 2-3 days (XmiModel factory updates)
- **Phase 3:** 1 day (XmiManager updates)
- **Phase 4:** 2-3 days (Test coverage)
- **Phase 5:** 1-2 days (Helper methods)

**Total:** 7-11 days

## Dependencies

- None external
- Depends on existing geometry classes for boundary validation
- Requires coordination with test suite updates
- Requires coordination with test suite updates