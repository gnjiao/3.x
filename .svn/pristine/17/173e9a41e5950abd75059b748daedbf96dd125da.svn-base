using System;
using HalconDotNet;
using System.ComponentModel;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class ThresholdByGrayMeanRegionExtractor : IRegionExtractor
    {
        public HRegion Extract(HImage image)
        {
            var domain = image.GetDomain();
            if (domain.CountObj() == 0 || domain.Area == 0)
            {
                var emptyRegion = new HRegion();
                emptyRegion.GenEmptyRegion();
                domain.Dispose();
                return emptyRegion;
            }

            double deviation;
            var grayMean = domain.Intensity(image, out deviation);

            double minGray, maxGray;

            if (MinGrayOffset==null)
            {
                minGray = 0;
            }
            else
            {
                minGray = grayMean + MinGrayOffset.Value;
            }

            if (MaxGrayOffset==null)
            {
                maxGray = 255;
            }
            else
            {
                maxGray = grayMean + MaxGrayOffset.Value;
            }

            if (minGray < 0)
                minGray = 0;

            if (maxGray > 255)
                maxGray = 255;

            if(minGray >= maxGray)
                throw new Exception("ThresholdByGrayMeanRegionExtractor:: minGray >= maxGray");

            var region = image.Threshold(minGray, maxGray);
            return region;
        }
        [Description("最小灰度值的偏移量")]
        public double? MinGrayOffset { get; set; }

        [Description("最大灰度值的偏移量")]

        public double? MaxGrayOffset { get; set; }
    }
}