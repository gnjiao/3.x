using System;
using HalconDotNet;
using Core.Serialization;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class ReferenceRegionExtractor : RegionExtractorBase
    {
        protected override HRegion ExtractInner(HImage image)
        {
            var extractor = FileName.DeserializeFromXamlFile<IRegionExtractor>();
            var region = extractor.Extract(image);
            return region;
        }

        public string FileName { get; set; }
    }
}