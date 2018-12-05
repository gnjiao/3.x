using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    public class InspectionResult : IDisposable
    {
        public int Index { get; set; }
        public string Comment { get; set; }

        public InspectionResult()
        {
        }

        // Coordinate
        public CircleSearchingResultCollection CoordinateCircles { get; set; } = new CircleSearchingResultCollection();
        public EdgeSearchingResultCollection CoordinateEdges { get; set; } = new EdgeSearchingResultCollection();
        public IList<RegionSearchingResult> CoordinateRegions { get; set; } = new List<RegionSearchingResult>();

        // Datum Plane
        public IList<RegionSearchingResult> DatumPlaneRegionSearchingResults { get; set; } = new List<RegionSearchingResult>();

        // Inspection
        public CircleSearchingResultCollection Circles { get; set; } = new CircleSearchingResultCollection();
        public EdgeSearchingResultCollection Edges { get; set; } = new EdgeSearchingResultCollection();

        public DistanceBetweenLinesResultCollection DistanceBetweenLinesResults { get; set; } =
            new DistanceBetweenLinesResultCollection();

        public DistanceBetweenPointsResultCollection DistanceBetweenPointsResults { get; set; } =
            new DistanceBetweenPointsResultCollection();

        public IList<RegionDefectResult> RegionDefectResults { get; set; } = new List<RegionDefectResult>();
        public ClosedRegionResultCollection ClosedRegionResults { get; set; } = new ClosedRegionResultCollection();
        public IList<PartSearchingResult> Parts { get; set; } = new List<PartSearchingResult>();
        public IList<RegionTargetResult> RegionTargets { get; set; } = new List<RegionTargetResult>();
        public IList<RegionSearchingResult> RegionSearchingResults { get; set; } = new List<RegionSearchingResult>();
        public HRegion UnionDefectRegion { get; set; }
        public IList<DefectResult> ConnectedDefectResults { get; set; } = new List<DefectResult>();
        public IList<DataCodeSearchingResult> DataCodeSearchingResults { get; set; } = new List<DataCodeSearchingResult>();
        public IList<XldSearchingResult> XldSearchingResults { get; set; } = new List<XldSearchingResult>();
        public IList<PointDefinition> ReferencePointDefinitions { get; set; } = new List<PointDefinition>();
        public IList<LineDefinition> ReferenceLineDefinitions { get; set; } = new List<LineDefinition>();
        public IList<RadialLineDefinition> ReferenceRadialLineDefinitions { get; set; } = new List<RadialLineDefinition>();
        public IList<PointOfXldAndRadialLineResult> PointOfXldAndRadialLineResults { get; set; } = new List<PointOfXldAndRadialLineResult>();
        public IList<PointOfEdgeAndRadialLineResult> PointOfEdgeAndRadialLineResults { get; set; } = new List<PointOfEdgeAndRadialLineResult>();
        public IList<DistanceBetweenPointsOfXldAndRadialLineResult> DistanceBetweenPointsOfXldAndRadialLineResults { get; set; } = new List<DistanceBetweenPointsOfXldAndRadialLineResult>();
        public IList<IntersectionPointOfTwoShapesResult> IntersectionPointOfTwoShapesResults { get; set; } = new List<IntersectionPointOfTwoShapesResult>();
        public IList<StepFromGrayValueResult> StepFromGrayValueResults { get; set; } = new List<StepFromGrayValueResult>();
        public IList<DistanceBetweenTwoPointsResult> DistanceBetweenTwoPointsResults { get; set; } = new List<DistanceBetweenTwoPointsResult>();

        public IRelativeCoordinate Coordinate { get; set; }
        public IDictionary<string, IRelativeCoordinate> Coordinates { get; set; } = new Dictionary<string, IRelativeCoordinate>();
        public InspectionSchema InspectionSchema { get; set; }

        public bool IsNG
        {
            get
            {
                var defectsCount = RegionDefectResults.SelectMany(x => x.DefectResults).Count();
                return defectsCount > 0;
            }
        }

        public void Dispose()
        {
            foreach (var regionDefectResult in RegionDefectResults)
            {
                regionDefectResult.Dispose();
            }

            foreach (var defectResult in ConnectedDefectResults)
            {
                defectResult.Dispose();
            }

            UnionDefectRegion?.Dispose();
        }
    }
}