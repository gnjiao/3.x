using System;
using System.Collections.ObjectModel;
using System.Windows;

// ReSharper disable InconsistentNaming
namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class InspectionSchema
    {
        public InspectionSchema()
        {
        }

        public int ImagePixelWidth { get; set; }
        public int ImagePixelHeight { get; set; }
        public string TestImageFilePath { get; set; }
        public string TestImagesDirectory { get; set; }

        public string EdgeSearching_InspectorName { get; set; }
        public bool EdgeSearching_Disabled { get; set; }
        public bool EdgeSearching_SaveCacheImage_Disabled { get; set; }

        public string CircleSearching_InspectorName { get; set; }
        public bool CircleSearching_Disabled { get; set; }
        public bool CircleSearching_SaveCacheImage_Disabled { get; set; }

        public string Defects_InspectorName { get; set; }
        public bool Defects_Disabled { get; set; }
        public bool Defects_SaveCacheImage_Disabled { get; set; }

        public string Surfaces_InspectorName { get; set; }

        public bool PartSearching_Disabled { get; set; }
        public bool PartSearching_SaveCacheImage_Disabled { get; set; }
        public bool CoordinateRegions_Display_Disabled { get; set; }

        // Coordinate
        public Collection<CircleSearchingDefinition> CoordinateCircles { get; set; } = new Collection<CircleSearchingDefinition>();
        public Collection<EdgeSearchingDefinition> CoordinateEdges { get; set; } = new Collection<EdgeSearchingDefinition>();
        public Collection<RegionSearchingDefinition> CoordinateRegions { get; set; } = new Collection<RegionSearchingDefinition>();
        public Collection<ICoordinateExtactor> CoordinateExtactors { get; set; } = new Collection<ICoordinateExtactor>();

        // Datum Plane
        
        public IDatumPlaneCreator DatumPlaneCreator { get; set; }

        //
        public Collection<EdgeSearchingDefinition> EdgeSearchingDefinitions { get; set; } = new Collection<EdgeSearchingDefinition>();
        public Collection<PartSearchingDefinition> PartSearchingDefinitions { get; set; } = new Collection<PartSearchingDefinition>();
        public Collection<CircleSearchingDefinition> CircleSearchingDefinitions { get; set; } = new Collection<CircleSearchingDefinition>();
        public Collection<DistanceBetweenLinesDefinition> DistanceBetweenLinesDefinitions { get; set; } = new Collection<DistanceBetweenLinesDefinition>();
        public Collection<DistanceBetweenLinesDefinition> DistanceBetweenIntersectionPointsDefinitions { get; set; } = new Collection<DistanceBetweenLinesDefinition>();
        public Collection<SurfaceDefinition> SurfaceDefinitions { get; set; } = new Collection<SurfaceDefinition>();
        public Collection<DefectDefinition> DefectDefinitions { get; set; } = new Collection<DefectDefinition>();
        [Obsolete("Use RegionSearchingDefinition instead")]
        public Collection<RegionTargetDefinition> RegionTargetDefinitions { get; set; } = new Collection<RegionTargetDefinition>();
        public Collection<RegionSearchingDefinition> RegionSearchingDefinitions { get; set; } = new Collection<RegionSearchingDefinition>();
        public Collection<DataCodeSearchingDefinition> DataCodeSearchingDefinitions { get; set; } = new Collection<DataCodeSearchingDefinition>();
        public Collection<XldSearchingDefinition> XldSearchingDefinitions { get; set; } = new Collection<XldSearchingDefinition>();

        public Collection<PointDefinition> ReferencePointDefinitions { get; set; } = new Collection<PointDefinition>();
        public Collection<LineDefinition> ReferenceLineDefinitions { get; set; } = new Collection<LineDefinition>();
        public Collection<RadialLineDefinition> ReferenceRadialLineDefinitions { get; set; } = new Collection<RadialLineDefinition>();

        public Collection<PointOfXldAndRadialLineDefinition> PointOfXldAndRadialLineDefinitions { get; set; } = new Collection<PointOfXldAndRadialLineDefinition>();
        public Collection<PointOfEdgeAndRadialLineDefinition> PointOfEdgeAndRadialLineDefinitions { get; set; } = new Collection<PointOfEdgeAndRadialLineDefinition>();
        public Collection<DistanceBetweenPointsOfXldAndRadialLineDefinition> DistanceBetweenPointsOfXldAndRadialLineDefinitions { get; set; } = new Collection<DistanceBetweenPointsOfXldAndRadialLineDefinition>();
        public Collection<IntersectionPointOfTwoShapesDefinition> IntersectionPointOfTwoShapesDefinitions { get; set; } = new Collection<IntersectionPointOfTwoShapesDefinition>();

        public Collection<StepFromGrayValueDefinition> StepFromGrayValueDefinitions { get; set; } = new Collection<StepFromGrayValueDefinition>();
        public Collection<DistanceBetweenTwoPointsDefinition> DistanceBetweenTwoPointsDefinitions { get; set; } = new Collection<DistanceBetweenTwoPointsDefinition>();

        public CoordinateType CoordinateType { get; set; }
        public Direction CoordinateType_RegionCenter_Direction { get; set; } = Direction.Center;

        public bool CoordinateOriginOffsetEnable { get; set; }
        public double CoordinateOriginOffsetX { get; set; }
        public double CoordinateOriginOffsetY { get; set; }
        public UnitType CoordinateOriginOffsetUnitType { get; set; }

        public bool Disabled { get; set; }
        public string Comment { get; set; }

        /// <summary>
        /// length per pixel in milimeter
        /// </summary>
        public double PixelCellSideLengthInMillimeter { get; set; }

        /// <summary>
        /// the height per gray value in milimeter
        /// </summary>
        public double ZScaleInMillimeter { get; set; }
    }
}
// ReSharper restore InconsistentNaming