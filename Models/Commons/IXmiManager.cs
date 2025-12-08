using XmiSchema.Models.Relationships;
using XmiSchema.Models.Entities.StructuralAnalytical;
using XmiSchema.Models.Enums;
using XmiSchema.Models.Parameters;
using XmiSchema.Models.Entities.Physical;
using XmiSchema.Models.Geometries;
using XmiSchema.Models.Bases;

namespace XmiSchema.Models.Commons
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
        void AddXmiMaterialToModel(int modelIndex, XmiMaterial material);

        /// <summary>
        /// Adds a structural cross-section to the requested model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the model receiving the cross-section.</param>
        /// <param name="crossSection">Cross-section entity to add.</param>
        void AddXmiCrossSectionToModel(int modelIndex, XmiCrossSection crossSection);

        /// <summary>
        /// Adds a beam physical element to the requested model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="beam">Beam entity to add.</param>
        void AddXmiBeamToModel(int modelIndex, XmiBeam beam);

        /// <summary>
        /// Adds a column physical element to the requested model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="column">Column entity to add.</param>
        void AddXmiColumnToModel(int modelIndex, XmiColumn column);

        /// <summary>
        /// Adds a slab physical element to the requested model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="slab">Slab entity to add.</param>
        void AddXmiSlabToModel(int modelIndex, XmiSlab slab);

        /// <summary>
        /// Adds a wall physical element to the requested model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="wall">Wall entity to add.</param>
        void AddXmiWallToModel(int modelIndex, XmiWall wall);

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
        void AddXmiStoreyToModel(int modelIndex, XmiStorey storey);

        /// <summary>
        /// Adds a point entity to the requested model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="point">Point to append to the model.</param>
        void AddXmiPoint3DToModel(int modelIndex, XmiPoint3d point);

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
        void AddXmiHasPoint3DToModel(int modelIndex, XmiHasPoint3d relation);

        /// <summary>
        /// Adds a relationship connecting an entity to a structural material.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="relation">Relationship referencing the structural material.</param>
        void AddXmiHasMaterialToModel(int modelIndex, XmiHasMaterial relation);

        /// <summary>
        /// Adds a relationship that links a structural node (point connection) to other entities.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="relation">Relationship referencing the structural node.</param>
        void AddXmiHasStructuralPointConnectionToModel(int modelIndex, XmiHasStructuralPointConnection relation);

        /// <summary>
        /// Adds a relationship that binds a structural cross-section to an owning entity.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="relation">Relationship referencing the cross-section.</param>
        void AddXmiHasCrossSectionToModel(int modelIndex, XmiHasCrossSection relation);

        /// <summary>
        /// Adds a relationship between storeys and the structural elements they contain.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="relation">Relationship referencing the storey.</param>
        void AddXmiHasStoreyToModel(int modelIndex, XmiHasStorey relation);

        /// <summary>
        /// Adds a relationship connecting a physical element to its structural curve member.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="relation">Relationship referencing the analytical curve member.</param>
        void AddXmiHasStructuralCurveMemberToModel(int modelIndex, XmiHasStructuralCurveMember relation);

        /// <summary>
        /// Retrieves entities of a specific type from the target model.
        /// </summary>
        /// <typeparam name="T">Concrete entity type to filter.</typeparam>
        /// <param name="modelIndex">Zero-based index of the model to query.</param>
        /// <returns>List containing all entities of <typeparamref name="T"/>.</returns>
        List<T> GetXmiEntitiesOfType<T>(int modelIndex) where T : XmiBaseEntity;

        /// <summary>
        /// Retrieves the first entity whose identifier matches the supplied id.
        /// </summary>
        /// <typeparam name="T">Concrete entity type expected in the result.</typeparam>
        /// <param name="modelIndex">Zero-based index of the model to query.</param>
        /// <param name="id">Entity identifier to match.</param>
        /// <returns>Entity instance or <c>null</c> when not found.</returns>
        T? GetXmiEntityById<T>(int modelIndex, string id) where T : XmiBaseEntity;

        /// <summary>
        /// Creates a beam physical element in the specified model.
        /// </summary>
        XmiBeam CreateXmiBeam(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterial? material,
            XmiSystemLineEnum systemLine,
            double length,
            string localAxisX,
            string localAxisY,
            string localAxisZ,
            double beginNodeXOffset,
            double endNodeXOffset,
            double beginNodeYOffset,
            double endNodeYOffset,
            double beginNodeZOffset,
            double endNodeZOffset
        );

        /// <summary>
        /// Creates a column physical element in the specified model.
        /// </summary>
        XmiColumn CreateXmiColumn(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterial? material,
            XmiSystemLineEnum systemLine,
            double length,
            string localAxisX,
            string localAxisY,
            string localAxisZ,
            double beginNodeXOffset,
            double endNodeXOffset,
            double beginNodeYOffset,
            double endNodeYOffset,
            double beginNodeZOffset,
            double endNodeZOffset
        );

        /// <summary>
        /// Creates a slab physical element in the specified model.
        /// </summary>
        XmiSlab CreateXmiSlab(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterial? material
        );

        /// <summary>
        /// Creates a wall physical element in the specified model.
        /// </summary>
        XmiWall CreateXmiWall(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterial? material
        );

        /// <summary>
        /// Finds a structural point connection in the specified model that references the same point coordinates as the provided connection.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the model to query.</param>
        /// <param name="inputConnection">Connection to match.</param>
        /// <returns>The ID for the matching connection if found; otherwise null.</returns>
        string? FindMatchingPointConnectionByPoint3D(int modelIndex, XmiStructuralPointConnection inputConnection);

        /// <summary>
        /// Alias for <see cref="FindMatchingPointConnectionByPoint3D"/> matching the XmiModel signature for parity.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the model to query.</param>
        /// <param name="inputConnection">Connection to match.</param>
        /// <returns>The ID for the matching connection if found; otherwise null.</returns>
        string? FindMatchingXmiStructuralPointConnectionByPoint3D(int modelIndex, XmiStructuralPointConnection inputConnection);

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
        /// <returns>The created <see cref="XmiPoint3d"/> instance.</returns>
        XmiPoint3d CreateXmiPoint3D(
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
        /// <returns>The created <see cref="XmiMaterial"/> instance.</returns>
        XmiMaterial CreateXmiMaterial(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterialTypeEnum materialType,
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
        /// <param name="parameters">Strongly typed parameter set describing the section profile.</param>
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
        /// <returns>The created <see cref="XmiCrossSection"/>.</returns>
        XmiCrossSection CreateXmiCrossSection(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterial? material,
            XmiShapeEnum shape,
            IXmiShapeParameters parameters,
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
        /// Creates a storey record for the requested model.
        /// </summary>
        /// <param name="modelIndex">Zero-based index of the destination model.</param>
        /// <param name="id">Stable identifier for the storey.</param>
        /// <param name="name">Human readable name.</param>
        /// <param name="ifcGuid">Optional IFC GUID reference.</param>
        /// <param name="nativeId">Native identifier within the authoring system.</param>
        /// <param name="description">Optional description for the storey.</param>
        /// <param name="storeyElevation">Elevation relative to global datum.</param>
        /// <param name="storeyMass">Total mass assigned to the storey.</param>
        /// <returns>The created <see cref="XmiStorey"/>.</returns>
        XmiStorey CreateXmiStorey(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            double storeyElevation,
            double storeyMass
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
        /// <param name="material">Optional material applied to the member.</param>
        /// <param name="crossSection">Optional cross-section applied to the member.</param>
        /// <param name="storey">Optional storey containing the member.</param>
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
        XmiStructuralCurveMember CreateXmiStructuralCurveMember(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterial? material,
            XmiCrossSection? crossSection,
            XmiStorey? storey,
            XmiStructuralCurveMemberTypeEnum curveMemberType,
            List<XmiStructuralPointConnection> nodes,
            List<XmiSegment>? segments,
            XmiSystemLineEnum systemLine,
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
        /// <param name="material">Optional material applied to the surface.</param>
        /// <param name="surfaceMemberType">Surface member classification.</param>
        /// <param name="thickness">Member thickness.</param>
        /// <param name="systemPlane">Plane classification.</param>
        /// <param name="nodes">Nodes defining the surface boundary.</param>
        /// <param name="storey">Optional storey containing the surface.</param>
        /// <param name="segments">Segments describing surface outline.</param>
        /// <param name="area">Surface area.</param>
        /// <param name="zOffset">Vertical offset.</param>
        /// <param name="localAxisX">Local axis X vector.</param>
        /// <param name="localAxisY">Local axis Y vector.</param>
        /// <param name="localAxisZ">Local axis Z vector.</param>
        /// <param name="height">Surface height or extrusion.</param>
        /// <returns>The created <see cref="XmiStructuralSurfaceMember"/>.</returns>
        XmiStructuralSurfaceMember CreateXmiStructuralSurfaceMember(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiMaterial? material,
            XmiStructuralSurfaceMemberTypeEnum surfaceMemberType,
            double thickness,
            XmiStructuralSurfaceMemberSystemPlaneEnum systemPlane,
            List<XmiStructuralPointConnection> nodes,
            XmiStorey? storey,
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
        /// <param name="storey">Optional storey containing the connection.</param>
        /// <param name="point">Point geometry represented by the connection.</param>
        /// <returns>The created <see cref="XmiStructuralPointConnection"/>.</returns>
        XmiStructuralPointConnection CreateXmiStructuralPointConnection(
            int modelIndex,
            string id,
            string name,
            string ifcGuid,
            string nativeId,
            string description,
            XmiStorey? storey,
            XmiPoint3d point
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
