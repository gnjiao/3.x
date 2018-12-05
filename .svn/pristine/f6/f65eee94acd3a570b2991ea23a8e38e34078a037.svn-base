using System.Diagnostics;
using Core;
using HalconDotNet;
using Core.Diagnostics;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    public class RegionSearchingInspector: IRegionSearchingInspector
    {
        public RegionSearchingResult SearchRegion(HImage image, RegionSearchingDefinition definition)
        {
            var swSearchEdge = new NotifyStopwatch($"{nameof(SearchRegion)}: " + definition.Name);

            var result = new RegionSearchingResult()
            {
                Definition = definition.DeepClone(),
            };

            var rectImage = HDevelopExport.Singletone.ChangeDomainForRectangle(
                image,
                definition.RoiActualLine,
                definition.RoiHalfWidth);

            if (definition.SaveCacheImageEnabled)
            {
                rectImage.WriteImageOfTiffLzwOfCropDomain(
                    definition.SaveCacheImageFileName + "_1_Domain_Cropped.tif");
            }

            // RegionExtractor
            if (result.Definition.RegionExtractor != null)
            {
                HRegion roiDomain;

                var swRegionExtractor = new NotifyStopwatch($"{nameof(RegionSearchingInspector)}.RegionExtractor: " + definition.Name);
                roiDomain = result.Definition.RegionExtractor.Extract(rectImage);
                swRegionExtractor.Dispose();

                result.Region = roiDomain;
                rectImage.Dispose();
            }
            else
            {
                result.Region = new HRegion();
                result.Region.GenEmptyRegion();
            }

            swSearchEdge.Dispose();
            return result;
        }
    }
}