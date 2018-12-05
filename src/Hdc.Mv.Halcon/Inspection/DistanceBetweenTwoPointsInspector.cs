using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Core;

namespace Hdc.Mv.Inspection
{
    public class DistanceBetweenTwoPointsInspector
    {
        public DistanceBetweenTwoPointsResult Calculate(
            DistanceBetweenTwoPointsDefinition definition, InspectionResult inspectionResult,
            IDictionary<string, IRelativeCoordinate> coordinates)
        {
            var result = new DistanceBetweenTwoPointsResult()
            {
                Definition = definition.DeepClone()
            };

            isXldAndRadial = definition.IsXldAndRadial;
            var PointOfEdgeAndRadialLineExit = inspectionResult.PointOfEdgeAndRadialLineResults
                    .Where(xx => xx.Definition != null)
                    .SingleOrDefault(xx => xx.Definition.Name == definition.PointName2);
            if (PointOfEdgeAndRadialLineExit!=null)
            {
                isPointOfEdgeAndRadialLineExit = !PointOfEdgeAndRadialLineExit.HasError;
            }
            else
            {
                isPointOfEdgeAndRadialLineExit = false;
            }
            var point1Result = GetPointFromInspectionResult(definition.PointName1, inspectionResult);
            var point2Result = GetPointFromInspectionResult(definition.PointName2, inspectionResult);

            var coord = coordinates[definition.CoordinateName];
            var relativeVector1 = coord.GetRelativeVector(new Vector(point1Result.ActualX, point1Result.ActualY));
            var relativeVector2 = coord.GetRelativeVector(new Vector(point2Result.ActualX, point2Result.ActualY));

            result.StartPointXPath = point1Result.ActualX;
            result.StartPointYPath = point1Result.ActualY;
            result.EndPointXPath = point2Result.ActualX;
            result.EndPointYPath = point2Result.ActualY;

            result.Distance = (relativeVector1 - relativeVector2).Length;
            result.DistanceInXAxis = Math.Abs(relativeVector1.X - relativeVector2.X);
            result.DistanceInYAxis = Math.Abs(relativeVector1.Y - relativeVector2.Y);

            if (point2Result.ActualX==-999.999)
            {
                result.Distance = -999999;
                result.DistanceInXAxis = -999999;
                result.DistanceInYAxis = -9999999;

            }

            return result;
        }

        private static PointDefinition GetPointFromInspectionResult(
            string pointName,
            InspectionResult inspectionResult)
        {

            var point1Result = inspectionResult.ReferencePointDefinitions
                .SingleOrDefault(x => x.Name == pointName);
            if (point1Result != null)
                return new PointDefinition
                {
                    ActualX = point1Result.ActualX,
                    ActualY = point1Result.ActualY,
                };
            if (isXldAndRadial)
            {
                var point2Result = inspectionResult.PointOfXldAndRadialLineResults
                .Where(x => x.Definition != null)
                .SingleOrDefault(x => x.Definition.Name == pointName);
                if (point2Result != null)
                    return new PointDefinition
                    {
                        ActualX = point2Result.X,
                        ActualY = point2Result.Y,
                    };
            }
            else
            {
                if (isPointOfEdgeAndRadialLineExit)
                {
                    var point2Result = inspectionResult.PointOfEdgeAndRadialLineResults
                  .Where(x => x.Definition != null)
                  .SingleOrDefault(x => x.Definition.Name == pointName);
                    if (point2Result != null)
                        return new PointDefinition
                        {
                            ActualX = point2Result.X,
                            ActualY = point2Result.Y,
                        };
                }
                
            }
            

            return new PointDefinition
            {
                ActualX = -999.999,
                ActualY = -999.999,
            };
        }

        private static bool isXldAndRadial;
        private static bool isPointOfEdgeAndRadialLineExit;
    }
}