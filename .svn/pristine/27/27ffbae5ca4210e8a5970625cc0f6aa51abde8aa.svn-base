using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows;
using HalconDotNet;
using Core.Collections.Generic;
using Core.Diagnostics;
using Core.Linq;
using Hdc.Mv.Halcon;
using Core.Reflection;
using Core.Serialization;
using Core;

namespace Hdc.Mv.Inspection
{
    public interface IInspectionController :
        IDisposable
    {
        InspectionResult InspectionResult { get; }

        IRelativeCoordinate Coordinate { get; }

        HImage Image { get; }

        InspectionSchema InspectionSchema { get; }
        void SetInspectionSchema(InspectionSchema inspectionSchema);
        void CreateCoordinate();
        void Inspect();
        void SetImage(HImage image);
    }

    public class InspectionController : IInspectionController
    {
        private IRelativeCoordinate _coordinate;
        private IDictionary<string, IRelativeCoordinate> _coordinates = new Dictionary<string, IRelativeCoordinate>();
        private HImage _image;
        private InspectionSchema _inspectionSchema;
        private InspectionResult _inspectionResult;

        public void SetImage(HImage image)
        {
            var dir = typeof(Ex).Assembly.GetAssemblyDirectoryPath();
            var cacheDir = Path.Combine(dir, "CacheImages");
            if (Directory.Exists(cacheDir))
            {
                foreach (var file in Directory.GetFiles(cacheDir))
                {
                    File.Delete(file);
                }
            }

            if (!Directory.Exists(cacheDir))
            {
                Directory.CreateDirectory(cacheDir);
            }

            _inspectionResult = new InspectionResult();

            _image = image;
        }

        public static InspectionSchema GetInspectionSchema()
        {
            var dir = typeof(InspectionController).Assembly.GetAssemblyDirectoryPath();

            return dir.GetInspectionSchemaFromDir();
        }

        public void SetInspectionSchema(string fileName = null)
        {
            var inspectionSchema = string.IsNullOrEmpty(fileName)
                ? GetInspectionSchema()
                : fileName.DeserializeFromXamlFile<InspectionSchema>();

            SetInspectionSchema(inspectionSchema);
        }

        public void SetInspectionSchema(InspectionSchema inspectionSchema)
        {
            _inspectionSchema = inspectionSchema;
        }

