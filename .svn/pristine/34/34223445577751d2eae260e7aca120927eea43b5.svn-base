using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Core;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    public class PointOfXldAndRadialLineInspector
    {
        public PointOfXldAndRadialLineResult CalculatePointOfXldAndRadialLine(
            HImage image,
            PointOfXldAndRadialLineDefinition definition, 
            InspectionResult inspectionResult)
        {
            var result = new PointOfXldAndRadialLineResult()
            {
                Definition = definition.DeepClone()
            };

            var xldResult = inspectionResult.XldSearchingResults
                .Where(x => x.Definition != null)
                .SingleOrDefault(x => x.Definition.Name == definition.XldName);

            var lineResult = inspectionResult.ReferenceRadialLineDefinitions
                .SingleOrDefault(x => x.Name == definition.RadialLineName);

            if (xldResult == null || lineResult == null)
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
            FindPointOfXldAndRadialLine(definition.SelectionMode, lineResult, xldResult, out finalX, out finalY, out finalDistance);

            result.X = finalX;
            result.Y = finalY;
            result.Distance = finalDistance;

            result.ActualOriginX = lineResult.ActualOriginX;
            result.ActualOriginY = lineResult.ActualOriginY;

            result.RelativeOriginX = lineResult.RelativeOriginX;
            result.RelativeOriginY = lineResult.RelativeOriginY;

            return result;
        }

        public static void FindPointOfXldAndRadialLine(
            SelectionMode selectionMode, 
            RadialLineDefinition radialLineDef, 
            XldSearchingResult xldResult, 
            out double finalX, 
            out double finalY, 
            out double finalDistance)
        {
            var startVector = radialLineDef.StartVector;
            var endVector = radialLineDef.EndVector;

            var originVector = new Vector()
            {
                X = radialLineDef.ActualOriginX,
                Y = radialLineDef.ActualOriginY
            };
            HTuple xs, ys, isOverlapping;

            finalX = -1;
            finalY = -1;
            finalDistance = -1;
            List<double> distances = new List<double>();

            var contCount = xldResult.Xld.CountObj();

            for (int i = 0; i < contCount; i++)
            {
                var xldClip = xldResult.Xld.SelectObj(i + 1);

                HOperatorSet.IntersectionSegmentContourXld(xldClip,
                    startVector.Y, startVector.X,
                    endVector.Y, endVector.X,
                    out ys, out xs, out isOverlapping);

                if (xs.Length == 0 || ys.Length == 0)
                    continue;

                for (int j = 0; j < xs.Length; j++)
                {
                    var x = xs[j];
                    var y = ys[j];
                    var v = originVector - new Vector(x, y);
                    var distance = v.Length;

                    var pixelRadius = radialLineDef.ActualRadius;

                    if (distance > pixelRadius)
                        continue;

                    distances.Add(distance);

                    if (i == 0 && j == 0)
                    {
                        finalDistance = distance;
                        finalX = x;
                        finalY = y;
                    }
                    else
                    {
                        if (selectionMode == SelectionMode.First)
                        {
                            if (distance < finalDistance)
                            {
                                finalDistance = distance;
                                finalX = x;
                                finalY = y;
                            }
                        }
                        else if (selectionMode == SelectionMode.Last)
                        {
                            if (distance > finalDistance)
                            {
                                finalDistance = distance;
                                finalX = x;
                                finalY = y;
                            }
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                }
            }
        }
    }
}