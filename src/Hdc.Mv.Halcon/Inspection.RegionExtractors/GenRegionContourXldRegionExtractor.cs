using System;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("XldExtractor")]
    public class GenRegionContourXldRegionExtractor : RegionExtractorBase
    {
        protected override HRegion ExtractInner(HImage image)
        {
            var xld = XldExtractor.Extract(image);

            var poly = xld as HXLDPoly;
            if (poly != null)
            {
                var region = poly.GenRegionPolygonXld(Mode);
                return region;
            }

            var cont = xld as HXLDCont;
            if (cont != null)
            {
                var region = cont.GenRegionContourXld(Mode);
                return region;
            }

            throw new InvalidOperationException("XldExtractor return neither HXLDPoly nor HXLDCont");
        }

        public string Mode { get; set; } = "filled";

        public IXldExtractor XldExtractor { get; set; }
    }
}