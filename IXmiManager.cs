using XmiSchema.Core.Entities;
using XmiSchema.Core.Geometries;
using XmiSchema.Core.Relationships;
using XmiSchema.Core.Models;
using XmiSchema.Core.Enums;

namespace XmiSchema.Core.Manager
{
    /// <summary>
    /// Contract for components that manage Cross Model Information graphs and helper creation routines.
    /// </summary>
    public interface IXmiManager
    {
        /// <summary>
        /// Gets or sets the collection of XMI models managed by this instance.
        /// </summary>
        List<XmiModel> Models { get; set; }

        /// <summary>
        /// Adds a structural material instance to the specified model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the <see cref="XmiModel"/> in <see cref="Models"/>.</param>
        /// <param name="material">Material entity that should be appended to the model.</param>
        void AddXmiStructuralMaterialToModel(int modelIndex, XmiStructuralMaterial material);

        /// <summary>
        /// Adds a structural cross-section to the requested model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the model receiving the cross-section.</param>
        /// <param name="crossSection">Cross-section entity to add.</param>
        void AddXmiStructuralCrossSectionToModel(int modelIndex, XmiStructuralCrossSection crossSection);

        /// <summary>
        /// Adds a structural curve member to the requested model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="member">Curve member entity to persist.</param>
        void AddXmiStructuralCurveMemberToModel(int modelIndex, XmiStructuralCurveMember member);

        /// <summary>
        /// Adds a structural surface member to the requested model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="member">Surface member entity to persist.</param>
        void AddXmiStructuralSurfaceMemberToModel(int modelIndex, XmiStructuralSurfaceMember member);

        /// <summary>
        /// Adds a structural point connection entity to the requested model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="connection">Point connection entity to add.</param>
        void AddXmiStructuralPointConnectionToModel(int modelIndex, XmiStructuralPointConnection connection);

        /// <summary>
        /// Adds a structural storey entity to the requested model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="storey">Storey entity to add.</param>
        void AddXmiStructuralStoreyToModel(int modelIndex, XmiStructuralStorey storey);

        /// <summary>
        /// Adds a point entity to the requested model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="point">Point to append to the model.</param>
        void AddXmiPoint3DToModel(int modelIndex, XmiPoint3D point);

        /// <summary>
        /// Adds a batch of base entities to the requested model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="entities">Entities to append to the model verbatim.</param>
        void AddEntitiesToModel(int modelIndex, List<XmiBaseEntity> entities);

        /// <summary>
        /// Adds a relationship connecting a source entity to a point geometry.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="relation">Relationship binding a source entity and point geometry.</param>
        void AddXmiHasPoint3DToModel(int modelIndex, XmiHasPoint3D relation);

        /// <summary>
        /// Adds a relationship connecting an entity to a structural material.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="relation">Relationship referencing the structural material.</param>
        void AddXmiHasStructuralMaterialToModel(int modelIndex, XmiHasStructuralMaterial relation);

        /// <summary>
        /// Adds a relationship that links a structural node (point connection) to other entities.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="relation">Relationship referencing the structural node.</param>
        void AddXmiHasStructuralNodeToModel(int modelIndex, XmiHasStructuralNode relation);

        /// <summary>
        /// Adds a relationship that binds a structural cross-section to an owning entity.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="relation">Relationship referencing the cross-section.</param>
        void AddXmiHasStructuralCrossSectionToModel(int modelIndex, XmiHasStructuralCrossSection relation);

        /// <summary>
        /// Adds a relationship between storeys and the structural elements they contain.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="relation">Relationship referencing the storey.</param>
        void AddXmiHasStoreyToModel(int modelIndex, XmiHasStructuralStorey relation);

        /// <summary>
        /// Retrieves entities of a specific type from the target model.
        /// </summary>
        /// <typeparam name="T">Concrete entity type to filter.</typeparam>
        /// <param name="modelIndex">Zero-based index of the model to query.</param>
        /// <returns>List containing all entities of <typeparamref name="T"/>.</returns>
        List<T> GetEntitiesOfType<T>(int modelIndex) where T : XmiBaseEntity;

        /// <summary>
        /// Retrieves the first entity whose identifier matches the supplied id.
        /// </summary>
        /// <typeparam name="T">Concrete entity type expected in the result.</typeparam>
        /// <param name="modelIndex">Zero-based index of the model to query.</param>
        /// <param name="id">Entity identifier to match.</param>
        /// <returns>Entity instance or <c>null</c> when not found.</returns>
        T? GetEntityById<T>(int modelIndex, string id) where T : XmiBaseEntity;

        /// <summary>
        /// Creates a point geometry within the requested model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="id">Stable identifier for the point.</param>
        /// <param name="name">Human readable name.</param>
        /// <param name="ifcGuid">Source IFC GUID, when applicable.</param>
        /// <param name="nativeId">Native application identifier.</param>
        /// <param name="description">Optional description for downstream consumers.</param>
        /// <param name="x">X coordinate in model units.</param>
        /// <param name="y">Y coordinate in model units.</param>
        /// <param name="z">Z coordinate in model units.</param>
        /// <returns>The created <see cref="XmiPoint3D"/> instance.</returns>
        XmiPoint3D CreatePoint3D(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            double x,
            double y,
            double z
        );

