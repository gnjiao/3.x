using System;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("XldExtractor")]
    public class StructuredXldExtractor : XldExtractorBase, IXldExtractor
    {
        protected override HXLD ExtractInner(HImage image)
        {
            HImage targetFilterImage;
            if (ImageFilter != null) targetFilterImage = ImageFilter.Process(image);
            else targetFilterImage = image;

            HXLD xld;
            if (XldExtractor != null)
                xld = XldExtractor.Extract(targetFilterImage);
            else return new HXLD();

            HXLD targetProcessedRegion;
            if (XldProcessor != null)
                targetProcessedRegion = XldProcessor.Process(xld);
            else targetProcessedRegion = xld;

            if (ImageFilter != null)
                targetFilterImage.Dispose();

            if ((XldProcessor != null) && (xld != targetProcessedRegion))
                xld.Dispose();

            return targetProcessedRegion;
        }

        public IImageFilter ImageFilter { get; set; }
        public IXldExtractor XldExtractor { get; set; }
        public IXldProcessor XldProcessor { get; set; }
    }
}