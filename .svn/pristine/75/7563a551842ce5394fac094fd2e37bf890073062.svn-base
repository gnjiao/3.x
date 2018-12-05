using System;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionExtractor")]
    public class ProjectionRectangle4HomMat2DExtractor : IHomMat2DAndRectExtractor
    {
        public IRegionExtractor RegionExtractor { get; set; }

        public void Extract(HImage image, out HHomMat2D homMat2D, out TopLeftRectangle rectangle)
        {
            var inputImage = image.CopyImage();
            var region = RegionExtractor.Extract(inputImage);

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

            homMat2D = new HHomMat2D(homMat2DTuple);

            

            if (CropWidth != 0 && CropHeight != 0)
            {
                var topLeftX = (int)(newRect4.CenterX - CropWidth / 2 + MarginLeft);
                var topLeftY = (int)(newRect4.CenterY - CropHeight / 2 + MarginTop);

                rectangle = new TopLeftRectangle(
                    topLeftX,
                    topLeftY,
                    CropWidth,
                    CropHeight);
            }
            else
            {
                var topLeftX = (int)(newRect4.TopLeftPoint.X + MarginLeft);
                var topLeftY = (int)(newRect4.TopLeftPoint.Y + MarginTop);

                var bottomRightX = (int)(newRect4.BottomRightPoint.X - MarginRight);
                var bottomRightY = (int)(newRect4.BottomRightPoint.Y - MarginBottom);

                var width = bottomRightX - topLeftX;
                var height = bottomRightY - topLeftY;

                rectangle = new TopLeftRectangle(
                    topLeftX,
                    topLeftY,
                    width,
                    height);
            }
        }

        public double MarginTop { get; set; }
        public double MarginBottom { get; set; }
        public double MarginLeft { get; set; }
        public double MarginRight { get; set; }

        public int CropWidth { get; set; }

        public int CropHeight { get; set; }

        public int BoundaryMaxDistance { get; set; } = 3;

        public BoundaryType BoundaryType { get; set; } = BoundaryType.Inner;

        public Interpolation Interpolation { get; set; } = Interpolation.Bilinear;
    }
}