        /// <summary>
        /// Creates a structural material definition within the requested model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="id">Stable identifier for the material.</param>
        /// <param name="name">Human readable name.</param>
        /// <param name="ifcGuid">Optional IFC GUID reference.</param>
        /// <param name="nativeId">Native identifier within the authoring system.</param>
        /// <param name="description">Optional description for the material.</param>
        /// <param name="materialType">Type classification for the material.</param>
        /// <param name="grade">Material grade number.</param>
        /// <param name="unitWeight">Density in kN/m^3 or similar units.</param>
        /// <param name="eModulus">Elastic modulus string as defined in source.</param>
        /// <param name="gModulus">Shear modulus string.</param>
        /// <param name="poissonRatio">Poisson ratio.</param>
        /// <param name="thermalCoefficient">Thermal coefficient.</param>
        /// <returns>The created <see cref="XmiStructuralMaterial"/> instance.</returns>
        XmiStructuralMaterial CreateStructuralMaterial(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiStructuralMaterialTypeEnum materialType,
            double grade,
            double unitWeight,
            string eModulus,
            string gModulus,
            string poissonRatio,
            double thermalCoefficient
        );

        /// <summary>
        /// Creates a structural cross-section entity under the specified model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="id">Stable identifier for the cross-section.</param>
        /// <param name="name">Human readable name.</param>
        /// <param name="ifcGuid">Optional IFC GUID reference.</param>
        /// <param name="nativeId">Native identifier within the authoring system.</param>
        /// <param name="description">Optional description for the cross-section.</param>
        /// <param name="material">Material to associate with the cross-section.</param>
        /// <param name="shape">Shape enumeration for downstream conversions.</param>
        /// <param name="parameters">Raw parameter list describing section profile.</param>
        /// <param name="area">Gross area.</param>
        /// <param name="secondMomentOfAreaXAxis">Second moment around x-axis.</param>
        /// <param name="secondMomentOfAreaYAxis">Second moment around y-axis.</param>
        /// <param name="radiusOfGyrationXAxis">Radius of gyration along x-axis.</param>
        /// <param name="radiusOfGyrationYAxis">Radius of gyration along y-axis.</param>
        /// <param name="elasticModulusXAxis">Elastic modulus about x-axis.</param>
        /// <param name="elasticModulusYAxis">Elastic modulus about y-axis.</param>
        /// <param name="plasticModulusXAxis">Plastic modulus about x-axis.</param>
        /// <param name="plasticModulusYAxis">Plastic modulus about y-axis.</param>
        /// <param name="torsionalConstant">Torsional constant.</param>
        /// <returns>The created <see cref="XmiStructuralCrossSection"/>.</returns>
        XmiStructuralCrossSection CreateStructuralCrossSection(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiStructuralMaterial? material,
            XmiShapeEnum shape,
            string[] parameters,
            double area,
            double secondMomentOfAreaXAxis,
            double secondMomentOfAreaYAxis,
            double radiusOfGyrationXAxis,
            double radiusOfGyrationYAxis,
            double elasticModulusXAxis,
            double elasticModulusYAxis,
            double plasticModulusXAxis,
            double plasticModulusYAxis,
            double torsionalConstant
        );

        /// <summary>
        /// Creates a structural storey record for the requested model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="id">Stable identifier for the storey.</param>
        /// <param name="name">Human readable name.</param>
        /// <param name="ifcGuid">Optional IFC GUID reference.</param>
        /// <param name="nativeId">Native identifier within the authoring system.</param>
        /// <param name="description">Optional description for the storey.</param>
        /// <param name="storeyElevation">Elevation relative to global datum.</param>
        /// <param name="storeyMass">Total mass assigned to the storey.</param>
        /// <param name="storeyHorizontalReactionX">Horizontal reaction along x-axis.</param>
        /// <param name="storeyHorizontalReactionY">Horizontal reaction along y-axis.</param>
        /// <param name="storeyVerticalReaction">Vertical reaction.</param>
        /// <returns>The created <see cref="XmiStructuralStorey"/>.</returns>
        XmiStructuralStorey CreateStructuralStorey(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            double storeyElevation,
            double storeyMass,
            string storeyHorizontalReactionX,
            string storeyHorizontalReactionY,
            string storeyVerticalReaction
        );

