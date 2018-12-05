using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using HalconDotNet;

namespace Hdc.Mv.Halcon
{
    public static class HRegionExtensions
    {
        public static int GetWidth(this HRegion region)
        {
            var value = region.RegionFeatures("width");
            var int32 = Convert.ToInt32(value);
            return int32;
        }

        public static int GetHeight(this HRegion region)
        {
            var value = region.RegionFeatures("height");
            var int32Value = Convert.ToInt32(value);
            return int32Value;
        }

        public static int GetRow1(this HRegion region)
        {
            var value = region.RegionFeatures("row1");
            var int32Value = Convert.ToInt32(value);
            return int32Value;
        }

        public static Point GetPoint1(this HRegion region)
        {
            var x = region.GetColumn1();
            var y = region.GetRow1();
            return new Point(x, y);
        }

        public static Point GetPoint2(this HRegion region)
        {
            var x = region.GetColumn2();
            var y = region.GetRow2();
            return new Point(x, y);
        }


        /// <summary>
        /// 区域中心，即重心
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public static Point GetCenterPoint(this HRegion region)
        {
            var x = region.GetColumn();
            var y = region.GetRow();
            return new Point(x, y);
        }

        public static int GetColumn1(this HRegion region)
        {
            var value = region.RegionFeatures("column1");
            var int32Value = Convert.ToInt32(value);
            return int32Value;
        }

        public static int GetCenterX(this HRegion region)
        {
            return region.GetColumn();
        }

        public static int GetCenterY(this HRegion region)
        {
            return region.GetRow();
        }

        public static int GetRow(this HRegion region)
        {
            var value = region.RegionFeatures("row");
            var int32Value = Convert.ToInt32(value);
            return int32Value;
        }

        public static int GetColumn(this HRegion region)
        {
            var value = region.RegionFeatures("column");
            var int32Value = Convert.ToInt32(value);
            return int32Value;
        }

        public static int GetRow2(this HRegion region)
        {
            var value = region.RegionFeatures("row2");
            var int32Value = Convert.ToInt32(value);
            return int32Value;
        }

        public static int GetColumn2(this HRegion region)
        {
            var value = region.RegionFeatures("column2");
            var int32Value = Convert.ToInt32(value);
            return int32Value;
        }

        public static double GetArea(this HRegion region)
        {
            var value = region.RegionFeatures("area");
            var int32Value = Convert.ToDouble(value);
            return int32Value;
        }

        public static IList<HRegion> ToList(this HRegion region)
        {
            IList<HRegion> list = new List<HRegion>();

            var count = region.CountObj();

            for (int i = 0; i < count; i++)
            {
                list.Add(region[i + 1]);
            }

            return list;
        }

        public static HRectangle2 GetSmallestHRectangle2(this HRegion region)
        {
            if (region.CountObj() == 0 || region.Area <= 0.0000001)
            {
                return new HRectangle2();
            }

            double row;
            double column;
            double phi;
            double length1;
            double length2;
            region.SmallestRectangle2(out row, out column, out phi, out length1, out length2);
            var smallestRect = new HRectangle2()
            {
                Row = row,
                Column = column,
                Phi = phi,
                Length1 = length1,
                Length2 = length2,
            };
            return smallestRect;
        }

        public static HRectangle1 GetSmallestRectangle1(this HRegion region)
        {
            int row1;
            int column1;
            int row2;
            int column2;
            region.SmallestRectangle1(out row1, out column1, out row2, out column2);
            var smallestRect = new HRectangle1()
            {
                Row1 = row1,
                Column1 = column1,
                Row2 = row2,
                Column2 = column2,
            };
            return smallestRect;
        }

        public static HRegion ToHRegion(this HRectangle1 rectangle1)
        {
            var region = new HRegion();
            region.GenRectangle1((double)rectangle1.Row1, rectangle1.Column1, rectangle1.Row2, rectangle1.Column2);

            return region;
        }

        public static HRegion GetSmallestRectangle1Region(this HRegion region)
        {
            int row1;
            int column1;
            int row2;
            int column2;
            region.SmallestRectangle1(out row1, out column1, out row2, out column2);

            var rect1 = new HRegion();
            rect1.GenRectangle1((double)row1, column1, row2, column2);

            return rect1;
        }

        public static Point GetSmallestRectangle1Center(this HRegion region)
        {
            var rect1 = region.GetSmallestRectangle1();
            var smallestRectangle1Center = new Point(
                (rect1.Column2 + rect1.Column1) / 2.0,
                (rect1.Row2 + rect1.Row1) / 2.0);
            return smallestRectangle1Center;
        }

