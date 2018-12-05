using System;
using HalconDotNet;
using System.ComponentModel;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class CircleRegionExtractor : RegionExtractorBase, IRegionExtractor
    {
        protected override HRegion ExtractInner(HImage image)
        {
            var rect = new HRegion();
            rect.GenCircle(Y, X, Radius);
            return rect;
        }
        [Description("Բ��X����")]
        public double X { get; set; }

        [Description("Բ��Y����")]
        public double Y { get; set; }

        [Description("Բ�İ뾶")]
        public double Radius { get; set; }
    }
}