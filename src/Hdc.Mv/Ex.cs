﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media;
using Core.IO;
using Core.Linq;
using Core.Reflection;
using Core.Windows.Media.Imaging;

namespace Hdc.Mv
{
    public static class Ex
    {
        static Ex()
        {
        }

        public static Point ToPoint(this Vector vector)
        {
            return new Point(vector.X, vector.Y);
        }

        public static Vector ToVector(this Point point)
        {
            return new Vector(point.X, point.Y);
        }

        public static Point GetCenterPoint(this Circle circle)
        {
            return new Point(circle.CenterX, circle.CenterY);
        }

        public static Vector GetCenterVector(this Circle circle)
        {
            return new Vector(circle.CenterX, circle.CenterY);
        }

        public static double GetDistanceToOrigin(this Circle circle)
        {
            return circle.GetCenterVector().Length;
        }

        public static Vector GetVectorTo(this Circle circle1, Circle circle2)
        {
            var v1 = circle1.GetCenterVector();
            var v2 = circle2.GetCenterVector();
            var v1to2 = v1 - v2;
            return v1to2;
        }

        public static Vector GetVectorTo(this Point p1, Point p2)
        {
            var v1 = p1.ToVector();
            var v2 = p2.ToVector();
            var v = v1 - v2;
            return v;
        }

        public static Vector GetVectorTo(this Vector p1, Vector p2)
        {
            var v = p1 - p2;
            return v;
        }

        public static double GetDistanceTo(this Circle circle1, Circle circle2)
        {
            return circle1.GetVectorTo(circle2).Length;
        }

        public static Point GetPoint1(this Line line)
        {
            return new Point(line.X1, line.Y1);
        }

        public static Point GetPoint2(this Line line)
        {
            return new Point(line.X2, line.Y2);
        }

        public static Vector GetVector1(this Line line)
        {
            return new Vector(line.X1, line.Y1);
        }

        public static Vector GetVector2(this Line line)
        {
            return new Vector(line.X2, line.Y2);
        }

        public static Vector GetVectorFrom1To2(this Line line)
        {
            var v1 = line.GetVector1();
            var v2 = line.GetVector2();
            var v = v1 - v2;
            return v;
        }

        public static Vector GetVectorFrom2To1(this Line line)
        {
            var v1 = line.GetVector1();
            var v2 = line.GetVector2();
            var v = v2 - v1;
            return v;
        }

        public static double GetLength(this Line line)
        {
            return line.GetVectorFrom1To2().Length;
        }

        public static Vector Rotate(this Vector vector, double angle)
        {
            var matrix = new Matrix();
            matrix.Rotate(angle);
            var rotatedVector = matrix.Transform(vector);
            return rotatedVector;
        }

        public static double GetAngleTo(this Vector fromVector, Vector toVector)
        {
            return Vector.AngleBetween(fromVector, toVector);
        }

        public static double GetAngleToX(this Vector fromVector)
        {
            return Vector.AngleBetween(fromVector, new Vector(10000,0));
        }

        public static double GetAngleFromX(this Vector toVector)
        {
            return Vector.AngleBetween(new Vector(10000, 0), toVector);
        }

        public static Point GetRelativePoint(this Point point, Line baseLine, double angle)
        {
            var vFromOriginToTarget = point.GetVectorTo(baseLine.GetPoint1());
            var vFromOriginToRight = baseLine.GetPoint2().GetVectorTo(baseLine.GetPoint1());
            var coordinateVector = vFromOriginToRight.Rotate(angle);
            // 0 degree mains: the line is X, direct to right. x>0, follow the clock.

            var angleBetweenTargetAndRight = vFromOriginToTarget.GetAngleTo(coordinateVector);

            var vectorNoAngle = new Vector(vFromOriginToTarget.Length, 0);
            var vectorIncludeAngle = vectorNoAngle.Rotate(0 - angleBetweenTargetAndRight);
            return vectorIncludeAngle.ToPoint();
        }

        public static Point CreateRelativeC(this Point point, Line baseLine, double angle)
        {
            var vFromOriginToTarget = point.GetVectorTo(baseLine.GetPoint1());
            var vFromOriginToRight = baseLine.GetPoint2().GetVectorTo(baseLine.GetPoint1());
            var coordinateVector = vFromOriginToRight.Rotate(angle);
            // 0 degree mains: the line is X, direct to right. x>0, follow the clock.

            var angleBetweenTargetAndRight = vFromOriginToTarget.GetAngleTo(coordinateVector);

            var vectorNoAngle = new Vector(vFromOriginToTarget.Length, 0);
            var vectorIncludeAngle = vectorNoAngle.Rotate(0 - angleBetweenTargetAndRight);
            return vectorIncludeAngle.ToPoint();
        }

