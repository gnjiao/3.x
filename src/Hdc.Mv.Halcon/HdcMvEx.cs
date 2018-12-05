using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using HalconDotNet;
using Core.IO;
using Hdc.Mv.Halcon;
using Core.Reflection;

namespace Hdc.Mv
{
    public static class HdcMvEx
    {


//        public static HRegion GetRegion(this IRectangle2 rect)
//        {
//            var processRegion = new HRegion();
//            processRegion.GenRectangle2(rect.Y, rect.X, rect.Angle, rect.HalfWidth, rect.HalfHeight);
//            return processRegion;
//        }

        public static void SaveCacheImagesForRegion(this HImage image, HRegion domain, HRegion region, string fileName)
        {
            var dir = typeof(Ex).Assembly.GetAssemblyDirectoryPath();
            var cacheDir = Path.Combine(dir, "CacheImages");

            if (!Directory.Exists(cacheDir))
            {
                Directory.CreateDirectory(cacheDir);
            }

            var reducedImage = image.ReduceDomain(domain);
            var croppedImage = reducedImage.CropDomain();
            croppedImage.WriteImageOfTiff(cacheDir.CombilePath(fileName) + ".Ori.tif");

            var paintRegion = reducedImage.PaintRegion(region, 255.0, "margin");
            var croppedImage2 = paintRegion.CropDomain();
            croppedImage2.WriteImageOfTiff(cacheDir.CombilePath(fileName) + ".Paint.Margin.tif");

            var paintRegion2 = reducedImage.PaintRegion(region, 255.0, "fill");
            var croppedImage2b = paintRegion2.CropDomain();
            croppedImage2b.WriteImageOfTiff(cacheDir.CombilePath(fileName) + ".Paint.Fill.tif");

            var reducedImage3 = image.ReduceDomain(region);
            var croppedImage3 = reducedImage3.CropDomain();
            croppedImage3.WriteImageOfTiff(cacheDir.CombilePath(fileName) + ".Crop.tif");

            domain.Dispose();
            reducedImage.Dispose();
            reducedImage3.Dispose();
            croppedImage.Dispose();
            croppedImage2.Dispose();
            croppedImage2b.Dispose();
            croppedImage3.Dispose();
            paintRegion.Dispose();
        }

        public static void SaveCacheImagesForRegion(this HImage image, HRegion domain, HRegion includeRegion,
                                                    HRegion excludeRegion, string fileName)
        {
            var dir = typeof(Ex).Assembly.GetAssemblyDirectoryPath();
            var cacheDir = Path.Combine(dir, "CacheImages");

            if (!Directory.Exists(cacheDir))
            {
                Directory.CreateDirectory(cacheDir);
            }

            var imageWidth = image.GetWidth();
            var imageHeight = image.GetHeight();

            // Domain.Ori
            var reducedImage = image.ChangeDomain(domain);
            var croppedImage = reducedImage.CropDomain();
            croppedImage.WriteImageOfTiff(cacheDir.CombilePath(fileName) + ".Domain.Ori.tif");
            reducedImage.Dispose();
            croppedImage.Dispose();

            // Domain.PaintMargin
            var reducedImage4 = image.ChangeDomain(domain);
            var paintRegionImage = reducedImage4.PaintRegion(includeRegion, 250.0, "margin");
            var paintRegion2Image = paintRegionImage.PaintRegion(excludeRegion, 5.0, "margin");
            var croppedImage2 = paintRegion2Image.CropDomain();
            croppedImage2.WriteImageOfTiff(cacheDir.CombilePath(fileName) + ".Domain.PaintMargin.tif");
            reducedImage4.Dispose();
            croppedImage2.Dispose();
            paintRegionImage.Dispose();
            paintRegion2Image.Dispose();

            // PaintFill
            //            var paintRegion3Image = reducedImage.PaintRegion(includeRegion, 250.0, "fill");
            //            var croppedImage2bImage = paintRegion3Image.CropDomain();
            //            croppedImage2bImage.ToBitmapSource().SaveToTiff(cacheDir.CombilePath(fileName) + ".Domain.PaintFill.tif");
            //            croppedImage2bImage.Dispose();

            // Domain.Crop
            var reducedImage3 = image.ChangeDomain(includeRegion);
            var croppedImage3 = reducedImage3.CropDomain();
            croppedImage3.WriteImageOfTiff(cacheDir.CombilePath(fileName) + ".Domain.Crop.tif");
            reducedImage3.Dispose();
            croppedImage3.Dispose();

            // bin image in domain
            var row1 = domain.GetRow1();
            var column1 = domain.GetColumn1();
            var movedRegion = includeRegion.MoveRegion(-row1, -column1);

            var w = domain.GetWidth();
            var h = domain.GetHeight();
            var binImage = movedRegion.RegionToBin(255, 0, w, h);
            binImage.WriteImageOfTiff(cacheDir.CombilePath(fileName) + ".Domain.Bin.tif");
            binImage.Dispose();
            movedRegion.Dispose();

            // Full.Bin, 
            var binImage2 = includeRegion.RegionToBin(255, 0, imageWidth, imageHeight);
            binImage2.WriteImageOfJpeg(cacheDir.CombilePath(fileName) + ".Full.Bin.jpg");
            binImage2.Dispose();

            // Full.BinOnlyDomain
            var binImage3 = includeRegion.RegionToBin(255, 0, imageWidth, imageHeight);
            var reducedImage5 = binImage3.ReduceDomain(domain);
            var binOnlyDomainImage = image.Clone();
            binOnlyDomainImage.OverpaintGray(reducedImage5);
            binOnlyDomainImage.WriteImageOfJpeg(cacheDir.CombilePath(fileName) + ".Full.BinOnlyDomain.jpg");

            binImage3.Dispose();
            reducedImage5.Dispose();
            binOnlyDomainImage.Dispose();
        }