        public void CreateCoordinate()
        {
            var sw = new NotifyStopwatch("InspectionController.CreateCoordinate.Inspect()");

            UpdatePixelCellSideLengthInMillimeter();

            try
            {
                switch (_inspectionSchema.CoordinateType)
                {
                    case CoordinateType.None:
                        _coordinate = new MockRelativeCoordinate();
                        break;
                    case CoordinateType.Baseline:
                        var origin = _inspectionResult.CoordinateCircles[0];
                        var refCircle = _inspectionResult.CoordinateCircles[1];
                        _coordinate = new RelativeCoordinate(
                            origin.Circle.GetCenterPoint(),
                            refCircle.Circle.GetCenterPoint(),
                            refCircle.Definition.BaselineAngle);
                        break;
                    case CoordinateType.VectorsCenter:
                        var inspector =
                            InspectorFactory.CreateCircleInspector(_inspectionSchema.CircleSearching_InspectorName);
                        var searchCoordinateCircles = inspector.SearchCircles(_image,
                            _inspectionSchema.CoordinateCircles);
                        _inspectionResult.CoordinateCircles =
                            new CircleSearchingResultCollection(searchCoordinateCircles);
                        _coordinate = RelativeCoordinateFactory.CreateCoordinate(_inspectionResult.CoordinateCircles);
                        break;
                    case CoordinateType.NearOrigin:
                        throw new NotSupportedException("CoordinateType does not implement!");
                    //                        break;
                    case CoordinateType.Border:
                        var inspector2 =
                            InspectorFactory.CreateEdgeInspector(_inspectionSchema.EdgeSearching_InspectorName);
                        var searchCoordinateEdges = inspector2.SearchEdges(_image, _inspectionSchema.CoordinateEdges);
                        _inspectionResult.CoordinateEdges = new EdgeSearchingResultCollection(searchCoordinateEdges);

                        if (_inspectionResult.CoordinateEdges.Count >= 4)
                        {
                            _coordinate = RelativeCoordinateFactory.CreateCoordinateUsingBorder(
                            _inspectionResult.CoordinateEdges);
                        }
                        else if (_inspectionResult.CoordinateEdges.Count == 1)
                        {
                            _coordinate = RelativeCoordinateFactory.CreateCoordinateUsingSingleLine(
                            _inspectionResult.CoordinateEdges.First());
                        }
                        break;
                    case CoordinateType.RegionCenter:
                        var inspector3 = InspectorFactory.CreateRegionSearchingInspector();
                        _inspectionResult.CoordinateRegions = inspector3.SearchRegions(_image,
                            _inspectionSchema.CoordinateRegions);

                        if (_inspectionResult.CoordinateRegions.Count == 1)
                        {
                            var region = _inspectionResult.CoordinateRegions[0].Region;
                            var rect2 = region.GetSmallestHRectangle2();

                            Point originPoint = new Point();
                            var line = rect2.GetRoiLineFromRectangle2Phi();

                            switch (_inspectionSchema.CoordinateType_RegionCenter_Direction)
                            {
                                case Direction.Center:
                                    originPoint = new Point(rect2.Column, rect2.Row);
                                    break;
                                case Direction.Left:
                                    originPoint = line.X1 < line.X2 ? line.GetPoint1() : line.GetPoint2();
                                    break;
                                case Direction.Right:
                                    originPoint = line.X1 > line.X2 ? line.GetPoint1() : line.GetPoint2();
                                    break;
                                case Direction.Top:
                                    throw new NotImplementedException();
                                    //                                    originPoint = line.Y1 > line.Y2 ? line.GetPoint1() : line.GetPoint2();
/*
                                    break;
*/
                                case Direction.Bottom:
                                    throw new NotImplementedException();
                                    //                                    originPoint = line.Y1 < line.Y2 ? line.GetPoint1() : line.GetPoint2();
/*
                                    break;
*/
                            }

                            _coordinate = RelativeCoordinateFactory.CreateCoordinateUsingPointAndAngle(
                                    //                                    rect2.Column, rect2.Row, rect2.Angle);
                                    originPoint.X, originPoint.Y, -rect2.Angle);
                        }
                        break;
                    default:
                        throw new NotSupportedException("CoordinateType does not support!");
                }
            }
            catch (CreateCoordinateFailedException)
            {
                //                _inspectionResult
                throw;
            }

            if (_inspectionSchema.CoordinateOriginOffsetEnable)
            {
                var xInPixel =
                _inspectionSchema.CoordinateOriginOffsetX.GetPixelValue(
                    _inspectionSchema.CoordinateOriginOffsetUnitType,
                    _inspectionSchema.PixelCellSideLengthInMillimeter);

                var yInPixel =
                _inspectionSchema.CoordinateOriginOffsetY.GetPixelValue(
                    _inspectionSchema.CoordinateOriginOffsetUnitType,
                    _inspectionSchema.PixelCellSideLengthInMillimeter);

                _coordinate.ChangeOriginOffsetUsingRelative(xInPixel, yInPixel);
                //                _coordinate.OriginOffset = new Vector(_inspectionSchema.CoordinateOriginOffsetX,
                //                    _inspectionSchema.CoordinateOriginOffsetY);
            }

            _coordinates.Add("Default", _coordinate);

            // primaryCoordinateExtactors
            var primaryCoordinateExtactors =
                InspectionSchema.CoordinateExtactors.Where(x => string.IsNullOrEmpty(x.CoordinateName));

            foreach (var coordinateExtactor in primaryCoordinateExtactors)
            {
                var coord = coordinateExtactor.Extact(_image, _coordinate,
                    _inspectionSchema.PixelCellSideLengthInMillimeter);
                _coordinates.Add(coordinateExtactor.Name, coord);
            }

            // secondaryCoordinateExtactors
            var secondaryCoordinateExtactors =
                InspectionSchema.CoordinateExtactors.Where(x => !string.IsNullOrEmpty(x.CoordinateName));

            foreach (var coordinateExtactor in secondaryCoordinateExtactors)
            {
                var refCoord = _coordinates[coordinateExtactor.CoordinateName];

                var coord = coordinateExtactor.Extact(_image, refCoord,
                    _inspectionSchema.PixelCellSideLengthInMillimeter);
                _coordinates.Add(coordinateExtactor.Name, coord);
            }

            //
            UpdateRelativeCoordinates();

            sw.Dispose();
        }

