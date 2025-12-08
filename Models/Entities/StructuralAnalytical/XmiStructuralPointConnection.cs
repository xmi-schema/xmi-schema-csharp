using System;
using XmiSchema.Models.Bases;
using XmiSchema.Models.Commons;
using XmiSchema.Models.Geometries;

namespace XmiSchema.Models.Entities.StructuralAnalytical
{
    /// <summary>
    /// Represents a discrete analytical point that can connect members, nodes, or storey definitions.
    /// </summary>
    public class XmiStructuralPointConnection : XmiBaseStructuralAnalyticalEntity, IEquatable<XmiStructuralPointConnection>
    {
        public XmiStorey? Storey { get; set; }
        public XmiPoint3d? Point { get; set; }

        /// <summary>
        /// Initializes a new <see cref="XmiStructuralPointConnection"/> that can be linked to storeys and geometry.
        /// </summary>
        /// <param name="id">Unique Cross Model identifier.</param>
        /// <param name="name">Descriptive name for UI clients.</param>
        /// <param name="ifcGuid">IFC GUID reference.</param>
        /// <param name="nativeId">Source-system identifier.</param>
        /// <param name="description">Optional notes on the connection.</param>
        public XmiStructuralPointConnection(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description //,
                               // XmiStorey storey,
                               // XmiPoint3d point
        ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiStructuralPointConnection))
        {
            // Storey = storey;
            // Point = point;
        }

        public bool Equals(XmiStructuralPointConnection? other)
        {
            if (other == null) return false;

            return Point != null && Point.Equals(other.Point);
        }

        public override bool Equals(object? obj) => Equals(obj as XmiStructuralPointConnection);

        public override int GetHashCode()
        {
            // Use point's hash code to represent this connection's spatial identity
            return Point?.GetHashCode() ?? 0;
        }
    }
}
