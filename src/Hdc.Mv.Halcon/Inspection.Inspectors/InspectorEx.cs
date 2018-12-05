using System.Collections.Generic;
using System.Threading.Tasks;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    public static class InspectorEx
    {
        public static IList<CircleSearchingResult> SearchCircles(this ICircleInspector inspector, HImage image,
                                                                 IList<CircleSearchingDefinition>
                                                                     definitions)
        {
            var csrs = new CircleSearchingResultCollection();

            int index = 0;
            foreach (var definition in definitions)
            {
                var csr = inspector.SearchCircle(image, definition);
                csr.Index = index;
                csrs.Add(csr);
                index++;
            }
            return csrs;
        }

        public static IList<EdgeSearchingResult> SearchEdges(this IEdgeInspector inspector, HImage image,
                                                             IList<EdgeSearchingDefinition> definitions)
        {
            var csrs = new EdgeSearchingResultCollection();

            int index = 0;
            foreach (var definition in definitions)
            {
                var csr = inspector.SearchEdge(image, definition);
                csr.Index = index;
                csrs.Add(csr);
                index++;
            }
            return csrs;
        }


        public static IList<SurfaceResult> SearchSurfaces(this ISurfaceInspector inspector,
                                                          HImage image, IList<SurfaceDefinition> definitions)
        {
            var csrs = new SurfaceResultCollection();

            int index = 0;
            foreach (var definition in definitions)
            {
                var csr = inspector.SearchSurface(image, definition);
                csr.Index = index;
                csrs.Add(csr);
                index++;
            }
            return csrs;
        }

        public static IList<RegionTargetResult> SearchRegionTargets(this IRegionTargetInspector inspector,
                                                                    HImage image,
                                                                    IList<RegionTargetDefinition> definitions)
        {
            var csrs = new List<RegionTargetResult>();

            int index = 0;
            foreach (var definition in definitions)
            {
                var csr = inspector.SearchRegionTarget(image, definition);
                csr.Index = index;
                csrs.Add(csr);
                index++;
            }
            return csrs;
        }

        public static IList<PartSearchingResult> SearchParts(this IPartInspector inspector, HImage image,
                                                             IList<PartSearchingDefinition> definitions)
        {
            var csrs = new List<PartSearchingResult>();

            int index = 0;
            foreach (var definition in definitions)
            {
                var csr = inspector.SearchPart(image, definition);
                csr.Index = index;
                csrs.Add(csr);
                index++;
            }
            return csrs;
        }

        public static IList<RegionDefectResult> SearchDefects(this IDefectInspector inspector,
                                                              HImage image, IList<DefectDefinition> definitions,
                                                              IList<SurfaceResult> surfaceResults)
        {
            var csrs = new List<RegionDefectResult>();

            var tasks = new List<Task>();

            foreach (var definition in definitions)
            {
                var definition1 = definition;
                var task = Task.Run(() =>
                {
                    var csr = inspector.SearchDefects(image, definition1);
                    csrs.Add(csr);
                });

                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());

            return csrs;
        }

        public static IList<DataCodeSearchingResult> SearchDataCodes(this IDataCodeSearchingInspector inspector, HImage image,
                                                              IList<DataCodeSearchingDefinition>
                                                                  definitions)
        {
            var csrs = new List<DataCodeSearchingResult>();

            int index = 0;
            foreach (var definition in definitions)
            {
                var csr = inspector.SearchDataCode(image, definition);
                csr.Index = index;
                csrs.Add(csr);
                index++;
            }
            return csrs;
        }

        public static IList<XldSearchingResult> SearchXlds(this IXldInspector inspector, HImage image,
                                                              IList<XldSearchingDefinition>
                                                                  definitions)
        {
            var csrs = new List<XldSearchingResult>();

            int index = 0;
            foreach (var definition in definitions)
            {
                var csr = inspector.SearchXld(image, definition);
                csr.Index = index;
                csrs.Add(csr);
                index++;
            }
            return csrs;
        }

        public static IList<RegionSearchingResult> SearchRegions(this IRegionSearchingInspector inspector, HImage image,
                                                              IList<RegionSearchingDefinition> definitions)
        {
            var csrs = new List<RegionSearchingResult>();

            int index = 0;
            foreach (var definition in definitions)
            {
                var csr = inspector.SearchRegion(image, definition);
                csr.Index = index;
                csrs.Add(csr);
                index++;
            }
            return csrs;
        }

        public static IList<IntersectionPointOfTwoShapesResult> CalculateIntersectionPointOfTwoShapesEx(
            this IntersectionPointOfTwoShapesInspector inspector, 
            HImage image,
            IList<IntersectionPointOfTwoShapesDefinition> definitions, 
            InspectionResult inspectionResult)
        {
            var csrs = new List<IntersectionPointOfTwoShapesResult>();

            int index = 0;

            foreach (var definition in definitions)
            {
                var csr = inspector.CalculateIntersectionPointOfTwoShapes(image, definition, inspectionResult);
                csr.Index = index;
                csrs.Add(csr);
                index++;
            }

            return csrs;
        }

        public static IList<StepFromGrayValueResult> Calculate(
            this StepFromGrayValueInspector inspector, 
            HImage image,
            IList<StepFromGrayValueDefinition> definitions, 
            InspectionResult inspectionResult)
        {
            var csrs = new List<StepFromGrayValueResult>();

            int index = 0;

            foreach (var definition in definitions)
            {
                var csr = inspector.Calculate(image, definition, inspectionResult);
                csrs.Add(csr);
                index++;
            }

            return csrs;
        }

        public static IList<PointOfXldAndRadialLineResult> CalculatePointsOfXldAndRadialLine(
            this PointOfXldAndRadialLineInspector inspector, 
            HImage image,
            IList<PointOfXldAndRadialLineDefinition> definitions, 
            InspectionResult inspectionResult)
        {
            var csrs = new List<PointOfXldAndRadialLineResult>();

            int index = 0;
            foreach (var definition in definitions)
            {
                var csr = inspector.CalculatePointOfXldAndRadialLine(image, definition, inspectionResult);
                csr.Index = index;
                csrs.Add(csr);
                index++;
            }
            return csrs;
        }

        public static IList<PointOfEdgeAndRadialLineResult> CalculatePointOfEdgeAndRadialLine(
            this PointOfEdgeAndRadialLineInspector inspector,
            HImage image,
            IList<PointOfEdgeAndRadialLineDefinition> definitions,
            InspectionResult inspectionResult)
        {
            var csrs = new List<PointOfEdgeAndRadialLineResult>();

            int index = 0;
            foreach (var definition in definitions)
            {
                var csr = inspector.CalculatePointOfEdgeAndRadialLine(image, definition, inspectionResult);
                csr.Index = index;
                csrs.Add(csr);
                index++;
            }
            return csrs;
        }
        public static IList<DistanceBetweenPointsOfXldAndRadialLineResult> CalculateDistanceBetweenPointsOfXldAndRadialLineDefinitions(this DistanceBetweenPointsOfXldAndRadialLineInspector inspector,
                                                              IList<DistanceBetweenPointsOfXldAndRadialLineDefinition> definitions, InspectionResult inspectionResult)
        {
            var csrs = new List<DistanceBetweenPointsOfXldAndRadialLineResult>();

            int index = 0;
            foreach (var definition in definitions)
            {
                var csr = inspector.Calculate(definition, inspectionResult);
                csr.Index = index;
                csrs.Add(csr);
                index++;
            }
            return csrs;
        }
    }
}