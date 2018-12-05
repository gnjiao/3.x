using System;
using System.Collections.Generic;
using System.Windows;
using HalconDotNet;
using Hdc.Mv;
using Hdc.Mv.Halcon;
using Core.Reflection;

public partial class HDevelopExport
{
    private HDevEngine MyEngine = new HDevEngine();

    public static HDevelopExport Singletone { get; set; }

    static HDevelopExport()
    {
        Singletone = new HDevelopExport();
    }

    public HDevelopExport()
    {
        string ProcedurePath = this.GetType().Assembly.GetAssemblyDirectoryPath();
        MyEngine.SetProcedurePath(ProcedurePath);
    }

    // Local procedures 
    public void RakeEdgeLine(HImage ho_Image, HTuple hv_Elements, HTuple hv_DetectHeight,
                             HTuple hv_DetectWidth, HTuple hv_Sigma, HTuple hv_Threshold,
                             HTuple hv_Transition,
                             HTuple hv_Select, HTuple hv_Row1, HTuple hv_Column1, HTuple hv_Row2,
                             HTuple hv_Column2,
                             out HTuple hv_BeginRow, out HTuple hv_BeginCol, out HTuple hv_EndRow,
                             out HTuple hv_EndCol)
    {
        // rake

        HObject ho_Regions;
        HOperatorSet.GenEmptyObj(out ho_Regions);
        ho_Regions.Dispose();

        rake(ho_Image, out ho_Regions, hv_Elements, hv_DetectHeight, hv_DetectWidth,
            hv_Sigma, hv_Threshold, hv_Transition, hv_Select, hv_Row1, hv_Column1, hv_Row2,
            hv_Column2, out hv_BeginRow, out hv_BeginCol);

        ho_Regions.Dispose();

        // pts_to_best_line

        HObject ho_Line;
        HOperatorSet.GenEmptyObj(out ho_Line);
        ho_Line.Dispose();

        var tupleCount = hv_BeginRow.Length;

        pts_to_best_line(out ho_Line, hv_BeginRow, hv_BeginCol, tupleCount, out hv_BeginRow, out hv_BeginCol,
            out hv_EndRow, out hv_EndCol);

        ho_Line.Dispose();
    }


