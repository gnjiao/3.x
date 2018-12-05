using System;
using System.Windows.Markup;
using HalconDotNet;
using Core.Diagnostics;
using Hdc.Mv.Halcon;
using System.ComponentModel;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("ThresholdImageFilter")]
    public class DynThresholdCroppedRegionExtractor : RegionExtractorBase, IRegionExtractor
    {
        protected override HRegion ExtractInner(HImage image)
        {
            var domain = image.GetDomain();
            var offsetRow1 = domain.GetRow1();
            var offsetColumn1 = domain.GetColumn1();
            var croppedImage = image.CropDomain();

            var swThresholdImageFilter = new NotifyStopwatch("DynThresholdCroppedRegionExtractor.ThresholdImageFilter");
            HImage thresholdImage = ThresholdImageFilter.Process(croppedImage);
            swThresholdImageFilter.Dispose();

            var swDynThreshold = new NotifyStopwatch("DynThresholdCroppedRegionExtractor.DynThreshold");
            HRegion region = croppedImage.DynThreshold(
                thresholdImage,
                Offset,
                LightDark.ToHalconString());
            swDynThreshold.Dispose();

            var movedRegion = region.MoveRegion(offsetRow1, offsetColumn1);

            croppedImage.Dispose();
            thresholdImage.Dispose();
            region.Dispose();

            return movedRegion;
        }

        public string Name { get; set; }

        public IImageFilter ThresholdImageFilter { get; set; }

        [Description("偏移量, 建议值: 1.0, 3.0, 5.0, 7.0, 10.0, 20.0, 30.0")]
        public double Offset { get; set; }

        [Description("提取前景或者背景,可选值：'Light', 'Dark', 'Equal', 'NotEqual'")]
        public LightDark LightDark { get; set; }
    }
}