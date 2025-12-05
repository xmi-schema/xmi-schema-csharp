# Transaction-Based Create Methods

## Overview

The XmiModel Create methods implement a transaction-based pattern to ensure data integrity. All Create operations are atomic - either all changes are committed to the model, or none are. If any step fails, the operation rolls back and logs the error.

## Why Transactions?

Create methods in XmiModel serve as utility functions for end users to simplify entity creation. Unlike the basic Add methods (which directly append entities/relationships to lists), Create methods:

1. **Perform validation** - Check IDs, names, and required parameters
2. **Handle relationships** - Automatically create and wire relationships between entities
3. **Reuse existing entities** - Check for duplicates and return existing instances when appropriate
4. **Maintain consistency** - Ensure the model is never left in a partially-updated state

## Transaction Architecture

### Components

#### 1. Error Log
```csharp
public List<string> ErrorLog { get; } = new();
```
Stores error messages from failed Create operations, allowing users to diagnose issues.

#### 2. Transaction Helper Method
```csharp
private T? ExecuteTransaction<T>(
    string operationName,
    Func<(T? entity, List<XmiBaseEntity> entitiesToAdd, List<XmiBaseRelationship> relationshipsToAdd)> createAction
) where T : class
```

### How It Works

#### Step 1: Snapshot Current State
```csharp
var entitiesSnapshot = new List<XmiBaseEntity>(Entities);
var relationshipsSnapshot = new List<XmiBaseRelationship>(Relationships);
```
Creates copies of the current entities and relationships lists before any modifications.

#### Step 2: Execute Creation Logic
```csharp
var (entity, entitiesToAdd, relationshipsToAdd) = createAction();
```
The `createAction` lambda:
- Creates the new entity
- Checks for existing entities (reuse logic)
- Prepares lists of entities and relationships to add
- Returns a tuple containing:
  - The entity to return (new or existing)
  - List of entities to add to the model
  - List of relationships to add to the model

#### Step 3: Validate Result
```csharp
if (entity == null)
{
    var errorMsg = $"{operationName} failed: Creation returned null.";
    ErrorLog.Add(errorMsg);
    return null;
}
```
Ensures the creation succeeded before committing.

#### Step 4: Commit Transaction
```csharp
foreach (var e in entitiesToAdd)
{
    Entities.Add(e);
}

foreach (var r in relationshipsToAdd)
{
    Relationships.Add(r);
}

return entity;
```
All entities and relationships are added atomically.

#### Step 5: Rollback on Failure
```csharp
catch (Exception ex)
{
    // Restore original state
    Entities.Clear();
    Entities.AddRange(entitiesSnapshot);
    Relationships.Clear();
    Relationships.AddRange(relationshipsSnapshot);

    // Log the error
    var errorMsg = $"{operationName} failed: {ex.Message}";
    ErrorLog.Add(errorMsg);

    // Re-throw with context
    throw new InvalidOperationException(errorMsg, ex);
}
```
If any exception occurs, the model is restored to its pre-transaction state.

## Example: CreateXmiPoint3D

### Before Transaction Pattern
```csharp
public XmiPoint3D CreateXmiPoint3D(...)
{
    if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty");
    if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty");

    try
    {
        // Check for existing point
        var existingPoint = GetEntitiesOfType<XmiPoint3D>().FirstOrDefault(p => p.Equals(tempPoint));
        if (existingPoint != null) return existingPoint;

        // Create new point
        var point = new XmiPoint3D(...);

        // Add directly to model (no rollback if this fails!)
        AddXmiPoint3D(point);

        return point;
    }
    catch (Exception ex)
    {
        throw new InvalidOperationException("Failed to create Point3D.", ex);
    }
}
```

**Problem**: If `AddXmiPoint3D` throws an exception, the point might be partially added. No error logging.