    public void SpokeCircle(HObject ho_Image, HTuple hv_ROICirCentre_Row, HTuple hv_ROICirCentre_Column,
                            HTuple hv_ROIBigCirRadius, HTuple hv_ROISmallCirRadius,
                            HTuple hv_Elements,
//        HTuple hv_DetectHeight , 
                            HTuple hv_DetectWidth,
                            HTuple hv_Sigma, HTuple hv_Threshold, HTuple hv_Transition, HTuple hv_Select,
                            HTuple hv_Direct, out HTuple hv_CirCentre_Row, out HTuple hv_CirCentre_Column,
                            out HTuple hv_CirRadius, out HTuple hv_CirRoundness, string ellipseMode, string ellipseAlgorithm)
    {
        // Local iconic variables 

        HObject ho_Regions, ho_Circle, ho_Cross, ho_Region;


        // Local control variables 

        HTuple
            hv_DetectHeight = null,
            hv_ROIRadius = null;
        HTuple
//            hv_Elements = null, 
            hv_ROIRows = null,
            hv_ROICols = null;
        HTuple hv_ResultRow = null, hv_ResultColumn = null, hv_ArcType = null;
        HTuple hv_Distance = null, hv_Sigma1 = null, hv_Sides = null;

        // Initialize local and output iconic variables 
        HOperatorSet.GenEmptyObj(out ho_Regions);
        HOperatorSet.GenEmptyObj(out ho_Circle);
        HOperatorSet.GenEmptyObj(out ho_Cross);
        HOperatorSet.GenEmptyObj(out ho_Region);

        hv_DetectHeight = hv_ROIBigCirRadius - hv_ROISmallCirRadius;
        hv_ROIRadius = hv_ROISmallCirRadius + (hv_DetectHeight/2);
//        hv_Elements = ((2*3.1415926)*hv_ROIRadius)/hv_DetectWidth;
        hv_ROIRows = new HTuple();
        hv_ROICols = new HTuple();
        if (hv_ROIRows == null)
            hv_ROIRows = new HTuple();
        hv_ROIRows[0] = hv_ROICirCentre_Row;
        if (hv_ROICols == null)
            hv_ROICols = new HTuple();
        hv_ROICols[0] = hv_ROICirCentre_Column + hv_ROIRadius;
        if (hv_ROIRows == null)
            hv_ROIRows = new HTuple();
        hv_ROIRows[1] = hv_ROICirCentre_Row + hv_ROIRadius;
        if (hv_ROICols == null)
            hv_ROICols = new HTuple();
        hv_ROICols[1] = hv_ROICirCentre_Column;
        if (hv_ROIRows == null)
            hv_ROIRows = new HTuple();
        hv_ROIRows[2] = hv_ROICirCentre_Row;
        if (hv_ROICols == null)
            hv_ROICols = new HTuple();
        hv_ROICols[2] = hv_ROICirCentre_Column - hv_ROIRadius;
        if (hv_ROIRows == null)
            hv_ROIRows = new HTuple();
        hv_ROIRows[3] = hv_ROICirCentre_Row - hv_ROIRadius;
        if (hv_ROICols == null)
            hv_ROICols = new HTuple();
        hv_ROICols[3] = hv_ROICirCentre_Column;
        if (hv_ROIRows == null)
            hv_ROIRows = new HTuple();
        hv_ROIRows[4] = hv_ROICirCentre_Row;
        if (hv_ROICols == null)
            hv_ROICols = new HTuple();
        hv_ROICols[4] = hv_ROICirCentre_Column + hv_ROIRadius;
        ho_Regions.Dispose();
        
        spoke(ho_Image, out ho_Regions, hv_Elements, hv_DetectHeight, hv_DetectWidth,
            hv_Sigma, hv_Threshold, hv_Transition, hv_Select, hv_ROIRows, hv_ROICols,
            hv_Direct, out hv_ResultRow, out hv_ResultColumn, out hv_ArcType);
        ho_Circle.Dispose();

        var tupleCount = hv_ResultRow.Length;

        switch (ellipseMode)
        {
            case "Circle":
                pts_to_best_circle(out ho_Circle, hv_ResultRow, hv_ResultColumn, tupleCount, "circle",
                    out hv_CirCentre_Row, out hv_CirCentre_Column, out hv_CirRadius);
                break;
            case "DiameterMin":
                pts_to_best_circle_using_ellipse(out ho_Circle, hv_ResultRow, hv_ResultColumn, tupleCount, "circle",
                    out hv_CirCentre_Row, out hv_CirCentre_Column, out hv_CirRadius, radiusMinOrMax: false, algorithm: ellipseAlgorithm);
                break;
            case "DiameterMax":
                pts_to_best_circle_using_ellipse(out ho_Circle, hv_ResultRow, hv_ResultColumn, tupleCount, "circle",
                    out hv_CirCentre_Row, out hv_CirCentre_Column, out hv_CirRadius, radiusMinOrMax: true, algorithm: ellipseAlgorithm);
                break;
            default:
                pts_to_best_circle(out ho_Circle, hv_ResultRow, hv_ResultColumn, tupleCount, "circle",
                    out hv_CirCentre_Row, out hv_CirCentre_Column, out hv_CirRadius);
                break;
        }

        ho_Cross.Dispose();
        HOperatorSet.GenCrossContourXld(out ho_Cross, hv_ResultRow, hv_ResultColumn,
            12, 0.785398);
        ho_Region.Dispose();
        HOperatorSet.GenRegionContourXld(ho_Circle, out ho_Region, "filled");
        HOperatorSet.Roundness(ho_Region, out hv_Distance, out hv_Sigma1, out hv_CirRoundness,
            out hv_Sides);
        ho_Regions.Dispose();
        ho_Circle.Dispose();
        ho_Cross.Dispose();
        ho_Region.Dispose();

        return;
    }

    public IList<Line> RakeEdgeLine(HImage hImage, Line line,
                                    int regionsCount, int regionHeight, int regionWidth,
                                    double sigma, double threshold, Transition transition,
                                    SelectionMode selectionMode)
    {
        return RakeEdgeLine(hImage, line.X1, line.Y1, line.X2, line.Y2, regionsCount, regionHeight, regionWidth,
            sigma, threshold, transition, selectionMode);
    }