        public void Inspect()
        {
            var sw2 = new NotifyStopwatch("IInspectionController.Inspect()");

            var inspectionSchema = _inspectionSchema.DeepClone();

            if (inspectionSchema.DatumPlaneCreator != null)
            {
                var sw = new NotifyStopwatch($"{nameof(inspectionSchema.DatumPlaneCreator)}() Total");

                var datumPlaneResult = inspectionSchema.DatumPlaneCreator.Create(_image);

                _image.Dispose();
                _image = datumPlaneResult.DatumPlaneImage;
                _inspectionResult.DatumPlaneRegionSearchingResults = datumPlaneResult.RegionSearchingResults;

                sw.Dispose();
            }

            var tasks = new List<Task>();

#pragma warning disable 618
            if (inspectionSchema.RegionTargetDefinitions.Any())
#pragma warning restore 618
            {
                tasks.Add(Task.Run(() => SearchRegionTargets(inspectionSchema.DeepClone())));
            }

            if (inspectionSchema.RegionSearchingDefinitions.Any())
            {
                tasks.Add(Task.Run(() => SearchRegionSearchingDefinitions(inspectionSchema.DeepClone())));
            }

            if (inspectionSchema.PartSearchingDefinitions.Any())
            {
                tasks.Add(Task.Run(() => SearchParts(inspectionSchema.DeepClone())));
            }

            if (inspectionSchema.CircleSearchingDefinitions.Any())
            {
                tasks.Add(Task.Run(() => SearchCircles(inspectionSchema.DeepClone())));
            }

            if (inspectionSchema.DefectDefinitions.Any())
            {
                tasks.Add(Task.Run(() => SearchDefects(inspectionSchema.DeepClone())));
            }

            if (inspectionSchema.SurfaceDefinitions.Any())
            {
                tasks.Add(Task.Run(() => SearchDefects(inspectionSchema.DeepClone())));
            }

            if (inspectionSchema.EdgeSearchingDefinitions.Any())
            {
                tasks.Add(Task.Run(() => SearchEdges(inspectionSchema.DeepClone())));
            }

            if (inspectionSchema.DataCodeSearchingDefinitions.Any())
            {
                tasks.Add(Task.Run(() => SearchDataCodes(inspectionSchema.DeepClone())));
            }

            if (inspectionSchema.XldSearchingDefinitions.Any())
            {
                tasks.Add(Task.Run(() => SearchXlds(inspectionSchema.DeepClone())));
            }

            if (inspectionSchema.ReferencePointDefinitions.Any())
            {
                tasks.Add(Task.Run(() => _inspectionResult.ReferencePointDefinitions
                = inspectionSchema.ReferencePointDefinitions.ToList()));
            }

            if (inspectionSchema.ReferenceLineDefinitions.Any())
            {
                tasks.Add(Task.Run(() => _inspectionResult.ReferenceLineDefinitions
                = inspectionSchema.ReferenceLineDefinitions.ToList()));
            }

            if (inspectionSchema.ReferenceRadialLineDefinitions.Any())
            {
                tasks.Add(Task.Run(() => _inspectionResult.ReferenceRadialLineDefinitions
                = inspectionSchema.ReferenceRadialLineDefinitions.ToList()));
            }

            Task.WaitAll(tasks.ToArray());

            if (inspectionSchema.ReferenceLineDefinitions.Any())
            {
                CalculateReferenceLineDefinitionsFromReferences(inspectionSchema.DeepClone());
            }

            if (inspectionSchema.ReferenceRadialLineDefinitions.Any())
            {
                CalculateReferenceRadialLineDefinitionsFromReferences(inspectionSchema.DeepClone());
            }

            if (inspectionSchema.PointOfXldAndRadialLineDefinitions.Any())
            {
                CalculatePointOfXldAndRadialLine(inspectionSchema.DeepClone());
            }

            if (inspectionSchema.PointOfEdgeAndRadialLineDefinitions.Any())
            {
                CalculatePointOfEdgeAndRadialLine(inspectionSchema.DeepClone());
            }
            if (inspectionSchema.IntersectionPointOfTwoShapesDefinitions.Any())
            {
                CalculateIntersectionPointOfTwoShapes(inspectionSchema.DeepClone());
            }

            if (inspectionSchema.DistanceBetweenPointsOfXldAndRadialLineDefinitions.Any())
            {
                CalculateDistanceBetweenPointsOfXldAndRadialLineDefinitions(inspectionSchema.DeepClone());
            }

            if (inspectionSchema.StepFromGrayValueDefinitions.Any())
            {
                CalculateStepFromGrayValueDefinitionsDefinitions(inspectionSchema.DeepClone());
            }

            if (inspectionSchema.DistanceBetweenTwoPointsDefinitions.Any())
            {
                CalculateDistanceBetweenTwoPointsDefinitions(inspectionSchema.DeepClone());
            }

            sw2.Dispose();
        }

