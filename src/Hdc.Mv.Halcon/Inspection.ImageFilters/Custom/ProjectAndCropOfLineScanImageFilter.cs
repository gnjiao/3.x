using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionExtractor")]
    public class ProjectAndCropOfLineScanImageFilter : ImageFilterBase, IImageFilter
    {
        protected override HImage ProcessInner(HImage image)
        {
            var inputImage = image.CopyImage();
            var region = TransformSourceRegionExtractor.Extract(inputImage);

            var rect4 = region.GetProjectionRectangle4(BoundaryType, BoundaryMaxDistance);
            var oldPoints = rect4.GetPoints();
            var newRect4 = rect4.GetHorizontalVertiacalRectangle4();
            var newPoints = newRect4.GetPoints();

            var px = oldPoints.GetRowTuple();
            var py = oldPoints.GetColumnTuple();
            var qx = newPoints.GetRowTuple();
            var qy = newPoints.GetColumnTuple();

            HTuple homMat2DTuple, covariance;
            HOperatorSet.VectorToProjHomMat2d(px, py, qx, qy, "normalized_dlt", new HTuple(), new HTuple(), new HTuple(),
                new HTuple(), new HTuple(), new HTuple(),
                out homMat2DTuple, out covariance);

            var homMat2D = new HHomMat2D(homMat2DTuple);

            var transImage = image.ProjectiveTransImage(homMat2D, Interpolation.ToHalconString(), "false", "false");

            var cropped = transImage.CropPart(
                (int) (newRect4.CenterY - CropHeight/2 + VerticalOffset),
                (int) (newRect4.CenterX - CropWidth/2 + HorizontalOffset),
                CropWidth,
                CropHeight);

            inputImage.Dispose();
            transImage.Dispose();

            return cropped;
        }

        public IRegionExtractor TransformSourceRegionExtractor { get; set; }

        // TODO, could support Left or Right
//        public Direction OffsetDirection { get; set; }

        public int HorizontalOffset { get; set; }

        public int VerticalOffset { get; set; }

        public int CropWidth { get; set; }

        public int CropHeight { get; set; }

        public int BoundaryMaxDistance { get; set; } = 3;

        public BoundaryType BoundaryType { get; set; } = BoundaryType.Inner;

        public Interpolation Interpolation { get; set; } = Interpolation.Bilinear;
    }
}