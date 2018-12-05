﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Core;

namespace Hdc.Mv.Inspection
{
    public static class Ex
    {
        public static Line GetLine(this EdgeSearchingDefinition edgeSearchingDefinition)
        {
            return new Line(edgeSearchingDefinition.StartX,
                edgeSearchingDefinition.StartY,
                edgeSearchingDefinition.EndX,
                edgeSearchingDefinition.EndY);
        }

        public static EdgeSearchingDefinition GetEdgeSearchingDefinition(this Line line, double roiWidth = 0)
        {
            return new EdgeSearchingDefinition(line, roiWidth);
        }

        public static Vector GetBaselineVector(this CircleSearchingDefinition circleSearchingDefinition)
        {
            return new Vector(circleSearchingDefinition.BaselineX, circleSearchingDefinition.BaselineY);
        }

        public static Vector GetBaselineVectorInPixel(this CircleSearchingDefinition circleSearchingDefinition)
        {
            var v = circleSearchingDefinition.GetBaselineVector();
            return new Vector(v.X * 1000 / 16.0, v.Y * 1000 / 16.0);
        }

        public static Point UpdateRelativeCoordinate(this Point relativePoint,
            IRelativeCoordinate relativeCoordinate)
        {
            var actualPoint = relativeCoordinate.GetOriginalPoint(relativePoint);

            return actualPoint;
        }

        public static Line UpdateRelativeCoordinate(this Line relativeLine,
            IRelativeCoordinate relativeCoordinate)
        {
            var p1 = relativeLine.GetPoint1();
            var p2 = relativeLine.GetPoint2();

            var actualP1 = relativeCoordinate.GetOriginalPoint(p1);
            var actualP2 = relativeCoordinate.GetOriginalPoint(p2);

            var actualLine = new Line { X1 = actualP1.X, Y1 = actualP1.Y, X2 = actualP2.X, Y2 = actualP2.Y };

            return actualLine;
        }

        public static IRectangle2 UpdateRelativeCoordinate(this IRectangle2 rectangle2,
            IRelativeCoordinate relativeCoordinate)
        {
            var relativeCenterVector = new Vector(rectangle2.X, rectangle2.Y);
            var actualCenterVector = relativeCoordinate.GetOriginalVector(relativeCenterVector);

            var relativeRect = new Rectangle2
            {
                X = actualCenterVector.X,
                Y = actualCenterVector.Y,
                Angle = relativeCoordinate.GetCoordinateAngle() + rectangle2.Angle,
                HalfWidth = rectangle2.HalfWidth,
                HalfHeight = rectangle2.HalfHeight
            };
            return relativeRect;
        }

        public static void UpdateRelativeCoordinate(this CircleSearchingResult circleResult,
                                                IRelativeCoordinate relativeCoordinate)
        {
            if (circleResult.Circle == null)
                return;

            var actualCenterPoint = circleResult.Circle.GetCenterPoint();
            var relativePoint = relativeCoordinate.GetRelativePoint(actualCenterPoint);
            circleResult.RelativeCircle = new Circle(relativePoint, circleResult.Circle.Radius);
        }

        public static void UpdateRelativeCoordinate(this IEnumerable<CircleSearchingResult> circleResults,
                                                 IRelativeCoordinate relativeCoordinate)
        {
            foreach (var circleResult in circleResults)
            {
                circleResult.UpdateRelativeCoordinate(relativeCoordinate);
            }
        }

        public static void UpdateRelativeCoordinate(this CircleSearchingDefinition definition,
            IRelativeCoordinate coordinate)
        {
            if (Math.Abs(definition.BaselineX) > 0.000001 || Math.Abs(definition.BaselineY) > 0.000001)
            {
                var relativeVector = new Vector(
                    definition.BaselineX * 1000.0 / definition.PixelCellSideLengthInMillimeter, 
                    definition.BaselineY * 1000.0 / definition.PixelCellSideLengthInMillimeter);

                var originalVector = coordinate.GetOriginalVector(relativeVector);
                definition.CenterX = originalVector.X;
                definition.CenterY = originalVector.Y;
            }
            else
            {
                definition.CenterX = definition.CenterX;
                definition.CenterY = definition.CenterY;
            }
        }