        private void CalculateDistanceBetweenTwoPointsDefinitions(InspectionSchema inspectionSchema)
        {
            var sw = new NotifyStopwatch($"{nameof(CalculateDistanceBetweenTwoPointsDefinitions)}() Total");

            var inspector = new DistanceBetweenTwoPointsInspector();

            var DistanceBetweenTwoPointsResults = new List<DistanceBetweenTwoPointsResult>();

            foreach (var def in inspectionSchema.DistanceBetweenTwoPointsDefinitions)
            {
                var result = inspector.Calculate(def, _inspectionResult, _coordinates);
                DistanceBetweenTwoPointsResults.Add(result);
            }

            _inspectionResult.DistanceBetweenTwoPointsResults = DistanceBetweenTwoPointsResults;

            sw.Dispose();
        }

        private void CalculateStepFromGrayValueDefinitionsDefinitions(InspectionSchema inspectionSchema)
        {
            var sw = new NotifyStopwatch($"{nameof(CalculateStepFromGrayValueDefinitionsDefinitions)}() Total");

            var inspector = new StepFromGrayValueInspector();
            var hImage = _image.CopyImage();
            _inspectionResult.StepFromGrayValueResults = inspector.Calculate(
                hImage, inspectionSchema.StepFromGrayValueDefinitions, _inspectionResult);

            hImage.Dispose();
            sw.Dispose();
        }

        private void CalculateIntersectionPointOfTwoShapes(InspectionSchema inspectionSchema)
        {
            var sw = new NotifyStopwatch($"{nameof(CalculateIntersectionPointOfTwoShapes)}() Total");

            var inspector = new IntersectionPointOfTwoShapesInspector();
            var hImage = _image.CopyImage();
            _inspectionResult.IntersectionPointOfTwoShapesResults = inspector.CalculateIntersectionPointOfTwoShapesEx(
                hImage, inspectionSchema.IntersectionPointOfTwoShapesDefinitions, _inspectionResult);

            hImage.Dispose();
            sw.Dispose();
        }

        private void CalculateReferenceLineDefinitionsFromReferences(InspectionSchema inspectionSchema)
        {
            var sw = new NotifyStopwatch($"{nameof(CalculateReferenceLineDefinitionsFromReferences)}() Total");

            for (int i = 0; i < inspectionSchema.ReferenceLineDefinitions.Count; i++)
            {
                var def = inspectionSchema.ReferenceLineDefinitions[i];
                if (def.ReferenceRelativePoint1Name.IsNullOrEmpty() ||
                    def.ReferenceRelativePoint2Name.IsNullOrEmpty())
                    continue;

                var pointDef1 = _inspectionResult.ReferencePointDefinitions
                    .SingleOrDefault(x => x.Name == def.ReferenceRelativePoint1Name);

                var pointDef2 = _inspectionResult.ReferencePointDefinitions
                    .SingleOrDefault(x => x.Name == def.ReferenceRelativePoint2Name);

                if (pointDef1 == null || pointDef2 == null)
                {
                    continue;
                }

                var finalDef = _inspectionResult.ReferenceLineDefinitions[i];

                finalDef.ActualLine = new Line(pointDef1.ActualPoint, pointDef2.ActualPoint);
                finalDef.RelativeLine = new Line(pointDef1.RelativePoint, pointDef2.RelativePoint);
            }

            sw.Dispose();
        }

