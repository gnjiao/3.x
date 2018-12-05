using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core;
using HalconDotNet;
using Core.Diagnostics;
using Hdc.Mv.Halcon;
using Core.Reflection;

namespace Hdc.Mv.Inspection
{
    public class DefectInspector : IDefectInspector
    {
        private string _cacheImageDir = typeof(Mv.Ex).Assembly.GetAssemblyDirectoryPath() + "\\CacheImages";

        public RegionDefectResult SearchDefects(HImage image, DefectDefinition definition)
        {
            var swSearchRegionTarget = new NotifyStopwatch("SearchRegionTarget: " + definition.Name);
            var result = new RegionDefectResult();


            var roiImage = HDevelopExport.Singletone.ChangeDomainForRectangle(
                image,
                definition.RoiActualLine,
                definition.RoiHalfWidth);

            if (definition.Domain_SaveCacheImageEnabled)
            {
                roiImage.WriteImageOfTiffLzwOfCropDomain(
                    _cacheImageDir + "\\SearchRegionTarget_" + definition.Name + "_1_Domain_Cropped.tif");
            }



            // Target

            HImage targetFilterImage;
            if (definition.DefectImageFilter != null)
            {
                var swTargetImageFilter = new NotifyStopwatch("RegionTargetInspector.TargetImageFilter: " + definition.Name);
                targetFilterImage = definition.DefectImageFilter.Process(image);
                swTargetImageFilter.Dispose();
            }
            else
            {
                targetFilterImage = image;
            }



            HRegion targetRegion;
            if (definition.DefectRegionExtractor != null)
            {
                var swTargetRegionExtractor = new NotifyStopwatch("RegionTargetInspector.TargetRegionExtractor: " + definition.Name);
                targetRegion = definition.DefectRegionExtractor.Extract(targetFilterImage);
                swTargetRegionExtractor.Dispose();
            }
            else
            {
                targetRegion = targetFilterImage.GetDomain();
            }

            HRegion targetProcessedRegion;
            if (definition.DefectRegionProcessor != null)
            {
                var swTargetRegionProcessor = new NotifyStopwatch("RegionTargetInspector.TargetRegionProcessor: " + definition.Name);
                targetProcessedRegion = definition.DefectRegionProcessor.Process(targetRegion);
                swTargetRegionProcessor.Dispose();
            }
            else
            {
                targetProcessedRegion = targetRegion;
            }

            targetFilterImage.Dispose();
            roiImage.Dispose();

            result.DefectRegion = targetProcessedRegion;
            if (targetProcessedRegion.IsNull())
            {
                result.DefectArea = 0;
                //result.HasError = false;
            }
            else
            {
                double w1, h1;
                result.DefectArea = result.DefectRegion.AreaCenter(out w1, out h1);
                //  result.HasError = true;
            }
            result.Location = definition.Location;
            swSearchRegionTarget.Dispose();
            return result;
        }
    }
}