        public static string ToHalconString(this LightDark lightDark)
        {
            switch (lightDark)
            {
                case LightDark.Dark:
                    return "dark";
                case LightDark.Light:
                    return "light";
                case LightDark.Equal:
                    return "equal";
                case LightDark.NotEqual:
                    return "not_equal";
                default:
                    throw new InvalidOperationException($"{nameof(LightDark)} cannot convert to string");
            }
        }

        public static string ToHalconString(this SortMode sortMode)
        {
            switch (sortMode)
            {
                case SortMode.Character:
                    return "character";
                case SortMode.FirstPoint:
                    return "first_point";
                case SortMode.LastPoint:
                    return "last_point";
                case SortMode.LowerLeft:
                    return "lower_left";
                case SortMode.LowerRight:
                    return "lower_right";
                case SortMode.UpperLeft:
                    return "upper_left";
                case SortMode.UpperRight:
                    return "upper_right";
                default:
                    throw new InvalidOperationException($"{nameof(SortMode)} cannot convert to string");
            }
        }

        public static string ToHalconString(this RowOrCol rowOrCol)
        {
            switch (rowOrCol)
            {
                case RowOrCol.Row:
                    return "row";
                case RowOrCol.Column:
                    return "column";
                default:
                    throw new InvalidOperationException($"{nameof(RowOrCol)} cannot convert to string");
            }
        }


        public static string ToHalconString(this MedianMaskType maskType)
        {
            switch (maskType)
            {
                case MedianMaskType.Circle:
                    return "circle";
                case MedianMaskType.Square:
                    return "square";
                default:
                    throw new InvalidOperationException($"{nameof(MedianMaskType)} cannot convert to string: " + maskType);
            }
        }


        public static string ToHalconString(this MedianMargin margin)
        {
            switch (margin)
            {
                case MedianMargin.Mirrored:
                    return "mirrored";
                case MedianMargin.Cyclic:
                    return "cyclic";
                case MedianMargin.Continued:
                    return "continued";
                default:
                    throw new InvalidOperationException($"{nameof(MedianMargin)} cannot convert to string: " + margin);
            }
        }

        public static string ToHalconString(this MaskShape maskShape)
        {
            switch (maskShape)
            {
                case MaskShape.Octagon:
                    return "octagon";
                case MaskShape.Rectangle:
                    return "rectangle";
                case MaskShape.Rhombus:
                    return "rhombus";
                default:
                    throw new InvalidOperationException($"{nameof(MaskShape)} cannot convert to string: " + maskShape);
            }
        }


