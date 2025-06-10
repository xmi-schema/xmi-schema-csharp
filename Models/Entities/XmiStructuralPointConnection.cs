using System;
using XmiSchema.Core.Enums;
using XmiSchema.Core.Geometries;

namespace XmiSchema.Core.Entities
{
    public class XmiStructuralPointConnection : XmiBaseEntity, IEquatable<XmiStructuralPointConnection>
    {
        public XmiStructuralStorey Storey { get; set; }
        public XmiPoint3D Point { get; set; }

        public XmiStructuralPointConnection(
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiStructuralStorey storey,
            XmiPoint3D point
        ) : base(id, name, ifcGuid, nativeId, description, nameof(XmiStructuralPointConnection))
        {
            Storey = storey;
            Point = point;
        }

        public bool Equals(XmiStructuralPointConnection other)
        {
            if (other == null) return false;

            return Point != null && Point.Equals(other.Point);
        }

        public override bool Equals(object obj) => Equals(obj as XmiStructuralPointConnection);

        public override int GetHashCode()
        {
            // Use point's hash code to represent this connection's spatial identity
            return Point?.GetHashCode() ?? 0;
        }
    }
}