        private void CalculateReferenceRadialLineDefinitionsFromReferences(InspectionSchema inspectionSchema)
        {
            var sw = new NotifyStopwatch($"{nameof(CalculateReferenceLineDefinitionsFromReferences)}() Total");

            for (int i = 0; i < inspectionSchema.ReferenceRadialLineDefinitions.Count; i++)
            {
                var def = inspectionSchema.ReferenceRadialLineDefinitions[i];
                if (def.ReferenceOriginName.IsNullOrEmpty() ||
                    def.ReferenceFarPointName.IsNullOrEmpty())
                    continue;

                var oriPointDef = _inspectionResult.ReferencePointDefinitions
                    .SingleOrDefault(x => x.Name == def.ReferenceOriginName);

                var farPointDef = _inspectionResult.ReferencePointDefinitions
                    .SingleOrDefault(x => x.Name == def.ReferenceFarPointName);

                if (oriPointDef == null || farPointDef == null)
                {
                    continue;
                }

                var finalDef = _inspectionResult.ReferenceRadialLineDefinitions[i];

                finalDef.RelativeOriginX = oriPointDef.RelativeX;
                finalDef.RelativeOriginY = oriPointDef.RelativeY;
                finalDef.ActualOriginX = oriPointDef.ActualX;
                finalDef.ActualOriginY = oriPointDef.ActualY;

                var lineVector = farPointDef.ActualPoint - oriPointDef.ActualPoint;
                var angle = lineVector.GetAngleToX();

                finalDef.Angle = -angle - def.ReferenceAngleOffset;
                finalDef.ActualRadius = def.ReferenceActualRadius > 0.00001
                    ? def.ReferenceActualRadius : lineVector.Length;
            }

            sw.Dispose();
        }

        private void CalculatePointOfXldAndRadialLine(InspectionSchema inspectionSchema)
        {
            var sw = new NotifyStopwatch($"{nameof(CalculatePointOfXldAndRadialLine)}() Total");

            var inspector = new PointOfXldAndRadialLineInspector();
            var hImage = _image.CopyImage();
            _inspectionResult.PointOfXldAndRadialLineResults = inspector.CalculatePointsOfXldAndRadialLine(
                hImage, inspectionSchema.PointOfXldAndRadialLineDefinitions, _inspectionResult);

            hImage.Dispose();
            sw.Dispose();
        }
       
        private void CalculatePointOfEdgeAndRadialLine(InspectionSchema inspectionSchema)
         {
             var sw = new NotifyStopwatch($"{nameof(CalculatePointOfEdgeAndRadialLine)}() Total");

             var inspector = new PointOfEdgeAndRadialLineInspector();
             var hImage = _image.CopyImage();
             _inspectionResult.PointOfEdgeAndRadialLineResults = inspector.CalculatePointOfEdgeAndRadialLine(
                 hImage, inspectionSchema.PointOfEdgeAndRadialLineDefinitions, _inspectionResult);

             hImage.Dispose();
             sw.Dispose();
         }
         
        private void CalculateDistanceBetweenPointsOfXldAndRadialLineDefinitions(InspectionSchema inspectionSchema)
        {
            var sw = new NotifyStopwatch($"{nameof(CalculatePointOfXldAndRadialLine)}() Total");

            var inspector = new DistanceBetweenPointsOfXldAndRadialLineInspector();
            _inspectionResult.DistanceBetweenPointsOfXldAndRadialLineResults = inspector.CalculateDistanceBetweenPointsOfXldAndRadialLineDefinitions(
                inspectionSchema.DistanceBetweenPointsOfXldAndRadialLineDefinitions, _inspectionResult);

            sw.Dispose();
        }

        private void SearchXlds(InspectionSchema inspectionSchema)
        {
            var sw = new NotifyStopwatch($"{nameof(SearchXlds)}() Total");

            var inspector = new XldInspector();
            var hImage = _image.CopyImage();
            var results = inspector.SearchXlds(hImage, inspectionSchema.XldSearchingDefinitions);
            _inspectionResult.XldSearchingResults = results;

            hImage.Dispose();
            sw.Dispose();
        }