    public IList<Line> RakeEdgeLine(HImage hImage, double startX, double startY, double endX, double endY,
                                    int regionsCount, int regionHeight, int regionWidth,
                                    double sigma, double threshold, Transition transition,
                                    SelectionMode selectionMode)
    {
        // Local iconic variables 

        HObject ho_Image, ho_Regions;

        // Local control variables 

        HTuple hv_Row1 = null, hv_Column1 = null, hv_Row2 = null;
        HTuple hv_Column2 = null, hv_BeginRow = null, hv_BeginCol = null;
        HTuple hv_EndRow = null, hv_EndCol = null;

        // Initialize local and output iconic variables 

        HOperatorSet.GenEmptyObj(out ho_Image);
        HOperatorSet.GenEmptyObj(out ho_Regions);

        try
        {
            //                ho_Image.Dispose();
            //                HOperatorSet.ReadImage(out ho_Image, @"B:\ConsoleApplication1\Untitled2.tif");
            ho_Regions.Dispose();

            hv_Row1 = new HTuple(startY);
            hv_Column1 = new HTuple(startX);
            hv_Row2 = new HTuple(endY);
            hv_Column2 = new HTuple(endX);

            RakeEdgeLine(hImage, regionsCount, regionHeight, regionWidth,
                sigma, threshold, transition.ToHalconString(), selectionMode.ToHalconString(),
                hv_Row1, hv_Column1, hv_Row2, hv_Column2,
                out hv_BeginRow, out hv_BeginCol, out hv_EndRow, out hv_EndCol);

            double[] BeginRow = hv_BeginRow;
            double[] BeginColumn = hv_BeginCol;
            double[] EndRow = hv_EndRow;
            double[] EndColumn = hv_EndCol;

            IList<Line> lines = new List<Line>();

            for (int i = 0; i < BeginRow.Length; i++)
            {
                lines.Add(new Line(BeginColumn[i], BeginRow[i], EndColumn[i], EndRow[i]));
            }

            return lines;
        }
        catch (HalconException HDevExpDefaultException)
        {
            ho_Image.Dispose();
            ho_Regions.Dispose();

            throw HDevExpDefaultException;
        }
//        ho_Image.Dispose();
//        ho_Regions.Dispose();
    }

    public bool ExtractCircle(HImage hImage, double centerX, double centerY, double innerCircleRadius,
                              double outerCircleRadius,
                              out Circle foundCircle, out double roundness,
                              int regionsCount,
                              //                                  int regionHeight,
                              int regionWidth,
                              double sigma, double threshold, SelectionMode selectionMode, Transition transition,
                              CircleDirect direct, string EllipseMode, string ellipseAlgorithm)
    {
        try
        {
            HTuple centerX2, centerY2, radius, roundness2;
            SpokeCircle(hImage, centerX, centerY, outerCircleRadius, innerCircleRadius,
                regionsCount, regionWidth,
                sigma, threshold,
                transition.ToHalconString(),
                selectionMode.ToHalconString(),
                direct.ToHalconString(),
                out centerX2, out centerY2, out radius, out roundness2, EllipseMode, ellipseAlgorithm);

            foundCircle = new Circle(centerX2, centerY2, radius);
            //roundness = roundness2;
            roundness = 0;
            return true;
        }
        catch (HOperatorException )
        {
            foundCircle = new Circle();
            roundness = 0;
            return false;
        }
    }

    public HImage ChangeDomainForRectangle(HImage image, Line line, double halfWidth,
                                           double dilationWidth, double dilationHeight)
    {
        return ChangeDomainForRectangle(image, line.Y1, line.X1, line.Y2, line.X2, halfWidth,
            dilationWidth, dilationHeight);
    }

    public HImage ChangeDomainForRectangle(HImage image, Line line, double halfWidth)
    {
        return ChangeDomainForRectangle(image, line.Y1, line.X1, line.Y2, line.X2, halfWidth);
    }

    public HImage ChangeDomainForRectangle(HImage image, Line line, double halfWidth,
                                           double margin)
    {
        return ChangeDomainForRectangle(image, line, halfWidth, margin, margin);
    }

