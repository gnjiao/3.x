using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Core;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    public class PointOfEdgeAndRadialLineInspector
    {
        public PointOfEdgeAndRadialLineResult CalculatePointOfEdgeAndRadialLine(
                    HImage image,
                    PointOfEdgeAndRadialLineDefinition definition,
                    InspectionResult inspectionResult)
        {
            var result = new PointOfEdgeAndRadialLineResult()
            {
                Definition = definition.DeepClone()
            };

            List<EdgeSearchingResult> edgeResulttt = new List<EdgeSearchingResult>();

            EdgeSearchingResult edgeResult =new EdgeSearchingResult();

            foreach (var edge in inspectionResult.Edges)
            {
                if (edge.Definition.Name.StartsWith(definition.EdgeName))
                    edgeResulttt.Add(edge);
            }

            if (edgeResulttt.Count > 1)
            {
                edgeResult = GetTheRightEdge(edgeResulttt,definition.Side, definition.OutOrIn);
            }
            else
            {
                edgeResult = edgeResulttt[0];
            }

            /*   var edgeResult = inspectionResult.Edges
                   .Where(x => x.Definition != null)
                   .SingleOrDefault(x => x.Definition.Name == definition.EdgeName);
             */
            var lineResult = inspectionResult.ReferenceRadialLineDefinitions 
                .SingleOrDefault(x => x.Name == definition.RadialLineName);

            if (edgeResult == null || lineResult == null)
            {
                result.HasError = true;
                return result;
            }

            if (edgeResult.HasError)
            {
                result.HasError = true;
                return result;
            }
            if (edgeResult.EdgeLine.GetLength() <= 0)
            {
                result.HasError = true;
                return result;
            }

            double finalX;
            double finalY;
            double finalDistance;
            FindPointOfEdgeAndRadialLine(definition.SelectionMode, lineResult, edgeResult, out finalX, out finalY, out finalDistance);

            result.X = finalX;
            result.Y = finalY;
            result.Distance = finalDistance;

            result.ActualOriginX = lineResult.ActualOriginX;
            result.ActualOriginY = lineResult.ActualOriginY;

            result.RelativeOriginX = lineResult.RelativeOriginX;
            result.RelativeOriginY = lineResult.RelativeOriginY;

            return result;
        }

        public static void FindPointOfEdgeAndRadialLine(
            SelectionMode selectionMode,
            RadialLineDefinition radialLineDef,
            EdgeSearchingResult edgeResult,
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
            HOperatorSet.IntersectionLines(edgeResult.EdgeLine.Row1, edgeResult.EdgeLine.Column1, edgeResult.EdgeLine.Row2, edgeResult.EdgeLine.Column2, startVector.Y, startVector.X,endVector.Y,endVector.X,out ys,out xs,out isOverlapping);

            finalX = xs;
            finalY = ys;
            #region MyRegion
            /*
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
           }*/
            #endregion
        }

        public EdgeSearchingResult GetTheRightEdge(List<EdgeSearchingResult> edgeResulttt, string Side, string outOrin)
        {
            var edgeResult = new EdgeSearchingResult();

            switch (Side)
            {
                case "Left":
                    if (outOrin == "Out")
                    {

                        if (Math.Abs(edgeResulttt[0].X1) <= Math.Abs(edgeResulttt[1].X1))
                            edgeResult = edgeResulttt[0];
                        else
                            edgeResult = edgeResulttt[1];
                    }
                    if (outOrin == "In")
                    {
                        if (Math.Abs(edgeResulttt[0].X1) > Math.Abs(edgeResulttt[1].X1))
                            edgeResult = edgeResulttt[0];
                        else
                            edgeResult = edgeResulttt[1];
                    }

                    break;
                case "Right":
                    if (outOrin == "Out")
                    {

                        if (Math.Abs(edgeResulttt[0].X1) >= Math.Abs(edgeResulttt[1].X1))
                            edgeResult = edgeResulttt[0];
                        else
                            edgeResult = edgeResulttt[1];
                    }
                    if (outOrin == "In")
                    {
                        if (Math.Abs(edgeResulttt[0].X1) < Math.Abs(edgeResulttt[1].X1))
                            edgeResult = edgeResulttt[0];
                        else
                            edgeResult = edgeResulttt[1];
                    }

                    break;
                case "Up":
                    if (outOrin == "Out")
                    {

                        if (Math.Abs(edgeResulttt[0].Y1) <= Math.Abs(edgeResulttt[1].Y1))
                            edgeResult = edgeResulttt[0];
                        else
                            edgeResult = edgeResulttt[1];
                    }
                    if (outOrin == "In")
                    {
                        if (Math.Abs(edgeResulttt[0].Y1) > Math.Abs(edgeResulttt[1].Y1))
                            edgeResult = edgeResulttt[0];
                        else
                            edgeResult = edgeResulttt[1];
                    }

                    break;
                case "Down":
                    if (outOrin == "Out")
                    {

                        if (Math.Abs(edgeResulttt[0].Y1) >= Math.Abs(edgeResulttt[1].Y1))
                            edgeResult = edgeResulttt[0];
                        else
                            edgeResult = edgeResulttt[1];
                    }
                    if (outOrin == "In")
                    {
                        if (Math.Abs(edgeResulttt[0].Y1) < Math.Abs(edgeResulttt[1].Y1))
                            edgeResult = edgeResulttt[0];
                        else
                            edgeResult = edgeResulttt[1];
                    }

                    break;
            }

            return edgeResult;
        }
    }
}