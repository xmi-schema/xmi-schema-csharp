# XmiHasStructuralNode

Relationship linking structural members and segments to their nodal connection points.

## Overview

`XmiHasStructuralNode` represents an association between structural elements (members, segments) and their connection nodes (`XmiStructuralPointConnection`). This relationship defines the topology of the structural system by connecting elements to discrete points.

## Location

`Models/Relationships/XmiHasStructrralNode.cs`

**Note**: There is a typo in the filename ("Structrral" instead of "Structural"), but the class name is correct.

## Inheritance

```
XmiBaseRelationship → XmiHasStructuralNode
```

## UML Type

**Association** - Represents connectivity between structural elements and nodes.

## Constructors

### Full Constructor

```csharp
public XmiHasStructuralNode(
    string id,
    XmiBaseEntity source,
    XmiBaseEntity target,
    string name,
    string description,
    string entityName,
    string umlType
)
```

### Simplified Constructor (Auto-generated ID)

```csharp
public XmiHasStructuralNode(
    XmiBaseEntity source,
    XmiBaseEntity target
)
```

## Automatic Creation

This relationship is automatically created by `ExtensionRelationshipExporter` for multiple scenarios:

### 1. Curve Member Nodes Collection

```csharp
if (entity is XmiStructuralCurveMember curveWithNodes)
{
    foreach (var node in curveWithNodes.Nodes)
    {
        _relationshipManager.AddRelationship(
            new XmiHasStructuralNode(curveWithNodes, node)
        );
    }
}
```

### 2. Curve Member Begin/End Nodes

```csharp
if (entity is XmiStructuralCurveMember curve && curve.BeginNode != null)
{
    _relationshipManager.AddRelationship(
        new XmiHasStructuralNode(curve, curve.BeginNode)
    );
}

if (entity is XmiStructuralCurveMember curve && curve.EndNode != null)
{
    _relationshipManager.AddRelationship(
        new XmiHasStructuralNode(curve, curve.EndNode)
    );
}
```

### 3. Surface Member Nodes

```csharp
if (entity is XmiStructuralSurfaceMember surfaceWithNodes)
{
    foreach (var node in surfaceWithNodes.Nodes)
    {
        _relationshipManager.AddRelationship(
            new XmiHasStructuralNode(surfaceWithNodes, node)
        );
    }
}
```

### 4. Segment Nodes

```csharp
if (entity is XmiSegment segment)
{
    if (segment.BeginNode != null)
        _relationshipManager.AddRelationship(
            new XmiHasStructuralNode(segment, segment.BeginNode)
        );

    if (segment.EndNode != null)
        _relationshipManager.AddRelationship(
            new XmiHasStructuralNode(segment, segment.EndNode)
        );
}
```

## Usage Examples

### Beam with End Nodes

```csharp
// Create nodes
var node1 = new XmiStructuralPointConnection("N1", "Node-1", ...);
var node2 = new XmiStructuralPointConnection("N2", "Node-2", ...);

// Create beam
var beam = new XmiStructuralCurveMember(
    id: "B001",
    name: "Beam-1",
    beginNode: node1,
    endNode: node2,
    nodes: new List<XmiStructuralPointConnection> { node1, node2 },
    ...
);

// Add to model
var builder = new XmiSchemaModelBuilder();
builder.AddEntities(new[] { node1, node2 });
builder.AddEntity(beam);

var model = builder.BuildModel();
// Relationships automatically created:
// - beam → node1 (begin)
// - beam → node2 (end)
// - beam → node1 (from nodes collection)
// - beam → node2 (from nodes collection)
```

### Slab with Boundary Nodes

```csharp
// Create corner nodes
var corner1 = new XmiStructuralPointConnection("C1", ...);
var corner2 = new XmiStructuralPointConnection("C2", ...);
var corner3 = new XmiStructuralPointConnection("C3", ...);
var corner4 = new XmiStructuralPointConnection("C4", ...);

// Create slab
var slab = new XmiStructuralSurfaceMember(
    id: "SLB001",
    name: "Floor-Slab",
    nodes: new List<XmiStructuralPointConnection> { corner1, corner2, corner3, corner4 },
    ...
);

builder.AddEntities(new[] { corner1, corner2, corner3, corner4 });
builder.AddEntity(slab);

model = builder.BuildModel();
// Relationships automatically created:
// - slab → corner1
// - slab → corner2
// - slab → corner3
// - slab → corner4
```

### Segment with Begin/End Nodes

```csharp
var startNode = new XmiStructuralPointConnection("START", ...);
var endNode = new XmiStructuralPointConnection("END", ...);

var segment = new XmiSegment(
    id: "SEG001",
    name: "Segment-1",
    beginNode: startNode,
    endNode: endNode,
    ...
);

builder.AddEntities(new[] { startNode, endNode });
builder.AddEntity(segment);

model = builder.BuildModel();
// Relationships automatically created:
// - segment → startNode
// - segment → endNode
```

## Graph Representations

### Curve Member (Beam)

```
                    XmiStructuralPointConnection (BeginNode)
                            ↑
                            │ [XmiHasStructuralNode]
                            │
XmiStructuralCurveMember ───┤
                            │
                            │ [XmiHasStructuralNode]
                            ↓
                    XmiStructuralPointConnection (EndNode)
```

### Surface Member (Slab)

```
                    XmiStructuralPointConnection (Corner 1)
                            ↑
                            │ [XmiHasStructuralNode]
XmiStructuralSurfaceMember ─┼─> XmiStructuralPointConnection (Corner 2)
                            │
                            ├─> XmiStructuralPointConnection (Corner 3)
                            │
                            └─> XmiStructuralPointConnection (Corner 4)
```

## JSON Export Example

```json
{
  "XmiHasStructuralNode": {
    "ID": "rel-003",
    "Source": "B001",
    "Target": "N1",
    "Name": "XmiHasStructuralNode",
    "Description": "",
    "EntityType": "XmiHasStructuralNode",
    "UmlType": "Association"
  }
}
```

## Connectivity Analysis

This relationship enables:

1. **Topology Analysis**: Finding all elements connected at a node
2. **Continuity Checking**: Verifying structural connectivity
3. **Joint Detection**: Identifying member intersections
4. **Load Path Analysis**: Tracing force distribution

### Query Example

```csharp
// Find all members connected to a specific node
var relationshipManager = new RelationshipManager<XmiBaseRelationship>();
var nodeId = "N1";

var connectedMembers = relationshipManager
    .FindByTarget(nodeId)
    .Where(r => r is XmiHasStructuralNode)
    .Select(r => r.Source);

Console.WriteLine($"Members connected to {nodeId}:");
foreach (var member in connectedMembers)
{
    Console.WriteLine($"  - {member.ID}: {member.Name}");
}
```

## Related Classes

- **XmiBaseRelationship**: Base class
- **XmiStructuralPointConnection**: Target entity (node)
- **XmiStructuralCurveMember**: Source entity (beam, column, brace)
- **XmiStructuralSurfaceMember**: Source entity (slab, wall)
- **XmiSegment**: Source entity (member segment)
- **ExtensionRelationshipExporter**: Creates this relationship automatically

## See Also

- [XmiStructuralPointConnection](../Entities/XmiStructuralPointConnection.README.md) - Node definition
- [XmiStructuralCurveMember](../Entities/XmiStructuralCurveMember.README.md) - Curve member
- [XmiStructuralSurfaceMember](../Entities/XmiStructuralSurfaceMember.README.md) - Surface member
- [XmiSegment](../Entities/XmiSegment.README.md) - Segment
- [XmiBaseRelationship](../Bases/XmiBaseRelationship.README.md) - Base class