        /// <summary>
        /// Creates a structural curve member, including topology references and offsets.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="id">Stable identifier for the curve member.</param>
        /// <param name="name">Human readable name.</param>
        /// <param name="ifcGuid">Optional IFC GUID reference.</param>
        /// <param name="nativeId">Native identifier within the authoring system.</param>
        /// <param name="description">Optional description for the member.</param>
        /// <param name="crossSection">Cross-section applied to the member.</param>
        /// <param name="storey">Storey containing the member.</param>
        /// <param name="curveMemberType">Curve member classification.</param>
        /// <param name="nodes">Nodes participating in the member.</param>
        /// <param name="segments">Optional explicit segments describing geometry.</param>
        /// <param name="systemLine">System line type.</param>
        /// <param name="beginNode">Starting node reference.</param>
        /// <param name="endNode">Ending node reference.</param>
        /// <param name="length">Physical length.</param>
        /// <param name="localAxisX">Local axis X vector.</param>
        /// <param name="localAxisY">Local axis Y vector.</param>
        /// <param name="localAxisZ">Local axis Z vector.</param>
        /// <param name="beginNodeXOffset">X offset at begin node.</param>
        /// <param name="endNodeXOffset">X offset at end node.</param>
        /// <param name="beginNodeYOffset">Y offset at begin node.</param>
        /// <param name="endNodeYOffset">Y offset at end node.</param>
        /// <param name="beginNodeZOffset">Z offset at begin node.</param>
        /// <param name="endNodeZOffset">Z offset at end node.</param>
        /// <param name="endFixityStart">Fixity condition at start.</param>
        /// <param name="endFixityEnd">Fixity condition at end.</param>
        /// <returns>The created <see cref="XmiStructuralCurveMember"/>.</returns>
        XmiStructuralCurveMember CreateStructuralCurveMember(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiStructuralCrossSection crossSection,
            XmiStructuralStorey storey,
            XmiStructuralCurveMemberTypeEnum curveMemberType,
            List<XmiStructuralPointConnection> nodes,
            List<XmiSegment>? segments,
            XmiStructuralCurveMemberSystemLineEnum systemLine,
            XmiStructuralPointConnection beginNode,
            XmiStructuralPointConnection endNode,
            double length,
            string localAxisX,
            string localAxisY,
            string localAxisZ,
            double beginNodeXOffset,
            double endNodeXOffset,
            double beginNodeYOffset,
            double endNodeYOffset,
            double beginNodeZOffset,
            double endNodeZOffset,
            string endFixityStart,
            string endFixityEnd
        );

        /// <summary>
        /// Creates a structural surface member entity for the specified model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="id">Stable identifier for the surface member.</param>
        /// <param name="name">Human readable name.</param>
        /// <param name="ifcGuid">Optional IFC GUID reference.</param>
        /// <param name="nativeId">Native identifier within the authoring system.</param>
        /// <param name="description">Optional description for the surface.</param>
        /// <param name="material">Material applied to the surface.</param>
        /// <param name="surfaceMemberType">Surface member classification.</param>
        /// <param name="thickness">Member thickness.</param>
        /// <param name="systemPlane">Plane classification.</param>
        /// <param name="nodes">Nodes defining the surface boundary.</param>
        /// <param name="storey">Storey containing the surface.</param>
        /// <param name="segments">Segments describing surface outline.</param>
        /// <param name="area">Surface area.</param>
        /// <param name="zOffset">Vertical offset.</param>
        /// <param name="localAxisX">Local axis X vector.</param>
        /// <param name="localAxisY">Local axis Y vector.</param>
        /// <param name="localAxisZ">Local axis Z vector.</param>
        /// <param name="height">Surface height or extrusion.</param>
        /// <returns>The created <see cref="XmiStructuralSurfaceMember"/>.</returns>
        XmiStructuralSurfaceMember CreateStructuralSurfaceMember(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiStructuralMaterial material,
            XmiStructuralSurfaceMemberTypeEnum surfaceMemberType,
            double thickness,
            XmiStructuralSurfaceMemberSystemPlaneEnum systemPlane,
            List<XmiStructuralPointConnection> nodes,
            XmiStructuralStorey storey,
            List<XmiSegment> segments,
            double area,
            double zOffset,
            string localAxisX,
            string localAxisY,
            string localAxisZ,
            double height
        );

        /// <summary>
        /// Creates a structural point connection bound to a storey and point definition.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="id">Stable identifier for the point connection.</param>
        /// <param name="name">Human readable name.</param>
        /// <param name="ifcGuid">Optional IFC GUID reference.</param>
        /// <param name="nativeId">Native identifier within the authoring system.</param>
        /// <param name="description">Optional description for the point connection.</param>
        /// <param name="storey">Storey containing the connection.</param>
        /// <param name="point">Point geometry represented by the connection.</param>
        /// <returns>The created <see cref="XmiStructuralPointConnection"/>.</returns>
        XmiStructuralPointConnection CreateStructuralPointConnection(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiStructuralStorey storey,
            XmiPoint3D point
        );

        /// <summary>
        /// Builds the JSON payload representation for the provided model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the model to serialize.</param>
        /// <returns>JSON string depicting the model.</returns>
        string BuildJson(int modelIndex);

        /// <summary>
        /// Persists the full JSON output to the provided file path.
        /// </summary>
        /// <param name="path">Target path for the serialized output.</param>
        void Save(string path);
    }
}