        public static Point GetCenterPoint(this Line line)
        {
            return new Point((line.X1 + line.X2)/2.0, (line.Y1 + line.Y2)/2.0);
        }

        public static string ToNumbericString(this double value, int intCount = 4)
        {
            switch (intCount)
            {
                case 0:
                    throw new NotSupportedException();
                case 1:
                    return value.ToString("+0.000;-0.000");
                case 2:
                    return value.ToString("+00.000;-00.000");
                case 3:
                    return value.ToString("+000.000;-000.000");
                case 4:
                    return value.ToString("+0000.000;-0000.000");
                case 5:
                    return value.ToString("+00000.000;-00000.000");
                case 6:
                    return value.ToString("+000000.000;-000000.000");
            }
            return null;
        }

        public static double ToMillimeterFromPixel(this double value, double factor)
        {
            return value*factor/1000.0;
        }

        public static double ToMicrometerFromPixel(this double value, double factor)
        {
            return value*factor;
        }

        public static string ToNumbericStringInMillimeterFromPixel(this double value, double factor, int intCount = 4)
        {
            return value.ToMillimeterFromPixel(factor).ToNumbericString(intCount); //+" mm";
        }

        public static string ToNumbericStringInMicrometerFromPixel(this double value, double factor, int intCount = 4)
        {
            return value.ToMicrometerFromPixel(factor).ToNumbericString(intCount); // + " um";
        }

        public static string ToHalconString(this Polarity polarity)
        {
            string polarityString = null;

            switch (polarity)
            {
                case Polarity.All:
                    polarityString = "all";
                    break;
                case Polarity.Negative:
                    polarityString = "negative";
                    break;
                case Polarity.Positive:
                    polarityString = "positive";
                    break;
            }

            return polarityString;
        }


        public static string ToHalconString(this Transition transition)
        {
            string polarityString = null;

            switch (transition)
            {
                case Transition.All:
                    polarityString = "all";
                    break;
                case Transition.Negative:
                    polarityString = "negative";
                    break;
                case Transition.Positive:
                    polarityString = "positive";
                    break;
            }

            return polarityString;
        }


        public static string ToHalconString(this SelectionMode selectionMode)
        {
            string selectionModeString = null;

            switch (selectionMode)
            {
                case SelectionMode.Max:
                    selectionModeString = "max";
                    break;
                case SelectionMode.First:
                    selectionModeString = "first";
                    break;
                case SelectionMode.Last:
                    selectionModeString = "last";
                    break;
            }

            return selectionModeString;
        }


        public static string ToHalconString(this CircleDirect selectionMode)
        {
            string selectionModeString = null;

            switch (selectionMode)
            {
                case CircleDirect.Inner:
                    selectionModeString = "inner";
                    break;
                case CircleDirect.Outer:
                    selectionModeString = "outer";
                    break;
            }

            return selectionModeString;
        }


        public static string ToHalconString(this Order order)
        {
            switch (order)
            {
                case Order.Increase:
                    return "true";
                case Order.Decrease:
                    return "false";
                default:
                    throw new InvalidOperationException("Order cannot convert to string");
            }
        }


        public static string ToHalconString(this LogicOperation operation)
        {
            switch (operation)
            {
                case LogicOperation.And:
                    return "and";
                case LogicOperation.Or:
                    return "or";
                default:
                    throw new InvalidOperationException("LogicOperation cannot convert to string: " + operation);
            }
        }