        private void SearchDataCodes(InspectionSchema inspectionSchema)
        {
            var hImage = _image.CopyImage();
            var sw = new NotifyStopwatch($"{nameof(SearchDataCodes)}() Total");

            var inspector = new DataCodeSearchingInspector();
            var results = inspector.SearchDataCodes(hImage, inspectionSchema.DataCodeSearchingDefinitions);

            _inspectionResult.DataCodeSearchingResults = results;
            sw.Dispose();
            hImage.Dispose();
        }

        private void SearchEdges(InspectionSchema inspectionSchema)
        {
            var hImage = _image.CopyImage();
            var sw = new NotifyStopwatch($"{nameof(SearchEdges)}() Total");
            if (inspectionSchema.EdgeSearching_SaveCacheImage_Disabled)
            {
                foreach (var def in inspectionSchema.EdgeSearchingDefinitions)
                {
                    def.Domain_SaveCacheImageEnabled = false;
                    def.RegionExtractor_Disabled = false;
                    def.ImageFilter_SaveCacheImageEnabled = false;
                }
            }

            var finalEdges = InspectorFactory
                .CreateEdgeInspector(inspectionSchema.EdgeSearching_InspectorName)
                .SearchEdges(hImage, inspectionSchema.EdgeSearchingDefinitions);
            _inspectionResult.Edges = new EdgeSearchingResultCollection(finalEdges);


            int i = 0;
            foreach (DistanceBetweenLinesDefinition def in inspectionSchema.DistanceBetweenIntersectionPointsDefinitions)
            {
                var line1 = finalEdges.Single(x => x.Name == def.Line1Name);
                var line2 = finalEdges.Single(x => x.Name == def.Line2Name);

                var line1Center = line1.Definition.Line.GetCenterPoint();
                var line2Center = line2.Definition.Line.GetCenterPoint();

                var linkLine = new Line(line1Center, line2Center);

                DistanceBetweenPointsResult distanceBetweenPointsResult;

                if (line1.EdgeLine == null || line2.EdgeLine == null)
                {
                    continue;
                }

                if (line1.EdgeLine.IsPoint || line2.EdgeLine.IsPoint)
                {
                    Debug.WriteLine(@"DistanceBetweenIntersectionPointsDefinitions IsNotFound: {0}", def.Name);

                    Point intersection1 = line1Center;
                    Point intersection2 = line2Center;

                    distanceBetweenPointsResult = new DistanceBetweenPointsResult()
                    {
                        Definition = def.DeepClone(),
                        Index = i,
                        Name = def.Name,
                        HasError = true,
                        IsNotFound = true,
                        Point1 = intersection1,
                        Point2 = intersection2,
                        DistanceInPixel = 999.999,
                        DistanceInWorld = 999.999,
                    };
                }
                else
                {
                    Point intersection1 = line1.EdgeLine.IntersectionWith(linkLine);
                    Point intersection2 = line2.EdgeLine.IntersectionWith(linkLine);

                    if (Math.Abs(intersection1.X) < 0.000001 ||
                        Math.Abs(intersection2.X) < 0.000001)
                    {
                        Debug.WriteLine(@"DistanceBetweenIntersectionPointsDefinitions failed: {0}", def.Name);
                    }

                    distanceBetweenPointsResult = new DistanceBetweenPointsResult()
                    {
                        Definition = def.DeepClone(),
                        Index = i,
                        Name = def.Name,
                        Point1 = intersection1,
                        Point2 = intersection2,
                        DistanceInPixel = (intersection1 - intersection2).Length,
                        DistanceInWorld =
                            (intersection1 - intersection2).Length
                                .ToMillimeterFromPixel(def.PixelCellSideLengthInMillimeter),
                    };
                }

                _inspectionResult.DistanceBetweenPointsResults.Add(distanceBetweenPointsResult);

                i++;
            }

            sw.Dispose();
            hImage.Dispose();
        }