### After Transaction Pattern
```csharp
public XmiPoint3D CreateXmiPoint3D(...)
{
    if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty");
    if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty");

    return ExecuteTransaction<XmiPoint3D>("CreateXmiPoint3D", () =>
    {
        var entitiesToAdd = new List<XmiBaseEntity>();
        var relationshipsToAdd = new List<XmiBaseRelationship>();

        // Check for existing point
        var tempPoint = new XmiPoint3D(...);
        var existingPoint = GetEntitiesOfType<XmiPoint3D>().FirstOrDefault(p => p.Equals(tempPoint));

        if (existingPoint != null)
        {
            // Return existing, no changes to model
            return (existingPoint, entitiesToAdd, relationshipsToAdd);
        }

        // Create new point
        var point = new XmiPoint3D(...);

        // Stage for addition (not yet added to model)
        entitiesToAdd.Add(point);

        // Return staged changes
        return (point, entitiesToAdd, relationshipsToAdd);
    })!;
}
```

**Benefits**:
- ✅ Atomic operation - point is only added if all validations pass
- ✅ Automatic rollback on any failure
- ✅ Error logging for diagnostics
- ✅ Clean separation of business logic and transaction management

## Example: CreateXmiStructuralPointConnection (Complex)

This method demonstrates a more complex scenario with multiple entities and relationships:

```csharp
public XmiStructuralPointConnection CreateXmiStructuralPointConnection(
    string id, string name, string ifcGuid, string nativeId,
    string description, XmiStorey? storey, XmiPoint3D point
)
{
    if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty");
    if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty");

    return ExecuteTransaction<XmiStructuralPointConnection>("CreateXmiStructuralPointConnection", () =>
    {
        var entitiesToAdd = new List<XmiBaseEntity>();
        var relationshipsToAdd = new List<XmiBaseRelationship>();

        // Check for existing connection at same coordinates
        var existingConnectionId = FindMatchingPointConnectionByPoint3D(...);
        if (existingConnectionId != null)
        {
            var existing = GetEntitiesOfType<XmiStructuralPointConnection>()
                .FirstOrDefault(c => c.Id == existingConnectionId);
            return (existing, entitiesToAdd, relationshipsToAdd);
        }

        // Reuse or add storey
        XmiStorey? existingStorey = null;
        if (storey != null)
        {
            existingStorey = GetEntitiesOfType<XmiStorey>()
                .FirstOrDefault(s => s.NativeId == storey.NativeId) ?? storey;
        }

        // Reuse or add point
        var existingPoint = GetEntitiesOfType<XmiPoint3D>()
            .FirstOrDefault(p => p.Equals(point)) ?? point;

        // Create the connection
        var connection = new XmiStructuralPointConnection(...);
        entitiesToAdd.Add(connection);

        // Create relationships
        if (existingStorey != null)
        {
            var storeyRelation = new XmiHasStorey(connection, existingStorey);
            relationshipsToAdd.Add(storeyRelation);
        }

        if (existingPoint != null)
        {
            var pointRelation = new XmiHasPoint3D(connection, existingPoint);
            relationshipsToAdd.Add(pointRelation);
        }

        return (connection, entitiesToAdd, relationshipsToAdd);
    })!;
}
```

### What Happens:
1. **Validation** - ID and name checked before transaction
2. **Duplicate Check** - Searches for existing connection at same coordinates
3. **Entity Reuse** - Finds or uses provided storey and point
4. **Staging** - Prepares connection and relationships for addition
5. **Atomic Commit** - All or nothing:
   - If successful: Connection + both relationships added
   - If failed: Nothing added, state rolled back, error logged

## Error Handling

### Accessing Errors
```csharp
var model = new XmiModel();

try
{
    var point = model.CreateXmiPoint3D("", "Point 1", ...); // Invalid: empty ID
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Operation failed: {ex.Message}");
}

// Check error log
foreach (var error in model.ErrorLog)
{
    Console.WriteLine($"Error: {error}");
}
```

