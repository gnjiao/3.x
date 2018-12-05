using System;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("ThresholdImageFilter")]
    public class SelectDarkAndLightRegionUsingDynThresholdRegionExtractor : RegionExtractorBase
    {
        protected override HRegion ExtractInner(HImage image)
        {
            var preprocessImage = PreprocessFilter != null ? PreprocessFilter.Process(image) : image;

            var thresholdImage = ThresholdImageFilter.Process(SeparateFilter ? image : preprocessImage);

            HRegion lightRegion = preprocessImage.DynThreshold(
                thresholdImage,
                LightOffset,
                LightDark.Light.ToHalconString());

            HRegion darkRegion = preprocessImage.DynThreshold(
                thresholdImage,
                DarkOffset,
                LightDark.Dark.ToHalconString());

            //
            var processedLightRegion = LightRegionProcessor != null
                ? LightRegionProcessor.Process(lightRegion)
                : lightRegion;
            var processedLightRegionUnion = processedLightRegion.Union1();
            if (processedLightRegionUnion.CountObj() == 0 || processedLightRegionUnion.Area == 0)
                return processedLightRegionUnion;

            //
            var processedDarkRegion = DarkRegionProcessor != null
                ? DarkRegionProcessor.Process(darkRegion)
                : darkRegion;
            var processedDarkRegionUnion = processedDarkRegion.Union1();
            if (processedDarkRegionUnion.CountObj() == 0 || processedDarkRegionUnion.Area == 0)
                return processedDarkRegionUnion;


            //
            var concatRegion = processedLightRegionUnion.ConcatObj(processedDarkRegionUnion);

            var foundRegion = SelectIntersectionRegionProcessor.Process(concatRegion);

            lightRegion.Dispose();
            darkRegion.Dispose();
            processedLightRegion.Dispose();
            processedLightRegionUnion.Dispose();
            processedDarkRegion.Dispose();
            processedDarkRegionUnion.Dispose();
            concatRegion.Dispose();

            if (!Equals(preprocessImage, image))
                preprocessImage.Dispose();

            thresholdImage.Dispose();

            return foundRegion;
        }

        public string Name { get; set; }

        public IImageFilter PreprocessFilter { get; set; }
        public IImageFilter ThresholdImageFilter { get; set; }
        public bool SeparateFilter { get; set; }

        public double LightOffset { get; set; }
        public double DarkOffset { get; set; }

        public IRegionProcessor LightRegionProcessor { get; set; }
        public IRegionProcessor DarkRegionProcessor { get; set; }

        public IRegionProcessor SelectIntersectionRegionProcessor { get; set; }
    }
}