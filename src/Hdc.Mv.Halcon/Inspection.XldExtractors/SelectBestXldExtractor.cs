using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SelectBestXldExtractor : XldExtractorBase
    {
        protected override HXLD ExtractInner(HImage image)
        {
            var copyImage = image.CopyImage();
            var primaryXld = PrimaryXldExtractor.Extract(copyImage);
            var xld = CheckRuleXldProcessor.Process(primaryXld);
            var count = xld.CountObj();

            if (count > 0 || SecondaryXldExtractor == null)
            {
                copyImage.Dispose();
                return xld;
            }

            var copyImage2 = image.CopyImage();
            var secondaryXld = SecondaryXldExtractor.Extract(image);
            copyImage2.Dispose();

            return secondaryXld;
        }

        public IXldExtractor PrimaryXldExtractor { get; set; }

        public IXldExtractor SecondaryXldExtractor { get; set; }

        public IXldProcessor CheckRuleXldProcessor { get; set; }
    }
}