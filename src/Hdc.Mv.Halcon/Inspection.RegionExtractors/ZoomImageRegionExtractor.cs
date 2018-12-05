using System;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Mv.Halcon;
using System.ComponentModel;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionExtractor")]
    public class ZoomImageRegionExtractor : RegionExtractorBase, IRegionExtractor
    {
        protected override HRegion ExtractInner(HImage image)
        {
            HImage zoomedImage = image.ZoomImageFactor(ScaleWidth, ScaleHeight, Interpolation.ToHalconString());
            HRegion zoomedRegion = RegionExtractor.Extract(zoomedImage);

            var oriRegion = zoomedRegion.ZoomRegion(1 / ScaleWidth, 1 / ScaleHeight);

            zoomedImage.Dispose();
            zoomedRegion.Dispose();

            return oriRegion;
        }

        [Description("图像宽度的比例因子,建议值：0.25, 0.5, 1.5, 2.0")]
        public double ScaleWidth { get; set; } = 0.5;

        [Description("图像高度的比例因子,建议值：0.25, 0.5, 1.5, 2.0")]
        public double ScaleHeight { get; set; } = 0.5;

        [Description("插值类型,可选值有： 'bicubic', 'bilinear', 'constant', 'nearest_neighbor', 'weighted'")]
        public Interpolation Interpolation { get; set; } = Interpolation.Constant;

        public IRegionExtractor RegionExtractor { get; set; }
    }
}