        public static HRegion Union(this IEnumerable<HRegion> regions)
        {
            var region = new HRegion();
            region.GenEmptyRegion();
            foreach (var hRegion in regions)
            {
                var tempRegion = region;
                region = tempRegion.Union2(hRegion);
                tempRegion.Dispose();
            }

            return region;
        }

        public static HRegion Concatenate(this IEnumerable<HRegion> regions)
        {
            var regionList = regions.ToList();

            HRegion finalRegion = null;

            if (!regionList.Any())
            {
                finalRegion = new HRegion();
                finalRegion.GenEmptyRegion();
                return finalRegion;
            }

            foreach (var region in regionList)
            {
                if (finalRegion == null)
                {
                    finalRegion = region;
                    continue;
                }

                var temp = finalRegion;
                finalRegion = temp.ConcatObj(region);
                temp.Dispose();
            }
            var count = finalRegion.CountObj();
            return finalRegion;
        }

        public static IList<Line> SplitSkeletonLines(this HRegion region, int maxDistance)
        {
            HTuple beginRow, beginCol, endRow, endCol;
            region.SplitSkeletonLines(maxDistance, out beginRow, out beginCol,out endRow, out endCol);
            var count = beginRow.Length;

            List<Line> lines = new List<Line>();

            for (int i = 0; i < count; i++)
            {
                var line = new Line(beginCol[i], beginRow[i], endCol[i], endRow[i]);
                lines.Add(line);
            }

            return lines;
        }

        public static Line FitLineContourXld(this HRegion region)
        {
            var contour = region.GenContoursSkeletonXld(1, "filter");
            var line = contour.FitLineContourXld();
            return line;
        }

        public static ProjectionRectangle4 GetProjectionRectangle4(this HRegion region, BoundaryType boundaryType, int boundaryMaxDistance)
        {
            var centerRow = region.GetRow();
            var centerColumn = region.GetColumn();
            var row2 = region.GetRow2();
            var col2 = region.GetColumn2();

            var boundary = region.Boundary(boundaryType.ToHalconString());
            var dilationBoundary = boundary.DilationCircle(11.0);
            var skeleton = dilationBoundary.Skeleton();
            var pruning = skeleton.Pruning(2);
            HRegion junctionPointsRegion;
            var endPointsRegion = pruning.JunctionsSkeleton(out junctionPointsRegion);
            var branchesRegion = pruning.Difference(junctionPointsRegion);
            var connectedBranches = branchesRegion.Connection();
            var largeBranches = connectedBranches.SelectShape("area", "and", 10, 99999);

            var lineRegion = largeBranches.SplitSkeletonRegion(boundaryMaxDistance);

            var horiLineRegion = lineRegion.SelectShape("rect2_phi", "and", -5.0.ToArcDegree(), 5.0.ToArcDegree());
            var horiLineRegionLarge = horiLineRegion.SelectShape("area", "and", 100, 99999);
            var vertLine1 = lineRegion.SelectShape("rect2_phi", "and", -90.1.ToArcDegree(), -5.0.ToArcDegree());
            var vertLine2 = lineRegion.SelectShape("rect2_phi", "and", 5.0.ToArcDegree(), 90.1.ToArcDegree());
            var vertLineRegion = vertLine1.ConcatObj(vertLine2);
            var vertLineRegionLarge = vertLineRegion.SelectShape("area", "and", 100, 99999);

            // upper
            var upperLineRegion = horiLineRegionLarge.SelectShape("row", "and", 0, centerRow);
            var upperLine = upperLineRegion.FitLineContourXld();

            // lower
            var lowerLineRegion = horiLineRegionLarge.SelectShape("row", "and", centerRow, row2);
            var lowerLine = lowerLineRegion.FitLineContourXld();

            // left
            var leftLineRegion = vertLineRegionLarge.SelectShape("column", "and", 0, centerColumn);
            var leftLine = leftLineRegion.FitLineContourXld();

            // right
            var rightLineRegion = vertLineRegionLarge.SelectShape("column", "and", centerColumn, col2);
            var rightLine = rightLineRegion.FitLineContourXld();

            var upperLeftPoint = upperLine.IntersectionLines(leftLine);
            var upperRightPoint = upperLine.IntersectionLines(rightLine);
            var lowerLeftPoint = lowerLine.IntersectionLines(leftLine);
            var lowerRightPoint = lowerLine.IntersectionLines(rightLine);

            var rect = new ProjectionRectangle4()
            {
                TopLeftPoint = upperLeftPoint,
                TopRightPoint = upperRightPoint,
                BottomLeftPoint = lowerLeftPoint,
                BottomRightPoint = lowerRightPoint,
            };

            return rect;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="mode">'border', 'border_holes', 'center'</param>
        /// <returns></returns>
        public static HXLDCont GenContourRegionXld(this HRegion region, string mode = "border")
        {
           return region.GenContourRegionXld(mode);
        }
    }
}