        public static string ToHalconString(this ShapeFeature feature)
        {
            switch (feature)
            {
                case ShapeFeature.Area:
                    return "area";
                case ShapeFeature.Bulkiness:
                    return "bulkiness";
                case ShapeFeature.Circularity:
                    return "circularity";
                case ShapeFeature.Row:
                    return "row";
                case ShapeFeature.Column:
                    return "column";
                case ShapeFeature.Row1:
                    return "row1";
                case ShapeFeature.Column1:
                    return "column1";
                case ShapeFeature.Row2:
                    return "row2";
                case ShapeFeature.Column2:
                    return "column2";
                case ShapeFeature.Convexity:
                    return "convexity";
                case ShapeFeature.Height:
                    return "height";
                case ShapeFeature.Width:
                    return "width";
                case ShapeFeature.Roundness:
                    return "roundness";
                case ShapeFeature.Rect2Len1:
                    return "rect2_len1";
                case ShapeFeature.Rect2Len2:
                    return "rect2_len2";
                case ShapeFeature.Rect2Phi:
                    return "rect2_phi";

                case ShapeFeature.OuterRadius:
                    return "outer_radius";
                case ShapeFeature.InnerRadius:
                    return "inner_radius";
                case ShapeFeature.InnerWidth:
                    return "inner_width";
                case ShapeFeature.InnerHeight:
                    return "inner_height";
                case ShapeFeature.ContLength:
                    return "contlength";

                default:
                    throw new InvalidOperationException("ShapeFeature cannot convert to string: " + feature);
            }
        }

        public static Line Reverse(this Line line)
        {
            return new Line(line.X2, line.Y2, line.X1, line.Y1);
        }

        public static Line GetLine(this Circle circle, double angle)
        {
            var centerVector = circle.GetCenterVector();
            var leftVector = new Vector(circle.Radius, 0).Rotate(angle);
            var rightVector = new Vector(circle.Radius, 0).Rotate(angle - 180);
            var offsetLeft = leftVector + centerVector;
            var offsetRight = rightVector + centerVector;
            return new Line(offsetLeft.ToPoint(), offsetRight.ToPoint());
        }

        public static Line GetLine(this IRoiRectangle roiRectangle)
        {
            return new Line(roiRectangle.StartX,
                roiRectangle.StartY,
                roiRectangle.EndX,
                roiRectangle.EndY);
        }

        public static Line GetWidthLine(this IRoiRectangle roiRectangle)
        {
            var centerVector = roiRectangle.GetCenterVector();
            var linkLine = roiRectangle.GetLine().GetVectorFrom2To1().GetAngleToX();

            var leftVector = new Vector(roiRectangle.ROIWidth, 0).Rotate(linkLine - 90);
            var rightVector = new Vector(roiRectangle.ROIWidth, 0).Rotate(linkLine + 90);
            var offsetLeft = leftVector + centerVector;
            var offsetRight = rightVector + centerVector;
            return new Line(offsetLeft.ToPoint(),offsetRight.ToPoint());
        }

        public static Vector GetStartVector(this IRoiRectangle roiRectangle)
        {
            return new Vector(roiRectangle.StartX , roiRectangle.StartY);
        }

        public static Vector GetEndVector(this IRoiRectangle roiRectangle)
        {
            return new Vector(roiRectangle.EndX , roiRectangle.EndY);
        }

        public static Vector GetCenterVector(this IRoiRectangle roiRectangle)
        {
            return new Vector((roiRectangle.StartX + roiRectangle.EndX) / 2.0, (roiRectangle.StartY + roiRectangle.EndY) / 2.0);
        }

        public static double GetPixelValue(this double valueInDouble, UnitType unitType, double pixelCellSideLengthInMillimeter)
        {
            switch (unitType)
            {
                case UnitType.Pixel:
                    return valueInDouble;
                case UnitType.Millimeter:
                    return valueInDouble / pixelCellSideLengthInMillimeter;
                case UnitType.Micrometer:
                    return valueInDouble * 1000.0 / pixelCellSideLengthInMillimeter;
                case UnitType.Meter:
                    return valueInDouble * 1000000.0 / pixelCellSideLengthInMillimeter;
                default:
                    throw new NotImplementedException();
            }
        }

        public static Line GetLineInPixel(this Line line, UnitType unitType, double pixelCellSideLengthInMillimeter)
        {
            var relativeLineInPixel = new Line(
              line.X1.GetPixelValue(unitType, pixelCellSideLengthInMillimeter),
              line.Y1.GetPixelValue(unitType, pixelCellSideLengthInMillimeter),
              line.X2.GetPixelValue(unitType, pixelCellSideLengthInMillimeter),
              line.Y2.GetPixelValue(unitType, pixelCellSideLengthInMillimeter)
              );
            return relativeLineInPixel;
        }

        public static Point GetPointInPixel(this Point point, UnitType unitType, double pixelCellSideLengthInMillimeter)
        {
            var pointInPixel = new Point(
              point.X.GetPixelValue(unitType, pixelCellSideLengthInMillimeter),
              point.Y.GetPixelValue(unitType, pixelCellSideLengthInMillimeter)
              );
            return pointInPixel;
        }
    }
}