        public static void UpdateRelativeCoordinate(this IEnumerable<CircleSearchingDefinition> circleResults,
            IRelativeCoordinate relativeCoordinate)
        {
            foreach (var circleResult in circleResults)
            {
                circleResult.UpdateRelativeCoordinate(relativeCoordinate);
            }
        }

        public static void UpdateRelativeCoordinate(this EdgeSearchingDefinition def,
                                                  IRelativeCoordinate relativeCoordinate)
        {
            if (def.RelativeLine == null)
                return;

            if (def.RelativeLine.IsEmpty)
                return;
            //            if (Math.Abs(def.RelativeLine.X1) < 0.000001 ||
            //                  Math.Abs(def.RelativeLine.Y1) < 0.000001 ||
            //                  Math.Abs(def.RelativeLine.X2) < 0.000001 ||
            //                  Math.Abs(def.RelativeLine.Y2) < 0.000001)
            //            { return; }

            var p1 = def.RelativeLine.GetPoint1();
            var p2 = def.RelativeLine.GetPoint2();

            var actualP1 = relativeCoordinate.GetOriginalPoint(p1);
            var actualP2 = relativeCoordinate.GetOriginalPoint(p2);

            def.StartX = actualP1.X;
            def.StartY = actualP1.Y;

            def.EndX = actualP2.X;
            def.EndY = actualP2.Y;
        }

        public static void UpdateRelativeCoordinate(this IEnumerable<EdgeSearchingDefinition> definitions,
                                                    IRelativeCoordinate relativeCoordinate)
        {
            foreach (var esd in definitions)
            {
                esd.UpdateRelativeCoordinate(relativeCoordinate);
            }
        }

        public static void UpdateRelativeCoordinate(this IEnumerable<EdgeSearchingDefinition> definitions,
                                                    IDictionary<string, IRelativeCoordinate> relativeCoordinates)
        {
            foreach (var esd in definitions)
            {
                var relativeCoordinate = esd.CoordinateName.IsNullOrEmpty() ?
                    relativeCoordinates["Default"] :
                    relativeCoordinates[esd.CoordinateName];

                esd.UpdateRelativeCoordinate(relativeCoordinate);
            }
        }

        public static void UpdateRelativeCoordinate(this PartSearchingDefinition def,
            IRelativeCoordinate relativeCoordinate)
        {
            var p1 = def.RoiRelativeLine.GetPoint1();
            var p2 = def.RoiRelativeLine.GetPoint2();

            var actualP1 = relativeCoordinate.GetOriginalPoint(p1);
            var actualP2 = relativeCoordinate.GetOriginalPoint(p2);

            var roiLine = new Line { X1 = actualP1.X, Y1 = actualP1.Y, X2 = actualP2.X, Y2 = actualP2.Y };
            def.RoiLine = roiLine;

            //
            var p1a = def.AreaRelativeLine.GetPoint1();
            var p2b = def.AreaRelativeLine.GetPoint2();

            var actualP1a = relativeCoordinate.GetOriginalPoint(p1a);
            var actualP2b = relativeCoordinate.GetOriginalPoint(p2b);

            var areaLine = new Line { X1 = actualP1a.X, Y1 = actualP1a.Y, X2 = actualP2b.X, Y2 = actualP2b.Y };
            def.AreaLine = areaLine;
        }

        public static void UpdateRelativeCoordinate(this IEnumerable<PartSearchingDefinition> definitions,
                                                    IRelativeCoordinate relativeCoordinate)
        {
            foreach (var esd in definitions)
            {
                esd.UpdateRelativeCoordinate(relativeCoordinate);
            }
        }

        public static void UpdateRelativeCoordinate(this RegionTargetDefinition definition,
                                                    IRelativeCoordinate relativeCoordinate)
        {
            if (definition.RoiRelativeLine==null)
                return;

            if (definition.RoiRelativeLine.IsEmpty)
                return;

            var actualLine = definition.RoiRelativeLine.UpdateRelativeCoordinate(relativeCoordinate);
            definition.RoiActualLine = actualLine;
        }