        private void SearchDefects(InspectionSchema inspectionSchema)
        {
            var hImage = _image.CopyImage();
            IList<SurfaceResult> regionResults = null;

            using (new NotifyStopwatch("SearchSurfaces()"))
                regionResults = InspectorFactory
                    .CreateSurfaceInspector(inspectionSchema.Surfaces_InspectorName)
                    .SearchSurfaces(_image, inspectionSchema.SurfaceDefinitions);

            //                IList<DefectResult> defectResultCollection = null;
            if (inspectionSchema.DefectDefinitions.Any())
            {
                var sw = new NotifyStopwatch("SearchDefects() Total");
                var defectInspector = InspectorFactory.CreateDefectInspector(inspectionSchema.Defects_InspectorName);
                var defectResultCollection = defectInspector.SearchDefects(hImage,
                    inspectionSchema.DefectDefinitions,
                    regionResults);
                _inspectionResult.RegionDefectResults = defectResultCollection;

                // UnionDefectRegion
                var regions = _inspectionResult.RegionDefectResults
                    .Where(x => x.DefectRegion != null)
                    .Select(x => x.DefectRegion)
                    .ToList();

                var union = regions.Union();
                _inspectionResult.UnionDefectRegion = union;

                //
                var defects = _inspectionResult.GetDefectResults();
                var blobs = defects.Where(x => x.Region != null).Select(x => x.Region);
                var unionBlobs = blobs.Union();

                var connectionBlobs = unionBlobs.Connection().ToList();
                var connectionBlobs2 = connectionBlobs.Where(x => x.Area > 0).ToList();

                //var emptyBlobs = connectionBlobs.Where(x => x.Area == 0).ToList();
                //if (emptyBlobs.Count > 0)
                //    throw new Exception("connectionBlobs have empty blob region");

                var defectResults = connectionBlobs2.Select(x => x.ToDefectResult());
                _inspectionResult.ConnectedDefectResults.AddRange(defectResults);


                sw.Dispose();
                hImage.Dispose();
                //                inspectionResult.ClosedRegionResults = regionResults;
            }

            foreach (var surfaceResult in regionResults)
            {
                surfaceResult.TransformedImage?.Dispose();
            }
        }

        private void SearchCircles(InspectionSchema inspectionSchema)
        {
            var hImage = _image.CopyImage();
            var sw = new NotifyStopwatch($"{nameof(SearchCircles)}() Total");
            var circles = InspectorFactory
                .CreateCircleInspector(inspectionSchema.CircleSearching_InspectorName)
                .SearchCircles(hImage, inspectionSchema.CircleSearchingDefinitions);
            _inspectionResult.Circles = new CircleSearchingResultCollection(circles);
            _inspectionResult.Circles.UpdateRelativeCoordinate(_coordinate);
            sw.Dispose();
            hImage.Dispose();
        }

        private void SearchParts(InspectionSchema inspectionSchema)
        {
            var sw = new NotifyStopwatch($"{nameof(SearchParts)}() Total");
            var partInspector = new PartInspector();
            var hImage = _image.CopyImage();
            var results = partInspector.SearchParts(hImage, inspectionSchema.PartSearchingDefinitions);
            _inspectionResult.Parts = results;
            sw.Dispose();
            hImage.Dispose();
        }

        private void SearchRegionTargets(InspectionSchema inspectionSchema)
        {
            var sw = new NotifyStopwatch($"{nameof(SearchRegionTargets)}() Total");
            var inspector = new RegionTargetInspector();
            var hImage = _image.CopyImage();
#pragma warning disable 618
            var results = inspector.SearchRegionTargets(hImage, inspectionSchema.RegionTargetDefinitions);
#pragma warning restore 618
            _inspectionResult.RegionTargets = results;
            sw.Dispose();
            hImage.Dispose();
        }

        private void SearchRegionSearchingDefinitions(InspectionSchema inspectionSchema)
        {
            var sw = new NotifyStopwatch($"{nameof(SearchRegionTargets)}() Total");
            var inspector = new RegionSearchingInspector();
            var hImage = _image.CopyImage();
            var results = inspector.SearchRegions(hImage, inspectionSchema.RegionSearchingDefinitions);
            _inspectionResult.RegionSearchingResults = results;
            sw.Dispose();
            hImage.Dispose();
        }

        public IDictionary<string, IRelativeCoordinate> Coordinates
        {
            get { return _coordinates; }
        }

        public IRelativeCoordinate Coordinate
        {
            get { return _coordinate; }
        }

        public HImage Image
        {
            get { return _image; }
        }