        public static string ToHalconString(this SobelAmpFilterType filterType)
        {
            switch (filterType)
            {
                case SobelAmpFilterType.SumAbs:
                    return "sum_abs";
                case SobelAmpFilterType.SumAbsBinomial:
                    return "sum_abs_binomial";
                case SobelAmpFilterType.SumSqrt:
                    return "sum_sqrt";
                case SobelAmpFilterType.SumSqrtBinomial:
                    return "sum_sqrt_binomial";
                case SobelAmpFilterType.ThinMaxAbs:
                    return "thin_max_abs";
                case SobelAmpFilterType.ThinMaxAbsBinomial:
                    return "thin_max_abs_binomial";
                case SobelAmpFilterType.X:
                    return "x";
                case SobelAmpFilterType.XBinomial:
                    return "x_binomial";
                case SobelAmpFilterType.Y:
                    return "y";
                case SobelAmpFilterType.YBinomial:
                    return "y_binomial";
                default:
                    throw new InvalidOperationException($"{nameof(SobelAmpFilterType)} cannot convert to string: " + filterType);
            }
        }

        public static string ToHalconString(this Interpolation interpolation)
        {
            switch (interpolation)
            {
                case Interpolation.Bilinear:
                    return "bilinear";
                case Interpolation.NearestNeighbor:
                    return "nearest_neighbor";
                case Interpolation.Constant:
                    return "constant";
                case Interpolation.Weighted:
                    return "weighted";
                case Interpolation.Bicubic:
                    return "bicubic";
                default:
                    throw new InvalidOperationException($"{nameof(Interpolation)} cannot convert to string: " + interpolation);
            }
        }

        public static string ToHalconString(this BoundaryType boundaryType)
        {
            switch (boundaryType)
            {
                case BoundaryType.Inner:
                    return "inner";
                case BoundaryType.InnerFilled:
                    return "inner_filled";
                case BoundaryType.Outer:
                    return "outer";
                default:
                    throw new InvalidOperationException($"{nameof(BoundaryType)} cannot convert to string: " + boundaryType);
            }
        }

        public static string ToHalconString(this ImageType imageType)
        {
            switch (imageType)
            {
                case ImageType.Byte:
                    return "byte";
                case ImageType.Complex:
                    return "complex";
                case ImageType.Cyclic:
                    return "cyclic";
                case ImageType.Direction:
                    return "direction";
                case ImageType.Int1:
                    return "int1";
                case ImageType.Int2:
                    return "int2";
                case ImageType.Int4:
                    return "int4";
                case ImageType.Int8:
                    return "int8";
                case ImageType.Real:
                    return "real";
                case ImageType.UInt2:
                    return "uint2";
                default:
                    throw new InvalidOperationException($"{nameof(ImageType)} cannot convert to string: " + imageType);
            }
        }



