using System.Linq;
using System.Windows;
using Core;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    public class DistanceBetweenPointsOfXldAndRadialLineInspector
    {
        public DistanceBetweenPointsOfXldAndRadialLineResult Calculate(
            DistanceBetweenPointsOfXldAndRadialLineDefinition definition, InspectionResult inspectionResult)
        {
            var result = new DistanceBetweenPointsOfXldAndRadialLineResult()
            {
                Definition = definition.DeepClone()
            };

            var point1Result = inspectionResult.PointOfXldAndRadialLineResults
                .Where(x => x.Definition != null)
                .SingleOrDefault(x => x.Definition.Name == definition.Point1OfXldAndRadialLineName);

            var point2Result = inspectionResult.PointOfXldAndRadialLineResults
                .Where(x => x.Definition != null)
                .SingleOrDefault(x => x.Definition.Name == definition.Point2OfXldAndRadialLineName);

            if (point1Result == null || point2Result == null)
            {
                result.HasError = true;
                return result;
            }

            var vector1 = new Vector(point1Result.X, point1Result.Y);
            var vector2 = new Vector(point2Result.X, point2Result.Y);
            var link = vector1 - vector2;

            result.X1 = point1Result.X;
            result.Y1 = point1Result.Y;
            result.X2 = point2Result.X;
            result.Y2 = point2Result.Y;
            result.Distance = link.Length;

            return result;
        }
    }
}