    public HImage ChangeDomainForRectangle(HImage ho_InputImage,
                                           double hv_LineStartPoint_Row, double hv_LineStartPoint_Column,
                                           double hv_LineEndPoint_Row,
                                           double hv_LineEndPoint_Column, double hv_RoiWidthLen,
                                           double hv_DilationWidth, double hv_DilationHeight)
    {
        HObject ho_EnhancedImage = null;
        ChangeDomainForRectangle(
            ho_InputImage, out ho_EnhancedImage,
            hv_LineStartPoint_Row, hv_LineStartPoint_Column,
            hv_LineEndPoint_Row,
            hv_LineEndPoint_Column, hv_RoiWidthLen,
            hv_DilationWidth, hv_DilationHeight
            );
        return new HImage(ho_EnhancedImage);
    }

    public HImage ChangeDomainForRectangle(HImage ho_InputImage,
                                           double hv_LineStartPoint_Row, double hv_LineStartPoint_Column,
                                           double hv_LineEndPoint_Row,
                                           double hv_LineEndPoint_Column, double hv_RoiWidthLen)
    {
        HObject ho_EnhancedImage = null;
        ChangeDomainForRectangle(
            ho_InputImage, out ho_EnhancedImage,
            hv_LineStartPoint_Row, hv_LineStartPoint_Column,
            hv_LineEndPoint_Row,
            hv_LineEndPoint_Column, hv_RoiWidthLen
            );
        return new HImage(ho_EnhancedImage);
    }

    public void DistanceOfLineToLine(Line line1, Line line2, out Line distanceLine, out Point root, out double angle)
    {
        HTuple distance, distanceBeginX, distanceBeginY, distanceEndX, distanceEndY, rootX, rootY, hAngle;

        HDevelopExport.Singletone.DistanceOfLineToLine(
            line1.Y1, line1.X1, line1.Y2, line1.X2,
            line2.Y1, line2.X1, line2.Y2, line2.X2,
            out distance, out distanceBeginY, out distanceBeginX, out distanceEndY, out distanceEndX, out rootY,
            out rootX, out hAngle);

        distanceLine = new Line(distanceBeginX, distanceBeginY, distanceEndX, distanceEndY);
        root = new Point(rootX, rootY);
        angle = hAngle.ToDArr()[0];
    }




    public static bool GetCalibrationParameters(string descriptionFileName, string dirName, double focus, double sx,
                                                double sy,
                                                double width, double height, string cameraType,
                                                out HTuple interCamera, out HTuple PoseNewOrigin,
                                                out double distance)
    {
        try
        {
            HTuple hv_interCamera, hv_PoseNewOrigin, hv_distance;
            HDevelopExport.Singletone.GetCalibrationParameters(descriptionFileName, dirName, focus, sx, sy,
                width, height, cameraType, out hv_interCamera, out hv_PoseNewOrigin, out hv_distance);

            interCamera = hv_interCamera;
            PoseNewOrigin = hv_PoseNewOrigin;
            distance = hv_distance;

            return true;
        }
        catch (HOperatorException )
        {
            interCamera = null;
            PoseNewOrigin = null;
            distance = 0;
            return false;
        }
    }

    public static bool GetCalibrationParameters(ImageInfo image, HTuple interCamera, HTuple PoseNewOrigin,
                                                double distance, out HImage calibImage)
    {
        try
        {
            var orignalImage = image.To8BppHImage();
            //                ImageInfo calibImage;

            HObject hCalibImage;
            HDevelopExport.Singletone.GetCalibratedImage(orignalImage, out hCalibImage, interCamera, PoseNewOrigin, distance);

            var hi = new HImage();
            hCalibImage.HobjectToHimage(ref hi);
            calibImage = hi;
            return true;
        }
        catch (HOperatorException )
        {
            calibImage = null;
            return false;
        }
    }

    public HRegion GetRegionByGrayAndArea(HImage image,
                                           int medianRadius,
                                           int empWidth, int empHeight, double empFactor,
                                           int thresholdMinGray, int thresholdMaxGray,
                                           int areaMin, int areaMax,
                                           double closingRadius, double dilationRadius)
    {
        HObject foundRegionObject;

        HDevelopExport.Singletone.GetRegionByGrayAndArea(image, out foundRegionObject, medianRadius,
            empWidth, empHeight, empFactor, thresholdMinGray, thresholdMaxGray, areaMin,
            areaMax,
            closingRadius, dilationRadius);

        return new HRegion(foundRegionObject);
    }



}