        public static Line GetMiddleLineUsingAngle(this Line line1, Line line2)
        {
            var vectorAxisX = new Vector(10000, 0);

            var vector1A = new Vector(line1.X1, line1.Y1);
            var vector1B = new Vector(line1.X2, line1.Y2);
            var vector2A = new Vector(line2.X1, line2.Y1);
            var vector2B = new Vector(line2.X2, line2.Y2);

            var vector1BToA = vector1B - vector1A;
            var angle1 = Vector.AngleBetween(vector1BToA, vectorAxisX);
            var vector2BToA = vector2B - vector2A;
            var angle2 = Vector.AngleBetween(vector2BToA, vectorAxisX);

            var angleAvg = (angle1 + angle2) / 2.0;

            var matrix1 = new Matrix();
            matrix1.Rotate(angle1 - angleAvg + 90);
            var vertical1BToA = matrix1.Transform(vector1BToA);


            var matrix2 = new Matrix();
            matrix2.Rotate(angle2 - angleAvg + 90);
            var vertical2BToA = matrix2.Transform(vector2BToA);

            var v1C = vertical1BToA + vector1A;
            var v2C = vertical2BToA + vector2A;
            var v2CB = vertical2BToA + vector2B;


            HTuple row, column, isOverlapping;
            HOperatorSet.IntersectionLines(vector1A.Y, vector1A.X, vector1B.Y, vector1B.X,
                vector2A.Y, vector2A.X, v2C.Y, v2C.X, out row, out column, out isOverlapping);

            HTuple rowB, columnB, isOverlappingB;
            HOperatorSet.IntersectionLines(vector1A.Y, vector1A.X, vector1B.Y, vector1B.X,
                vector2B.Y, vector2B.X, v2CB.Y, v2CB.X, out rowB, out columnB, out isOverlappingB);

            HTuple row2, column2, isOverlapping2;
            HOperatorSet.IntersectionLines(vector2A.Y, vector2A.X, vector2B.Y, vector2B.X,
                vector1A.Y, vector1A.X, v1C.Y, v1C.X, out row2, out column2, out isOverlapping2);

            if (columnB.Length == 0) columnB = 0;
            if (rowB.Length == 0) rowB = 0;
            if (column2.Length == 0) column2 = 0;
            if (row2.Length == 0) row2 = 0;


             var middle1X = (vector2B.X + columnB) / 2.0;
            var middle1Y = (vector2B.Y + rowB) / 2.0;

            var middle2X = (vector1A.X + column2) / 2.0;
            var middle2Y = (vector1A.Y + row2) / 2.0;

            return new Line(middle2X, middle2Y, middle1X, middle1Y);
        }

        public static HRegion GenRegion(this IRectangle2 rectangle2)
        {
            var region = new HRegion();
            region.GenRectangle2(
                rectangle2.Y,
                rectangle2.X,
                rectangle2.Angle / 180.0 * Math.PI,
                rectangle2.HalfWidth,
                rectangle2.HalfHeight);
            return region;
        }

        public static string ToHalconString(this RegionFillMode regionFillMode)
        {
            switch (regionFillMode)
            {
                case RegionFillMode.Fill:
                    return "fill";
                case RegionFillMode.Margin:
                    return "margin";
                default:
                    throw new InvalidOperationException("regionFillMode cannot convert to string: " + regionFillMode);
            }
        }

        public static double ToArcDegree(this double angleDegree)
        {
            return angleDegree/180.0*Math.PI;
        }

        public static double ToAngleDegree(this double arcDegree)
        {
            return arcDegree/Math.PI*180.0;
        }

        public static Point IntersectionLines(this Line line1,Line line2)
        {
            HTuple row, column, isOverLapping;
            HOperatorSet.IntersectionLines(line1.Row1, line1.Column1, line1.Row2, line1.Column2,
                line2.Row1, line2.Column1, line2.Row2, line2.Column2,out row, out column, out isOverLapping);

            var point = new Point(column, row);
            return point;
        }

        public static double GetColumn(this Point point)
        {
            return point.X;
        }

        public static double GetRow(this Point point)
        {
            return point.Y;
        }

        public static HTuple GetRowTuple(this IEnumerable<Point> points)
        {
            var xs = points.Select(x => x.Y).ToArray();
            return new HTuple(xs);
        }

        public static HTuple GetColumnTuple(this IEnumerable<Point> points)
        {
            var xs = points.Select(x => x.X).ToArray();
            return new HTuple(xs);
        }

        public static HRegion ToRectangle1Region(this TopLeftRectangle topLeftRectangle)
        {
            var newDomain = new HRegion();
            newDomain.GenRectangle1((double)topLeftRectangle.Row1, topLeftRectangle.Column1,
                topLeftRectangle.Row2, topLeftRectangle.Column2);

            return newDomain;
        }

        public static Point IntersectionWith(this Line line1, Line line2)
        {
            double row, column;
            int isParallel;

            HMisc.IntersectionLl( line1.Y1, line1.X1, line1.Y2, line1.X2,
                line2.Y1, line2.X1, line2.Y2, line2.X2, out row, out column, out isParallel);

            if(isParallel==1)
                return new Point();

            return new Point(column, row);
        }
    }
}