        public static void UpdateRelativeCoordinate(this RegionSearchingDefinition definition,
                                                    IRelativeCoordinate relativeCoordinate)
        {
            if (definition.RoiRelativeLine==null)
                return;

            if (definition.RoiRelativeLine.IsEmpty)
                return;

            var actualLine = definition.RoiRelativeLine.UpdateRelativeCoordinate(relativeCoordinate);
            definition.RoiActualLine = actualLine;
        }

        public static void UpdateRelativeCoordinate(this IEnumerable<RegionTargetDefinition> definitions,
            IRelativeCoordinate relativeCoordinate)
        {
            foreach (var esd in definitions)
            {
                esd.UpdateRelativeCoordinate(relativeCoordinate);
            }
        }

        public static void UpdateRelativeCoordinate(this IEnumerable<RegionSearchingDefinition> definitions,
            IDictionary<string, IRelativeCoordinate> relativeCoordinates)
        {
            foreach (var esd in definitions)
            {
                var relativeCoordinate = esd.CoordinateName.IsNullOrEmpty() ?
                    relativeCoordinates["Default"] :
                    relativeCoordinates[esd.CoordinateName];

                esd.UpdateRelativeCoordinate(relativeCoordinate);
            }
        }

        public static void UpdateRelativeCoordinate(this IEnumerable<RegionSearchingDefinition> definitions,
            IRelativeCoordinate relativeCoordinate)
        {
            foreach (var esd in definitions)
            {
                esd.UpdateRelativeCoordinate(relativeCoordinate);
            }
        }

        public static void UpdateRelativeCoordinate(this SurfacePartDefinition def,
                                                    IRelativeCoordinate relativeCoordinate)
        {
            if (def.RoiRelativeRect == null) return;
            var rect = def.RoiRelativeRect.UpdateRelativeCoordinate(relativeCoordinate);
            def.RoiActualRect = rect;
        }

        public static void UpdateRelativeCoordinate(this SurfaceDefinition def,
            IRelativeCoordinate relativeCoordinate)
        {
            foreach (var includeRegion in def.IncludeParts)
            {
                includeRegion.UpdateRelativeCoordinate(relativeCoordinate);
            }

            foreach (var excludeRegion in def.ExcludeParts)
            {
                excludeRegion.UpdateRelativeCoordinate(relativeCoordinate);
            }
        }

        public static void UpdateRelativeCoordinate(this IEnumerable<SurfaceDefinition> definitions,
                                                    IRelativeCoordinate relativeCoordinate)
        {
            foreach (var esd in definitions)
            {
                esd.UpdateRelativeCoordinate(relativeCoordinate);
            }
        }

        public static void UpdateRelativeCoordinate(this XldSearchingDefinition def,
            IRelativeCoordinate relativeCoordinate)
        {
            def.RoiActualLine = def.RoiRelativeLine.UpdateRelativeCoordinate(relativeCoordinate);
        }

        public static void UpdateRelativeCoordinate(this IEnumerable<XldSearchingDefinition> definitions,
                                                    IRelativeCoordinate relativeCoordinate)
        {
            foreach (var esd in definitions)
            {
                esd.UpdateRelativeCoordinate(relativeCoordinate);
            }
        }

        public static void UpdateRelativeCoordinate(this IEnumerable<XldSearchingDefinition> definitions,
                                                    IDictionary<string, IRelativeCoordinate> relativeCoordinates)
        {
            foreach (var esd in definitions)
            {
                var relativeCoordinate = esd.CoordinateName.IsNullOrEmpty() ? 
                    relativeCoordinates["Default"] : 
                    relativeCoordinates[esd.CoordinateName];

                esd.UpdateRelativeCoordinate(relativeCoordinate);
            }
        }

        public static void UpdateRelativeCoordinate(this PointDefinition def,
            IRelativeCoordinate relativeCoordinate)
        {
            var relativeLineInPixel = def.RelativePoint.GetPointInPixel(def.UnitType, 
                def.PixelCellSideLengthInMillimeter);
            def.ActualPoint = relativeLineInPixel.UpdateRelativeCoordinate(relativeCoordinate);
        }