### Error Log Format
```
CreateXmiPoint3D failed: ID cannot be null or empty (Parameter 'id')
CreateXmiStorey failed: Native ID 'LEVEL_1' already exists with different properties
CreateXmiStructuralPointConnection failed: Point3D with coordinates (0,0,0) not found in model
```

## Best Practices

### 1. Validate Early
```csharp
// Validate required parameters BEFORE transaction
if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty");
if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty");

return ExecuteTransaction<T>(...);
```
Simple validation exceptions are thrown immediately without logging.

### 2. Prepare Staging Lists
```csharp
var entitiesToAdd = new List<XmiBaseEntity>();
var relationshipsToAdd = new List<XmiBaseRelationship>();
```
Always initialize these lists at the start of your createAction lambda.

### 3. Return Existing Entities Correctly
```csharp
if (existingEntity != null)
{
    // Return existing with empty staging lists
    return (existingEntity, entitiesToAdd, relationshipsToAdd);
}
```
When reusing an entity, return it with empty staging lists to avoid duplicating work.

### 4. Stage All Changes
```csharp
// Don't call Add methods directly!
// AddXmiPoint3D(point);  ❌ Wrong

// Stage for transactional addition
entitiesToAdd.Add(point);  ✅ Correct
relationshipsToAdd.Add(relationship);  ✅ Correct
```

### 5. Let Transaction Handle Commit
The `ExecuteTransaction` method handles all additions to `Entities` and `Relationships`. Never modify these collections directly in Create methods.

## Comparison: Add vs Create Methods

### Add Methods (Direct Access)
```csharp
public void AddXmiPoint3D(XmiPoint3D point)
{
    Entities.Add(point);  // Direct addition, no transaction
}

public void AddXmiHasPoint3D(XmiHasPoint3D relation)
{
    Relationships.Add(relation);  // Direct addition, no transaction
}
```
- ✅ Fast and simple
- ✅ Full control over what's added
- ❌ No validation
- ❌ No duplicate checking
- ❌ No automatic relationships
- ❌ No rollback on failure
- **Use case**: When you need direct control and handle validation yourself

### Create Methods (Transaction-Based)
```csharp
public XmiPoint3D CreateXmiPoint3D(...)
{
    return ExecuteTransaction<XmiPoint3D>("CreateXmiPoint3D", () =>
    {
        // Validation, duplicate checking, staging, etc.
    })!;
}
```
- ✅ Automatic validation
- ✅ Duplicate checking and reuse
- ✅ Automatic relationship creation
- ✅ Atomic operations with rollback
- ✅ Error logging
- ❌ Slightly more overhead
- **Use case**: Primary API for end users creating entities

## Implementation Checklist

When converting a Create method to use transactions:

- [ ] Add parameter validation before `ExecuteTransaction`
- [ ] Initialize `entitiesToAdd` and `relationshipsToAdd` lists
- [ ] Implement duplicate checking/reuse logic
- [ ] Stage new entities in `entitiesToAdd`
- [ ] Stage new relationships in `relationshipsToAdd`
- [ ] Return tuple: `(entity, entitiesToAdd, relationshipsToAdd)`
- [ ] Remove all direct calls to `Add` methods
- [ ] Update operation name in `ExecuteTransaction` call
- [ ] Test rollback behavior with failing scenarios
- [ ] Verify error logging works correctly

## Future Enhancements

Potential improvements to the transaction system:

1. **Transaction Isolation Levels** - Support for different isolation levels (read committed, serializable, etc.)
2. **Nested Transactions** - Allow Create methods to call other Create methods safely
3. **Transaction Events** - Pre-commit and post-commit hooks for custom logic
4. **Performance Optimization** - Copy-on-write or differential snapshots for large models
5. **Async Transactions** - Support for asynchronous creation operations

---

**Document Version**: 1.0
**Last Updated**: 2025-12-05
**Author**: XMI Schema Core Team
