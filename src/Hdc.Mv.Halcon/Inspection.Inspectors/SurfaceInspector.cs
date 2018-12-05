using System.Collections.Generic;
using System.Linq;
using Core;
using HalconDotNet;
using Core.Diagnostics;

namespace Hdc.Mv.Inspection
{
    public class SurfaceInspector : ISurfaceInspector
    {
        public SurfaceResult SearchSurface(HImage image, SurfaceDefinition definition)
        {
            var swSearchSurface = new NotifyStopwatch("SearchSurface: " + definition.Name);

            if (definition.SaveAllCacheImageEnabled)
            {
                //                definition.SaveCacheImageEnabled = true;
                foreach (var part in definition.ExcludeParts)
                {
                    part.SaveCacheImageEnabled = true;
                }
                foreach (var part in definition.IncludeParts)
                {
                    part.SaveCacheImageEnabled = true;
                }
            }

            HHomMat2D homMat2D = null;
            TopLeftRectangle topLeftRectangle = null;

            HImage inputImage;
            if (definition.HomMat2DAndRectExtractor != null)
            {
                definition.HomMat2DAndRectExtractor.Extract(image, out homMat2D, out topLeftRectangle);
                var transImage = image.ProjectiveTransImage(homMat2D,
                    definition.HomMat2DAndRectExtractor.Interpolation.ToHalconString(), "false", "false");

//                image.WriteImage("tiff", 0, "b:\\test-01.tif");
//                transImage.WriteImage("tiff", 0, "b:\\test-01b.tif");

                var newDomain = topLeftRectangle.ToRectangle1Region();
                inputImage = transImage.ChangeDomain(newDomain);
                transImage.Dispose();
            }
            else
            {
                inputImage = image.CopyImage();
            }

            var surfaceResult = new SurfaceResult()
            {
                Definition = definition.DeepClone(),
                HomMat2D = homMat2D,
                TopLeftRectangle = topLeftRectangle,
                TransformedImage = inputImage,
            };

            var unionIncludeRegion = new HRegion();
            var unionIncludeDomain = new HRegion();
            unionIncludeRegion.GenEmptyRegion();
            unionIncludeDomain.GenEmptyRegion();

            var unionExcludeRegion = new HRegion();
            var unionExcludeDomain = new HRegion();
            unionExcludeRegion.GenEmptyRegion();
            unionExcludeDomain.GenEmptyRegion();

            foreach (var excludeRegion in definition.ExcludeParts)
            {
                //                var sw = new NotifyStopwatch("excludeRegion.Process: " + excludeRegion.Name);
                HRegion region = excludeRegion.Extract(inputImage);
                //                sw.Dispose();
                var domain = excludeRegion.GetOrInitDomain(inputImage);

                unionExcludeRegion = unionExcludeRegion.Union2(region);
                unionExcludeDomain = unionExcludeDomain.Union2(domain);

                if (excludeRegion.SaveCacheImageEnabled)
                {
                    var fileName = "SurfaceDefinition_" + definition.Name + "_Exclude_" + excludeRegion.Name;
                    inputImage.SaveCacheImagesForRegion(domain, region, fileName);
                }

                surfaceResult.ExcludeRegionResults.Add(new RegionResult()
                {
                    SurfaceGroupName = definition.GroupName,
                    SurfaceName = definition.Name,
                    RegionName = excludeRegion.Name,
                    Domain = domain,
                    Region = region,
                });

                //                    region.Dispose();
                //                    domain.Dispose();
            }

            foreach (var includePart in definition.IncludeParts)
            {
                var domain = includePart.GetOrInitDomain(inputImage);
                unionIncludeDomain = unionIncludeDomain.Union2(domain);

                var remainDomain = domain.Difference(unionExcludeRegion);
                var reducedImage = inputImage.ChangeDomain(remainDomain);

                HRegion region;
                //                using (new NotifyStopwatch("includeRegion.Process: " + includeRegion.Name))
                region = includePart.Extract(reducedImage);
                var remainRegion = region.Difference(unionExcludeRegion);
                unionIncludeRegion = unionIncludeRegion.Union2(remainRegion);

                if (includePart.SaveCacheImageEnabled)
                {
                    var fileName = "SurfaceDefinition_" + definition.Name + "_Include_" + includePart.Name;
                    //                        _hDevelopExportHelper.HImage.SaveCacheImagesForRegion(domain, remainRegion, fileName);
                    inputImage.SaveCacheImagesForRegion(domain, remainRegion, unionExcludeRegion,
                        fileName);
                }

                surfaceResult.IncludeRegionResults.Add(new RegionResult()
                {
                    SurfaceGroupName = definition.GroupName,
                    SurfaceName = definition.Name,
                    RegionName = includePart.Name,
                    Domain = domain,
                    Region = remainRegion,
                });
            }

            if (definition.SaveCacheImageEnabled && definition.IncludeParts.Any())
            {
                var fileName = "SurfaceDefinition_" + definition.Name + "_Include";
                inputImage.SaveCacheImagesForRegion(unionIncludeDomain, unionIncludeRegion,
                    unionExcludeRegion, fileName);
            }

            if (definition.SaveCacheImageEnabled && definition.ExcludeParts.Any())
            {
                var fileName = "SurfaceDefinition_" + definition.Name + "_Exclude";
                inputImage.SaveCacheImagesForRegion(unionExcludeDomain, unionExcludeRegion,
                    fileName);
            }

            surfaceResult.ExcludeRegion = unionExcludeRegion;
            surfaceResult.IncludeRegion = unionIncludeRegion;

            swSearchSurface.Dispose();
//            inputImage.Dispose();

            return surfaceResult;
        }
    }
}