        public static void UpdateRelativeCoordinate(this LineDefinition def,
            IRelativeCoordinate relativeCoordinate)
        {
            if (def.RelativeLine == null)
                return;

            var relativeLineInPixel = def.RelativeLine.GetLineInPixel(def.UnitType, 
                def.PixelCellSideLengthInMillimeter);
            def.ActualLine = relativeLineInPixel.UpdateRelativeCoordinate(relativeCoordinate);
        }

        public static void UpdateRelativeCoordinate(this IEnumerable<LineDefinition> definitions,
                                                    IRelativeCoordinate relativeCoordinate)
        {
            foreach (var esd in definitions)
            {
                esd.UpdateRelativeCoordinate(relativeCoordinate);
            }
        }

        public static void UpdateRelativeCoordinate(this IEnumerable<PointDefinition> definitions,
                                                    IDictionary<string, IRelativeCoordinate> relativeCoordinates)
        {
            foreach (var esd in definitions)
            {
                var relativeCoordinate = esd.CoordinateName.IsNullOrEmpty() ?
                    relativeCoordinates["Default"] :
                    relativeCoordinates[esd.CoordinateName];

                esd.UpdateRelativeCoordinate(relativeCoordinate);
            }
        }

        public static void UpdateRelativeCoordinate(this IEnumerable<LineDefinition> definitions,
                                                    IDictionary<string, IRelativeCoordinate> relativeCoordinates)
        {
            foreach (var esd in definitions)
            {
                var relativeCoordinate = esd.CoordinateName.IsNullOrEmpty() ?
                    relativeCoordinates["Default"] :
                    relativeCoordinates[esd.CoordinateName];

                esd.UpdateRelativeCoordinate(relativeCoordinate);
            }
        }

        public static void UpdateRelativeCoordinate(this RadialLineDefinition def,
            IRelativeCoordinate relativeCoordinate)
        {
            var RelativeOriginXInPixel = def.RelativeOriginX.GetPixelValue(def.UnitType, 
                def.PixelCellSideLengthInMillimeter);
            var RelativeOriginYInPixel = def.RelativeOriginY.GetPixelValue(def.UnitType, 
                def.PixelCellSideLengthInMillimeter);
            var RelativeRadiusInPixel = def.RelativeRadius.GetPixelValue(def.UnitType, 
                def.PixelCellSideLengthInMillimeter);

            var actualOrigin = relativeCoordinate.GetOriginalVector(
                new Vector(RelativeOriginXInPixel, RelativeOriginYInPixel));

            def.ActualOriginX = actualOrigin.X;
            def.ActualOriginY = actualOrigin.Y;
            def.ActualRadius = RelativeRadiusInPixel;
            def.Angle = def.Angle + relativeCoordinate.GetCoordinateAngle();

            // if ReferenceRelativeRadius or ReferenceActualRadius >= 0, use the radius
            // otherwise use distance between ReferenceOrigin and ReferenceFarPoint to be radius
            var ReferenceRelativeRadiusInPixel = def.ReferenceRelativeRadius.GetPixelValue(
                def.UnitType,def.PixelCellSideLengthInMillimeter);
            def.ReferenceActualRadius = ReferenceRelativeRadiusInPixel;
        }

        public static void UpdateRelativeCoordinate(this IEnumerable<RadialLineDefinition> definitions,
                                                    IRelativeCoordinate relativeCoordinate)
        {
            foreach (var esd in definitions)
            {
                esd.UpdateRelativeCoordinate(relativeCoordinate);
            }
        }

        public static void UpdateRelativeCoordinate(this IEnumerable<RadialLineDefinition> definitions,
                                                    IDictionary<string, IRelativeCoordinate> relativeCoordinates)
        {
            foreach (var esd in definitions)
            {
                var relativeCoordinate = esd.CoordinateName.IsNullOrEmpty() ?
                    relativeCoordinates["Default"] :
                    relativeCoordinates[esd.CoordinateName];

                esd.UpdateRelativeCoordinate(relativeCoordinate);
            }
        }

