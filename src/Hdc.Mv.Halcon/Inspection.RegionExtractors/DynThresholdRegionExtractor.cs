using System;
using System.Windows.Markup;
using HalconDotNet;
using Core.Diagnostics;
using System.ComponentModel;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("ThresholdImageFilter")]
    public class DynThresholdRegionExtractor : RegionExtractorBase, IRegionExtractor
    {
        protected override HRegion ExtractInner(HImage image)
        {
            HImage preprocessImage;
            if (PreprocessFilter != null)
                preprocessImage = PreprocessFilter.Process(image);
            else
            {
                preprocessImage = image;
            }

            HImage thresholdImage;
            if (SeparateFilter)
            {
                //                var swThresholdImageFilter = new NotifyStopwatch("DynThresholdRegionExtractor.ThresholdImageFilter");
                thresholdImage = ThresholdImageFilter.Process(image);
                //                swThresholdImageFilter.Dispose();
            }
            else
            {
                //                var swThresholdImageFilter = new NotifyStopwatch("DynThresholdRegionExtractor.ThresholdImageFilter");
                thresholdImage = ThresholdImageFilter.Process(preprocessImage);
                //                swThresholdImageFilter.Dispose();
            }

            //            var swDynThreshold = new NotifyStopwatch("DynThresholdRegionExtractor.DynThreshold");
            HRegion region = preprocessImage.DynThreshold(
                thresholdImage,
                Offset,
                LightDark.ToHalconString());
            //            swDynThreshold.Dispose();

            preprocessImage.Dispose();
            thresholdImage.Dispose();

            return region;
        }

        public string Name { get; set; }

        public IImageFilter PreprocessFilter { get; set; }
        public IImageFilter ThresholdImageFilter { get; set; }

        [Description("ƫ����, ����ֵ: 1.0, 3.0, 5.0, 7.0, 10.0, 20.0, 30.0")]
        public double Offset { get; set; }

        [Description("��ȡǰ�����߱���,��ѡֵ��'Light', 'Dark', 'Equal', 'NotEqual'")]
        public LightDark LightDark { get; set; }
        public bool SeparateFilter { get; set; }
    }
}