using System;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionProcessor")]
    public class GetDomainRect1AndMarginRegionExtractor : RegionExtractorBase, IRegionExtractor
    {
        protected override HRegion ExtractInner(HImage image)
        {
            var region = image.GetDomain();

            var rect1= region.GetSmallestRectangle1();
            var marginRect1 = new HRegion();
            marginRect1.GenRectangle1(rect1.Row1 + Top, rect1.Column1 + Left, rect1.Row2 - Bottom, rect1.Column2 - Right);

            region.Dispose();
            return marginRect1;
        }

        public double Top { get; set; }
        public double Bottom { get; set; }
        public double Left { get; set; }
        public double Right { get; set; }
    }
}