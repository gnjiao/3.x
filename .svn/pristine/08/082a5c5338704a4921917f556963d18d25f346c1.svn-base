using System.Diagnostics;
using Core;
using HalconDotNet;
using Core.Diagnostics;
using Hdc.Mv.Halcon;
using Core.Reflection;

namespace Hdc.Mv.Inspection
{
    public class DataCodeSearchingInspector : IDataCodeSearchingInspector
    {
        private string _cacheImageDir = typeof(Mv.HdcMvEx).Assembly.GetAssemblyDirectoryPath() + "\\CacheImages";

        public DataCodeSearchingResult SearchDataCode(HImage image, DataCodeSearchingDefinition definition)
        {
            var swSearchEdge = new NotifyStopwatch("SearchDataCode: " + definition.Name);

            var result = new DataCodeSearchingResult()
            {
                Definition = definition.DeepClone(),
                Name = definition.Name,
            };

            var rectImage = HDevelopExport.Singletone.ChangeDomainForRectangle(
                image,
                definition.Line,
                definition.ROIHalfWidth);

            if (definition.SaveCacheImageEnabled)
            {
                rectImage.WriteImageOfTiffLzwOfCropDomain(
                    definition.SaveCacheImageFileName + "_1_Domain_Cropped.tif");
            }

            // RegionExtractor
            HImage roiImage = null;

            if (result.Definition.RegionExtractor != null)
            {
//                var rectDomain = rectImage.GetDomain();
                HRegion roiDomain;

                var swRegionExtractor = new NotifyStopwatch($"{nameof(DataCodeSearchingInspector)}.RegionExtractor: " + definition.Name);
                roiDomain = result.Definition.RegionExtractor.Extract(rectImage);
                swRegionExtractor.Dispose();

                roiImage = rectImage.ReduceDomain(roiDomain);
                rectImage.Dispose();
            }
            else
            {
                roiImage = rectImage;
            }

            // ImageFilter
            HImage filterImage = null;


            filterImage = roiImage;


            //            var swLineExtractor = new NotifyStopwatch("EdgeInspector.LineExtractor: " + definition.Name);
            var decodeString = definition.DataCodeExtractor.FindDataCode(filterImage);
            //            swLineExtractor.Dispose();

            if (string.IsNullOrEmpty(decodeString))
            {
                result.IsNotFound = true;
                Debug.WriteLine("DataCode not found: " + result.Name);
            }

            result.DecodeString = decodeString;

            swSearchEdge.Dispose();
            return result;
        }
    }
}