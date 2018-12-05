using System;
using System.Linq;
using Core;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    public class IntersectionPointOfTwoShapesInspector
    {
        public IntersectionPointOfTwoShapesResult CalculateIntersectionPointOfTwoShapes(HImage image,
            IntersectionPointOfTwoShapesDefinition definition, InspectionResult inspectionResult)
        {
            var result = new IntersectionPointOfTwoShapesResult()
            {
                Definition = definition.DeepClone()
            };

            // Line to Line
            if (definition.ShapeType1 == ShapeType.Line &&
                definition.ShapeType2 == ShapeType.Line)
            {
                var lineDefinition1 = inspectionResult.ReferenceLineDefinitions
                    .SingleOrDefault(x => x.Name == definition.ShapeName1);

                var lineDefinition2 = inspectionResult.ReferenceLineDefinitions
                    .SingleOrDefault(x => x.Name == definition.ShapeName2);

                if (lineDefinition1 == null || lineDefinition2 == null)
                {
                    result.HasError = true;
                    return result;
                }

                var intersection = lineDefinition1.ActualLine.IntersectionWith(lineDefinition2.ActualLine);
                result.X = intersection.X;
                result.Y = intersection.Y;
                return result;
            }

            // RadialLine to XLD
            if (definition.ShapeType1 == ShapeType.RadialLine &&
                definition.ShapeType2 == ShapeType.Xld)
            {
                var radialLineDefinition = inspectionResult.ReferenceRadialLineDefinitions
                    .SingleOrDefault(x => x.Name == definition.ShapeName1);

                var xldResult = inspectionResult.XldSearchingResults
                    .Where(x => x.Definition != null)
                    .SingleOrDefault(x => x.Definition.Name == definition.ShapeName2);

                if (xldResult == null || radialLineDefinition == null)
                {
                    result.HasError = true;
                    return result;
                }

                if (xldResult.Xld.CountObj() <= 0)
                {
                    result.HasError = true;
                    return result;
                }

                double finalX;
                double finalY;
                double finalDistance;

                PointOfXldAndRadialLineInspector.FindPointOfXldAndRadialLine(
                    definition.SelectionMode, radialLineDefinition, xldResult, 
                    out finalX, out finalY, out finalDistance);

                result.X = finalX;
                result.Y = finalY;
                result.Distance = finalDistance;

                return result;
            }

            throw new NotImplementedException($"{nameof(ShapeType)} is not support: " +
                                              $"{nameof(definition.ShapeType1)}={definition.ShapeType1}, " +
                                              $"{nameof(definition.ShapeType2)} ={definition.ShapeType2}");
        }
    }
}