        public InspectionSchema InspectionSchema
        {
            get { return _inspectionSchema; }
        }

        public InspectionResult InspectionResult
        {
            get { return _inspectionResult; }
        }

        public void Dispose()
        {
            _image?.Dispose();
        }

        private void UpdateRelativeCoordinates()
        {
            // coordinate
            _inspectionSchema.CoordinateCircles.UpdateRelativeCoordinate(_coordinate);
            _inspectionSchema.CoordinateEdges.UpdateRelativeCoordinate(_coordinate);

            // DatumPlane
            _inspectionSchema.DatumPlaneCreator?.UpdateRelativeCoordinate(_coordinate);

            // only support default coordinate
            _inspectionSchema.CircleSearchingDefinitions.UpdateRelativeCoordinate(_coordinate);
            _inspectionSchema.PartSearchingDefinitions.UpdateRelativeCoordinate(_coordinate);
            _inspectionSchema.SurfaceDefinitions.UpdateRelativeCoordinate(_coordinate);
#pragma warning disable 618
            _inspectionSchema.RegionTargetDefinitions.UpdateRelativeCoordinate(_coordinate);
#pragma warning restore 618

            // support mutli-coordinates
            _inspectionSchema.RegionSearchingDefinitions.UpdateRelativeCoordinate(_coordinates);
            _inspectionSchema.EdgeSearchingDefinitions.UpdateRelativeCoordinate(_coordinates);
            _inspectionSchema.XldSearchingDefinitions.UpdateRelativeCoordinate(_coordinates);
            _inspectionSchema.ReferencePointDefinitions.UpdateRelativeCoordinate(_coordinates);
            _inspectionSchema.ReferenceLineDefinitions.UpdateRelativeCoordinate(_coordinates);
            _inspectionSchema.ReferenceRadialLineDefinitions.UpdateRelativeCoordinate(_coordinates);
            _inspectionSchema.DataCodeSearchingDefinitions.UpdateRelativeCoordinate(_coordinates);
        }

        private void UpdatePixelCellSideLengthInMillimeter()
        {
            foreach (var def in _inspectionSchema.CoordinateCircles)
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
            }

            foreach (var def in _inspectionSchema.CoordinateEdges)
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
            }

            foreach (var def in _inspectionSchema.CircleSearchingDefinitions)
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
            }

            foreach (var def in _inspectionSchema.EdgeSearchingDefinitions)
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
            }

            foreach (var def in _inspectionSchema.PartSearchingDefinitions)
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
            }

            foreach (var def in _inspectionSchema.DistanceBetweenLinesDefinitions)
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
            }

            foreach (var def in _inspectionSchema.DistanceBetweenIntersectionPointsDefinitions)
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
            }

            foreach (var def in _inspectionSchema.SurfaceDefinitions)
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
            }

            foreach (var def in _inspectionSchema.DefectDefinitions)
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
            }

#pragma warning disable 618
            foreach (var def in _inspectionSchema.RegionTargetDefinitions)
#pragma warning restore 618
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
            }

            foreach (var def in _inspectionSchema.RegionSearchingDefinitions)
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
                def.ZScaleInMillimeter = _inspectionSchema.ZScaleInMillimeter;
            }

            foreach (var def in _inspectionSchema.DataCodeSearchingDefinitions)
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
            }

            foreach (var def in _inspectionSchema.XldSearchingDefinitions)
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
            }

            foreach (var def in _inspectionSchema.ReferencePointDefinitions)
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
            }

            foreach (var def in _inspectionSchema.ReferenceLineDefinitions)
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
            }

            foreach (var def in _inspectionSchema.ReferenceRadialLineDefinitions)
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
            }

            foreach (var def in _inspectionSchema.CoordinateRegions)
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
            }

            foreach (var def in _inspectionSchema.PointOfXldAndRadialLineDefinitions)
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
            }

            foreach (var def in _inspectionSchema.DistanceBetweenPointsOfXldAndRadialLineDefinitions)
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
            }

            foreach (var def in _inspectionSchema.DistanceBetweenTwoPointsDefinitions)
            {
                def.PixelCellSideLengthInMillimeter = _inspectionSchema.PixelCellSideLengthInMillimeter;
            }
        }
    }
}