        public static IEnumerable<EdgeSearchingDefinition> GetEdgeSearchingDefinitions(
            this InspectionResult inspectionResult)
        {
            return inspectionResult.Edges.Select(x => x.Definition);
        }

        public static IEnumerable<DataCodeSearchingDefinition> GetDataCodeSearchingDefinitions(
            this InspectionResult inspectionResult)
        {
            return inspectionResult.DataCodeSearchingResults.Select(x => x.Definition);
        }

        public static IEnumerable<PartSearchingDefinition> GetPartSearchingDefinitions(
            this InspectionResult inspectionResult)
        {
            return inspectionResult.Parts.Select(x => x.Definition);
        }

        public static IEnumerable<RegionTargetDefinition> GetRegionTargetDefinitions(
            this InspectionResult inspectionResult)
        {
            return inspectionResult.RegionTargets.Select(x => x.Definition);
        }

        public static IEnumerable<EdgeSearchingDefinition> GetCoordinateEdges(this InspectionResult inspectionResult)
        {
            return inspectionResult.CoordinateEdges.Select(x => x.Definition);
        }

        public static IEnumerable<CircleSearchingDefinition> GetCircleSearchingDefinitions(
            this InspectionResult inspectionResult)
        {
            return inspectionResult.Circles.Select(x => x.Definition);
        }

        public static IEnumerable<CircleSearchingDefinition> GetCoordinateCircleSearchingDefinitions(
            this InspectionResult inspectionResult)
        {
            return inspectionResult.CoordinateCircles.Select(x => x.Definition);
        }

        public static RegionResult GetRegionResult(this SurfaceResult surfaceResult, string regionName)
        {
            var region = surfaceResult.IncludeRegionResults.SingleOrDefault(x => x.RegionName == regionName);
            return region;
        }

        public static RegionResult GetRegionResult(this IEnumerable<SurfaceResult> surfaceResults, string surfaceName,
                                                   string regionName)
        {
            var surface = surfaceResults.SingleOrDefault(x => x.Definition.Name == surfaceName);
            if (surface == null) return null;
            return surface.GetRegionResult(regionName);
        }

        public static bool IsHorizontalOrVertical(this IRoiRectangle roiRectangle)
        {
            if (Math.Abs(roiRectangle.StartX - roiRectangle.EndX) < 0.000001 ||
                Math.Abs(roiRectangle.StartY - roiRectangle.EndY) < 0.000001)
            {
                return true;
            }

            return false;
        }

        public static bool IsHorizontalOrVertical(this Line line)
        {
            if (Math.Abs(line.X1 - line.X2) < 0.000001 ||
                Math.Abs(line.Y1 - line.Y2) < 0.000001)
            {
                return true;
            }

            return false;
        }

        public static void UpdateRelativeCoordinate(this IEnumerable<DataCodeSearchingDefinition> definitions,
                                                    IDictionary<string, IRelativeCoordinate> relativeCoordinates)
        {
            foreach (var esd in definitions)
            {
                var relativeCoordinate = esd.CoordinateName.IsNullOrEmpty() ?
                    relativeCoordinates["Default"] :
                    relativeCoordinates[esd.CoordinateName];

                esd.UpdateRelativeCoordinate(relativeCoordinate);
            }
        }

        public static void UpdateRelativeCoordinate(this DataCodeSearchingDefinition def,
                                                  IRelativeCoordinate relativeCoordinate)
        {
            if (def.RelativeLine == null)
                return;

            if (def.RelativeLine.IsEmpty)
                return;

            var p1 = def.RelativeLine.GetPoint1();
            var p2 = def.RelativeLine.GetPoint2();

            var actualP1 = relativeCoordinate.GetOriginalPoint(p1);
            var actualP2 = relativeCoordinate.GetOriginalPoint(p2);

            def.StartX = actualP1.X;
            def.StartY = actualP1.Y;

            def.EndX = actualP2.X;
            def.EndY = actualP2.Y;
        }
    }
}